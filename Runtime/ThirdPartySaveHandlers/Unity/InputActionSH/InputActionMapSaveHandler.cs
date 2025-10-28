
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.UtilScripts;
using UnityEngine.InputSystem;
using System;

namespace Assets._Project.Scripts.SaveHandlers.Manuals.InputActionSH
{

    [SaveHandler(6787213423445435, nameof(InputActionMap), typeof(InputActionMap))]
    public class InputActionMapSaveHandler : UnmanagedSaveHandler<InputActionMap, InputActionMapSaveData>
    {
        public InputActionMapSaveHandler()
        {

        }

        public override void Init(object instance)
        {
            base.Init(instance);


            __saveData.InputAssetId = GetAssetId(__instance.asset);
            __saveData.InputActionMapGuid = __instance.id;
        }

        public override void CreateObject()
        {
            var inputAsset = AddressableDb.Singleton.GetAssetByIdOrFallback<InputActionAsset>(null, ref __saveData.InputAssetId);

            __instance = inputAsset.FindActionMap(__saveData.InputActionMapGuid);

            HandledObjectId = __saveData._ObjectId_;

            Infra.Singleton.RegisterReference(__instance, HandledObjectId);
        }
    }
    public class InputActionMapSaveData : SaveDataBase
    {
        public RandomId InputAssetId;
        public Guid InputActionMapGuid;
    }
}
