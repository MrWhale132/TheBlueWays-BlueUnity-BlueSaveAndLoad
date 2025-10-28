using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.CodeGen;
using Assets._Project.Scripts.UtilScripts.Extensions;
using Assets._Project.Scripts.UtilScripts.Misc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;



namespace Assets._Project.Scripts.Infrastructure
{//TODO: remove it later once it moved into the bootstrap scene
    [DefaultExecutionOrder(-10000)]
    public class Infra : MonoBehaviour
    {
        public static Infra Singleton { get; private set; }

        public RandomId GlobalReferencing => __staticSubtituteIdsByType[typeof(Infra)];



        public HashSet<RandomId> __rootObjectIds;
        //this is error prone since it only works with types whose GetHashCode() does not change
        //most ref types are safe, value types are not
        public Dictionary<object, RandomId> __objectIds;

        public Dictionary<RandomId, object> __globalReferenceCache;

        public Dictionary<RandomId, List<RandomId>> __objectReferences;





        private void Awake()
        {
            if (Singleton != null)
            {
                Debug.LogError("Infra instance already exists. Destroying the new instance.");
                Destroy(gameObject);
                return;
            }

            Singleton = this;
            DontDestroyOnLoad(gameObject);

            __rootObjectIds = new();
            __objectIds = new(MyReferenceEqualityComparer.Instance);
            __globalReferenceCache = new();
            __objectReferences = new();

            CreateDefaultJsonSerializerSettings();

        }





        public void StartNewReferenceGraph()
        {
            __objectReferences.Clear();
        }


        public HashSet<RandomId> GetReferencedObjects()
        {
            return _GetReachableObjects(__objectReferences, __rootObjectIds);
        }


        //depth first search algorithm
        public HashSet<RandomId> _GetReachableObjects(
                        Dictionary<RandomId, List<RandomId>> objectReferences,
                        IEnumerable<RandomId> rootIds)
        {
            var reachable = new HashSet<RandomId>();
            var stack = new Stack<RandomId>(rootIds);

            while (stack.Count > 0)
            {
                RandomId current = stack.Pop();
                if (!reachable.Add(current))
                    continue;

                if (objectReferences.TryGetValue(current, out var references))
                {
                    foreach (RandomId target in references)
                        stack.Push(target);
                }
            }

            return reachable;
        }


        public void _AddToReferenceGraph(RandomId referenced, RandomId referencedBy)
        {
            if (!__objectReferences.ContainsKey(referencedBy))
            {
                __objectReferences[referencedBy] = new List<RandomId>();
            }

            __objectReferences[referencedBy].Add(referenced);
        }


        public void KeepAlive(RandomId referenced, RandomId referencedBy)
        {
            if (referenced.IsDefault)
            {
                Debug.LogError("Referenced object id is not set.");
                return;
            }
            if (referencedBy.IsDefault)
            {
                Debug.LogError("ReferencedBy object is not set.");
                return;
            }

            _AddToReferenceGraph(referenced, referencedBy);
        }










        public void RegisterReference(object reference, RandomId key, bool rootObject = false)
        {
            bool isNull = reference is Object unityObject ?
                unityObject == null : reference == null;

            if (isNull)
            {
                Debug.LogError("Can not register null reference.");
                return;
            }

            if (key.IsDefault)
            {
                Debug.LogError("Can not register default id.");
                return;
            }

            if (__globalReferenceCache.ContainsKey(key))
            {
                bool same = reference == __globalReferenceCache[key];

                if (same)
                    Debug.LogError($"Infra: Reference with key {key} is already registered. Skipping registration.");
                else
                    Debug.LogError($"Infra: Object id {key} is already registered for a different object refernce. Skipping registration.");

                return;
            }

            if (__objectIds.ContainsKey(reference))
            {
                bool same = key == __objectIds[reference];

                if (same)
                    Debug.LogError($"Infra: Object Reference is already registered with this id: {key}");
                else
                    Debug.LogError($"Infra: Object reference is already registered with a different id. " +
                        $"Requested id: {key} " +
                        $"Found id: {__objectIds[reference]}");

                return;
            }

            __objectIds.Add(reference, key);
            __globalReferenceCache.Add(key, reference);

            if (rootObject)
                __rootObjectIds.Add(key);
        }



        public object GetReference(RandomId key)
        {
            if (key == RandomId.Default)
            {
                return null;
            }


            if (__globalReferenceCache.TryGetValue(key, out var reference))
            {
                return reference;
            }
            else
            {
                Debug.LogError($"Infra: Reference with key {key} not found.");
                return null;
            }
        }

        public T GetObjectById<T>(RandomId id)
        {
            var reference = GetReference(id);


            if (reference == null) return default;


            if (reference is T typedReference)
            {
                return typedReference;
            }
            else
            {
                Debug.LogError($"Infra: Object with id {id} is not of type {typeof(T)}. Object type is: {reference?.GetType()}");
                return default;
            }
        }







        public RandomId GetObjectId(Component obj, RandomId referencedBy, bool setLoadingOrder = false)
        {
            if (obj == null) return RandomId.Default;


            if (referencedBy.IsDefault)
            {
                Debug.LogError($"An object can not be referenced by a DefaultId.");
            }


            var objectId = _GetObjectIdWithoutReferencing(obj, autoRegister: false);

            if (!objectId.IsDefault)
            {
                _AddToReferenceGraph(objectId, referencedBy);

                if (setLoadingOrder)
                {
                    _SetLoadingOrder(objectId, referencedBy);
                }

                return objectId;
            }


            bool isProbablyPrefab = obj.gameObject.IsProbablyPrefab();

            if (isProbablyPrefab && !IsRegistered(obj.gameObject))
            {
                _RegisterPrefab(obj.gameObject);
            }

            objectId = Register(obj, rootObject: true, createSaveHandler: !isProbablyPrefab);
            return objectId;
        }


