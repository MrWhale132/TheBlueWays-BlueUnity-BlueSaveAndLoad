//auto-generated
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine_
{
    [SaveHandler(549644073204883384, "Material", typeof(UnityEngine.Material), order: -6, dependsOn: new[] { typeof(Shader) }, generationMode: SaveHandlerGenerationMode.Manual)]
    public class MaterialSaveHandler : AssetSaveHandlerBase<UnityEngine.Material, MaterialSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.shader = GetObjectId(__instance.shader, setLoadingOrder: true);
            __saveData.renderQueue = __instance.renderQueue;
            __saveData.globalIlluminationFlags = __instance.globalIlluminationFlags;
            __saveData.doubleSidedGI = __instance.doubleSidedGI;
            __saveData.enableInstancing = __instance.enableInstancing;
            __saveData.hideFlags = __instance.hideFlags;

            __saveData.shaderKeywords = __instance.shaderKeywords;

            __saveData.colors.Clear();
            __saveData.vectors.Clear();
            __saveData.floats.Clear();
            __saveData.textures.Clear();


            int count = __instance.shader.GetPropertyCount();

            for (int i = 0; i < count; i++)
            {
                var type = __instance.shader.GetPropertyType(i);
                var name = __instance.shader.GetPropertyName(i);

                switch (type)
                {
                    case ShaderPropertyType.Color:
                        var color = __instance.GetColor(name);
                        var colorOverride = new ColorOverride() { name = name, value = color };
                        __saveData.colors.Add(colorOverride);
                        break;
                    case ShaderPropertyType.Vector:
                        var vector = __instance.GetVector(name);
                        var vectorOverride = new VectorOverride() { name = name, value = vector };
                        __saveData.vectors.Add(vectorOverride);
                        break;
                    case ShaderPropertyType.Float:
                    case ShaderPropertyType.Range:
                        var floatValue = __instance.GetFloat(name);
                        var floatOverride = new FloatOverride() { name = name, value = floatValue };
                        __saveData.floats.Add(floatOverride);
                        break;
                    case ShaderPropertyType.Texture:
                        var texture = GetObjectId(__instance.GetTexture(name), setLoadingOrder: true);
                        var textureScale = __instance.GetTextureScale(name);
                        var textureOffset = __instance.GetTextureOffset(name);
                        var textureOverride = new TextureOverride() { name = name, texture = texture, scale = textureScale, offset = textureOffset };
                        __saveData.textures.Add(textureOverride);
                        break;
                }
            }

        }

        public override void LoadReferences()
        {
            base.LoadReferences();
            //order matters

            foreach (var colorOverride in __saveData.colors)
            {
                var color = colorOverride.value;
                __instance.SetColor(colorOverride.name, color);
            }
            foreach (var vectorOverride in __saveData.vectors)
            {
                var vector = vectorOverride.value;
                __instance.SetVector(vectorOverride.name, vector);
            }
            foreach (var floatOverride in __saveData.floats)
            {
                var floatValue = floatOverride.value;
                __instance.SetFloat(floatOverride.name, floatValue);
            }
            foreach (var textureOverride in __saveData.textures)
            {
                var texture = GetObjectById<Texture>(textureOverride.texture);
                __instance.SetTexture(textureOverride.name, texture);
                __instance.SetTextureScale(textureOverride.name, textureOverride.scale);
                __instance.SetTextureOffset(textureOverride.name, textureOverride.offset);
            }


            __instance.shaderKeywords = __saveData.shaderKeywords;

            __instance.renderQueue = __saveData.renderQueue;
            __instance.globalIlluminationFlags = __saveData.globalIlluminationFlags;
            __instance.doubleSidedGI = __saveData.doubleSidedGI;
            __instance.enableInstancing = __saveData.enableInstancing;
            __instance.hideFlags = __saveData.hideFlags;

        }


        public override void _AssignInstance()
        {
            base._AssignInstance();


            if (__instance == null)
            {
                {
                    var shader = GetObjectById<UnityEngine.Shader>(__saveData.shader);

                    if (shader != null)
                    {
                        __instance = new UnityEngine.Material(shader);
                    }
                    else
                    {
                        Debug.LogError("Did not find shader to create Material");
                    }
                }
            }
        }





        static MaterialSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
				/// methodToId map for <see cref="Material"/>
				{$"HasProperty(mscorlib System.Int32):mscorlib System.Boolean", 436926293688441328},
                {$"HasProperty(mscorlib System.String):mscorlib System.Boolean", 521855450597813725},
                {$"HasFloat(mscorlib System.String):mscorlib System.Boolean", 887819822065737234},
                {$"HasFloat(mscorlib System.Int32):mscorlib System.Boolean", 757057570686561544},
                {$"HasInt(mscorlib System.String):mscorlib System.Boolean", 161193348575058806},
                {$"HasInt(mscorlib System.Int32):mscorlib System.Boolean", 883033924640318216},
                {$"HasInteger(mscorlib System.String):mscorlib System.Boolean", 356370809452776476},
                {$"HasInteger(mscorlib System.Int32):mscorlib System.Boolean", 103727970816774155},
                {$"HasTexture(mscorlib System.String):mscorlib System.Boolean", 140312356186483221},
                {$"HasTexture(mscorlib System.Int32):mscorlib System.Boolean", 352840009378852043},
                {$"HasMatrix(mscorlib System.String):mscorlib System.Boolean", 578047078332245289},
                {$"HasMatrix(mscorlib System.Int32):mscorlib System.Boolean", 319021308832434064},
                {$"HasVector(mscorlib System.String):mscorlib System.Boolean", 321577176602357282},
                {$"HasVector(mscorlib System.Int32):mscorlib System.Boolean", 444674823690728006},
                {$"HasColor(mscorlib System.String):mscorlib System.Boolean", 863295479548514262},
                {$"HasColor(mscorlib System.Int32):mscorlib System.Boolean", 955764357688425132},
                {$"HasBuffer(mscorlib System.String):mscorlib System.Boolean", 814920428507555514},
                {$"HasBuffer(mscorlib System.Int32):mscorlib System.Boolean", 119984788012247235},
                {$"HasConstantBuffer(mscorlib System.String):mscorlib System.Boolean", 703523684012381102},
                {$"HasConstantBuffer(mscorlib System.Int32):mscorlib System.Boolean", 393013141526921080},
                {$"EnableKeyword(mscorlib System.String):mscorlib System.Void", 159529077573113504},
                {$"DisableKeyword(mscorlib System.String):mscorlib System.Void", 268037215718129004},
                {$"IsKeywordEnabled(mscorlib System.String):mscorlib System.Boolean", 377304657741232781},
                {$"EnableKeyword(UnityEngine.CoreModule UnityEngine.Rendering.LocalKeyword&):mscorlib System.Void", 302221325844300932},
                {$"DisableKeyword(UnityEngine.CoreModule UnityEngine.Rendering.LocalKeyword&):mscorlib System.Void", 786347056447544880},
                {$"SetKeyword(UnityEngine.CoreModule UnityEngine.Rendering.LocalKeyword&,mscorlib System.Boolean):mscorlib System.Void", 304697871243039999},
                {$"IsKeywordEnabled(UnityEngine.CoreModule UnityEngine.Rendering.LocalKeyword&):mscorlib System.Boolean", 586417498720952063},
                {$"SetShaderPassEnabled(mscorlib System.String,mscorlib System.Boolean):mscorlib System.Void", 818947671240864439},
                {$"GetShaderPassEnabled(mscorlib System.String):mscorlib System.Boolean", 488610914138197618},
                {$"GetPassName(mscorlib System.Int32):mscorlib System.String", 153874615796642268},
                {$"FindPass(mscorlib System.String):mscorlib System.Int32", 519893254214315547},
                {$"SetOverrideTag(mscorlib System.String,mscorlib System.String):mscorlib System.Void", 650400411748683919},
                {$"GetTag(mscorlib System.String,mscorlib System.Boolean,mscorlib System.String):mscorlib System.String", 926962040047100560},
                {$"GetTag(mscorlib System.String,mscorlib System.Boolean):mscorlib System.String", 701937250662140551},
                {$"Lerp(UnityEngine.CoreModule UnityEngine.Material,UnityEngine.CoreModule UnityEngine.Material,mscorlib System.Single):mscorlib System.Void", 169846947363711211},
                {$"SetPass(mscorlib System.Int32):mscorlib System.Boolean", 898871137790755840},
                {$"CopyPropertiesFromMaterial(UnityEngine.CoreModule UnityEngine.Material):mscorlib System.Void", 296599088695182875},
                {$"CopyMatchingPropertiesFromMaterial(UnityEngine.CoreModule UnityEngine.Material):mscorlib System.Void", 428860814760886102},
                {$"ComputeCRC():mscorlib System.Int32", 520889244261787951},
                {$"GetTexturePropertyNames():mscorlib System.String[]", 232677521880897945},
                {$"GetTexturePropertyNameIDs():mscorlib System.Int32[]", 654952555307292597},
                {$"GetTexturePropertyNames(mscorlib System.Collections.Generic.List<mscorlib System.String>):mscorlib System.Void", 184235746346374139},
                {$"GetTexturePropertyNameIDs(mscorlib System.Collections.Generic.List<mscorlib System.Int32>):mscorlib System.Void", 727002212458720096},
				#if UNITY_EDITOR
				{$"IsChildOf(UnityEngine.CoreModule UnityEngine.Material):mscorlib System.Boolean", 131811510802306386},
				#endif
				#if UNITY_EDITOR
				{$"RevertAllPropertyOverrides():mscorlib System.Void", 997292338752638819},
				#endif
				#if UNITY_EDITOR
				{$"IsPropertyOverriden(mscorlib System.Int32):mscorlib System.Boolean", 880649141914244309},
				#endif
				#if UNITY_EDITOR
				{$"IsPropertyLocked(mscorlib System.Int32):mscorlib System.Boolean", 884958275826449750},
				#endif
				#if UNITY_EDITOR
				{$"IsPropertyLockedByAncestor(mscorlib System.Int32):mscorlib System.Boolean", 635805031451822893},
				#endif
				#if UNITY_EDITOR
				{$"IsPropertyOverriden(mscorlib System.String):mscorlib System.Boolean", 582237332456449377},
				#endif
				#if UNITY_EDITOR
				{$"IsPropertyLocked(mscorlib System.String):mscorlib System.Boolean", 507992681499455626},
				#endif
				#if UNITY_EDITOR
				{$"IsPropertyLockedByAncestor(mscorlib System.String):mscorlib System.Boolean", 830747267590145538},
				#endif
				#if UNITY_EDITOR
				{$"SetPropertyLock(mscorlib System.Int32,mscorlib System.Boolean):mscorlib System.Void", 658054593200172254},
				#endif
				#if UNITY_EDITOR
				{$"ApplyPropertyOverride(UnityEngine.CoreModule UnityEngine.Material,mscorlib System.Int32,mscorlib System.Boolean):mscorlib System.Void", 766063418025491616},
				#endif
				#if UNITY_EDITOR
				{$"RevertPropertyOverride(mscorlib System.Int32):mscorlib System.Void", 877524395742888076},
				#endif
				#if UNITY_EDITOR
				{$"SetPropertyLock(mscorlib System.String,mscorlib System.Boolean):mscorlib System.Void", 258554646881846547},
				#endif
				#if UNITY_EDITOR
				{$"ApplyPropertyOverride(UnityEngine.CoreModule UnityEngine.Material,mscorlib System.String,mscorlib System.Boolean):mscorlib System.Void", 724166776623631545},
				#endif
				#if UNITY_EDITOR
				{$"RevertPropertyOverride(mscorlib System.String):mscorlib System.Void", 822297840499524135},
				#endif
				{$"SetInt(mscorlib System.String,mscorlib System.Int32):mscorlib System.Void", 208287892225920172},
                {$"SetInt(mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 542732527609949634},
                {$"SetFloat(mscorlib System.String,mscorlib System.Single):mscorlib System.Void", 653449451556944169},
                {$"SetFloat(mscorlib System.Int32,mscorlib System.Single):mscorlib System.Void", 346666018237639541},
                {$"SetInteger(mscorlib System.String,mscorlib System.Int32):mscorlib System.Void", 909328007176588938},
                {$"SetInteger(mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 709743012492639226},
                {$"SetColor(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Color):mscorlib System.Void", 991755984275969084},
                {$"SetColor(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Color):mscorlib System.Void", 215621661443625293},
                {$"SetVector(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Vector4):mscorlib System.Void", 794486984752255500},
                {$"SetVector(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Vector4):mscorlib System.Void", 137019443055966638},
                {$"SetMatrix(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Matrix4x4):mscorlib System.Void", 983973691928711416},
                {$"SetMatrix(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Matrix4x4):mscorlib System.Void", 668593283978552652},
                {$"SetTexture(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Texture):mscorlib System.Void", 716332851433345006},
                {$"SetTexture(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Texture):mscorlib System.Void", 148969072237616980},
                {$"SetTexture(mscorlib System.String,UnityEngine.CoreModule UnityEngine.RenderTexture,UnityEngine.CoreModule UnityEngine.Rendering.RenderTextureSubElement):mscorlib System.Void", 123674275962245551},
                {$"SetTexture(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.RenderTexture,UnityEngine.CoreModule UnityEngine.Rendering.RenderTextureSubElement):mscorlib System.Void", 464736051471746508},
                {$"SetBuffer(mscorlib System.String,UnityEngine.CoreModule UnityEngine.ComputeBuffer):mscorlib System.Void", 446379311999115541},
                {$"SetBuffer(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.ComputeBuffer):mscorlib System.Void", 638661389321674711},
                {$"SetBuffer(mscorlib System.String,UnityEngine.CoreModule UnityEngine.GraphicsBuffer):mscorlib System.Void", 409431505226629497},
                {$"SetBuffer(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.GraphicsBuffer):mscorlib System.Void", 978508909921549404},
                {$"SetConstantBuffer(mscorlib System.String,UnityEngine.CoreModule UnityEngine.ComputeBuffer,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 209605616360218674},
                {$"SetConstantBuffer(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.ComputeBuffer,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 399152267377621538},
                {$"SetConstantBuffer(mscorlib System.String,UnityEngine.CoreModule UnityEngine.GraphicsBuffer,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 726389263861387511},
                {$"SetConstantBuffer(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.GraphicsBuffer,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 505744724864878095},
                {$"SetFloatArray(mscorlib System.String,mscorlib System.Collections.Generic.List<mscorlib System.Single>):mscorlib System.Void", 790088939608000002},
                {$"SetFloatArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<mscorlib System.Single>):mscorlib System.Void", 308256530909329182},
                {$"SetFloatArray(mscorlib System.String,mscorlib System.Single[]):mscorlib System.Void", 141770401585386578},
                {$"SetFloatArray(mscorlib System.Int32,mscorlib System.Single[]):mscorlib System.Void", 743355893937982896},
                {$"SetColorArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Color>):mscorlib System.Void", 152874595451375043},
                {$"SetColorArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Color>):mscorlib System.Void", 869688657359349042},
                {$"SetColorArray(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Color[]):mscorlib System.Void", 686885934165284802},
                {$"SetColorArray(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Color[]):mscorlib System.Void", 746412713610223255},
                {$"SetVectorArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>):mscorlib System.Void", 798364448117444639},
                {$"SetVectorArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>):mscorlib System.Void", 617317132393293810},
                {$"SetVectorArray(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Vector4[]):mscorlib System.Void", 437106459616669373},
                {$"SetVectorArray(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Vector4[]):mscorlib System.Void", 838023205491362420},
                {$"SetMatrixArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Matrix4x4>):mscorlib System.Void", 240095841563492908},
                {$"SetMatrixArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Matrix4x4>):mscorlib System.Void", 240499196735323813},
                {$"SetMatrixArray(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Matrix4x4[]):mscorlib System.Void", 374327163209935428},
                {$"SetMatrixArray(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Matrix4x4[]):mscorlib System.Void", 974199901624958962},
                {$"GetInt(mscorlib System.String):mscorlib System.Int32", 481223599689999593},
                {$"GetInt(mscorlib System.Int32):mscorlib System.Int32", 818431654934196204},
                {$"GetFloat(mscorlib System.String):mscorlib System.Single", 675150921162436515},
                {$"GetFloat(mscorlib System.Int32):mscorlib System.Single", 594066965548269591},
                {$"GetInteger(mscorlib System.String):mscorlib System.Int32", 776851948149768180},
                {$"GetInteger(mscorlib System.Int32):mscorlib System.Int32", 960819447299394473},
                {$"GetColor(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Color", 864302771678018853},
                {$"GetColor(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Color", 285741365984538532},
                {$"GetVector(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Vector4", 665814786928144039},
                {$"GetVector(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Vector4", 812303689604057618},
                {$"GetMatrix(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Matrix4x4", 827196444773059971},
                {$"GetMatrix(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Matrix4x4", 136212407287052521},
                {$"GetTexture(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Texture", 261686990555417668},
                {$"GetTexture(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Texture", 350469825672052861},
                {$"GetBuffer(mscorlib System.String):UnityEngine.CoreModule UnityEngine.GraphicsBufferHandle", 144021184177938427},
                {$"GetConstantBuffer(mscorlib System.String):UnityEngine.CoreModule UnityEngine.GraphicsBufferHandle", 179276022850264662},
                {$"GetFloatArray(mscorlib System.String):mscorlib System.Single[]", 422838464873363589},
                {$"GetFloatArray(mscorlib System.Int32):mscorlib System.Single[]", 276890933491227600},
                {$"GetColorArray(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Color[]", 414170125948777458},
                {$"GetColorArray(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Color[]", 652290497354575217},
                {$"GetVectorArray(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Vector4[]", 609566408571387914},
                {$"GetVectorArray(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Vector4[]", 711468377137246094},
                {$"GetMatrixArray(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Matrix4x4[]", 898689192606766059},
                {$"GetMatrixArray(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Matrix4x4[]", 513128936123529301},
                {$"GetFloatArray(mscorlib System.String,mscorlib System.Collections.Generic.List<mscorlib System.Single>):mscorlib System.Void", 613227577046018657},
                {$"GetFloatArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<mscorlib System.Single>):mscorlib System.Void", 167075296818736544},
                {$"GetColorArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Color>):mscorlib System.Void", 628665188000486123},
                {$"GetColorArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Color>):mscorlib System.Void", 463893066920057302},
                {$"GetVectorArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>):mscorlib System.Void", 840288628642161218},
                {$"GetVectorArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>):mscorlib System.Void", 183559815894318328},
                {$"GetMatrixArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Matrix4x4>):mscorlib System.Void", 307516442376874831},
                {$"GetMatrixArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Matrix4x4>):mscorlib System.Void", 938137372322473200},
                {$"SetTextureOffset(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Vector2):mscorlib System.Void", 256601653892057639},
                {$"SetTextureOffset(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Vector2):mscorlib System.Void", 917024311990256377},
                {$"SetTextureScale(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Vector2):mscorlib System.Void", 533748347027583439},
                {$"SetTextureScale(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Vector2):mscorlib System.Void", 402229076363577910},
                {$"GetTextureOffset(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Vector2", 626868638341682796},
                {$"GetTextureOffset(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Vector2", 521367298455056380},
                {$"GetTextureScale(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Vector2", 354468062577077148},
                {$"GetTextureScale(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Vector2", 925867028239429244},
                {$"GetPropertyNames(UnityEngine.CoreModule UnityEngine.MaterialPropertyType):mscorlib System.String[]", 306230572037790746},
            };
            Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
            Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
            Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(UnityEngine.Material);
        static Type _typeDefinition = typeof(UnityEngine.Material);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                436926293688441328 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasProperty)),
                521855450597813725 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasProperty)),
                887819822065737234 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasFloat)),
                757057570686561544 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasFloat)),
                161193348575058806 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasInt)),
                883033924640318216 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasInt)),
                356370809452776476 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasInteger)),
                103727970816774155 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasInteger)),
                140312356186483221 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasTexture)),
                352840009378852043 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasTexture)),
                578047078332245289 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasMatrix)),
                319021308832434064 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasMatrix)),
                321577176602357282 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasVector)),
                444674823690728006 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasVector)),
                863295479548514262 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasColor)),
                955764357688425132 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasColor)),
                814920428507555514 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasBuffer)),
                119984788012247235 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasBuffer)),
                703523684012381102 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasConstantBuffer)),
                393013141526921080 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasConstantBuffer)),
                159529077573113504 => new Func<object, Delegate>((instance) => new Action<System.String>(((UnityEngine.Material)instance).EnableKeyword)),
                268037215718129004 => new Func<object, Delegate>((instance) => new Action<System.String>(((UnityEngine.Material)instance).DisableKeyword)),
                377304657741232781 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).IsKeywordEnabled)),
                818947671240864439 => new Func<object, Delegate>((instance) => new Action<System.String, System.Boolean>(((UnityEngine.Material)instance).SetShaderPassEnabled)),
                488610914138197618 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).GetShaderPassEnabled)),
                153874615796642268 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.String>(((UnityEngine.Material)instance).GetPassName)),
                519893254214315547 => new Func<object, Delegate>((instance) => new Func<System.String, System.Int32>(((UnityEngine.Material)instance).FindPass)),
                650400411748683919 => new Func<object, Delegate>((instance) => new Action<System.String, System.String>(((UnityEngine.Material)instance).SetOverrideTag)),
                926962040047100560 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean, System.String, System.String>(((UnityEngine.Material)instance).GetTag)),
                701937250662140551 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean, System.String>(((UnityEngine.Material)instance).GetTag)),
                169846947363711211 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Material, UnityEngine.Material, System.Single>(((UnityEngine.Material)instance).Lerp)),
                898871137790755840 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).SetPass)),
                296599088695182875 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Material>(((UnityEngine.Material)instance).CopyPropertiesFromMaterial)),
                428860814760886102 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Material>(((UnityEngine.Material)instance).CopyMatchingPropertiesFromMaterial)),
                520889244261787951 => new Func<object, Delegate>((instance) => new Func<System.Int32>(((UnityEngine.Material)instance).ComputeCRC)),
                232677521880897945 => new Func<object, Delegate>((instance) => new Func<System.String[]>(((UnityEngine.Material)instance).GetTexturePropertyNames)),
                654952555307292597 => new Func<object, Delegate>((instance) => new Func<System.Int32[]>(((UnityEngine.Material)instance).GetTexturePropertyNameIDs)),
                184235746346374139 => new Func<object, Delegate>((instance) => new Action<System.Collections.Generic.List<System.String>>(((UnityEngine.Material)instance).GetTexturePropertyNames)),
                727002212458720096 => new Func<object, Delegate>((instance) => new Action<System.Collections.Generic.List<System.Int32>>(((UnityEngine.Material)instance).GetTexturePropertyNameIDs)),
