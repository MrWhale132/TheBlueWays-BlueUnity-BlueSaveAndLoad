//auto-generated
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.Exceptions;
using UnityEngine;

namespace UnityEngine_
{
    [SaveHandler(801744359158797818, "Shader", typeof(UnityEngine.Shader), order: -9, generationMode: SaveHandlerGenerationMode.Manual)]
    public class ShaderSaveHandler : AssetSaveHandlerBase<UnityEngine.Shader, ShaderSaveData>
    {
        public static Dictionary<string, RandomId> _registeredShadersByName = new();


        public override void Init(object instance)
        {
            base.Init(instance);

            AddRegisteredShader(__instance.name, HandledObjectId, false);
        }

        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.name = __instance.name;
            __saveData.maximumLOD = __instance.maximumLOD;
            __saveData.hideFlags = __instance.hideFlags;
        }

        public override void LoadReferences()
        {
            base.LoadReferences();
            //no need to set name here because it already has that name
            __instance.maximumLOD = __saveData.maximumLOD;
            __instance.hideFlags = __saveData.hideFlags;
        }

        public override void _AssignInstance()
        {
            AddRegisteredShader(__saveData.name, HandledObjectId, true);

            __instance = Shader.Find(__saveData.name);
#if UNITY_EDITOR
            __instance = UnityEngine.Object.Instantiate(__instance);
#endif
        }


        public override void ReleaseObject()
        {
            _registeredShadersByName.Remove(__instance.name);

            base.ReleaseObject();
        }


        public static void AddRegisteredShader(string name, RandomId objectId, bool isLoading)
        {
            if (!_registeredShadersByName.ContainsKey(name))
                _registeredShadersByName.Add(name, objectId);
            else
            {
                string message = $"ShaderSaveHandler: It as detected that multiple c# managed object references the same engine side shader object. " +
                    $"Which is a problem because c# Shader object instances can not be created on command, thus the handler of this instance will " +
                    $"not be able recreate a Shader instance on loading back. This does not addect saving but do affect loading. " +
                    $"Loading this save will not be possbile and will throw error.\n" +
                    $"objectid: {objectId}, shader name: {name}";

#if UNITY_EDITOR
                if (isLoading)
                {
                    throw new InstanceCreationException(message);
                }
                else
                    Debug.LogError(message);
#endif
            }
        }