        public RandomId GetObjectId(GameObject obj, RandomId referencedBy, bool setLoadingOrder = false)
        {
            if (obj == null) return RandomId.Default;


            if (referencedBy.IsDefault)
            {
                Debug.LogError($"An object can not be referenced by a DefaultId.");
            }


            var objectId = _GetObjectIdWithoutReferencing(obj, autoRegister: false);

            if (!objectId.IsDefault)
            {
                _AddToReferenceGraph(objectId, referencedBy);

                if (setLoadingOrder)
                {
                    _SetLoadingOrder(objectId, referencedBy);
                }

                return objectId;
            }


            if (obj.IsProbablyPrefab())
            {
                return _RegisterPrefab(obj);
            }
            else
            {
                objectId = Register(obj, rootObject: true, createSaveHandler: true);
                return objectId;
            }
        }


        public RandomId _RegisterPrefab(GameObject obj)
        {
            //Debug.Log("register prefab "+obj.name);
            if (obj.transform.parent != null)
            {
                Debug.LogError($"Only the root of a prefab can be registered. " +
                    "This error most likely indicates that has reference to a child object of a prefab template. " +
                    $"Child object path: {obj.transform.root.gameObject.HierarchyPath()} ");
                return RandomId.Default;
            }


            var objectId = AddressableDb.Singleton.GetAssetIdByAssetName(obj);

            if (objectId.IsDefault) return objectId;


            RegisterReference(obj, objectId, rootObject: true);


            _CreateSaveHandler(obj);

            //Debug.LogWarning("registered prefab " + obj.name +" with "+ objectId);
            return objectId;
        }


        public RandomId GetObjectId(object obj, RandomId referencedBy, bool setLoadingOrder = false)
        {
            if (referencedBy.IsDefault)
            {
                Debug.LogError($"An object can not be referenced by a DefaultId.");
            }

            var objectId = _GetObjectIdWithoutReferencing(obj, autoRegister: true);

            _AddToReferenceGraph(objectId, referencedBy);

            if (setLoadingOrder)
            {
                _SetLoadingOrder(objectId, referencedBy);
            }

            return objectId;
        }


        //this order setting thing is may not necessary to keep track. It may can be done on saving by using the reference graph
        //the referencing object depends on the referenced object, so the referenced object should be loaded earlier
        public void _SetLoadingOrder(RandomId referenced, RandomId referencing)
        {
            //todo
            var lookUp = SaveAndLoadManager.Singleton.__saveHandlerByHandledObjectIdLookUp;
            
            if (lookUp.TryGetValue(referenced, out var dependency) && lookUp.TryGetValue(referencing, out var dependant))
            {
                dependant.Order = Math.Max(dependency.Order/* + 1*/, dependant.Order); //that + 1 need some more thinking, it can create self-referencing loops more often
            }
        }

        public RandomId _GetObjectIdWithoutReferencing(object obj, bool autoRegister = true)
        {
            if ((obj is UnityEngine.Object unityObject && unityObject == null)
                || obj == null)
            {
                return RandomId.Default;
            }


            if (__objectIds.TryGetValue(obj, out RandomId id))
            {
                return id;
            }
            else if (!autoRegister)
            {
                return RandomId.Default;
            }

            //warning: this way if non-unity objects (pure c#) are unregistering themself they will be added back if someone
            //still referencing them, unable to unregister themself.

            id = Register(obj);

            return id;
        }


        public RandomId Register(object obj, bool rootObject = false, bool createSaveHandler = true)
        {
            if (__objectIds.ContainsKey(obj))
            {
                var id = __objectIds[obj];
                //Debug.LogWarning($"Infra: object with id {id} is already registered. Skipping registration.");
                //todo: check if it has savehandler, it should if it is already enlisted. Except if multiple caller want to register this obj and they supply different value for createSaveHandler
                //if (rootObject)
                //{
                //    if (!__rootObjectIds.Contains(id))
                //    {
                //        Debug.LogError("an object is already registered but it was not requested to be a root object that time.");
                //    }
                //}
                return id;
            }


            RandomId objId = RandomId.Get();

            __objectIds.Add(obj, objId);
            __globalReferenceCache.Add(objId, obj);

            if (rootObject)
            {
                __rootObjectIds.Add(objId);
            }



            if (createSaveHandler)
            {
                _CreateSaveHandler(obj);
            }


            return objId;
        }

        

        public void _CreateSaveHandler(object obj)
        {
            var handler = SaveAndLoadManager.Singleton.GetSaveHandlerFor(obj);
            if (handler != null)
            {
                handler.Init(obj);
                SaveAndLoadManager.Singleton.AddSaveHandler(handler);
            }
            else
            {
                Debug.LogWarning($"[Infra][{UnregisteredObjectTypeLogCode}] A type has no savehandler registered for it. {TypeAnnotation} {obj.GetType().CleanAssemblyQualifiedName()}");
            }
        }

        //used by editor scripts
        public const string TypeAnnotation = "$type:";
        public const int UnregisteredObjectTypeLogCode = 10;



