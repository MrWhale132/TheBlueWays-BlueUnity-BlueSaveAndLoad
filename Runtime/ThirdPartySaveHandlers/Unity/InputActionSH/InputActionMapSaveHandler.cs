
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.UtilScripts;
using UnityEngine.InputSystem;
using System;

namespace Assets._Project.Scripts.SaveHandlers.Manuals.InputActionSH
{
    //if an actionMap has no asset, it means it was cloned
    //if couldnt find by name, we assume it was created by unity internally (eg. when an InputAction has no actionMap)
    [SaveHandler(6787213423445435, nameof(InputActionMap), typeof(InputActionMap), order: -99, dependsOn: new[] { typeof(InputActionAsset) })]
    public class InputActionMapSaveHandler : UnmanagedSaveHandler<InputActionMap, InputActionMapSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            
            __saveData.asset = GetObjectId(__instance.asset);
            __saveData.name = __instance.name;
            __saveData.enabled = __instance.enabled;
        }


        public override void _AssignInstance()
        {
            //if (__saveData.IsClone)
            {
                if (__saveData.asset.IsNotDefault)
                {
                    var asset = GetObjectById<InputActionAsset>(__saveData.asset);
                    __instance = asset.FindActionMap(__saveData.name);
                }
                else
                {
                    //WARNING todo: unfortunatelly, this instance will be totally different from what Unity creates internally
                    //when an InputAction has no actionMap
                var map = InputSystem.actions.FindActionMap(__saveData.name);

                    if(map != null)
                    {
                        __instance = map.Clone();
                    }
                    else
                    {
                        __instance = new InputActionMap(__saveData.name);
                    }
                }
            }
            //else
            //{
            //    __instance = InputSystem.actions.FindActionMap(__saveData.name);
            //}
        }


        public override void LoadPhase1()
        {
            base.LoadPhase1();

            if (__saveData.enabled)
            {
                __instance.Enable();
            }
            else __instance.Disable();
        }
    }

    public class InputActionMapSaveData : SaveDataBase
    {
        public RandomId asset;
        public string name;
        public bool enabled;
    }
}
