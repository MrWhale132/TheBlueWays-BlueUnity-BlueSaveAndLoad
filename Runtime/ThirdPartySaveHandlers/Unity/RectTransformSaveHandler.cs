using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity
{
    [SaveHandler(746022504230144684, "RectTransform", typeof(UnityEngine.RectTransform), order:-9)]
    public class RectTransformSaveHandler : MonoSaveHandler<RectTransform, RectTransformSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.anchorMin = __instance.anchorMin;
            __saveData.anchorMax = __instance.anchorMax;
            __saveData.anchoredPosition = __instance.anchoredPosition;
            __saveData.sizeDelta = __instance.sizeDelta;
            __saveData.pivot = __instance.pivot;

            __saveData.localRotation = __instance.localRotation;
            __saveData.localScale = __instance.localScale;
            __saveData.localPosition = __instance.localPosition;
            __saveData.SiblingIndex = __instance.GetSiblingIndex();

            if (__instance.parent != null)
            {
                __saveData.ParentGOId = Infra.Singleton.GetObjectId(__instance.parent.gameObject, HandledObjectId);
            }
        }

        public override void LoadPhase1()
        {
            base.LoadPhase1();

            if (!__saveData.ParentGOId.IsDefault)
            {
                GameObject parent = Infra.Singleton.GetObjectById<GameObject>(__saveData.ParentGOId);
                __instance.SetParent(parent.transform);
            }


            __instance.localPosition = __saveData.localPosition;
            __instance.localRotation = __saveData.localRotation;
            __instance.localScale = __saveData.localScale;

            __instance.anchorMin = __saveData.anchorMin;
            __instance.anchorMax = __saveData.anchorMax;
            __instance.anchoredPosition = __saveData.anchoredPosition;
            __instance.sizeDelta = __saveData.sizeDelta;
            __instance.pivot = __saveData.pivot;
        }


        //doc: SetSiblingIndex can cause issues if the sibling index is out of range (e.g. if the parent has less children than the sibling index). So we set the sibling index in a separate phase after all objects have been loaded and parented to ensure that the sibling index is valid.
        public override void LoadPhase2()
        {
            base.LoadPhase2();

            __instance.SetSiblingIndex(__saveData.SiblingIndex);
        }







        static RectTransformSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
                {"ForceUpdateRectTransforms():mscorlib System.Void", 620057455443606195},
                {"GetLocalCorners(UnityEngine.CoreModule UnityEngine.Vector3[]):mscorlib System.Void", 573915097327885007},
                {"GetWorldCorners(UnityEngine.CoreModule UnityEngine.Vector3[]):mscorlib System.Void", 369957705153494810},
                {"SetInsetAndSizeFromParentEdge(UnityEngine.CoreModule UnityEngine.RectTransform+Edge,mscorlib System.Single,mscorlib System.Single):mscorlib System.Void", 261595903766769992},
                {"SetSizeWithCurrentAnchors(UnityEngine.CoreModule UnityEngine.RectTransform+Axis,mscorlib System.Single):mscorlib System.Void", 651510094482297483}
            };
            Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
            Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
            Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(RectTransform);
        static Type _typeDefinition = typeof(UnityEngine.RectTransform);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                620057455443606195 => new Func<object, Delegate>((instance) => new Action(((RectTransform)instance).ForceUpdateRectTransforms)),
                573915097327885007 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3[]>(((RectTransform)instance).GetLocalCorners)),
                369957705153494810 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3[]>(((RectTransform)instance).GetWorldCorners)),
                261595903766769992 => new Func<object, Delegate>((instance) => new Action<UnityEngine.RectTransform.Edge, System.Single, System.Single>(((RectTransform)instance).SetInsetAndSizeFromParentEdge)),
                651510094482297483 => new Func<object, Delegate>((instance) => new Action<UnityEngine.RectTransform.Axis, System.Single>(((RectTransform)instance).SetSizeWithCurrentAnchors)),
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
    public class RectTransformSaveData : MonoSaveDataBase
    {
        public RandomId ParentGOId;
        public UnityEngine.Vector2 anchorMin;
        public UnityEngine.Vector2 anchorMax;
        public UnityEngine.Vector2 anchoredPosition;
        public UnityEngine.Vector2 sizeDelta;
        public UnityEngine.Vector2 pivot;
        public Quaternion localRotation;
        public Vector3 localScale;
        public Vector3 localPosition;
        public int SiblingIndex;
    }
}