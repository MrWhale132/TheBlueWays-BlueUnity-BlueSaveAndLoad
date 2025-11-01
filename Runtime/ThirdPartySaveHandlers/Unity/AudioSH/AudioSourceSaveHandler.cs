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
    [SaveHandler(560933255331122082, "AudioSource", typeof(UnityEngine.AudioSource), generationMode: SaveHandlerGenerationMode.FullAutomata)]
    public class AudioSourceSaveHandler : MonoSaveHandler<UnityEngine.AudioSource, AudioSourceSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            if(__instance.resource != null)
            {
            __saveData.time = __instance.time;
            __saveData.timeSamples = __instance.timeSamples;
            __saveData.resource = GetAssetId(__instance.resource);
            }
            __saveData.volume = __instance.volume;
            __saveData.outputAudioMixerGroup = GetAssetId(__instance.outputAudioMixerGroup);
            __saveData.gamepadSpeakerOutputType = __instance.gamepadSpeakerOutputType;
            __saveData.loop = __instance.loop;
            __saveData.ignoreListenerVolume = __instance.ignoreListenerVolume;
            __saveData.playOnAwake = __instance.playOnAwake;
            __saveData.ignoreListenerPause = __instance.ignoreListenerPause;
            __saveData.velocityUpdateMode = __instance.velocityUpdateMode;
            __saveData.panStereo = __instance.panStereo;
            __saveData.spatialBlend = __instance.spatialBlend;
            __saveData.spatialize = __instance.spatialize;
            __saveData.spatializePostEffects = __instance.spatializePostEffects;
            __saveData.reverbZoneMix = __instance.reverbZoneMix;
            __saveData.bypassEffects = __instance.bypassEffects;
            __saveData.bypassListenerEffects = __instance.bypassListenerEffects;
            __saveData.bypassReverbZones = __instance.bypassReverbZones;
            __saveData.dopplerLevel = __instance.dopplerLevel;
            __saveData.spread = __instance.spread;
            __saveData.priority = __instance.priority;
            __saveData.mute = __instance.mute;
            __saveData.minDistance = __instance.minDistance;
            __saveData.maxDistance = __instance.maxDistance;
            __saveData.rolloffMode = __instance.rolloffMode;
            __saveData.enabled = __instance.enabled;
            __saveData.hideFlags = __instance.hideFlags;
        }

        public override void LoadReferences()
        {
            base.LoadReferences();
            __instance.outputAudioMixerGroup = GetAssetById(__saveData.outputAudioMixerGroup, __instance.outputAudioMixerGroup);
            var resource = GetAssetById(__saveData.resource, __instance.resource);
            if (resource != null && resource != __instance.resource)
            {
                __instance.resource = resource;
                __instance.time = __saveData.time;
                __instance.timeSamples = __saveData.timeSamples;
            }
            __instance.volume = __saveData.volume;
            __instance.gamepadSpeakerOutputType = __saveData.gamepadSpeakerOutputType;
            __instance.loop = __saveData.loop;
            __instance.ignoreListenerVolume = __saveData.ignoreListenerVolume;
            __instance.playOnAwake = __saveData.playOnAwake;
            __instance.ignoreListenerPause = __saveData.ignoreListenerPause;
            __instance.velocityUpdateMode = __saveData.velocityUpdateMode;
            __instance.panStereo = __saveData.panStereo;
            __instance.spatialBlend = __saveData.spatialBlend;
            __instance.spatialize = __saveData.spatialize;
            __instance.spatializePostEffects = __saveData.spatializePostEffects;
            __instance.reverbZoneMix = __saveData.reverbZoneMix;
            __instance.bypassEffects = __saveData.bypassEffects;
            __instance.bypassListenerEffects = __saveData.bypassListenerEffects;
            __instance.bypassReverbZones = __saveData.bypassReverbZones;
            __instance.dopplerLevel = __saveData.dopplerLevel;
            __instance.spread = __saveData.spread;
            __instance.priority = __saveData.priority;
            __instance.mute = __saveData.mute;
            __instance.minDistance = __saveData.minDistance;
            __instance.maxDistance = __saveData.maxDistance;
            __instance.rolloffMode = __saveData.rolloffMode;
            __instance.enabled = __saveData.enabled;
            __instance.hideFlags = __saveData.hideFlags;
        }

        static AudioSourceSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
                {"PlayOnGamepad(mscorlib System.Int32):mscorlib System.Boolean", 732176665521984435},
                {"DisableGamepadOutput():mscorlib System.Boolean", 912366887665600428},
                {"SetGamepadSpeakerMixLevel(mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Boolean", 228337772445679717},
                {"SetGamepadSpeakerMixLevelDefault(mscorlib System.Int32):mscorlib System.Boolean", 936638999982146620},
                {"SetGamepadSpeakerRestrictedAudio(mscorlib System.Int32,mscorlib System.Boolean):mscorlib System.Boolean", 757469444109699258},
                {"Play():mscorlib System.Void", 259944878029805805},
                {"Play(mscorlib System.UInt64):mscorlib System.Void", 957728478038289746},
                {"PlayDelayed(mscorlib System.Single):mscorlib System.Void", 271358131529489429},
                {"PlayScheduled(mscorlib System.Double):mscorlib System.Void", 143156584974916872},
                {"PlayOneShot(UnityEngine.AudioModule UnityEngine.AudioClip):mscorlib System.Void", 460968694965985069},
                {"PlayOneShot(UnityEngine.AudioModule UnityEngine.AudioClip,mscorlib System.Single):mscorlib System.Void", 491487315751350606},
                {"SetScheduledStartTime(mscorlib System.Double):mscorlib System.Void", 918456262809894747},
                {"SetScheduledEndTime(mscorlib System.Double):mscorlib System.Void", 187254383250428905},
                {"Stop():mscorlib System.Void", 184522441695761351},
                {"Pause():mscorlib System.Void", 965416290247189966},
                {"UnPause():mscorlib System.Void", 356581378859917704},
                {"SetCustomCurve(UnityEngine.AudioModule UnityEngine.AudioSourceCurveType,UnityEngine.CoreModule UnityEngine.AnimationCurve):mscorlib System.Void", 966723220340679409},
                {"GetCustomCurve(UnityEngine.AudioModule UnityEngine.AudioSourceCurveType):UnityEngine.CoreModule UnityEngine.AnimationCurve", 538404743132948400},
                {"GetOutputData(mscorlib System.Single[],mscorlib System.Int32):mscorlib System.Void", 953325092828941726},
                {"GetSpectrumData(mscorlib System.Single[],mscorlib System.Int32,UnityEngine.AudioModule UnityEngine.FFTWindow):mscorlib System.Void", 645207909730217954},
                {"SetSpatializerFloat(mscorlib System.Int32,mscorlib System.Single):mscorlib System.Boolean", 610240292518121185},
                {"GetSpatializerFloat(mscorlib System.Int32,mscorlib System.Single&):mscorlib System.Boolean", 575531625150008610},
                {"GetAmbisonicDecoderFloat(mscorlib System.Int32,mscorlib System.Single&):mscorlib System.Boolean", 630590539061812148},
                {"SetAmbisonicDecoderFloat(mscorlib System.Int32,mscorlib System.Single):mscorlib System.Boolean", 907861413140040155}
            };
            Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
            Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
            Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(UnityEngine.AudioSource);
        static Type _typeDefinition = typeof(UnityEngine.AudioSource);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                732176665521984435 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.AudioSource)instance).PlayOnGamepad)),
                912366887665600428 => new Func<object, Delegate>((instance) => new Func<System.Boolean>(((UnityEngine.AudioSource)instance).DisableGamepadOutput)),
                228337772445679717 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Int32, System.Boolean>(((UnityEngine.AudioSource)instance).SetGamepadSpeakerMixLevel)),
                936638999982146620 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean>(((UnityEngine.AudioSource)instance).SetGamepadSpeakerMixLevelDefault)),
                757469444109699258 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Boolean, System.Boolean>(((UnityEngine.AudioSource)instance).SetGamepadSpeakerRestrictedAudio)),
                259944878029805805 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.AudioSource)instance).Play)),
                957728478038289746 => new Func<object, Delegate>((instance) => new Action<System.UInt64>(((UnityEngine.AudioSource)instance).Play)),
                271358131529489429 => new Func<object, Delegate>((instance) => new Action<System.Single>(((UnityEngine.AudioSource)instance).PlayDelayed)),
                143156584974916872 => new Func<object, Delegate>((instance) => new Action<System.Double>(((UnityEngine.AudioSource)instance).PlayScheduled)),
                460968694965985069 => new Func<object, Delegate>((instance) => new Action<UnityEngine.AudioClip>(((UnityEngine.AudioSource)instance).PlayOneShot)),
                491487315751350606 => new Func<object, Delegate>((instance) => new Action<UnityEngine.AudioClip, System.Single>(((UnityEngine.AudioSource)instance).PlayOneShot)),
                918456262809894747 => new Func<object, Delegate>((instance) => new Action<System.Double>(((UnityEngine.AudioSource)instance).SetScheduledStartTime)),
                187254383250428905 => new Func<object, Delegate>((instance) => new Action<System.Double>(((UnityEngine.AudioSource)instance).SetScheduledEndTime)),
                184522441695761351 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.AudioSource)instance).Stop)),
                965416290247189966 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.AudioSource)instance).Pause)),
                356581378859917704 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.AudioSource)instance).UnPause)),
                966723220340679409 => new Func<object, Delegate>((instance) => new Action<UnityEngine.AudioSourceCurveType, UnityEngine.AnimationCurve>(((UnityEngine.AudioSource)instance).SetCustomCurve)),
                538404743132948400 => new Func<object, Delegate>((instance) => new Func<UnityEngine.AudioSourceCurveType, UnityEngine.AnimationCurve>(((UnityEngine.AudioSource)instance).GetCustomCurve)),
                953325092828941726 => new Func<object, Delegate>((instance) => new Action<System.Single[], System.Int32>(((UnityEngine.AudioSource)instance).GetOutputData)),
                645207909730217954 => new Func<object, Delegate>((instance) => new Action<System.Single[], System.Int32, UnityEngine.FFTWindow>(((UnityEngine.AudioSource)instance).GetSpectrumData)),
                610240292518121185 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Single, System.Boolean>(((UnityEngine.AudioSource)instance).SetSpatializerFloat)),
                907861413140040155 => new Func<object, Delegate>((instance) => new Func<System.Int32, System.Single, System.Boolean>(((UnityEngine.AudioSource)instance).SetAmbisonicDecoderFloat)),
                _ => Infra.Singleton.GetIdToMethodMapForType(_typeReference.BaseType)(id),
            };
            return method;
        }
        public static MethodInfo _idToMethodInfo(long id)
        {
            MethodInfo methodDef = id switch
            {
                575531625150008610 => typeof(UnityEngine.AudioSource).GetMethod(nameof(UnityEngine.AudioSource.GetSpatializerFloat), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(System.Int32), typeof(System.Single).MakeByRefType() }, null),
                630590539061812148 => typeof(UnityEngine.AudioSource).GetMethod(nameof(UnityEngine.AudioSource.GetAmbisonicDecoderFloat), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(System.Int32), typeof(System.Single).MakeByRefType() }, null),
                _ => Infra.Singleton.GetMethodInfoIdToMethodMapForType(_typeReference.BaseType)(id),
            };
            return methodDef;
        }
    }


    public class AudioSourceSaveData : MonoSaveDataBase
    {
        public System.Single volume;
        public System.Single time;
        public System.Int32 timeSamples;
        public RandomId resource;
        public RandomId outputAudioMixerGroup;
        public UnityEngine.GamepadSpeakerOutputType gamepadSpeakerOutputType;
        public System.Boolean loop;
        public System.Boolean ignoreListenerVolume;
        public System.Boolean playOnAwake;
        public System.Boolean ignoreListenerPause;
        public UnityEngine.AudioVelocityUpdateMode velocityUpdateMode;
        public System.Single panStereo;
        public System.Single spatialBlend;
        public System.Boolean spatialize;
        public System.Boolean spatializePostEffects;
        public System.Single reverbZoneMix;
        public System.Boolean bypassEffects;
        public System.Boolean bypassListenerEffects;
        public System.Boolean bypassReverbZones;
        public System.Single dopplerLevel;
        public System.Single spread;
        public System.Int32 priority;
        public System.Boolean mute;
        public System.Single minDistance;
        public System.Single maxDistance;
        public UnityEngine.AudioRolloffMode rolloffMode;
        public System.Boolean enabled;
        public UnityEngine.HideFlags hideFlags;
    }


    public class StaticAudioSourceSubtitute : StaticSubtitute
    {
        public override Type SubtitutedType => typeof(UnityEngine.AudioSource);
    }

    [SaveHandler(455102278940854590, "StaticAudioSourceSubtitute", typeof(StaticAudioSourceSubtitute), generationMode: SaveHandlerGenerationMode.FullAutomata, staticHandlerOf: typeof(UnityEngine.AudioSource))]
    public class StaticAudioSourceSaveHandler : StaticSaveHandlerBase<StaticAudioSourceSubtitute, StaticAudioSourceSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();

        }

        public override void LoadReferences()
        {
            base.LoadReferences();

        }
        static StaticAudioSourceSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
                {"GamepadSpeakerSupportsOutputType(UnityEngine.AudioModule UnityEngine.GamepadSpeakerOutputType):mscorlib System.Boolean", 674886960180149985},
                {"PlayClipAtPoint(UnityEngine.AudioModule UnityEngine.AudioClip,UnityEngine.CoreModule UnityEngine.Vector3):mscorlib System.Void", 346974444306878819},
                {"PlayClipAtPoint(UnityEngine.AudioModule UnityEngine.AudioClip,UnityEngine.CoreModule UnityEngine.Vector3,mscorlib System.Single):mscorlib System.Void", 167824208889096924}
            };
            Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
            Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
            Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(UnityEngine.AudioSource);
        static Type _typeDefinition = typeof(UnityEngine.AudioSource);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                674886960180149985 => new Func<object, Delegate>((instance) => new Func<UnityEngine.GamepadSpeakerOutputType, System.Boolean>(UnityEngine.AudioSource.GamepadSpeakerSupportsOutputType)),
                346974444306878819 => new Func<object, Delegate>((instance) => new Action<UnityEngine.AudioClip, UnityEngine.Vector3>(UnityEngine.AudioSource.PlayClipAtPoint)),
                167824208889096924 => new Func<object, Delegate>((instance) => new Action<UnityEngine.AudioClip, UnityEngine.Vector3, System.Single>(UnityEngine.AudioSource.PlayClipAtPoint)),
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


    public class StaticAudioSourceSaveData : StaticSaveDataBase
    {

    }

}