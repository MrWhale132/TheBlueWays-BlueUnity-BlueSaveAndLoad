using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.CodeGen;
using Assets._Project.Scripts.UtilScripts.Extensions;
using Eflatun.SceneReference;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.InfraScripts;
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
        public static PrefabDescriptionRegistry PrefabDescriptionRegistry { get; set; } = new PrefabDescriptionRegistry();
        public static ScenePlacedObjectRegistry ScenePlacedObjectRegistry { get; set; } = new ScenePlacedObjectRegistry();


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

            try
            {
                CollectSaveHandlers();
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                throw;
            }

            BuildCustomSaveDataLookUp();
            BuildCustomSaveDataFactories(__isTypesHandledByCustomSaveDataLookup.Keys, __isTypesHandledByCustomSaveDataLookup.Values);
        }


        private void Start()
        {
            Infra.Singleton.Register(PrefabDescriptionRegistry, rootObject: true, createSaveHandler: true);
            Infra.Singleton.Register(ScenePlacedObjectRegistry, rootObject: true, createSaveHandler: true);
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

        public static class CtorFactory<T> where T : new()
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







        public static Service Service_ { get; } = new Service();

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
            public Dictionary<Type, Type> __saveHandlerTypeByHandledObjectTypeLookUp = new();



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
                        if (converter.GetType().Assembly.GetName() == typeof(JsonConverter).Assembly.GetName())
                        {
                            continue;
                        }
                        Debug.LogWarning("There is a json converter that does not directly inherits from JsonConverter<>. " +
                            $"Type: {converter.GetType().CleanAssemblyQualifiedName()}. " +
                            "It may not be a problem." +
                            "This is just notice to know about.");
                        continue;
                    }

                    var typeConverted = baseType.GetGenericArguments()[0];

                    hasJsonConverter.Add(typeConverted);
                }

                __serializeableTypes = hasJsonConverter;
            }








            public void InitServiceIfNeeded()
            {
#if UNITY_EDITOR
                if (!__hadInit)
                {
                    __hadInit = true;
                    CollectSaveHandlers(fromEditor: true);
                }
#endif
            }



