using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.Extensions;
using Packages.com.blueutils.core.Runtime.Misc;
using System.Collections.Generic;
using System.Linq;
using Theblueway.SaveAndLoad;
using Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.InfraScripts;
using Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.UtilsScripts.Extensions;
using UnityEditor;
using UnityEngine;


namespace Assets._Project.Scripts.Infrastructure
{
    [DisallowMultipleComponent]
    public class GOInfra : MonoBehaviour
    {
        [field: SerializeField]
        public RandomId ObjectId { get; set; }//id for this component

        public RandomIdReference PrefabAssetId;
        //design decision: why two seperate descriptors for prefab and scene-placed instances?
        //Why not just one descriptor if these descriptors are not tied to specific workflows?
        //Answer: because when a prefab is dragged into a scene, you may not want to save it the same way you would a prefab instance, vica-versa.
        public ObjectDescriptor PrefabDescriptor;
        public ObjectDescriptor ScenePlacedDescriptor;


        public bool HasPrefabParts => PrefabDescriptor != null;
        public bool HasSceneParts => ScenePlacedDescriptor != null;


        #region OldPrefabWorkflow
        //        [field: SerializeField, ReadOnly]
        //        public bool IsPrefabRoot { get; set; }
        //        [field: SerializeField, ReadOnly]
        //        public bool IsPartOfPrefab { get; set; }
        //        [field: SerializeField, ReadOnly]
        //        public bool IsScenePlaced { get; set; }
        //        [field: SerializeField]
        //        public RandomId PrefabRootInstanceId { get; set; }
        //        [field: SerializeField]
        //        public RandomId PrefabPartId { get; set; }
        //        //the logic it self will may be useful for later for something else
        //        public void OldPrefabWorkflow()
        //        {
        //#if UNITY_EDITOR
        //            if (!Application.isPlaying)
        //            {
        //                bool IsInPrefabView(out GameObject root)
        //                {
        //                    root = gameObject;

        //                    while (root.transform.parent != null) root = root.transform.parent.gameObject;

        //                    //like Canvas
        //                    if (root.hideFlags.HasFlag(HideFlags.NotEditable) && root.hideFlags.HasFlag(HideFlags.DontSave))
        //                    {
        //                        root = root.transform.GetChild(0).gameObject;
        //                    }

        //                    if (root.scene.name == root.name)
        //                    {
        //                        return true;
        //                    }
        //                    return false;
        //                }

        //                bool isBeingImported = string.IsNullOrEmpty(gameObject.scene.name);

        //                if (isBeingImported)
        //                {
        //                    IsPartOfPrefab = true;
        //                    IsScenePlaced = false;
        //                    IsPrefabRoot = gameObject.transform.parent == null;

        //                    if (PrefabPartId.IsDefault)
        //                        PrefabPartId = RandomId.Get();
        //                }
        //                else if (IsInPrefabView(out var root))
        //                {
        //                    IsPartOfPrefab = true;
        //                    IsScenePlaced = false;

        //                    bool wasPrefabRoot = IsPrefabRoot;
        //                    IsPrefabRoot = root == gameObject;

        //                    if (PrefabPartId.IsDefault)
        //                        PrefabPartId = RandomId.Get();

        //                    else if (wasPrefabRoot && !IsPrefabRoot)//when you drag a prefab under another prefab
        //                    {
        //                        PrefabPartId = RandomId.Get();

        //                        void Traverse(GOInfra parent)
        //                        {
        //                            var _children = parent.GetChildrenInfra();

        //                            foreach (var child in _children)
        //                            {
        //                                child.PrefabPartId = RandomId.Get();
        //                                Traverse(child);
        //                            }
        //                        }

        //                        Traverse(this);
        //                    }
        //                }
        //                else
        //                {
        //                    IsPrefabRoot = false;
        //                    IsPartOfPrefab = false;
        //                    IsScenePlaced = true;
        //                }
        //            }
        //#endif
        //        }
        #endregion



        private void OnValidate()
        {
#if UNITY_EDITOR



            //todo: if prefabassetid just assigned, clear child prefabassetids
            bool prefabRootCandidate = PrefabAssetId != null;

            if (prefabRootCandidate)
            {
                var parent = gameObject.transform.parent;

                while (parent != null)
                {
                    if (parent.TryGetComponent<GOInfra>(out var parentInfra))
                    {
                        if (parentInfra.PrefabAssetId != null)
                        {
                            PrefabAssetId = null;
                            break;
                        }
                    }

                    parent = parent.parent;
                }
            }
#endif
        }




        public bool DescribeSceneObject(out GraphWalkingResult result)
        {
            if (ScenePlacedDescriptor == null)
            {
                result = null;
                return false;
            }

            var walker = new ObjectMemberGraphWalker(ScenePlacedDescriptor);

            var saveHandlerInitContext = new InitContext { isScenePlaced = true };

            result = walker.Walk(gameObject, saveHandlerInitContext);

            return true;
        }


        public bool DescribePrefab(out GraphWalkingResult result)
        {
            if (PrefabDescriptor == null)
            {
                result = null;
                return false;
            }

            var walker = new ObjectMemberGraphWalker(PrefabDescriptor);

            var saveHandlerInitContext = new InitContext { isPrefabPart = true };

            result = walker.Walk(gameObject, saveHandlerInitContext);

            return true;
        }


