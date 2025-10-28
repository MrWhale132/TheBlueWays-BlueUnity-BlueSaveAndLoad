using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.CodeGen;
using Assets._Project.Scripts.UtilScripts.Misc;
using Eflatun.SceneReference;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;



namespace Assets._Project.Scripts.SaveAndLoad
{
    //TODO: remove it later once it moved into the bootstrap scene
    [DefaultExecutionOrder(-1000)]
    public class SaveAndLoadManager : MonoBehaviour
    {
        public static SaveAndLoadManager Singleton { get; private set; }


        public enum SaveState
        {
            Start,
            Main,
            TempA,
            TempB,
            End,
            Terminate,
        }

        public SaveState __currentSaveState = SaveState.Main;


        public List<ISaveAndLoad> __mainSaveHandlers;
        public List<ISaveAndLoad> __tempA_saveHandlers;
        public List<ISaveAndLoad> __tempB_saveHandlers;
        //state machine vars
        public List<ISaveAndLoad> __currentSaveHandlers;
        public List<ISaveAndLoad> __iteratedSaveHandlers;

        public Dictionary<RandomId, ISaveAndLoad> __saveHandlerByHandledObjectIdLookUp = new();
        public Dictionary<Type, Type> __saveHandlerTypeByHandledObjectTypeLookUp = new();


        public bool IsIteratingSaveHandlers { get; private set; }





        public void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogError("Multiple instances of SaveAndLoadManager detected. Destroying the new instance.");
                Destroy(gameObject);
            }


            __mainSaveHandlers = new();
            __tempA_saveHandlers = new();
            __tempB_saveHandlers = new();

            __currentSaveHandlers = __mainSaveHandlers;




            __isTypesHandledByCustomSaveDataLookup = new();


            InitSaveHandlerWork();

            CollectSaveHandlers();

