
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.UtilScripts.Misc;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases
{
    public class AssetSaveHandlerBase<TAsset, TSaveData> : SaveHandlerGenericBase<TAsset, TSaveData>
        where TAsset : UnityEngine.Object
        where TSaveData : AssetSaveData, new()
    {
        public bool IsProbablyUnmodifiedCopyOfOriginalAsset;
        public override bool IsValid => __instance != null;


        public override void Init(object instance)
        {
            base.Init(instance);

            IsProbablyUnmodifiedCopyOfOriginalAsset = __instance.IsProbablyUnmodifiedCopyOfOriginalAsset();

            __saveData.IsProbablyUnmodifiedCopyOfOriginalAsset = IsProbablyUnmodifiedCopyOfOriginalAsset;

            if (__saveData.IsProbablyUnmodifiedCopyOfOriginalAsset)
                __saveData._AssetId_ = AddressableDb.Singleton.GetAssetIdOfOriginalAssetFromCopy(__instance);
        }

        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.assetName = __instance.name;
        }


        public override void CreateObject()
        {
            base.CreateObject();

                HandledObjectId = __saveData._ObjectId_;


            IsProbablyUnmodifiedCopyOfOriginalAsset = __saveData.IsProbablyUnmodifiedCopyOfOriginalAsset;

            if (IsProbablyUnmodifiedCopyOfOriginalAsset)
            {
                _AssignInstance();


                Infra.Singleton.RegisterReference(__instance, HandledObjectId);
            }
            else
            {
                Debug.LogError($"Recreating runtime generated asset is not supported currently. AssetId: {__saveData._AssetId_}");
            }
        }

        public override void _AssignInstance()
        {
            if (IsProbablyUnmodifiedCopyOfOriginalAsset)
            {
                var orig = GetAssetById<TAsset>(__saveData._AssetId_, null);

                if (orig != null)
                {
                    var copy = Object.Instantiate(orig);

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
        public bool IsProbablyUnmodifiedCopyOfOriginalAsset;
        public string assetName;
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