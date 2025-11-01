//auto-generated
using UnityEngine;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace DevTest
{
	[SaveHandler(485481875751895630, "Material", typeof(UnityEngine.Material),order:-8)]
	public class MaterialSaveHandler : AssetSaveHandlerBase<UnityEngine.Material, MaterialSaveData> 
	{
		public override void WriteSaveData()
		{
			if (IsProbablyUnmodifiedCopyOfOriginalAsset) return;
			base.WriteSaveData();
			__saveData.shader = GetAssetId(__instance.shader);
			__saveData.renderQueue = __instance.renderQueue;
			__saveData.globalIlluminationFlags = __instance.globalIlluminationFlags;
			__saveData.doubleSidedGI = __instance.doubleSidedGI;
			__saveData.enableInstancing = __instance.enableInstancing;
			__saveData.parent = GetAssetId(__instance.parent);
			__saveData.hideFlags = __instance.hideFlags;
		}

        public override void _AssignInstance()
        {
            if (IsProbablyUnmodifiedCopyOfOriginalAsset)
            {
                var orig = GetAssetById<Material>(__saveData._AssetId_, null);

                if (orig != null)
                {
					var copy = new Material(orig);

                    __instance = copy;
                }
            }
        }
        public override void LoadReferences()
		{
            if (IsProbablyUnmodifiedCopyOfOriginalAsset) return;
            base.LoadReferences();
			__instance.shader = GetAssetById(__saveData.shader, __instance.shader);
			__instance.renderQueue = __saveData.renderQueue;
			__instance.globalIlluminationFlags = __saveData.globalIlluminationFlags;
			__instance.doubleSidedGI = __saveData.doubleSidedGI;
			__instance.enableInstancing = __saveData.enableInstancing;
			__instance.parent = GetAssetById(__saveData.parent, __instance.parent);
			__instance.hideFlags = __saveData.hideFlags;
		}

		static MaterialSaveHandler()
		{
			Dictionary<string, long> methodToId = new()
			{
				{"HasProperty(mscorlib System.Int32):mscorlib System.Boolean", 472435341783000368},
				{"HasProperty(mscorlib System.String):mscorlib System.Boolean", 185050022788045718},
				{"HasFloat(mscorlib System.String):mscorlib System.Boolean", 495004009265152755},
				{"HasFloat(mscorlib System.Int32):mscorlib System.Boolean", 532130265264839187},
				{"HasInt(mscorlib System.String):mscorlib System.Boolean", 308194185763638927},
				{"HasInt(mscorlib System.Int32):mscorlib System.Boolean", 625573884533696544},
				{"HasInteger(mscorlib System.String):mscorlib System.Boolean", 733794559253590156},
				{"HasInteger(mscorlib System.Int32):mscorlib System.Boolean", 273431065117718428},
				{"HasTexture(mscorlib System.String):mscorlib System.Boolean", 482092462094243937},
				{"HasTexture(mscorlib System.Int32):mscorlib System.Boolean", 130173933244506277},
				{"HasMatrix(mscorlib System.String):mscorlib System.Boolean", 417006906096378039},
				{"HasMatrix(mscorlib System.Int32):mscorlib System.Boolean", 810905811749707661},
				{"HasVector(mscorlib System.String):mscorlib System.Boolean", 902327694697647735},
				{"HasVector(mscorlib System.Int32):mscorlib System.Boolean", 640118515728961603},
				{"HasColor(mscorlib System.String):mscorlib System.Boolean", 659321122410107101},
				{"HasColor(mscorlib System.Int32):mscorlib System.Boolean", 178480296673593318},
				{"HasBuffer(mscorlib System.String):mscorlib System.Boolean", 632909600552175953},
				{"HasBuffer(mscorlib System.Int32):mscorlib System.Boolean", 516803445981688857},
				{"HasConstantBuffer(mscorlib System.String):mscorlib System.Boolean", 907877073449764089},
				{"HasConstantBuffer(mscorlib System.Int32):mscorlib System.Boolean", 272855471987421894},
				{"EnableKeyword(mscorlib System.String):mscorlib System.Void", 664883090512759878},
				{"DisableKeyword(mscorlib System.String):mscorlib System.Void", 505140608592786942},
				{"IsKeywordEnabled(mscorlib System.String):mscorlib System.Boolean", 628885585923855249},
				{"EnableKeyword(UnityEngine.CoreModule UnityEngine.Rendering.LocalKeyword&):mscorlib System.Void", 941327540883671624},
				{"DisableKeyword(UnityEngine.CoreModule UnityEngine.Rendering.LocalKeyword&):mscorlib System.Void", 152955478474643799},
				{"SetKeyword(UnityEngine.CoreModule UnityEngine.Rendering.LocalKeyword&,mscorlib System.Boolean):mscorlib System.Void", 120033570124212028},
				{"IsKeywordEnabled(UnityEngine.CoreModule UnityEngine.Rendering.LocalKeyword&):mscorlib System.Boolean", 609685807513097150},
				{"SetShaderPassEnabled(mscorlib System.String,mscorlib System.Boolean):mscorlib System.Void", 492837434425162509},
				{"GetShaderPassEnabled(mscorlib System.String):mscorlib System.Boolean", 153522696713616163},
				{"GetPassName(mscorlib System.Int32):mscorlib System.String", 686115048144148700},
				{"FindPass(mscorlib System.String):mscorlib System.Int32", 787013171171999992},
				{"SetOverrideTag(mscorlib System.String,mscorlib System.String):mscorlib System.Void", 494747455166490548},
				{"GetTag(mscorlib System.String,mscorlib System.Boolean,mscorlib System.String):mscorlib System.String", 656293487624515641},
				{"GetTag(mscorlib System.String,mscorlib System.Boolean):mscorlib System.String", 604966891478135133},
				{"Lerp(UnityEngine.CoreModule UnityEngine.Material,UnityEngine.CoreModule UnityEngine.Material,mscorlib System.Single):mscorlib System.Void", 619025072054084994},
				{"SetPass(mscorlib System.Int32):mscorlib System.Boolean", 459930201859948202},
				{"CopyPropertiesFromMaterial(UnityEngine.CoreModule UnityEngine.Material):mscorlib System.Void", 257898373639313077},
				{"CopyMatchingPropertiesFromMaterial(UnityEngine.CoreModule UnityEngine.Material):mscorlib System.Void", 567982412311687273},
				{"ComputeCRC():mscorlib System.Int32", 756066656253587074},
				{"GetTexturePropertyNames():mscorlib System.String[]", 527447606205114931},
				{"GetTexturePropertyNameIDs():mscorlib System.Int32[]", 249122616882450364},
				{"GetTexturePropertyNames(mscorlib System.Collections.Generic.List<mscorlib System.String>):mscorlib System.Void", 621836945103854593},
				{"GetTexturePropertyNameIDs(mscorlib System.Collections.Generic.List<mscorlib System.Int32>):mscorlib System.Void", 430572171375474031},
				{"IsChildOf(UnityEngine.CoreModule UnityEngine.Material):mscorlib System.Boolean", 120318751202795693},
				{"RevertAllPropertyOverrides():mscorlib System.Void", 415966456063982356},
				{"IsPropertyOverriden(mscorlib System.Int32):mscorlib System.Boolean", 301182197968978152},
				{"IsPropertyLocked(mscorlib System.Int32):mscorlib System.Boolean", 606496179731044939},
				{"IsPropertyLockedByAncestor(mscorlib System.Int32):mscorlib System.Boolean", 123287959808472974},
				{"IsPropertyOverriden(mscorlib System.String):mscorlib System.Boolean", 191591236582404902},
				{"IsPropertyLocked(mscorlib System.String):mscorlib System.Boolean", 932843636025806984},
				{"IsPropertyLockedByAncestor(mscorlib System.String):mscorlib System.Boolean", 852295137740457904},
				{"SetPropertyLock(mscorlib System.Int32,mscorlib System.Boolean):mscorlib System.Void", 224796420067541915},
				{"ApplyPropertyOverride(UnityEngine.CoreModule UnityEngine.Material,mscorlib System.Int32,mscorlib System.Boolean):mscorlib System.Void", 163761689667789276},
				{"RevertPropertyOverride(mscorlib System.Int32):mscorlib System.Void", 105677416929844293},
				{"SetPropertyLock(mscorlib System.String,mscorlib System.Boolean):mscorlib System.Void", 245300685327736590},
				{"ApplyPropertyOverride(UnityEngine.CoreModule UnityEngine.Material,mscorlib System.String,mscorlib System.Boolean):mscorlib System.Void", 939525476972259603},
				{"RevertPropertyOverride(mscorlib System.String):mscorlib System.Void", 170499218918334426},
				{"SetInt(mscorlib System.String,mscorlib System.Int32):mscorlib System.Void", 551082360307812352},
				{"SetInt(mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 943931755864773214},
				{"SetFloat(mscorlib System.String,mscorlib System.Single):mscorlib System.Void", 298812980000598257},
				{"SetFloat(mscorlib System.Int32,mscorlib System.Single):mscorlib System.Void", 342033650563350725},
				{"SetInteger(mscorlib System.String,mscorlib System.Int32):mscorlib System.Void", 413004064527651606},
				{"SetInteger(mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 306515232774977284},
				{"SetColor(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Color):mscorlib System.Void", 414706812280175351},
				{"SetColor(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Color):mscorlib System.Void", 889887239055937728},
				{"SetVector(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Vector4):mscorlib System.Void", 340960294159851549},
				{"SetVector(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Vector4):mscorlib System.Void", 553656775735334711},
				{"SetMatrix(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Matrix4x4):mscorlib System.Void", 291958207919349727},
				{"SetMatrix(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Matrix4x4):mscorlib System.Void", 558329456981514045},
				{"SetTexture(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Texture):mscorlib System.Void", 759445985548561625},
				{"SetTexture(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Texture):mscorlib System.Void", 377240132530529407},
				{"SetTexture(mscorlib System.String,UnityEngine.CoreModule UnityEngine.RenderTexture,UnityEngine.CoreModule UnityEngine.Rendering.RenderTextureSubElement):mscorlib System.Void", 801343390731089886},
				{"SetTexture(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.RenderTexture,UnityEngine.CoreModule UnityEngine.Rendering.RenderTextureSubElement):mscorlib System.Void", 840920334592906481},
				{"SetBuffer(mscorlib System.String,UnityEngine.CoreModule UnityEngine.ComputeBuffer):mscorlib System.Void", 583394949055163265},
				{"SetBuffer(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.ComputeBuffer):mscorlib System.Void", 697001879883569749},
				{"SetBuffer(mscorlib System.String,UnityEngine.CoreModule UnityEngine.GraphicsBuffer):mscorlib System.Void", 445598127943863179},
				{"SetBuffer(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.GraphicsBuffer):mscorlib System.Void", 438141415648532382},
				{"SetConstantBuffer(mscorlib System.String,UnityEngine.CoreModule UnityEngine.ComputeBuffer,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 900897393692948030},
				{"SetConstantBuffer(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.ComputeBuffer,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 229417348840047116},
				{"SetConstantBuffer(mscorlib System.String,UnityEngine.CoreModule UnityEngine.GraphicsBuffer,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 964085781369040126},
				{"SetConstantBuffer(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.GraphicsBuffer,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 359244945265483117},
				{"SetFloatArray(mscorlib System.String,mscorlib System.Collections.Generic.List<mscorlib System.Single>):mscorlib System.Void", 603363698889082086},
				{"SetFloatArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<mscorlib System.Single>):mscorlib System.Void", 771913051698683351},
				{"SetFloatArray(mscorlib System.String,mscorlib System.Single[]):mscorlib System.Void", 398655605527239643},
				{"SetFloatArray(mscorlib System.Int32,mscorlib System.Single[]):mscorlib System.Void", 679931190920573079},
				{"SetColorArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Color>):mscorlib System.Void", 701621842435893978},
				{"SetColorArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Color>):mscorlib System.Void", 336756467405325093},
				{"SetColorArray(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Color[]):mscorlib System.Void", 304718866503750584},
				{"SetColorArray(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Color[]):mscorlib System.Void", 395515334701980967},
				{"SetVectorArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>):mscorlib System.Void", 441798996474483179},
				{"SetVectorArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>):mscorlib System.Void", 878847406552279303},
				{"SetVectorArray(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Vector4[]):mscorlib System.Void", 350652995021780119},
				{"SetVectorArray(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Vector4[]):mscorlib System.Void", 860438243310518082},
				{"SetMatrixArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Matrix4x4>):mscorlib System.Void", 812584237476216944},
				{"SetMatrixArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Matrix4x4>):mscorlib System.Void", 382240868541370829},
				{"SetMatrixArray(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Matrix4x4[]):mscorlib System.Void", 737619086719357379},
				{"SetMatrixArray(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Matrix4x4[]):mscorlib System.Void", 957162668600649731},
				{"GetInt(mscorlib System.String):mscorlib System.Int32", 776215415829309086},
				{"GetInt(mscorlib System.Int32):mscorlib System.Int32", 256595037516062669},
				{"GetFloat(mscorlib System.String):mscorlib System.Single", 374175674930055258},
				{"GetFloat(mscorlib System.Int32):mscorlib System.Single", 517128880886763071},
				{"GetInteger(mscorlib System.String):mscorlib System.Int32", 397182110645170414},
				{"GetInteger(mscorlib System.Int32):mscorlib System.Int32", 314538488816707496},
				{"GetColor(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Color", 132401724471634976},
				{"GetColor(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Color", 665960429393867338},
				{"GetVector(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Vector4", 271381401728183182},
				{"GetVector(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Vector4", 117207132060329679},
				{"GetMatrix(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Matrix4x4", 528445960121068422},
				{"GetMatrix(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Matrix4x4", 297324843039850365},
				{"GetTexture(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Texture", 466133601109189757},
				{"GetTexture(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Texture", 210450283126060306},
				{"GetBuffer(mscorlib System.String):UnityEngine.CoreModule UnityEngine.GraphicsBufferHandle", 606371274485019314},
				{"GetConstantBuffer(mscorlib System.String):UnityEngine.CoreModule UnityEngine.GraphicsBufferHandle", 500694057413987215},
				{"GetFloatArray(mscorlib System.String):mscorlib System.Single[]", 284818570701674680},
				{"GetFloatArray(mscorlib System.Int32):mscorlib System.Single[]", 690231486059770413},
				{"GetColorArray(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Color[]", 168101324573920751},
				{"GetColorArray(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Color[]", 485243252003125051},
				{"GetVectorArray(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Vector4[]", 248980780212901802},
				{"GetVectorArray(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Vector4[]", 872595832145887951},
				{"GetMatrixArray(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Matrix4x4[]", 350455303010131228},
				{"GetMatrixArray(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Matrix4x4[]", 965305764432504055},
				{"GetFloatArray(mscorlib System.String,mscorlib System.Collections.Generic.List<mscorlib System.Single>):mscorlib System.Void", 341629182290356809},
				{"GetFloatArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<mscorlib System.Single>):mscorlib System.Void", 694469186140517607},
				{"GetColorArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Color>):mscorlib System.Void", 723611356318660446},
				{"GetColorArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Color>):mscorlib System.Void", 177244203168365817},
				{"GetVectorArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>):mscorlib System.Void", 697211286847006162},
				{"GetVectorArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>):mscorlib System.Void", 147930718894275285},
				{"GetMatrixArray(mscorlib System.String,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Matrix4x4>):mscorlib System.Void", 875696745505110286},
				{"GetMatrixArray(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Matrix4x4>):mscorlib System.Void", 165750238257282038},
				{"SetTextureOffset(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Vector2):mscorlib System.Void", 745028421268399922},
				{"SetTextureOffset(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Vector2):mscorlib System.Void", 112536849832318254},
				{"SetTextureScale(mscorlib System.String,UnityEngine.CoreModule UnityEngine.Vector2):mscorlib System.Void", 294622906085826902},
				{"SetTextureScale(mscorlib System.Int32,UnityEngine.CoreModule UnityEngine.Vector2):mscorlib System.Void", 610827968895914482},
				{"GetTextureOffset(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Vector2", 839963976301631664},
				{"GetTextureOffset(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Vector2", 426665401106134316},
				{"GetTextureScale(mscorlib System.String):UnityEngine.CoreModule UnityEngine.Vector2", 365794156725435916},
				{"GetTextureScale(mscorlib System.Int32):UnityEngine.CoreModule UnityEngine.Vector2", 616355287926732539},
				{"GetPropertyNames(UnityEngine.CoreModule UnityEngine.MaterialPropertyType):mscorlib System.String[]", 862151968566112760}
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
				472435341783000368 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasProperty)),
				185050022788045718 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasProperty)),
				495004009265152755 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasFloat)),
				532130265264839187 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasFloat)),
				308194185763638927 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasInt)),
				625573884533696544 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasInt)),
				733794559253590156 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasInteger)),
				273431065117718428 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasInteger)),
				482092462094243937 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasTexture)),
				130173933244506277 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasTexture)),
				417006906096378039 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasMatrix)),
				810905811749707661 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasMatrix)),
				902327694697647735 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasVector)),
				640118515728961603 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasVector)),
				659321122410107101 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasColor)),
				178480296673593318 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasColor)),
				632909600552175953 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasBuffer)),
				516803445981688857 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasBuffer)),
				907877073449764089 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).HasConstantBuffer)),
				272855471987421894 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).HasConstantBuffer)),
				664883090512759878 => new Func<object, Delegate>((instance) => new Action<System.String>(((UnityEngine.Material)instance).EnableKeyword)),
				505140608592786942 => new Func<object, Delegate>((instance) => new Action<System.String>(((UnityEngine.Material)instance).DisableKeyword)),
				628885585923855249 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).IsKeywordEnabled)),
				492837434425162509 => new Func<object, Delegate>((instance) => new Action<System.String, System.Boolean>(((UnityEngine.Material)instance).SetShaderPassEnabled)),
				153522696713616163 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).GetShaderPassEnabled)),
				686115048144148700 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.String>(((UnityEngine.Material)instance).GetPassName)),
				787013171171999992 => new Func<object, Delegate>((instance) => new Func<System.String, System.Int32>(((UnityEngine.Material)instance).FindPass)),
				494747455166490548 => new Func<object, Delegate>((instance) => new Action<System.String, System.String>(((UnityEngine.Material)instance).SetOverrideTag)),
				656293487624515641 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean, System.String, System.String>(((UnityEngine.Material)instance).GetTag)),
				604966891478135133 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean, System.String>(((UnityEngine.Material)instance).GetTag)),
				619025072054084994 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Material, UnityEngine.Material, System.Single>(((UnityEngine.Material)instance).Lerp)),
				459930201859948202 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).SetPass)),
				257898373639313077 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Material>(((UnityEngine.Material)instance).CopyPropertiesFromMaterial)),
				567982412311687273 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Material>(((UnityEngine.Material)instance).CopyMatchingPropertiesFromMaterial)),
				756066656253587074 => new Func<object, Delegate>((instance) => new Func<System.Int32>(((UnityEngine.Material)instance).ComputeCRC)),
				527447606205114931 => new Func<object, Delegate>((instance) => new Func<System.String[]>(((UnityEngine.Material)instance).GetTexturePropertyNames)),
				249122616882450364 => new Func<object, Delegate>((instance) => new Func<System.Int32[]>(((UnityEngine.Material)instance).GetTexturePropertyNameIDs)),
				621836945103854593 => new Func<object, Delegate>((instance) => new Action<System.Collections.Generic.List<System.String>>(((UnityEngine.Material)instance).GetTexturePropertyNames)),
				430572171375474031 => new Func<object, Delegate>((instance) => new Action<System.Collections.Generic.List<System.Int32>>(((UnityEngine.Material)instance).GetTexturePropertyNameIDs)),
				120318751202795693 => new Func<object, Delegate>((instance) => new Func<UnityEngine.Material, System.Boolean>(((UnityEngine.Material)instance).IsChildOf)),
				415966456063982356 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.Material)instance).RevertAllPropertyOverrides)),
				301182197968978152 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).IsPropertyOverriden)),
				606496179731044939 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).IsPropertyLocked)),
				123287959808472974 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.Material)instance).IsPropertyLockedByAncestor)),
				191591236582404902 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).IsPropertyOverriden)),
				932843636025806984 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).IsPropertyLocked)),
				852295137740457904 => new Func<object, Delegate>((instance) => new Func<System.String, System.Boolean>(((UnityEngine.Material)instance).IsPropertyLockedByAncestor)),
				224796420067541915 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Boolean>(((UnityEngine.Material)instance).SetPropertyLock)),
				163761689667789276 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Material, System.Int32, System.Boolean>(((UnityEngine.Material)instance).ApplyPropertyOverride)),
				105677416929844293 => new Func<object, Delegate>((instance) => new Action<System.Int32>(((UnityEngine.Material)instance).RevertPropertyOverride)),
				245300685327736590 => new Func<object, Delegate>((instance) => new Action<System.String, System.Boolean>(((UnityEngine.Material)instance).SetPropertyLock)),
				939525476972259603 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Material, System.String, System.Boolean>(((UnityEngine.Material)instance).ApplyPropertyOverride)),
				170499218918334426 => new Func<object, Delegate>((instance) => new Action<System.String>(((UnityEngine.Material)instance).RevertPropertyOverride)),
				551082360307812352 => new Func<object, Delegate>((instance) => new Action<System.String, System.Int32>(((UnityEngine.Material)instance).SetInt)),
				943931755864773214 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Int32>(((UnityEngine.Material)instance).SetInt)),
				298812980000598257 => new Func<object, Delegate>((instance) => new Action<System.String, System.Single>(((UnityEngine.Material)instance).SetFloat)),
				342033650563350725 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Single>(((UnityEngine.Material)instance).SetFloat)),
				413004064527651606 => new Func<object, Delegate>((instance) => new Action<System.String, System.Int32>(((UnityEngine.Material)instance).SetInteger)),
				306515232774977284 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Int32>(((UnityEngine.Material)instance).SetInteger)),
				414706812280175351 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Color>(((UnityEngine.Material)instance).SetColor)),
				889887239055937728 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Color>(((UnityEngine.Material)instance).SetColor)),
				340960294159851549 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Vector4>(((UnityEngine.Material)instance).SetVector)),
				553656775735334711 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Vector4>(((UnityEngine.Material)instance).SetVector)),
				291958207919349727 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Matrix4x4>(((UnityEngine.Material)instance).SetMatrix)),
				558329456981514045 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Matrix4x4>(((UnityEngine.Material)instance).SetMatrix)),
				759445985548561625 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Texture>(((UnityEngine.Material)instance).SetTexture)),
				377240132530529407 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Texture>(((UnityEngine.Material)instance).SetTexture)),
				801343390731089886 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.RenderTexture, UnityEngine.Rendering.RenderTextureSubElement>(((UnityEngine.Material)instance).SetTexture)),
				840920334592906481 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.RenderTexture, UnityEngine.Rendering.RenderTextureSubElement>(((UnityEngine.Material)instance).SetTexture)),
				583394949055163265 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.ComputeBuffer>(((UnityEngine.Material)instance).SetBuffer)),
				697001879883569749 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.ComputeBuffer>(((UnityEngine.Material)instance).SetBuffer)),
				445598127943863179 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.GraphicsBuffer>(((UnityEngine.Material)instance).SetBuffer)),
				438141415648532382 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.GraphicsBuffer>(((UnityEngine.Material)instance).SetBuffer)),
				900897393692948030 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.ComputeBuffer, System.Int32, System.Int32>(((UnityEngine.Material)instance).SetConstantBuffer)),
				229417348840047116 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.ComputeBuffer, System.Int32, System.Int32>(((UnityEngine.Material)instance).SetConstantBuffer)),
				964085781369040126 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.GraphicsBuffer, System.Int32, System.Int32>(((UnityEngine.Material)instance).SetConstantBuffer)),
				359244945265483117 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.GraphicsBuffer, System.Int32, System.Int32>(((UnityEngine.Material)instance).SetConstantBuffer)),
				603363698889082086 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<System.Single>>(((UnityEngine.Material)instance).SetFloatArray)),
				771913051698683351 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<System.Single>>(((UnityEngine.Material)instance).SetFloatArray)),
				398655605527239643 => new Func<object, Delegate>((instance) => new Action<System.String, System.Single[]>(((UnityEngine.Material)instance).SetFloatArray)),
				679931190920573079 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Single[]>(((UnityEngine.Material)instance).SetFloatArray)),
				701621842435893978 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Color>>(((UnityEngine.Material)instance).SetColorArray)),
				336756467405325093 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Color>>(((UnityEngine.Material)instance).SetColorArray)),
				304718866503750584 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Color[]>(((UnityEngine.Material)instance).SetColorArray)),
				395515334701980967 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Color[]>(((UnityEngine.Material)instance).SetColorArray)),
				441798996474483179 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Vector4>>(((UnityEngine.Material)instance).SetVectorArray)),
				878847406552279303 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Vector4>>(((UnityEngine.Material)instance).SetVectorArray)),
				350652995021780119 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Vector4[]>(((UnityEngine.Material)instance).SetVectorArray)),
				860438243310518082 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Vector4[]>(((UnityEngine.Material)instance).SetVectorArray)),
				812584237476216944 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Matrix4x4>>(((UnityEngine.Material)instance).SetMatrixArray)),
				382240868541370829 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Matrix4x4>>(((UnityEngine.Material)instance).SetMatrixArray)),
				737619086719357379 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Matrix4x4[]>(((UnityEngine.Material)instance).SetMatrixArray)),
				957162668600649731 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Matrix4x4[]>(((UnityEngine.Material)instance).SetMatrixArray)),
				776215415829309086 => new Func<object, Delegate>((instance) => new Func<System.String, System.Int32>(((UnityEngine.Material)instance).GetInt)),
				256595037516062669 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Int32>(((UnityEngine.Material)instance).GetInt)),
				374175674930055258 => new Func<object, Delegate>((instance) => new Func<System.String, System.Single>(((UnityEngine.Material)instance).GetFloat)),
				517128880886763071 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Single>(((UnityEngine.Material)instance).GetFloat)),
				397182110645170414 => new Func<object, Delegate>((instance) => new Func<System.String, System.Int32>(((UnityEngine.Material)instance).GetInteger)),
				314538488816707496 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Int32>(((UnityEngine.Material)instance).GetInteger)),
				132401724471634976 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Color>(((UnityEngine.Material)instance).GetColor)),
				665960429393867338 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Color>(((UnityEngine.Material)instance).GetColor)),
				271381401728183182 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Vector4>(((UnityEngine.Material)instance).GetVector)),
				117207132060329679 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Vector4>(((UnityEngine.Material)instance).GetVector)),
				528445960121068422 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Matrix4x4>(((UnityEngine.Material)instance).GetMatrix)),
				297324843039850365 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Matrix4x4>(((UnityEngine.Material)instance).GetMatrix)),
				466133601109189757 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Texture>(((UnityEngine.Material)instance).GetTexture)),
				210450283126060306 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Texture>(((UnityEngine.Material)instance).GetTexture)),
				606371274485019314 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.GraphicsBufferHandle>(((UnityEngine.Material)instance).GetBuffer)),
				500694057413987215 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.GraphicsBufferHandle>(((UnityEngine.Material)instance).GetConstantBuffer)),
				284818570701674680 => new Func<object, Delegate>((instance) => new Func<System.String, System.Single[]>(((UnityEngine.Material)instance).GetFloatArray)),
				690231486059770413 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Single[]>(((UnityEngine.Material)instance).GetFloatArray)),
				168101324573920751 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Color[]>(((UnityEngine.Material)instance).GetColorArray)),
				485243252003125051 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Color[]>(((UnityEngine.Material)instance).GetColorArray)),
				248980780212901802 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Vector4[]>(((UnityEngine.Material)instance).GetVectorArray)),
				872595832145887951 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Vector4[]>(((UnityEngine.Material)instance).GetVectorArray)),
				350455303010131228 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Matrix4x4[]>(((UnityEngine.Material)instance).GetMatrixArray)),
				965305764432504055 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Matrix4x4[]>(((UnityEngine.Material)instance).GetMatrixArray)),
				341629182290356809 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<System.Single>>(((UnityEngine.Material)instance).GetFloatArray)),
				694469186140517607 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<System.Single>>(((UnityEngine.Material)instance).GetFloatArray)),
				723611356318660446 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Color>>(((UnityEngine.Material)instance).GetColorArray)),
				177244203168365817 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Color>>(((UnityEngine.Material)instance).GetColorArray)),
				697211286847006162 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Vector4>>(((UnityEngine.Material)instance).GetVectorArray)),
				147930718894275285 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Vector4>>(((UnityEngine.Material)instance).GetVectorArray)),
				875696745505110286 => new Func<object, Delegate>((instance) => new Action<System.String, System.Collections.Generic.List<UnityEngine.Matrix4x4>>(((UnityEngine.Material)instance).GetMatrixArray)),
				165750238257282038 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.Matrix4x4>>(((UnityEngine.Material)instance).GetMatrixArray)),
				745028421268399922 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Vector2>(((UnityEngine.Material)instance).SetTextureOffset)),
				112536849832318254 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Vector2>(((UnityEngine.Material)instance).SetTextureOffset)),
				294622906085826902 => new Func<object, Delegate>((instance) => new Action<System.String, UnityEngine.Vector2>(((UnityEngine.Material)instance).SetTextureScale)),
				610827968895914482 => new Func<object, Delegate>((instance) => new Action<System.Int32, UnityEngine.Vector2>(((UnityEngine.Material)instance).SetTextureScale)),
				839963976301631664 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Vector2>(((UnityEngine.Material)instance).GetTextureOffset)),
				426665401106134316 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Vector2>(((UnityEngine.Material)instance).GetTextureOffset)),
				365794156725435916 => new Func<object, Delegate>((instance) => new Func<System.String, UnityEngine.Vector2>(((UnityEngine.Material)instance).GetTextureScale)),
				616355287926732539 => new Func<object, Delegate>((instance) => new Func<System.Int32, UnityEngine.Vector2>(((UnityEngine.Material)instance).GetTextureScale)),
				862151968566112760 => new Func<object, Delegate>((instance) => new Func<UnityEngine.MaterialPropertyType, System.String[]>(((UnityEngine.Material)instance).GetPropertyNames)),
				_ => Infra.Singleton.GetIdToMethodMapForType(_typeReference.BaseType)(id),
			};
			return method;
		}
		public static MethodInfo _idToMethodInfo(long id)
		{
			MethodInfo methodDef = id switch
			{
				941327540883671624 => typeof(UnityEngine.Material).GetMethod(nameof(UnityEngine.Material.EnableKeyword), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Rendering.LocalKeyword).MakeByRefType() }, null),
				152955478474643799 => typeof(UnityEngine.Material).GetMethod(nameof(UnityEngine.Material.DisableKeyword), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Rendering.LocalKeyword).MakeByRefType() }, null),
				120033570124212028 => typeof(UnityEngine.Material).GetMethod(nameof(UnityEngine.Material.SetKeyword), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Rendering.LocalKeyword).MakeByRefType(), typeof(System.Boolean) }, null),
				609685807513097150 => typeof(UnityEngine.Material).GetMethod(nameof(UnityEngine.Material.IsKeywordEnabled), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Rendering.LocalKeyword).MakeByRefType() }, null),
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
		public RandomId parent;
		public UnityEngine.HideFlags hideFlags;
	}


	public class StaticMaterialSubtitute : StaticSubtitute 
	{
		public override Type SubtitutedType => typeof(UnityEngine.Material);
	}

	[SaveHandler(341386988675097556, "StaticMaterialSubtitute", typeof(StaticMaterialSubtitute), generationMode: SaveHandlerGenerationMode.FullAutomata, staticHandlerOf: typeof(UnityEngine.Material))]
	public class StaticMaterialSaveHandler : StaticSaveHandlerBase<StaticMaterialSubtitute, StaticMaterialSaveData> 
	{
		public override void WriteSaveData()
		{
			base.WriteSaveData();

		}

		public override void LoadReferences()
		{
			base.LoadReferences();

		}
		static StaticMaterialSaveHandler()
		{
			Dictionary<string, long> methodToId = new()
			{

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


	public class StaticMaterialSaveData : StaticSaveDataBase 
	{

	}

}