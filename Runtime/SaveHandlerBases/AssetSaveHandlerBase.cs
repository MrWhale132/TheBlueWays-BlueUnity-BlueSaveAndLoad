
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.Misc;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases
{
    public class AssetSaveHandlerBase<TAsset, TSaveData> : SaveHandlerGenericBase<TAsset, TSaveData>
        where TAsset : UnityEngine.Object
        where TSaveData : AssetSaveData, new()
    {
        public bool _isRuntimeGenerated;
        public bool _isClone;


        public override void Init(object instance)
        {
            base.Init(instance);

            _isRuntimeGenerated = __instance.IsProbablyRuntimeGenerated(out var _isClone);

            __saveData._isRuntimeGenerated = _isRuntimeGenerated;
            __saveData._isClone = _isClone;

            __saveData._AssetId_ = GetAssetId(__instance);
        }

        public override void CreateObject()
        {
            base.CreateObject();

            if (!__saveData._isRuntimeGenerated)
            {
                __saveData._AssetId_.AssignAssetById(ref __instance);

                HandledObjectId = __saveData._ObjectId_;

                Infra.Singleton.RegisterReference(__instance, HandledObjectId);
            }
            else
            {
                Debug.LogError($"Recreating runtime generated asset is not supported currently. AssetId: {__saveData._AssetId_}");
            }
        }
    }


    public class AssetSaveData : SaveDataBase
    {
        public RandomId _AssetId_;
        public bool _isRuntimeGenerated;
        public bool _isClone;
    }
}
