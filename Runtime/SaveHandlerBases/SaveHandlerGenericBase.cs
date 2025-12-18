using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using Assets._Project.Scripts.UtilScripts;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases
{
    public class SaveHandlerGenericBase<TSavable, TSaveData> : SaveHandlerBase
        where TSaveData : SaveDataBase, new()
    {
        public TSavable __instance;
        public TSaveData __saveData;

        public SaveHandlerGenericBase()
        {

        }

        public SaveHandlerGenericBase(TSavable instance) : base()
        {
            _Init(instance);
        }

        public override ObjectMetaData MetaData => __saveData._MetaData_;
        public override int Order { get => __saveData._MetaData_.Order; set => __saveData._MetaData_.Order = value; }


        public override void Init(object instance)
        {
            base.Init(instance);

            _Init((TSavable)instance);
        }



        public void _Init(TSavable instance)
        {
            __instance = instance;
            __saveData = new TSaveData();

            _SetObjectId();


            __saveData._ObjectId_ = HandledObjectId;

            var handlerType = GetType();

            __saveData._MetaData_ = new()
            {
                SaveHandlerId = SaveHandlerId,
                IsGeneric = handlerType.IsGenericType,
                SaveHandlerType = handlerType.AssemblyQualifiedName,
                ObjectId = HandledObjectId,
                //Order = Order,
            };

            var attr = __attributeCache[GetType()];
            Order = attr.Order;

            __saveData._DataGroupId_ = DataGroupId;
            __saveData._AssemblyQualifiedName_ = instance.GetType().AssemblyQualifiedName;
        }


        public virtual void _SetObjectId()
        {
            HandledObjectId = Infra.Singleton.GetObjectId(__instance, Infra.Singleton.GlobalReferencing);

            if (HandledObjectId.IsDefault)
            {
                Debug.LogError(__instance.GetType().FullName);
            }
        }


        public virtual void _AssignInstance()
        {

        }


        public override void ReleaseObject()
        {
            //this boxes a new instance in case of value types
            //fix?: cache default values per type
            __instance = default;
        }


        public override string Serialize()
        {
            return JsonConvert.SerializeObject(__saveData);
        }

        public override void Deserialize(string json)
        {
            //var metaData = JsonConvert.DeserializeObject<SavedObject>(json);
            //Debug.Log(metaData.MetaData.ObjectId);
            __saveData = JsonConvert.DeserializeObject<TSaveData>(json);
        }


        /// <see cref="FieldInfo.GetValue(object)"/> requires an instance, that is why this helper is here and in the base class
        public InvocationList GetInvocationList(string eventName)
        {
            if (!__eventBackingFields.ContainsKey(eventName))
            {
                var fieldToAdd = HandledType.GetField(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
                if (fieldToAdd == null)
                {
                    Debug.LogError($"SaveHandlerBase: Could not find event backing field for event {eventName} in type {HandledType.Name}. ");
                    return null;
                }
                __eventBackingFields[eventName] = fieldToAdd;
            }

            var field = __eventBackingFields[eventName];
            var dlg = (Delegate)field.GetValue(__instance);

            return Infra.Singleton.GetInvocationList(dlg);
        }



        //these are quick fixes because dynamic object load order setting is not ready yet
        //todo
        public void GetAssetIdList(IEnumerable<UnityEngine.Object> objs, List<RandomId> ids)
        {
            ids.Clear();
            int i = 0;
            foreach (var obj in objs)
            {
                var material = obj;
                if (material != null)
                {
                    var assetId = AddressableDb.Singleton.GetAssetIdByAssetName(material);
                    ids.Add(assetId);
                }
                i++;
            }
        }

        public T[] GetAssetList<T>(List<RandomId> ids) where T : UnityEngine.Object
        {
            T[] assets = new T[ids.Count];

            for (int i = 0; i < ids.Count; i++)
            {
                var id = ids[i];

                var asset = AddressableDb.Singleton.GetAssetByIdOrFallback<T>(null, ref id);

                if (asset != null)
                    assets[i] = asset;
            }

            return assets;
        }
    }
}
