//auto-generated
using UnityEngine.InputSystem;
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
	[SaveHandler(909970018741147360, "InputActionAsset", typeof(UnityEngine.InputSystem.InputActionAsset), order:-100)]
	public class InputActionAssetSaveHandler : ScriptableSaveHandlerBase<UnityEngine.InputSystem.InputActionAsset, InputActionAssetSaveData> 
	{
        public override void Init(object instance)
        {
            base.Init(instance);

			__saveData.IsClone = InputSystem.actions != __instance;
        }
        public override void WriteSaveData()
		{
			base.WriteSaveData();
			__saveData.hideFlags = __instance.hideFlags;
		}

        public override void _AssignInstance()
        {
			if (__saveData.IsClone)
			{
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
			__instance.hideFlags = __saveData.hideFlags;
		}

		static InputActionAssetSaveHandler()
		{
			Dictionary<string, long> methodToId = new()
			{
				{"ToJson():mscorlib System.String", 516864360498327020},
				{"LoadFromJson(mscorlib System.String):mscorlib System.Void", 712809578488879142},
				{"FindAction(mscorlib System.String,mscorlib System.Boolean):Unity.InputSystem UnityEngine.InputSystem.InputAction", 614561062031302764},
				{"FindBinding(Unity.InputSystem UnityEngine.InputSystem.InputBinding,Unity.InputSystem UnityEngine.InputSystem.InputAction&):mscorlib System.Int32", 751728587152918440},
				{"FindActionMap(mscorlib System.String,mscorlib System.Boolean):Unity.InputSystem UnityEngine.InputSystem.InputActionMap", 614183754607601448},
				{"FindActionMap(mscorlib System.Guid):Unity.InputSystem UnityEngine.InputSystem.InputActionMap", 299697381902396214},
				{"FindAction(mscorlib System.Guid):Unity.InputSystem UnityEngine.InputSystem.InputAction", 957797488054147025},
				{"FindControlSchemeIndex(mscorlib System.String):mscorlib System.Int32", 291789274231148648},
				{"FindControlScheme(mscorlib System.String):mscorlib System.Nullable<Unity.InputSystem UnityEngine.InputSystem.InputControlScheme>", 149120933686526102},
				{"IsUsableWithDevice(Unity.InputSystem UnityEngine.InputSystem.InputDevice):mscorlib System.Boolean", 327841315791649898},
				{"Enable():mscorlib System.Void", 697185569771493642},
				{"Disable():mscorlib System.Void", 229780524204763850},
				{"Contains(Unity.InputSystem UnityEngine.InputSystem.InputAction):mscorlib System.Boolean", 514228254439790766},
				{"GetEnumerator():mscorlib System.Collections.Generic.IEnumerator<Unity.InputSystem UnityEngine.InputSystem.InputAction>", 881755036400754212}
			};
			Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
			Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
			Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
		}
		static Type _typeReference = typeof(UnityEngine.InputSystem.InputActionAsset);
		static Type _typeDefinition = typeof(UnityEngine.InputSystem.InputActionAsset);
		static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
		public static Func<object, Delegate> _idToMethod(long id)
		{
			Func<object, Delegate> method = id switch
			{
				516864360498327020 => new Func<object, Delegate>((instance) => new Func<System.String>(((UnityEngine.InputSystem.InputActionAsset)instance).ToJson)),
				712809578488879142 => new Func<object, Delegate>((instance) => new Action<System.String>(((UnityEngine.InputSystem.InputActionAsset)instance).LoadFromJson)),
				614561062031302764 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean, UnityEngine.InputSystem.InputAction>(((UnityEngine.InputSystem.InputActionAsset)instance).FindAction)),
				614183754607601448 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean, UnityEngine.InputSystem.InputActionMap>(((UnityEngine.InputSystem.InputActionAsset)instance).FindActionMap)),
				299697381902396214 => new Func<object, Delegate>((instance) => new Func<System.Guid, UnityEngine.InputSystem.InputActionMap>(((UnityEngine.InputSystem.InputActionAsset)instance).FindActionMap)),
				957797488054147025 => new Func<object, Delegate>((instance) => new Func<System.Guid, UnityEngine.InputSystem.InputAction>(((UnityEngine.InputSystem.InputActionAsset)instance).FindAction)),
				291789274231148648 => new Func<object, Delegate>((instance) => new Func<System.String, System.Int32>(((UnityEngine.InputSystem.InputActionAsset)instance).FindControlSchemeIndex)),
				149120933686526102 => new Func<object, Delegate>((instance) => new Func<System.String, System.Nullable<UnityEngine.InputSystem.InputControlScheme>>(((UnityEngine.InputSystem.InputActionAsset)instance).FindControlScheme)),
				327841315791649898 => new Func<object, Delegate>((instance) => new Func<UnityEngine.InputSystem.InputDevice, System.Boolean>(((UnityEngine.InputSystem.InputActionAsset)instance).IsUsableWithDevice)),
				697185569771493642 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.InputSystem.InputActionAsset)instance).Enable)),
				229780524204763850 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.InputSystem.InputActionAsset)instance).Disable)),
				514228254439790766 => new Func<object, Delegate>((instance) => new Func<UnityEngine.InputSystem.InputAction, System.Boolean>(((UnityEngine.InputSystem.InputActionAsset)instance).Contains)),
				881755036400754212 => new Func<object, Delegate>((instance) => new Func<System.Collections.Generic.IEnumerator<UnityEngine.InputSystem.InputAction>>(((UnityEngine.InputSystem.InputActionAsset)instance).GetEnumerator)),
				_ => Infra.Singleton.GetIdToMethodMapForType(_typeReference.BaseType)(id),
			};
			return method;
		}
		public static MethodInfo _idToMethodInfo(long id)
		{
			MethodInfo methodDef = id switch
			{
				751728587152918440 => typeof(UnityEngine.InputSystem.InputActionAsset).GetMethod(nameof(UnityEngine.InputSystem.InputActionAsset.FindBinding), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.InputSystem.InputBinding), typeof(UnityEngine.InputSystem.InputAction).MakeByRefType() }, null),
				_ => Infra.Singleton.GetMethodInfoIdToMethodMapForType(_typeReference.BaseType)(id),
			};
			return methodDef;
		}
	}


	public class InputActionAssetSaveData : SaveDataBase 
	{
		public bool IsClone;
		public UnityEngine.HideFlags hideFlags;
	}


	public class StaticInputActionAssetSubtitute : StaticSubtitute 
	{
		public override Type SubtitutedType => typeof(UnityEngine.InputSystem.InputActionAsset);
	}

	[SaveHandler(372178640045123207, "StaticInputActionAssetSubtitute", typeof(StaticInputActionAssetSubtitute), generationMode: SaveHandlerGenerationMode.FullAutomata, staticHandlerOf: typeof(UnityEngine.InputSystem.InputActionAsset))]
	public class StaticInputActionAssetSaveHandler : StaticSaveHandlerBase<StaticInputActionAssetSubtitute, StaticInputActionAssetSaveData> 
	{
		public override void WriteSaveData()
		{
			base.WriteSaveData();

		}

		public override void LoadReferences()
		{
			base.LoadReferences();

		}
		static StaticInputActionAssetSaveHandler()
		{
			Dictionary<string, long> methodToId = new()
			{
				{"FromJson(mscorlib System.String):Unity.InputSystem UnityEngine.InputSystem.InputActionAsset", 109114525936152842}
			};
			Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
			Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
			Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
		}
		static Type _typeReference = typeof(UnityEngine.InputSystem.InputActionAsset);
		static Type _typeDefinition = typeof(UnityEngine.InputSystem.InputActionAsset);
		static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
		public static Func<object, Delegate> _idToMethod(long id)
		{
			Func<object, Delegate> method = id switch
			{
				109114525936152842 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.InputSystem.InputActionAsset>(UnityEngine.InputSystem.InputActionAsset.FromJson)),
				_ => Infra.Singleton.GetIdToMethodMapForType(_typeReference.BaseType)(id),
			};
			return method;
		}
		public static MethodInfo _idToMethodInfo(long id)
		{
			MethodInfo methodDef = id switch
			{
				_ => Infra.Singleton.GetMethodInfoIdToMethodMapForType(_typeReference.BaseType)(id),
			};
			return methodDef;
		}
	}


	public class StaticInputActionAssetSaveData : StaticSaveDataBase 
	{

	}

}