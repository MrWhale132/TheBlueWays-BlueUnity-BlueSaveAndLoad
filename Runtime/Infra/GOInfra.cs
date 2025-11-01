using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.Extensions;
using System;
using System.Collections.Generic;
//using Unity.Netcode;
using UnityEngine;

namespace Assets._Project.Scripts.Infrastructure
{
    [DisallowMultipleComponent]
    public class GOInfra : MonoBehaviour
    {
        public RandomId ObjectId { get; set; }//id for this component

        //any of these are unused currently
        //[field: SerializeField]
        public bool IsNetworked { get; set; }

        //[field: SerializeField]
        public bool IsUIElement { get; set; }
        [HideInInspector]
        public bool prefab;

        //[Obsolete]
        //public GameObjectSaveHandler __goSaveHandler;

        public Dictionary<Component, RandomId> __compoenentObjectIds;


        public void Awake()
        {
            if (SaveAndLoadManager.IsObjectLoading(gameObject)) return;

            if (Infra.Singleton == null) return;


            if (gameObject.IsProbablyPrefab())
            {
                prefab = true;
                return;
                //Debug.LogWarning(gameObject.HierarchyPath() + " is a prefab");
            }
            
            bool registered = Infra.Singleton.IsRegistered(gameObject);

            if (!registered)
                Infra.Singleton.Register(gameObject, rootObject: true, createSaveHandler: true);


            List<Component> components = new List<Component>();

            gameObject.GetComponents(typeof(Component), components);

            IsUIElement = components[0].GetType() == typeof(RectTransform);


            for (int i = 0; i < components.Count; i++)
            {
                var comp = components[i];

                if (!registered)
                    Infra.Singleton.Register(comp, rootObject: true, createSaveHandler: true);

                //once set true it stays true
                IsNetworked = IsNetworked || comp.GetType().Name == "NetworkObject";
            }

            ObjectId = Infra.Singleton.GetObjectId(this, Infra.Singleton.GlobalReferencing);
        }


        public void OnDestroy()
        {
            //TODO: remove after SaveAndLoad system is fully implemented
            if (SaveAndLoadManager.IsObjectLoading(gameObject)) return;

            if (Infra.Singleton == null) return;


            List<Component> components = new List<Component>();

            gameObject.GetComponents(typeof(Component), components);

            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];

                Infra.Singleton.Unregister(component);
            }
            //SaveAndLoadManager.Singleton.RemoveSaveHandler(__goSaveHandler);

            Infra.Singleton.Unregister(gameObject);
        }



        public T AddComponent<T>() where T : Component
        {
            var component = gameObject.AddComponent<T>();

            Infra.Singleton.Register(component, rootObject: true, createSaveHandler: true);

            return component;
        }




        //editor only
        public void AddInfraToAllChild()
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<GOInfra>() == null)
                {
                    child.gameObject.AddComponent<GOInfra>();
                }
                child.GetComponent<GOInfra>().AddInfraToAllChild();
            }
        }


        [SaveHandler(id: 67685676547, dataGroupName: nameof(GOInfra), typeof(GOInfra))]
        public class GOInfraSaveHandler : MonoSaveHandler<GOInfra, GOInfraSaveData>
        {
            public GOInfraSaveHandler() { }


            public override void Init(object instance)
            {
                base.Init(instance);

                __saveData.IsNetworked = __instance.IsNetworked;
                __saveData.IsUIElement = __instance.IsUIElement;
            }

            public override void LoadValues()
            {
                base.LoadValues();

                __instance.ObjectId = __saveData._ObjectId_;
                __instance.IsNetworked = __saveData.IsNetworked;
                __instance.IsUIElement = __saveData.IsUIElement;
            }
        }
    }


    public class GOInfraSaveData : MonoSaveDataBase
    {
        public bool IsNetworked;
        public bool IsUIElement;
    }
}