        static ShaderSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
				/// methodToId map for <see cref="Shader"/>
				{$"GetDependency(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Shader", 216031852506197163},
                {$"GetPassCountInSubshader(mscorlib System.Int32):mscorlib System.Int32", 463975320479908623},
                {$"FindPassTagValue(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Rendering.ShaderTagId):UnityEngine.CoreModule UnityEngine.Rendering.ShaderTagId", 710181013836145966},
                {$"FindPassTagValue(mscorlib System.Int32,mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Rendering.ShaderTagId):UnityEngine.CoreModule UnityEngine.Rendering.ShaderTagId", 323070047369756344},
                {$"FindSubshaderTagValue(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Rendering.ShaderTagId):UnityEngine.CoreModule UnityEngine.Rendering.ShaderTagId", 328186258423394130},
                {$"GetPropertyCount():mscorlib System.Int32", 164014474308079163},
                {$"FindPropertyIndex(mscorlib System.String):mscorlib System.Int32", 813781261496561637},
                {$"GetPropertyName(mscorlib System.Int32):mscorlib System.String", 921470185934323141},
                {$"GetPropertyNameId(mscorlib System.Int32):mscorlib System.Int32", 737275298884734213},
                {$"GetPropertyType(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Rendering.ShaderPropertyType", 234250622606332090},
                {$"GetPropertyDescription(mscorlib System.Int32):mscorlib System.String", 964617230120010990},
                {$"GetPropertyFlags(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Rendering.ShaderPropertyFlags", 647765269154264424},
                {$"GetPropertyAttributes(mscorlib System.Int32):mscorlib System.String[]", 861791059259534134},
                {$"GetPropertyDefaultFloatValue(mscorlib System.Int32):mscorlib System.Single", 320935818585569603},
                {$"GetPropertyDefaultVectorValue(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Vector4", 984570500951003019},
                {$"GetPropertyRangeLimits(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Vector2", 482698346469092125},
                {$"GetPropertyDefaultIntValue(mscorlib System.Int32):mscorlib System.Int32", 964042486658244922},
                {$"GetPropertyTextureDimension(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Rendering.TextureDimension", 128622313095988714},
                {$"GetPropertyTextureDefaultName(mscorlib System.Int32):mscorlib System.String", 534011279812407019},
                {$"FindTextureStack(mscorlib System.Int32,mscorlib System.String&,mscorlib System.Int32&):mscorlib System.Boolean", 624163849066043048},
            };
            Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
            Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
            Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(UnityEngine.Shader);
        static Type _typeDefinition = typeof(UnityEngine.Shader);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                216031852506197163 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Shader>(((UnityEngine.Shader)instance).GetDependency)),
                463975320479908623 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Int32>(((UnityEngine.Shader)instance).GetPassCountInSubshader)),
                710181013836145966 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Rendering.ShaderTagId, UnityEngine.Rendering.ShaderTagId>(((UnityEngine.Shader)instance).FindPassTagValue)),
                323070047369756344 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Int32, UnityEngine.Rendering.ShaderTagId, UnityEngine.Rendering.ShaderTagId>(((UnityEngine.Shader)instance).FindPassTagValue)),
                328186258423394130 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Rendering.ShaderTagId, UnityEngine.Rendering.ShaderTagId>(((UnityEngine.Shader)instance).FindSubshaderTagValue)),
                164014474308079163 => new Func<object, Delegate>((instance) => new Func<System.Int32>(((UnityEngine.Shader)instance).GetPropertyCount)),
                813781261496561637 => new Func<object, Delegate>((instance) => new Func<System.String, System.Int32>(((UnityEngine.Shader)instance).FindPropertyIndex)),
                921470185934323141 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.String>(((UnityEngine.Shader)instance).GetPropertyName)),
                737275298884734213 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Int32>(((UnityEngine.Shader)instance).GetPropertyNameId)),
                234250622606332090 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Rendering.ShaderPropertyType>(((UnityEngine.Shader)instance).GetPropertyType)),
                964617230120010990 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.String>(((UnityEngine.Shader)instance).GetPropertyDescription)),
                647765269154264424 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Rendering.ShaderPropertyFlags>(((UnityEngine.Shader)instance).GetPropertyFlags)),
                861791059259534134 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.String[]>(((UnityEngine.Shader)instance).GetPropertyAttributes)),
                320935818585569603 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Single>(((UnityEngine.Shader)instance).GetPropertyDefaultFloatValue)),
                984570500951003019 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Vector4>(((UnityEngine.Shader)instance).GetPropertyDefaultVectorValue)),
                482698346469092125 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Vector2>(((UnityEngine.Shader)instance).GetPropertyRangeLimits)),
                964042486658244922 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Int32>(((UnityEngine.Shader)instance).GetPropertyDefaultIntValue)),
                128622313095988714 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Rendering.TextureDimension>(((UnityEngine.Shader)instance).GetPropertyTextureDimension)),
                534011279812407019 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.String>(((UnityEngine.Shader)instance).GetPropertyTextureDefaultName)),
                _ => Infra.Singleton.GetIdToMethodMapForType(_typeReference.BaseType)(id),
            };
            return method;
        }
        public static MethodInfo _idToMethodInfo(long id)
        {
            MethodInfo methodDef = id switch
            {
                624163849066043048 => typeof(UnityEngine.Shader).GetMethod(nameof(UnityEngine.Shader.FindTextureStack), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(System.Int32), typeof(System.String).MakeByRefType(), typeof(System.Int32).MakeByRefType() }, null),
                _ => Infra.Singleton.GetMethodInfoIdToMethodMapForType(_typeReference.BaseType)(id),
            };
            return methodDef;
        }
    }


    public class ShaderSaveData : AssetSaveData
    {
        public string name;
        public System.Int32 maximumLOD;
        public UnityEngine.HideFlags hideFlags;
    }


    public class StaticShaderSubtitute : StaticSubtitute
    {
        public override Type SubtitutedType => typeof(UnityEngine.Shader);
    }

    [SaveHandler(625657544175383752, "StaticShaderSubtitute", typeof(StaticShaderSubtitute), generationMode: SaveHandlerGenerationMode.FullAutomata, staticHandlerOf: typeof(UnityEngine.Shader))]
    public class StaticShaderSaveHandler : StaticSaveHandlerBase<StaticShaderSubtitute, StaticShaderSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.maximumChunksOverride = UnityEngine.Shader.maximumChunksOverride;
            __saveData.globalMaximumLOD = UnityEngine.Shader.globalMaximumLOD;
            __saveData.globalRenderPipeline = UnityEngine.Shader.globalRenderPipeline;
        }

        public override void LoadReferences()
        {
            base.LoadReferences();
            UnityEngine.Shader.maximumChunksOverride = __saveData.maximumChunksOverride;
            UnityEngine.Shader.globalMaximumLOD = __saveData.globalMaximumLOD;
            UnityEngine.Shader.globalRenderPipeline = __saveData.globalRenderPipeline;
        }
        static StaticShaderSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
				/// methodToId map for static <see cref="Shader"/>
				{$"Find(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Shader", 139791503018059385},
                {$"EnableKeyword(mscorlib System.String):mscorlib System.Void", 854948126224128074},
                {$"DisableKeyword(mscorlib System.String):mscorlib System.Void", 283304452511500799},
                {$"IsKeywordEnabled(mscorlib System.String):mscorlib System.Boolean", 819067538889997420},
                {$"EnableKeyword(UnityEngine.CoreModule UnityEngine.Rendering.GlobalKeyword&):mscorlib System.Void", 245948650462442826},
                {$"DisableKeyword(UnityEngine.CoreModule UnityEngine.Rendering.GlobalKeyword&):mscorlib System.Void", 511686656351407955},
                {$"SetKeyword(UnityEngine.CoreModule UnityEngine.Rendering.GlobalKeyword&,mscorlib System.Boolean):mscorlib System.Void", 486031607544617190},
                {$"IsKeywordEnabled(UnityEngine.CoreModule UnityEngine.Rendering.GlobalKeyword&):mscorlib System.Boolean", 845839239905448733},
                {$"WarmupAllShaders():mscorlib System.Void", 947008901164330710},
                {$"PropertyToID(mscorlib System.String):mscorlib System.Int32", 955780279119587415},
                {$"SetGlobalInt(mscorlib System.String,mscorlib System.Int32):mscorlib System.Void", 474792706457572878},
                {$"SetGlobalInt(mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 725683888258682434},
                {$"SetGlobalFloat(mscorlib System.String,mscorlib System.Single):mscorlib System.Void", 626207471254671978},
                {$"SetGlobalFloat(mscorlib System.Int32,mscorlib System.Single):mscorlib System.Void", 969180355886503824},
                {$"SetGlobalInteger(mscorlib System.String,mscorlib System.Int32):mscorlib System.Void", 342820162457671356},
                {$"SetGlobalInteger(mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 543594269465085662},
                {$"SetGlobalVector(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Vector4):mscorlib System.Void", 614892880710525737},
                {$"SetGlobalVector(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Vector4):mscorlib System.Void", 275766169646200964},
                {$"SetGlobalColor(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Color):mscorlib System.Void", 354160444894034759},
                {$"SetGlobalColor(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Color):mscorlib System.Void", 784873214341965147},
                {$"SetGlobalMatrix(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Matrix4x4):mscorlib System.Void", 390521127560244128},
                {$"SetGlobalMatrix(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Matrix4x4):mscorlib System.Void", 947195519283998745},
                {$"SetGlobalTexture(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Texture):mscorlib System.Void", 816265575310683710},
                {$"SetGlobalTexture(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Texture):mscorlib System.Void", 910591283456240820},
                {$"SetGlobalTexture(mscorlib System.String,UnityEngine.CoreModule UnityEngine.RenderTexture,UnityEngine.CoreModule UnityEngine.Rendering.RenderTextureSubElement):mscorlib System.Void", 267261910950928872},
                {$"SetGlobalTexture(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.RenderTexture,UnityEngine.CoreModule UnityEngine.Rendering.RenderTextureSubElement):mscorlib System.Void", 532407111916975354},
                {$"SetGlobalBuffer(mscorlib System.String,UnityEngine.CoreModule UnityEngine.ComputeBuffer):mscorlib System.Void", 375867122342837739},
                {$"SetGlobalBuffer(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.ComputeBuffer):mscorlib System.Void", 876014000498567360},
                {$"SetGlobalBuffer(mscorlib System.String,UnityEngine.CoreModule UnityEngine.GraphicsBuffer):mscorlib System.Void", 776437486808814558},
                {$"SetGlobalBuffer(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.GraphicsBuffer):mscorlib System.Void", 816085762478557706},
                {$"SetGlobalConstantBuffer(mscorlib System.String,UnityEngine.CoreModule UnityEngine.ComputeBuffer,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 333862819257300944},
                {$"SetGlobalConstantBuffer(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.ComputeBuffer,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 458166590835010721},
                {$"SetGlobalConstantBuffer(mscorlib System.String,UnityEngine.CoreModule UnityEngine.GraphicsBuffer,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 505326651417750335},
                {$"SetGlobalConstantBuffer(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.GraphicsBuffer,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 716626197314757943},
                {$"SetGlobalRayTracingAccelerationStructure(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Rendering.RayTracingAccelerationStructure):mscorlib System.Void", 802241508307578500},
                {$"SetGlobalRayTracingAccelerationStructure(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Rendering.RayTracingAccelerationStructure):mscorlib System.Void", 587912824775981742},
                {$"SetGlobalFloatArray(mscorlib System.String,mscorlib System.Collections.Generic.List<mscorlib System.Single>):mscorlib System.Void", 256502889084615746},
                {$"SetGlobalFloatArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<mscorlib System.Single>):mscorlib System.Void", 698932782231672534},
                {$"SetGlobalFloatArray(mscorlib System.String,mscorlib System.Single[]):mscorlib System.Void", 487503384916924415},
                {$"SetGlobalFloatArray(mscorlib System.Int32,mscorlib System.Single[]):mscorlib System.Void", 421482955830207035},
                {$"SetGlobalVectorArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>):mscorlib System.Void", 908564777630616711},
                {$"SetGlobalVectorArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>):mscorlib System.Void", 389465844370998851},
                {$"SetGlobalVectorArray(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Vector4[]):mscorlib System.Void", 685671532411348575},
                {$"SetGlobalVectorArray(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Vector4[]):mscorlib System.Void", 644372040674276503},
                {$"SetGlobalMatrixArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Matrix4x4>):mscorlib System.Void", 661883342292131331},
                {$"SetGlobalMatrixArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Matrix4x4>):mscorlib System.Void", 730362195813584491},
                {$"SetGlobalMatrixArray(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Matrix4x4[]):mscorlib System.Void", 475093974743284587},
                {$"SetGlobalMatrixArray(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Matrix4x4[]):mscorlib System.Void", 339879752871864906},
                {$"GetGlobalInt(mscorlib System.String):mscorlib System.Int32", 663739844253909715},
                {$"GetGlobalInt(mscorlib System.Int32):mscorlib System.Int32", 340534000387838892},
                {$"GetGlobalFloat(mscorlib System.String):mscorlib System.Single", 359902550996115875},
                {$"GetGlobalFloat(mscorlib System.Int32):mscorlib System.Single", 602963945632401746},
                {$"GetGlobalInteger(mscorlib System.String):mscorlib System.Int32", 623798410327205229},
                {$"GetGlobalInteger(mscorlib System.Int32):mscorlib System.Int32", 138605791848547834},
                {$"GetGlobalVector(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Vector4", 280355252833628160},
                {$"GetGlobalVector(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Vector4", 792790667287936103},
                {$"GetGlobalColor(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Color", 968345385811937713},
                {$"GetGlobalColor(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Color", 339969796499641520},
                {$"GetGlobalMatrix(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Matrix4x4", 335640584525022286},
                {$"GetGlobalMatrix(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Matrix4x4", 499964889526040647},
                {$"GetGlobalTexture(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Texture", 859848427036736513},
                {$"GetGlobalTexture(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Texture", 137085096757547442},
                {$"GetGlobalFloatArray(mscorlib System.String):mscorlib System.Single[]", 755143675378053751},
                {$"GetGlobalFloatArray(mscorlib System.Int32):mscorlib System.Single[]", 177374276959839375},
                {$"GetGlobalVectorArray(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Vector4[]", 799340071760488636},
                {$"GetGlobalVectorArray(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Vector4[]", 537096762544977796},
                {$"GetGlobalMatrixArray(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Matrix4x4[]", 363730241656129284},
                {$"GetGlobalMatrixArray(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Matrix4x4[]", 860267742789368237},
                {$"GetGlobalFloatArray(mscorlib System.String,mscorlib System.Collections.Generic.List<mscorlib System.Single>):mscorlib System.Void", 379674511542568292},
                {$"GetGlobalFloatArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<mscorlib System.Single>):mscorlib System.Void", 690698755128798200},
                {$"GetGlobalVectorArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>):mscorlib System.Void", 519575620022007664},
                {$"GetGlobalVectorArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>):mscorlib System.Void", 768182838274325877},
                {$"GetGlobalMatrixArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Matrix4x4>):mscorlib System.Void", 492597873312113454},
                {$"GetGlobalMatrixArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Matrix4x4>):mscorlib System.Void", 332663744569613036},
            };
            Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
            Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
            Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(UnityEngine.Shader);
        static Type _typeDefinition = typeof(UnityEngine.Shader);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                139791503018059385 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Shader>(UnityEngine.Shader.Find)),
                854948126224128074 => new Func<object, Delegate>((instance) => new Action<System.String>(UnityEngine.Shader.EnableKeyword)),
                283304452511500799 => new Func<object, Delegate>((instance) => new Action<System.String>(UnityEngine.Shader.DisableKeyword)),
                819067538889997420 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(UnityEngine.Shader.IsKeywordEnabled)),
                947008901164330710 => new Func<object, Delegate>((instance) => new Action(UnityEngine.Shader.WarmupAllShaders)),
                955780279119587415 => new Func<object, Delegate>((instance) => new Func<System.String, System.Int32>(UnityEngine.Shader.PropertyToID)),
                474792706457572878 => new Func<object, Delegate>((instance) => new Action<System.String, System.Int32>(UnityEngine.Shader.SetGlobalInt)),
                725683888258682434 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Int32>(UnityEngine.Shader.SetGlobalInt)),
                626207471254671978 => new Func<object, Delegate>((instance) => new Action<System.String, System.Single>(UnityEngine.Shader.SetGlobalFloat)),
                969180355886503824 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Single>(UnityEngine.Shader.SetGlobalFloat)),
                342820162457671356 => new Func<object, Delegate>((instance) => new Action<System.String, System.Int32>(UnityEngine.Shader.SetGlobalInteger)),
                543594269465085662 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Int32>(UnityEngine.Shader.SetGlobalInteger)),
                614892880710525737 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Vector4>(UnityEngine.Shader.SetGlobalVector)),
                275766169646200964 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Vector4>(UnityEngine.Shader.SetGlobalVector)),
                354160444894034759 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Color>(UnityEngine.Shader.SetGlobalColor)),
                784873214341965147 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Color>(UnityEngine.Shader.SetGlobalColor)),
                390521127560244128 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Matrix4x4>(UnityEngine.Shader.SetGlobalMatrix)),
                947195519283998745 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Matrix4x4>(UnityEngine.Shader.SetGlobalMatrix)),
                816265575310683710 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Texture>(UnityEngine.Shader.SetGlobalTexture)),
                910591283456240820 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Texture>(UnityEngine.Shader.SetGlobalTexture)),
                267261910950928872 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.RenderTexture, UnityEngine.Rendering.RenderTextureSubElement>(UnityEngine.Shader.SetGlobalTexture)),
                532407111916975354 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.RenderTexture, UnityEngine.Rendering.RenderTextureSubElement>(UnityEngine.Shader.SetGlobalTexture)),
                375867122342837739 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.ComputeBuffer>(UnityEngine.Shader.SetGlobalBuffer)),
                876014000498567360 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.ComputeBuffer>(UnityEngine.Shader.SetGlobalBuffer)),
                776437486808814558 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.GraphicsBuffer>(UnityEngine.Shader.SetGlobalBuffer)),
                816085762478557706 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.GraphicsBuffer>(UnityEngine.Shader.SetGlobalBuffer)),
                333862819257300944 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.ComputeBuffer, System.Int32, System.Int32>(UnityEngine.Shader.SetGlobalConstantBuffer)),
                458166590835010721 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.ComputeBuffer, System.Int32, System.Int32>(UnityEngine.Shader.SetGlobalConstantBuffer)),
                505326651417750335 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.GraphicsBuffer, System.Int32, System.Int32>(UnityEngine.Shader.SetGlobalConstantBuffer)),
                716626197314757943 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.GraphicsBuffer, System.Int32, System.Int32>(UnityEngine.Shader.SetGlobalConstantBuffer)),
                802241508307578500 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Rendering.RayTracingAccelerationStructure>(UnityEngine.Shader.SetGlobalRayTracingAccelerationStructure)),
                587912824775981742 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Rendering.RayTracingAccelerationStructure>(UnityEngine.Shader.SetGlobalRayTracingAccelerationStructure)),
                256502889084615746 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<System.Single>>(UnityEngine.Shader.SetGlobalFloatArray)),
                698932782231672534 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<System.Single>>(UnityEngine.Shader.SetGlobalFloatArray)),
                487503384916924415 => new Func<object, Delegate>((instance) => new Action<System.String, System.Single[]>(UnityEngine.Shader.SetGlobalFloatArray)),
                421482955830207035 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Single[]>(UnityEngine.Shader.SetGlobalFloatArray)),
                908564777630616711 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Vector4>>(UnityEngine.Shader.SetGlobalVectorArray)),
                389465844370998851 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Vector4>>(UnityEngine.Shader.SetGlobalVectorArray)),
                685671532411348575 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Vector4[]>(UnityEngine.Shader.SetGlobalVectorArray)),
                644372040674276503 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Vector4[]>(UnityEngine.Shader.SetGlobalVectorArray)),
                661883342292131331 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Matrix4x4>>(UnityEngine.Shader.SetGlobalMatrixArray)),
                730362195813584491 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Matrix4x4>>(UnityEngine.Shader.SetGlobalMatrixArray)),
                475093974743284587 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Matrix4x4[]>(UnityEngine.Shader.SetGlobalMatrixArray)),
                339879752871864906 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Matrix4x4[]>(UnityEngine.Shader.SetGlobalMatrixArray)),
                663739844253909715 => new Func<object, Delegate>((instance) => new Func<System.String, System.Int32>(UnityEngine.Shader.GetGlobalInt)),
                340534000387838892 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Int32>(UnityEngine.Shader.GetGlobalInt)),
                359902550996115875 => new Func<object, Delegate>((instance) => new Func<System.String, System.Single>(UnityEngine.Shader.GetGlobalFloat)),
                602963945632401746 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Single>(UnityEngine.Shader.GetGlobalFloat)),
                623798410327205229 => new Func<object, Delegate>((instance) => new Func<System.String, System.Int32>(UnityEngine.Shader.GetGlobalInteger)),
                138605791848547834 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Int32>(UnityEngine.Shader.GetGlobalInteger)),
                280355252833628160 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Vector4>(UnityEngine.Shader.GetGlobalVector)),
                792790667287936103 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Vector4>(UnityEngine.Shader.GetGlobalVector)),
                968345385811937713 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Color>(UnityEngine.Shader.GetGlobalColor)),
                339969796499641520 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Color>(UnityEngine.Shader.GetGlobalColor)),
                335640584525022286 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Matrix4x4>(UnityEngine.Shader.GetGlobalMatrix)),
                499964889526040647 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Matrix4x4>(UnityEngine.Shader.GetGlobalMatrix)),
                859848427036736513 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Texture>(UnityEngine.Shader.GetGlobalTexture)),
                137085096757547442 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Texture>(UnityEngine.Shader.GetGlobalTexture)),
                755143675378053751 => new Func<object, Delegate>((instance) => new Func<System.String, System.Single[]>(UnityEngine.Shader.GetGlobalFloatArray)),
                177374276959839375 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Single[]>(UnityEngine.Shader.GetGlobalFloatArray)),
                799340071760488636 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Vector4[]>(UnityEngine.Shader.GetGlobalVectorArray)),
                537096762544977796 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Vector4[]>(UnityEngine.Shader.GetGlobalVectorArray)),
                363730241656129284 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Matrix4x4[]>(UnityEngine.Shader.GetGlobalMatrixArray)),
                860267742789368237 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Matrix4x4[]>(UnityEngine.Shader.GetGlobalMatrixArray)),
                379674511542568292 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<System.Single>>(UnityEngine.Shader.GetGlobalFloatArray)),
                690698755128798200 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<System.Single>>(UnityEngine.Shader.GetGlobalFloatArray)),
                519575620022007664 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Vector4>>(UnityEngine.Shader.GetGlobalVectorArray)),
                768182838274325877 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Vector4>>(UnityEngine.Shader.GetGlobalVectorArray)),
                492597873312113454 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Matrix4x4>>(UnityEngine.Shader.GetGlobalMatrixArray)),
                332663744569613036 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Matrix4x4>>(UnityEngine.Shader.GetGlobalMatrixArray)),
                _ => Infra.Singleton.GetIdToMethodMapForType(_typeReference.BaseType)(id),
            };
            return method;
        }
        public static MethodInfo _idToMethodInfo(long id)
        {
            MethodInfo methodDef = id switch
            {
                245948650462442826 => typeof(UnityEngine.Shader).GetMethod(nameof(UnityEngine.Shader.EnableKeyword), BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(UnityEngine.Rendering.GlobalKeyword).MakeByRefType() }, null),
                511686656351407955 => typeof(UnityEngine.Shader).GetMethod(nameof(UnityEngine.Shader.DisableKeyword), BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(UnityEngine.Rendering.GlobalKeyword).MakeByRefType() }, null),
                486031607544617190 => typeof(UnityEngine.Shader).GetMethod(nameof(UnityEngine.Shader.SetKeyword), BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(UnityEngine.Rendering.GlobalKeyword).MakeByRefType(), typeof(System.Boolean) }, null),
                845839239905448733 => typeof(UnityEngine.Shader).GetMethod(nameof(UnityEngine.Shader.IsKeywordEnabled), BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(UnityEngine.Rendering.GlobalKeyword).MakeByRefType() }, null),
                _ => Infra.Singleton.GetMethodInfoIdToMethodMapForType(_typeReference.BaseType)(id),
            };
            return methodDef;
        }
    }


    public class StaticShaderSaveData : StaticSaveDataBase
    {
        public System.Int32 maximumChunksOverride;
        public System.Int32 globalMaximumLOD;
        public System.String globalRenderPipeline;
    }

}