        public void Unregister(object obj)
        {
            if (!__objectIds.TryGetValue(obj, out var id))
            {
                Debug.LogWarning($"Infra: object is not registered. Skipping unregistration.");
                return;
            }


            SaveAndLoadManager.Singleton.RemoveSaveHandler(id);


            if (__rootObjectIds.Contains(id))
            {
                __rootObjectIds.Remove(id);
            }

            __objectIds.Remove(obj);
            __globalReferenceCache.Remove(id);
        }


        //maybe this does not need a UnityEngine.Object overload (?)
        public bool IsRegistered(object obj)
        {
            if (obj == null) return false;

            return __objectIds.ContainsKey(obj);
        }





        public Dictionary<Type, Dictionary<string, long>> __methodIdsByMethodSignaturePerType = new();
        public Dictionary<Type, Func<long, Func<object, Delegate>>> __methodGetterFactoryPerType = new();
        public Dictionary<Type, Dictionary<long, Func<object, Delegate>>> __methodGettersByIdPerType = new();

        public Dictionary<Type, Func<long, MethodInfo>> __methodInfoGettersPerType = new();
        public Dictionary<Type, Dictionary<long, MethodInfo>> __genericMethodDefGettersByIdPerType = new();

        public Dictionary<RandomId, Dictionary<long, Dictionary<long, Delegate>>> __delegateMapPerInstance = new();





        public void AddMethodSignatureToMethodIdMap(Type type, Dictionary<string, long> methodToIdMap)
        {
            if (!__methodIdsByMethodSignaturePerType.ContainsKey(type))
            {
                __methodIdsByMethodSignaturePerType.Add(type, methodToIdMap);
            }
            else
            {
                var dict = __methodIdsByMethodSignaturePerType[type];
                foreach (var pair in methodToIdMap)
                {
                    if (!dict.ContainsKey(pair.Key))
                    {
                        dict.Add(pair.Key, pair.Value);
                    }
                    else
                    {
                        Debug.LogError($"The method signature {pair.Key} is already registered for type {type.CleanAssemblyQualifiedName()}. Skipping registration.");
                    }
                }
            }
        }


        public void AddMethodIdToMethodMap(Type type, Func<long, Func<object, Delegate>> idToMethodMap)
        {
            if(!__methodGetterFactoryPerType.ContainsKey(type))
                __methodGetterFactoryPerType.Add(type, idToMethodMap);
            else
                __methodGetterFactoryPerType[type] += idToMethodMap;
        }

        public void AddMethodIdToMethodInfoMap(Type type, Func<long, MethodInfo> idToMethodInfoMap)
        {
            if (!__methodInfoGettersPerType.ContainsKey(type))
                __methodInfoGettersPerType.Add(type, idToMethodInfoMap);
            else
                __methodInfoGettersPerType[type] += idToMethodInfoMap;
        }



        public void EnsureSaveHandlerTypeIsInitialized(Type type)
        {
            if (type != null)
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle); //enforce static ctor if not yet ran
        }


        //delegate helpers

        public Func<long, Func<object, Delegate>> GetIdToMethodMapForType(Type type)
        {
            if (!__methodGetterFactoryPerType.ContainsKey(type))
            {
                Type saveHandler = SaveAndLoadManager.Singleton.GetSaveHandlerTypeFrom(type);

                EnsureSaveHandlerTypeIsInitialized(saveHandler);

                if (!__methodGetterFactoryPerType.ContainsKey(type))
                {
                    Debug.LogError($"The type {type.CleanAssemblyQualifiedName()} does not have a methodid to method map registered. " +
                    $"This means the type's savehandler, if it is exists, initialization logic did not run before something " +
                    $"else queried saveandload logic about this type. (Possible culprit: method reference through reflection during runtime.)" +
                    $" | 25.9");

                    return (id) => null;
                }
            }

            return __methodGetterFactoryPerType[type];
        }

        public Func<long, MethodInfo> GetMethodInfoIdToMethodMapForType(Type type)
        {
            if (!__methodInfoGettersPerType.ContainsKey(type))
            {
                Type saveHandler = SaveAndLoadManager.Singleton.GetSaveHandlerTypeFrom(type);

                EnsureSaveHandlerTypeIsInitialized(saveHandler);

                if (!__methodInfoGettersPerType.ContainsKey(type))
                {
                    Debug.LogError($"The type {type.CleanAssemblyQualifiedName()} does not have a gen method def id to method map registered. " +
                    $"This means the type's savehandler, if it is exists, initialization logic did not run before something " +
                    $"else queried saveandload logic about this type. (Possible culprit: method reference through reflection during runtime.)" +
                    $" | 25.9");

                    return (id) => null;
                }
            }

            return __methodInfoGettersPerType[type];
        }







        //for loading
        public T GetDelegate<T>(InvocationList invocationList)
        {
            if (typeof(Delegate).IsAssignableFrom(typeof(T)) == false)
            {
                Debug.LogError($"The type {typeof(T).Name} is not a delegate type. " +
                                $"Please ensure you are trying to get a delegate of the correct type. " +
                                $"Going to return default value.");
                return default;
            }

            if (invocationList == null || invocationList.Delegates == null || invocationList.Delegates.Count == 0)
            {
                return default;
            }


            var dells = new List<Delegate>();

            foreach (var saveInfo in invocationList.Delegates)
            {
                var singleDel = GetDelegate<T>(saveInfo);

                if (singleDel != null)
                {
                    dells.Add((Delegate)(object)singleDel);
                }
            }

            T del = (T)(object)Delegate.Combine(dells.ToArray());
            return del;
        }



