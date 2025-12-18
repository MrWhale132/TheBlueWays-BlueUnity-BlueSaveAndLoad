
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.UtilScripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.InfraScripts
{
    [DefaultExecutionOrder(-100000)]
    [ExecuteInEditMode]
    public class SceneInfra : MonoBehaviour
    {
        public List<GOInfra> scenePlacedGOInfras_EditorView = new();
        public HashSet<GOInfra> ScenePlacedGOInfras { get; set; } = new();

        public bool IsObjectLoading => SaveAndLoadManager.IsObjectLoading(this);



#if UNITY_EDITOR
        public bool _collectScenePlacedGOInfras;

        public void CollectInfras()
        {
            if (_collectScenePlacedGOInfras)
            {
                _collectScenePlacedGOInfras = false;
                ScenePlacedGOInfras.Clear();
                var allInfras = FindObjectsByType<GOInfra>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var infra in allInfras)
                {
                    ScenePlacedGOInfras.Add(infra);
                }
            }
        }
#endif



        private void OnValidate()
        {
#if UNITY_EDITOR
            CollectInfras();
#endif
        }


        private void Awake()
        {
            if (!Application.isPlaying) return;

            ScenePlacedGOInfras = scenePlacedGOInfras_EditorView.ToHashSet();
        }


        private void Start()
        {
            if (!Application.isPlaying) return;
            
            if (IsObjectLoading)
            {
                Infra.SceneManagement.SceneInfrasBySceneHandle.Add(gameObject.scene.handle, this);
                return;
            }


            var results = new List<GraphWalkingResult>();

            foreach (var infra in ScenePlacedGOInfras)
            {
                if(infra == null)
                {
                    Debug.LogError("Null GOInfra found in SceneInfra's _scenePlacedGOInfras list. Please fix the references. " +
                        "You might forgot to refresh the list after you removed GOInfra components from the scene.");
                    continue;
                }
                if (infra.DescribeSceneObject(out var result))
                {
                    results.Add(result);
                }
            }

            SaveAndLoadManager.ScenePlacedObjectRegistry.Register(this, results);
        }


        private void OnDestroy()
        {
            if (!Application.isPlaying) return;

            Infra.SceneManagement.SceneInfrasBySceneHandle.Remove(gameObject.scene.handle);
        }


        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                var didDomainReload = ScenePlacedGOInfras is null or { Count: 0 };

                if (didDomainReload)
                {
                    ScenePlacedGOInfras = scenePlacedGOInfras_EditorView.ToHashSet();
                }

                return;
            }
        }


        private void Update()
        {
            if (!Application.isPlaying)
            {
                ScenePlacedGOInfras.RemoveWhere(infra => infra == null);
                
                scenePlacedGOInfras_EditorView.Clear();
                scenePlacedGOInfras_EditorView.AddRange(ScenePlacedGOInfras);
                return;
            }
        }


        public List<ObjectMemberGraphWalker.MemberCollectionResult> CollectScenePlacedObjects()
        {
            var results = new List<ObjectMemberGraphWalker.MemberCollectionResult>();

            foreach (var infra in ScenePlacedGOInfras)
            {
                if (infra == null)
                {
                    Debug.LogError("Null GOInfra found in SceneInfra's _scenePlacedGOInfras list. Please fix the references. " +
                        "You might forgot to refresh the list after you removed GOInfra components from the scene.");
                    continue;
                }

                if (!infra.HasSceneParts) continue;

                var result = infra.CollectSceneParts();

                results.Add(result);
            }

            return results;
        }
    }
}
