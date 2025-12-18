using UnityEngine;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Assets._Project.Scripts.SaveAndLoad.Runtime.ThirdPartySaveHandlers.Unity.UnityEngineSHs
{
	[SaveHandler(514865915141030483, "ParticleSystemRenderer", typeof(UnityEngine.ParticleSystemRenderer))]
	public class ParticleSystemRendererSaveHandler : MonoSaveHandler<UnityEngine.ParticleSystemRenderer, ParticleSystemRendererSaveData> 
	{
   //     public override void _AssignInstance()
   //     {
   //         var go = Infra.Singleton.GetObjectById<GameObject>(__saveData.GameObjectId);
			/////<see cref="UnityEngine.ParticleSystem"/> already added the ParticleSystemRenderer component
   //         __instance = go.GetComponent<ParticleSystemRenderer>();
   //     }
        public override void WriteSaveData()
		{
			base.WriteSaveData();
			__saveData.alignment = __instance.alignment;
			__saveData.renderMode = __instance.renderMode;
			__saveData.meshDistribution = __instance.meshDistribution;
			__saveData.sortMode = __instance.sortMode;
			__saveData.lengthScale = __instance.lengthScale;
			__saveData.velocityScale = __instance.velocityScale;
			__saveData.cameraVelocityScale = __instance.cameraVelocityScale;
			__saveData.normalDirection = __instance.normalDirection;
			__saveData.shadowBias = __instance.shadowBias;
			__saveData.sortingFudge = __instance.sortingFudge;
			__saveData.minParticleSize = __instance.minParticleSize;
			__saveData.maxParticleSize = __instance.maxParticleSize;
			__saveData.pivot = __instance.pivot;
			__saveData.flip = __instance.flip;
			__saveData.maskInteraction = __instance.maskInteraction;
			__saveData.trailMaterial = GetAssetId(__instance.trailMaterial);
			__saveData.enableGPUInstancing = __instance.enableGPUInstancing;
			__saveData.allowRoll = __instance.allowRoll;
			__saveData.freeformStretching = __instance.freeformStretching;
			__saveData.rotateWithStretchDirection = __instance.rotateWithStretchDirection;
			__saveData.mesh = GetAssetId(__instance.mesh);
			//__saveData.bounds.ReadFrom(__instance.bounds);
			//__saveData.localBounds.ReadFrom(__instance.localBounds);
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
		}

		public override void LoadReferences()
		{
			base.LoadReferences();
			__instance.alignment = __saveData.alignment;
			__instance.renderMode = __saveData.renderMode;
			__instance.meshDistribution = __saveData.meshDistribution;
			__instance.sortMode = __saveData.sortMode;
			__instance.lengthScale = __saveData.lengthScale;
			__instance.velocityScale = __saveData.velocityScale;
			__instance.cameraVelocityScale = __saveData.cameraVelocityScale;
			__instance.normalDirection = __saveData.normalDirection;
			__instance.shadowBias = __saveData.shadowBias;
			__instance.sortingFudge = __saveData.sortingFudge;
			__instance.minParticleSize = __saveData.minParticleSize;
			__instance.maxParticleSize = __saveData.maxParticleSize;
			__instance.pivot = __saveData.pivot;
			__instance.flip = __saveData.flip;
			__instance.maskInteraction = __saveData.maskInteraction;
			__instance.trailMaterial = GetAssetById(__saveData.trailMaterial, __instance.trailMaterial);
			__instance.enableGPUInstancing = __saveData.enableGPUInstancing;
			__instance.allowRoll = __saveData.allowRoll;
			__instance.freeformStretching = __saveData.freeformStretching;
			__instance.rotateWithStretchDirection = __saveData.rotateWithStretchDirection;
			__instance.mesh = GetAssetById(__saveData.mesh, __instance.mesh);
			//__saveData.bounds.WriteTo(__instance.bounds);
			//__saveData.localBounds.WriteTo(__instance.localBounds);
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

		static ParticleSystemRendererSaveHandler()
		{
			Dictionary<string, long> methodToId = new()
			{
				{"GetMeshes(UnityEngine.CoreModule UnityEngine.Mesh[]):mscorlib System.Int32", 893960047498605073},
				{"SetMeshes(UnityEngine.CoreModule UnityEngine.Mesh[],mscorlib System.Int32):mscorlib System.Void", 934681053424171071},
				{"SetMeshes(UnityEngine.CoreModule UnityEngine.Mesh[]):mscorlib System.Void", 354482752644596744},
				{"GetMeshWeightings(mscorlib System.Single[]):mscorlib System.Int32", 811281752331762471},
				{"SetMeshWeightings(mscorlib System.Single[],mscorlib System.Int32):mscorlib System.Void", 955283930892742625},
				{"SetMeshWeightings(mscorlib System.Single[]):mscorlib System.Void", 907272041098102479},
				{"BakeMesh(UnityEngine.CoreModule UnityEngine.Mesh,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemBakeMeshOptions):mscorlib System.Void", 953834186959913647},
				{"BakeMesh(UnityEngine.CoreModule UnityEngine.Mesh,UnityEngine.CoreModule UnityEngine.Camera,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemBakeMeshOptions):mscorlib System.Void", 467224184168036266},
				{"BakeTrailsMesh(UnityEngine.CoreModule UnityEngine.Mesh,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemBakeMeshOptions):mscorlib System.Void", 317218609218455401},
				{"BakeTrailsMesh(UnityEngine.CoreModule UnityEngine.Mesh,UnityEngine.CoreModule UnityEngine.Camera,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemBakeMeshOptions):mscorlib System.Void", 273964091792364633},
				{"BakeTexture(UnityEngine.CoreModule UnityEngine.Texture2D&,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemBakeTextureOptions):mscorlib System.Int32", 115951587661252509},
				{"BakeTexture(UnityEngine.CoreModule UnityEngine.Texture2D&,UnityEngine.CoreModule UnityEngine.Camera,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemBakeTextureOptions):mscorlib System.Int32", 434222680694371248},
				{"BakeTexture(UnityEngine.CoreModule UnityEngine.Texture2D&,UnityEngine.CoreModule UnityEngine.Texture2D&,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemBakeTextureOptions):mscorlib System.Int32", 621147742954975430},
				{"BakeTexture(UnityEngine.CoreModule UnityEngine.Texture2D&,UnityEngine.CoreModule UnityEngine.Texture2D&,UnityEngine.CoreModule UnityEngine.Camera,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemBakeTextureOptions):mscorlib System.Int32", 879326189228000498},
				{"BakeTrailsTexture(UnityEngine.CoreModule UnityEngine.Texture2D&,UnityEngine.CoreModule UnityEngine.Texture2D&,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemBakeTextureOptions):mscorlib System.Int32", 829648973671894046},
				{"BakeTrailsTexture(UnityEngine.CoreModule UnityEngine.Texture2D&,UnityEngine.CoreModule UnityEngine.Texture2D&,UnityEngine.CoreModule UnityEngine.Camera,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemBakeTextureOptions):mscorlib System.Int32", 820405851385358760},
				{"SetActiveVertexStreams(mscorlib System.Collections.Generic.List<UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemVertexStream>):mscorlib System.Void", 113134341733405356},
				{"GetActiveVertexStreams(mscorlib System.Collections.Generic.List<UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemVertexStream>):mscorlib System.Void", 832388658831312303},
				{"SetActiveTrailVertexStreams(mscorlib System.Collections.Generic.List<UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemVertexStream>):mscorlib System.Void", 379515402898999193},
				{"GetActiveTrailVertexStreams(mscorlib System.Collections.Generic.List<UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemVertexStream>):mscorlib System.Void", 862457641942009208}
			};
			Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
			Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
			Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
		}
		static Type _typeReference = typeof(UnityEngine.ParticleSystemRenderer);
		static Type _typeDefinition = typeof(UnityEngine.ParticleSystemRenderer);
		static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
		public static Func<object, Delegate> _idToMethod(long id)
		{
			Func<object, Delegate> method = id switch
			{
				893960047498605073 => new Func<object, Delegate>((instance) => new Func<UnityEngine.Mesh[], System.Int32>(((UnityEngine.ParticleSystemRenderer)instance).GetMeshes)),
				934681053424171071 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Mesh[], System.Int32>(((UnityEngine.ParticleSystemRenderer)instance).SetMeshes)),
				354482752644596744 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Mesh[]>(((UnityEngine.ParticleSystemRenderer)instance).SetMeshes)),
				811281752331762471 => new Func<object, Delegate>((instance) => new Func<System.Single[], System.Int32>(((UnityEngine.ParticleSystemRenderer)instance).GetMeshWeightings)),
				955283930892742625 => new Func<object, Delegate>((instance) => new Action<System.Single[], System.Int32>(((UnityEngine.ParticleSystemRenderer)instance).SetMeshWeightings)),
				907272041098102479 => new Func<object, Delegate>((instance) => new Action<System.Single[]>(((UnityEngine.ParticleSystemRenderer)instance).SetMeshWeightings)),
				953834186959913647 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Mesh, UnityEngine.ParticleSystemBakeMeshOptions>(((UnityEngine.ParticleSystemRenderer)instance).BakeMesh)),
				467224184168036266 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Mesh, UnityEngine.Camera, UnityEngine.ParticleSystemBakeMeshOptions>(((UnityEngine.ParticleSystemRenderer)instance).BakeMesh)),
				317218609218455401 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Mesh, UnityEngine.ParticleSystemBakeMeshOptions>(((UnityEngine.ParticleSystemRenderer)instance).BakeTrailsMesh)),
				273964091792364633 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Mesh, UnityEngine.Camera, UnityEngine.ParticleSystemBakeMeshOptions>(((UnityEngine.ParticleSystemRenderer)instance).BakeTrailsMesh)),
				113134341733405356 => new Func<object, Delegate>((instance) => new Action<System.Collections.Generic.List<UnityEngine.ParticleSystemVertexStream>>(((UnityEngine.ParticleSystemRenderer)instance).SetActiveVertexStreams)),
				832388658831312303 => new Func<object, Delegate>((instance) => new Action<System.Collections.Generic.List<UnityEngine.ParticleSystemVertexStream>>(((UnityEngine.ParticleSystemRenderer)instance).GetActiveVertexStreams)),
				379515402898999193 => new Func<object, Delegate>((instance) => new Action<System.Collections.Generic.List<UnityEngine.ParticleSystemVertexStream>>(((UnityEngine.ParticleSystemRenderer)instance).SetActiveTrailVertexStreams)),
				862457641942009208 => new Func<object, Delegate>((instance) => new Action<System.Collections.Generic.List<UnityEngine.ParticleSystemVertexStream>>(((UnityEngine.ParticleSystemRenderer)instance).GetActiveTrailVertexStreams)),
				_ => Infra.Singleton.GetIdToMethodMapForType(_typeReference.BaseType)(id),
			};
			return method;
		}
		public static MethodInfo _idToMethodInfo(long id)
		{
			MethodInfo methodDef = id switch
			{
				115951587661252509 => typeof(UnityEngine.ParticleSystemRenderer).GetMethod(nameof(UnityEngine.ParticleSystemRenderer.BakeTexture), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Texture2D).MakeByRefType(), typeof(UnityEngine.ParticleSystemBakeTextureOptions) }, null),
				434222680694371248 => typeof(UnityEngine.ParticleSystemRenderer).GetMethod(nameof(UnityEngine.ParticleSystemRenderer.BakeTexture), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Texture2D).MakeByRefType(), typeof(UnityEngine.Camera), typeof(UnityEngine.ParticleSystemBakeTextureOptions) }, null),
				621147742954975430 => typeof(UnityEngine.ParticleSystemRenderer).GetMethod(nameof(UnityEngine.ParticleSystemRenderer.BakeTexture), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Texture2D).MakeByRefType(), typeof(UnityEngine.Texture2D).MakeByRefType(), typeof(UnityEngine.ParticleSystemBakeTextureOptions) }, null),
				879326189228000498 => typeof(UnityEngine.ParticleSystemRenderer).GetMethod(nameof(UnityEngine.ParticleSystemRenderer.BakeTexture), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Texture2D).MakeByRefType(), typeof(UnityEngine.Texture2D).MakeByRefType(), typeof(UnityEngine.Camera), typeof(UnityEngine.ParticleSystemBakeTextureOptions) }, null),
				829648973671894046 => typeof(UnityEngine.ParticleSystemRenderer).GetMethod(nameof(UnityEngine.ParticleSystemRenderer.BakeTrailsTexture), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Texture2D).MakeByRefType(), typeof(UnityEngine.Texture2D).MakeByRefType(), typeof(UnityEngine.ParticleSystemBakeTextureOptions) }, null),
				820405851385358760 => typeof(UnityEngine.ParticleSystemRenderer).GetMethod(nameof(UnityEngine.ParticleSystemRenderer.BakeTrailsTexture), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Texture2D).MakeByRefType(), typeof(UnityEngine.Texture2D).MakeByRefType(), typeof(UnityEngine.Camera), typeof(UnityEngine.ParticleSystemBakeTextureOptions) }, null),
				_ => Infra.Singleton.GetMethodInfoIdToMethodMapForType(_typeReference.BaseType)(id),
			};
			return methodDef;
		}
	}


	public class ParticleSystemRendererSaveData : MonoSaveDataBase 
	{
		public UnityEngine.ParticleSystemRenderSpace alignment;
		public UnityEngine.ParticleSystemRenderMode renderMode;
		public UnityEngine.ParticleSystemMeshDistribution meshDistribution;
		public UnityEngine.ParticleSystemSortMode sortMode;
		public System.Single lengthScale;
		public System.Single velocityScale;
		public System.Single cameraVelocityScale;
		public System.Single normalDirection;
		public System.Single shadowBias;
		public System.Single sortingFudge;
		public System.Single minParticleSize;
		public System.Single maxParticleSize;
		public UnityEngine.Vector3 pivot;
		public UnityEngine.Vector3 flip;
		public UnityEngine.SpriteMaskInteraction maskInteraction;
		public RandomId trailMaterial;
		public System.Boolean enableGPUInstancing;
		public System.Boolean allowRoll;
		public System.Boolean freeformStretching;
		public System.Boolean rotateWithStretchDirection;
		public RandomId mesh;
		//public DevTest.BoundsSaveData bounds = new();
		//public DevTest.BoundsSaveData localBounds = new();
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


	public class StaticParticleSystemRendererSubtitute : StaticSubtitute 
	{
		public override Type SubtitutedType => typeof(UnityEngine.ParticleSystemRenderer);
	}

	[SaveHandler(311736785272193606, "StaticParticleSystemRendererSubtitute", typeof(StaticParticleSystemRendererSubtitute), generationMode: SaveHandlerGenerationMode.FullAutomata, staticHandlerOf: typeof(UnityEngine.ParticleSystemRenderer))]
	public class StaticParticleSystemRendererSaveHandler : StaticSaveHandlerBase<StaticParticleSystemRendererSubtitute, StaticParticleSystemRendererSaveData> 
	{
		public override void WriteSaveData()
		{
			base.WriteSaveData();

		}

		public override void LoadReferences()
		{
			base.LoadReferences();

		}
		static StaticParticleSystemRendererSaveHandler()
		{
			Dictionary<string, long> methodToId = new()
			{

			};
			Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
			Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
			Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
		}
		static Type _typeReference = typeof(UnityEngine.ParticleSystemRenderer);
		static Type _typeDefinition = typeof(UnityEngine.ParticleSystemRenderer);
		static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
		public static Func<object, Delegate> _idToMethod(long id)
		{
			Func<object, Delegate> method = id switch
			{
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


	public class StaticParticleSystemRendererSaveData : StaticSaveDataBase 
	{

	}

}