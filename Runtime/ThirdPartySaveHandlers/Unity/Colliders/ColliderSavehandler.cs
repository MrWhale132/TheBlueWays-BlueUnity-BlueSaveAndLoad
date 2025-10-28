using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity.Colliders
{
    //base class for all collider save handlers
    public class ColliderSavehandler<TSavable, TSaveData> : MonoSaveHandler<TSavable, TSaveData>
        where TSavable : UnityEngine.Collider
        where TSaveData : ColliderSaveData, new()
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.IsTrigger = __instance.isTrigger;
            __saveData.ProvidesContacts = __instance.providesContacts;
            __saveData.LayerOverridePriority = __instance.layerOverridePriority;
            __saveData.ExcludeLayers = __instance.excludeLayers;
            __saveData.IncludeLayers = __instance.includeLayers;

            __saveData.MaterialId = AddressableDb.Singleton.GetAssetIdByAssetName(__instance.sharedMaterial);
        }


        public override void LoadReferences()
        {
            base.LoadReferences();

            __instance.material = AddressableDb.Singleton.GetAssetByIdOrFallback(__instance.sharedMaterial, ref __saveData.MaterialId);
            
        }


        public override void LoadValues()
        {
            base.LoadValues();


            __instance.isTrigger = __saveData.IsTrigger;
            __instance.providesContacts = __saveData.ProvidesContacts;
            __instance.layerOverridePriority = __saveData.LayerOverridePriority;
            __instance.excludeLayers = __saveData.ExcludeLayers;
            __instance.includeLayers = __saveData.IncludeLayers;
        }
    }

    public class ColliderSaveData : MonoSaveDataBase
    {
        public bool IsTrigger;
        public bool ProvidesContacts;
        public bool? Convex;
        public RandomId MaterialId;
        public int LayerOverridePriority;
        public int ExcludeLayers;
        public int IncludeLayers;
    }
}
