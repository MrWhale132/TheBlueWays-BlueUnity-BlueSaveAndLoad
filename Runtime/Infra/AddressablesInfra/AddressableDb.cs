
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.Addressables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading;
using Object = UnityEngine.Object;






#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
#endif


namespace Assets._Project.Scripts.Infrastructure.AddressableInfra
{
    public class AddressableDb : MonoBehaviour
    {
        [Serializable]
        public class AddressableDTO
        {
            public RandomId id;
            public string address;
            public string typedAddress ="";
            public string assetName;
        }

        public List<Object> _unityBuiltInResources;


        [Serializable]
        public class DataBase
        {
            public List<AddressableDTO> _addressables = new();

            [Newtonsoft.Json.JsonIgnore]
            public Dictionary<RandomId, AddressableDTO> _id;
            [Newtonsoft.Json.JsonIgnore]
            public Dictionary<string, AddressableDTO> _assetName;


            [Newtonsoft.Json.JsonIgnore]
            public bool Dirty { get; set; }


            public void BuildIndexes()
            {
                if (_addressables == null)
                {
                    Debug.LogError("Addressables list is null. Cannot build indexes.");
                    return;
                }
                if (_id != null)
                {
                    return;
                }

                _id = new Dictionary<RandomId, AddressableDTO>();
                _assetName = new Dictionary<string, AddressableDTO>();

                foreach (var entry in _addressables)
                {
                    if (!_id.ContainsKey(entry.id))
                    {
                        _id.Add(entry.id, entry);
                    }
                    else
                    {
                        Debug.LogError($"Duplicate ID found: {entry.id}. Skipping addition.");
                    }

                    if (!_assetName.ContainsKey(entry.assetName))
                    {
                        _assetName.Add(entry.assetName, entry);
                    }
                    else
                    {
                        Debug.LogError($"Duplicate asset name found: {entry.assetName}. Skipping addition.");
                    }
                }
            }


            public void Add(AddressableDTO dto)
            {
                if (_addressables.Contains(dto))
                {
                    Debug.LogError($"AddressableDb: Addressable with ID '{dto.id}' and asset name '{dto.assetName}' already exists. Skipping addition.");
                    return;
                }
                if (_id.ContainsKey(dto.id))
                {
                    Debug.LogError($"AddressableDb: Addressable with ID '{dto.id}' already exists. Skipping addition.");
                    return;
                }
                if (_assetName.TryGetValue(dto.assetName, out var existingAddressable))
                {
                    Debug.LogError($"Cant add addressable entry with name '{dto.assetName}' and address '{dto.typedAddress}' " +
                             $"because an other addressable had already been added with this asset name. " +
                             $"Already existing addressable: {existingAddressable.typedAddress}. " +
                             $"Skipping addition.");
                    return;
                }


                _addressables.Add(dto);
                _id.Add(dto.id, dto);
                _assetName.Add(dto.assetName, dto);

                Dirty = true;
            }


            public void Remove(AddressableDTO dto)
            {
                if (_addressables.Contains(dto))
                {
                    _addressables.Remove(dto);
                    _id.Remove(dto.id);
                    _assetName.Remove(dto.assetName);

                    Dirty = true;
                }
                else
                {
                    Debug.LogError($"AddressableDb: Addressable with ID '{dto.id}' and asset name '{dto.assetName}' does not exist. Cannot remove." +
                        $"Skipping removal.");
                }
            }
        }







#if UNITY_EDITOR

        public AddressableDTO ToDTO(AddressableAssetEntry entry)
        {
            var assetName = _service.GetExtendedAssetName(entry.AssetName(), entry.MainAssetType);
            return new AddressableDTO
            {
                id = RandomId.Get(),
                address = entry.address,
                typedAddress = _service.GetTypedAddress(entry),
                assetName = assetName,
            };
        }




        [Tooltip("If true, will reuse existing IDs for addressables with the same asset name. " +
            "Can be used when the path of assets changed but their names not.")]

        public bool _reuseIds = true;
        public bool _includeSubAssetsToo = false;