            BuildCustomSaveDataLookUp();
            BuildCustomSaveDataFactories(__isTypesHandledByCustomSaveDataLookup.Keys, __isTypesHandledByCustomSaveDataLookup.Values);
        }


        public void InitSaveHandlerWork()
        {
            __saveHandlerCreatorsByType = new();
            __saveHandlerCreatorsById = new();
            __genericSaveHandlerCreatorsByTypePerId = new();
            __genericSaveHandlerCreatorsByTypePerTypeDef = new();
            __arraySaveHandlerCreatorsByTypePerDimension = new();
        }



        public Dictionary<string, RandomId> __singletonObjectIdsBySaveHandlerIds = new();


        public RandomId GetOrCreateSingletonObjectIdBySaveHandlerId(string handlerId)
        {
            if (!__singletonObjectIdsBySaveHandlerIds.ContainsKey(handlerId))
            {
                var objectId = RandomId.Get();
                __singletonObjectIdsBySaveHandlerIds.Add(handlerId, objectId);
            }

            return __singletonObjectIdsBySaveHandlerIds[handlerId];
        }



        public void CollectSaveHandlers()
        {

            using var fileStream = File.Open("SingletonObjectIds.json", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            using var reader = new StreamReader(fileStream);

            var json = reader.ReadToEnd();

            __singletonObjectIdsBySaveHandlerIds = JsonConvert.DeserializeObject<Dictionary<string, RandomId>>(json) ?? new();



            //todo: find all relevant assemblies. Only user handled ones.
            var types = AppDomain.CurrentDomain.GetUserAssemblies().SelectMany(asm => asm.GetTypes());

            foreach (Type type in types)
            {
                if (type.IsInterface || type.IsAbstract)
                    continue;




                var attr = type.GetCustomAttribute<SaveHandlerAttribute>();
                if (attr == null)
                {
                    continue;
                }

                if (attr.RequiresManualAttributeCreation)
                {
                    var method = type.GetMethod("ManualSaveHandlerAttributeCreation", BindingFlags.Public | BindingFlags.Static);
                    if (method == null)
                    {
                        Debug.LogError($"SaveHandler {type.FullName} requires manual attribute creation but does not have a public static method named ManualSaveHandlerAttributeCreation. " +
                            $"Skipping this SaveHandler.");
                        continue;
                    }

                    var result = method.Invoke(null, null);

                    if (result is not SaveHandlerAttribute manualAttr)
                    {
                        Debug.LogError($"SaveHandler {type.FullName} ManualSaveHandlerAttributeCreation method did not return a SaveHandlerAttribute. " +
                            $"Skipping this SaveHandler.");
                        continue;
                    }

                    attr = manualAttr;
                }


                void LogDuplicateIdError()
                {
                    Debug.LogError($"SaveHandler with id {attr.Id} is already registered. " +
                        $"This means that there are multiple SaveHandlers with the same id. " +
                        $"Please ensure that each SaveHandler has a unique id." +
                        $"Going to skip the new one.");
                }

                void LogDuplicateHandledTypeError()
                {
                    Debug.LogError($"More then one savehandler found for the same handled type {attr.HandledType.FullName}. " +
                        $"Only one savehandler is allowed per type");
                }


                //Debug.Log($"TRACE: Registering SaveHandler for type {attr.HandledType.FullName} with id {attr.Id}.");

                if (attr.HandledType.IsGenericTypeDefinition)
                {
                    var genericHandlerTypeDef = type.GetGenericTypeDefinition();

                    if (__genericSaveHandlerCreatorsByTypePerId.ContainsKey(attr.Id.ToString()))
                    {
                        LogDuplicateIdError();
                        continue;
                    }

                    if (__genericSaveHandlerCreatorsByTypePerTypeDef.ContainsKey(attr.HandledType))
                    {
                        LogDuplicateHandledTypeError();
                        continue;
                    }


                    //factory methods for generic handlers are lazy loaded, they are created when they first requested
                    __genericSaveHandlerCreatorsByTypePerId.Add(attr.Id.ToString(), new());
                    __genericSaveHandlerCreatorsByTypePerTypeDef.Add(attr.HandledType, new(genericHandlerTypeDef, new()));
                }
                else if (attr.HandledType == typeof(Array) && attr.ArrayDimension != 0)
                {
                    var genericHandlerTypeDef = type.GetGenericTypeDefinition();

                    if (__genericSaveHandlerCreatorsByTypePerId.ContainsKey(attr.Id.ToString()))
                    {
                        LogDuplicateIdError();
                        continue;
                    }

                    if (__arraySaveHandlerCreatorsByTypePerDimension.ContainsKey(attr.ArrayDimension))
                    {
                        LogDuplicateHandledTypeError();
                        continue;
                    }


                    //same as for generics, except the groupping is by array dimension, not by handled type def
                    //array savehandlers are generics too on element type, no need to track them in a seperate list
                    __genericSaveHandlerCreatorsByTypePerId.Add(attr.Id.ToString(), new());
                    __arraySaveHandlerCreatorsByTypePerDimension.Add(attr.ArrayDimension, new(genericHandlerTypeDef, new()));
                }
                else
                {

                    if (__saveHandlerCreatorsById.ContainsKey(attr.Id.ToString()))
                    {
                        LogDuplicateIdError();
                        continue;
                    }

                    if (__saveHandlerCreatorsByType.ContainsKey(attr.HandledType))
                    {
                        LogDuplicateHandledTypeError();
                        continue;
                    }


                    Func<SaveHandlerBase> ctor = CreateTypedCtor<SaveHandlerBase>(type);

                    __saveHandlerCreatorsById[attr.Id.ToString()] = ctor;
                    __saveHandlerCreatorsByType[attr.HandledType] = ctor;

                    __saveHandlerTypeByHandledObjectTypeLookUp.Add(attr.HandledType, type);


                    if (attr.IsStatic)
                    {
                        ///dont forget about generic static classes. <see cref="GenericWithStaticExampleClass{T}"/>
                        ///a handler of an object is added when an other object asks for its object id with a reference
                        ///since static types are not referenced via an object instance, naturally, nobody will ask for their id
                        ///thus they wont get a handler automatically as others would.
                        var handler = ctor();
                        handler.Init(null);
                        AddSaveHandler(handler);
                    }
                }
            }


            fileStream.Position = 0;

            json = JsonConvert.SerializeObject(__singletonObjectIdsBySaveHandlerIds);

            using var writer = new StreamWriter(fileStream);
            writer.Write(json);
        }




        //        On IL2CPP:
        //Unity replaces it with a fallback “interpreter mode” for simple expressions only, but anything nontrivial will fail or run extremely slowly.
        //In many builds(especially older Unitys), it will just throw PlatformNotSupportedException.
        // Workaround: use Expression.Compile(preferInterpretation: true) on newer Unitys(2021.3+). That uses a safe interpreted mode — slower, but portable.
        public Func<TBase> CreateTypedCtor<TBase>(Type derivedType)
        {
            if (!typeof(TBase).IsAssignableFrom(derivedType))
                throw new ArgumentException($"{derivedType} is not assignable to {typeof(TBase)}");

            ConstructorInfo ctor = derivedType.GetConstructor(Type.EmptyTypes);

            if (ctor == null)
                throw new ArgumentException("Parameterless constructor not found.");

            NewExpression newExpr = Expression.New(ctor);

            // Cast to TBase (if needed — usually implicit, but good for clarity)
            UnaryExpression castExpr = Expression.Convert(newExpr, typeof(TBase));


            //todo:test this out
            // Bind dynamically:
            var del = (Func<TBase>)Delegate.CreateDelegate(typeof(Func<TBase>),
                typeof(CtorFactory<>).MakeGenericType(derivedType).GetMethod("Create"));



            return Expression.Lambda<Func<TBase>>(castExpr).Compile();
            //todo: test this out
            return Expression.Lambda<Func<TBase>>(castExpr).Compile(preferInterpretation: true);
        }
        public static class CtorFactory<T>
    where T : new()
        {
            public static T Create() => new T();
        }



        //Some reflection and delegate creation APIs do still work — because they rely on existing, precompiled metadata:
        //Works fine:
        //Activator.CreateInstance(typeof(MyComponent));
        //Delegate.CreateDelegate(typeof(Action), target, "MethodName");
        //MethodInfo.Invoke(target, args);
        //FieldInfo.GetValue(obj);
        //These don’t generate new code — they just use metadata from already compiled IL2CPP stubs.








        public class Service
        {
            public Dictionary<Type, Func<SaveHandlerBase>> __saveHandlerCreatorsByType = new();
            public Dictionary<string, Func<SaveHandlerBase>> __saveHandlerCreatorsById = new();

            //the first Type is the HandledType's typedef, the second Type is the generic savehandler's typedef
            //the third one is the HandledType's concrete types
            public Dictionary<Type, (Type typeDef, Dictionary<Type, Func<SaveHandlerBase>> concreteTypes)> __genericSaveHandlerCreatorsByTypePerTypeDef = new();
            public Dictionary<int, (Type saveHandlerTypeDef, Dictionary<Type, Func<SaveHandlerBase>> concreteTypes)> __arraySaveHandlerCreatorsByTypePerDimension = new();

            public Dictionary<string, Dictionary<Type, Func<SaveHandlerBase>>> __genericSaveHandlerCreatorsByTypePerId = new();


            public HashSet<Type> __serializeableTypes;



            public void CollectSerilaizeableTypes()
            {
                HashSet<Type> hasJsonConverter = new HashSet<Type>();

                var converters = JsonConvert.DefaultSettings().Converters;

                foreach (var converter in converters)
                {
                    //todo: this validation should happen when these converters collected
                    if (converter.GetType().IsGenericType)
                    {
                        Debug.LogWarning("There is a generic json converter which is not supported for now. Newtonsoft doesnt handle very well open generics." +
                            "It requires to register every possible closed type variant of that generic beforehand." +
                            "It can be workaround but its not priority at the time I wrote this.");
                        continue;
                    }

                    var baseType = converter.GetType().BaseType;

                    if (!baseType.IsGenericType || baseType.GetGenericTypeDefinition() != typeof(JsonConverter<>))
                    {
                        Debug.LogWarning("There is a json converter that does not directly inherits from JsonConverter<>. It may not be a problem." +
                            "This is just notice to know about.");
                        continue;
                    }

                    var typeConverted = baseType.GetGenericArguments()[0];

                    hasJsonConverter.Add(typeConverted);
                }

                __serializeableTypes = hasJsonConverter;
            }




            public bool HasSaveHandlerForType(Type type, bool isStatic)
            {
                if (isStatic)
                {
                    if (type.IsArray) return true;
                    if (type.IsGenericType) type = type.GetGenericTypeDefinition();

                    if (__staticSaveHandlerAttributesByHandledType.ContainsKey(type))
                    {
                        return true;
                    }
                }

                if (type.IsGenericType)
                    return __genericSaveHandlerCreatorsByTypePerTypeDef.ContainsKey(type.GetGenericTypeDefinition());

                else if (type.IsArray)
                    return __arraySaveHandlerCreatorsByTypePerDimension.ContainsKey(type.GetArrayRank());

                return __saveHandlerCreatorsByType.ContainsKey(type);
            }



#if UNITY_EDITOR
            public bool __hadInit;


            public bool IsTypeHandled_Editor(Type type, bool isStatic)
            {
                if (isStatic)
                {
                    return HasSaveHandlerForType_Editor(type, isStatic: true);
                }
                else
                {

                    if (HasSaveHandlerForType_Editor(type, isStatic: false)) return true;

                    if (HasCustomSaveData_Editor(type)) return true;

                    return HasSerializer_Editor(type);
                }
            }

            public bool IsTypeHandled_Editor(Type type)
            {
                return IsTypeHandled_Editor(type, isStatic: false) && IsTypeHandled_Editor(type, isStatic: true);
            }

            public void IsTypeManuallyHandled_Editor(Type type, out bool hasManualInstanceHandler, out bool hasManualStaticHandler)
            {
                if (type.IsArray)
                {
                    hasManualInstanceHandler = true;
                    hasManualStaticHandler = true;
                    return;
                }


                if (HasSaveHandlerForType_Editor(type, isStatic: true))
                {
                    var attr = GetSaveHandlerAttributeForType_Editor(type, isStatic: true);
                    hasManualStaticHandler = attr.GenerationMode is SaveHandlerGenerationMode.Manual;
                }
                else
                {
                    hasManualStaticHandler = false;
                }


                if (HasSaveHandlerForType_Editor(type, isStatic: false))
                {
                    var attr = GetSaveHandlerAttributeForType_Editor(type, isStatic: false);
                    hasManualInstanceHandler = attr.GenerationMode is SaveHandlerGenerationMode.Manual;
                    return;
                }


                if (HasCustomSaveData_Editor(type))
                {
                    var attr = GetCustomSaveDataAttribute_Editor(type);
                    hasManualInstanceHandler = attr.GenerationMode is SaveHandlerGenerationMode.Manual;
                    return;
                }


                if (HasSerializer_Editor(type))
                {
                    hasManualInstanceHandler = true;
                }

                hasManualInstanceHandler = false;
            }




            public SaveHandlerAttribute GetSaveHandlerAttributeForType_Editor(Type type, bool isStatic)
            {
                InitServiceIfNeeded();

                if (isStatic)
                {
                    if (__staticSaveHandlerAttributesByHandledType.TryGetValue(type, out var attr2))
                    {
                        return attr2;
                    }
                    else
                    {
                        return null;
                    }
                }

                if (__saveHandlerAttributesByHandledType.TryGetValue(type, out var attr))
                {
                    return attr;
                }
                else
                {
                    return null;
                }
            }


            public bool HasManualSaveHandlerForType_Editor(Type type, bool isStatic)
            {
                InitServiceIfNeeded();


                if (type.IsArray) return true;

                if (type.IsGenericType) type = type.GetGenericTypeDefinition();

                if (isStatic)
                {
                    if (__staticSaveHandlerAttributesByHandledType.TryGetValue(type, out var attribute2))
                    {
                        return attribute2.GenerationMode is SaveHandlerGenerationMode.Manual;
                    }

                    return false;
                }

                if (__saveHandlerAttributesByHandledType.TryGetValue(type, out var attribute))
                {
                    return attribute.GenerationMode is SaveHandlerGenerationMode.Manual;
                }

                return false;
            }


            public bool HasSaveHandlerForType_Editor(Type type)
            {
                return HasSaveHandlerForType_Editor(type, isStatic: false) && HasSaveHandlerForType_Editor(type, isStatic: true);
            }

            public bool HasSaveHandlerForType_Editor(Type type, bool isStatic)
            {
                InitServiceIfNeeded();

                return HasSaveHandlerForType(type, isStatic);
            }

            public void InitServiceIfNeeded()
            {
                if (!__hadInit)
                {
                    __hadInit = true;
                    CollectSaveHandlers(fromEditor: true);
                }
            }



            public bool HasSerializer_Editor(Type type)
            {
                if (__serializeableTypes == null) CollectSerilaizeableTypes();

                if (type.IsGenericType) type = type.GetGenericTypeDefinition();

                return __serializeableTypes.Contains(type);
            }





            public SaveHandlerAttribute GetSaveHandlerAttributeOfType_Editor(Type type, bool isStatic)
            {
                InitServiceIfNeeded();

                if (isStatic)
                {
                    if (__staticSaveHandlerAttributesByHandledType.TryGetValue(type, out var attr2))
                    {
                        return attr2;
                    }
                    else
                    {
                        return null;
                    }
                }

                if (__saveHandlerAttributesByHandledType.TryGetValue(type, out var attr))
                {
                    return attr;
                }
                else
                {
                    return null;
                }
            }

#endif





            public Dictionary<long, RandomId> __singletonObjectIdsBySaveHandlerIds = new();


            public RandomId GetOrCreateSingletonObjectIdBySaveHandlerId(long handlerId)
            {
                if (!__singletonObjectIdsBySaveHandlerIds.ContainsKey(handlerId))
                {
                    var objectId = RandomId.Get();
                    __singletonObjectIdsBySaveHandlerIds.Add(handlerId, objectId);
                }

                return __singletonObjectIdsBySaveHandlerIds[handlerId];
            }





            public Dictionary<Type, SaveHandlerAttribute> __staticSaveHandlerAttributesByHandledType = new();

            public Dictionary<Type, SaveHandlerAttribute> __saveHandlerAttributesByHandledType = new();
            public Dictionary<Type, SaveHandlerAttribute> SaveHandlerAttributesByHandledType {
                get
                { InitServiceIfNeeded(); return __saveHandlerAttributesByHandledType; }
            }





            public void CollectSaveHandlers(bool fromEditor = false)
            {
                //using var fileStream = File.Open("SingeltonObjectIds.json", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                //using var reader = new StreamReader(fileStream);

                //var json = reader.ReadToEnd();

                //__singletonObjectIdsBySaveHandlerIds = JsonConvert.DeserializeObject<Dictionary<long, RandomId>>(json);


                //todo: find all relevant assemblies
                var types = AppDomain.CurrentDomain.GetUserAssemblies().SelectMany(asm => asm.GetTypes());

                foreach (Type saveHandlerType in types)
                {
                    if (saveHandlerType.IsInterface || saveHandlerType.IsAbstract)
                        continue;

                    var attr = saveHandlerType.GetCustomAttribute<SaveHandlerAttribute>();
                    if (attr == null)
                    {
                        continue;
                    }

                    if (attr.RequiresManualAttributeCreation)
                    {
                        var method = saveHandlerType.GetMethod("ManualSaveHandlerAttributeCreation", BindingFlags.Public | BindingFlags.Static);
                        if (method == null)
                        {
                            Debug.LogError($"SaveHandler {saveHandlerType.FullName} requires manual attribute creation but does not have a public static method named ManualSaveHandlerAttributeCreation. " +
                                $"Skipping this SaveHandler.");
                            continue;
                        }

                        var result = method.Invoke(null, null);

                        if (result is not SaveHandlerAttribute manualAttr)
                        {
                            Debug.LogError($"SaveHandler {saveHandlerType.FullName} ManualSaveHandlerAttributeCreation method did not return a SaveHandlerAttribute. " +
                                $"Skipping this SaveHandler.");
                            continue;
                        }

                        attr = manualAttr;
                    }



                    void LogDuplicateIdError()
                    {
                        Debug.LogError($"SaveHandler with id {attr.Id} is already registered. " +
                            $"This means that there are multiple SaveHandlers with the same id. " +
                            $"Please ensure that each SaveHandler has a unique id." +
                            $"Going to skip the new one.");
                    }

                    void LogDuplicateHandledTypeError()
                    {
                        Debug.LogError($"More then one savehandler found for the same handled type {attr.HandledType.FullName}. " +
                            $"Only one savehandler is allowed per type");
                    }


                    //Debug.Log($"TRACE: Registering SaveHandler for type {attr.HandledType.FullName} with id {attr.Id}.");

                    if (attr.HandledType.IsGenericTypeDefinition)
                    {
                        var genericHandlerTypeDef = saveHandlerType.GetGenericTypeDefinition();

                        if (__genericSaveHandlerCreatorsByTypePerId.ContainsKey(attr.Id.ToString()))
                        {
                            LogDuplicateIdError();
                            continue;
                        }

                        if (__genericSaveHandlerCreatorsByTypePerTypeDef.ContainsKey(attr.HandledType))
                        {
                            LogDuplicateHandledTypeError();
                            continue;
                        }


                        if (attr.IsStatic)
                        {
                            if (attr.StaticHandlerOf is not null)//todo: tmp fix
                                __staticSaveHandlerAttributesByHandledType[attr.StaticHandlerOf] = attr;
                        }


                        __saveHandlerAttributesByHandledType.Add(attr.HandledType, attr);


                        //factory methods for generic handlers are lazy loaded, they are created when they first requested
                        __genericSaveHandlerCreatorsByTypePerId.Add(attr.Id.ToString(), new());
                        __genericSaveHandlerCreatorsByTypePerTypeDef.Add(attr.HandledType, new(genericHandlerTypeDef, new()));
                    }
                    else if (attr.HandledType == typeof(Array) && attr.ArrayDimension != 0)
                    {
                        var genericHandlerTypeDef = saveHandlerType.GetGenericTypeDefinition();

                        if (__genericSaveHandlerCreatorsByTypePerId.ContainsKey(attr.Id.ToString()))
                        {
                            LogDuplicateIdError();
                            continue;
                        }

                        if (__arraySaveHandlerCreatorsByTypePerDimension.ContainsKey(attr.ArrayDimension))
                        {
                            LogDuplicateHandledTypeError();
                            continue;
                        }


                        //same as for generics, except the groupping is by array dimension, not by handled type def
                        //array savehandlers are generics too on the element type, no need to track them in a seperate list
                        __genericSaveHandlerCreatorsByTypePerId.Add(attr.Id.ToString(), new());
                        __arraySaveHandlerCreatorsByTypePerDimension.Add(attr.ArrayDimension, new(genericHandlerTypeDef, new()));
                    }
                    else
                    {

                        if (__saveHandlerCreatorsById.ContainsKey(attr.Id.ToString()))
                        {
                            LogDuplicateIdError();
                            continue;
                        }

                        if (__saveHandlerCreatorsByType.ContainsKey(attr.HandledType))
                        {
                            LogDuplicateHandledTypeError();
                            continue;
                        }


                        __saveHandlerAttributesByHandledType.Add(attr.HandledType, attr);


                        Func<SaveHandlerBase> ctor = CreateTypedCtor<SaveHandlerBase>(saveHandlerType);

                        __saveHandlerCreatorsById[attr.Id.ToString()] = ctor;
                        __saveHandlerCreatorsByType[attr.HandledType] = ctor;



                        if (attr.IsStatic)
                        {
                            if (attr.StaticHandlerOf is not null)//todo: tmp fix
                                __staticSaveHandlerAttributesByHandledType[attr.StaticHandlerOf] = attr;


                            if (!fromEditor)
                            {
                                ///dont forget about generic static classes. <see cref="GenericWithStaticExampleClass{T}"/>
                                var handler = ctor();
                                handler.Init(null);
                                AddSaveHandler(handler);
                            }
                        }
                    }
                }

                //json = JsonConvert.SerializeObject(__singletonObjectIdsBySaveHandlerIds);

                //using var writer = new StreamWriter(fileStream);

                //writer.Write(json);
            }



            public Func<TBase> CreateTypedCtor<TBase>(Type derivedType)
            {
                if (!typeof(TBase).IsAssignableFrom(derivedType))
                    throw new ArgumentException($"{derivedType} is not assignable to {typeof(TBase)}");

                ConstructorInfo ctor = derivedType.GetConstructor(Type.EmptyTypes);

                if (ctor == null)
                    throw new ArgumentException("Parameterless constructor not found.");

                NewExpression newExpr = Expression.New(ctor);

                // Cast to TBase (if needed — usually implicit, but good for clarity)
                UnaryExpression castExpr = Expression.Convert(newExpr, typeof(TBase));

                return Expression.Lambda<Func<TBase>>(castExpr).Compile();
            }



            public List<ISaveAndLoad> __currentSaveHandlers = new();

            public void AddSaveHandler(ISaveAndLoad saveHandler)
            {
                var dataGroupId = saveHandler.DataGroupId;

                if (dataGroupId == null || string.IsNullOrEmpty(dataGroupId))
                {
                    Debug.LogError("Invalid DataGroupId provided.");
                    return;
                }


                __currentSaveHandlers.Add(saveHandler);
            }






            //there is purposefuly no = new() here because null means "not built yet"
            public Dictionary<Type, Type> __isTypesHandledByCustomSaveDataLookup;
            public Dictionary<Type, CustomSaveDataAttribute> __customSaveDataAttributesByType = new();


#if UNITY_EDITOR

            public bool HasCustomSaveData_Editor(Type type)
            {
                return HasCustomSaveData_Editor(type, out _);
            }

            public bool HasCustomSaveData_Editor(Type type, out Type customSaveDataType)
            {
                if (__isTypesHandledByCustomSaveDataLookup == null) BuildCustomSaveDataLookUp();
                if (__isTypesHandledByCustomSaveDataLookup.TryGetValue(type, out customSaveDataType))
                {
                    return true;
                }
                else
                {
                    customSaveDataType = null;
                    return false;
                }
            }


            public CustomSaveDataAttribute GetCustomSaveDataAttribute_Editor(Type type)
            {
                if (__customSaveDataAttributesByType.TryGetValue(type, out var attr))
                {
                    return attr;
                }
                else
                {
                    return null;
                }
            }
#endif


            public void BuildCustomSaveDataLookUp()
            {
                __isTypesHandledByCustomSaveDataLookup = new();

                var types = AppDomain.CurrentDomain.GetUserAssemblies().SelectMany(asm => asm.GetTypes());

                foreach (var type in types)
                {
                    if (typeof(CustomSaveData).IsAssignableFrom(type)
                        && type != typeof(CustomSaveData) && type != typeof(CustomSaveData<>))
                    {
                        if (type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() != typeof(CustomSaveData<>))
                        {
                            Debug.LogError($"Found a CustomSaveData type that does not" +
                                $"inherit from CustomSaveData<T> where T is the type it saves. " +
                                $"Skipping this type.");
                            continue;
                        }

                        Type handledType = type.BaseType.GetGenericArguments()[0];

                        if (handledType.IsGenericType)
                        {
                            Debug.LogError("Generic custom save data types are not supported for now. " +
                                "Found type: " + handledType.CleanAssemblyQualifiedName());
                            continue;
                            //todo
                            //handledType = handledType.GetGenericTypeDefinition();
                        }
                        else if (!handledType.IsStruct())
                        {
                            Debug.LogError("A custom save data type found that operates on a non-struct type. " +
                                "Custom save datas should only be used with structs. " +
                                "Found type: " + handledType.CleanAssemblyQualifiedName());
                            continue;
                        }


                        var attr = type.GetCustomAttribute<CustomSaveDataAttribute>();

                        if (attr != null)
                        {
                            __customSaveDataAttributesByType[type] = attr;
                        }


                        __isTypesHandledByCustomSaveDataLookup[handledType] = type;
                    }
                }
            }
        }











        public Dictionary<Type, Func<SaveHandlerBase>> __saveHandlerCreatorsByType;
        public Dictionary<string, Func<SaveHandlerBase>> __saveHandlerCreatorsById;

        //the first Type is the HandledType's typedef, the second Type is the generic savehandler's typedef
        //the third one is the HandledType's concrete types
        public Dictionary<Type, (Type typeDef, Dictionary<Type, Func<SaveHandlerBase>> concreteTypes)> __genericSaveHandlerCreatorsByTypePerTypeDef;
        public Dictionary<int, (Type saveHandlerTypeDef, Dictionary<Type, Func<SaveHandlerBase>> concreteTypes)> __arraySaveHandlerCreatorsByTypePerDimension = new();

        public Dictionary<string, Dictionary<Type, Func<SaveHandlerBase>>> __genericSaveHandlerCreatorsByTypePerId;



        public bool HasSaveHandlerForType(Type type)
        {
            if (type.IsGenericType)
                return __genericSaveHandlerCreatorsByTypePerTypeDef.ContainsKey(type.GetGenericTypeDefinition());

            else if (type.IsArray)
                return __arraySaveHandlerCreatorsByTypePerDimension.ContainsKey(type.GetArrayRank());

            return __saveHandlerCreatorsByType.ContainsKey(type);
        }



#if UNITY_EDITOR
        public bool HasSaveHandlerForType_Editor(Type type)
        {
            if (__saveHandlerCreatorsById == null)
            {
                InitSaveHandlerWork();
                CollectSaveHandlers();
            }

            return HasSaveHandlerForType(type);
        }
#endif




        public bool _muteMissingSaveHandlerWarnings = false;


        public T GetSaveHandlerById<T>(RandomId id) where T : SaveHandlerBase
        {
            if (__saveHandlerByHandledObjectIdLookUp.TryGetValue(id, out var saveHandler))
            {
                var handler = saveHandler as T;
                
                if (handler == null)
                {
                    Debug.LogError($"SaveHandler with id {id} is not of type {typeof(T).CleanAssemblyQualifiedName()}. " +
                        $"This means that the requested type does not match the registered SaveHandler type.");
                    return null;
                }
                return handler;
            }
            else
            {
                    Debug.LogError($"No SaveHandler found for id {id}. " +
                        $"This means that this id does not have a SaveHandler registered for it. ");
                return default;
            }
        }


        public ISaveAndLoad GetSaveHandlerById(ObjectMetaData metaData)
        {
            if (__saveHandlerByHandledObjectIdLookUp.TryGetValue(metaData.ObjectId, out var saveHandlerId))
                return saveHandlerId;


            if (metaData.IsGeneric)
                return GetSaveHandlerById(metaData.SaveHandlerId, metaData.SaveHandlerType);

            return GetSaveHandlerById(metaData.SaveHandlerId);
        }


        public SaveHandlerBase GetSaveHandlerById(string id, string type)
        {
            if (__genericSaveHandlerCreatorsByTypePerId.TryGetValue(id, out var dict))
            {
                Type handlerType = Type.GetType(type);
                //Type typeDef = handlerType.GetGenericTypeDefinition();


                if (!dict.ContainsKey(handlerType))
                {
                    Func<SaveHandlerBase> ctor = CreateTypedCtor<SaveHandlerBase>(handlerType);

                    dict.Add(handlerType, ctor);
                }

                var factory = dict[handlerType];

                var newHandler = factory();
                return newHandler;
            }
            else
            {
                if (!_muteMissingSaveHandlerWarnings)
                    Debug.LogError($"No SaveHandler found for id {id}. " +
                        $"This means that this id does not have a SaveHandler registered for it. ");
                return null;
            }
        }


        public SaveHandlerBase GetSaveHandlerById(string id)
        {
            if (__saveHandlerCreatorsById.TryGetValue(id, out var factory))
            {
                return factory();
            }
            else
            {
                if (!_muteMissingSaveHandlerWarnings)
                    Debug.LogError($"No SaveHandler found for id {id}. " +
                        $"This means that this id does not have a SaveHandler registered for it. ");
                return null;
            }
        }


        public SaveHandlerBase GetSaveHandlerFor(object instance)
        {
            Type objectType = instance.GetType();



            if (objectType.IsGenericType)
            {
                Type objectTypeDef = objectType.GetGenericTypeDefinition();


                if (__genericSaveHandlerCreatorsByTypePerTypeDef.TryGetValue(objectTypeDef, out var tuple))
                {
                    if (!tuple.concreteTypes.ContainsKey(objectType))
                    {
                        Type handlerTypeDef = tuple.typeDef;

                        var objectTypeArgs = objectType.GetGenericArguments();

                        Type constructedGenericHandlerType = handlerTypeDef.MakeGenericType(objectTypeArgs);

                        var ctor = CreateTypedCtor<SaveHandlerBase>(constructedGenericHandlerType);

                        tuple.concreteTypes[objectType] = ctor;
                    }

                    var factory2 = tuple.concreteTypes[objectType];
                    var newHandler = factory2();

                    return newHandler;
                }
            }
            else if (objectType.IsArray)
            {
                Type elementType = objectType.GetElementType();
                int dimRank = objectType.GetArrayRank();


                if (__arraySaveHandlerCreatorsByTypePerDimension.TryGetValue(dimRank, out var tuple))
                {
                    if (!tuple.concreteTypes.ContainsKey(elementType))
                    {
                        Type handlerTypeDef = tuple.saveHandlerTypeDef;

                        var objectTypeArgs = new Type[] { elementType };

                        Type constructedGenericHandlerType = handlerTypeDef.MakeGenericType(objectTypeArgs);

                        var ctor = CreateTypedCtor<SaveHandlerBase>(constructedGenericHandlerType);

                        tuple.concreteTypes[objectType] = ctor;
                    }

                    var factory2 = tuple.concreteTypes[objectType];
                    var newHandler = factory2();

                    return newHandler;
                }
            }
            else if (__saveHandlerCreatorsByType.TryGetValue(objectType, out var factory))
            {
                return factory();
            }


            {
                if (!_muteMissingSaveHandlerWarnings)
                    Debug.LogError($"No SaveHandler found for type {objectType.FullName}. " +
                        $"This means that this type does not have a SaveHandler registered for it. ");
                return null;
            }
        }



        public Type GetSaveHandlerTypeFrom(Type objectType)
        {
            if (__saveHandlerTypeByHandledObjectTypeLookUp.TryGetValue(objectType, out var handlerType))
            {
                return handlerType;
            }

            if (objectType.IsGenericType)
            {
                Type objectTypeDef = objectType.GetGenericTypeDefinition();


                if (__genericSaveHandlerCreatorsByTypePerTypeDef.TryGetValue(objectTypeDef, out var tuple))
                {
                    Type handlerTypeDef = tuple.typeDef;

                    var objectTypeArgs = objectType.GetGenericArguments();

                    Type constructedGenericHandlerType = handlerTypeDef.MakeGenericType(objectTypeArgs);

                    __saveHandlerTypeByHandledObjectTypeLookUp.Add(objectType, constructedGenericHandlerType);

                    return constructedGenericHandlerType;
                }
            }
            else if (objectType.IsArray)
            {
                Type elementType = objectType.GetElementType();
                int dimRank = objectType.GetArrayRank();


                if (__arraySaveHandlerCreatorsByTypePerDimension.TryGetValue(dimRank, out var tuple))
                {
                    Type handlerTypeDef = tuple.saveHandlerTypeDef;

                    var objectTypeArgs = new Type[] { elementType };

                    Type constructedGenericHandlerType = handlerTypeDef.MakeGenericType(objectTypeArgs);

                    __saveHandlerTypeByHandledObjectTypeLookUp.Add(objectType, constructedGenericHandlerType);

                    return constructedGenericHandlerType;
                }
            }

            {
                if (!_muteMissingSaveHandlerWarnings)
                    Debug.LogError($"No SaveHandler type found for type {objectType.FullName}. " +
                        $"This means that this type does not have a SaveHandler registered for it. ");
                return null;
            }
        }




        #region GameLoopIntegrators


        public HashSet<IGameLoopIntegrator> __integrators = new();

        public void RegisterIntegrator(IGameLoopIntegrator integrator)
        {
            if (integrator == null)
            {
                Debug.LogError("IGameLoopIntegrator is null. Return");
                return;
            }
            if (__integrators.Contains(integrator))
            {
                Debug.LogError("IGameLoopIntegrator is already added. Return.");
                return;
            }

            __integrators.Add(integrator);
        }

        #endregion




        //there is purposefuly no = new() here because null means "not built yet"
        public Dictionary<Type, Type> __isTypesHandledByCustomSaveDataLookup;
        public Dictionary<Type, Func<CustomSaveData>> __customSaveDataFactories;
        public Dictionary<Type, CustomSaveDataAttribute> __customSaveDataAttributesByType = new();


#if UNITY_EDITOR

        public bool HasCustomSaveData_Editor(Type type)
        {
            return HasCustomSaveData_Editor(type, out _);
        }

        public bool HasCustomSaveData_Editor(Type type, out Type customSaveDataType)
        {
            if (__isTypesHandledByCustomSaveDataLookup == null) BuildCustomSaveDataLookUp();

            if (__isTypesHandledByCustomSaveDataLookup.TryGetValue(type, out customSaveDataType))
            {
                return true;
            }
            else
            {
                customSaveDataType = null;
                return false;
            }
        }
#endif


        public void BuildCustomSaveDataLookUp()
        {
            __isTypesHandledByCustomSaveDataLookup = new();

            var types = AppDomain.CurrentDomain.GetUserAssemblies().SelectMany(asm => asm.GetTypes());

            foreach (var type in types)
            {
                if (typeof(CustomSaveData).IsAssignableFrom(type)
                    && type != typeof(CustomSaveData) && type != typeof(CustomSaveData<>))
                {
                    if (type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() != typeof(CustomSaveData<>))
                    {
                        Debug.LogError($"Found a CustomSaveData type that does not" +
                            $"inherit from CustomSaveData<T> where T is the type it saves. " +
                            $"Skipping this type.");
                        continue;
                    }

                    Type handledType = type.BaseType.GetGenericArguments()[0];

                    if (handledType.IsGenericType)
                    {
                        Debug.LogError("Generic custom save data types are not supported for now. " +
                            "Found type: " + handledType.CleanAssemblyQualifiedName());
                        continue;
                        //todo
                        //handledType = handledType.GetGenericTypeDefinition();
                    }
                    else if (!handledType.IsStruct())
                    {
                        Debug.LogError("A custom save data type found that operates on a non-struct type. " +
                            "Custom save datas should only be used with structs. " +
                            "Found type: " + handledType.CleanAssemblyQualifiedName());
                        continue;
                    }



                    var attr = type.GetCustomAttribute<CustomSaveDataAttribute>();

                    if (attr != null)
                    {
                        __customSaveDataAttributesByType[type] = attr;
                    }


                    __isTypesHandledByCustomSaveDataLookup[handledType] = type;
                }
            }
        }


        public void BuildCustomSaveDataFactories(IEnumerable<Type> handledTypes, IEnumerable<Type> saveDataTypes)
        {
            __customSaveDataFactories = new();

            for (int i = 0; i < handledTypes.Count(); i++)
            {
                var handledType = handledTypes.ElementAt(i);
                var saveDataType = saveDataTypes.ElementAt(i);
                var ctor = CreateTypedCtor<CustomSaveData>(saveDataType);
                __customSaveDataFactories.Add(handledType, ctor);
            }
        }

        public bool HasCustomSaveData<T>(Type type, out CustomSaveData<T> saveData)
        {
            if (__customSaveDataFactories.TryGetValue(type, out var factory))
            {
                saveData = (CustomSaveData<T>)factory();
                return true;
            }
            else
            {
                saveData = null;
                return false;
            }
        }



        public bool IsTypeInheritsFromGenericBaseType(Type type, Type genericTypeDefinition)
        {
            if (type == null || genericTypeDefinition == null)
                return false;

            if (!genericTypeDefinition.IsGenericTypeDefinition)
                throw new ArgumentException($"{genericTypeDefinition} must be a generic type definition");

            // Check base types
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericTypeDefinition)
                    return true;

                type = type.BaseType;
            }

            return false;
        }









        //todo: use ids as keys instead.
        //public Dictionary<Object, bool> __isUnityObjectLoading;
        public Dictionary<RandomId, bool> __isObjectLoading = new();

        public bool ExpectingIsObjectLoadingRequest { get; set; }


        public static bool IsObjectLoading(Component component)
        {
            return Singleton == null ? false : Singleton._IsObjectLoading(component);
        }

        public bool _IsObjectLoading(Component component)
        {
            if (component == null) return false;

            return _IsObjectLoading(component.gameObject);
        }

        public static bool IsObjectLoading(Object go)
        {
            return Singleton == null ? false : Singleton._IsObjectLoading(go);
        }

        public bool _IsObjectLoading(Object go)
        {
            if (go == null)
            {
                Debug.LogError("Unity object is null.");
                return false;
            }

            if (ExpectingIsObjectLoadingRequest) return true;


            var id = Infra.Singleton._GetObjectIdWithoutReferencing(go, autoRegister: false);

            if (id.IsDefault) return false;


            if (__isObjectLoading.ContainsKey(id))
            {
                return __isObjectLoading[id];
            }
            else
            {
                //Debug.LogError(go.name + " - GameObject not found in __isGOLoading dictionary. Going to assume its no loading from disk");
                return false;
            }
        }

        public void SetObjectLoading(RandomId id, bool isLoading)
        {
            if (id.IsDefault)
            {
                Debug.LogError("Parameter object id is default.");
                return;
            }


            __isObjectLoading[id] = isLoading;
        }



        public static bool IsObjectLoading(object obj)
        {
            return Singleton == null ? false : Singleton._IsObjectLoading(obj);
        }


        public bool _IsObjectLoading(object obj)
        {
            if (obj == null)
            {
                Debug.LogError("GameObject is null.");
                return false;
            }

            if (ExpectingIsObjectLoadingRequest) return true;


            var id = Infra.Singleton._GetObjectIdWithoutReferencing(obj, autoRegister: false);


            if (id.IsDefault) return false;


            if (__isObjectLoading.ContainsKey(id))
            {
                return __isObjectLoading[id];
            }
            else
            {
                //Debug.LogError(go.name + " - GameObject not found in __isGOLoading dictionary. Going to assume its no loading from disk");
                return false;
            }
        }

        //public void SetObjectLoading(object obj, bool isLoading)
        //{
        //    if (obj == null)
        //    {
        //        Debug.LogError("GameObject is null.");
        //        return;
        //    }

        //    var id = Infra.Singleton._GetObjectIdWithoutReferencing(obj);

        //    __isObjectLoading[id] = isLoading;
        //}







        public bool _NewSaveHandlersWereAdded()
        {
            return __currentSaveHandlers.Count != 0;
        }


        public void _MergeTempCollectionToMainCollection()
        {
            if (__iteratedSaveHandlers == __mainSaveHandlers)
            {
                Debug.LogError($"{nameof(_MergeTempCollectionToMainCollection)} should only be called when the current collection " +
                    $"is not the main collection. Merging the main collection to it self woudl result in duplicates." +
                    $"Cancel merging and do nothing.");
                return;
            }


            //foreach(var handler in __iteratedSaveHandlers)
            //{
            //    Debug.LogWarning((handler.HandledType, "  ",handler.HandledObjectId));
            //}
            __mainSaveHandlers.AddRange(__iteratedSaveHandlers);
            __iteratedSaveHandlers.Clear();
        }




        public bool __debugSaveStateMachine = false;

        public void _MoveToNextState()
        {
            SaveState nextState;

            switch (__currentSaveState)
            {
                case SaveState.Start:
                    nextState = SaveState.Main;
                    break;

                case SaveState.Main:
                    if (_NewSaveHandlersWereAdded())
                        nextState = SaveState.TempA;
                    else
                        nextState = SaveState.End;
                    if (__debugSaveStateMachine)
                    {
                        Debug.Log("From main to " + nextState);
                    }

                    break;

                case SaveState.TempA:
                    if (_NewSaveHandlersWereAdded())
                    {
                        nextState = SaveState.TempB;
                    }
                    else
                        nextState = SaveState.End;

                    if (__debugSaveStateMachine)
                    {
                        Debug.Log("From A to " + nextState);
                    }

                    _MergeTempCollectionToMainCollection();
                    break;

                case SaveState.TempB:
                    if (_NewSaveHandlersWereAdded())
                    {
                        nextState = SaveState.TempA;
                    }
                    else
                        nextState = SaveState.End;

                    if (__debugSaveStateMachine)
                    {
                        Debug.Log("From B to " + nextState);
                    }

                    _MergeTempCollectionToMainCollection();
                    break;

                case SaveState.End:
                    Debug.LogError($"{nameof(SaveAndLoadManager)}: The current state of the save statemachine is already in End state, " +
                        $"since the machine can not go to any other state from the End state, its an error to call a NextState on it.");
                    nextState = SaveState.Terminate;
                    return;

                default:
                    Debug.LogError($"{nameof(SaveAndLoadManager)}: Save statemachine encountered a state that it is not prepared for. " +
                        $"Please handle the missing cases(s). Missing case: {__currentSaveState}");
                    nextState = SaveState.Terminate;
                    return;
            }


            switch (nextState)
            {
                case SaveState.Start:
                    Debug.LogError($"{nameof(SaveAndLoadManager)}: Start state can not be the next state for the save statemachine.");
                    nextState = SaveState.Terminate;
                    break;

                case SaveState.Main:
                    __iteratedSaveHandlers = __mainSaveHandlers;
                    __currentSaveHandlers = __tempA_saveHandlers;
                    break;

                case SaveState.TempA:
                    __iteratedSaveHandlers = __tempA_saveHandlers;
                    __currentSaveHandlers = __tempB_saveHandlers;
                    break;

                case SaveState.TempB:
                    __iteratedSaveHandlers = __tempB_saveHandlers;
                    __currentSaveHandlers = __tempA_saveHandlers;
                    break;

                case SaveState.End:
                    __iteratedSaveHandlers = null;
                    __currentSaveHandlers = __mainSaveHandlers;
                    break;

                default:
                    Debug.LogError($"{nameof(SaveAndLoadManager)}: The next state of the Save statemachine is not defined. " +
                        $"Please handle the missing state. Missing state: {nextState}. " +
                        $"Setting next state to {SaveState.Terminate}");
                    nextState = SaveState.Terminate;
                    break;
            }

            __currentSaveState = nextState;
        }


        public bool _ShouldStopIteratingSaveHandlers()
        {
            return __currentSaveState switch
            {
                SaveState.End or SaveState.Terminate => true,
                _ => false,
            };
        }


        public void _ResetSaveStateMachine()
        {
            __currentSaveState = SaveState.Start;
        }






        public void Save()
        {
            IsIteratingSaveHandlers = true;
            //var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            Infra.Singleton.StartNewReferenceGraph();


            _ResetSaveStateMachine();
            _MoveToNextState();

            do
            {
                foreach (var handler in __iteratedSaveHandlers)
                {
                    if (handler == null)
                    {
                        Debug.LogError("Save handler is null, it means it wasnt removed from save manager when the object was destroyed. " +
                            "Skipping.");
                        continue;
                    }

                    handler.WriteSaveData();
                }

                _MoveToNextState();
            }
            while (!_ShouldStopIteratingSaveHandlers());

            //Debug.LogWarning("write "+stopwatch.ElapsedMilliseconds / 1000f);

            IsIteratingSaveHandlers = false;




            if (__currentSaveState == SaveState.Terminate)
            {
                Debug.LogError($"The saving of the objects was terminated. Going to skip the rest of process.");
                return;
            }



            HashSet<RandomId> referencedObjects = Infra.Singleton.GetReferencedObjects();


            var processedHandlers = new List<ISaveAndLoad>(__mainSaveHandlers);


            for (int i = processedHandlers.Count - 1; i > -1; i--)
            {
                var handler = processedHandlers[i];

                if (!referencedObjects.Contains(handler.HandledObjectId))
                {
                    processedHandlers.RemoveAt(i);
                    __mainSaveHandlers.Remove(handler);

                    handler.ReleaseObject();
                    //todo: remove the handled object's delegates from the delegate map


                    //used this to debug, leaving it here for a while to see if anything unexpected happens
                    Debug.LogWarning($"removing handler, datagroup: {handler.DataGroupId}, handled object id: {handler.HandledObjectId}");
                }
            }
            //Debug.LogWarning("discover "+stopwatch.ElapsedMilliseconds / 1000f);



            var saveData = new Dictionary<string, List<string>>();



            foreach (var handler in processedHandlers)
            {
                {
                    if (handler == null)
                    {
                        Debug.LogError("Save handler is null, it means it wasnt removed from save manager when the object was destroyed. " +
                            "Skipping.");
                        continue;
                    }

                    var data = handler.Serialize();


                    if (!saveData.ContainsKey(handler.DataGroupId))
                    {
                        var list = new List<string>();
                        saveData[handler.DataGroupId] = list;
                    }

                    saveData[handler.DataGroupId].Add(data);
                }
            }
            //Debug.LogWarning("serialize "+stopwatch.ElapsedMilliseconds / 1000f);


            IEnumerable<string> flatList = saveData.SelectMany(x => x.Value);


            string now = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            string fileName = "/savedata_" + now + ".json";

            string path = Paths.Singleton.WorldSavePath + fileName;

            Debug.Log($"Saving data to {path}.");

            JsonUtil.WriteObjects(path, flatList);

            //stopwatch.Stop();
            //Debug.LogWarning("save to disk "+stopwatch.ElapsedMilliseconds / 1000f);
            //Debug.LogWarning(stopwatch.ElapsedMilliseconds / 1000f);
        }




        public SceneReference _mainMenuSceneRef;
        public SceneReference _emptySceneForLoadingFromSaveFile;


        public Coroutine __loadCoroutine;

        public AsyncOperation op;

        public void Load(string saveFileAbsPath)
        {
            __loadCoroutine = StartCoroutine(LoadRoutine(saveFileAbsPath));
        }


        public IEnumerator LoadRoutine(string saveFileAbsPath)
        {
            if (string.IsNullOrEmpty(saveFileAbsPath))
            {
                Debug.LogError("Save file path is null or empty.");
                yield break;
            }

            if (!System.IO.File.Exists(saveFileAbsPath))
            {
                Debug.LogError($"Save file does not exist at path: {saveFileAbsPath}");
                yield break;
            }


            yield return StartCoroutine(LoadEmptyScene());

            yield return StartCoroutine(UnLoadMainMenuScene());



            Debug.Log($"Loading save file from {saveFileAbsPath}.");



            List<string> objects = JsonUtil.ReadObjects(saveFileAbsPath, relative: false);

            List<ISaveAndLoad> saveHandlers = new List<ISaveAndLoad>();

            int i = 0;


            foreach (var obj in objects)
            {
                var saveInfo = JsonConvert.DeserializeObject<SavedObject>(obj);

                if (saveInfo == null || saveInfo._MetaData_ == null)
                {
                    Debug.LogError($"SaveObject or its MetaData is null at {i} th object. Skipping this object.");
                    continue;
                }

                var handler = GetSaveHandlerById(saveInfo._MetaData_);

                if (handler == null)
                {
                    Debug.LogError($"No SaveHandler found for id {saveInfo._MetaData_.SaveHandlerId}. Skipping this object.");
                    continue;
                }

                handler.Deserialize(obj);
                saveHandlers.Add(handler);

                SetObjectLoading(saveInfo._MetaData_.ObjectId, true);

                i++;
            }



            var ordered = saveHandlers.GroupBy(handler => handler.MetaData.Order).OrderBy(group => group.Key);

            //these are different stages that build upon each other, iterating over them multiple times is by design, dont put them in one loop
            foreach (var group in ordered)
            {
                d_loadedOrder = group.Key;

                foreach (var handler in group)
                {
                    handler.CreateObject();

                    //if (d_suspendLoading && !d_found)
                    //{
                    //var test = Component.FindAnyObjectByType<Canvas>();
                    //if (test != null)
                    //{
                    //    Debug.Log(handler.HandledObjectId);
                    //    yield return new WaitWhile(() => !d_continue);
                    //    d_continue = false;
                    //        d_found = true;
                    //}
                    //}

                    //if (handler.HandledObjectId.ToString() == "956694569789028744")
                    //{

                    //    yield return new WaitWhile(() =>
                    //    {
                    //        return !d_continue;
                    //    });

                    //    d_continue = false;
                    //}

                    AddSaveHandler(handler);//to add a savehandler it needs to have a HandledObjectId assigned, which happens in CreateObject
                }

                foreach (var handler in group)
                {
                    handler.LoadReferences();
                }

                foreach (var handler in group)
                {
                    handler.LoadValues();
                }


                if (d_suspendLoading)
                {

                    yield return new WaitWhile(() =>
                    {
                        return !d_continue;
                    });

                    d_continue = false;
                }
            }


            if (d_suspendLoading)
            {

                yield return new WaitWhile(() =>
                {
                    return !d_continue;
                });

                d_continue = false;
            }



            foreach (var integrator in __integrators)
            {
                integrator.StartIntegration();
            }

            //let collisions and everything else we dont have control over trigger
            //our components should filter these if they are loading
            //update: this logic I think could be handled by integrators
            yield return null;





            __isObjectLoading.Clear();
            __integrators.Clear();

            Debug.Log("loading completed");
        }


        public bool d_continue;
        public bool d_suspendLoading;
        public int d_loadedOrder;

        public IEnumerator LoadEmptyScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_emptySceneForLoadingFromSaveFile.BuildIndex, LoadSceneMode.Additive);

            while (!loadOperation.isDone)
            {
                yield return null; // Wait for the load operation to complete
            }
        }


        public IEnumerator UnLoadMainMenuScene()
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(_mainMenuSceneRef.BuildIndex);


            while (!unloadOperation.isDone)
            {
                yield return null; // Wait for the unload operation to complete
            }

            if (unloadOperation.isDone)
            {
                Debug.Log("Main menu scene unloaded successfully.");
            }
            else
            {
                Debug.LogError("Failed to unload main menu scene.");
            }
        }






        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Debug.Log("Saving game...");
                Save();
                Debug.Log("Save completed.");
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                //Debug.Log("Loading game...");
                //Load();
            }
        }


        public void AddSaveHandler(ISaveAndLoad saveHandler)
        {
            if (!saveHandler.IsInitialized)
            {
                Debug.LogError($"SaveHandler is uninitialized. This api can not be used with an uninitialized savehandler." +
                    $"handler type name:{saveHandler.GetType().FullName} handler id: {saveHandler.SaveHandlerId}");
                return;
            }


            //already added before
            if (__saveHandlerByHandledObjectIdLookUp.ContainsKey(saveHandler.HandledObjectId))
                return;


            var dataGroupId = saveHandler.DataGroupId;

            if (dataGroupId == null || string.IsNullOrEmpty(dataGroupId))
            {
                Debug.LogError("Invalid DataGroupId provided.");
                return;
            }


            __currentSaveHandlers.Add(saveHandler);
            __saveHandlerByHandledObjectIdLookUp.Add(saveHandler.HandledObjectId, saveHandler);
        }


        public void RemoveSaveHandler(RandomId objectId)
        {
            if (!__saveHandlerByHandledObjectIdLookUp.ContainsKey(objectId))
            {
                var obj = Infra.Singleton.GetReference(objectId);

                Debug.LogError($"No save handlers found for object with id: {objectId}. " +
                    $"This means that the handler was never added or already have been removed. Object type: {obj.GetType().CleanAssemblyQualifiedName()}");
                return;
            }

            var handler = __saveHandlerByHandledObjectIdLookUp[objectId];
            RemoveSaveHandler(handler);
        }

        public void RemoveSaveHandler(ISaveAndLoad saveHandler)
        {
            if (IsIteratingSaveHandlers)
            {
                Debug.LogError($"{nameof(SaveAndLoadManager)}: Removing a save handler while saving is not supported currently. " +
                    $"Going to cancel removing.");
                return;
            }


            if (!saveHandler.IsInitialized)
            {
                Debug.LogError($"SaveHandler is uninitialized. This api can not be used with an uninitialized savehandler.");
                return;
            }


            var dataGroupId = saveHandler.DataGroupId;

            if (dataGroupId == null || string.IsNullOrEmpty(dataGroupId))
            {
                Debug.LogError("Invalid DataGroupId provided.");
                return;
            }

            __mainSaveHandlers.Remove(saveHandler);
            __saveHandlerByHandledObjectIdLookUp.Remove(saveHandler.HandledObjectId);




            saveHandler.ReleaseObject();
            //else
            //{
            //    Debug.LogError($"No save handlers found for DataGroupId: {dataGroupId}." +
            //        $"This means that the handler was never added or already have been removed and the object of this handler wants to desubscribe " +
            //        $"a second time. Since this unsubscribing happens mostly on Destroy(), " +
            //        $"it might means the object needs to check if it has already been destroyed.");
            //}
        }
    }


    public class ObjectMetaData
    {
        public string SaveHandlerId;
        public bool IsGeneric;
        public bool IsArray;
        public int Order;
        public string SaveHandlerType;
        public RandomId ObjectId;
    }

    public class SavedObject
    {
        public ObjectMetaData _MetaData_;
    }


    public class SaveData
    {
        public string SerializedData;
        public RandomId HandledObjectId;
        public RandomId ParentObjectId;//for future use
        public int Order;
        public string DataGroupId;
    }

    public class SaveDataTable
    {
        public List<SaveData> Datas;
    }
}