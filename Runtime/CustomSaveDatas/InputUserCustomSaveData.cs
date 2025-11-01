//auto-generated
using UnityEngine.InputSystem.Users;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace DevTest
{
	public class InputUserSaveData : CustomSaveData<UnityEngine.InputSystem.Users.InputUser> 
	{
		public List<RandomId> pairedDevices = new List<RandomId>();
		public RandomId actions;
		public string controlSchemeName;

		public override void ReadFrom(in UnityEngine.InputSystem.Users.InputUser instance)
		{
			foreach(var device in instance.pairedDevices)
            {
                pairedDevices.Add(GetObjectId(device));
            }
            actions = GetObjectId(instance.actions);
			controlSchemeName = instance.controlScheme?.name;
			
		}
		public override void WriteTo(ref UnityEngine.InputSystem.Users.InputUser instance)
		{
			instance = InputUser.CreateUserWithoutPairedDevices();
			
			foreach(var deviceId in pairedDevices)
			{
				var device = GetObjectById<UnityEngine.InputSystem.InputDevice>(deviceId);

				InputUser.PerformPairingWithDevice(device, instance);
			}

			var actions = GetObjectById<UnityEngine.InputSystem.InputActionAsset>(this.actions);

			instance.AssociateActionsWithUser(actions);

			if(!string.IsNullOrEmpty(controlSchemeName))
            {
                instance.ActivateControlScheme(controlSchemeName);
            }

			if(instance.valid == false)
			{

			}                
		}
	}

	public class StaticInputUserSubtitute : StaticSubtitute 
	{
		public override Type SubtitutedType => typeof(UnityEngine.InputSystem.Users.InputUser);
	}

	[SaveHandler(981949032348359028, "StaticInputUserSubtitute", typeof(StaticInputUserSubtitute), generationMode: SaveHandlerGenerationMode.FullAutomata, staticHandlerOf: typeof(UnityEngine.InputSystem.Users.InputUser))]
	public class StaticInputUserSaveHandler : StaticSaveHandlerBase<StaticInputUserSubtitute, StaticInputUserSaveData> 
	{
		public override void WriteSaveData()
		{
			base.WriteSaveData();
			__saveData.onChange = GetInvocationList(nameof(UnityEngine.InputSystem.Users.InputUser.onChange));
			__saveData.onUnpairedDeviceUsed = GetInvocationList(nameof(UnityEngine.InputSystem.Users.InputUser.onUnpairedDeviceUsed));
			__saveData.onPrefilterUnpairedDeviceActivity = GetInvocationList(nameof(UnityEngine.InputSystem.Users.InputUser.onPrefilterUnpairedDeviceActivity));
		}

		public override void LoadReferences()
		{
			base.LoadReferences();
			var onChangeDel = GetDelegate<System.Action<UnityEngine.InputSystem.Users.InputUser, UnityEngine.InputSystem.Users.InputUserChange, UnityEngine.InputSystem.InputDevice>>(__saveData.onChange);
			if(onChangeDel != null)
			UnityEngine.InputSystem.Users.InputUser.onChange += onChangeDel;
			var onUnpairedDeviceUsedDel = GetDelegate<System.Action<UnityEngine.InputSystem.InputControl, UnityEngine.InputSystem.LowLevel.InputEventPtr>>(__saveData.onUnpairedDeviceUsed);
			if(onUnpairedDeviceUsedDel != null)
			UnityEngine.InputSystem.Users.InputUser.onUnpairedDeviceUsed += onUnpairedDeviceUsedDel;
			var onPrefilterUnpairedDeviceActivityDel = GetDelegate<System.Func<UnityEngine.InputSystem.InputDevice, UnityEngine.InputSystem.LowLevel.InputEventPtr, System.Boolean>>(__saveData.onPrefilterUnpairedDeviceActivity);
			if(onPrefilterUnpairedDeviceActivityDel != null)
			UnityEngine.InputSystem.Users.InputUser.onPrefilterUnpairedDeviceActivity += onPrefilterUnpairedDeviceActivityDel;
		}
		static StaticInputUserSaveHandler()
		{
			Dictionary<string, long> methodToId = new()
			{
				{"GetUnpairedInputDevices():Unity.InputSystem UnityEngine.InputSystem.InputControlList<Unity.InputSystem UnityEngine.InputSystem.InputDevice>", 522239805719509499},
				{"GetUnpairedInputDevices(Unity.InputSystem UnityEngine.InputSystem.InputControlList<Unity.InputSystem UnityEngine.InputSystem.InputDevice>&):mscorlib System.Int32", 878703747058961572},
				{"FindUserPairedToDevice(Unity.InputSystem UnityEngine.InputSystem.InputDevice):mscorlib System.Nullable<Unity.InputSystem UnityEngine.InputSystem.Users.InputUser>", 565113432273886915},
				{"FindUserByAccount(Unity.InputSystem UnityEngine.InputSystem.Users.InputUserAccountHandle):mscorlib System.Nullable<Unity.InputSystem UnityEngine.InputSystem.Users.InputUser>", 961959275674802702},
				{"CreateUserWithoutPairedDevices():Unity.InputSystem UnityEngine.InputSystem.Users.InputUser", 475536046317104650},
				{"PerformPairingWithDevice(Unity.InputSystem UnityEngine.InputSystem.InputDevice,Unity.InputSystem UnityEngine.InputSystem.Users.InputUser,Unity.InputSystem UnityEngine.InputSystem.Users.InputUserPairingOptions):Unity.InputSystem UnityEngine.InputSystem.Users.InputUser", 141683876747017454}
			};
			Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
			Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
			Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
		}
		static Type _typeReference = typeof(UnityEngine.InputSystem.Users.InputUser);
		static Type _typeDefinition = typeof(UnityEngine.InputSystem.Users.InputUser);
		static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
		public static Func<object, Delegate> _idToMethod(long id)
		{
			Func<object, Delegate> method = id switch
			{
				522239805719509499 => new Func<object, Delegate>((instance) => new Func<UnityEngine.InputSystem.InputControlList<UnityEngine.InputSystem.InputDevice>>(UnityEngine.InputSystem.Users.InputUser.GetUnpairedInputDevices)),
				565113432273886915 => new Func<object, Delegate>((instance) => new Func<UnityEngine.InputSystem.InputDevice, System.Nullable<UnityEngine.InputSystem.Users.InputUser>>(UnityEngine.InputSystem.Users.InputUser.FindUserPairedToDevice)),
				961959275674802702 => new Func<object, Delegate>((instance) => new Func<UnityEngine.InputSystem.Users.InputUserAccountHandle, System.Nullable<UnityEngine.InputSystem.Users.InputUser>>(UnityEngine.InputSystem.Users.InputUser.FindUserByAccount)),
				475536046317104650 => new Func<object, Delegate>((instance) => new Func<UnityEngine.InputSystem.Users.InputUser>(UnityEngine.InputSystem.Users.InputUser.CreateUserWithoutPairedDevices)),
				141683876747017454 => new Func<object, Delegate>((instance) => new Func<UnityEngine.InputSystem.InputDevice, UnityEngine.InputSystem.Users.InputUser, UnityEngine.InputSystem.Users.InputUserPairingOptions, UnityEngine.InputSystem.Users.InputUser>(UnityEngine.InputSystem.Users.InputUser.PerformPairingWithDevice)),
				_ => Infra.Singleton.GetIdToMethodMapForType(_typeReference.BaseType)(id),
			};
			return method;
		}
		public static MethodInfo _idToMethodInfo(long id)
		{
			MethodInfo methodDef = id switch
			{
				878703747058961572 => typeof(UnityEngine.InputSystem.Users.InputUser).GetMethod(nameof(UnityEngine.InputSystem.Users.InputUser.GetUnpairedInputDevices), BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(UnityEngine.InputSystem.InputControlList<>).MakeGenericType(typeof(UnityEngine.InputSystem.InputDevice)).MakeByRefType() }, null),
				_ => Infra.Singleton.GetMethodInfoIdToMethodMapForType(_typeReference.BaseType)(id),
			};
			return methodDef;
		}
	}


	public class StaticInputUserSaveData : StaticSaveDataBase 
	{
		public InvocationList onChange = new();
		public InvocationList onUnpairedDeviceUsed = new();
		public InvocationList onPrefilterUnpairedDeviceActivity = new();
	}

}