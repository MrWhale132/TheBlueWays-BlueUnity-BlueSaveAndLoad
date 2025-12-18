using System.Collections.Generic;
using Assets._Project.Scripts.UtilScripts.Extensions;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
//using UnityEngine.WSA;

public class BulkAssetEditor : EditorWindow
{
    public AddressableDb.Service _addressableDbService = new();


    string _selectedPath = "<none>";
    string prefixOrSuffix = "NEW_";
    bool usePrefix = true;
    Vector2 _scrollPosFoundAssetsWithSameName;

    public bool _lockSelectedPathToggle = false;

    public Dictionary<string, List<string>> _foundAssetPathListsWithSameNameByName = new();



    [MenuItem("Window/Bulk Asset Editor")]
    public static void ShowWindow()
    {
        GetWindow<BulkAssetEditor>("Bulk Asset Editor").minSize = new Vector2(420, 200);
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }


    void OnGUI()
    {
        EditorGUILayout.LabelField("Batch Rename (Files & Sub-Assets)", EditorStyles.boldLabel);
        EditorGUILayout.Space();


        EditorGUILayout.LabelField("Selected (Project) path:", EditorStyles.label);
        if (!_lockSelectedPathToggle)
        {
            var selection = Selection.activeObject;
            string selectedPath = selection == null ? "<none>" : AssetDatabase.GetAssetPath(selection);

            if (ValidateSelectionFolder(selectedPath))
                _selectedPath = selectedPath;
        }
        EditorGUILayout.SelectableLabel(_selectedPath, GUILayout.Height(16));

        EditorGUILayout.BeginHorizontal();

        _lockSelectedPathToggle = EditorGUILayout.ToggleLeft("Lock selected path", _lockSelectedPathToggle, GUILayout.Width(140));

        if (GUILayout.Button("Select Assets Folder", GUILayout.Width(200)))
        {
            _selectedPath = "Assets";
        }

        EditorGUILayout.EndHorizontal();



        EditorGUILayout.Space();



        prefixOrSuffix = EditorGUILayout.TextField("Prefix / Suffix text", prefixOrSuffix);
        usePrefix = EditorGUILayout.ToggleLeft("Apply as prefix (unchecked = suffix)", usePrefix);

        EditorGUILayout.Space();


        if (GUILayout.Button("Find assets with same name"))
        {
            var assetPaths = FindSuitableAssetsInFolder(_selectedPath).ToList();

            var assetsByName = assetPaths.GroupBy(path => _addressableDbService.GetExtendedAssetName(path))
                                         .Where(g => g.Count() > 1)
                                         .ToDictionary(g => g.Key, g => g.ToList());

            _foundAssetPathListsWithSameNameByName = assetsByName;
        }


        GUILayout.Space(10);


        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Rename Files"))
        {
            if (ValidateSelectionFolder(_selectedPath))
            {
                if (!string.IsNullOrEmpty(prefixOrSuffix))
                    RenameFilesWithSameName();
                else
                { Debug.Log("Cancelled: empty text."); }
            }
            else EditorUtility.DisplayDialog("Select Folder", "Please select a folder in the Project window first.", "OK");
        }

        if (GUILayout.Button("Rename Sub-Assets In Folder"))
        {
            if (ValidateSelectionFolder(_selectedPath))
            {
                if (!string.IsNullOrEmpty(prefixOrSuffix))
                    RenameSubAssetsInFolder(_selectedPath, prefixOrSuffix, usePrefix);
                else { Debug.Log("Cancelled: empty text."); }
            }
            else EditorUtility.DisplayDialog("Select Folder", "Please select a folder in the Project window first.", "OK");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Select a folder in the Project window and click one of the buttons.\n" +
                                "Rename Files will rename asset files (preserving extensions).\n" +
                                "Rename Sub-Assets will rename embedded objects inside assets (e.g. meshes inside FBX).",
                                MessageType.Info);

        EditorGUILayout.Space();
        if (GUILayout.Button("Refresh Project Window"))
            AssetDatabase.Refresh();


        EditorGUILayout.Space();


        if (_foundAssetPathListsWithSameNameByName.IsNotNullAndNotEmpty())
        {
            _scrollPosFoundAssetsWithSameName = EditorGUILayout.BeginScrollView(_scrollPosFoundAssetsWithSameName, GUILayout.Height(300));

            EditorGUILayout.LabelField($"Found {_foundAssetPathListsWithSameNameByName.Count} asset names with multiple assets:", EditorStyles.boldLabel);

            foreach (var kvp in _foundAssetPathListsWithSameNameByName)
            {
                string name = kvp.Key;
                List<string> paths = kvp.Value;

                EditorGUILayout.LabelField($"Name: {name} ({paths.Count} assets)", EditorStyles.label);
                foreach (string path in paths)
                {
                    var style = new GUIStyle("Button")
                    {
                        alignment = TextAnchor.MiddleLeft
                    };

                    //EditorGUILayout.SelectableLabel("    " + path, GUILayout.Height(16));
                    if (GUILayout.Button(path, style, GUILayout.Width(400)))
                    {
                        FocusAsset(path);
                    }
                }
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndScrollView();
        }
    }

    bool ValidateSelectionFolder(string selectedPath)
    {
        if (string.IsNullOrEmpty(selectedPath) || !AssetDatabase.IsValidFolder(selectedPath))
        {
            //EditorUtility.DisplayDialog("Select Folder", "Please select a folder in the Project window first.", "OK");
            return false;
        }
        return true;
    }




    void RenameFilesWithSameName()
    {
        foreach ((var name, var paths) in _foundAssetPathListsWithSameNameByName)
        {
            Debug.Log($"Renaming {paths.Count} files named {name}");

            foreach (var path in paths)
            {
                //Debug.Log($" - {path}");

                string newName = name + "__" + Random.Range(1000, 9999);

                AssetDatabase.RenameAsset(path, newName);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Bulk rename of files completed.");
    }



    void RenameFilesInFolder(string selectedPath, string text, bool addPrefix)
    {
        int renamedCount = 0;
        // Use AssetDatabase to find assets; using Directory.GetFiles works too but ensure path mapping.
        string physicalPath = Path.GetFullPath(selectedPath);

        foreach (string filePath in Directory.GetFiles(physicalPath, "*", SearchOption.AllDirectories))
        {
            if (filePath.EndsWith(".meta")) continue;

            // Convert to AssetDatabase path form
            string relativeAssetPath = filePath.Replace("\\", "/");
            // convert physical full path to "Assets/..." path
            // physicalPath may contain project folder, so normalize.
            string projectPath = Application.dataPath.Replace("\\", "/");
            if (relativeAssetPath.StartsWith(projectPath))
            {
                relativeAssetPath = "Assets" + relativeAssetPath.Substring(projectPath.Length);
            }

            string dir = Path.GetDirectoryName(relativeAssetPath).Replace("\\", "/");
            string fileName = Path.GetFileNameWithoutExtension(relativeAssetPath);
            string ext = Path.GetExtension(relativeAssetPath);

            string newName = addPrefix ? text + fileName : fileName + text;
            string newPath = Path.Combine(dir, newName + ext).Replace("\\", "/");

            string error = AssetDatabase.MoveAsset(relativeAssetPath, newPath);
            if (string.IsNullOrEmpty(error))
                renamedCount++;
            else
                Debug.LogWarning($"Failed to rename {relativeAssetPath}: {error}");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Renamed {renamedCount} asset files under {selectedPath}");
    }


    public Object[] LoadRealSubAssetsOnly(string path)
    {
        var subassets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);

        return System.Array.FindAll(subassets, IsAsset);
    }

    public bool IsAsset(Object obj)
    {
        return /*obj.GetType() != typeof(GameObject) && */!typeof(Component).IsAssignableFrom(obj.GetType());
    }



    void RenameSubAssetsInFolder(string selectedPath, string text, bool addPrefix)
    {
        int renamedCount = 0;

        //string[] guids = AssetDatabase.FindAssets("t:Model", new[] { selectedPath }); // many sub-assets are inside models

        // Also scan all files in folder for any subassets
        var files = Directory.GetFiles(Path.GetFullPath(selectedPath), "*", SearchOption.AllDirectories);
        foreach (var f in files)
        {
            if (f.EndsWith(".meta")) continue;
            string assetPath = f.Replace("\\", "/");
            string projectPath = Application.dataPath.Replace("\\", "/");
            if (assetPath.StartsWith(projectPath))
            {
                assetPath = "Assets" + assetPath.Substring(projectPath.Length);
            }

            //Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
            Object[] subAssets = LoadRealSubAssetsOnly(assetPath);
            if (subAssets == null || subAssets.Length == 0) continue;

            Debug.Log("subassets found in " + assetPath + ": " + subAssets.Length);
            Debug.Log(AssetDatabase.GetMainAssetTypeAtPath(assetPath));


            Object main = null;
            foreach (Object subAsset in subAssets)
            {
                //if (AssetDatabase.IsMainAsset(subAsset))
                //{
                //    main = subAsset;
                //    //AssetDatabase.AddObjectToAsset(new AnimationClip(), assetPath);
                //    continue;
                //};
                //if (subAsset == null) continue;

                if (subAsset is GameObject go)
                {
                    Debug.LogError(go.gameObject + " at " + AssetDatabase.GetAssetPath(subAsset));
                    continue;
                }
                Debug.Log($"Considering sub-asset: {subAsset.name} ({subAsset.GetType().Name})");
                //continue;
                //string subAssetPath = AssetDatabase.GetAssetPath(subAsset);

                //Debug.Log($"Sub-asset path: {subAssetPath}");

                //continue;
                string oldName = subAsset.name;
                string newName = addPrefix ? text + oldName : oldName + text;
                newName = ReplaceIllegalCharacters(newName);

                //if(subAsset is MeshFilter mf)
                //{
                //    mf.sharedMesh.name = newName;
                //}

                //if (oldName == newName) continue;

                // Changing the subAsset.name and marking dirty is the proper way.
                //subAsset.name = newName;
                //EditorUtility.SetDirty(subAsset);
                //AssetDatabase.RenameAsset(assetPath, newName);
                //subAsset.name = newName;
                //EditorUtility.SetDirty(subAsset);

                var dir = Path.GetDirectoryName(assetPath);
                var newAssetPath = Path.Combine(dir, newName + ".asset").Replace("\\", "/");



                if (AssetDatabase.AssetPathExists(newAssetPath))
                {
                    Debug.LogWarning($"Skipped creating sub-asset {oldName} from {assetPath} to {newAssetPath}: target already exists.");
                    continue;
                }
                Object copy = Object.Instantiate(subAsset);
                AssetDatabase.CreateAsset(copy, newAssetPath);
                //if (!string.IsNullOrEmpty(error))
                //{
                //    Debug.LogWarning($"Failed to extract sub-asset {oldName} from {assetPath} to {newAssetPath}: {error}");
                //    continue;
                //}
                renamedCount++;
            }
            //EditorUtility.SetDirty(main);
            //AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Renamed {renamedCount} sub-assets under {selectedPath}");
    }




    public IEnumerable<string> FindSuitableAssetsInFolder(string folderPath)
    {
        // 1. Find all assets in that folder
        string[] allGuids = AssetDatabase.FindAssets("", new[] { folderPath });

        string[] excluded = AssetDatabase.FindAssets("t:folder t:script t:ScriptableObject t:TextAsset", new[] { folderPath });

        HashSet<string> models = AssetDatabase.FindAssets("t:Model", new[] { folderPath }).ToHashSet();


        var filtered = allGuids.Except(excluded);

        Debug.Log($"Found {filtered.Count()} suitable assets in {folderPath}:");

        foreach (var guid in filtered)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            string fileName = Path.GetFileName(path);

            var type = AssetDatabase.GetMainAssetTypeAtPath(path);

            if (type == typeof(UnityEngine.AI.NavMeshData))
            {
                //Debug.Log($"Skipping {path} of type {type}");
                continue;
            }

            //if (!path.StartsWith("Assets/_Project")) continue;

            if (!models.Contains(guid))
                yield return path;
            //Debug.Log(path);


            var subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
            foreach (var asset in subAssets)
            {
                yield return $"{path}@{asset.name}.{asset.GetType().Name}";
            }
        }
    }


    private void FocusAsset(string path)
    {
        var parts = path.Split('@');
        string mainAssetPath = parts[0];

        Object asset = AssetDatabase.LoadAssetAtPath<Object>(mainAssetPath);

        if (parts.Length == 2)
        {
            var subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(mainAssetPath);
            string subAssetNameAndType = parts[1];
            int dotIndex = subAssetNameAndType.LastIndexOf('.');
            var nameAndType = new string[]
            {
                subAssetNameAndType.Substring(0, dotIndex),
                subAssetNameAndType.Substring(dotIndex + 1)
            };

            asset = subAssets.FirstOrDefault(sub => sub.name == nameAndType[0] && sub.GetType().Name == nameAndType[1]);
        }

        if (asset == null)
        {
            Debug.LogWarning($"Asset not found at path: {path}");
            return;
        }

        // Ping it (highlights the asset)
        EditorGUIUtility.PingObject(asset);

        // Also select it (focus in Project window)
        Selection.activeObject = asset;

        // Force Project window to show it (optional)
        EditorApplication.ExecuteMenuItem("Window/General/Project");
    }



    public string ReplaceIllegalCharacters(string fileName, string replacement = "__")
    {
        char[] illegalChars = Path.GetInvalidFileNameChars();

        foreach (char c in illegalChars)
        {
            fileName = fileName.Replace(c.ToString(), replacement);
        }
        return fileName;
    }
}
