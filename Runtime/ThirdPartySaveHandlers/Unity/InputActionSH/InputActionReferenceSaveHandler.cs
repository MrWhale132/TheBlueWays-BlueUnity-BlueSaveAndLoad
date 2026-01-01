
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.UtilScripts;
using UnityEngine.InputSystem;
using System;

namespace Assets._Project.Scripts.SaveHandlers.Manuals.InputActionSH
{
    [SaveHandler(3423423523535234, nameof(InputActionReference), typeof(InputActionReference))]
    public class InputActionReferenceSaveHandler : ScriptableSaveHandlerBase<InputActionReference, InputActionReferenceSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.InputAssetId = GetAssetId(__instance.action.actionMap.asset);
            __saveData.InputActionGuid = __instance.action.id;
        }


        public override void _AssignInstance()
        {
            var inputAsset = AddressableDb.Singleton.GetAssetByIdOrFallback<InputActionAsset>(null, ref __saveData.InputAssetId);

            var action = inputAsset.FindAction(__saveData.InputActionGuid);

            __instance = InputActionReference.Create(action);
        }
    }

    public class InputActionReferenceSaveData : SaveDataBase
    {
        public RandomId InputAssetId;
        public Guid InputActionGuid;
    }

}