        public void Refresh()
        {
            Init();

            var entries = AddressableUtils.GetAllAddressableEntries();
            //var test = entries.Find(e => e.address == "Assets/FreeCharacter/FreeCharacter_Mecanim.FBX");

            if (_includeSubAssetsToo)
            {
                List<AddressableAssetEntry> subs = new List<AddressableAssetEntry>();
                foreach (var entry in entries)
                {
                    entry.GatherAllAssets(subs, false, true, true);
                }
                entries.AddRange(subs);
            }
            

            List<AddressableAssetEntry> distinct = new List<AddressableAssetEntry>();
            Dictionary<string, AddressableAssetEntry> byAddress = new Dictionary<string, AddressableAssetEntry>();
            Dictionary<string, AddressableAssetEntry> byAssetName = new Dictionary<string, AddressableAssetEntry>();


            foreach (var entry in entries)
            {
                string assetName;

                assetName = _service.GetExtendedAssetName(entry.AssetName(), entry.MainAssetType);

                if (byAssetName.TryGetValue(assetName, out AddressableAssetEntry existing))
                {
                    Debug.LogError($"AddressableDb: Two addressable with the same asset name found:" +
                        $"{existing.address}, \n" +
                        $"{entry.address} " +
                        $"Please make sure that each addressable has a uniqe asset name.");
                }
                else
                {
                    distinct.Add(entry);
                    byAddress.Add(_service.GetTypedAddress(entry), entry);
                    byAssetName.Add(assetName, entry);
                }
            }



            foreach (var entry in distinct)
            {
                string assetName = _service.GetExtendedAssetName(entry.AssetName(), entry.MainAssetType);
                string typedAddress = _service.GetTypedAddress(entry);

                RandomId id;


                if (__db._assetName.TryGetValue(assetName, out var dto))
                {
                    bool isSame = dto.typedAddress == typedAddress;
                    if (isSame) continue;


                    bool stillExists = byAddress.ContainsKey(dto.typedAddress);

                    if (stillExists)
                    {
                        Debug.LogError($"AddressableDb: Addressable with asset name '{assetName}' already exists with address '{dto.typedAddress}'. " +
                            $"Skipping addition.");
                        continue;
                    }
                    //TODO: additional checks if the asset type is the same, etc.
                    //For example, allow reassign only if the asset type is the same as the existing one.
                    __db.Remove(dto);

                    Debug.Log($"TRACE: Removed unused addressable DTO with asset name '{assetName}' and address '{dto.typedAddress}'.");


                    if (_reuseIds)
                    {
                        Debug.Log($"TRACE: Reusing ID for addressable with asset name '{assetName}' and address '{typedAddress}'. " +
                            $"Old address: {dto.typedAddress}, New address: {typedAddress} " +
                            $"ID: {dto.id}.");

                        id = dto.id;
                    }
                    else
                    {
                        id = RandomId.Get();

                        Debug.Log($"TRACE: Reusing IDs for the same asset name is false so going to create a new one." +
                            $"Old addressable with same name: {dto.typedAddress}, New addressable with same name: {typedAddress}" +
                            $"New ID: {id}");
                    }
                }
                else id = RandomId.Get();



                var newDto = ToDTO(entry);
                newDto.id = id;

                __db.Add(newDto);

                Debug.Log($"TRACE: Added addressable DTO with asset name '{assetName}' and address '{typedAddress}'. " +
                    $"ID: {newDto.id}.");
            }





            foreach (var dbEntry in __db._addressables.ToList())
            {
                if (!byAddress.ContainsKey(dbEntry.typedAddress))
                {
                    Debug.Log($"TRACE: AddressableDb: Removing Addressable with asset name '{dbEntry.assetName}' and address '{dbEntry.typedAddress}' " +
                        $"does not exist in the current addressables. Going to remove it from the database.");

                    __db.Remove(dbEntry);
                }
            }



            _reuseIds = true;

            Debug.Log($"AddressableDb: Refresh completed.");

            if (__db.Dirty)
            {
                Debug.Log($"AddressableDb: Changes detected. Going to save to disk.");
                SaveToDisk();
                __db.Dirty = false;
            }
            else
            {
                Debug.Log($"AddressableDb: No changes detected. No need to save to disk.");
            }
        }


#endif

        public static AddressableDb Singleton { get; private set; }

        //[NonSerialized] //this is the way to drop the db
        public DataBase __db;

        public Service _service = new Service();



        private void Awake()
        {
            if (Singleton != null && Singleton != this)
            {
                DebugUtil.LogFatal("AddressableDb: Singleton instance already exists. Destroying the new instance.");
                Destroy(gameObject);
                return;
            }

            Singleton = this;
            DontDestroyOnLoad(gameObject);


            Init();
        }


