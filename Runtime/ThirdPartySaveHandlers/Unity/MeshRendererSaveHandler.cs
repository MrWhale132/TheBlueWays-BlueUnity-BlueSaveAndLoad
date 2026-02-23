using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.UnitySHs
{
    [SaveHandler(id: 982070258784696813, dataGroupName: nameof(MeshRenderer), typeof(MeshRenderer), order: -7, dependsOn: new Type[] { typeof(MeshFilter) })]
    public class MeshRendererSaveHandler : MonoSaveHandler<MeshRenderer, MeshRendererSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.additionalVertexStreams = GetAssetId(__instance.additionalVertexStreams);
            __saveData.enlightenVertexStream = GetAssetId(__instance.enlightenVertexStream);
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

            var meshFilter = __instance.gameObject.GetComponent<MeshFilter>();

            __saveData.meshFilter = GetObjectId(meshFilter);

            __saveData.shaders.Clear();

            __saveData.materials.Clear();
            __saveData.materialAssetIds.Clear();
            foreach (var mat in __instance.materials)
            {
                var id = GetObjectId(mat);
                __saveData.materials.Add(id);

                AssetIdMap.ObjectIdToAssetId.TryGetValue(id, out var assetId);
                __saveData.materialAssetIds.Add(assetId);
            }

            __saveData.sharedMaterials.Clear();
            __saveData.sharedMaterialAssetIds.Clear();
            foreach (var mat in __instance.sharedMaterials)
            {
                var id = GetObjectId(mat);
                __saveData.sharedMaterials.Add(id);

                var shaderId = GetObjectId(mat.shader);
                __saveData.shaders.Add(shaderId);

                AssetIdMap.ObjectIdToAssetId.TryGetValue(id, out var assetId);
                __saveData.sharedMaterialAssetIds.Add(assetId);
            }
        }


        public override void LoadPhase1()
        {
            base.LoadPhase1();


            if (__saveData.sharedMaterials.IsNotNullAndNotEmpty())
            {
                if (__saveData.meshFilter.IsNotDefault)
                {
                    var meshFilter = GetObjectById<MeshFilter>(__saveData.meshFilter);
                    var mesh = meshFilter.sharedMesh;

                    if (mesh.subMeshCount != __saveData.materials.Count)
                    {
                        Debug.LogError($"ERROR MeshRendererSaveHandler: the MeshFilter's submeshcount is not equal to the number of save materials.\n" +
                            $"Number of saved materials: {__saveData.materials.Count}\n" +
                            $"submeshcount: {mesh.subMeshCount}\n" +
                            $"Renderer's ObjectId: {HandledObjectId}, MeshFilter's ObjectId: {__saveData.meshFilter}");
                    }
                }



                var materialsToSet = new Material[__saveData.sharedMaterials.Count];

                for (int i = 0; i < __saveData.sharedMaterials.Count; i++)
                {
                    var objectId = __saveData.sharedMaterials[i];
                    var assetId = __saveData.sharedMaterialAssetIds[i];
                    var shaderId = __saveData.shaders[i];


                    if (AssetIdMap.HasInstance(objectId, out Material instance))
                    {

#if UNITY_EDITOR
                        if (__saveData.materials[i].IsNotDefault)
                        {
                            AssetIdMap.LogSharedPrivateCopiesError(HandledObjectId, objectId, __saveData.materials[i]);
                            materialsToSet[i] = null;
                        }
#else
                        materialsToSet[i] = instance;
#endif
                    }
                    if (shaderId.IsNotDefault)
                    {
                        var shader = GetObjectById<Shader>(shaderId);

                        var mat = new Material(shader);
                        materialsToSet[i] = mat;
                    }
                    else if (assetId.IsNotDefault)
                    {
                        var orig = GetAssetById2<Material>(assetId, null);

                        if (orig != null)
                        {
                            var copy = Object.Instantiate(orig); //in editor-time assets returned by asset providers are pointing to the original asset, copy to protect accidental editor-time modifications
                            materialsToSet[i] = copy;
                        }
                        else
                        {
                            materialsToSet[i] = null;
                            Debug.LogError($"Object: {__saveData._ObjectId_} had an assetid: {assetId} but no asset was found with such id.\n" +
                                $"This might cause null reference exceptions downstream.");
                        }
                    }
                    else
                    {
                        materialsToSet[i] = null;

                        if (objectId.IsNotDefault)
                        {
                            Debug.LogError($"Can not create material: {objectId} because it does not have neither a shaderId nor an assetId.");
                        }
                    }
                }



                //note: actual internal array size that will be set: min(value.length, mesh.submeshcount)
                //for this reason meshrenderer depends on meshfilter to be set first

                //important!: SkinnedMeshRenderer: Some versions auto-expand materials array to match submesh count

                __instance.sharedMaterials = materialsToSet;

                if (__saveData.materials.IsNotNullAndNotEmpty())
                {
                    materialsToSet = __instance.materials; ///(too lazy to rewrite) trigger unity's copy mechanism by accessing the .mesh property, which sets .sharedMesh to the newly copied instance too
                    //the actual array returned by .materials can have fewer or more elements than what we saved
                }


                for (int i = 0; i < materialsToSet.Length && i < __saveData.materials.Count; i++)
                {
                    var mat = materialsToSet[i];
                    var id = __saveData.materials[i];

                    //note: the handled of the material does not know yet that the asset it was originated from was not found, therefore no instance
                    ///could be created for it. By not adding a null to <see cref="AssetIdMap"/> its handler will try to create a new instance.
                    if (mat != null)
                    {
                        AssetIdMap.AddInstance(id, mat);
                    }
                }
            }


            __instance.additionalVertexStreams = GetAssetById(__saveData.additionalVertexStreams, __instance.additionalVertexStreams);
            __instance.enlightenVertexStream = GetAssetById(__saveData.enlightenVertexStream, __instance.enlightenVertexStream);
            //todo: for most cases these dont have to bet set, in some cases however they are need to handle manually
            //a config is needed to drive this.
            //__instance.bounds = __saveData.bounds.WriteInto(__instance.bounds);
            //__instance.localBounds = __saveData.localBounds.WriteInto(__instance.localBounds);
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
        public RandomId meshFilter;
        public List<RandomId> sharedMaterialAssetIds = new();
        public List<RandomId> materialAssetIds = new();
        public List<RandomId> sharedMaterials = new();
        public List<RandomId> materials = new();
        public List<RandomId> shaders = new();
        public RandomId additionalVertexStreams;
        public RandomId enlightenVertexStream;
        public CustomSaveData<Bounds> bounds = CustomSaveData.CreateFor<Bounds>();
        public CustomSaveData<Bounds> localBounds = CustomSaveData.CreateFor<Bounds>();
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
