
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Packages.com.theblueway.saveandload.Samples
{
    public class MyGameManager:MonoBehaviour
    {
        public static MyGameManager Singleton { get; private set;  }


        public HashSet<RandomId> _objectIdsWhenEnteredWorld;


        private void Awake()
        {
            if (Singleton != null && Singleton != this)
            {
                Debug.LogError("Multiple instances of MyGameManager detected. Destroying duplicate instance.");
                Destroy(gameObject);
                return;
            }

            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }


        public void OnBootstrapCompleted() { StartCoroutine(OnBootstrapCompletedRoutine()); }

        public IEnumerator OnBootstrapCompletedRoutine()
        {
            yield return MySceneManager.Singleton.OnBootstrapCompletedRoutine();

        }


        public void StartNewWorld()
        {
            StartCoroutine(StartNewWorldRoutine());
        }

        public IEnumerator StartNewWorldRoutine()
        {
            BeforeEnterWorld();

            yield return MySceneManager.Singleton.StartNewWorldRoutine();

            AfterEnterWorld();
        }




        public void LoadSavedWorld(string filePath)
        {
            StartCoroutine(LoadSavedWorldRoutine(filePath));
        }

        public IEnumerator LoadSavedWorldRoutine(string filePath)
        {
            BeforeEnterWorld();

            yield return MySceneManager.Singleton.PrepareLoadSavedWorldRoutine();

            yield return SaveAndLoadManager.Singleton.LoadRoutine(filePath);

            yield return MySceneManager.Singleton.OnLoadSavedWorldCompletedRoutine();

            AfterEnterWorld();
        }


        public void BeforeEnterWorld()
        {
            _objectIdsWhenEnteredWorld = Infra.Singleton.GetAllObjectIds();
        }

        public void AfterEnterWorld()
        {
            AddressableDb.Singleton.RegisterUnityBuiltinResources();
        }



        public void ExitWorld()
        {

            var allObjectIdsNow = Infra.Singleton.GetAllObjectIds();

            var addedObjects = new HashSet<RandomId>(allObjectIdsNow.Except(_objectIdsWhenEnteredWorld));
            
            foreach(var id in addedObjects)
            {
                Infra.Singleton.Unregister(id);
                AssetIdMap.ObjectIdToAssetId.Remove(id);
            }

            MySceneManager.Singleton.ExitWorld();
        }
    }
}