        public void Init()
        {
            __db.BuildIndexes();

            CreateUnityBuiltInResourceLookUp();
            LoadInUnityBuiltInResourceObjectIds();
            UpdateUnityBuiltInResourceObjectIdsIfNeeded();
        }





        public Dictionary<string, Object> _unityBuiltInResourcesByExtendedName = new();
        public Dictionary<RandomId, string> _unityBuiltInResourceObjectIdsToExtendedNameMap = new();
        public Dictionary<string, RandomId> _unityBuiltInResourceExtendedNamesToObjectIdsMap = new();




        public void UpdateUnityBuiltInResourceObjectIdsIfNeeded()
        {
            bool updateNeeded = false;

            var knownAssetIdsByName = _unityBuiltInResourceObjectIdsToExtendedNameMap.Values.ToHashSet();

            foreach (string name in _unityBuiltInResourcesByExtendedName.Keys)
            {
                if (!knownAssetIdsByName.Contains(name))
                {
                    updateNeeded = true;
                    _unityBuiltInResourceObjectIdsToExtendedNameMap.Add(RandomId.Get(), name);
                }
            }


            List<RandomId> idsToRemove = new List<RandomId>();

            foreach ((var id, string name) in _unityBuiltInResourceObjectIdsToExtendedNameMap)
            {
                if (!_unityBuiltInResourcesByExtendedName.ContainsKey(name))
                {
                    updateNeeded = true;
                    idsToRemove.Add(id);
                    Debug.Log($"AddressableDb: Unity built-in resource with name '{name}' not found in the resources dictionary. Removing its ID entry.");
                }
            }

            foreach (var id in idsToRemove)
            {
                _unityBuiltInResourceObjectIdsToExtendedNameMap.Remove(id);
            }


            if (updateNeeded)
            {
                using var fileStream = File.Open("UnityBuiltInResourceObjectIds.json", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                using var writer = new StreamWriter(fileStream);

                var json = JsonConvert.SerializeObject(_unityBuiltInResourceObjectIdsToExtendedNameMap.ToList(), Formatting.Indented);

                writer.Write(json);
            }
        }


        public void CreateUnityBuiltInResourceLookUp()
        {
            _unityBuiltInResourcesByExtendedName.Clear();

            foreach (var obj in _unityBuiltInResources)
            {
                if (obj == null)
                {
                    Debug.LogWarning("AddressableDb: Null object found in Unity built-in resources list. Skipping.");
                    continue;
                };


                string extendedName = GetExtendedAssetName(obj);

                if (_unityBuiltInResourcesByExtendedName.ContainsKey(extendedName))
                {
                    Debug.LogError($"AddressableDb: Duplicate Unity built-in resource extended name found: {extendedName}. Skipping addition.");
                    continue;
                }

                _unityBuiltInResourcesByExtendedName.Add(extendedName, obj);
            }
        }


        public string GetExtendedAssetName(Object asset)
        {
            return _service.GetExtendedAssetName(asset);
        }


        public void LoadInUnityBuiltInResourceObjectIds()
        {
            using var fileStream = File.Open("UnityBuiltInResourceObjectIds.json", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            using var reader = new StreamReader(fileStream);

            var json = reader.ReadToEnd();

            var keyvaluePairs = JsonConvert.DeserializeObject<List<KeyValuePair<RandomId, string>>>(json) ?? new();

            _unityBuiltInResourceObjectIdsToExtendedNameMap = keyvaluePairs.ToDictionary(kv => kv.Key, kv => kv.Value);
            _unityBuiltInResourceExtendedNamesToObjectIdsMap = keyvaluePairs.ToDictionary(kv => kv.Value, kv => kv.Key);
        }




        public class Service
        {

#if UNITY_EDITOR
            public string GetTypedAddress(AddressableAssetEntry entry)
            {
                return $"{entry.address}.{entry.MainAssetType.Name}";
            }
#endif


            public string GetExtendedAssetName(Object asset)
            {
                return GetExtendedAssetName(asset.name, asset.GetType());
            }

            public string GetExtendedAssetName(string name, Type type)
            {
                return $"{name} ({type.Name})";
            }

#if UNITY_EDITOR
            public string GetExtendedAssetName(string assetPath)
            {
                var parts = assetPath.Split('@');
                string mainAssetPath = parts[0];

                Object asset = AssetDatabase.LoadAssetAtPath<Object>(mainAssetPath);

                if (parts.Length == 2)
                {
                    var subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(mainAssetPath);
                    string subAssetNameAndType = parts[1];

                    int lastIndex = subAssetNameAndType.LastIndexOf('.');

                    var nameAndType = new string[]
                    {
                        subAssetNameAndType.Substring(0, lastIndex),
                        subAssetNameAndType.Substring(lastIndex + 1)
                    };

                    asset = subAssets.FirstOrDefault(sub => sub.name == nameAndType[0] && sub.GetType().Name == nameAndType[1]);
                }

                if (asset == null)
                {
                    Debug.LogError($"AddressableDb: Could not load asset at path: {assetPath}. Returning empty extended name.");
                    return string.Empty;
                }
                return GetExtendedAssetName(asset);
            }
#endif
        }





        public RandomId GetAssetIdByAssetName(UnityEngine.Object unityObj)
        {
            if (unityObj == null) return RandomId.Default;

            string extendedName = GetExtendedAssetName(unityObj);

            if (_unityBuiltInResourceExtendedNamesToObjectIdsMap.TryGetValue(extendedName, out var id2))
            {
                return id2;
            }


            RandomId id = GetIdByAssetName(extendedName);

            if (id.IsDefault && !string.IsNullOrEmpty(unityObj.name))
            {
                if (unityObj.name != "Default UI Material"
                && (!unityObj.name.EndsWith(" Instance", System.StringComparison.Ordinal))
                && (!unityObj.name.EndsWith(" (Instance)", System.StringComparison.Ordinal))
                )
                {
                    Debug.LogWarning($"AddressableDb: No ID found for asset type {unityObj.GetType().FullName} with name {unityObj.name}. Going to return a default value.");
                }
            }

            return id;
        }

        public RandomId GetIdByAssetName(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                return RandomId.Default;
            }

            //if (assetName.EndsWith(" Instance", System.StringComparison.Ordinal))
            //{
            //    assetName = assetName.Substring(0, assetName.Length - " Instance".Length);
            //}
            //else if (assetName.EndsWith(" (Instance)", System.StringComparison.Ordinal))
            //{
            //    assetName = assetName.Substring(0, assetName.Length - " (Instance)".Length);
            //}


            if (__db._assetName.TryGetValue(assetName, out var dto))
            {
                return dto.id;
            }
            else
            {
                //Debug.LogWarning($"AddressableDb: No ID found for asset name {assetName}. Going to return a default value.");
                return RandomId.Default;
            }
        }






        private string GetAddressById(RandomId id)
        {
            if (id.IsDefault)
            {
                //Debug.LogWarning("AddressableDb: Attempted to get address by default ID. Returning null.");
                return null;
            }


            if (__db._id.TryGetValue(id, out var dto))
            {
                return dto.address;
            }
            else
            {
                Debug.LogWarning($"AddressableDb: No address found for ID {id}. Going to return a default value.");
                return null;
            }
        }


        public T GetAssetById<T>(RandomId id) where T : UnityEngine.Object
        {
            if (_unityBuiltInResourceObjectIdsToExtendedNameMap.TryGetValue(id, out var extendedName))
            {
                if (_unityBuiltInResourcesByExtendedName.TryGetValue(extendedName, out var obj))
                {
                    if (obj is T tObj)
                    {
                        return tObj;
                    }
                    else
                    {
                        Debug.LogError($"AddressableDb: Unity built-in resource with ID {id} and extended name '{extendedName}' is not of type {typeof(T).FullName}. It is of type {obj.GetType().FullName}. Returning null.");
                        return null;
                    }
                }
                else
                {
                    Debug.LogError($"AddressableDb: Unity built-in resource with ID {id} and extended name '{extendedName}' not found in the resources dictionary. Returning null.");
                    return null;
                }
            }



            string address = GetAddressById(id);

            if (string.IsNullOrEmpty(address))
            {
                return default;
            }


            if (address == null)
            {
                Debug.LogError($"AddressableDb: No address found for ID {id}. Cannot get asset.");
                return null;
            }
            new ManualResetEvent(false);

            return Addressables.LoadAssetAsync<T>(address).WaitForCompletion();
            //return AddressablesHelper.SafeLoadAssetBlocking<T>(address);


            var handle = Addressables.LoadAssetAsync<T>(address);

            while (!handle.IsDone) { }

            //handle.Completed += op =>
            {
                if (handle.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"AddressableDb: Failed to load asset of type {typeof(T).FullName} at address '{address}' for ID {id}. exception: {handle.OperationException}");
                }
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    return handle.Result;
                }
                else return default;
            }
        }


