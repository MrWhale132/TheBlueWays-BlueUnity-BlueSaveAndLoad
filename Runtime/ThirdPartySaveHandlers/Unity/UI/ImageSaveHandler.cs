//auto-generated
using UnityEngine.UI;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.UnitySHs.UI
{
    [SaveHandler(389237057858509605, "Image", typeof(Image))]
    public class ImageSaveHandler : MonoSaveHandler<UnityEngine.UI.Image, ImageSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.sprite = GetAssetId(__instance.sprite);
            __saveData.overrideSprite = GetAssetId(__instance.overrideSprite);
            __saveData.material = GetAssetId(__instance.material);
            __saveData.color = __instance.color;
            __saveData.type = __instance.type;
            __saveData.fillMethod = __instance.fillMethod;
            __saveData.preserveAspect = __instance.preserveAspect;
            __saveData.fillCenter = __instance.fillCenter;
            __saveData.fillClockwise = __instance.fillClockwise;
            __saveData.useSpriteMesh = __instance.useSpriteMesh;
            __saveData.fillAmount = __instance.fillAmount;
            __saveData.alphaHitTestMinimumThreshold = __instance.alphaHitTestMinimumThreshold;
            __saveData.pixelsPerUnitMultiplier = __instance.pixelsPerUnitMultiplier;
            __saveData.fillOrigin = __instance.fillOrigin;
            __saveData.onCullStateChanged = GetObjectId(__instance.onCullStateChanged);
            __saveData.raycastPadding = __instance.raycastPadding;
            __saveData.useGUILayout = __instance.useGUILayout;
            __saveData.runInEditMode = __instance.runInEditMode;
            __saveData.enabled = __instance.enabled;
            __saveData.hideFlags = __instance.hideFlags;
        }
        public override void LoadReferences()
        {
            base.LoadReferences();
            __instance.sprite = GetAssetById(__saveData.sprite, __instance.sprite);
            __instance.overrideSprite = GetAssetById(__saveData.overrideSprite, __instance.overrideSprite);
            __instance.material = GetAssetById(__saveData.material, __instance.material);
            __instance.color = __saveData.color;
            __instance.type = __saveData.type;
            __instance.fillMethod = __saveData.fillMethod;
            __instance.preserveAspect = __saveData.preserveAspect;
            __instance.fillCenter = __saveData.fillCenter;
            __instance.fillClockwise = __saveData.fillClockwise;
            __instance.useSpriteMesh = __saveData.useSpriteMesh;
            __instance.fillAmount = __saveData.fillAmount;
            //todo: InvalidOperationException: alphaHitTestMinimumThreshold should not be modified on a texture not readeable or not using Crunch Compression.
            //__instance.alphaHitTestMinimumThreshold = __saveData.alphaHitTestMinimumThreshold;
            __instance.pixelsPerUnitMultiplier = __saveData.pixelsPerUnitMultiplier;
            __instance.fillOrigin = __saveData.fillOrigin;
            __instance.onCullStateChanged = GetObjectById<UnityEngine.UI.MaskableGraphic.CullStateChangedEvent>(__saveData.onCullStateChanged);
            __instance.raycastPadding = __saveData.raycastPadding;
            __instance.useGUILayout = __saveData.useGUILayout;
            __instance.runInEditMode = __saveData.runInEditMode;
            __instance.enabled = __saveData.enabled;
            __instance.hideFlags = __saveData.hideFlags;
        }
        static ImageSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
                {"DisableSpriteOptimizations():mscorlib System.Void", 840416889058538654},
                {"OnBeforeSerialize():mscorlib System.Void", 257249290922597230},
                {"OnAfterDeserialize():mscorlib System.Void", 686334700922994455},
                {"SetNativeSize():mscorlib System.Void", 850011586655039896},
                {"CalculateLayoutInputHorizontal():mscorlib System.Void", 871254937892703138},
                {"CalculateLayoutInputVertical():mscorlib System.Void", 957575805488319767},
                {"IsRaycastLocationValid(UnityEngine.CoreModule UnityEngine.Vector2,UnityEngine.CoreModule UnityEngine.Camera):mscorlib System.Boolean", 450617479394199084}
            };
            Infra.Singleton.__methodIdsByMethodSignaturePerType.Add(_typeReference, methodToId);
            Infra.Singleton.__methodGetterFactoryPerType.Add(_typeReference, _idToMethod);
            Infra.Singleton.__methodInfoGettersPerType.Add(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(UnityEngine.UI.Image);
        static Type _typeDefinition = typeof(UnityEngine.UI.Image);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                840416889058538654 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.UI.Image)instance).DisableSpriteOptimizations)),
                257249290922597230 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.UI.Image)instance).OnBeforeSerialize)),
                686334700922994455 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.UI.Image)instance).OnAfterDeserialize)),
                850011586655039896 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.UI.Image)instance).SetNativeSize)),
                871254937892703138 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.UI.Image)instance).CalculateLayoutInputHorizontal)),
                957575805488319767 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.UI.Image)instance).CalculateLayoutInputVertical)),
                450617479394199084 => new Func<object, Delegate>((instance) => new Func<UnityEngine.Vector2, UnityEngine.Camera, System.Boolean>(((UnityEngine.UI.Image)instance).IsRaycastLocationValid)),
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
    public class ImageSaveData : MonoSaveDataBase
    {
        public RandomId sprite;
        public RandomId overrideSprite;
        public RandomId onCullStateChanged;
        public RandomId material;
        public UnityEngine.UI.Image.Type type;
        public UnityEngine.UI.Image.FillMethod fillMethod;
        public bool preserveAspect;
        public bool fillCenter;
        public bool fillClockwise;
        public bool useSpriteMesh;
        public float fillAmount;
        public float alphaHitTestMinimumThreshold;
        public float pixelsPerUnitMultiplier;
        public int fillOrigin;
        public UnityEngine.Vector4 raycastPadding;
        public System.Boolean useGUILayout;
        public System.Boolean runInEditMode;
        public System.Boolean enabled;
        public UnityEngine.HideFlags hideFlags;
        public Color color;
    }
}