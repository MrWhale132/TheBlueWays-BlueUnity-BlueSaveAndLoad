////auto-generated
//using UnityEngine;
//using Assets._Project.Scripts.UtilScripts;
//using Assets._Project.Scripts.Infrastructure;
//using Assets._Project.Scripts.SaveAndLoad;
//using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
//using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
//using System.Collections.Generic;
//using System;
//using System.Reflection;

//namespace DevTest
//{
//	public class StaticMaterialSubtitute : StaticSubtitute 
//	{
//		public override Type SubtitutedType => typeof(UnityEngine.Material);
//	}

//	[SaveHandler(341386988675097556, "StaticMaterialSubtitute", typeof(StaticMaterialSubtitute), generationMode: SaveHandlerGenerationMode.FullAutomata, staticHandlerOf: typeof(UnityEngine.Material))]
//	public class StaticMaterialSaveHandler : StaticSaveHandlerBase<StaticMaterialSubtitute, StaticMaterialSaveData> 
//	{
//		public override void WriteSaveData()
//		{
//			base.WriteSaveData();

//		}

//		public override void LoadReferences()
//		{
//			base.LoadReferences();

//		}
//		static StaticMaterialSaveHandler()
//		{
//			Dictionary<string, long> methodToId = new()
//			{
//				/// methodToId map for static <see cref="Material"/>
//			};
//			Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
//			Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
//			Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
//		}
//		static Type _typeReference = typeof(UnityEngine.Material);
//		static Type _typeDefinition = typeof(UnityEngine.Material);
//		static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
//		public static Func<object, Delegate> _idToMethod(long id)
//		{
//			Func<object, Delegate> method = id switch
//			{
//				_ => Infra.Singleton.GetIdToMethodMapForType(_typeReference.BaseType)(id),
//			};
//			return method;
//		}
//		public static MethodInfo _idToMethodInfo(long id)
//		{
//			MethodInfo methodDef = id switch
//			{
//				_ => Infra.Singleton.GetMethodInfoIdToMethodMapForType(_typeReference.BaseType)(id),
//			};
//			return methodDef;
//		}
//	}


//	public class StaticMaterialSaveData : StaticSaveDataBase 
//	{

//	}

//}