        public T GetAssetByIdOrFallback<T>(in T fallbackAsset, ref RandomId id) where T : UnityEngine.Object
        {
            if (id.IsDefault)
            {
                return fallbackAsset;
            }

            return GetAssetById<T>(id);
        }




        public bool IsAssetType(Type type)
        {
            return typeof(UnityEngine.Object).IsAssignableFrom(type)
               && !typeof(UnityEngine.Component).IsAssignableFrom(type)
               && !typeof(UnityEngine.GameObject).IsAssignableFrom(type)
               && !typeof(UnityEngine.ScriptableObject).IsAssignableFrom(type);
        }




        //not working, delete somewhen
        public static class AddressablesHelper
        {
            public static T SafeLoadAssetBlocking<T>(object key) where T : UnityEngine.Object
            {
                var handle = Addressables.LoadAssetAsync<T>(key);

                // Use a manual reset event to block until the callback fires
                using var waitHandle = new ManualResetEvent(false);

                T result = null;
                Exception exception = null;

                handle.Completed += op =>
                {
                    if (op.Status == AsyncOperationStatus.Succeeded)
                    {
                        result = op.Result;
                    }
                    else
                    {
                        exception = op.OperationException ?? new System.Exception("Addressable load failed with unknown reason.");
                    }

                    waitHandle.Set();
                };

                // Wait until the operation completes
                waitHandle.WaitOne();

                if (exception != null)
                {
                    Debug.LogError($"[Addressables] Failed to load asset '{key}': {exception}");
                    return null;
                }

                return result;
            }
        }



