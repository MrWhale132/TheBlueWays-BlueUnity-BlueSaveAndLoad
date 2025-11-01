using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.Editor;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.CodeGen;
using Assets._Project.Scripts.UtilScripts.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static SaveHandlerAutoGenerator;
using Debug = UnityEngine.Debug;



public class SaveAndLoadCodeGenWindow : EditorWindow
{
    public static ConcurrentQueue<FileSystemEventArgsDto> _eventQueue = new();

    public SaveAndLoadCodeGenWindowState _state;
    public SaveAndLoadManager.Service _saveAndLoadService = new();

    public SaveAndLoadCodeGenSettings _userSettings;


    public Vector2 _scrollPos;
    public List<FileSystemEventArgsDto> _changedFiles = new();
    public HashSet<int> _selectedIndices = new HashSet<int>(); // which entries are selected


    private SceneAsset _selectedSceneToScan;



    //public int _test;


    private void OnEnable()
    {
        LoadState();
    }
    private void OnDisable()
    {
        SaveState();
    }



    public void SaveState()
    {
        //Debug.Log("save state");
        _state._eventqueue = _eventQueue.ToList();

        _state._scrollPos = _scrollPos;
        _state._changedFiles = _changedFiles;
        _state._selectedIndices = _selectedIndices.ToList();
        _state._userSettings = _userSettings;
        _state._selectedFolderToScan = _selectedFolderToScan;
        EditorUtility.SetDirty(_state);
        EditorUtility.SetDirty(_userSettings);
        AssetDatabase.SaveAssets();
    }




    public void LoadState()
    {
        var guid = AssetDatabase.FindAssets($"{nameof(SaveAndLoadCodeGenWindow)}").First();
        var windowPath = AssetDatabase.GUIDToAssetPath(guid);
        var dir = Path.GetDirectoryName(windowPath);
        var statePath = Path.Combine(dir, $"{nameof(SaveAndLoadCodeGenWindowState)}.asset");

        _state = AssetDatabase.LoadAssetAtPath<SaveAndLoadCodeGenWindowState>(statePath);
        if (_state == null)
        {
            _state = CreateInstance<SaveAndLoadCodeGenWindowState>();
            AssetDatabase.CreateAsset(_state, statePath);
            AssetDatabase.SaveAssets();
        }
        _selectedFolderToScan = _state._selectedFolderToScan;
        _userSettings = _state._userSettings;
        _eventQueue = new ConcurrentQueue<FileSystemEventArgsDto>(_state._eventqueue);
        _scrollPos = _state._scrollPos;
        _changedFiles = _state._changedFiles;
        _selectedIndices = new HashSet<int>(_state._selectedIndices);
    }





    public static DateTime __lastCheckedDeltaTime;

    private void OnInspectorUpdate()
    {
        if (DateTime.Now - __lastCheckedDeltaTime < TimeSpan.FromSeconds(1)) return;
        __lastCheckedDeltaTime = DateTime.Now;

        bool changed = false;

        while (_eventQueue.TryDequeue(out var e))
        {
            // Now safely handle on Unity’s main thread
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                _changedFiles.RemoveAll(f => f.FullPath == e.FullPath);
                continue;
            }


            if (e.FullPath.Replace("\\", "/").Contains("/Editor/"))
            {
                continue;
            }

            bool duplicate = _changedFiles.Any(f => f.FullPath == e.FullPath && f.ChangeType == e.ChangeType);

            if (!duplicate)
            {
                changed = true;
                _changedFiles.Add(e);
            }
        }

