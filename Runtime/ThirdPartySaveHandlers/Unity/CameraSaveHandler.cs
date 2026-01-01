
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.Extensions;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity
{
    [SaveHandler(98989312353456, nameof(Camera), typeof(Camera))]
    public class CameraSaveHandler : MonoSaveHandler<Camera, CameraSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.Orthographic = __instance.orthographic;
            __saveData.FieldOfView = __instance.fieldOfView;
            __saveData.NearClipPlane = __instance.nearClipPlane;
            __saveData.FarClipPlane = __instance.farClipPlane;
            __saveData.PhysicalCamera = __instance.usePhysicalProperties;

            __saveData.AllowDynamicResolution = __instance.allowDynamicResolution;
            __saveData.CullingMask = __instance.cullingMask;
            __saveData.OcclusionCulling = __instance.useOcclusionCulling;
            __saveData.TargetDisplay = __instance.targetDisplay;
            __saveData.TargetTexture = GetObjectId(__instance.targetTexture);
            __saveData.Depth = __instance.depth;
            __saveData.Rect = __instance.rect;
        }

        public override void CreateObject()
        {
            base.CreateObject();

            //var go = Infra.Singleton.GetObjectById<GameObject>(__saveData.GameObjectId);

            //var cam = go.GetComponent<Camera>();
        }

        public override void LoadReferences()
        {
            base.LoadReferences();

            __instance.targetTexture = GetObjectById<RenderTexture>(__saveData.TargetTexture);
        }

        public override void LoadValues()
        {
            base.LoadValues();

            __instance.orthographic = __saveData.Orthographic;
            __instance.fieldOfView = __saveData.FieldOfView;
            __instance.nearClipPlane = __saveData.NearClipPlane;
            __instance.farClipPlane = __saveData.FarClipPlane;
            __instance.usePhysicalProperties = __saveData.PhysicalCamera;
            __instance.allowDynamicResolution = __saveData.AllowDynamicResolution;
            __instance.cullingMask = __saveData.CullingMask;
            __instance.useOcclusionCulling = __saveData.OcclusionCulling;
            __instance.targetDisplay = __saveData.TargetDisplay;
            __instance.depth = __saveData.Depth;
            __instance.rect = __saveData.Rect;
        }
    }

    public class CameraSaveData : MonoSaveDataBase
    {
        public bool Orthographic;
        public float FieldOfView;
        public float NearClipPlane;
        public float FarClipPlane;
        public bool PhysicalCamera;

        public bool AllowDynamicResolution;
        public int CullingMask;
        public bool OcclusionCulling;

        public int TargetDisplay;
        public RandomId TargetTexture;
        public float Depth;
        public Rect Rect;
    }



    


}
