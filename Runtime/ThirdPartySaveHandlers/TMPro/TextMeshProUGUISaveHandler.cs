//auto-generated
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

namespace DevTest
{
    [SaveHandler(564255576904848038, "TextMeshProUGUI", typeof(TMPro.TextMeshProUGUI))]
    public class TextMeshProUGUISaveHandler : MonoSaveHandler<TMPro.TextMeshProUGUI, TextMeshProUGUISaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.text = __instance.text;
            __saveData.fontSize = __instance.fontSize;
            __saveData.fontStyle = __instance.fontStyle;
            __saveData.color = __instance.color;
            __saveData.alignment = __instance.alignment;
            __saveData.characterSpacing = __instance.characterSpacing;
            __saveData.wordSpacing = __instance.wordSpacing;
            __saveData.lineSpacing = __instance.lineSpacing;
            __saveData.paragraphSpacing = __instance.paragraphSpacing;
            __saveData.overflowMode = __instance.overflowMode;
            __saveData.textWrappingMode = __instance.textWrappingMode;

            __saveData.textPreprocessor = GetObjectId(__instance.textPreprocessor);
            __saveData.isUsingLegacyAnimationComponent = __instance.isUsingLegacyAnimationComponent;
            __saveData.onCullStateChanged = GetObjectId(__instance.onCullStateChanged);
            __saveData.raycastPadding = __instance.raycastPadding;
            __saveData.useGUILayout = __instance.useGUILayout;
            __saveData.runInEditMode = __instance.runInEditMode;
            __saveData.enabled = __instance.enabled;
            __saveData.hideFlags = __instance.hideFlags;
            __saveData.OnPreRenderText = GetInvocationList(nameof(TMPro.TextMeshProUGUI.OnPreRenderText));
        }

        public override void LoadReferences()
        {
            base.LoadReferences();
            __instance.text = __saveData.text;
            __instance.fontSize = __saveData.fontSize;
            __instance.fontStyle = __saveData.fontStyle;
            __instance.color = __saveData.color;
            __instance.alignment = __saveData.alignment;
            __instance.characterSpacing = __saveData.characterSpacing;
            __instance.wordSpacing = __saveData.wordSpacing;
            __instance.lineSpacing = __saveData.lineSpacing;
            __instance.paragraphSpacing = __saveData.paragraphSpacing;
            __instance.overflowMode = __saveData.overflowMode;
            __instance.textWrappingMode = __saveData.textWrappingMode;

            __instance.textPreprocessor = GetObjectById<TMPro.ITextPreprocessor>(__saveData.textPreprocessor);
            __instance.isUsingLegacyAnimationComponent = __saveData.isUsingLegacyAnimationComponent;
            __instance.onCullStateChanged = GetObjectById<UnityEngine.UI.MaskableGraphic.CullStateChangedEvent>(__saveData.onCullStateChanged);
            __instance.raycastPadding = __saveData.raycastPadding;
            __instance.useGUILayout = __saveData.useGUILayout;
            __instance.runInEditMode = __saveData.runInEditMode;
            __instance.enabled = __saveData.enabled;
            __instance.hideFlags = __saveData.hideFlags;
            __instance.OnPreRenderText += GetDelegate<System.Action<TMPro.TMP_TextInfo>>(__saveData.OnPreRenderText);
        }

        static TextMeshProUGUISaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
                {"CalculateLayoutInputHorizontal():mscorlib System.Void", 442434695831110131},
                {"CalculateLayoutInputVertical():mscorlib System.Void", 312806375735304374},
                {"SetVerticesDirty():mscorlib System.Void", 393965974488461672},
                {"SetLayoutDirty():mscorlib System.Void", 643516985942773683},
                {"SetMaterialDirty():mscorlib System.Void", 834693299637473172},
                {"SetAllDirty():mscorlib System.Void", 168707092270477773},
                {"Rebuild(UnityEngine.UI UnityEngine.UI.CanvasUpdate):mscorlib System.Void", 895931624857950051},
                {"GetModifiedMaterial(UnityEngine.CoreModule UnityEngine.Material):UnityEngine.CoreModule UnityEngine.Material", 513769102816969937},
                {"RecalculateClipping():mscorlib System.Void", 703607314521941626},
                {"Cull(UnityEngine.CoreModule UnityEngine.Rect,mscorlib System.Boolean):mscorlib System.Void", 102583971898702234},
                {"UpdateMeshPadding():mscorlib System.Void", 871635457097424393},
                {"ForceMeshUpdate(mscorlib System.Boolean,mscorlib System.Boolean):mscorlib System.Void", 512072641682970775},
                {"GetTextInfo(mscorlib System.String):Unity.TextMeshPro TMPro.TMP_TextInfo", 536713588346441871},
                {"ClearMesh():mscorlib System.Void", 380898387512675985},
                {"UpdateGeometry(UnityEngine.CoreModule UnityEngine.Mesh,mscorlib System.Int32):mscorlib System.Void", 670848320523109949},
                {"UpdateVertexData(Unity.TextMeshPro TMPro.TMP_VertexDataUpdateFlags):mscorlib System.Void", 564742607666156520},
                {"UpdateVertexData():mscorlib System.Void", 233246886341272541},
                {"UpdateFontAsset():mscorlib System.Void", 183724175337757970},
                {"ComputeMarginSize():mscorlib System.Void", 621795626539460454}
            };
            Infra.Singleton.__methodIdsByMethodSignaturePerType.Add(_typeReference, methodToId);
            Infra.Singleton.__methodGetterFactoryPerType.Add(_typeReference, _idToMethod);
            Infra.Singleton.__methodInfoGettersPerType.Add(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(TMPro.TextMeshProUGUI);
        static Type _typeDefinition = typeof(TMPro.TextMeshProUGUI);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                442434695831110131 => new Func<object, Delegate>((instance) => new Action(((TMPro.TextMeshProUGUI)instance).CalculateLayoutInputHorizontal)),
                312806375735304374 => new Func<object, Delegate>((instance) => new Action(((TMPro.TextMeshProUGUI)instance).CalculateLayoutInputVertical)),
                393965974488461672 => new Func<object, Delegate>((instance) => new Action(((TMPro.TextMeshProUGUI)instance).SetVerticesDirty)),
                643516985942773683 => new Func<object, Delegate>((instance) => new Action(((TMPro.TextMeshProUGUI)instance).SetLayoutDirty)),
                834693299637473172 => new Func<object, Delegate>((instance) => new Action(((TMPro.TextMeshProUGUI)instance).SetMaterialDirty)),
                168707092270477773 => new Func<object, Delegate>((instance) => new Action(((TMPro.TextMeshProUGUI)instance).SetAllDirty)),
                895931624857950051 => new Func<object, Delegate>((instance) => new Action<UnityEngine.UI.CanvasUpdate>(((TMPro.TextMeshProUGUI)instance).Rebuild)),
                513769102816969937 => new Func<object, Delegate>((instance) => new Func<UnityEngine.Material, UnityEngine.Material>(((TMPro.TextMeshProUGUI)instance).GetModifiedMaterial)),
                703607314521941626 => new Func<object, Delegate>((instance) => new Action(((TMPro.TextMeshProUGUI)instance).RecalculateClipping)),
                102583971898702234 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Rect, System.Boolean>(((TMPro.TextMeshProUGUI)instance).Cull)),
                871635457097424393 => new Func<object, Delegate>((instance) => new Action(((TMPro.TextMeshProUGUI)instance).UpdateMeshPadding)),
                512072641682970775 => new Func<object, Delegate>((instance) => new Action<System.Boolean, System.Boolean>(((TMPro.TextMeshProUGUI)instance).ForceMeshUpdate)),
                536713588346441871 => new Func<object, Delegate>((instance) => new Func<System.String, TMPro.TMP_TextInfo>(((TMPro.TextMeshProUGUI)instance).GetTextInfo)),
                380898387512675985 => new Func<object, Delegate>((instance) => new Action(((TMPro.TextMeshProUGUI)instance).ClearMesh)),
                670848320523109949 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Mesh, System.Int32>(((TMPro.TextMeshProUGUI)instance).UpdateGeometry)),
                564742607666156520 => new Func<object, Delegate>((instance) => new Action<TMPro.TMP_VertexDataUpdateFlags>(((TMPro.TextMeshProUGUI)instance).UpdateVertexData)),
                233246886341272541 => new Func<object, Delegate>((instance) => new Action(((TMPro.TextMeshProUGUI)instance).UpdateVertexData)),
                183724175337757970 => new Func<object, Delegate>((instance) => new Action(((TMPro.TextMeshProUGUI)instance).UpdateFontAsset)),
                621795626539460454 => new Func<object, Delegate>((instance) => new Action(((TMPro.TextMeshProUGUI)instance).ComputeMarginSize)),
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


    public class TextMeshProUGUISaveData : MonoSaveDataBase
    {
        public RandomId textPreprocessor;
        public System.Boolean isUsingLegacyAnimationComponent;
        public RandomId onCullStateChanged;
        public UnityEngine.Vector4 raycastPadding;
        public System.Boolean useGUILayout;
        public System.Boolean runInEditMode;
        public System.Boolean enabled;
        public UnityEngine.HideFlags hideFlags;
        public InvocationList OnPreRenderText = new();
        public string text;
        public float fontSize;
        public TMPro.FontStyles fontStyle;
        public UnityEngine.Color color;
        public TextAlignmentOptions alignment;
        public float characterSpacing;
        public float wordSpacing;
        public float lineSpacing;
        public float paragraphSpacing;
        public TextOverflowModes overflowMode;
        public TextWrappingModes textWrappingMode;

    }


    public class StaticTextMeshProUGUISubtitute : StaticSubtitute<TMPro.TextMeshProUGUI>
    {
    }

    [SaveHandler(373649652201181185, "StaticTextMeshProUGUISubtitute", typeof(StaticTextMeshProUGUISubtitute), generationMode: SaveHandlerGenerationMode.FullAutomata, staticHandlerOf: typeof(TMPro.TextMeshProUGUI))]
    public class StaticTextMeshProUGUISaveHandler : StaticSaveHandlerBase<StaticTextMeshProUGUISubtitute, StaticTextMeshProUGUISaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();

        }

        public override void LoadReferences()
        {
            base.LoadReferences();

        }
    }


    public class StaticTextMeshProUGUISaveData : StaticSaveDataBase
    {

    }

}