        if (changed)
        {
            Repaint();
        }
    }


    public class FileSystemEventArgsDtoEqualityComparer : IEqualityComparer<FileSystemEventArgsDto>
    {
        public bool Equals(FileSystemEventArgsDto x, FileSystemEventArgsDto y)
        {
            return x.FullPath == y.FullPath && x.ChangeType == y.ChangeType;
        }

        public int GetHashCode(FileSystemEventArgsDto obj)
        {
            return obj.FullPath.GetHashCode() ^ obj.ChangeType.GetHashCode();
        }
    }




    [MenuItem("Window/Save&Load CodeGen")]
    public static void ShowWindow()
    {
        // Creates (or focuses) a new tabbed editor window
        GetWindow<SaveAndLoadCodeGenWindow>("Save&Load CodeGen");
    }

    private void OnGUI()
    {
        CodeGenUtils.Config = ToCodeGenConfig(_userSettings);


        //
        //changed files section
        //

        EditorGUILayout.LabelField("Changed Files", EditorStyles.boldLabel);


        EditorGUILayout.BeginHorizontal();
        // Scrollable list
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(200), GUILayout.Width(350));
        for (int i = 0; i < _changedFiles.Count; i++)
        {
            var entry = _changedFiles[i];

            bool selected = _selectedIndices.Contains(i);

            var style = new GUIStyle("Button")
            {
                alignment = TextAnchor.MiddleLeft
            };


            bool newSelected = GUILayout.Toggle(selected, Path.GetFileName(entry.FullPath), style, GUILayout.Width(250));


            if (newSelected != selected)
            {
                if (newSelected)
                    _selectedIndices.Add(i);
                else
                    _selectedIndices.Remove(i);
            }
        }
        EditorGUILayout.EndScrollView();


        EditorGUILayout.Space(100);

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("CodeGen Settings", GUILayout.Width(120));

        _userSettings = (SaveAndLoadCodeGenSettings)EditorGUILayout.ObjectField(
            _userSettings,
            typeof(SaveAndLoadCodeGenSettings),
            false,
            options: GUILayout.Width(150));

        EditorGUILayout.EndHorizontal();


        EditorGUILayout.Space();


        if (GUILayout.Button("Regenerate All Existing SaveHandlers"))
        {
            var tasks = CreateCodeGenTasks(_saveAndLoadService.SaveHandlerAttributesByHandledType.Keys);

            CreateTypeReportsAndRunCodeGen(tasks);
        }

        _userSettings.GenerateExampleSaveHandlersForManuallyHandledTypesToo =
            GUILayout.Toggle(_userSettings.GenerateExampleSaveHandlersForManuallyHandledTypesToo, "Generate Example SaveHandlers For Manually Handled Types Too");

        EditorGUILayout.Space(10);



        if (GUILayout.Button("Clear All"))
        {
            ClearFileList();
        }
        if (GUILayout.Button("Discard Selection"))
        {
            DiscardSelected();
        }
        if (GUILayout.Button("Run CodeGen on Selected"))
        {
            var tasks = CreateCodeGenTasksFromSelected();

            CreateTypeReportsAndRunCodeGen(tasks);

            //turned off for easier dev testing
            //RemoveSelectedFiles();
        }


        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();


        //
        //scene selection section
        //


        EditorGUILayout.Space(30);

        EditorGUILayout.LabelField("Select a Scene", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();


        EditorGUILayout.LabelField("Scene", GUILayout.Width(60));

        _selectedSceneToScan = (SceneAsset)EditorGUILayout.ObjectField(
            _selectedSceneToScan,
            typeof(SceneAsset),
            false,
            options: GUILayout.Width(150));


        if (_selectedSceneToScan != null)
        {
            if (GUILayout.Button("Find Unhandled Types"))
            {
                _foundTypesInSelectedScene = FindUnhandledTypesInScene(_selectedSceneToScan);

                if (_foundTypesInSelectedScene.Count == 0)
                    Debug.Log("No unhandled types found.");
            }

            if (GUILayout.Button("Select All"))
            {
                _selectedTypesFoundInSelectedSceneTypesList = new HashSet<Type>(_foundTypesInSelectedScene);
            }

            if (GUILayout.Button("Discard All Selected"))
            {
                _selectedTypesFoundInSelectedSceneTypesList.Clear();
            }

            if (GUILayout.Button("Generate"))
            {
                CreateTypeReportsAndRunCodeGen(_selectedTypesFoundInSelectedSceneTypesList);
                _selectedTypesFoundInSelectedSceneTypesList.Clear();
            }
        }
        EditorGUILayout.EndHorizontal();



        if (_selectedSceneToScan != null && _foundTypesInSelectedScene != null && _foundTypesInSelectedScene.Count != 0)
        {
            _sceneTypesListScrollPos = EditorGUILayout.BeginScrollView(_sceneTypesListScrollPos, GUILayout.Height(200));

            {
                int i = 0;

                foreach (var type in _foundTypesInSelectedScene)
                {
                    var style = new GUIStyle("Button")
                    {
                        alignment = TextAnchor.MiddleLeft
                    };

                    bool selected = _selectedTypesFoundInSelectedSceneTypesList.Contains(type);

                    bool newSelected = GUILayout.Toggle(selected, type.Name, style, GUILayout.Width(250));


                    if (newSelected != selected)
                    {
                        if (newSelected)
                            _selectedTypesFoundInSelectedSceneTypesList.Add(type);
                        else
                            _selectedTypesFoundInSelectedSceneTypesList.Remove(type);
                    }

                    i++;
                }
            }
            EditorGUILayout.EndScrollView();
        }


        //
        //prefab folders section
        //




        EditorGUILayout.Space(30);

        EditorGUILayout.BeginHorizontal();


        if (_userSettings != null)
        {
            if (GUILayout.Button("Find Unhandled Types In Prefabs"))
            {
                _foundTypesInPrefabFolders = FindUnhandledTypesInPrefabFolders(_userSettings);

                if (_foundTypesInPrefabFolders.Count == 0)
                    Debug.Log("No unhandled types found.");
            }

            if (GUILayout.Button("Select All"))
            {
                _selectedTypesFoundInfPrefabFoldersTypesList = new HashSet<Type>(_foundTypesInPrefabFolders);
            }

            if (GUILayout.Button("Discard All Selected"))
            {
                _selectedTypesFoundInfPrefabFoldersTypesList.Clear();
            }

            if (GUILayout.Button("Generate"))
            {
                CreateTypeReportsAndRunCodeGen(_selectedTypesFoundInfPrefabFoldersTypesList);
                _selectedTypesFoundInfPrefabFoldersTypesList.Clear();
            }
        }
        EditorGUILayout.EndHorizontal();



        if (_userSettings != null && _foundTypesInPrefabFolders != null && _foundTypesInPrefabFolders.Count != 0)
        {
            _prefabFolderTypesListScrollPos = EditorGUILayout.BeginScrollView(_prefabFolderTypesListScrollPos, GUILayout.Height(200));

            if (_foundTypesInPrefabFolders != null)
            {
                int i = 0;

                foreach (var type in _foundTypesInPrefabFolders)
                {
                    var style = new GUIStyle("Button")
                    {
                        alignment = TextAnchor.MiddleLeft
                    };

                    bool selected = _selectedTypesFoundInfPrefabFoldersTypesList.Contains(type);

                    bool newSelected = GUILayout.Toggle(selected, type.Name, style, GUILayout.Width(250));


                    if (newSelected != selected)
                    {
                        if (newSelected)
                            _selectedTypesFoundInfPrefabFoldersTypesList.Add(type);
                        else
                            _selectedTypesFoundInfPrefabFoldersTypesList.Remove(type);
                    }

                    i++;
                }
            }
            EditorGUILayout.EndScrollView();
        }





        //
        //scan folder selection section
        //


        EditorGUILayout.Space(30);

        EditorGUILayout.LabelField("Input an Assets relative folder to scan for types. (Recursively)", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();


        EditorGUILayout.LabelField("Folder", GUILayout.Width(60));

        _selectedFolderToScan = EditorGUILayout.TextField(_selectedFolderToScan, options: GUILayout.Width(150));

        _selectedFolderToScan ??= "";


        bool exists = Directory.Exists(Path.Combine(Application.dataPath, _selectedFolderToScan));


        if (!exists)
        {
            EditorGUILayout.LabelField("Folder does not exist!", GUILayout.Width(150));
        }
        else
        {
            if (GUILayout.Button("Find Unhandled Types"))
            {
                _foundTypesInFolderToScan = FindUnhandledTypesInFolder(_selectedFolderToScan);

                if (_foundTypesInFolderToScan.Count == 0)
                    Debug.Log("No unhandled types found.");
            }

            if (GUILayout.Button("Select All"))
            {
                _selectedTypesFoundInfFolderToScanTypesList = new HashSet<Type>(_foundTypesInFolderToScan);
            }

            if (GUILayout.Button("Discard All Selected"))
            {
                _selectedTypesFoundInfFolderToScanTypesList.Clear();
            }

            if (GUILayout.Button("Generate"))
            {
                CreateTypeReportsAndRunCodeGen(_selectedTypesFoundInfFolderToScanTypesList);
                _selectedTypesFoundInfFolderToScanTypesList.Clear();
            }
        }
        EditorGUILayout.EndHorizontal();



        if (_foundTypesInFolderToScan != null && _foundTypesInFolderToScan.Count != 0)
        {
            _folderToScanTypesListScrollPos = EditorGUILayout.BeginScrollView(_folderToScanTypesListScrollPos, GUILayout.Height(200));

            {
                int i = 0;

                foreach (var type in _foundTypesInFolderToScan)
                {
                    var style = new GUIStyle("Button")
                    {
                        alignment = TextAnchor.MiddleLeft
                    };

                    bool selected = _selectedTypesFoundInfFolderToScanTypesList.Contains(type);

                    bool newSelected = GUILayout.Toggle(selected, type.Name, style, GUILayout.Width(250));


                    if (newSelected != selected)
                    {
                        if (newSelected)
                            _selectedTypesFoundInfFolderToScanTypesList.Add(type);
                        else
                            _selectedTypesFoundInfFolderToScanTypesList.Remove(type);
                    }

                    i++;
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }




    public HashSet<Type> _foundTypesInSelectedScene = new();
    public HashSet<Type> _selectedTypesFoundInSelectedSceneTypesList = new();
    public Vector2 _sceneTypesListScrollPos;


    public HashSet<Type> _foundTypesInPrefabFolders = new();
    public HashSet<Type> _selectedTypesFoundInfPrefabFoldersTypesList = new();
    public Vector2 _prefabFolderTypesListScrollPos;


    public HashSet<Type> _foundTypesInFolderToScan = new();
    public HashSet<Type> _selectedTypesFoundInfFolderToScanTypesList = new();
    public Vector2 _folderToScanTypesListScrollPos;
    public string _selectedFolderToScan;






    public HashSet<Type> FindUnhandledTypesInFolder(string assetsRelativeFolder)
    {
        if (assetsRelativeFolder is null) assetsRelativeFolder = "";

        string absPath = Path.Combine(Application.dataPath, assetsRelativeFolder);

        var csFiles = GetCsFiles(absPath);

        var foundTypes = new HashSet<Type>();

        foreach (var file in csFiles)
        {
            foundTypes.AddRange(FindTypesInFile(file));
        }


        var unhandledTypes = new HashSet<Type>();

        foreach (var type in foundTypes)
        {
            if (!_saveAndLoadService.IsTypeHandled_Editor(type)
                && !_userSettings.TypeExclusionSettings.ShouldExclude(type))
            {
                unhandledTypes.Add(type);
            }
        }

        return unhandledTypes;
    }


    IEnumerable<string> GetCsFiles(string root)
    {

        foreach (var file in Directory.EnumerateFiles(root, "*.cs"))
            yield return file;

        foreach (var dir in Directory.EnumerateDirectories(root))
        {
            var name = Path.GetFileName(dir);
            if (name.EndsWith("~")) continue; // skip ignored

            foreach (var file in Directory.EnumerateFiles(dir, "*.cs"))
                yield return file;

            foreach (var subFile in GetCsFiles(dir))
                yield return subFile;
        }
    }








    public HashSet<Type> FindUnhandledTypesInScene(SceneAsset sceneAsset)
    {
        string path = AssetDatabase.GetAssetPath(sceneAsset);

        Scene existingScene = SceneManager.GetSceneByPath(path);
        bool wasLoaded = existingScene.isLoaded;

        // Load only if not already loaded
        Scene scene = wasLoaded
            ? existingScene
            : EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);


        GameObject[] roots = scene.GetRootGameObjects();

        var types = GetAllComponentTypesFromGameObjects(roots);

        if (!wasLoaded)
            EditorSceneManager.CloseScene(scene, true);

        return types;
    }


    public HashSet<Type> GetAllComponentTypesFromGameObjects(IEnumerable<GameObject> gameObjects)
    {
        List<Component> allComponentsOfAllGameObjects = new();
        List<Component> components = new();


        foreach (var go in gameObjects)
        {
            go.GetComponentsInChildren(includeInactive:true, components);
            allComponentsOfAllGameObjects.AddRange(components);
        }

        var foundUnhandledTypes = allComponentsOfAllGameObjects
                            .Select(x => x.GetType())
                            .Where(t => !_saveAndLoadService.HasSaveHandlerForType_Editor(t)
                                        && !_userSettings.TypeExclusionSettings.ShouldExclude(t))
                            .ToHashSet();


        return foundUnhandledTypes;
    }




    public HashSet<Type> FindUnhandledTypesInPrefabFolders(SaveAndLoadCodeGenSettings settings)
    {
        IEnumerable<GameObject> prefabs = settings.PrefabFolderPaths.Select(path => GetPrefabsInDirectory(path)).SelectMany(x => x);

        return GetAllComponentTypesFromGameObjects(prefabs);
    }


    public List<GameObject> GetPrefabsInDirectory(string folderPath)
    {
        // Get all prefab GUIDs in the folder
        string[] guids = AssetDatabase.FindAssets("t:prefab", new[] { folderPath });

        var prefabs = new List<GameObject>();

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            prefabs.Add(AssetDatabase.LoadAssetAtPath<GameObject>(assetPath));
        }

        return prefabs;
    }








    private void DiscardSelected()
    {
        RemoveSelectedFiles();
    }





    public class CodeGenTask
    {
        public string filePath;
        public List<(Type rootType, List<Type> nestedTypes)> rootAndChildren = new();
    }


    public List<CodeGenTask> CreateCodeGenTasksFromSelected()
    {
        var filePaths = _changedFiles.Where((file, i) => _selectedIndices.Contains(i)).Select(f => f.FullPath);

        var tasks = CreateCodeGenTasks(filePaths);

        return tasks;
    }





    public IEnumerable<Type> FindTypesInFile(string filePath)
    {
        List<Type> typesInFile = new List<Type>();

        var inspectionReport = Roslyn.CodeGen.InspectCodeFile(filePath);


        var assemblyName = AssemblyResolver.ResolveAssembly(filePath);


        foreach (var typeReport in inspectionReport.TypeReports)
        {
            var namespaceName = typeReport.Namespace;
            var typeName = typeReport.TypeName;


            Type type = ResolveType(assemblyName, namespaceName, typeName);

            if (type == null)
            {
                Debug.LogError($"Could not resolve type {typeName} in namespace '{namespaceName}' from assembly {assemblyName}. Make sure the assembly is loaded.");
                continue;
            }

            typesInFile.Add(type);
        }

        return typesInFile;
    }



    public List<CodeGenTask> CreateCodeGenTasks(IEnumerable<string> filePaths)
    {
        List<Type> typesInFiles = new List<Type>();

        foreach (var file in filePaths)
        {
            typesInFiles.AddRange(FindTypesInFile(file));
        }

        return CreateCodeGenTasks(typesInFiles);
    }


    public List<CodeGenTask> CreateCodeGenTasks(IEnumerable<Type> types)
    {
        var codegenTasks = new List<CodeGenTask>();

        Dictionary<string, Type> rootTypesByName = new();
        Dictionary<string, List<Type>> nestedTypesByRootTypeName = new();

        foreach (var type in types)
        {
            if (type.IsNested)
            {
                int rootTypeNameEndIndex = type.FullName.IndexOf("+");

                var rootTypeName = type.FullName.Substring(0, rootTypeNameEndIndex);

                if (!nestedTypesByRootTypeName.ContainsKey(rootTypeName))
                {
                    nestedTypesByRootTypeName[rootTypeName] = new List<Type>();
                }

                nestedTypesByRootTypeName[rootTypeName].Add(type);
            }
            else
            {
                //todo: this will fail with two types with same fullname is two different assembly
                //perhaps just use the type isntance instead of its name?
                rootTypesByName.Add(type.FullName, type);
            }
        }


        //todo: lookup the containing file location of the type
        //a groupby will aslo be needed
        var codegenTask = new CodeGenTask { filePath = null };

        foreach ((string name, Type rootType) in rootTypesByName)
        {
            var nestedTypesOfRootType = nestedTypesByRootTypeName.TryGetValue(name, out var nestedTypes) ? nestedTypes : new List<Type>();

            codegenTask.rootAndChildren.Add((rootType, nestedTypesOfRootType));
        }

        codegenTasks.Add(codegenTask);


        return codegenTasks;
    }





    private void CreateTypeReportsAndRunCodeGen(IEnumerable<CodeGenTask> codeGenTasks)
    {
        IEnumerable<Type> Flatten(IEnumerable<CodeGenTask> codegenTasks)
        {
            var result = new List<Type>();

            foreach (var task in codegenTasks)
            {
                foreach ((var rootType, var nestedTypes) in task.rootAndChildren)
                {
                    result.Add(rootType);

                    foreach (var nestedType in nestedTypes)
                        result.Add(nestedType);
                }
            }
            return result;
        }

        var flattened = Flatten(codeGenTasks);

        CreateTypeReportsAndRunCodeGen(flattened);
    }


    private void CreateTypeReportsAndRunCodeGen(IEnumerable<Type> typesToHandle)
    {
        var allAsms = AppDomain.CurrentDomain.GetAssemblies();


        #region Dev Test/Debug

        //var refasms = typeof(Rigidbody).Assembly.GetReferencedAssemblies();

        //var refAsms2 = allAsms.Where(a => refasms.Any(r => r.FullName == a.FullName)).ToList();
        ////Debug.Log("ref1 count: " + refasms.Count());
        ////Debug.Log("ref2 count: " + refAsms2.Count());
        ////Debug.Log("type count: " + refAsms2.SelectMany(a => a.GetTypes()).Count());


        //var assemblies = refAsms2
        //        .Select(a =>
        //        {
        //            try { return (a.GetName().Name, a.GetTypes().Length); }
        //            catch (ReflectionTypeLoadException ex) { return ("excp", ex.Types.Where(t => t != null).Count()); }
        //        });

        //assemblies = assemblies.OrderByDescending(a => a.Item2);

        //var writer2 = File.CreateText("C:/temp/refassemblies.txt");

        //foreach (var asm in assemblies)
        //{
        //    writer2.WriteLine($"{asm.Item2}\t{asm.Item1}");
        //}
        //writer2.Close();
        //writer2.Dispose();



        //var unitycoremodulename = assemblies.ElementAt(2).Item1;

        //var module = refAsms2.FirstOrDefault(a => a.GetName().Name == unitycoremodulename);

        //writer2 = File.CreateText("C:/temp/coretypes.txt");

        //foreach (var type in module.GetTypes())
        //{
        //    writer2.WriteLine(type.FullName);
        //}
        //writer2.Close();
        //writer2.Dispose();



        //var depAsm = typeof(Animal).Assembly;

        //var dependantAsms = allAsms.Where(a => a.GetReferencedAssemblies().Any(r => r.FullName == depAsm.FullName)).OrderByDescending(a => a.GetTypes().Length).ToList();

        //writer2 = File.CreateText("C:/temp/dependants.txt");

        //foreach (var asm in dependantAsms)
        //{
        //    writer2.WriteLine(asm.GetTypes().Length + "\t" + asm.GetName().Name);
        //}
        //writer2.Close();
        //writer2.Dispose();


        #endregion


        //Debug.Log("running codegen... Task coun: " + typesToHandle.Count());

        Queue<Type> discoveryQueue = new Queue<Type>();
        Dictionary<Type, TypeReport> discoveredTypes = new();


        foreach (var type in typesToHandle)
        {
            discoveryQueue.Enqueue(type);
        }


        //todo: move these to type exlusion config
        HashSet<string> excludedNameSpaces = new()
        {
            typeof(Assets._Project.Scripts.SaveAndLoad.SaveAndLoadManager).Namespace,
            typeof(Newtonsoft.Json.JsonConvert).Namespace,
            typeof(System.Collections.IEnumerable).Namespace,//and maybe this one too
            typeof(System.Reflection.Emit.TypeBuilder).Namespace,//except this one
            //typeof(Assets._Project.Scripts.UtilScripts.DataStructures.NetworkSerializables.ListBytes<>).Namespace,
        };


        //this is not a type exclusion list, this is a checkIfOthersImplementThisType exclusion. So these types will still get their savehandlers
        HashSet<Type> visitedTypes = new()
        {
            //pre-exclude this type from type discovery because we dont want to add and iterate over all of types that inherits from it
            typeof(object),
            typeof(UnityEngine.Object),
            typeof(System.ValueType),
        };


        //debug purpose
        bool found = false;


        HashSet<Type> typesFromChangedFiles = new(discoveryQueue);






        string SINGLEITERATION = "SINGLE_ITERATION";
        string STATICREFERENCEINSPECTION = "STATIC_REFERENCE_INSPECTION";
        string GET_CECIL_TYPE = "GET_CECIL_TYPE";

        //Dictionary<Type,Dictionary<string, List<string>>> benchmarkPerType = new();
        Dictionary<string, List<TimeSpan>> benchmark = new()
        {
            {SINGLEITERATION, new ()},
            {STATICREFERENCEINSPECTION, new ()},
            {GET_CECIL_TYPE, new ()},
        };

        Stopwatch stopwatch = Stopwatch.StartNew();

        TimeSpan start;
        TimeSpan end;




        int maxIterations = 1000;
        //todo:
        //left to do: function pointers, pointer types, dynamic
        //
        while (discoveryQueue.Count > 0)
        {
            maxIterations--;

            if (maxIterations < 0)
            {
                Debug.LogError("Max iterations reached. There is probably a bug causing infinite loop. Stopping.");
                break;
            }



            var type = discoveryQueue.Dequeue();

            //todo: remove once static are supported
            //if (type.IsStatic()) continue;



            if (_userSettings.IgnoreAnyObsolete && type.IsDefined(typeof(ObsoleteAttribute), true))
            {
                Debug.LogWarning("Skipping obsolete type that it self obsolete or inherits from an obsolete type: " + type.AssemblyQualifiedName);
                continue;
            }


            if (_userSettings.TypeExclusionSettings.ShouldExclude(type)) continue;


            //if the codegen logic was ran for a type we can not change, for example Unity's or Microsoft's types,
            // there is no point to discover their dependencies again as they most probably didnt change since then.
            // even if they did, an option to force discovery to update their handles will be implemented
            //todo:
            if (_saveAndLoadService.HasSerializer_Editor(type)) continue;

            if (!_userSettings.GenerateExampleSaveHandlersForManuallyHandledTypesToo)
            {
                bool notTheTypeThatWasChanged = !typesFromChangedFiles.Contains(type);
                if (notTheTypeThatWasChanged)
                {
                    bool alreadyHandled = _saveAndLoadService.IsTypeHandled_Editor(type);

                    if (alreadyHandled) continue;
                }
            }



            bool excluded = type.Namespace != null && excludedNameSpaces.Any(ns => type.Namespace.StartsWith(ns));
            if (excluded)
            {
                Debug.LogWarning("Skipping type from excluded namespace: " + type.AssemblyQualifiedName);
                continue;
            }





            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                foreach (var argType in type.GenericTypeArguments)
                    discoveryQueue.Enqueue(argType);

                //keep in mind, after this, we work with the type def, not with the constructed type
                type = type.GetGenericTypeDefinition();

                //this is the first time we encounter this gen type def
                if (!discoveredTypes.ContainsKey(type))
                    foreach (var constraint in type.GetGenericArguments().SelectMany(a => a.GetGenericParameterConstraints()))
                        discoveryQueue.Enqueue(constraint);
            }



            if (discoveredTypes.ContainsKey(type)) continue;



            //todo: should we prepare for other element types too? ByRef and Pointer types
            if (type.IsArray)
            {
                discoveryQueue.Enqueue(type.GetElementType());
                continue;
            }




            bool classOrStructOrInterface = type.IsClass || type.IsStruct() || type.IsInterface;

            if (!classOrStructOrInterface || type == typeof(string)
                || type.IsGenericParameter
                || type.IsAssignableTo(typeof(Delegate)))
            {
                continue;
            }



            start = stopwatch.Elapsed;

            var cecilType = CodeGenUtils.GetCecilTypeDefinition(type);

            end = stopwatch.Elapsed;
            benchmark[GET_CECIL_TYPE].Add(end - start);


            if (cecilType == null)
            {
                Debug.LogWarning($"No Cecil typedef has found for type {type.CleanAssemblyQualifiedName()}.");
            }
            if (cecilType != null && !cecilType.IsCompileTimePublic())
            {
                Debug.LogWarning("Skipping non public type: " + type.AssemblyQualifiedName);
                continue;
            }



            //too slow: from 30ms to ~350ms per type
            //start = stopwatch.Elapsed;

            //var staticReferences = CodeGenUtils.GetStaticlyReferencedTypes(cecilType);

            //foreach (var staticTypeDef in staticReferences.ResolvedTypes)
            //{
            //    var staticType = CodeGenUtils.ResolveType(staticTypeDef);
            //    discoveryQueue.Enqueue(staticType);
            //    //Debug.Log(staticType.FullName);
            //}

            //end = stopwatch.Elapsed;
            //benchmark[STATICREFERENCEINSPECTION].Add(end - start);






            string assemblyName = type.Assembly.GetName().Name;

            // we do not want to traverse microsoft's interfaces like IDisposable, IComparable, etc.
            // tough with this, there is no guarantee that every implementation will be covered
            //todo: config to switch this logic
            bool interfaceFromMicroSoft = type.IsInterface && assemblyName.StartsWith("mscorlib");

            bool checkTypesIfTheyInheritOrImplementThisType =
                                       !visitedTypes.Contains(type)
                                    && !type.IsAbstract && !type.IsSealed && !type.IsValueType && !type.IsStatic()
                                    && !interfaceFromMicroSoft;


            if (checkTypesIfTheyInheritOrImplementThisType)
            {
                List<Assembly> dependantAsms;

                //these assemblies have too many dependant assemblies, iterating over their types too is too slow (minutes).
                if (assemblyName.StartsWith("Unity", StringComparison.OrdinalIgnoreCase)
                  || assemblyName.StartsWith("mscorlib"))
                {
                    dependantAsms = new List<Assembly>() { type.Assembly };
                }
                else
                    dependantAsms = allAsms.Where(a => a.GetReferencedAssemblies().Any(r => r.FullName == type.Assembly.FullName))
                                           .Append(type.Assembly).ToList();


                var allTypes = dependantAsms.SelectMany(a => a.GetTypes()).ToList();
                //Debug.Log(allTypes.Count + " types in dependant assemblies for " + type.FullName);



                int count = 0;
                foreach (var otherType in allTypes)
                {
                    var assignable = type.IsGenericType
                                    ? otherType.IsAssignableToGenericTypeDefinition(type)
                                    : otherType.IsAssignableTo(type);


                    if (assignable)
                    {
                        discoveryQueue.Enqueue(otherType);

                        count++;
                        if (otherType.IsGenericType)
                            visitedTypes.Add(otherType.GetGenericTypeDefinition());
                        else
                            visitedTypes.Add(otherType);
                    }
                }
                //if (type.IsInterface)
                //    Debug.Log($"Found {count} types implementing interface {type.FullName}");
            }




            //if anything fails after this, we still gain the perforamnce of not visiting this type again.
            //if we failed ones, no reason (so far) to check it again.
            discoveredTypes.Add(type, null);

            //Debug.Log(type.FullName);



            TypeReport CreateTypeReport(Type t, BindingFlags binding)
            {

                var fieldsReport = GetFieldInfos(type, binding);

                var properties = CodeGenUtils.GetSimpleProperties(type, binding);

                //todo: config
                var methods = type.GetUsableMethods(binding | BindingFlags.DeclaredOnly).ToArray();

                var events = type.GetEvents(binding);

                if (_userSettings.IgnoreAnyObsolete)
                {
                    events = events.Where(e => !e.IsDefined(typeof(ObsoleteAttribute))).ToArray();
                }


                var typeReport = new TypeReport
                {
                    ReportedType = type,
                    FieldsReport = fieldsReport,
                    Properties = properties,
                    Events = events,
                    Methods = methods
                };

                return typeReport;
            }



            //Debug.LogWarning(type);

            var binding = BindingFlags.Public | BindingFlags.Static;

            var typeReport = CreateTypeReport(type, binding);

            //Debug.Log(typeReport.FieldsReport.ValidFields.Count + " valid fields found.");

            if (!type.IsStatic())
            {
                binding |= BindingFlags.Instance;
                binding &= ~BindingFlags.Static;

                if(type == typeof(UnityEngine.UI.MaskableGraphic))
                {

                }
                var instanceReport = CreateTypeReport(type, binding);

                var staticReport = typeReport;

                instanceReport.StaticReport = staticReport;
                typeReport = instanceReport;
            }


            discoveredTypes[type] = typeReport;


            if(type == typeof(UnityEngine.UI.MaskableGraphic.CullStateChangedEvent))
            {

            }
            List<Type> GetDependencies(TypeReport typeReport)
            {
                var deps = new List<Type>();
                deps.AddRange(typeReport.FieldsReport.ValidFields.Select(f => f.FieldInfo.FieldType));
                deps.AddRange(typeReport.Properties.Select(p => p.PropertyType));
                deps.AddRange(typeReport.Events.Select(e => e.EventHandlerType));

                return deps;
            }

            var dependencies = new List<Type>();

            dependencies.AddRange(GetDependencies(typeReport));

            if (!type.IsStatic())
            {
                dependencies.AddRange(GetDependencies(typeReport.StaticReport));
            }



            foreach (var depType in dependencies)
            {
                discoveryQueue.Enqueue(depType);
            }

            if (type.BaseType != null)
            {
                discoveryQueue.Enqueue(type.BaseType);
                visitedTypes.Add(type.BaseType.IsGenericType ? type.BaseType.GetGenericTypeDefinition() : type.BaseType);//so we dont check who is assignable to it
            }


            benchmark[SINGLEITERATION].Add(stopwatch.Elapsed);
            stopwatch.Restart();
            //if (!found && discoveryQueue.Any(t => t.FullName == "UnityEngine.Animation"))
            //{
            //    Debug.LogError(type.FullName + " added unwanted type");
            //    found = true;
            //}
        }

        //Debug.Log("discovery done. found " + discoveredTypes.Count + " types.");

        Debug.Log("Benchmark: Sum Total");
        foreach ((var key, var times) in benchmark)
        {
            TimeSpan total = new TimeSpan(times.Sum(t => t.Ticks));
            Debug.Log($"{key}: {total.TotalMilliseconds} ms over {times.Count} iterations. Avg: {total.TotalMilliseconds / times.Count} ms");
        }











        if (_userSettings.DoNotGenerateFilesAtTheEnd) return;



        //dev test/debug
        var names = discoveredTypes.Select(t => t.Key.FullName).OrderBy(name => name).ToList();

        if (Directory.Exists("C:/temp") == false) Directory.CreateDirectory("C:/temp");
        File.WriteAllText("C:/temp/discoveredTypes.txt", string.Join("\n", names));




        var codeGenerator = ScriptableObject.CreateInstance<SaveHandlerAutoGenerator>();


        List<(Type type, CodeGenerationResult generationResult)> typesAndTheirgenerationResults = new();

        foreach ((var type, var report) in discoveredTypes)
        {
            //if (_saveAndLoadService.HasManualSaveHandlerForType_Editor(type)) continue;

            var generationResult = codeGenerator.GenerateSavingAndLoadingCode(report);
            typesAndTheirgenerationResults.Add((type, generationResult));
        }


        Type d_type = null;
        try
        {
            AssetDatabase.StartAssetEditing();


            foreach ((var type, var generationResult) in typesAndTheirgenerationResults)
            {
                d_type = type;
                string relativeDirPath = $"_Project/Scripts/SaveHandlers/{(type.IsStruct() ? "DevTestCustomDatas" : "DevTest")}";

                relativeDirPath = relativeDirPath.Replace("/", Path.DirectorySeparatorChar.ToString());


                string absDirPath = Path.Combine(Application.dataPath, relativeDirPath);

                if (!Directory.Exists(absDirPath))
                {
                    Directory.CreateDirectory(absDirPath);
                }


                _saveAndLoadService.IsTypeManuallyHandled_Editor(type, out bool hasManualInstanceHandler, out bool hasManualStaticHandler);


                string fileName = type.IsStatic() ?
                    generationResult.StaticHandlerInfo.GeneratedTypeName + ".cs" :
                    generationResult.HandlerInfo.GeneratedTypeName + ".cs";


                List<string> parts = new();

                if (!hasManualStaticHandler)
                {
                    parts.Add(generationResult.StaticHandlerInfo.GeneratedTypeText);
                    parts.Add(generationResult.StaticSaveDataInfo.GeneratedTypeText);
                }
                if (!type.IsStatic() && !hasManualInstanceHandler)
                {
                    parts.Insert(0, generationResult.HandlerInfo.GeneratedTypeText);

                    if (generationResult.SaveDataInfo != null) //customsavedatas dont have savedata
                        parts.Insert(1, generationResult.SaveDataInfo.GeneratedTypeText);
                }

                string mergedFileContent = parts.StringJoin(Environment.NewLine + Environment.NewLine);


                //Debug.LogError(mergedFileContent);


                var namespaces = new List<string>();

                if (generationResult.HandlerInfo != null)
                    namespaces.AddRange(generationResult.HandlerInfo.NameSpaceNames);
                if (generationResult.SaveDataInfo != null)
                    namespaces.AddRange(generationResult.SaveDataInfo.NameSpaceNames);

                namespaces.AddRange(generationResult.StaticHandlerInfo.NameSpaceNames);
                namespaces.AddRange(generationResult.StaticSaveDataInfo.NameSpaceNames);



                CsFileBuilder builder = new CsFileBuilder()
                {
                    GeneratedTypeText = mergedFileContent,
                    NameSpace = "DevTest",
                    NameSpaceNames = namespaces.ToHashSet(),
                };



                string fileContent = builder.BuildFile();





                if (hasManualInstanceHandler && hasManualStaticHandler)
                {
                    if (_userSettings.GenerateExampleSaveHandlersForManuallyHandledTypesToo)
                    {
                        relativeDirPath = _userSettings.InactiveSaveHandlersFolder;
                        //Debug.Log("here " + type.FullName ?? type.Name);
                    }
                    else
                        continue;
                }

                //todo: accpet path from config
                string outputPath = Path.Combine(absDirPath, fileName);

                using var writer = File.CreateText(outputPath);

                writer.Write(fileContent);

                Debug.Log($"SaveHandler file {fileName} created at {outputPath}");
            }

        }
        catch (Exception ex)
        {
            Debug.LogError($"something bad happend at type: {d_type.CleanAssemblyQualifiedName()}. Exception: " + ex.ToString());
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
        }



        //uncomment after finished testing
        //RemoveSelectedFiles();
    }






    public class FileReport
    {
        public string FilePath;
        public List<TypeReport> TypeReports = new();
        public List<TypeReport> DependencyTypeReports = new();
    }
    public class TypeReport
    {
        public TypeReport StaticReport;
        public Type ReportedType;
        public GetFieldInfosReport FieldsReport;
        public IEnumerable<PropertyInfo> Properties;
        public MethodInfo[] Methods;
        public EventInfo[] Events;
    }

    [Flags]
    public enum FieldInfoCodeGenValidationCode
    {
        Valid = 0,
        NonPublic = 1,
        ReadOnly = 2,
        //Required, c#11
        Const = 4,
        UnityEvent = 8,
        Obsolete = 16,
    }
    public class GetFieldInfosReport
    {
        public List<FieldInfoReport> FieldInfoReports;
        public List<FieldInfoReport> StaticFieldInfoReports;

        public List<FieldInfoReport> ValidFields =>
            FieldInfoReports.Where(r => r.ValidationCode == FieldInfoCodeGenValidationCode.Valid).ToList();

        //public List<FieldInfoReport> ValidStaticFields =>
        // StaticFieldInfoReports.Where(r => r.ValidationCode == FieldInfoCodeGenValidationCode.Valid).ToList();

        public List<FieldInfoReport> InvalidFields =>
        FieldInfoReports.Where(r => r.ValidationCode != FieldInfoCodeGenValidationCode.Valid).ToList();

        //public List<FieldInfoReport> InvalidStaticFields =>
        //    StaticFieldInfoReports.Where(r => r.ValidationCode != FieldInfoCodeGenValidationCode.Valid).ToList();
    }
    public class FieldInfoReport
    {
        public FieldInfo FieldInfo;
        public FieldInfoCodeGenValidationCode ValidationCode;
    }

    public GetFieldInfosReport GetFieldInfos(Type type, BindingFlags binding)
    {
        var report = new GetFieldInfosReport
        {
            FieldInfoReports = new List<FieldInfoReport>(),
            StaticFieldInfoReports = new List<FieldInfoReport>()
        };

        var fieldReports = type.GetFields(binding)
                               .Select(f => new FieldInfoReport() { FieldInfo = f });


        bool getNonPublicsToo = binding.HasFlag(BindingFlags.NonPublic);


        foreach (var fieldReport in fieldReports)
        {
            var field = fieldReport.FieldInfo;

            if (!getNonPublicsToo && !field.IsPublic) fieldReport.ValidationCode |= FieldInfoCodeGenValidationCode.NonPublic;
            if (field.IsInitOnly) fieldReport.ValidationCode |= FieldInfoCodeGenValidationCode.ReadOnly;
            if (field.IsLiteral && !field.IsInitOnly) fieldReport.ValidationCode |= FieldInfoCodeGenValidationCode.Const;
            if (typeof(UnityEvent).IsAssignableFrom(field.FieldType)) fieldReport.ValidationCode |= FieldInfoCodeGenValidationCode.UnityEvent;
            if (_userSettings.IgnoreAnyObsolete && field.IsDefined(typeof(ObsoleteAttribute))) fieldReport.ValidationCode |= FieldInfoCodeGenValidationCode.Obsolete;

            //Valid is the default, so no need to set it explicitly

            //if (field.IsStatic) report.StaticFieldInfoReports.Add(fieldReport);
            else report.FieldInfoReports.Add(fieldReport);
        }

        return report;
    }



    //todo: test out this method
    public static bool IsAutoImplemented(PropertyInfo prop) =>
prop.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any() ||
(prop.GetMethod?.IsDefined(typeof(CompilerGeneratedAttribute), false) ?? false) ||
        (prop.SetMethod?.IsDefined(typeof(CompilerGeneratedAttribute), false) ?? false);

    //tests
    public int _field;
    int _field2;
    public int Prop1 { get; set; }
    public int Prop2 { get => _field; set => _field = value; }
    public int Prop3 { get { return _field; } set => _field = value; }
    public int prop4 { get; }
    public int prop9 { set { } }
    public int prop5 { set => _field = value; }
    public int prop6 { get => _field2; set => _field = value; }
    public int prop7 { get { var a = _field2 * 3; return _field; } set => _field = value; }
    public int prop8 { get { var a = _field2 * 3; return _field; } set { var a = _field * 3; _field = value; } }




    public static Type ResolveType(string assemblyName, string namespaceName, string typeName)
    {
        return CodeGenUtils.ResolveType(assemblyName, namespaceName, typeName);
    }






    public void ClearFileList()
    {
        _changedFiles.Clear();
        _selectedIndices.Clear();
    }


    private void RemoveSelectedFiles()
    {
        // Remove in reverse order to avoid index shifting
        var indices = new List<int>(_selectedIndices);
        indices.Sort((a, b) => b.CompareTo(a));


        foreach (var index in indices)
            _changedFiles.RemoveAt(index);

        _selectedIndices.Clear();
    }


    // For testing you can add dummy files
    [MenuItem("Window/Changed Files/Add Dummy")]
    public static void AddDummy()
    {
        return;
        var window = GetWindow<SaveAndLoadCodeGenWindow>();
        return;
        var path = Application.dataPath + "/Test" + UnityEngine.Random.Range(0, 1000) + ".cs";
        var dto = new FileSystemEventArgsDto
        {
            ChangeType = WatcherChangeTypes.Changed,
            FullPath = path,
            Name = Path.GetFileName(path)
        };
        window._changedFiles.Add(dto);
        window.Repaint();
    }



    public CodeGenUtils.Configuration _codeGenConfig = CodeGenUtils.CreateDefaultConfig();

    public CodeGenUtils.Configuration ToCodeGenConfig(SaveAndLoadCodeGenSettings userSettings)
    {
        if(userSettings == null) return _codeGenConfig;

        _codeGenConfig.IgnoreAnyObsolete = userSettings.IgnoreAnyObsolete;
        return _codeGenConfig;
    }
}


[Serializable]
public class FileSystemEventArgsDto
{
    public WatcherChangeTypes ChangeType;
    public string FullPath;
    public string Name;
}



public static class FileSystemEventArgsExtensions
{
    public static FileSystemEventArgsDto ToDto(this FileSystemEventArgs e)
    {
        return new FileSystemEventArgsDto
        {
            ChangeType = e.ChangeType,
            FullPath = e.FullPath,
            Name = e.Name
        };
    }

    //public static FileSystemEventArgs FromDto(this FileSystemEventArgsDto dto)
    //{
    //    return new FileSystemEventArgs(
    //        (WatcherChangeTypes)Enum.Parse(typeof(WatcherChangeTypes), dto.ChangeType),
    //        Path.GetDirectoryName(dto.FullPath),
    //        dto.Name);
    //}
}
