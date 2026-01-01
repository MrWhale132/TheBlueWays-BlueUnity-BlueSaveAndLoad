using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.UtilScripts;
using System.Collections.Generic;
using UnityEngine.InputSystem.Users;

namespace Packages.com.theblueway.saveandload.Runtime.CustomSaveDatas
{
    [CustomSaveDataAttribute(HandledType = typeof(InputUser), GenerationMode = SaveHandlerGenerationMode.Manual)]
    public class InputUserSaveData : CustomSaveData<UnityEngine.InputSystem.Users.InputUser>
    {
        public string controlScheme;
        public RandomId actions;
        public List<RandomId> pairedDevices = new();
        public bool valid;

        public override void ReadFrom(in UnityEngine.InputSystem.Users.InputUser instance)
        {
            valid = instance.valid;
            if(!valid)
                return;

            if (instance.controlScheme != null)
                controlScheme = instance.controlScheme.Value.name;

            actions = GetObjectId(instance.actions);

            pairedDevices.Clear();

            foreach (var device in instance.pairedDevices)
            {
                pairedDevices.Add(GetObjectId(device));
            }
        }
        public override void WriteTo(ref UnityEngine.InputSystem.Users.InputUser instance)
        {
            if(!valid)
            {
                instance = default;
                return;
            }

            foreach (var deviceId in pairedDevices)
            {
                var device = GetObjectById<UnityEngine.InputSystem.InputDevice>(deviceId);

                instance = InputUser.PerformPairingWithDevice(device, instance);
            }

            var asset = GetObjectById<UnityEngine.InputSystem.InputActionAsset>(actions);

            instance.AssociateActionsWithUser(asset);

            if (!string.IsNullOrEmpty(controlScheme))
            {
                instance.ActivateControlScheme(controlScheme);
            }
        }
    }

}