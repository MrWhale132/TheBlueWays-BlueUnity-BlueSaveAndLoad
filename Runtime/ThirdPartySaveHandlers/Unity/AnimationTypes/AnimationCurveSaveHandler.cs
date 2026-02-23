
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.UtilScripts;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity.AnimationTypes
{
    [SaveHandler(98234986982345, nameof(AnimationCurve), typeof(AnimationCurve))]
    public class AnimationCurveSaveHandler:UnmanagedSaveHandler<AnimationCurve,AnimationCurveSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            
            __saveData.keys = GetObjectId(__instance.keys);
        }

        public override void LoadPhase1()
        {
            base.LoadPhase1();

            __instance.keys = Infra.Singleton.GetObjectById<Keyframe[]>(__saveData.keys);
        }
    }

    public class AnimationCurveSaveData:SaveDataBase
    {
        public RandomId keys;
    }


}
