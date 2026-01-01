
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.UtilScripts;
using UnityEngine.InputSystem;
using System;
using UnityEngine;
namespace Assets._Project.Scripts.SaveHandlers.Manuals.InputActionSH
{
    [SaveHandler(366884624243324234, nameof(InputAction), typeof(InputAction), order: -98, dependsOn: new[] { typeof(InputActionMap) })]
    public class InputActionSaveHandler : UnmanagedSaveHandler<InputAction, InputActionSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.name = __instance.name;
            __saveData.enabled = __instance.enabled;


            if (__instance.actionMap != null)
            {
                __saveData.InputActionMapName = __instance.actionMap.name;
                __saveData.actionMap = GetObjectId(__instance.actionMap);

                if (__instance.actionMap.asset != null)
                {
                    __saveData.InputActionAssetName = __instance.actionMap.asset.name;
                    __saveData.InputActionAssetId = GetObjectId(__instance.actionMap.asset);
                }
            }
        }


        public override void _AssignInstance()
        {
            {
                if (__saveData.actionMap.IsNotDefault)
                {
                    var map = GetObjectById<InputActionMap>(__saveData.actionMap);
                    var action = map.FindAction(__saveData.name);
                    if (action != null)
                        __instance = action;
                    else
                        __instance = InputSystem.actions.FindAction(__saveData.name).Clone();
                }
                else
                {
                    __instance = InputSystem.actions.FindAction(__saveData.name).Clone();
                }
            }
            //else
            //{
            //    __instance = InputSystem.actions.FindActionMap(__saveData.InputActionMapName).FindAction(__saveData.name);
            //}
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
        public string InputActionAssetName;
        public string InputActionMapName;
        public string name;

        public RandomId InputActionAssetId;
        public RandomId actionMap;

        public bool enabled;
    }
}
