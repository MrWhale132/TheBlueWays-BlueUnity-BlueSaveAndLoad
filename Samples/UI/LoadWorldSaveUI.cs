using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.UI.MainMenuUI;
using Assets._Project.Scripts.UtilScripts;
using Eflatun.SceneReference;
using Packages.com.theblueway.saveandload.Samples;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadWorldSaveUI : MonoBehaviour
{
    public static LoadWorldSaveUI Singleton { get; private set; }


    public SceneReference _worldScene;
    public SceneReference _mainMenuScene;
    public SceneReference _tempScene;

    public Button _createNewWorldButton;


    public WorldElement _worldElementPrefab;
    public Transform _worldListContainer;

    public List<FileInfo> __foundWorldSaves;


    private void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Debug.LogError("Multiple instances of LoadWorldSaveUI detected. Destroying duplicate instance.");
            Destroy(gameObject);
            return;
        }

        Singleton = this;
    }


    protected virtual void Start()
    {
        _createNewWorldButton.onClick.AddListener(MyGameManager.Singleton.StartNewWorld);
    }


    //protected virtual void OnCreateNewWorldButtonClick()
    //{
    //    SceneManager.LoadSceneAsync(_tempScene.BuildIndex, LoadSceneMode.Additive).completed += (op) => UnloadMainMenu();


    //    void UnloadMainMenu()
    //    {
    //        SceneManager.UnloadSceneAsync(_mainMenuScene.BuildIndex).completed += (op) => _LoadWorld();
    //    }
    //}


    //protected virtual void _LoadWorld()
    //{
    //    SceneManager.LoadSceneAsync(_worldScene.BuildIndex, LoadSceneMode.Additive).completed += (op) => _UnLoadTempScene();

    //    void _UnLoadTempScene()
    //    {
    //        SceneManager.UnloadSceneAsync(_tempScene.BuildIndex);
    //    }
    //}




    public void Open()
    {
        gameObject.SetActive(true);


        string folderPath = Paths.Singleton.WorldSavePath;

        if(!Directory.Exists(folderPath))
        {
            return;
        }


        __foundWorldSaves =
            Directory.GetFiles(folderPath, "*.json")
                     .Select(path => new FileInfo(path))
                     .OrderByDescending(fileInfo => fileInfo.LastWriteTime)
                     .ToList();

        for (int i = 0; i < __foundWorldSaves.Count; i++)
        {
            var file = __foundWorldSaves[i];

            var element = Instantiate(_worldElementPrefab, _worldListContainer);

            int index = i; // Capture the current index for the listener

            element._worldNameText.text = file.Name;
            element._selectWorldButton.onClick.AddListener(() => OnWorldMenuItemClicked(index));
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        //_worldListContainer.DestroyAllChildren();
    }

    protected virtual void OnWorldMenuItemClicked(int i)
    {
        FileInfo fileInfo = __foundWorldSaves[i];

        MyGameManager.Singleton.LoadSavedWorld(fileInfo.FullName);
        //MySceneManager.Singleton.LoadWorld(fileInfo.FullName);
    }


}
