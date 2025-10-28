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
namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.UnityShs.AnimationTypes
{
    //[SaveHandler(420303703206319869, "AnimationClip", typeof(UnityEngine.AnimationClip))]
    public class AnimationClipSaveHandler : AssetSaveHandlerBase<AnimationClip, AnimationClipSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.frameRate = __instance.frameRate;
            __saveData.wrapMode = __instance.wrapMode;
            __saveData.localBounds.ReadFrom(__instance.localBounds);
            __saveData.legacy = __instance.legacy;
            __saveData.events = GetObjectId(__instance.events);
            __saveData.hideFlags = __instance.hideFlags;
        }
        public override void LoadReferences()
        {
            base.LoadReferences();
            __instance.frameRate = __saveData.frameRate;
            __instance.wrapMode = __saveData.wrapMode;
            __saveData.localBounds.WriteTo(__instance.localBounds);
            __instance.legacy = __saveData.legacy;
            __instance.events = GetObjectById<AnimationEvent[]>(__saveData.events);
            __instance.hideFlags = __saveData.hideFlags;
        }
        static AnimationClipSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
                {"SampleAnimation(UnityEngine.CoreModule UnityEngine.GameObject,mscorlib System.Single):mscorlib System.Void", 824938845156773174},
                {"SetCurve(mscorlib System.String,mscorlib System.Type,mscorlib System.String,UnityEngine.CoreModule UnityEngine.AnimationCurve):mscorlib System.Void", 902334852855540488},
                {"EnsureQuaternionContinuity():mscorlib System.Void", 149256854643213486},
                {"ClearCurves():mscorlib System.Void", 748917599961538402},
                {"AddEvent(UnityEngine.AnimationModule UnityEngine.AnimationEvent):mscorlib System.Void", 674647911963105373}
            };
            Infra.Singleton.__methodIdsByMethodSignaturePerType.Add(_typeReference, methodToId);
            Infra.Singleton.__methodGetterFactoryPerType.Add(_typeReference, _idToMethod);
            Infra.Singleton.__methodInfoGettersPerType.Add(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(AnimationClip);
        static Type _typeDefinition = typeof(UnityEngine.AnimationClip);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                824938845156773174 => new Func<object, Delegate>((instance) => new Action<UnityEngine.GameObject, System.Single>(((AnimationClip)instance).SampleAnimation)),
                902334852855540488 => new Func<object, Delegate>((instance) => new Action<System.String, System.Type, System.String, UnityEngine.AnimationCurve>(((AnimationClip)instance).SetCurve)),
                149256854643213486 => new Func<object, Delegate>((instance) => new Action(((AnimationClip)instance).EnsureQuaternionContinuity)),
                748917599961538402 => new Func<object, Delegate>((instance) => new Action(((AnimationClip)instance).ClearCurves)),
                674647911963105373 => new Func<object, Delegate>((instance) => new Action<UnityEngine.AnimationEvent>(((AnimationClip)instance).AddEvent)),
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
    public class AnimationClipSaveData : AssetSaveData
    {
        public System.Single frameRate;
        public UnityEngine.WrapMode wrapMode;
        public DevTest.BoundsSaveData localBounds = new();
        public System.Boolean legacy;
        public RandomId events;
        public UnityEngine.HideFlags hideFlags;
    }
}