        public T GetDelegate<T>(DelegateSaveInfo saveInfo)
        {
            if (saveInfo == null)
            {
                return default;
            }

            if (!typeof(Delegate).IsAssignableFrom(typeof(T)))
            {
                Debug.LogError($"The type {typeof(T).Name} is not a delegate type. " +
                    $"Please ensure you are trying to get a delegate of the correct type. " +
                    $"Going to return default value.");
                return default;
            }

            RandomId targetId = saveInfo.TargeId;
            long methodId = saveInfo.MethodId;
            long variantId = saveInfo.IsGeneric ? saveInfo.GenericVairantId : methodId;


            if (!__delegateMapPerInstance.ContainsKey(targetId))
            {
                __delegateMapPerInstance[targetId] = new();
            }
            if (!__delegateMapPerInstance[targetId].ContainsKey(methodId))
            {
                __delegateMapPerInstance[targetId][methodId] = new();
            }


            //if cached, return it
            if (__delegateMapPerInstance[targetId][methodId].ContainsKey(variantId))
            {
                var del = __delegateMapPerInstance[targetId][methodId][variantId];


                T typedDel = TryGetOrCreateDelegate(del);

                if (typedDel != null) //todo: ??? , why null does work and default does not? shouldnt be the opposite? (replace null with default and see the error)
                {
                    //__delegateMapPerInstance[targetId][methodId].Add(variantId, del);
                }

                return typedDel;
            }
            //if not, get it and cache it
            else
            {
                //pay special attention to static methods, they have a targetId too that points to the StaticSubtitute instance
                //also pay attention that in case of generics, method id represents the generic method def id
                //in case of non-generics, method id and variant id are the same

                var target = GetReference(targetId);

                if (target == null) return default;


                //todo: build lookup
                Type declaringTpye = target is StaticSubtitute subtitute
                                ? subtitute.SubtitutedType
                                : target.GetType();


                //if (declaringTpye.IsGenericType) declaringTpye = declaringTpye.GetGenericTypeDefinition();


                if (!__methodGettersByIdPerType.ContainsKey(declaringTpye))
                {
                    __methodGettersByIdPerType[declaringTpye] = new();
                }
                if (!__genericMethodDefGettersByIdPerType.ContainsKey(declaringTpye))
                {
                    __genericMethodDefGettersByIdPerType[declaringTpye] = new();
                }

                if (!saveInfo.GetByMethodInfo)
                {
                    if (!__methodGettersByIdPerType[declaringTpye].ContainsKey(methodId))
                    {
                        __methodGettersByIdPerType[declaringTpye][methodId] = __methodGetterFactoryPerType[declaringTpye](methodId);
                    }
                }
                else
                {
                    if (!__methodGettersByIdPerType[declaringTpye].ContainsKey(variantId))
                    {
                        //get the closed generic method that can be used to get the methodDef and cache it
                        if (!__genericMethodDefGettersByIdPerType[declaringTpye].ContainsKey(methodId))
                        {
                            __genericMethodDefGettersByIdPerType[declaringTpye][methodId] = __methodInfoGettersPerType[declaringTpye](methodId);
                        }

                        MethodInfo methodInfo = __genericMethodDefGettersByIdPerType[declaringTpye][methodId];

                        if (saveInfo.IsGeneric)
                        {
                            Type[] typeArgs = new Type[saveInfo.GenericTypeArguments.Count];

                            for (int i = 0; i < typeArgs.Length; i++)
                            {
                                string stringType = saveInfo.GenericTypeArguments[i];
                                Type type = Type.GetType(stringType);
                                typeArgs[i] = type;
                            }

                            MethodInfo concreteGeneric = methodInfo.MakeGenericMethod(typeArgs);

                            methodInfo = concreteGeneric;
                        }

                        Func<object, Delegate> getter = (instance) => methodInfo.CreateDelegate(typeof(T), instance);

                        //update the method getter cache to directly create the closed generic next time
                        __methodGettersByIdPerType[declaringTpye][variantId] = getter;
                    }
                }

                //calling the getter with an other type in case of static methods is fine as the instance is not used
                var del = __methodGettersByIdPerType[declaringTpye][variantId](target);


                T typedDel = TryGetOrCreateDelegate(del);

                if (typedDel != null) //todo: ??? , why null does work and default not? shouldnt be the opposite? (replace null with default and see error)
                {
                    __delegateMapPerInstance[targetId][methodId].Add(variantId, del);
                }

                return typedDel;
            }


            T TryGetOrCreateDelegate(Delegate del)
            {
                if (del is T typedDel)
                {
                    return typedDel;
                }


                //as of now, the save system only works with Action and Func, if the above type check failes because the requested
                //type is a for example UnityAction, or any other delegate type, then we try to cast the Action or Func into that type

                Type systemDelType = typeof(System.Action);
                Type targetType = typeof(T);

                bool targetisActionOrFunc = targetType.Assembly.FullName == systemDelType.Assembly.FullName
                    && (targetType.Name.StartsWith("Action") || targetType.Name.StartsWith("Func"));


                if (!targetisActionOrFunc)
                {
                    try
                    {
                        typedDel = (T)(object)Delegate.CreateDelegate(targetType, del.Target, del.Method);

                        return typedDel;
                    }
                    catch { Debug.Log("catch"); }
                }

                Debug.LogError($"The delegate found for targetId {targetId}, methodId {methodId}, variantId {variantId} " +
                    $"is not of the expected type {typeof(T).AssemblyQualifiedName}. It is of type {del.GetType().AssemblyQualifiedName}. " +
                    $"Going to return default value.");
                return default;
            }
        }




