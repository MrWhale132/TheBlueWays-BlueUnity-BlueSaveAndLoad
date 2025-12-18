
using Assets._Project.Scripts.SaveAndLoad;
using Packages.com.theblueway.saveandload.Editor.SaveAndLoad.HandledTypeNameSearchFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Packages.com.theblueway.saveandload.Editor.SaveAndLoad
{

    [CreateAssetMenu(fileName = "SaveHandlerTypeGenerationConfig", menuName = "Scriptable Objects/SaveAndLoad/SaveHandlerTypeGenerationConfig")]
    public class SaveHandlerTypeGenerationConfigSO : UnityEngine.ScriptableObject
    {
        public SaveHandlerTypeGenerationConfig config;

        public void OnValidate()
        {
            config.CacheInvalidate();
            config.logContext = this;

            if (config._triggerValidate)
            {
                config._triggerValidate = false;
                config.IsValid(true, this);
            }
        }
    }

    [Serializable]
    public class SaveHandlerTypeGenerationConfig
    {
        public bool _triggerValidate;

        [HandledTypeSaveHandlerId]
        public long handlerIdOfConfiguredType;
        public List<MemberConfig> memberConfigs;

        [HideInInspector]
        public UnityEngine.Object logContext;


        public Dictionary<string, FieldInfo> _fieldInfoCache = new();
        public Dictionary<string, PropertyInfo> _propertyInfoCache = new();
        public Dictionary<string, MethodInfo> _methodInfoCache = new();
        public Dictionary<string, EventInfo> _eventInfoCache = new();

        [NonSerialized]
        public bool _cacheIsBuilt = false;


        public bool IsValid(bool logErrorMessages = false, UnityEngine.Object context = null)
        {
            bool isValid = true;

            Type configuredType = SaveAndLoadManager.Service_.GetHandledTypeByHandlerId(handlerIdOfConfiguredType);
            if (configuredType == null)
            {
                if (logErrorMessages)
                    Debug.LogError($"SaveHandlerTypeGenerationConfig: Configured type with handler ID {handlerIdOfConfiguredType} could not be found.", context);
                return false;
            }

            if (memberConfigs.Count > 0)
            {
                BuildCache();

                var checkedMemberNames = new HashSet<string>();

                foreach (var memberConfig in memberConfigs)
                {
                    if (memberConfig.methodId != 0)
                    {
                        if(memberConfig.inclusionMode is MemberInclusionMode.Exclude)
                        {
                            Debug.LogError($"Methods can not be excluded via UI by refering them by their methodid.");
                        }
                        continue;
                    }


                    if (checkedMemberNames.Contains(memberConfig.memberName))
                    {
                        if (logErrorMessages)
                            Debug.LogError($"SaveHandlerTypeGenerationConfig: Duplicate member name '{memberConfig.memberName}' found in configuration for type {configuredType.AssemblyQualifiedName} {handlerIdOfConfiguredType}.", context);
                        isValid = false;
                    }

                    checkedMemberNames.Add(memberConfig.memberName);


                    bool memberExists;

                    if (_fieldInfoCache.TryGetValue(memberConfig.memberName,out FieldInfo field))
                    {
                        memberExists = true;
                        memberConfig.MemberInfo = field;
                    }
                    else if(_propertyInfoCache.TryGetValue(memberConfig.memberName, out var property))
                    {
                        memberExists = true;
                        memberConfig.MemberInfo = property;
                    }
                    else if(_eventInfoCache.TryGetValue(memberConfig.memberName, out var evt))
                    {
                        memberExists = true;
                        memberConfig.MemberInfo = evt;
                    }
                    else
                    {
                        memberExists= false;
                    }
                    

                    if (!memberExists)
                    {
                        if (logErrorMessages)
                            Debug.LogError($"SaveHandlerTypeGenerationConfig: Member '{memberConfig.memberName}' does not exist in configured type {configuredType.AssemblyQualifiedName} {handlerIdOfConfiguredType}.", context);
                        isValid = false;
                    }
                }
            }

            //if (logErrorMessages)
            //{
            //    Debug.Log("Validation was " + (isValid ?"success":"failed"));
            //}

            return isValid;
        }


        public void BuildCache()
        {
            if (_cacheIsBuilt) return;

            Type configuredType = SaveAndLoadManager.Service_.GetHandledTypeByHandlerId(handlerIdOfConfiguredType);

            FieldInfo[] fields = configuredType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            PropertyInfo[] properties = configuredType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            MethodInfo[] methods = configuredType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            EventInfo[] events = configuredType.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var field in fields) _fieldInfoCache.Add(field.Name, field);
            foreach (var property in properties) _propertyInfoCache.Add(property.Name, property);
            //cant create dict because of method overloads. Should get the methodids instead.
            //foreach (var method in methods) _methodInfoCache.Add(method.Name, method);
            foreach (var ev in events) _eventInfoCache.Add(ev.Name, ev);

            _cacheIsBuilt = true;
        }

        public void CacheInvalidate()
        {
            _fieldInfoCache.Clear();
            _propertyInfoCache.Clear();
            _methodInfoCache.Clear();
            _eventInfoCache.Clear();

            _cacheIsBuilt = false;
        }


        [Serializable]
        public class MemberConfig
        {
            public string memberName;
            //public MemberType memberType = MemberType.Property;
            public MemberInclusionMode inclusionMode = MemberInclusionMode.Include;
            public string directive;
            public long methodId; //todo: add validation that this id exists

            public MemberInfo MemberInfo { get; set; }
        }
    }

    public enum MemberType
    {
        Field,
        Property,
        Method,
        Event,
    }
    public enum MemberInclusionMode
    {
        Include,
        Exclude,
    }
}