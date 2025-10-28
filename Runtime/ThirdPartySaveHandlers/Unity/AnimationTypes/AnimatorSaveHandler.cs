
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity.AnimationTypes
{
    [SaveHandler(9821232136982345, nameof(Animator), typeof(Animator))]
    public class AnimatorSaveHandler : MonoSaveHandler<Animator, AnimatorSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();

            __saveData.runtimeAnimatorController = GetAssetId(__instance.runtimeAnimatorController);
            __saveData.avatar = GetAssetId(__instance.avatar);
        }

        public override void LoadReferences()
        {
            base.LoadReferences();

            __instance.runtimeAnimatorController = AddressableDb.Singleton.GetAssetByIdOrFallback(
                __instance.runtimeAnimatorController, ref __saveData.runtimeAnimatorController);

            __instance.avatar = AddressableDb.Singleton.GetAssetByIdOrFallback(
                __instance.avatar, ref __saveData.avatar);

            //__instance.Play();
        }
    }

    public class AnimatorSaveData : MonoSaveDataBase
    {
        public RandomId runtimeAnimatorController;
        public RandomId avatar;
    }
}