        //for saving
        public InvocationList GetInvocationList<T>(T del) where T : System.Delegate
        {
            if (del == null)
            {
                return null;
            }

            var delegates = del.GetInvocationList();
            var saveInfos = new List<DelegateSaveInfo>();

            foreach (var singleDel in delegates)
            {
                var saveInfo = GetDelegateSaveInfo(singleDel);

                if (saveInfo != null)
                {
                    saveInfos.Add(saveInfo);
                }
            }

            if (saveInfos.Count == 0)
            {
                return null;
            }

            return new InvocationList { Delegates = saveInfos };
        }




        public DelegateSaveInfo GetDelegateSaveInfo(Delegate del)
        {
            return GetDelegateSaveInfo(del.Method, del.Target);
        }


        public DelegateSaveInfo GetDelegateSaveInfo(MethodInfo Method, object Target)
        {
            bool isStatic = Target == null;

            RandomId targetId = isStatic ?
                  GetStaticSubtituteId(Method.DeclaringType)
                //TODO: accept the caller object too and use it as the referencee to count the target as referenced
                //or just use .Keepalive() on it
                : _GetObjectIdWithoutReferencing(Target);

            Type targetType = Method.DeclaringType;

            //if (targetType.IsGenericType) targetType = targetType.GetGenericTypeDefinition();


            string signature = CodeGenUtils.GetMethodSignature(Method);



            

            if (!__methodIdsByMethodSignaturePerType.ContainsKey(targetType))
            {
                Type saveHandler = SaveAndLoadManager.Singleton.GetSaveHandlerTypeFrom(targetType);

                EnsureSaveHandlerTypeIsInitialized(saveHandler);

                if (!__methodIdsByMethodSignaturePerType.ContainsKey(targetType))
                {
                    Debug.LogError($"No method ids registered for type {targetType}. Method name: {Method.Name}, TargetId: {targetId} . " +
                        $"Please ensure you register method ids for this type before trying to get delegate save infos for its methods. " +
                        $"Going to return null.");
                    return null;
                }
            }



            //if the delegate registered return a saveinfo
            if (__methodIdsByMethodSignaturePerType[targetType].TryGetValue(signature, out var variantId))
            {
                long methodId = variantId;

                if (Method.IsGenericMethod)
                {
                    //todo: cache the signature per MethodInfo
                    string methodDefSignature = CodeGenUtils.GetMethodSignature(Method.GetGenericMethodDefinition());
                    methodId = __methodIdsByMethodSignaturePerType[targetType][methodDefSignature];
                }


                //todo: this instance should be cached too
                var saveInfo = new DelegateSaveInfo(targetId, methodId)
                {
                    GetByMethodInfo = Method.IsGenericMethod || Method.GetParameters().Any(p => p.ParameterType.CanNotBeUsedAsGenericParameter()) || Method.ReturnType.CanNotBeUsedAsGenericParameter(),
                    GenericVairantId = variantId,
                    //todo: to get the type args every single time is costly, we could cache them per method info
                    GenericTypeArguments = Method.IsGenericMethod
                        ? Method.GetGenericArguments().Select(arg => arg.AssemblyQualifiedName).ToList()
                        : null,
                };
                return saveInfo;
            }
            //if not, check if generic, if it is, no problem, their different closed construct versions registered dynamicaly, otherwise its an error
            else
            {
                if (Method.IsGenericMethod)
                {
                    var methodDef = Method.GetGenericMethodDefinition();

                    var methodDefSignature = CodeGenUtils.GetMethodSignature(methodDef);


                    if (__methodIdsByMethodSignaturePerType[targetType].TryGetValue(methodDefSignature, out var methodId))
                    {
                        variantId = long.Parse(RandomId.Get().ToString());

                        var saveInfo = new DelegateSaveInfo(targetId, methodId)
                        {
                            GetByMethodInfo = Method.IsGenericMethod || Method.GetParameters().Any(p => p.ParameterType.CanNotBeUsedAsGenericParameter()) || Method.ReturnType.CanNotBeUsedAsGenericParameter(),
                            GenericVairantId = variantId,
                            GenericTypeArguments = Method.GetGenericArguments().Select(arg => arg.AssemblyQualifiedName).ToList(),
                        };

                        __methodIdsByMethodSignaturePerType[targetType][signature] = variantId;

                        return saveInfo;
                    }
                    else
                    {
                        Debug.LogError($"No method id found for generic method definition signature {methodDefSignature} in type {targetType}. " +
                            $"Please ensure you register a method id for this method before trying to get delegate save infos for it. " +
                            $"Going to return null.");
                        return null;
                    }
                }
                else
                {
                    Debug.LogError($"No method id found for method signature {signature} in type {targetType}. " +
                        $"Please ensure you register a method id for this method before trying to get delegate save infos for it. " +
                        $"Going to return null.");
                    return null;
                }
            }
        }



        //with dynamic registration it is possible that a delegate has multiple registrations with different ids
        //lets imagine a chunk based loading with chunk A and B
        //during a previous play, a delegate was registered dynamically, meaning an id was generated for it during runtime,
        //and was saved with chunk B. (Game is exited and loaded again)
        //Next time the game is loaded with chunk A only, so the game doesnt know about that that delegate was registered before.
        //When something tries to get the delegate save info, it will see that it is not registered and so it will register it.
        //after that, B loads and it loads a saveinfo that represents the same delegate but with a different id
        //thus, the same delegate is identified by multiple ids
        //as long as they point to the same target and method, it should not cause problems, I think...
        //except some performance overhead because we do not register the variant id again if the method and target are the same
        ///thus, closed generic methods will be created again and again, <see cref="GetDelegate{T}(DelegateSaveInfo)"/>
        //currently, only generic variants are registered dynamically

