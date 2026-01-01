#if UNITY_EDITOR

using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using System;
using UnityEngine.Audio;

namespace Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.ThirdPartySaveHandlers.Unity.UnityEditor_
{
    [SaveHandler(id: 111945637836243288, "AudioMixerGroupController", handledType: null, RequiresManualAttributeCreation = true)]
    public class AudioMixerGroupControllerSaveHandler : AssetSaveHandlerBase<AudioMixerGroup, AudioMixerGroupControllerSaveData>
    {
        public static SaveHandlerAttribute ManualSaveHandlerAttributeCreation()
        {
            var type = Type.GetType("UnityEditor.Audio.AudioMixerGroupController, UnityEditor.CoreModule");

            var attr = SaveHandlerBase.ManualSaveHandlerAttributeCreation(type, typeof(AudioMixerGroupControllerSaveHandler));

            return attr;
        }
    }

    public class AudioMixerGroupControllerSaveData : AssetSaveData
    {
    }
}

#endif