        public void Awake()
        {
            if (SaveAndLoadManager.IsObjectLoading(gameObject)) return;

            if (Infra.Singleton == null) return;


            bool registered = Infra.Singleton.IsRegistered(gameObject);

            if (!registered)
            {
                if (PrefabAssetId != null)
                {
                    if (PrefabDescriptor == null)
                    {
                        Debug.LogError($"GOInfra on GameObject '{gameObject.HierarchyPath()}' has a PrefabAssetId assigned but no PrefabDescriptor. A PrefabDescriptor is required if a PrefabAssetId is assigned.", gameObject);
                    }

                    var results = new List<GraphWalkingResult>();

                    DescribePrefab(out var result);

                    results.Add(result);

                    var _children = GetChildrenInfra();

                    foreach (var child in _children)
                    {
                        if (child.DescribePrefab(out result))
                        {
                            results.Add(result);
                        }
                    }

                    SaveAndLoadManager.PrefabDescriptionRegistry.Register(this, results);
                }
                else
                    Infra.Singleton.Register(gameObject, rootObject: true, createSaveHandler: true);
            }

            if (HasPrefabParts || HasSceneParts) return;


            List<Component> components = new List<Component>();

            gameObject.GetComponents(typeof(Component), components);



            for (int i = 0; i < components.Count; i++)
            {
                var comp = components[i];

                if (!registered)
                    Infra.Singleton.Register(comp, rootObject: true, createSaveHandler: true);
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




        public List<ObjectMemberGraphWalker.MemberCollectionResult> CollectPrefabParts()
        {
            var walker = new ObjectMemberGraphWalker();
            var results = new List<ObjectMemberGraphWalker.MemberCollectionResult>();

            var result = walker.CollectMembersV2(gameObject, PrefabDescriptor);

            results.Add(result);


            var children = GetChildrenInfra();

            foreach (var child in children)
            {
                if (!child.HasPrefabParts) continue;

                result = walker.CollectMembersV2(child.gameObject, child.PrefabDescriptor);

                results.Add(result);
            }
            return results;
        }



        public ObjectMemberGraphWalker.MemberCollectionResult CollectSceneParts()
        {
            var walker = new ObjectMemberGraphWalker();

            var result = walker.CollectMembersV2(gameObject, ScenePlacedDescriptor);

            ///here we dont need to iterate children because <see cref="SceneInfra"/> call each inidividual GOInfra

            return result;
        }





        private void Reset()
        {
            var sceneInfraGO = gameObject.scene.GetRootGameObjects().FirstOrDefault(go => go.GetComponent<SceneInfra>() != null);

            if (sceneInfraGO != null)
            {
                var sceneInfra = sceneInfraGO.GetComponent<SceneInfra>();

                sceneInfra.ScenePlacedGOInfras.Add(this);
            }
        }


        //public List<GOInfra> _children = new();

        public List<GOInfra> GetChildrenInfra()
        {
            return gameObject.GetComponentsInChildrenExcludeSelf<GOInfra>(includeInactive: true).ToList();
        }



#if UNITY_EDITOR
        //editor only
        public void AddInfraToAllChildren()
        {
            foreach (Transform child in transform)
            {
                var childInfra = child.GetComponent<GOInfra>();
                if (childInfra == null)
                {
                    childInfra = child.gameObject.AddComponent<GOInfra>();
                }

                childInfra.AddInfraToAllChildren();
            }

            OnValidate();
        }


        public void RemoveInfraFromAllChildren()
        {
            var children = gameObject.GetComponentsInChildrenExcludeSelf<GOInfra>(includeInactive: true);

            foreach (GOInfra child in children)
            {
                DestroyImmediate(child);
            }
        }
#endif

        [ReadOnly]
        public List<Component> _cachedComponents = new();

#if UNITY_EDITOR


        public void CacheComponentsInChildrenAndSelf()
        {
            GetComponents(_cachedComponents);

            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<GOInfra>(out var childInfra))
                {
                    childInfra.CacheComponentsInChildrenAndSelf();
                }
            }
        }
#endif

#if UNITY_EDITOR
        //tells if we have already hooked-up to the SceneView delegate, once, some time ago
        [System.NonSerialized]
        bool linked_SV = false;

        public void OnDrawGizmos()
        {
            if (linked_SV == false)
            {
                linked_SV = true;
                SceneView.duringSceneGui -= OnSceneDraw;
                SceneView.duringSceneGui += OnSceneDraw;
            }
        }


        void OnSceneDraw(SceneView sceneView)
        {

            try
            {
                if (gameObject) { };
            }
            catch
            {
                SceneView.duringSceneGui -= OnSceneDraw;
                //Debug.Log("on destroy");
                //custom logic here, but no unity api calls on 'this'

                return;
            }
        }
#endif




        [SaveHandler(id: 67685676547, dataGroupName: nameof(GOInfra), typeof(GOInfra))]
        public class GOInfraSaveHandler : MonoSaveHandler<GOInfra, GOInfraSaveData>
        {
            public GOInfraSaveHandler() { }


            public override void LoadValues()
            {
                base.LoadValues();

                __instance.ObjectId = __saveData._ObjectId_;
            }
        }
    }


    public class GOInfraSaveData : MonoSaveDataBase
    {
        public bool IsNetworked;
        public bool IsUIElement;
    }
}