        //fix, todo, error: maybe we could use a bool flag in the saveinfo to indicate that this delegate was registered dynamically
        ///and if true, we dont check if <see cref="__delegatSaveDataByMethodInfo"/> already contains the method info










        #region Old Delegate handling

        //public Dictionary<MethodInfo, DelegateSaveInfo> __delegatSaveDataByMethodInfo = new();



        //public void RegisterDelegate<T>(T del, DelegateSaveInfo saveInfo) where T : Delegate
        //{
        //    _RegisterDelegate(del, saveInfo, ifGenericTreatAsMethodDef: true, isSaveInfoFromLoading: false);
        //}



        //public void _RegisterDelegate<T>(T del, DelegateSaveInfo saveInfo,
        //                                 bool ifGenericTreatAsMethodDef, bool isSaveInfoFromLoading) where T : Delegate
        //{
        //    ///<see cref="ifGenericTreatAsMethodDef"/> should only be true when saving, and not loading
        //    if (ifGenericTreatAsMethodDef && isSaveInfoFromLoading)
        //    {
        //        Debug.LogError("invalid paramter arguments");
        //        return;
        //    }
        //    if (saveInfo == null)
        //    {
        //        Debug.LogError($"Tried to register a delegate with null saveinfo.");
        //        return;
        //    }
        //    if (del == null)
        //    {
        //        Debug.LogError($"Tried to register a null delegate. It is not allowed. Skipping registration. " +
        //            $"TargetId: {saveInfo.TargeId}, MethodId: {saveInfo.MethodId}");
        //        return;
        //    }
        //    if (saveInfo.TargeId.IsDefault)
        //    {
        //        Debug.LogError("Invalid TargetId");
        //        return;
        //    }
        //    //if (isSaveInfoFromLoading && del.Method.IsGenericMethod && saveInfo.GenericVairantId.IsDefault)
        //    //{
        //    //    Debug.LogError("Invalid GenericVariantId");
        //    //    return;
        //    //}


        //    bool nonGenericOrGenericMethodDef = !del.Method.IsGenericMethod || (del.Method.IsGenericMethod && ifGenericTreatAsMethodDef);
        //    bool genericAndNotMethodDef = !nonGenericOrGenericMethodDef;//much easier to read, understand, and work with the following code

        //    var method = del.Method.IsGenericMethod && ifGenericTreatAsMethodDef
        //               ? del.Method.GetGenericMethodDefinition() : del.Method;


        //    if (!__delegatSaveDataByMethodInfo.ContainsKey(method))
        //    {
        //        __delegatSaveDataByMethodInfo.Add(method, saveInfo);
        //    }
        //    else
        //    {
        //        //this error message will spam in case of dynamically registered generic variants, see the explonation above
        //        if (!(isSaveInfoFromLoading && saveInfo.IsGeneric))
        //        {
        //            Debug.LogError($"Duplicate delegate registration detected, it is not allowed. Please make sure to register a delegate only once. " +
        //                $"Method name: {method}, " +
        //                $"MethodId: {saveInfo.MethodId}, TargetId: {saveInfo.TargeId}");
        //            return;
        //        }
        //    }


        //    //method id is the same for all type concrete generic variants of a generic method, example:
        //    //Method<T>, Method<int>, Method<string> ect... will all get the same id
        //    //type arguments info are saved with the delegate save info to be able to further distinct between the variants
        //    RandomId instanceId = saveInfo.TargeId;
        //    long methodId = saveInfo.MethodId;

        //    if (!__delegateMapPerInstance.ContainsKey(instanceId))
        //    {
        //        __delegateMapPerInstance[instanceId] = new();
        //    }

        //    //only one method registration allowed for non-generic methods and one generic method def for generic ones
        //    if (nonGenericOrGenericMethodDef && __delegateMapPerInstance[instanceId].ContainsKey(methodId))
        //    {
        //        Debug.LogError($"Delegate with methodId {methodId} already registered for instance {instanceId}. " +
        //            $"This means that there are multiple delegates with the same methodId for the same instance. " +
        //            $"Please ensure that each delegate has a unique methodId.");
        //        return;
        //    }
        //    else if (genericAndNotMethodDef)
        //    {
        //        if (!__delegateMapPerInstance[instanceId].ContainsKey(methodId))
        //        {
        //            Debug.LogError($"generic method def should have already been registered. " +
        //                $"InstanceId: {instanceId}, MethodId: {methodId}");
        //            return;
        //        }
        //        else if (isSaveInfoFromLoading && __delegateMapPerInstance[instanceId][methodId].ContainsKey(saveInfo.GenericVairantId))
        //        {
        //            Debug.LogError($"duplicate generic variant registration. " +
        //                $"InstanceId: {instanceId}, MethodId: {methodId}, GenericVairantId: {saveInfo.GenericVairantId}");
        //            return;
        //        }
        //        else if (!isSaveInfoFromLoading)
        //        {
        //            //TODO: do something with this
        //            saveInfo.GenericVairantId = long.Parse(RandomId.Get().ToString());

        //            saveInfo.GenericTypeArguments = new List<string>();

        //            foreach (var argType in method.GetGenericArguments())
        //            {
        //                saveInfo.GenericTypeArguments.Add(argType.AssemblyQualifiedName);
        //            }
        //        }
        //    }

