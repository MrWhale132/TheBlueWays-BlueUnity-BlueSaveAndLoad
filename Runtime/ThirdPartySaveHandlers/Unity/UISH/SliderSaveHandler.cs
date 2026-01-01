
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.UtilScripts;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.ThirdPartySaveHandlers.Unity.UISH
{
    [SaveHandler(367908648901075775, "Slider", typeof(UnityEngine.UI.Slider), generationMode: SaveHandlerGenerationMode.Manual)]
    public class SliderSaveHandler : MonoSaveHandler<UnityEngine.UI.Slider, SliderSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.maxValue = __instance.maxValue;
            __saveData.minValue = __instance.minValue;
            __saveData.wholeNumbers = __instance.wholeNumbers;
            __saveData.value = __instance.value;
            __saveData.direction = __instance.direction;
            __saveData.fillRect = GetObjectId(__instance.fillRect);
            __saveData.handleRect = GetObjectId(__instance.handleRect);
            __saveData.onValueChanged = GetObjectId(__instance.onValueChanged);
            __saveData.useGUILayout = __instance.useGUILayout;
            __saveData.enabled = __instance.enabled;
            __saveData.hideFlags = __instance.hideFlags;
        }

        public override void LoadReferences()
        {
            base.LoadReferences();
            __instance.maxValue = __saveData.maxValue;
            __instance.minValue = __saveData.minValue;
            __instance.wholeNumbers = __saveData.wholeNumbers;
            __instance.value = __saveData.value;
            __instance.direction = __saveData.direction;
            __instance.fillRect = GetObjectById<UnityEngine.RectTransform>(__saveData.fillRect);
            __instance.handleRect = GetObjectById<UnityEngine.RectTransform>(__saveData.handleRect);
            __instance.onValueChanged = GetObjectById<UnityEngine.UI.Slider.SliderEvent>(__saveData.onValueChanged);
            __instance.useGUILayout = __saveData.useGUILayout;
            __instance.enabled = __saveData.enabled;
            __instance.hideFlags = __saveData.hideFlags;
        }

        static SliderSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
				/// methodToId map for <see cref="Slider"/>
				{$"SetValueWithoutNotify(mscorlib System.Single):mscorlib System.Void", 717541424392232241},
                {$"Rebuild(UnityEngine.UI UnityEngine.UI.CanvasUpdate):mscorlib System.Void", 666080250260318523},
                {$"LayoutComplete():mscorlib System.Void", 752107241317611357},
                {$"GraphicUpdateComplete():mscorlib System.Void", 463628125397471848},
                {$"OnPointerDown(UnityEngine.UI UnityEngine.EventSystems.PointerEventData):mscorlib System.Void", 946983983355806015},
                {$"OnDrag(UnityEngine.UI UnityEngine.EventSystems.PointerEventData):mscorlib System.Void", 103308184394586331},
                {$"OnMove(UnityEngine.UI UnityEngine.EventSystems.AxisEventData):mscorlib System.Void", 247974576049146583},
                {$"FindSelectableOnLeft():UnityEngine.UI UnityEngine.UI.Selectable", 564780959977856597},
                {$"FindSelectableOnRight():UnityEngine.UI UnityEngine.UI.Selectable", 703983103525890277},
                {$"FindSelectableOnUp():UnityEngine.UI UnityEngine.UI.Selectable", 735702469919264887},
                {$"FindSelectableOnDown():UnityEngine.UI UnityEngine.UI.Selectable", 168157642407767985},
                {$"OnInitializePotentialDrag(UnityEngine.UI UnityEngine.EventSystems.PointerEventData):mscorlib System.Void", 299041329426091221},
                {$"SetDirection(UnityEngine.UI UnityEngine.UI.Slider+Direction,mscorlib System.Boolean):mscorlib System.Void", 957816905125137978},
            };
            Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
            Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
            Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(UnityEngine.UI.Slider);
        static Type _typeDefinition = typeof(UnityEngine.UI.Slider);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                717541424392232241 => new Func<object, Delegate>((instance) => new Action<System.Single>(((UnityEngine.UI.Slider)instance).SetValueWithoutNotify)),
                666080250260318523 => new Func<object, Delegate>((instance) => new Action<UnityEngine.UI.CanvasUpdate>(((UnityEngine.UI.Slider)instance).Rebuild)),
                752107241317611357 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.UI.Slider)instance).LayoutComplete)),
                463628125397471848 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.UI.Slider)instance).GraphicUpdateComplete)),
                946983983355806015 => new Func<object, Delegate>((instance) => new Action<UnityEngine.EventSystems.PointerEventData>(((UnityEngine.UI.Slider)instance).OnPointerDown)),
                103308184394586331 => new Func<object, Delegate>((instance) => new Action<UnityEngine.EventSystems.PointerEventData>(((UnityEngine.UI.Slider)instance).OnDrag)),
                247974576049146583 => new Func<object, Delegate>((instance) => new Action<UnityEngine.EventSystems.AxisEventData>(((UnityEngine.UI.Slider)instance).OnMove)),
                564780959977856597 => new Func<object, Delegate>((instance) => new Func<UnityEngine.UI.Selectable>(((UnityEngine.UI.Slider)instance).FindSelectableOnLeft)),
                703983103525890277 => new Func<object, Delegate>((instance) => new Func<UnityEngine.UI.Selectable>(((UnityEngine.UI.Slider)instance).FindSelectableOnRight)),
                735702469919264887 => new Func<object, Delegate>((instance) => new Func<UnityEngine.UI.Selectable>(((UnityEngine.UI.Slider)instance).FindSelectableOnUp)),
                168157642407767985 => new Func<object, Delegate>((instance) => new Func<UnityEngine.UI.Selectable>(((UnityEngine.UI.Slider)instance).FindSelectableOnDown)),
                299041329426091221 => new Func<object, Delegate>((instance) => new Action<UnityEngine.EventSystems.PointerEventData>(((UnityEngine.UI.Slider)instance).OnInitializePotentialDrag)),
                957816905125137978 => new Func<object, Delegate>((instance) => new Action<UnityEngine.UI.Slider.Direction, System.Boolean>(((UnityEngine.UI.Slider)instance).SetDirection)),
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


    public class SliderSaveData : MonoSaveDataBase
    {
        public float value;
        public float maxValue;
        public float minValue;
        public bool wholeNumbers;
        public UnityEngine.UI.Slider.Direction direction;
        public RandomId fillRect;
        public RandomId handleRect;
        public RandomId onValueChanged;
        public System.Boolean useGUILayout;
        public System.Boolean runInEditMode;
        public System.Boolean enabled;
        public UnityEngine.HideFlags hideFlags;
    }
}
