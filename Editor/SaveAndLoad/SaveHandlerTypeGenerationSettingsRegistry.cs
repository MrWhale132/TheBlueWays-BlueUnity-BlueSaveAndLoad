
using Assets._Project.Scripts.SaveAndLoad;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.Analytics;

namespace Packages.com.theblueway.saveandload.Editor.SaveAndLoad
{
    public class SaveHandlerTypeGenerationSettingsRegistry
    {
        public Dictionary<long, List<SaveHandlerTypeGenerationConfig>> _foundTypeGenConfigsByHandlerId = new();
        public Dictionary<long, SaveHandlerTypeGenerationSettings> _typeGenerationSettingsByHandlerId = new();
        public bool _isBuilt = false;


        public bool HasSettingsForHandledType(Type handledType, bool isStatic, out SaveHandlerTypeGenerationSettings settings)
        {
            BuildLookupIfNeeded();

            var handlerId = SaveAndLoadManager.Service_.GetHandlerIdByHandledType(handledType, isStatic);
            if(handledType.Name == "TankInputUser")
            {

            }
            if (!_typeGenerationSettingsByHandlerId.ContainsKey(handlerId))
            {
                if (isStatic)
                {
                    if (_foundTypeGenConfigsByHandlerId.TryGetValue(handlerId, out var configs))
                    {
                        var settings_ = new SaveHandlerTypeGenerationSettings(configs);
                        if (settings_.IsValid(true))
                        {
                            _typeGenerationSettingsByHandlerId.Add(handlerId, settings_);
                        }
                    }
                }
                else
                {
                    List<SaveHandlerTypeGenerationConfig> configs = new();

                    Type current = handledType;

                    while (current != null)
                    {
                        var id = SaveAndLoadManager.Service_.GetHandlerIdByHandledType(current, false);

                        if (_foundTypeGenConfigsByHandlerId.TryGetValue(id, out var configsForCurrentType))
                        {
                            configs.AddRange(configsForCurrentType);
                        }

                        current = current.BaseType;
                    }

                    if (configs.Count > 0)
                    {
                        var settings_ = new SaveHandlerTypeGenerationSettings(configs);
                        if (settings_.IsValid(true))
                        {
                            _typeGenerationSettingsByHandlerId.Add(handlerId, settings_);
                        }
                    }
                }
            }

            return _typeGenerationSettingsByHandlerId.TryGetValue(handlerId, out settings);
        }


        public void BuildLookupIfNeeded()
        {
            if (_isBuilt) return;

            _foundTypeGenConfigsByHandlerId.Clear();
            _typeGenerationSettingsByHandlerId.Clear();

            var type2 = typeof(SaveHandlerTypeGenerationConfigSO);

            var typeGenConfigSOs = AssetDatabase.FindAssets("t:" + type2.Name)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path => AssetDatabase.LoadAssetAtPath<SaveHandlerTypeGenerationConfigSO>(path))
                .Where(asset => asset != null);

            var byHandlerId = typeGenConfigSOs
                .GroupBy(so => so.config.handlerIdOfConfiguredType)
                .ToDictionary(g => g.Key, g => g.ToList());



            //foreach((var handlerId, var configs) in byHandlerId)
            //{
            //    Type handledType = SaveAndLoadManager.Service_.GetHandledTypeByHandlerId(handlerId, out bool isStatic);

            //    if (isStatic) continue;


            //    Type baseType = handledType.BaseType;

            //    while (baseType != null)
            //    {
            //        var idOfBaseType = SaveAndLoadManager.Service_.GetHandlerIdByHandledType(baseType,isStatic:false);

            //        if (byHandlerId.TryGetValue(idOfBaseType, out var baseConfigs))
            //        {
            //            byHandlerId[handlerId].AddRange(baseConfigs);
            //        }

            //        baseType = baseType.BaseType;
            //    }
            //}


            foreach ((var id, var configs) in byHandlerId)
            {
                var validOnes = new List<SaveHandlerTypeGenerationConfig>();

                foreach (var configSO in configs)
                {
                    if (configSO.config.IsValid(true, configSO))
                    {
                        validOnes.Add(configSO.config);
                    }
                }

                if (validOnes.Count > 0)
                {
                    _foundTypeGenConfigsByHandlerId.Add(id, validOnes);
                }
            }

            _isBuilt = true;
        }
    }
}