#if UNITY_EDITOR
            public bool __hadInit;



            //todo: do we need to check serializables too?
            public bool IsTypeHandled_Editor(Type type, bool isStatic, out Type handlerType)
            {
                if (type.IsGenericType && !type.IsGenericTypeDefinition)
                {
                    Debug.LogError($"This api is not designed to work on close constructed generic types. It expects generic type definitions only. " +
                        $"It will use the gen type def anyway. " +
                        $"Type: {type.CleanAssemblyQualifiedName()}, isStatic:{isStatic}");
                }


                InitServiceIfNeeded();


                if (type.IsGenericType) type = type.GetGenericTypeDefinition();

                if (isStatic)
                {
                    if (type.IsArray)
                    {
                        //todo:
                        throw new NotSupportedException("in progeress");
                    }

                    if (__staticSaveHandlerAttributesByHandledType.TryGetValue(type, out var attr2))
                    {
                        handlerType = attr2.HandlerType;
                        return true;
                    }

                    handlerType = null;
                    return false;
                }


                if (type.IsArray)
                {
                    if (__arraySaveHandlerCreatorsByTypePerDimension.TryGetValue(type.GetArrayRank(), out var lookup))
                    {
                        handlerType = lookup.saveHandlerTypeDef;
                        return true;
                    }

                    handlerType = null;
                    return false;
                }


                if (__saveHandlerAttributesByHandledType.TryGetValue(type, out var attr))
                {
                    handlerType = attr.HandlerType;
                    return true;
                }

                if (__customSaveDataAttributesByType.TryGetValue(type, out var customSaveDataAttribute))
                {
                    handlerType = customSaveDataAttribute.SaveHandlerType;
                    return true;
                }

                handlerType = null;
                return false;
            }


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

            public bool IsTypeManuallyHandled_Editor(Type type, bool isStatic)
            {
                IsTypeManuallyHandled_Editor(type, out var hasManualInstanceHandler, out var hasManualStaticHandler);

                if (isStatic)
                {
                    return hasManualStaticHandler;
                }
                else
                {
                    return hasManualInstanceHandler;
                }
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



            public bool HasSaveHandlerForType_Editor(Type type)
            {
                return HasSaveHandlerForType_Editor(type, isStatic: false) && HasSaveHandlerForType_Editor(type, isStatic: true);
            }

            public bool HasSaveHandlerForType_Editor(Type type, bool isStatic)
            {
                InitServiceIfNeeded();

                if (isStatic)
                {
                    if (type.IsArray) return true;
                    if (type.IsGenericType) type = type.GetGenericTypeDefinition();

                    if (__staticSaveHandlerAttributesByHandledType.ContainsKey(type))
                    {
                        return true;
                    }

                    return false;
                }

                if (type.IsGenericType)
                    return __genericSaveHandlerCreatorsByTypePerTypeDef.ContainsKey(type.GetGenericTypeDefinition());

                else if (type.IsArray)
                    return __arraySaveHandlerCreatorsByTypePerDimension.ContainsKey(type.GetArrayRank());

                return __saveHandlerCreatorsByType.ContainsKey(type);
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






            public SaveHandlerBase.TypeMetaData GetSaveHandlerMetaData_Editor(Type handledType, bool isStaticHandler)
            {
                InitServiceIfNeeded();

                if (!HasSaveHandlerForType_Editor(handledType, isStaticHandler)) { return null; }

                var handlerType = GetSaveHandlerTypeFrom(handledType);

                var methodName = nameof(SaveHandlerBase._methodToId);

                var methodInfo = handlerType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
                var methods = handlerType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                if (methodInfo == null)
                {
                    Debug.LogError($"SaveHandler {handlerType.FullName} does not have a public static method named {methodName}. " +
                        $"Cannot get method id mapping for this SaveHandler.");
                    return null;
                }

                var methodMapping = (Dictionary<string, long>)methodInfo.Invoke(null, null);

                if (methodMapping == null)
                {
                    Debug.LogError($"SaveHandler {handlerType.FullName} {methodName} method returned null. " +
                        $"Cannot get method id mapping for this SaveHandler.");
                    return null;
                }

                return new SaveHandlerBase.TypeMetaData()
                {
                    MethodSignatureToMethodId = methodMapping,
                };
            }

#endif




            public Type GetSaveHandlerTypeFrom(Type objectType)
            {
                InitServiceIfNeeded();

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

                return null;

            }



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

            //todo:remove if no problems
            //            public Dictionary<Type, SaveHandlerAttribute> SaveHandlerAttributesByHandledType {
            //                get
            //                {
            //#if UNITY_EDITOR
            //                    InitServiceIfNeeded();
            //#endif
            //                    return __saveHandlerAttributesByHandledType;
            //                }
            //            }





            public void CollectSaveHandlers(bool fromEditor = false)
            {
                //using var fileStream = File.Open("SingeltonObjectIds.json", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                //using var reader = new StreamReader(fileStream);

                //var json = reader.ReadToEnd();

                //__singletonObjectIdsBySaveHandlerIds = JsonConvert.DeserializeObject<Dictionary<long, RandomId>>(json);


                //todo: find all relevant assemblies
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

                    Type saveHandlerType = type;


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


                    attr.HandlerType = saveHandlerType;



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

                        __saveHandlerTypeByHandledObjectTypeLookUp.Add(attr.HandledType, saveHandlerType);



                        if (attr.IsStatic)
                        {
                            if (attr.StaticHandlerOf is not null)//todo: tmp fix
                                __staticSaveHandlerAttributesByHandledType[attr.StaticHandlerOf] = attr;
                            else
                            {
                                Debug.LogError($"Savehandler {attr.Id} isStatic but still does not use the {nameof(SaveHandlerAttribute.StaticHandlerOf)} property.");
                            }

                            if (!fromEditor)
                            {
                                ///todo: dont forget about generic static classes. <see cref="GenericWithStaticExampleClass{T}"/>
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






            public Dictionary<long, SaveHandlerAttribute> __saveHandlerAttributesById;
            public Dictionary<long, SaveHandlerAttribute> __staticSaveHandlerAttributesById;
            public bool HadBuiltSaveHandlerIdByTypeLookups => __saveHandlerAttributesById != null && __staticSaveHandlerAttributesById != null;

            public Type GetHandledTypeByHandlerId(long id)
            {
                return GetHandledTypeByHandlerId(id, out var _);
            }

            public Type GetHandledTypeByHandlerId(long id, out bool isStatic)
            {
                InitServiceIfNeeded();

                if (!HadBuiltSaveHandlerIdByTypeLookups)
                {
                    var handlers = __saveHandlerAttributesByHandledType.Values;

                    __saveHandlerAttributesById = new();

                    foreach (var handler in handlers)
                    {
                        __saveHandlerAttributesById[handler.Id] = handler;
                    }


                    handlers = __staticSaveHandlerAttributesByHandledType.Values;

                    __staticSaveHandlerAttributesById = new();

                    foreach (var handler in handlers)
                    {
                        __staticSaveHandlerAttributesById[handler.Id] = handler;
                    }
                }

                if (__staticSaveHandlerAttributesById.TryGetValue(id, out var attr))
                {
                    isStatic = true;
                    return attr.StaticHandlerOf;
                }
                if (__saveHandlerAttributesById.TryGetValue(id, out attr))
                {
                    isStatic = false;
                    return attr.HandledType;
                }

                Debug.LogError($"No savehandler found with id {id}.");
                //Debug.LogError($"No handled type found for id {id}");
                isStatic = false;
                return null;
            }



            public Dictionary<long, (string handledTypeName, bool isStatic)> GetHandledTypeNameByHandlerIdLookup()
            {
                InitServiceIfNeeded();


                var handlerIdsByTypeName = new Dictionary<long, (string handledTypeName, bool isStatic)>();

                var handlers = __saveHandlerAttributesByHandledType.Values;

                foreach (var handler in handlers)
                {
                    handlerIdsByTypeName[handler.Id] = (handler.HandledType.CleanAssemblyQualifiedName(), isStatic: false);
                }

                handlers = __staticSaveHandlerAttributesByHandledType.Values;

                foreach (var handler in handlers)
                {
                    //todo: here the handled type is the subtitue type. Get the subtituted type
                    handlerIdsByTypeName[handler.Id] = (handler.StaticHandlerOf.CleanAssemblyQualifiedName(), isStatic: true);
                }

                return handlerIdsByTypeName;
            }



            public long GetHandlerIdByHandledType(Type handledType, bool isStatic)
            {
                InitServiceIfNeeded();

                if (isStatic)
                {
                    if (__staticSaveHandlerAttributesByHandledType.TryGetValue(handledType, out var attr2))
                    {
                        return attr2.Id;
                    }
                    else
                    {
                        return -1;
                    }
                }

                if (__saveHandlerAttributesByHandledType.TryGetValue(handledType, out var attr))
                {
                    return attr.Id;
                }
                else
                {
                    return -1;
                }
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

                        //todo: swtich to this when the times come
                        //handledType = attr.HandledType;


                        if (attr != null)
                        {
                            attr.SaveHandlerType = type;
                            __customSaveDataAttributesByType[handledType] = attr;
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
                    Debug.LogError($"No SaveHandler found for type {objectType.CleanAssemblyQualifiedName()}. " +
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
            //todo: check against multiple save requests
            StartCoroutine(SaveRoutine());
        }

        public IEnumerator SaveRoutine()
        {
            IsIteratingSaveHandlers = true;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            Infra.Singleton.StartNewReferenceGraph();

            var handlersToRemove = new List<ISaveAndLoad>();


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

                    if (!handler.IsValid)
                    {
                        handlersToRemove.Add(handler);
                        continue;
                    }

                    handler.WriteSaveData();
                }

                _MoveToNextState();
            }
            while (!_ShouldStopIteratingSaveHandlers());

            //Debug.LogWarning("write " + stopwatch.ElapsedMilliseconds / 1000f);

            IsIteratingSaveHandlers = false;


            if (__currentSaveState == SaveState.Terminate)
            {
                Debug.LogError($"The saving of the objects was terminated. Going to skip the rest of process.");
                yield break;
            }


            foreach (var handler in handlersToRemove)
            {
                //Debug.Log($"[Trace] Removing invalid save handler, datagroup: {handler.DataGroupId}, handled object id: {handler.HandledObjectId}");
                //__mainSaveHandlers.Remove(handler);
                Infra.Singleton.Unregister(handler.HandledObjectId);

                //if (handler is GameObjectSaveHandler goHandler)
                {
                    //Debug.LogWarning(goHandler.__saveData.HierarchyPath);
                }
            }

            Infra.Singleton.RemoveUnreferencedObjects();



            //todo: create an immutable snapshot of the data accesd by background tasks

            var snapshot = new List<ISaveAndLoad>(__mainSaveHandlers);

            //perf test
            //for (int i = 0; i < 100; i++)
            //{
            //    snapshot.AddRange(__mainSaveHandlers);
            //}

            var serTask = Task.Run(() => { return SerializeSnapshot(snapshot); });

            while (!serTask.IsCompleted)
                yield return null;

            if (serTask.Exception != null)
            {
                Debug.LogError("Exception during serialization of save data: " + serTask.Exception);
                yield break;
            }

            IEnumerable<string> flatList = serTask.Result;

            //Debug.LogWarning("serialize " + stopwatch.ElapsedMilliseconds / 1000f);


            string now = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            string fileName = "/savedata_" + now + ".json";

            string relPath = Paths.Singleton.WorldSavePath + fileName;

            var basePath = Application.persistentDataPath; //this is not thread-safe, thats why it is here
            var absPath = Path.Combine(basePath, relPath);

            Debug.Log($"Saving data to {absPath}.");

            var writeTask = Task.Run(() => { WriteSnapshotToDisk(absPath, flatList); });

            while (!writeTask.IsCompleted)
                yield return null;

            if (writeTask.Exception != null)
            {
                Debug.LogError("Exception during writing to disk of save data: " + writeTask.Exception);
                yield break;
            }


            stopwatch.Stop();
            //Debug.LogWarning("save to disk " + stopwatch.ElapsedMilliseconds / 1000f);
            Debug.Log("Save completed.");
        }



        public void WriteSnapshotToDisk(string path, IEnumerable<string> snapshot)
        {
            JsonUtil.WriteObjects(path, snapshot, relative: false);
        }


        public IEnumerable<string> SerializeSnapshot(IEnumerable<ISaveAndLoad> handlers)
        {
            var saveData = new Dictionary<string, List<string>>();


            foreach (var handler in handlers)
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


            IEnumerable<string> flatList = saveData.SelectMany(x => x.Value);

            return flatList;
        }








        public SceneReference _mainMenuSceneRef;
        public SceneReference _emptySceneForLoadingFromSaveFile;


        public Coroutine __loadCoroutine;

        public AsyncOperation op;


        public enum LoadingStage
        {
            LoadingObjects,
            LoadingScenes,
            Completed,
        }

        public class LoadingDebugContext
        {
            public LoadingStage CurrentStage;
            public ISaveAndLoad handler;
        }

        public LoadingDebugContext d_laodingContext = new();



        public void Load(string saveFileAbsPath)
        {
            try
            {
                __loadCoroutine = StartCoroutine(LoadRoutine(saveFileAbsPath));
            }
            catch (Exception e)
            {

                throw;
            }
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


            //yield return StartCoroutine(LoadEmptyScene());

            //yield return StartCoroutine(UnLoadMainMenuScene());



            Debug.Log($"Loading save file from {saveFileAbsPath}.");



            List<string> objects = JsonUtil.ReadObjects(saveFileAbsPath, relative: false);

            List<ISaveAndLoad> saveHandlers = new List<ISaveAndLoad>();

            int i = 0;


            try
            {
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
            }
            catch (Exception e)
            {
                Debug.LogError($"Error during loading from save file at object index {i}: {e.Message}. The object that caused the error:\n{objects[i]}");
                throw;
            }




            d_laodingContext.CurrentStage = LoadingStage.LoadingObjects;


            try
            {
                var ordered = saveHandlers.GroupBy(handler => handler.MetaData.Order).OrderBy(group => group.Key);

                //these are different stages that build upon each other, iterating over them multiple times is by design, dont put them in one loop
                foreach (var group in ordered)
                {
                    d_loadedOrder = group.Key;

                    foreach (var handler in group)
                    {
                        d_laodingContext.handler = handler;
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
                        d_laodingContext.handler = handler;
                        handler.LoadReferences();
                    }

                    foreach (var handler in group)
                    {
                        d_laodingContext.handler = handler;
                        handler.LoadValues();
                    }



                    foreach (var hanler in group)
                    {
                        if (hanler.HandledType == typeof(SceneManagement))
                        {
                            yield return LoadAllSavedScenes();
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
                }

                d_laodingContext.CurrentStage = LoadingStage.Completed;
            }
            finally
            {
                if (d_laodingContext.CurrentStage != LoadingStage.Completed)
                {
                    Debug.LogError($"Error occurred while loading object with id {d_laodingContext.handler.HandledObjectId}. " +
                                   $"Handled type: {d_laodingContext.handler.HandledType.CleanAssemblyQualifiedName()}. " +
                                   $"At loading stage: {d_laodingContext.CurrentStage}");
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
            yield return null;
            yield return null;


            Scene activeScene = Infra.SceneManagement.ActiveSceneInstanceIdFromSaveFile;
            SceneManager.SetActiveScene(activeScene);


            __isObjectLoading.Clear();
            __integrators.Clear();
            ///todo: clear <see cref="PrefabDescriptionRegistry"/> and <see cref="ScenePlacedObjectRegistry"/>

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


        public IEnumerator UnloadEmptyScene()
        {
            AsyncOperation loadOperation = SceneManager.UnloadSceneAsync(_emptySceneForLoadingFromSaveFile.BuildIndex);

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





        public IEnumerator LoadAllSavedScenes()
        {
            SaveAndLoadManager.Singleton.ExpectingIsObjectLoadingRequest = true;

            yield return Infra.SceneManagement.EnsureScenesAreLoadedFromSaveFile();

            SaveAndLoadManager.Singleton.ExpectingIsObjectLoadingRequest = false;
        }





        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Debug.Log("Saving game...");
                Save();
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

            if (string.IsNullOrEmpty(dataGroupId))
            {
                Debug.LogError("Invalid DataGroupId provided. (null or empty)");
                return;
            }

            __mainSaveHandlers.Remove(saveHandler);
            __saveHandlerByHandledObjectIdLookUp.Remove(saveHandler.HandledObjectId);

            //todo: remove the handled object's delegates from the delegate map



            saveHandler.ReleaseObject();
            //else
            //{
            //    Debug.LogError($"No save handlers found for DataGroupId: {dataGroupId}." +
            //        $"This means that the handler was never added or already have been removed and the object of this handler wants to desubscribe " +
            //        $"a second time. Since this unsubscribing happens mostly on Destroy(), " +
            //        $"it might means the object needs to check if it has already been destroyed.");
            //}
        }











        public bool IsPartOfPrefabOrScenePlaced<T>(RandomId instanceId, out T instance)
        {
            bool isPartOfPrefab = PrefabDescriptionRegistry.IsPartOfPrefab<T>(instanceId, out instance);

            if (isPartOfPrefab)
            {
                return true;
            }

            bool isPartOfScenePlaced = ScenePlacedObjectRegistry.IsScenePlaced<T>(instanceId, out instance);

            if (isPartOfScenePlaced)
            {
                return true;
            }

            instance = default;
            return false;

        }

    }











    public class MemberToInstanceId
    {
        public RandomId memberId;
        public RandomId instanceId;
    }

    public class ArrayMemberToElementIds
    {
        public RandomId memberId;
        public List<RandomId> elementIds = new();
    }



    public class PrefabDescriptionRegistry
    {
        public class PrefabDescription
        {
            public RandomId prefabAssetId;
            public List<MemberToInstanceId> memberToInstanceIds = new();
            public List<ArrayMemberToElementIds> arrayMemberToElementIdsList = new();
        }

        public Dictionary<RandomId, PrefabDescription> _prefabDescriptionByPrefabPartInstanceId = new();

        public Dictionary<RandomId, object> _prefabPartsByInstanceId = new();


        public void RemoveIfPartOfPrefab(RandomId instanceId)
        {
            if (!_IsPartOfPrefab(instanceId)) return;

            _prefabDescriptionByPrefabPartInstanceId.Remove(instanceId);
            _prefabPartsByInstanceId.Remove(instanceId);
        }


        public bool IsPartOfPrefab<T>(RandomId instanceId, out T instance)
        {
            bool isPartOfPrefab = _IsPartOfPrefab(instanceId);

            if (isPartOfPrefab)
            {
                instance = _GetPrefabPart<T>(instanceId);
            }
            else
            {
                instance = default;
            }

            return isPartOfPrefab;
        }


        public bool _IsPartOfPrefab(RandomId instanceId)
        {
            bool isPartOfPrefab = _prefabDescriptionByPrefabPartInstanceId.ContainsKey(instanceId);

            return isPartOfPrefab;
        }


        public T _GetPrefabPart<T>(RandomId prefabPartInstanceId)
        {
            bool isPartOfPrefab = _IsPartOfPrefab(prefabPartInstanceId);

            if (isPartOfPrefab && !_prefabPartsByInstanceId.ContainsKey(prefabPartInstanceId))
            {
                var desc = _prefabDescriptionByPrefabPartInstanceId[prefabPartInstanceId];

                var prefab = AddressableDb.Singleton.GetAssetByIdOrFallback<GameObject>(null, ref desc.prefabAssetId);


                SaveAndLoadManager.Singleton.ExpectingIsObjectLoadingRequest = true;

                var instance = Object.Instantiate(prefab);

                SaveAndLoadManager.Singleton.ExpectingIsObjectLoadingRequest = false;


                var infra = instance.GetComponent<GOInfra>();

                if (infra == null)
                {
                    Debug.LogError($"Invalid workflow. An instance of an object was saved as part of a prefab but the root of its prefab" +
                        $"does not have a {nameof(GOInfra)} component. The root gameobject should have a component that handles prefab workflows." +
                        $"Solution: add a {nameof(GOInfra)} component to the root of the prefab (and set it up correctly).");
                }

                var results = infra.CollectPrefabParts();

                Dictionary<RandomId, object> prefabPartsByPrefabPartId = new();
                Dictionary<RandomId, List<object>> arrayElementsByArrayMemberId = new();

                foreach (var result in results)
                {
                    foreach (var idPair in result.membersById)
                    {
                        prefabPartsByPrefabPartId.Add(idPair.Key, idPair.Value);
                    }
                    foreach (var arrayPair in result.arrayElementMembersByArrayMemberId)
                    {
                        arrayElementsByArrayMemberId.Add(arrayPair.Key, arrayPair.Value);
                    }
                }


                foreach (var idPair in desc.memberToInstanceIds)
                {
                    var part2 = prefabPartsByPrefabPartId[idPair.memberId];

                    _prefabPartsByInstanceId.Add(idPair.instanceId, part2);
                }

                foreach (var arrayPair in desc.arrayMemberToElementIdsList)
                {
                    var elements = arrayElementsByArrayMemberId[arrayPair.memberId];

                    for (int i = 0; i < arrayPair.elementIds.Count; i++)
                    {
                        var elementId = arrayPair.elementIds[i];
                        if (i >= elements.Count)
                        {
                            Debug.LogError("Mismatch in number of array elements found in prefab instance and the number of element Ids stored in PrefabDescription. " +
                                $"GameObject: {instance.HierarchyPath()}, Prefab asset id: {desc.prefabAssetId}, array member id: {arrayPair.memberId}. " +
                                $"Going to skip the rest of the elements.");

                            var idlist = string.Join(", ", arrayPair.elementIds);
                            var types = string.Join(", ", elements.Select(e => e.GetType().Name));

                            Debug.LogError($"Element Ids: {idlist}");
                            Debug.LogError($"Element types: {types}");
                            break;
                        }
                        var element = elements[i];

                        _prefabPartsByInstanceId.Add(elementId, element);
                    }
                }
            }

            var part = _prefabPartsByInstanceId[prefabPartInstanceId];

            return (T)part;
        }




        public void Register(GOInfra infra, List<GraphWalkingResult> results)
        {
            var idPairs = new List<MemberToInstanceId>();
            var arraysAndTheirElementIds = new List<ArrayMemberToElementIds>();


            foreach (var result in results)
            {
                for (int i = 0; i < result.memberIds.Count; i++)
                {
                    var pair = new MemberToInstanceId()
                    {
                        memberId = result.memberIds[i],
                        instanceId = result.generatedIds[i],
                    };

                    idPairs.Add(pair);
                }

                for (int i = 0; i < result.arrayMemberIds.Count; i++)
                {
                    var pair = new ArrayMemberToElementIds()
                    {
                        memberId = result.arrayMemberIds[i],
                        elementIds = result.arrayElementMemberIdsPerArrayMembers[i],
                    };

                    arraysAndTheirElementIds.Add(pair);
                }
            }




            var description = new PrefabDescription()
            {
                prefabAssetId = infra.PrefabAssetId,
                memberToInstanceIds = idPairs,
                arrayMemberToElementIdsList = arraysAndTheirElementIds,
            };

            //Debug.Log(infra.gameObject.HierarchyPath());

            //        Debug.Log(string.Join(", ", _prefabDescriptionByPrefabPartInstanceId.Keys));
            //        Debug.Log(string.Join(", ", instanceIds));


            foreach (var pair in idPairs)
            {
                var id = pair.instanceId;

                if (_prefabDescriptionByPrefabPartInstanceId.ContainsKey(id))
                {
                    Debug.LogError("Duplicate instance id registration in PrefabDescriptionRegistry. " +
                        $"Duplicate instance Id: {id} of memberId: {pair.memberId}");
                    continue;
                }
                _prefabDescriptionByPrefabPartInstanceId.Add(id, description);
            }

            foreach (var pair in arraysAndTheirElementIds)
            {
                foreach (var id in pair.elementIds)
                {
                    if (_prefabDescriptionByPrefabPartInstanceId.ContainsKey(id))
                    {
                        Debug.LogError("Duplicate instance id registration in PrefabDescriptionRegistry. " +
                            $"Duplicate instance Id: {id} of memberId: {pair.memberId}");
                        continue;
                    }
                    _prefabDescriptionByPrefabPartInstanceId.Add(id, description);
                }
            }
        }


        [SaveHandler(id: 107204973066903000, nameof(PrefabDescriptionRegistry), typeof(PrefabDescriptionRegistry), order: -90)]
        public class PrefabDescriptionRegistrySaveHandler : UnmanagedSaveHandler<PrefabDescriptionRegistry, PrefabDescriptionRegistrySaveData>
        {
            public override void WriteSaveData()
            {
                base.WriteSaveData();

                __saveData._prefabDescriptionByPrefabPartInstanceId = GetObjectId(__instance._prefabDescriptionByPrefabPartInstanceId, setLoadingOrder: true);
            }

            public override void CreateObject()
            {
                if (SaveAndLoadManager.PrefabDescriptionRegistry != null)
                {
                    Infra.Singleton.Unregister(SaveAndLoadManager.PrefabDescriptionRegistry);
                    //SaveAndLoadManager.PrefabDescriptionRegistry = null;
                }

                base.CreateObject();

                //SaveAndLoadManager.PrefabDescriptionRegistry = __instance;
            }

            public override void _AssignInstance()
            {
                __instance = SaveAndLoadManager.PrefabDescriptionRegistry;
            }


            public override void LoadReferences()
            {
                base.LoadReferences();
                __instance._prefabDescriptionByPrefabPartInstanceId = GetObjectById<Dictionary<RandomId, PrefabDescription>>(__saveData._prefabDescriptionByPrefabPartInstanceId);
            }
        }

        public class PrefabDescriptionRegistrySaveData : SaveDataBase
        {
            public RandomId _prefabDescriptionByPrefabPartInstanceId;
        }



        [SaveHandler(id: 937204973066903000, nameof(PrefabDescription), typeof(PrefabDescription))]
        public class PrefabDescriptionSaveHandler : UnmanagedSaveHandler<PrefabDescription, PrefabDescriptionSaveData>
        {
            public override void WriteSaveData()
            {
                base.WriteSaveData();
                __saveData.prefabAssetId = __instance.prefabAssetId;
                __saveData.memberToInstanceIds = GetObjectId(__instance.memberToInstanceIds, setLoadingOrder: true);
                __saveData.arrayMemberToElementIdsList = GetObjectId(__instance.arrayMemberToElementIdsList, setLoadingOrder: true);
            }

            public override void LoadReferences()
            {
                base.LoadReferences();
                __instance.prefabAssetId = __saveData.prefabAssetId;
                __instance.memberToInstanceIds = GetObjectById<List<MemberToInstanceId>>(__saveData.memberToInstanceIds);
                __instance.arrayMemberToElementIdsList = GetObjectById<List<ArrayMemberToElementIds>>(__saveData.arrayMemberToElementIdsList);
            }
        }

        public class PrefabDescriptionSaveData : SaveDataBase
        {
            public RandomId prefabAssetId;
            public RandomId memberToInstanceIds;
            public RandomId arrayMemberToElementIdsList;
        }
    }










    public class SceneDescription
    {
        public RandomId sceneId;
        public List<MemberToInstanceId> memberToInstanceIds = new();
        public List<ArrayMemberToElementIds> arrayMemberToElementIdsList = new();
    }


    public class ScenePlacedObjectRegistry
    {
        public Dictionary<RandomId, SceneDescription> _sceneDescriptionBySceneObjectId = new();

        public Dictionary<RandomId, object> _sceneObjectsById = new();


        public void RemoveIfScenePlaced(RandomId objectId)
        {
            if (!_IsScenePlaced(objectId)) return;

            _sceneDescriptionBySceneObjectId.Remove(objectId);
            _sceneObjectsById.Remove(objectId);
        }


        public bool IsScenePlaced<T>(RandomId instanceId, out T instance)
        {
            bool isScenePlaced = _IsScenePlaced(instanceId);

            if (isScenePlaced)
            {
                instance = _GetSceneObject<T>(instanceId);
            }
            else
            {
                instance = default;
            }

            return isScenePlaced;
        }


        public bool _IsScenePlaced(RandomId instanceId)
        {
            bool isScenePlaced = _sceneDescriptionBySceneObjectId.ContainsKey(instanceId);

            return isScenePlaced;
        }


        public T _GetSceneObject<T>(RandomId sceneObjectInstanceId)
        {
            bool isScenePlaced = _IsScenePlaced(sceneObjectInstanceId);

            if (isScenePlaced && !_sceneObjectsById.ContainsKey(sceneObjectInstanceId))
            {
                var desc = _sceneDescriptionBySceneObjectId[sceneObjectInstanceId];

                Scene scene = Infra.SceneManagement.SceneById(desc.sceneId);

                SceneInfra infra = Infra.SceneManagement.SceneInfrasBySceneHandle[scene.handle];


                var results = infra.CollectScenePlacedObjects();

                Dictionary<RandomId, object> sceneObjectsByMemberId = new();
                Dictionary<RandomId, List<object>> arrayElementsByArrayMemberId = new();

                foreach (var result in results)
                {
                    foreach (var idPair in result.membersById)
                    {
                        sceneObjectsByMemberId.Add(idPair.Key, idPair.Value);
                    }
                    foreach (var arrayPair in result.arrayElementMembersByArrayMemberId)
                    {
                        arrayElementsByArrayMemberId.Add(arrayPair.Key, arrayPair.Value);
                    }
                }


                foreach (var idPair in desc.memberToInstanceIds)
                {
                    var part2 = sceneObjectsByMemberId[idPair.memberId];

                    _sceneObjectsById.Add(idPair.instanceId, part2);
                }

                foreach (var arrayPair in desc.arrayMemberToElementIdsList)
                {
                    var elements = arrayElementsByArrayMemberId[arrayPair.memberId];

                    for (int i = 0; i < arrayPair.elementIds.Count; i++)
                    {
                        var elementId = arrayPair.elementIds[i];
                        var element = elements[i];

                        _sceneObjectsById.Add(elementId, element);
                    }
                }
            }

            var part = _sceneObjectsById[sceneObjectInstanceId];

            return (T)part;
        }



        public void Register(SceneInfra infra, List<GraphWalkingResult> results)
        {
            var idPairs = new List<MemberToInstanceId>();
            var arraysAndTheirElementIds = new List<ArrayMemberToElementIds>();

            foreach (var result in results)
            {
                for (int i = 0; i < result.memberIds.Count; i++)
                {
                    var pair = new MemberToInstanceId()
                    {
                        memberId = result.memberIds[i],
                        instanceId = result.generatedIds[i],
                    };

                    idPairs.Add(pair);
                }

                for (int i = 0; i < result.arrayMemberIds.Count; i++)
                {
                    var pair = new ArrayMemberToElementIds()
                    {
                        memberId = result.arrayMemberIds[i],
                        elementIds = result.arrayElementMemberIdsPerArrayMembers[i],
                    };

                    arraysAndTheirElementIds.Add(pair);
                }
            }



            var description = new SceneDescription()
            {
                sceneId = Infra.SceneManagement.SceneIdByHandle(infra.gameObject.scene.handle),
                memberToInstanceIds = idPairs,
                arrayMemberToElementIdsList = arraysAndTheirElementIds,
            };

            //Debug.Log(infra.gameObject.HierarchyPath());

            //        Debug.Log(string.Join(", ", _prefabDescriptionByPrefabPartInstanceId.Keys));
            //        Debug.Log(string.Join(", ", instanceIds));


            foreach (var pair in idPairs)
            {
                var id = pair.instanceId;

                if (_sceneDescriptionBySceneObjectId.ContainsKey(id))
                {
                    Debug.LogError("Duplicate instance id registration in ScenePlacedObjectRegistry. " +
                        $"Duplicate instance Id: {id} of memberId: {pair.memberId}");
                    continue;
                }
                _sceneDescriptionBySceneObjectId.Add(id, description);
            }

            foreach (var pair in arraysAndTheirElementIds)
            {
                foreach (var id in pair.elementIds)
                {
                    if (_sceneDescriptionBySceneObjectId.ContainsKey(id))
                    {
                        Debug.LogError("Duplicate instance id registration in ScenePlacedObjectRegistry. " +
                            $"Duplicate instance Id: {id} of memberId: {pair.memberId}");
                        continue;
                    }
                    _sceneDescriptionBySceneObjectId.Add(id, description);
                }
            }
        }






        [SaveHandler(id: 207204973066903000, nameof(ScenePlacedObjectRegistry), typeof(ScenePlacedObjectRegistry), order: -90)]
        public class ScenePlacedObjectRegistrySaveHandler : UnmanagedSaveHandler<ScenePlacedObjectRegistry, ScenePlacedObjectRegistrySaveData>
        {
            public override void WriteSaveData()
            {
                base.WriteSaveData();

                __saveData._sceneDescriptionBySceneObjectId = GetObjectId(__instance._sceneDescriptionBySceneObjectId, setLoadingOrder: true);
            }

            public override void CreateObject()
            {
                if (SaveAndLoadManager.ScenePlacedObjectRegistry != null)
                {
                    Infra.Singleton.Unregister(SaveAndLoadManager.ScenePlacedObjectRegistry);
                    //SaveAndLoadManager.ScenePlacedObjectRegistry = null;
                }

                base.CreateObject();

                //SaveAndLoadManager.ScenePlacedObjectRegistry = __instance;
            }

            public override void _AssignInstance()
            {
                __instance = SaveAndLoadManager.ScenePlacedObjectRegistry;
            }


            public override void LoadReferences()
            {
                base.LoadReferences();
                __instance._sceneDescriptionBySceneObjectId = GetObjectById<Dictionary<RandomId, SceneDescription>>(__saveData._sceneDescriptionBySceneObjectId);
            }
        }

        public class ScenePlacedObjectRegistrySaveData : SaveDataBase
        {
            public RandomId _sceneDescriptionBySceneObjectId;
        }



        [SaveHandler(id: 900204973066903000, nameof(SceneDescription), typeof(SceneDescription))]
        public class SceneDescriptionSaveHandler : UnmanagedSaveHandler<SceneDescription, SceneDescriptionSaveData>
        {
            public override void WriteSaveData()
            {
                base.WriteSaveData();
                __saveData.sceneId = __instance.sceneId;
                __saveData.memberToInstanceIds = GetObjectId(__instance.memberToInstanceIds, setLoadingOrder: true);
                __saveData.arrayMemberToElementIdsList = GetObjectId(__instance.arrayMemberToElementIdsList, setLoadingOrder: true);
            }

            public override void LoadReferences()
            {
                base.LoadReferences();
                __instance.sceneId = __saveData.sceneId;
                __instance.memberToInstanceIds = GetObjectById<List<MemberToInstanceId>>(__saveData.memberToInstanceIds);
                __instance.arrayMemberToElementIdsList = GetObjectById<List<ArrayMemberToElementIds>>(__saveData.arrayMemberToElementIdsList);
            }
        }

        public class SceneDescriptionSaveData : SaveDataBase
        {
            public RandomId sceneId;
            public RandomId memberToInstanceIds;
            public RandomId arrayMemberToElementIdsList;
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