#if UNITY_EDITOR
                131811510802306386 => new Func<object, Delegate>((instance) => new Func<UnityEngine.Material, System.Boolean>(((UnityEngine.Material)instance).IsChildOf)),
#endif
#if UNITY_EDITOR
                997292338752638819 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.Material)instance).RevertAllPropertyOverrides)),
#endif
#if UNITY_EDITOR
                880649141914244309 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).IsPropertyOverriden)),
#endif
#if UNITY_EDITOR
                884958275826449750 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).IsPropertyLocked)),
#endif
#if UNITY_EDITOR
                635805031451822893 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).IsPropertyLockedByAncestor)),
#endif
#if UNITY_EDITOR
                582237332456449377 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).IsPropertyOverriden)),
#endif
#if UNITY_EDITOR
                507992681499455626 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).IsPropertyLocked)),
#endif
#if UNITY_EDITOR
                830747267590145538 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).IsPropertyLockedByAncestor)),
#endif
#if UNITY_EDITOR
                658054593200172254 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Boolean>(((UnityEngine.Material)instance).SetPropertyLock)),
#endif
#if UNITY_EDITOR
                766063418025491616 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Material, System.Int32, System.Boolean>(((UnityEngine.Material)instance).ApplyPropertyOverride)),
#endif
#if UNITY_EDITOR
                877524395742888076 => new Func<object, Delegate>((instance) => new Action<System.Int32>(((UnityEngine.Material)instance).RevertPropertyOverride)),
