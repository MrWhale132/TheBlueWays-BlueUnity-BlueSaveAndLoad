using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using UnityEngine.InputSystem;

namespace Assets._Project.Scripts.SaveHandlers.Manuals.InputActionSH
{
    [SaveHandler(324543645323234, nameof(InputActionAsset), typeof(InputActionAsset), order: -99, dependsOn: new[] { typeof(InputActionAsset) })]
    public class InputActionAssetSaveHandler : UnmanagedSaveHandler<InputActionAsset, InputActionAssetSaveData>
    {
        public override void Init(object instance, InitContext context)
        {
            base.Init(instance, context);

            var isClone = InputSystem.actions != __instance;
            
            __saveData.isClone = isClone;
        }

        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.enabled = __instance.enabled;
            __saveData.name = __instance.name;
        }

        public override void _AssignInstance()
        {
            if (__saveData.isClone)
            {
                //todo: get the asset from assetdb
                __instance = InputActionAsset.FromJson(InputSystem.actions.ToJson());
            }
            else
            {
                __instance = InputSystem.actions;
            }
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

    public class InputActionAssetSaveData : SaveDataBase
    {
        public bool isClone;
        public string name;
        public bool enabled;
    }
}