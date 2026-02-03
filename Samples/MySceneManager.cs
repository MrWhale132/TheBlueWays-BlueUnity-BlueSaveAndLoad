
using Assets._Project.Scripts.SaveAndLoad;
using Eflatun.SceneReference;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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



        public void OnLoadSavedWorld()
        {
            StartCoroutine(OnLoadSavedWorldRoutine());
        }

        public IEnumerator OnLoadSavedWorldRoutine()
        {
            yield return StartCoroutine(LoadEmptyScene());

            yield return StartCoroutine(UnLoadMainMenuScene());
        }


        public IEnumerator OnLoadSavedWorldCompletedRoutine()
        {
            if (_transition.LoadedScene.GetRootGameObjects().Length > 0)
            {
                Debug.LogError($"Some objects are still present in the temporary scene used for loading the saved world. " +
                    $"This is most probably due to an error in the loading process. " +
                    $"Leaving the scene loaded for debugging.");
            }
            else
            {
                yield return StartCoroutine(UnloadScene(_transition.LoadedScene));
            }
        }




        public void StartNewWorld()
        {
            StartCoroutine(StartNewWorldRoutine());
        }

        public IEnumerator StartNewWorldRoutine()
        {
            yield return StartCoroutine(LoadEmptyScene());

            yield return StartCoroutine(UnLoadMainMenuScene());

            yield return StartCoroutine(LoadScene(_worldScene.BuildIndex));

            yield return StartCoroutine(UnloadEmptyScene());

            SceneManager.SetActiveScene(_worldScene.LoadedScene);
        }


        public void ExitWorld()
        {
            StartCoroutine(ExitWorldRoutine());
        }

        public IEnumerator ExitWorldRoutine()
        {
            yield return StartCoroutine(LoadScene(_transition.BuildIndex));

            yield return StartCoroutine(UnloadScene(_worldScene.LoadedScene));

            yield return StartCoroutine(LoadScene(_mainMenu.BuildIndex));

            yield return StartCoroutine(UnloadScene(_transition.LoadedScene));

            SceneManager.SetActiveScene(_mainMenu.LoadedScene);
        }


        public void OnBootstrapCompleted()
        {
            StartCoroutine(OnBootstrapCompletedRoutine());
        }

        public IEnumerator OnBootstrapCompletedRoutine()
        {
            yield return StartCoroutine(LoadMainMenu());

            yield return StartCoroutine(UnloadBootstrapScene());
        }



        public HashSet<Scene> GetAffectedSceneByExitingWorld()
        {
            HashSet<Scene> loadedWorldScenes = new HashSet<Scene>();

            int count = SceneManager.sceneCount;
            for (int i = 0; i < count; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if (scene == _worldScene.LoadedScene)
                {
                    loadedWorldScenes.Add(scene);
                }
            }

            return loadedWorldScenes;
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


        public IEnumerator UnloadBootstrapScene()
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(_bootstrapScene.BuildIndex);

            while (!unloadOperation.isDone)
            {
                yield return null;
            }
        }


        public IEnumerator LoadEmptyScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_transition.BuildIndex, LoadSceneMode.Additive);

            while (!loadOperation.isDone)
            {
                yield return null;
            }
        }


        public IEnumerator UnloadEmptyScene()
        {
            AsyncOperation loadOperation = SceneManager.UnloadSceneAsync(_transition.BuildIndex);

            while (!loadOperation.isDone)
            {
                yield return null;
            }
        }


        public IEnumerator LoadMainMenu()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_mainMenu.BuildIndex, LoadSceneMode.Additive);

            while (!loadOperation.isDone)
            {
                yield return null;
            }
        }

        public IEnumerator UnLoadMainMenuScene()
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(_mainMenu.BuildIndex);


            while (!unloadOperation.isDone)
            {
                yield return null;
            }

            //if (unloadOperation.isDone)
            //{
            //    Debug.Log("Main menu scene unloaded successfully.");
            //}
            //else
            //{
            //    Debug.LogError("Failed to unload main menu scene.");
            //}
        }
    }
}
