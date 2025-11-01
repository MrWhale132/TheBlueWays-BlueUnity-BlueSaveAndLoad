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
	[SaveHandler(737179718849734247, "WaitForSeconds", typeof(UnityEngine.WaitForSeconds), generationMode: SaveHandlerGenerationMode.FullAutomata)]
	public class WaitForSecondsSaveHandler : UnmanagedSaveHandler<UnityEngine.WaitForSeconds, WaitForSecondsSaveData> 
	{
        public override void Init(object instance)
        {
            base.Init(instance);
			__saveData.m_Seconds = (float)__instance.GetType().GetField("m_Seconds", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
        }
        public override void WriteSaveData()
		{
			base.WriteSaveData();

		}

        public override void _AssignInstance()
        {
			__instance = new WaitForSeconds(__saveData.m_Seconds);
        }

        public override void LoadReferences()
		{
			base.LoadReferences();

		}

		static WaitForSecondsSaveHandler()
		{
			Dictionary<string, long> methodToId = new()
			{

			};
			Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
			Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
			Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
		}
		static Type _typeReference = typeof(UnityEngine.WaitForSeconds);
		static Type _typeDefinition = typeof(UnityEngine.WaitForSeconds);
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


	public class WaitForSecondsSaveData : SaveDataBase 
	{
		public float m_Seconds;
    }


	public class StaticWaitForSecondsSubtitute : StaticSubtitute 
	{
		public override Type SubtitutedType => typeof(UnityEngine.WaitForSeconds);
	}

	[SaveHandler(492016996830242889, "StaticWaitForSecondsSubtitute", typeof(StaticWaitForSecondsSubtitute), generationMode: SaveHandlerGenerationMode.FullAutomata, staticHandlerOf: typeof(UnityEngine.WaitForSeconds))]
	public class StaticWaitForSecondsSaveHandler : StaticSaveHandlerBase<StaticWaitForSecondsSubtitute, StaticWaitForSecondsSaveData> 
	{
		public override void WriteSaveData()
		{
			base.WriteSaveData();

		}

		public override void LoadReferences()
		{
			base.LoadReferences();

		}
		static StaticWaitForSecondsSaveHandler()
		{
			Dictionary<string, long> methodToId = new()
			{

			};
			Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
			Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
			Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
		}
		static Type _typeReference = typeof(UnityEngine.WaitForSeconds);
		static Type _typeDefinition = typeof(UnityEngine.WaitForSeconds);
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


	public class StaticWaitForSecondsSaveData : StaticSaveDataBase 
	{

	}

}