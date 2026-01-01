
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
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


        public void StartNewWorld()
        {
            EnterWorld();

            MySceneManager.Singleton.StartNewWorld();
        }

        public void LoadSavedWorld(string filePath)
        {
            StartCoroutine(LoadSavedWorldRoutine(filePath));
        }

        public IEnumerator LoadSavedWorldRoutine(string filePath)
        {
            EnterWorld();

            yield return MySceneManager.Singleton.OnLoadSavedWorldRoutine();

            SaveAndLoadManager.Singleton.Load(filePath);
        }


        public void EnterWorld()
        {
            _objectIdsWhenEnteredWorld = Infra.Singleton.GetAllObjectIds();
        }


        public void ExitWorld()
        {
            var allObjectIdsNow = Infra.Singleton.GetAllObjectIds();

            var addedObjects = new HashSet<RandomId>(allObjectIdsNow.Except(_objectIdsWhenEnteredWorld));

            foreach(var id in addedObjects)
            {
                Infra.Singleton.Unregister(id); 
            }

            MySceneManager.Singleton.ExitWorld();


            //return;
            //var scenesToBeUnloaded = MySceneManager.Singleton.GetAffectedSceneByExitingWorld();

            //foreach (var scene in scenesToBeUnloaded)
            //{
            //    var infras = GOInfra.GetInfrasByScene(scene);
            //    //return;
            //    foreach (var infra in infras)
            //    {
            //        infra.Unregister();
            //    }
            //}

            //var unreferencedObjects = Infra.Singleton.GetUnreferencedObjects();

            //foreach (RandomId obj in unreferencedObjects)
            //{
            //    Infra.Singleton.Unregister(obj);
            //}

            //MySceneManager.Singleton.ExitWorld();
        }
    }
}
