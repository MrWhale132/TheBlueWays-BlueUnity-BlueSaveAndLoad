
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
        public InputActionReferenceSaveHandler()
        {

        }


        public override void Init(object instance)
        {
            base.Init(instance);


            __saveData.InputAssetId = GetAssetId(__instance.action.actionMap.asset);
            __saveData.InputActionGuid = __instance.action.id;
        }

        public override void CreateObject()
        {
            var inputAsset = AddressableDb.Singleton.GetAssetByIdOrFallback<InputActionAsset>(null, ref __saveData.InputAssetId);

            var action = inputAsset.FindAction(__saveData.InputActionGuid);

            __instance = InputActionReference.Create(action);
            //Debug.Log("creating: " + __instance.action.name);
            HandledObjectId = __saveData._ObjectId_;

            Infra.Singleton.RegisterReference(__instance, HandledObjectId);
        }
    }

    public class InputActionReferenceSaveData : SaveDataBase
    {
        public RandomId InputAssetId;
        public Guid InputActionGuid;
    }

}
