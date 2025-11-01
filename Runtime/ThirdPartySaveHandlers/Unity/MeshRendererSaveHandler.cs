
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.UnitySHs
{
    [SaveHandler(id: 982070258784696813, dataGroupName: nameof(MeshRenderer), typeof(MeshRenderer), order: -8)]
    public class MeshRendererSaveHandler : MonoSaveHandler<MeshRenderer, MeshRendererSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.additionalVertexStreams = GetAssetId(__instance.additionalVertexStreams);
            __saveData.enlightenVertexStream = GetAssetId(__instance.enlightenVertexStream);
            __saveData.scaleInLightmap = __instance.scaleInLightmap;
            __saveData.receiveGI = __instance.receiveGI;
            __saveData.stitchLightmapSeams = __instance.stitchLightmapSeams;
            __saveData.bounds.ReadFrom(__instance.bounds);
            __saveData.localBounds.ReadFrom(__instance.localBounds);
            __saveData.enabled = __instance.enabled;
            __saveData.shadowCastingMode = __instance.shadowCastingMode;
            __saveData.receiveShadows = __instance.receiveShadows;
            __saveData.forceRenderingOff = __instance.forceRenderingOff;
            __saveData.motionVectorGenerationMode = __instance.motionVectorGenerationMode;
            __saveData.lightProbeUsage = __instance.lightProbeUsage;
            __saveData.reflectionProbeUsage = __instance.reflectionProbeUsage;
            __saveData.renderingLayerMask = __instance.renderingLayerMask;
            __saveData.rendererPriority = __instance.rendererPriority;
            __saveData.rayTracingMode = __instance.rayTracingMode;
            __saveData.rayTracingAccelerationStructureBuildFlags = __instance.rayTracingAccelerationStructureBuildFlags;
            __saveData.rayTracingAccelerationStructureBuildFlagsOverride = __instance.rayTracingAccelerationStructureBuildFlagsOverride;
            __saveData.sortingLayerName = __instance.sortingLayerName;
            __saveData.sortingLayerID = __instance.sortingLayerID;
            __saveData.sortingOrder = __instance.sortingOrder;
            __saveData.allowOcclusionWhenDynamic = __instance.allowOcclusionWhenDynamic;
            __saveData.lightProbeProxyVolumeOverride = GetObjectId(__instance.lightProbeProxyVolumeOverride);
            __saveData.probeAnchor = GetObjectId(__instance.probeAnchor);
            __saveData.hideFlags = __instance.hideFlags;
            //__saveData.sharedMaterials = GetObjectId(__instance.sharedMaterials);
            //__saveData.materials = GetObjectId(__instance.materials);
            GetAssetIdList(__instance.sharedMaterials,__saveData.sharedMaterials);
            GetAssetIdList(__instance.materials,__saveData.materials);
            //__saveData.sharedMaterials.Clear();

            //for (int i = 0; i < __instance.sharedMaterials.Length; i++)
            //{
            //    var material = __instance.sharedMaterials[i];
            //    if (material != null)
            //    {
            //        var assetId = AddressableDb.Singleton.GetAssetIdByAssetName(material);
            //        __saveData.sharedMaterials.Add(assetId);
            //    }
            //}
        }

        public override void LoadReferences()
        {
            base.LoadReferences();

            //List<Material> materials = new List<Material>();

            //foreach (var assetId in __saveData.sharedMaterials)
            //{
            //    var id = assetId;

            //    var material = AddressableDb.Singleton.GetAssetByIdOrFallback<Material>(null, ref id);

            //    if (material != null)
            //    {
            //        materials.Add(material);
            //    }
            //}

            //__instance.sharedMaterials = materials.ToArray();
            //__instance.sharedMaterials = GetObjectById<UnityEngine.Material[]>(__saveData.sharedMaterials);
            //__instance.materials = GetObjectById<UnityEngine.Material[]>(__saveData.materials);
            __instance.sharedMaterials = GetAssetList<UnityEngine.Material>(__saveData.sharedMaterials);
            __instance.materials = GetAssetList<UnityEngine.Material>(__saveData.materials);
            __instance.additionalVertexStreams = GetAssetById(__saveData.additionalVertexStreams, __instance.additionalVertexStreams);
            __instance.enlightenVertexStream = GetAssetById(__saveData.enlightenVertexStream, __instance.enlightenVertexStream);
            __instance.scaleInLightmap = __saveData.scaleInLightmap;
            __instance.receiveGI = __saveData.receiveGI;
            __instance.stitchLightmapSeams = __saveData.stitchLightmapSeams;
            __saveData.bounds.WriteTo(__instance.bounds);
            __saveData.localBounds.WriteTo(__instance.localBounds);
            __instance.enabled = __saveData.enabled;
            __instance.shadowCastingMode = __saveData.shadowCastingMode;
            __instance.receiveShadows = __saveData.receiveShadows;
            __instance.forceRenderingOff = __saveData.forceRenderingOff;
            __instance.motionVectorGenerationMode = __saveData.motionVectorGenerationMode;
            __instance.lightProbeUsage = __saveData.lightProbeUsage;
            __instance.reflectionProbeUsage = __saveData.reflectionProbeUsage;
            __instance.renderingLayerMask = __saveData.renderingLayerMask;
            __instance.rendererPriority = __saveData.rendererPriority;
            __instance.rayTracingMode = __saveData.rayTracingMode;
            __instance.rayTracingAccelerationStructureBuildFlags = __saveData.rayTracingAccelerationStructureBuildFlags;
            __instance.rayTracingAccelerationStructureBuildFlagsOverride = __saveData.rayTracingAccelerationStructureBuildFlagsOverride;
            __instance.sortingLayerName = __saveData.sortingLayerName;
            __instance.sortingLayerID = __saveData.sortingLayerID;
            __instance.sortingOrder = __saveData.sortingOrder;
            __instance.allowOcclusionWhenDynamic = __saveData.allowOcclusionWhenDynamic;
            __instance.lightProbeProxyVolumeOverride = GetObjectById<UnityEngine.GameObject>(__saveData.lightProbeProxyVolumeOverride);
            __instance.probeAnchor = GetObjectById<UnityEngine.Transform>(__saveData.probeAnchor);
            __instance.hideFlags = __saveData.hideFlags;
        }
    }

    public class MeshRendererSaveData : MonoSaveDataBase
    {
        public List<RandomId> sharedMaterials=new();
        public List<RandomId> materials=new();
        public RandomId additionalVertexStreams;
        public RandomId enlightenVertexStream;
        public System.Single scaleInLightmap;
        public UnityEngine.ReceiveGI receiveGI;
        public System.Boolean stitchLightmapSeams;
        public DevTest.BoundsSaveData bounds = new();
        public DevTest.BoundsSaveData localBounds = new();
        public System.Boolean enabled;
        public UnityEngine.Rendering.ShadowCastingMode shadowCastingMode;
        public System.Boolean receiveShadows;
        public System.Boolean forceRenderingOff;
        public UnityEngine.MotionVectorGenerationMode motionVectorGenerationMode;
        public UnityEngine.Rendering.LightProbeUsage lightProbeUsage;
        public UnityEngine.Rendering.ReflectionProbeUsage reflectionProbeUsage;
        public System.UInt32 renderingLayerMask;
        public System.Int32 rendererPriority;
        public UnityEngine.Experimental.Rendering.RayTracingMode rayTracingMode;
        public UnityEngine.Rendering.RayTracingAccelerationStructureBuildFlags rayTracingAccelerationStructureBuildFlags;
        public System.Boolean rayTracingAccelerationStructureBuildFlagsOverride;
        public System.String sortingLayerName;
        public System.Int32 sortingLayerID;
        public System.Int32 sortingOrder;
        public System.Boolean allowOcclusionWhenDynamic;
        public RandomId lightProbeProxyVolumeOverride;
        public RandomId probeAnchor;
        public UnityEngine.HideFlags hideFlags;
    }
}
