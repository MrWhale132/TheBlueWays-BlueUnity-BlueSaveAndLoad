
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.Misc;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases
{
    public class AssetSaveHandlerBase<TAsset, TSaveData> : SaveHandlerGenericBase<TAsset, TSaveData>
        where TAsset : UnityEngine.Object
        where TSaveData : AssetSaveData, new()
    {
        public bool IsDefensiveCopyOfOriginal => __saveData.instanceCopiedFromAsset.IsNotDefault;
        public bool IsOriginalAsset => __saveData._AssetId_.IsNotDefault;
        public bool IsRuntimeGenerated => !IsOriginalAsset && !IsDefensiveCopyOfOriginal;
        public override bool IsValid => __instance != null;
        public virtual bool CopyRegardlessIfItIsOriginal => false;
        public virtual bool SupportsModificationsToTheInstance => false;


        public override void Init(object instance)
        {
            base.Init(instance);

            __saveData.IsDefensiveCopyOfOriginal = __instance.IsDefensiveCopyOfOriginal(out string originalName);

            var assetId = GetAssetId2(__instance);

            bool assetWasFound = assetId.IsNotDefault;
            __saveData.assetWasFound = assetWasFound;

            if (assetWasFound)
            {
                __saveData.originalAssetsName = originalName;

                if (IsDefensiveCopyOfOriginal)
                {
                    __saveData.instanceCopiedFromAsset = assetId;
                }
                else
                {
                    __saveData._AssetId_ = assetId;
                }
            }
            else if (!assetWasFound && !SupportsModificationsToTheInstance)
            {
                Debug.LogError($"not found asset, name: {__instance.name}, type: {__instance.GetType().Name}");
            }

            __saveData.IsRuntimeGenerated = IsRuntimeGenerated;
            __saveData.SupportsModificationsToTheInstance = SupportsModificationsToTheInstance;
        }


        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.name = __instance.name;
        }


        public override void CreateObject()
        {
            base.CreateObject();

            HandledObjectId = __saveData._ObjectId_;


            if (IsRuntimeGenerated && !SupportsModificationsToTheInstance)
            {
                Debug.LogError($"Recreating runtime generated asset is not supported currently. ObjectId: {HandledObjectId}");
            }
            else
            {
                _AssignInstance();

                Infra.Singleton.RegisterReference(__instance, HandledObjectId, rootObject: __saveData._isRootObject_);
            }
        }

        public override void _AssignInstance()
        {
            if (IsOriginalAsset)
            {
                __instance = GetAssetById2<TAsset>(__saveData._AssetId_, null);

                if (CopyRegardlessIfItIsOriginal)
                {
                    __instance = Object.Instantiate(__instance);

                    __instance.name = __saveData.name; //unity ads a (Clone) suffix
                }
            }
            else if (IsDefensiveCopyOfOriginal)
            {
                var orig = GetAssetById2<TAsset>(__saveData.instanceCopiedFromAsset, null);

                if (orig != null)
                {
                    var copy = Object.Instantiate(orig);

                    copy.name = __saveData.originalAssetsName; //unity ads a (Clone) suffix

                    __instance = copy;
                }
                else
                {
                    Debug.LogError("error");
                }
            }
        }
    }


    public class AssetSaveData : SaveDataBase
    {
        public RandomId _AssetId_;
        public RandomId instanceCopiedFromAsset;
        public bool IsDefensiveCopyOfOriginal;
        public bool assetWasFound;
        public bool IsRuntimeGenerated;
        public bool SupportsModificationsToTheInstance;
        public string originalAssetsName;
        public string name;
    }
}


/*
| asset type                                       | original clone best practice                                                       |
| ------------------------------------------------ | ---------------------------------------------------------------------------------- |
| Material                                         | `new Material(mat)` (best) OR `Instantiate(mat)`                                   |
| Mesh                                             | `Instantiate(mesh)` **only**. (`new Mesh(mesh)` doesn’t exist)                     |
| Texture2D                                        | `Instantiate(tex)`                                                                 |
| Cubemap                                          | `Instantiate(cubemap)`                                                             |
| AudioClip                                        | `Instantiate(audioClip)` (works for non streaming clips)                           |
| Shader                                           | **cannot** really clone (they’re engine level singletons)                          |
| ComputeShader                                    | `Instantiate(computeShader)`                                                       |
| AnimationClip                                    | `Instantiate(clip)`                                                                |
| AnimatorController                               | `Instantiate(controller)`                                                          |
| AvatarMask                                       | `Instantiate(mask)`                                                                |
| PhysicsMaterial / PhysicsMaterial2D              | `Instantiate(mat)`                                                                 |
| Sprite                                           | `Instantiate(sprite)` OR `Sprite.Create(...)` if you want modify pixels/boundaries |
| Prefab instance                                  | you can't “clone” the asset, but you `Instantiate(prefab)` to spawn runtime clone  |
| ScriptableObject                                 | `Instantiate(scriptableObj)`                                                       |
| MeshCollider.sharedMesh clone                    | `mc.sharedMesh = Instantiate(originalMesh)`                                        |
| RenderTexture                                    | `new RenderTexture(rt)` (copy constructor)                                         |
| Font                                             | `Instantiate(font)`                                                                |
| VFXGraph asset                                   | `Instantiate(vfxGraph)`                                                            |
| Timeline PlayableAsset / PlayableDirector config | `Instantiate(playable)`                                                            |
*/