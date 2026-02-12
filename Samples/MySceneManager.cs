
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.Extensions;
using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Packages.com.theblueway.saveandload.Samples
{
    public class MySceneManager : MonoBehaviour
    {
        public static MySceneManager Singleton { get; private set; }


        public SceneReference _transition;
        public SceneReference _mainMenu;
        public SceneReference _worldScene;
        public SceneReference _bootstrapScene;


        private void Awake()
        {
            if (Singleton != null && Singleton != this)
            {
                Debug.LogError("Multiple instances of MySceneManager detected. Destroying duplicate instance.");
                Destroy(gameObject);
                return;
            }

            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }






        public void StartNewWorld()
        {
            StartCoroutine(StartNewWorldRoutine());
        }

        public IEnumerator StartNewWorldRoutine()
        {
            yield return Transition(_mainMenu, _worldScene);
        }


        public void ExitWorld()
        {
            StartCoroutine(ExitWorldRoutine());
        }

        public IEnumerator ExitWorldRoutine()
        {
            yield return Transition(_worldScene, _mainMenu);
        }


        public void OnBootstrapCompleted()
        {
            StartCoroutine(OnBootstrapCompletedRoutine());
        }

        public IEnumerator OnBootstrapCompletedRoutine()
        {
            yield return Transition(_bootstrapScene, _mainMenu);
        }


        public IEnumerator PrepareLoadSavedWorldRoutine()
        {
            yield return StartCoroutine(LoadScene(_transition.BuildIndex));

            yield return StartCoroutine(UnloadScene(_mainMenu.LoadedScene));
        }




        public IEnumerator OnLoadSavedWorldCompletedRoutine()
        {
            var rootObjects = _transition.LoadedScene.GetRootGameObjects();

            var unhandledObjects = rootObjects.Where(x => !Infra.S.IsScheduledOrDestroyed(x));

            if (unhandledObjects.Count() > 0)
            {
                Debug.LogError($"Some objects are still present in the temporary scene used for loading the saved world. " +
                    $"This is most probably due to an error in the loading process. " +
                    $"Leaving the scene loaded for debugging.");

                List<string> reportList = new();

                foreach (var obj in unhandledObjects)
                {
                    var id = Infra.S.IsRegistered(obj) ? Infra.S.GetObjectId(obj, Infra.GlobalReferencing) : RandomId.Default;
                    var path = obj.HierarchyPath();
                    reportList.Add(id + " | " + path + " | " + (obj == null).ToString());
                }

                string report = string.Join("\n", reportList);

                Debug.LogError($"The list of root objects that are still present in the temporary scene:\n{report}");

#if UNITY_EDITOR
                Debug.Log("Going to pause the editor to let inspect the stall gameobject.");
                Debug.Break();
#endif
            }
            else
            {
                yield return StartCoroutine(UnloadScene(_transition.LoadedScene));
            }
        }





        public IEnumerator Transition(SceneReference from, SceneReference to)
        {
            yield return StartCoroutine(LoadScene(_transition.BuildIndex));

            yield return StartCoroutine(UnloadScene(from.LoadedScene));

            SceneManager.SetActiveScene(_transition.LoadedScene);

            yield return StartCoroutine(LoadScene(to.BuildIndex));

            SceneManager.SetActiveScene(to.LoadedScene);

            foreach (var go in _transition.LoadedScene.GetRootGameObjects())
            {
                SceneManager.MoveGameObjectToScene(go, to.LoadedScene);
            }

            yield return StartCoroutine(UnloadScene(_transition.LoadedScene));
        }



        public IEnumerator LoadScene(int index)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            
            while (!loadOperation.isDone)
            {
                yield return null;
            }
        }

        public IEnumerator UnloadScene(Scene scene)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(scene.buildIndex);

            while (!unloadOperation.isDone)
            {
                yield return null;
            }
        }
    }
}
