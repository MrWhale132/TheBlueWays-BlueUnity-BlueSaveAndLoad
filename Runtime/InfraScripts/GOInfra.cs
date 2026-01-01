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
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using NUnit.Framework.Internal;


#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Assets._Project.Scripts.Infrastructure
{
    [DisallowMultipleComponent]
    public class GOInfra : MonoBehaviour
    {
        //not used currently
        public static HashSet<GOInfra> _allInfras = new HashSet<GOInfra>();


        [field: SerializeField]
        public RandomId ObjectId { get; set; }//id for this component

        public RandomIdReference PrefabAssetId;
        //design decision: why two seperate descriptors for prefab and scene-placed instances?
        //Why not just one descriptor if these descriptors are not tied to specific workflows?
        //Answer: because when a prefab is dragged into a scene, you may not want to save it the same way you would a prefab instance, vica-versa.
        public ObjectDescriptor PrefabDescriptor;
        public ObjectDescriptor ScenePlacedDescriptor;


        public bool IsPrefabRoot => PrefabAssetId != null;
        public bool HasPrefabParts => PrefabDescriptor != null;
        public bool HasSceneParts => ScenePlacedDescriptor != null;


        public bool IsObjectLoading => SaveAndLoadManager.IsObjectLoading(this);


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




        public void Awake()
        {
            if (Infra.Singleton == null) return;


            bool isManaged = HasPrefabParts || HasSceneParts;

            if (IsPrefabRoot)
            {
                var childrenAndSelf = GetChildrenInfra(includeSelf: true);

                foreach (var child in childrenAndSelf)
                {
                    child.ApplyAssetNameAliases();
                }
            }
            else if (!isManaged)
            {
                   ApplyAssetNameAliases();
            }



            if (IsObjectLoading) return;



            bool registered = Infra.Singleton.IsRegistered(gameObject);

            if (!registered)
            {
                _allInfras.Add(this);


                if (IsPrefabRoot)
                {
                    var childrenAndSelf = GetChildrenInfra(includeSelf: true);

                    foreach (var child in childrenAndSelf)
                    {
                        child.ApplyAssetNameAliases();
                    }


                    if (PrefabDescriptor == null)
                    {
                        Debug.LogError($"GOInfra on GameObject '{gameObject.HierarchyPath()}' has a PrefabAssetId assigned but no PrefabDescriptor. A PrefabDescriptor is required if a PrefabAssetId is assigned.", gameObject);
                    }


                    var results = new List<GraphWalkingResult>();

                    foreach (var child in childrenAndSelf)
                    {
                        _allInfras.Add(child);

                        if (child.DescribePrefab(out var result))
                        {
                            results.Add(result);
                        }
                    }

                    SaveAndLoadManager.PrefabDescriptionRegistry.Register(this, results);
                }




                if (!isManaged)
                {
                    ApplyAssetNameAliases();


                    Infra.Singleton.Register(gameObject, rootObject: true, createSaveHandler: true);

                    List<Component> components = new List<Component>();

                    gameObject.GetComponents(typeof(Component), components);


                    for (int i = 0; i < components.Count; i++)
                    {
                        var comp = components[i];

                        registered = Infra.Singleton.IsRegistered(comp);

                        if (!registered)
                            Infra.Singleton.Register(comp, rootObject: true, createSaveHandler: true);
                    }
                }
            }
        }


        //note: on sceneplaced or prefab part gameobject that are disabled from the start, Awake and Start wont be called
        //and thus, the actions inside them may not be executed
        private void Start()
        {
            if (IsObjectLoading) return;

            if (Infra.Singleton == null) return;

            ObjectId = Infra.Singleton.GetObjectId(this, Infra.Singleton.GlobalReferencing);
        }







        public void OnDestroy()
        {
            Unregister();
        }

        [ReadOnly]
        public bool _isUnregistered = false;


        public void Unregister()
        {
            if (_isUnregistered) return;


            //TODO: remove after SaveAndLoad system is fully implemented
            if (IsObjectLoading) return;

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

            _allInfras.Remove(this);
            _isUnregistered = true;
        }



        public static void AddToAllInfras(GOInfra infra)
        {
            if (_allInfras.Contains(infra)) return;

            _allInfras.Add(infra);
        }


        public static HashSet<GOInfra> GetInfrasByScene(Scene scene)
        {
            var result = new HashSet<GOInfra>();

            foreach (var infra in _allInfras)
            {
                if (infra == null)
                {
                    continue;
                }

                if (infra.gameObject.scene == scene)
                {
                    result.Add(infra);
                }
            }

            return result;
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



        public List<AssetNameAlias> _assetNameAliases;


        public void ApplyAssetNameAliases()
        {
            if (_assetNameAliases is null or { Count: 0 })
            {
                return;
            }
            //return;

            foreach (var alias in _assetNameAliases)
            {
                if (alias.assetHolder is MeshFilter meshFilter)
                {
                    Apply(alias, meshFilter.mesh);
                    Apply(alias, meshFilter.sharedMesh);
                }
            }

            void Apply(AssetNameAlias alias, UnityEngine.Object asset)
            {
                bool isValid = alias.assetHolder != null && alias.assetId.IsNotDefault && asset != null;

                if (!isValid) return;

                string nameAlias = AddressableDb.Singleton.GetAssetNameById(alias.assetId);

                if (string.IsNullOrEmpty(nameAlias))
                {
                    return;
                }

                asset.name = nameAlias;
            }
        }




        private void OnValidate()
        {
#if UNITY_EDITOR

            if (_assetNameAliases != null && _assetNameAliases.Count > 0)
            {
                foreach (var alias in _assetNameAliases)
                {
                    if (alias.assetHolder != null)
                    {
                        if (alias.assetHolder is MeshFilter meshFilter)
                        {
                            //Validate(alias, meshFilter.sharedMesh);
                        }
                    }
                }


                void Validate(AssetNameAlias alias, Object asset)
                {
                    string compTypeName = alias.assetHolder.GetType().Name;
                    string assetTypeName = asset.name;

                    if (asset == null)
                    {
                        Debug.LogError($"Applying AssetNameAlias failed: {compTypeName} on GameObject " +
                        $"{gameObject.HierarchyPath()} has no assigned {assetTypeName}.", gameObject);
                        return;
                    }

                    //convenience: try to auto-assign assetId if not assigned
                    if (alias.assetId.IsDefault)
                    {
                        var assetId = AddressableDb.Singleton.GetAssetIdByAssetName(asset);

                        if (!assetId.IsDefault)
                        {
                            alias.assetId = assetId;
                        }
                        else
                        {
                            Debug.LogError($"Applying AssetNameAlias failed: Could not find AssetId for {assetTypeName} " +
                                    $"{asset.name} assigned to {compTypeName} on GameObject " +
                                    $"{gameObject.HierarchyPath()}.", gameObject);
                            return;
                        }
                    }
                    else //if assigned, check if the asset this id is pointing to is the same as this asset
                    {
                        string expectedAssetnName = AddressableDb.Singleton.GetTypedNameById(alias.assetId);

                        string actualName = AddressableDb.Singleton.GetExtendedAssetName(asset);

                        if (expectedAssetnName != actualName)
                        {
                            Debug.LogError($"Applying AssetNameAlias failed: AssetId '{alias.assetId}' " +
                                $"points to asset '{expectedAssetnName}' but {compTypeName} on GameObject " +
                                $"{gameObject.HierarchyPath()} has assigned {assetTypeName} '{actualName}'.",
                                gameObject);
                            return;
                        }
                    }
                }
            }


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


        private void Reset()
        {
            var sceneInfraGO = gameObject.scene.GetRootGameObjects().FirstOrDefault(go => go.GetComponent<SceneInfra>() != null);

            if (sceneInfraGO != null)
            {
                var sceneInfra = sceneInfraGO.GetComponent<SceneInfra>();

                sceneInfra.ScenePlacedGOInfras.Add(this);
            }
        }


        [HideInInspector]
        public List<GOInfra> _children = new();

        public List<GOInfra> GetChildrenInfra(bool includeSelf = false)
        {
            if (includeSelf)
            {
                _children.Clear();
                gameObject.GetComponentsInChildren<GOInfra>(includeInactive: true, _children);
                return _children;
            }
            else
            {
                return gameObject.GetComponentsInChildrenExcludeSelf<GOInfra>(includeInactive: true).ToList();
            }
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
            public override void Init(object instance, InitContext context)
            {
                base.Init(instance, context);

                __saveData.ObjectId = __instance.ObjectId;
            }

            public override void _AssignInstance()
            {
                var go = Infra.Singleton.GetObjectById<GameObject>(__saveData.GameObjectId);

                if (!go.TryGetComponent<GOInfra>(out __instance))
                {
                    __instance = go.AddComponent<GOInfra>();
                }
            }

            public override void LoadValues()
            {
                base.LoadValues();

                __instance.ObjectId = __saveData.ObjectId;
            }
        }
    }


    public class GOInfraSaveData : MonoSaveDataBase
    {
        public RandomId ObjectId;
        public bool IsNetworked;
        public bool IsUIElement;
    }
}