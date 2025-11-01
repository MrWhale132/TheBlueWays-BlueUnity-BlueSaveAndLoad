
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.Misc;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases
{
    public class AssetSaveHandlerBase<TAsset, TSaveData> : SaveHandlerGenericBase<TAsset, TSaveData>
        where TAsset : UnityEngine.Object
        where TSaveData : AssetSaveData, new()
    {
        public bool IsProbablyUnmodifiedCopyOfOriginalAsset;
        public override bool IsValid => __instance != null;


        public override void Init(object instance)
        {
            base.Init(instance);

            IsProbablyUnmodifiedCopyOfOriginalAsset = __instance.IsProbablyUnmodifiedCopyOfOriginalAsset();

            __saveData.IsProbablyUnmodifiedCopyOfOriginalAsset = IsProbablyUnmodifiedCopyOfOriginalAsset;

            if (__saveData.IsProbablyUnmodifiedCopyOfOriginalAsset)
                __saveData._AssetId_ = AddressableDb.Singleton.GetAssetIdOfOriginalAssetFromCopy(__instance);
        }

        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.assetName = __instance.name;
        }


        public override void CreateObject()
        {
            base.CreateObject();

            IsProbablyUnmodifiedCopyOfOriginalAsset = __saveData.IsProbablyUnmodifiedCopyOfOriginalAsset;

            if (IsProbablyUnmodifiedCopyOfOriginalAsset)
            {
                _AssignInstance();

                HandledObjectId = __saveData._ObjectId_;

                Infra.Singleton.RegisterReference(__instance, HandledObjectId);
            }
            else
            {
                Debug.LogError($"Recreating runtime generated asset is not supported currently. AssetId: {__saveData._AssetId_}");
            }
        }

        public override void _AssignInstance()
        {
            if (IsProbablyUnmodifiedCopyOfOriginalAsset)
            {
                var orig = GetAssetById<TAsset>(__saveData._AssetId_, null);

                if (orig != null)
                {
                    var copy = Object.Instantiate(orig);

                    __instance = copy;
                }
            }
        }
    }


    public class AssetSaveData : SaveDataBase
    {
        public RandomId _AssetId_;
        public bool IsProbablyUnmodifiedCopyOfOriginalAsset;
        public string assetName;
    }
}