        //dont want to have dependency on SaintsField in package. May want to implement our own.
        //[SaintsField.ReadOnly]
        public int __majorVersion;
        //[SaintsField.ReadOnly]
        public int __minorVersion;
        //[SaintsField.ReadOnly]
        public int __patchVersion;

        public string _savePathRelativeToAssetsFolder;
        public string _saveNameBase;



        public void SaveToDisk()
        {
            int major = __majorVersion;
            int minor = __minorVersion;
            int patch = __patchVersion;

            if (__db.Dirty)
            {
                minor++;
                patch = 0;
            }
            else patch++;

            string version = $"v{major}.{minor}.{patch}";


            var metadata = new AddressableDataBaseMetaData
            {
                version = version,
                creationDate = DateTime.UtcNow
            };
            var data = new AddressableDataBasePersistenData
            {
                metaData = metadata,
                db = __db
            };

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);


            string dirPath = Path.Combine(Application.dataPath, _savePathRelativeToAssetsFolder);

            string fileName = $"{_saveNameBase}_{version}.json";

            string path = Path.Combine(dirPath, fileName);


            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                if (File.Exists(path))
                {
                    Debug.LogError($"AddressableDb: An addressable db save file already exists at {path}." +
                        $"The save process is canceled. Do something with that file then comeback and trigger the flow again with some change, for example by adding some dummy asset.");
                    return;
                }


                File.WriteAllText(path, json);
                Debug.Log($"AddressableDb: Data saved to disk at {path}");

                __majorVersion = major;
                __minorVersion = minor;
                __patchVersion = patch;

                __db.Dirty = false; // Reset dirty flag after saving
            }
            catch (Exception e)
            {
                Debug.LogError($"AddressableDb: Failed to save data to disk. Error: {e.Message}");
            }
        }


        public class AddressableDataBasePersistenData
        {
            public AddressableDataBaseMetaData metaData;
            public DataBase db;
        }

        public class AddressableDataBaseMetaData
        {
            public string version;
            public DateTime creationDate;
        }
    }


    public static class AddressableExtensions
    {
#if UNITY_EDITOR
        public static string AssetName(this AddressableAssetEntry entry)
        {
            if (string.IsNullOrEmpty(entry.AssetPath))
            {
                var start = entry.address.IndexOf('[') + 1;
                var end = entry.address.IndexOf(']');
                var name = entry.address.Substring(start, end - start);
                return name;
            }
            return Path.GetFileNameWithoutExtension(entry.address);
        }
#endif
    }
}
