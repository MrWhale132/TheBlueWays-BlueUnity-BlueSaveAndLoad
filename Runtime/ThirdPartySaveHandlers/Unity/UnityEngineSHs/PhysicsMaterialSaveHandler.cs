//auto-generated
using UnityEngine;
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
	[SaveHandler(398928724705221252, "PhysicsMaterial", typeof(UnityEngine.PhysicsMaterial), generationMode: SaveHandlerGenerationMode.Manual)]
	public class PhysicsMaterialSaveHandler : AssetSaveHandlerBase<UnityEngine.PhysicsMaterial, PhysicsMaterialSaveData> 
	{
        public override bool CopyRegardlessIfItIsOriginal => true;
        public override void WriteSaveData()
		{
			base.WriteSaveData();
			__saveData.bounciness = __instance.bounciness;
			__saveData.dynamicFriction = __instance.dynamicFriction;
			__saveData.staticFriction = __instance.staticFriction;
			__saveData.frictionCombine = __instance.frictionCombine;
			__saveData.bounceCombine = __instance.bounceCombine;
			__saveData.hideFlags = __instance.hideFlags;
			
		}

		public override void LoadReferences()
		{
			base.LoadReferences();
			__instance.bounciness = __saveData.bounciness;
			__instance.dynamicFriction = __saveData.dynamicFriction;
			__instance.staticFriction = __saveData.staticFriction;
			__instance.frictionCombine = __saveData.frictionCombine;
			__instance.bounceCombine = __saveData.bounceCombine;
			__instance.hideFlags = __saveData.hideFlags;
		}

		static PhysicsMaterialSaveHandler()
		{
			Dictionary<string, long> methodToId = new()
			{
				/// methodToId map for <see cref="PhysicsMaterial"/>
			};
			Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
			Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
			Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
		}
		static Type _typeReference = typeof(UnityEngine.PhysicsMaterial);
		static Type _typeDefinition = typeof(UnityEngine.PhysicsMaterial);
		static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
		public static Func<object, Delegate> _idToMethod(long id)
		{
			Func<object, Delegate> method = id switch
			{
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


	public class PhysicsMaterialSaveData : AssetSaveData 
	{
		public System.Single bounciness;
		public System.Single dynamicFriction;
		public System.Single staticFriction;
		public UnityEngine.PhysicsMaterialCombine frictionCombine;
		public UnityEngine.PhysicsMaterialCombine bounceCombine;
		public UnityEngine.HideFlags hideFlags;
	}


	public class StaticPhysicsMaterialSubtitute : StaticSubtitute 
	{
		public override Type SubtitutedType => typeof(UnityEngine.PhysicsMaterial);
	}

	[SaveHandler(390373157617222646, "StaticPhysicsMaterialSubtitute", typeof(StaticPhysicsMaterialSubtitute), generationMode: SaveHandlerGenerationMode.FullAutomata, staticHandlerOf: typeof(UnityEngine.PhysicsMaterial))]
	public class StaticPhysicsMaterialSaveHandler : StaticSaveHandlerBase<StaticPhysicsMaterialSubtitute, StaticPhysicsMaterialSaveData> 
	{
		public override void WriteSaveData()
		{
			base.WriteSaveData();

		}

		public override void LoadReferences()
		{
			base.LoadReferences();

		}
		static StaticPhysicsMaterialSaveHandler()
		{
			Dictionary<string, long> methodToId = new()
			{
				/// methodToId map for static <see cref="PhysicsMaterial"/>
			};
			Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
			Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
			Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
		}
		static Type _typeReference = typeof(UnityEngine.PhysicsMaterial);
		static Type _typeDefinition = typeof(UnityEngine.PhysicsMaterial);
		static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
		public static Func<object, Delegate> _idToMethod(long id)
		{
			Func<object, Delegate> method = id switch
			{
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


	public class StaticPhysicsMaterialSaveData : StaticSaveDataBase 
	{

	}

}