#endif
#if UNITY_EDITOR
                258554646881846547 => new Func<object, Delegate>((instance) => new Action<System.String, System.Boolean>(((UnityEngine.Material)instance).SetPropertyLock)),
#endif
#if UNITY_EDITOR
                724166776623631545 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Material, System.String, System.Boolean>(((UnityEngine.Material)instance).ApplyPropertyOverride)),
#endif
#if UNITY_EDITOR
                822297840499524135 => new Func<object, Delegate>((instance) => new Action<System.String>(((UnityEngine.Material)instance).RevertPropertyOverride)),
#endif
                208287892225920172 => new Func<object, Delegate>((instance) => new Action<System.String, System.Int32>(((UnityEngine.Material)instance).SetInt)),
                542732527609949634 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Int32>(((UnityEngine.Material)instance).SetInt)),
                653449451556944169 => new Func<object, Delegate>((instance) => new Action<System.String, System.Single>(((UnityEngine.Material)instance).SetFloat)),
                346666018237639541 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Single>(((UnityEngine.Material)instance).SetFloat)),
                909328007176588938 => new Func<object, Delegate>((instance) => new Action<System.String, System.Int32>(((UnityEngine.Material)instance).SetInteger)),
                709743012492639226 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Int32>(((UnityEngine.Material)instance).SetInteger)),
                991755984275969084 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Color>(((UnityEngine.Material)instance).SetColor)),
                215621661443625293 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Color>(((UnityEngine.Material)instance).SetColor)),
                794486984752255500 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Vector4>(((UnityEngine.Material)instance).SetVector)),
                137019443055966638 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Vector4>(((UnityEngine.Material)instance).SetVector)),
                983973691928711416 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Matrix4x4>(((UnityEngine.Material)instance).SetMatrix)),
                668593283978552652 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Matrix4x4>(((UnityEngine.Material)instance).SetMatrix)),
                716332851433345006 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Texture>(((UnityEngine.Material)instance).SetTexture)),
                148969072237616980 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Texture>(((UnityEngine.Material)instance).SetTexture)),
                123674275962245551 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.RenderTexture, UnityEngine.Rendering.RenderTextureSubElement>(((UnityEngine.Material)instance).SetTexture)),
                464736051471746508 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.RenderTexture, UnityEngine.Rendering.RenderTextureSubElement>(((UnityEngine.Material)instance).SetTexture)),
                446379311999115541 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.ComputeBuffer>(((UnityEngine.Material)instance).SetBuffer)),
                638661389321674711 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.ComputeBuffer>(((UnityEngine.Material)instance).SetBuffer)),
                409431505226629497 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.GraphicsBuffer>(((UnityEngine.Material)instance).SetBuffer)),
                978508909921549404 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.GraphicsBuffer>(((UnityEngine.Material)instance).SetBuffer)),
                209605616360218674 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.ComputeBuffer, System.Int32, System.Int32>(((UnityEngine.Material)instance).SetConstantBuffer)),
                399152267377621538 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.ComputeBuffer, System.Int32, System.Int32>(((UnityEngine.Material)instance).SetConstantBuffer)),
                726389263861387511 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.GraphicsBuffer, System.Int32, System.Int32>(((UnityEngine.Material)instance).SetConstantBuffer)),
                505744724864878095 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.GraphicsBuffer, System.Int32, System.Int32>(((UnityEngine.Material)instance).SetConstantBuffer)),
                790088939608000002 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<System.Single>>(((UnityEngine.Material)instance).SetFloatArray)),
                308256530909329182 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<System.Single>>(((UnityEngine.Material)instance).SetFloatArray)),
                141770401585386578 => new Func<object, Delegate>((instance) => new Action<System.String, System.Single[]>(((UnityEngine.Material)instance).SetFloatArray)),
                743355893937982896 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Single[]>(((UnityEngine.Material)instance).SetFloatArray)),
                152874595451375043 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Color>>(((UnityEngine.Material)instance).SetColorArray)),
                869688657359349042 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Color>>(((UnityEngine.Material)instance).SetColorArray)),
                686885934165284802 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Color[]>(((UnityEngine.Material)instance).SetColorArray)),
                746412713610223255 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Color[]>(((UnityEngine.Material)instance).SetColorArray)),
                798364448117444639 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Vector4>>(((UnityEngine.Material)instance).SetVectorArray)),
                617317132393293810 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Vector4>>(((UnityEngine.Material)instance).SetVectorArray)),
                437106459616669373 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Vector4[]>(((UnityEngine.Material)instance).SetVectorArray)),
                838023205491362420 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Vector4[]>(((UnityEngine.Material)instance).SetVectorArray)),
                240095841563492908 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Matrix4x4>>(((UnityEngine.Material)instance).SetMatrixArray)),
                240499196735323813 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Matrix4x4>>(((UnityEngine.Material)instance).SetMatrixArray)),
                374327163209935428 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Matrix4x4[]>(((UnityEngine.Material)instance).SetMatrixArray)),
                974199901624958962 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Matrix4x4[]>(((UnityEngine.Material)instance).SetMatrixArray)),
                481223599689999593 => new Func<object, Delegate>((instance) => new Func<System.String, System.Int32>(((UnityEngine.Material)instance).GetInt)),
                818431654934196204 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Int32>(((UnityEngine.Material)instance).GetInt)),
                675150921162436515 => new Func<object, Delegate>((instance) => new Func<System.String, System.Single>(((UnityEngine.Material)instance).GetFloat)),
                594066965548269591 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Single>(((UnityEngine.Material)instance).GetFloat)),
                776851948149768180 => new Func<object, Delegate>((instance) => new Func<System.String, System.Int32>(((UnityEngine.Material)instance).GetInteger)),
                960819447299394473 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Int32>(((UnityEngine.Material)instance).GetInteger)),
                864302771678018853 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Color>(((UnityEngine.Material)instance).GetColor)),
                285741365984538532 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Color>(((UnityEngine.Material)instance).GetColor)),
                665814786928144039 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Vector4>(((UnityEngine.Material)instance).GetVector)),
                812303689604057618 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Vector4>(((UnityEngine.Material)instance).GetVector)),
                827196444773059971 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Matrix4x4>(((UnityEngine.Material)instance).GetMatrix)),
                136212407287052521 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Matrix4x4>(((UnityEngine.Material)instance).GetMatrix)),
                261686990555417668 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Texture>(((UnityEngine.Material)instance).GetTexture)),
                350469825672052861 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Texture>(((UnityEngine.Material)instance).GetTexture)),
                144021184177938427 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.GraphicsBufferHandle>(((UnityEngine.Material)instance).GetBuffer)),
                179276022850264662 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.GraphicsBufferHandle>(((UnityEngine.Material)instance).GetConstantBuffer)),
                422838464873363589 => new Func<object, Delegate>((instance) => new Func<System.String, System.Single[]>(((UnityEngine.Material)instance).GetFloatArray)),
                276890933491227600 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Single[]>(((UnityEngine.Material)instance).GetFloatArray)),
                414170125948777458 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Color[]>(((UnityEngine.Material)instance).GetColorArray)),
                652290497354575217 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Color[]>(((UnityEngine.Material)instance).GetColorArray)),
                609566408571387914 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Vector4[]>(((UnityEngine.Material)instance).GetVectorArray)),
                711468377137246094 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Vector4[]>(((UnityEngine.Material)instance).GetVectorArray)),
                898689192606766059 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Matrix4x4[]>(((UnityEngine.Material)instance).GetMatrixArray)),
                513128936123529301 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Matrix4x4[]>(((UnityEngine.Material)instance).GetMatrixArray)),
                613227577046018657 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<System.Single>>(((UnityEngine.Material)instance).GetFloatArray)),
                167075296818736544 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<System.Single>>(((UnityEngine.Material)instance).GetFloatArray)),
                628665188000486123 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Color>>(((UnityEngine.Material)instance).GetColorArray)),
                463893066920057302 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Color>>(((UnityEngine.Material)instance).GetColorArray)),
                840288628642161218 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Vector4>>(((UnityEngine.Material)instance).GetVectorArray)),
                183559815894318328 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Vector4>>(((UnityEngine.Material)instance).GetVectorArray)),
                307516442376874831 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Matrix4x4>>(((UnityEngine.Material)instance).GetMatrixArray)),
                938137372322473200 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Matrix4x4>>(((UnityEngine.Material)instance).GetMatrixArray)),
                256601653892057639 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Vector2>(((UnityEngine.Material)instance).SetTextureOffset)),
                917024311990256377 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Vector2>(((UnityEngine.Material)instance).SetTextureOffset)),
                533748347027583439 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Vector2>(((UnityEngine.Material)instance).SetTextureScale)),
                402229076363577910 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Vector2>(((UnityEngine.Material)instance).SetTextureScale)),
                626868638341682796 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Vector2>(((UnityEngine.Material)instance).GetTextureOffset)),
                521367298455056380 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Vector2>(((UnityEngine.Material)instance).GetTextureOffset)),
                354468062577077148 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Vector2>(((UnityEngine.Material)instance).GetTextureScale)),
                925867028239429244 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Vector2>(((UnityEngine.Material)instance).GetTextureScale)),
                306230572037790746 => new Func<object, Delegate>((instance) => new Func<UnityEngine.MaterialPropertyType, System.String[]>(((UnityEngine.Material)instance).GetPropertyNames)),
                _ => Infra.Singleton.GetIdToMethodMapForType(_typeReference.BaseType)(id),
            };
            return method;
        }
        public static MethodInfo _idToMethodInfo(long id)
        {
            MethodInfo methodDef = id switch
            {
                302221325844300932 => typeof(UnityEngine.Material).GetMethod(nameof(UnityEngine.Material.EnableKeyword), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Rendering.LocalKeyword).MakeByRefType() }, null),
                786347056447544880 => typeof(UnityEngine.Material).GetMethod(nameof(UnityEngine.Material.DisableKeyword), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Rendering.LocalKeyword).MakeByRefType() }, null),
                304697871243039999 => typeof(UnityEngine.Material).GetMethod(nameof(UnityEngine.Material.SetKeyword), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Rendering.LocalKeyword).MakeByRefType(), typeof(System.Boolean) }, null),
                586417498720952063 => typeof(UnityEngine.Material).GetMethod(nameof(UnityEngine.Material.IsKeywordEnabled), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Rendering.LocalKeyword).MakeByRefType() }, null),
                _ => Infra.Singleton.GetMethodInfoIdToMethodMapForType(_typeReference.BaseType)(id),
            };
            return methodDef;
        }
    }


    public class MaterialSaveData : AssetSaveData
    {
        public RandomId shader;
        public System.Int32 renderQueue;
        public UnityEngine.MaterialGlobalIlluminationFlags globalIlluminationFlags;
        public System.Boolean doubleSidedGI;
        public System.Boolean enableInstancing;
        public UnityEngine.HideFlags hideFlags;

        public List<FloatOverride> floats = new();
        public List<ColorOverride> colors = new();
        public List<VectorOverride> vectors = new();
        public List<TextureOverride> textures = new();

        public string[] shaderKeywords;
    }


    [Serializable]
    public struct FloatOverride
    {
        public string name;
        public float value;
    }

    [Serializable]
    public struct ColorOverride
    {
        public string name;
        public Color value;
    }

    [Serializable]
    public struct VectorOverride
    {
        public string name;
        public Vector4 value;
    }

    [Serializable]
    public struct TextureOverride
    {
        public string name;
        public RandomId texture;
        public Vector2 scale;
        public Vector2 offset;
    }


}