        //    //internally, non-generic methods' variant id is the same as their methodid
        //    long variantId = genericAndNotMethodDef ? saveInfo.GenericVairantId : methodId;


        //    if (nonGenericOrGenericMethodDef)
        //    {
        //        __delegateMapPerInstance[instanceId][methodId] = new(capacity: 1);
        //    }
        //    //in case of generics, their generic method def should came first and creating the collection before hand


        //    __delegateMapPerInstance[instanceId][methodId].Add(variantId, del);
        //}



        ////for writing
        //public InvocationList GetInvocationList<T>(T del) where T : System.Delegate
        //{
        //    if (del == null)
        //    {
        //        return null;
        //    }

        //    var delegates = del.GetInvocationList();
        //    var saveInfos = new List<DelegateSaveInfo>();

        //    foreach (var singleDel in delegates)
        //    {
        //        var saveInfo = GetDelegateSaveInfo(singleDel);

        //        if (saveInfo != null)
        //        {
        //            saveInfos.Add(saveInfo);
        //        }
        //    }

        //    if (saveInfos.Count == 0)
        //    {
        //        return null;
        //    }

        //    return new InvocationList { Delegates = saveInfos };
        //}



        //public DelegateSaveInfo GetDelegateSaveInfo<T>(T del) where T : System.Delegate
        //{
        //    if (del == null)
        //    {
        //        return null;
        //    }


        //    if (__delegatSaveDataByMethodInfo.TryGetValue(del.Method, out var saveInfo))
        //    {
        //        return saveInfo;
        //    }
        //    else if (del.Method.IsGenericMethod)
        //    {
        //        var methodDef = del.Method.GetGenericMethodDefinition();

        //        if (__delegatSaveDataByMethodInfo.TryGetValue(methodDef, out saveInfo))
        //        {
        //            var typeSpecificCopy = saveInfo.Copy();

        //            _RegisterDelegate(del, typeSpecificCopy, ifGenericTreatAsMethodDef: false, isSaveInfoFromLoading: false);

