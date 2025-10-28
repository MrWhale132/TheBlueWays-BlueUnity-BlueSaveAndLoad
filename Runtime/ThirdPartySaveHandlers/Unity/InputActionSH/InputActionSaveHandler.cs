
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.UtilScripts;
using UnityEngine.InputSystem;
using System;
namespace Assets._Project.Scripts.SaveHandlers.Manuals.InputActionSH
{

    [SaveHandler(366884624243324234, nameof(InputAction), typeof(InputAction))]
    public class InputActionSaveHandler : UnmanagedSaveHandler<InputAction, InputActionSaveData>
    {

        public override void Init(object instance)
        {
            base.Init(instance);


            __saveData.InputAssetId = GetAssetId(__instance.actionMap.asset);
            __saveData.InputActionGuid = __instance.id;
        }

        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.enabled = __instance.enabled;
        }

        public override void CreateObject()
        {
            var inputAsset = AddressableDb.Singleton.GetAssetByIdOrFallback<InputActionAsset>(null, ref __saveData.InputAssetId);

            __instance = inputAsset.FindAction(__saveData.InputActionGuid);

            HandledObjectId = __saveData._ObjectId_;

            Infra.Singleton.RegisterReference(__instance, HandledObjectId);
        }

        public override void LoadReferences()
        {
            base.LoadReferences();

            if (__saveData.enabled)
            {
                __instance.Enable();
            }
            else __instance.Disable();
        }
    }

    public class InputActionSaveData : SaveDataBase
    {
        public RandomId InputAssetId;
        public Guid InputActionGuid;
        public bool enabled;
    }
}
