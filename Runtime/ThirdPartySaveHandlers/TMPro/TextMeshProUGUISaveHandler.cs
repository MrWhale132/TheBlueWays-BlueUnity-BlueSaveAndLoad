////auto-generated
//using TMPro;
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
//	public class StaticTextMeshProUGUISubtitute : StaticSubtitute 
//	{
//		public override Type SubtitutedType => typeof(TMPro.TextMeshProUGUI);
//	}

//	[SaveHandler(373649652201181185, "StaticTextMeshProUGUISubtitute", typeof(StaticTextMeshProUGUISubtitute), generationMode: SaveHandlerGenerationMode.FullAutomata, staticHandlerOf: typeof(TMPro.TextMeshProUGUI))]
//	public class StaticTextMeshProUGUISaveHandler : StaticSaveHandlerBase<StaticTextMeshProUGUISubtitute, StaticTextMeshProUGUISaveData> 
//	{
//		public override void WriteSaveData()
//		{
//			base.WriteSaveData();

//		}

//		public override void LoadReferences()
//		{
//			base.LoadReferences();

//		}
//		static StaticTextMeshProUGUISaveHandler()
//		{
//			Dictionary<string, long> methodToId = new()
//			{
//				/// methodToId map for static <see cref="TextMeshProUGUI"/>
//			};
//			Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
//			Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
//			Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
//		}
//		static Type _typeReference = typeof(TMPro.TextMeshProUGUI);
//		static Type _typeDefinition = typeof(TMPro.TextMeshProUGUI);
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


//	public class StaticTextMeshProUGUISaveData : StaticSaveDataBase 
//	{

//	}

//}