
using Assets._Project.Scripts.UtilScripts;
using System;
using UnityEngine.Scripting;

namespace Assets._Project.Scripts.SaveAndLoad
{
    [Preserve]
    public interface ISaveAndLoad
    {
        public ObjectMetaData MetaData { get; }
        public string SaveHandlerId { get; }
        public RandomId HandledObjectId { get; }
        public bool IsInitialized { get; }
        public string DataGroupId { get; }
        public Type HandledType { get; }
        public int Order { get; set; }
        public bool IsValid { get; }
        public void ReleaseObject();
        //save
        public void WriteSaveData();
        public string Serialize();
        //load
        public void Deserialize(string json);
        public void CreateObject();
        public void RegisterDelegates();
        public void LoadReferences();
        public void LoadValues();
    }
}
