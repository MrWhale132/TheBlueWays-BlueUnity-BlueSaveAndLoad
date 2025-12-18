
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.Extensions;
using Newtonsoft.Json;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets._Project.Scripts.SaveAndLoad
{
    //todo, optimization: create an other ctor that accepts the "type" of T so it doesn't have to be checked every time
    public class Data<T>
    {
        public T _Value;
        public CustomSaveData<T> _SaveData;
        public RandomId _ObjectId;
        public RandomId _AssetId;
        public InvocationList _InvocationList;

        [JsonIgnore]
        public UnityEngine.Object _asset;

        [JsonIgnore]
        public Action<T> _setter;
        [JsonIgnore]
        public Func<T> _getter;

        [JsonIgnore]
        public T Value { get => Get(); set => Set(value); }

        public RandomId ReferencedBy { get; set; }

        public Data()
        {
            var type = typeof(T);


            if (AddressableDb.Singleton.IsAssetType(type))
            {
                _setter = (value) => _AssetId = AddressableDb.Singleton.GetAssetIdByAssetName((UnityEngine.Object)(object)value);
                _getter = () =>
                {
                    return (T)(object)AddressableDb.Singleton.GetAssetByIdOrFallback<Object>(null, ref _AssetId);
                };
            }
            else if (SaveAndLoadManager.Singleton.HasSaveHandlerForType(typeof(T))
                || typeof(T).IsInterface) //WARNING: HUGE, huge temporary solution is this. This assumes and requires that if an interface used as a 
                                          //generic type param, than only class type types implement it
            {
                _setter = (value) => _ObjectId = Infra.Singleton.GetObjectId(value, ReferencedBy,setLoadingOrder:true);
                _getter = () =>
                {
                    return Infra.Singleton.GetObjectById<T>(_ObjectId);
                };
            }
            //else if isAsset
            else if (typeof(Delegate).IsAssignableFrom(typeof(T)))
            {
                _setter = (value) => _InvocationList = Infra.Singleton.GetInvocationList(value as Delegate);
                _getter = () => Infra.Singleton.GetDelegate<T>(_InvocationList);
            }
            else if (SaveAndLoadManager.Singleton.HasCustomSaveData<T>(typeof(T), out var saveData))
            {
                _SaveData = saveData;
                _setter = (val) => _SaveData.ReadFrom(in val);
                _getter = () =>
                {
                    //T instance = default(T); //custom save datas designed to work with structs only, so this should be fine.
                    _SaveData.WriteTo(ref _Value);
                    return _Value;
                };
            }
            else
            {
                _setter = (value) =>
                {
                    if ((typeof(GameObject).IsAssignableFrom(typeof(T))))
                    {
                        Debug.LogError((value as GameObject).HierarchyPath());
                    }
                    _Value = value;
                };
                _getter = () => _Value != null ? _Value : default(T);
            }
        }
        public void Set(T value)
        {
            _setter(value);
        }
        public T Get()
        {
            return _getter();
        }
    }
}