        //            return typeSpecificCopy;
        //        }
        //        else
        //        {
        //            Debug.LogError($"A save info was reqested for a generic delegate but its generic method definition was not registered. " +
        //                $"This is not allowed. " +
        //                $"Please ensure you register your generic delegate def before trying to get infos for its closed definitions. " +
        //                $"Delegate name {del.Method.Name}, " +
        //                $"ObjectId: {Infra.Singleton.GetObjectId(del.Target)}. " +
        //                $"Going to return null.");
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogError($"A save info was reqested for a delegate that was not registered before. This is not allowed. " +
        //            $"Please ensure you register your delegates before trying to get their save infos. " +
        //            $"Delegate name {del.Method.Name}, " +
        //            $"ObjectId: {Infra.Singleton.GetObjectId(del.Target)}. " +
        //            $"Going to return null.");
        //        return null;
        //    }
        //}



        ////for loading
        //public T GetDelegate<T>(InvocationList invocationList)
        //{
        //    if (typeof(Delegate).IsAssignableFrom(typeof(T)) == false)
        //    {
        //        Debug.LogError($"The type {typeof(T).Name} is not a delegate type. " +
        //                        $"Please ensure you are trying to get a delegate of the correct type. " +
        //                        $"Going to return default value.");
        //        return default;
        //    }

        //    if (invocationList == null || invocationList.Delegates == null || invocationList.Delegates.Count == 0)
        //    {
        //        return default;
        //    }

        //    T del = default;

        //    foreach (var saveInfo in invocationList.Delegates)
        //    {
        //        var singleDel = GetDelegate<T>(saveInfo);

        //        if (singleDel != null)
        //        {
        //            del = (T)(object)(Delegate.Combine((Delegate)(object)del, (Delegate)(object)singleDel));
        //        }
        //    }

        //    return del;
        //}


        //public T GetDelegate<T>(DelegateSaveInfo saveInfo)
        //{
        //    if (saveInfo == null)
        //    {
        //        return default;
        //    }

        //    if (!typeof(Delegate).IsAssignableFrom(typeof(T)))
        //    {
        //        Debug.LogError($"The type {typeof(T).Name} is not a delegate type. " +
        //            $"Please ensure you are trying to get a delegate of the correct type. " +
        //            $"Going to return default value.");
        //        return default;
        //    }


        //    RandomId instanceId = saveInfo.TargeId;
        //    long methodId = saveInfo.MethodId;


        //    if (__delegateMapPerInstance.TryGetValue(instanceId, out var methodMap))
        //    {
        //        if (methodMap.TryGetValue(methodId, out var delegateVariants))
        //        {
        //            Delegate del;

        //            if (saveInfo.IsGeneric)
        //            {
        //                bool contains = delegateVariants.TryGetValue(saveInfo.GenericVairantId, out del);

        //                if (!contains)
        //                {
        //                    Type[] typeArgs = new Type[saveInfo.GenericTypeArguments.Count];

        //                    for (int i = 0; i < typeArgs.Length; i++)
        //                    {
        //                        string stringType = saveInfo.GenericTypeArguments[i];
        //                        Type type = Type.GetType(stringType);
        //                        typeArgs[i] = type;
        //                    }

        //                    //the methodid in case of generics should represent the method that was used to register this generic method
        //                    MethodInfo methodDef = delegateVariants[methodId].Method.GetGenericMethodDefinition();

        //                    MethodInfo concreteGeneric = methodDef.MakeGenericMethod(typeArgs);

        //                    bool isStatic = delegateVariants[methodId].Target == null;
        //                    var target = isStatic ? null : Infra.Singleton.GetObjectById<object>(instanceId);

        //                    del = concreteGeneric.CreateDelegate(typeof(T), target);

        //                    _RegisterDelegate(del, saveInfo, ifGenericTreatAsMethodDef: false, isSaveInfoFromLoading: true);
        //                }
        //            }
        //            else
        //            {
        //                if (delegateVariants.Count != 1)
        //                {
        //                    Debug.LogError($"Non-generic delegates should have only one 'variant' registered to them.");
        //                }

        //                del = delegateVariants[methodId];
        //            }


        //            if (del is T typedDel)
        //            {
        //                return typedDel;
        //            }
        //            else
        //            {
        //                Debug.LogError($"Delegate with methodId {methodId} for instance {instanceId} is not of type {typeof(T).Name}. " +
        //                    $"This means that the delegate is of a different type than expected.");
        //                return default;
        //            }
        //        }
        //        else
        //        {
        //            Debug.LogError($"No delegate found for instance {instanceId} with methodId {methodId}. " +
        //                $"This means that this methodId does not have a delegate registered for it. ");
        //            return default;
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogError($"No delegates found for instance {instanceId}. " +
        //            $"This means that this instance does not have any delegates registered for it. ");
        //        return default;
        //    }
        //}



        #endregion



        public Dictionary<Coroutine, CoroutineHandler> __coroutineSaveDataLookUp = new();
        public Dictionary<RandomId, HashSet<Coroutine>> __routinesByTargetMono = new();


        public void RegisterCoroutine(Coroutine routine, CoroutineHandler saveData)
        {
            if (__coroutineSaveDataLookUp.ContainsKey(routine))
            {
                Debug.LogError("Coroutine is already registered. Return");
                return;
            }
            if (!__routinesByTargetMono.ContainsKey(saveData._targetMonoId))
            {
                __routinesByTargetMono[saveData._targetMonoId] = new HashSet<Coroutine>();
            }


            __coroutineSaveDataLookUp[routine] = saveData;
            __routinesByTargetMono[saveData._targetMonoId].Add(routine);
        }

        public CoroutineHandler GetCoroutineHandler(Coroutine routine)
        {
            if (!__coroutineSaveDataLookUp.ContainsKey(routine))
            {
                Debug.LogError("Coroutine is not registered. Return default.");
                return null;
            }

            return __coroutineSaveDataLookUp[routine];
        }


        public HashSet<Coroutine> GetAllCoroutinesByMono(MonoBehaviour mono)
        {
            var id = _GetObjectIdWithoutReferencing(mono);

            if (id.IsDefault) return new();

            var routines = __routinesByTargetMono[id];

            return routines;
        }




        public Dictionary<Type, RandomId> __staticSubtituteIdsByType = new();
        //public Dictionary<RandomId, Type> __typesByStaticSubtituteId = new();


        public void RegisterStaticSubtitute<T>(T subtitute, RandomId id) where T : StaticSubtitute
        {
            Type type = subtitute.SubtitutedType;

            if (__staticSubtituteIdsByType.ContainsKey(type))
            {
                Debug.LogError($"Infra: Static subtitute of type {type} is already registered. Skipping registration.");
                return;
            }
            //if (__typesByStaticSubtituteId.ContainsKey(id))
            //{
            //    Debug.LogError($"Infra: Static subtitute id {id} is already registered. Skipping registration.");
            //    return;
            //}

            __staticSubtituteIdsByType[type] = id;
            //__typesByStaticSubtituteId[id] = type;
        }

        public RandomId GetStaticSubtituteId(Type type)
        {

            if (__staticSubtituteIdsByType.TryGetValue(type, out var id))
            {
                return id;
            }
            else
            {
                Debug.LogError($"Infra: Static subtitute of type {type} is not registered.");
                return RandomId.Default;
            }
        }





        public void CreateDefaultJsonSerializerSettings()
        {
            //todo: this searching method assumes there is no inheritance between converters
            var converters = AppDomain.CurrentDomain.GetUserAssemblies().SelectMany(asm => asm.GetTypes())
                .Where(t => !t.IsAbstract && typeof(JsonConverter).IsAssignableFrom(t))
                .Select(t => (JsonConverter)Activator.CreateInstance(t))
                .ToList();

            Debug.Log($"Found {converters.Count} JsonConverters in the assembly.");

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = converters,
                TypeNameHandling = TypeNameHandling.Auto,
                //WARNING: DO NOT FUCKING SET THIS. From 0.03 to 1.8s on first call, then worse and worse on subsequent calls.
                //if newtonsoft tries to read a field or property via reflection in a unityobject, unity might trigger events to them, forexample gpu readbacks
                //ContractResolver = new DefaultContractResolver
                //{
                //    DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                //}
            };
        }

        //public class CustomNewtonsoftContractResolver: DefaultContractResolver
        //{
        //    protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        //    {
        //        return base.GetSerializableMembers(objectType);
        //    }
        //}
    }


    public class StaticInfraSubtitute : StaticSubtitute<Infra> { }

    [SaveHandler(83297439587234832, nameof(StaticInfraSubtitute), typeof(StaticInfraSubtitute), isStatic: true)]
    public class StaticInfraSaveHandler : StaticSaveHandlerBase<StaticInfraSubtitute, StaticInfraSaveData>
    {

    }

    public class StaticInfraSaveData : StaticSaveDataBase
    {

    }
}
