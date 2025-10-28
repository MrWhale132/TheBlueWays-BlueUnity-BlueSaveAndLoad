
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases
{
    //todo: optimize
    public class ComponentAddingTracker
    {
        //RequireComponent, .AddComponent in Awake, Start, etc
        public static Dictionary<Type, bool> _mayAddAdditionalComponents = new();


        public GameObject __gameobject;
        public GameObject GameObject {
            get => __gameobject;
            set { if (__gameobject != null) { Debug.LogError("This component tracker is already bound to a gameobject"); return; } __gameobject = value; }
        }


        public Dictionary<Type, List<Component>> _unboundComponentsByIndexPerType = new();


        public T AddComponent<T>() where T : Component
        {
            Type type = typeof(T);

            if(typeof(T) == typeof(UnityEngine.ParticleSystem) || typeof(T) == typeof(UnityEngine.ParticleSystemRenderer))
            {
                
            }
            if (HasUnboundComponent<T>(out var comp))
            {
                return comp;
            }

            if (!_mayAddAdditionalComponents.ContainsKey(typeof(T)))
            {
                bool hasRequireComponent = typeof(T).IsDefined(typeof(RequireComponent), true);
                _mayAddAdditionalComponents[typeof(T)] = hasRequireComponent;
            }


            ///WARNING: even though the instances added here because of the RequireComponent are NOT HANDLED here
            ///the <see cref="SaveAndLoadManager.IsObjectLoading(Component)"/> apis will still work because
            ///they check the comp's gameobject's loading status instead
            if (_mayAddAdditionalComponents[typeof(T)])
            {
                int countBefore = GameObject.GetComponentCount();

                comp = GameObject.AddComponent<T>();

                int countAfter = GameObject.GetComponentCount();

                if(comp == null)
                {

                }

                
                for (int i = countBefore; i < countAfter; i++)
                {
                    var c = GameObject.GetComponentAtIndex(i);

                    if (c == comp) continue;

                    Type ctype = c.GetType();
                    if (!_unboundComponentsByIndexPerType.ContainsKey(ctype))
                    {
                        _unboundComponentsByIndexPerType[ctype] = new List<Component>();
                    }
                    _unboundComponentsByIndexPerType[ctype].Add(c);
                }

                return comp;
            }
            else
            {
                return GameObject.AddComponent<T>();
            }
        }
        public bool HasUnboundComponent<T>(out T comp) where T : Component
        {
            Type type = typeof(T);

            if (_unboundComponentsByIndexPerType.TryGetValue(type, out var byIndexes))
            {
                var comp2 = byIndexes[0];
                comp = comp2 as T;
                if (comp != null)
                {
                    byIndexes.RemoveAt(0);
                    if (byIndexes.Count == 0)
                        _unboundComponentsByIndexPerType.Remove(type);
                    return true;
                }
                else
                {
                    Debug.LogError("Something bad happend. This should not have happened. Type mismatch in unbound components");
                    return false;
                }
            }
            comp = null;
            return false;
        }
    }


    [SaveHandler(id: 432468912, dataGroupName: nameof(GameObject), handledType: typeof(GameObject), order: -10)]
    public class GameObjectSaveHandler : SaveHandlerGenericBase<GameObject, GameObjectSaveData>
    {
        List<Component> _components = new List<Component>();
        public GOInfra _goInfra;

        public ComponentAddingTracker ComponentAddingTracker { get; set; }



        public T AddComponent<T>() where T : Component
        {
            if (ComponentAddingTracker == null)
            {
                Debug.LogError("ComponentAddingTracker is null. Cannot add component via tracker");
                return null;
            }

            return ComponentAddingTracker.AddComponent<T>();
        }



        public override void Init(object instance)
        {
            base.Init(instance);


            __saveData.GameObjectId = __saveData._ObjectId_;
            __saveData.IsPrefab = __instance.IsProbablyPrefab();

            if (!__saveData.IsPrefab)
            {
                _goInfra = __instance.GetOrAddComponent<GOInfra>();

                __saveData.IsNetworked = _goInfra.IsNetworked;
                __saveData.IsUIElement = _goInfra.IsUIElement;
            }

            //easier troubleshooting if this is set now so it can be used later
            __saveData.HierarchyPath = __instance.HierarchyPath();
        }


        //public override void _GetObjectId()
        //{
        //        HandledObjectId = Infra.Singleton.GetObjectId(__instance, Infra.Singleton.GlobalReferencing);
        //    //if (__instance.IsProbablyPrefab())
        //    //{
        //    //}
        //    //else
        //    //    base._GetObjectId();
        //}


        public override void ReleaseObject()
        {
            base.ReleaseObject();

            __instance = null;
            _goInfra = null;
        }



        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.GameObjectName = __instance.name;
            __saveData.tag = __instance.tag;
            __saveData.layer = __instance.layer;
            __saveData.activeSelf = __instance.activeSelf;
            __saveData.IsStatic = __instance.isStatic;

            __saveData._MetaData_.Order = Order;

            _components.Clear();

            __instance.GetComponents(typeof(Component), _components);

            for (int i = 0; i < _components.Count; i++)
            {
                var comp = _components[i];

                var compId = Infra.Singleton.GetObjectId(comp, HandledObjectId);

                __saveData.Components.Add(compId);
            }
        }




        public override void CreateObject()
        {
            base.CreateObject();


            if (__saveData.IsPrefab)
            {
                __instance = AddressableDb.Singleton.GetAssetById<GameObject>(__saveData.GameObjectId);


                _components.Clear();

                __instance.GetComponents(typeof(Component), _components);

                for (int i = 0; i < _components.Count; i++)
                {
                    var comp = _components[i];

                    var compId = __saveData.Components[i];

                    Infra.Singleton.RegisterReference(comp, compId, rootObject: true);
                }
            }
            else
                __instance = new GameObject();


            ComponentAddingTracker = new ComponentAddingTracker() { GameObject = __instance };


            HandledObjectId = __saveData.GameObjectId;

            if (HandledObjectId.IsDefault)
            {
                Debug.LogWarning(__saveData.GameObjectName);
            }

            Infra.Singleton.RegisterReference(__instance, __saveData.GameObjectId);


            //for easier troubleshoot
            __instance.name = __saveData.GameObjectName;


        }

        public override void LoadValues()
        {
            if (__saveData.IsPrefab) return;

            __instance.name = __saveData.GameObjectName;
            __instance.tag = __saveData.tag;
            __instance.layer = __saveData.layer;
            __instance.isStatic = __saveData.IsStatic;
            __instance.SetActive(__saveData.activeSelf);

        }
    }

    public class GameObjectSaveData : SaveDataBase
    {
        public RandomId GameObjectId;
        public string GameObjectName;
        public string HierarchyPath;
        public bool IsPrefab;
        public bool IsNetworked;
        public bool IsUIElement;
        public List<RandomId> Components = new();
        public string tag;
        public int layer;
        public bool activeSelf;
        public bool IsStatic;
    }
}
