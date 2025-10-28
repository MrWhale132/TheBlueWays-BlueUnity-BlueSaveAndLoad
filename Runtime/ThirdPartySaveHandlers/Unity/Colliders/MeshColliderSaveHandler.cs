
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.UtilScripts;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity.Colliders
{
    //[SaveHandler(423878123784,nameof(MeshCollider),typeof(MeshCollider))]
    public class MeshColliderSaveHandler:ColliderSavehandler<MeshCollider,MeshColliderSaveData>
    {
        public MeshColliderSaveHandler()
        {
            
        }

        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.Convex = __instance.convex;
            __saveData.CookingOptions = (int)__instance.cookingOptions;

            __saveData.MeshAssetId = AddressableDb.Singleton.GetAssetIdByAssetName(__instance.sharedMesh);
        }

        public override void LoadReferences()
        {
            base.LoadReferences();

            __instance.sharedMesh = AddressableDb.Singleton.GetAssetByIdOrFallback(__instance.sharedMesh, ref __saveData.MeshAssetId);
        }
    }

    public class MeshColliderSaveData : ColliderSaveData
    {
        public int CookingOptions;
        public RandomId MeshAssetId;
    }
}
