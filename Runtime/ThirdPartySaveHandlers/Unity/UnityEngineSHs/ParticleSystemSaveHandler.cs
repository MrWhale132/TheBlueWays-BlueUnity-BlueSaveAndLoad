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
    [SaveHandler(521664889686445854, "ParticleSystem", typeof(UnityEngine.ParticleSystem), generationMode: SaveHandlerGenerationMode.FullAutomata)]
    public class ParticleSystemSaveHandler : MonoSaveHandler<UnityEngine.ParticleSystem, ParticleSystemSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.time = __instance.time;
            __saveData.randomSeed = __instance.randomSeed;
            __saveData.useAutoRandomSeed = __instance.useAutoRandomSeed;
            __saveData.hideFlags = __instance.hideFlags;
        }

        public override void LoadReferences()
        {
            base.LoadReferences();
            __instance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            __instance.time = __saveData.time;
            __instance.randomSeed = __saveData.randomSeed;
            __instance.useAutoRandomSeed = __saveData.useAutoRandomSeed;
            __instance.hideFlags = __saveData.hideFlags;
        }

        static ParticleSystemSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
                {"SetParticles(UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle[],mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 563826332231445504},
                {"SetParticles(UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle[],mscorlib System.Int32):mscorlib System.Void", 392984952240952324},
                {"SetParticles(UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle[]):mscorlib System.Void", 869422255723193055},
                {"SetParticles(UnityEngine.CoreModule Unity.Collections.NativeArray<UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle>,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 257887419138902073},
                {"SetParticles(UnityEngine.CoreModule Unity.Collections.NativeArray<UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle>,mscorlib System.Int32):mscorlib System.Void", 597073923815689616},
                {"SetParticles(UnityEngine.CoreModule Unity.Collections.NativeArray<UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle>):mscorlib System.Void", 797290202671102066},
                {"GetParticles(UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle[],mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Int32", 817496360454828537},
                {"GetParticles(UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle[],mscorlib System.Int32):mscorlib System.Int32", 629677024160362389},
                {"GetParticles(UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle[]):mscorlib System.Int32", 920183084785266443},
                {"GetParticles(UnityEngine.CoreModule Unity.Collections.NativeArray<UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle>,mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Int32", 714036219919143960},
                {"GetParticles(UnityEngine.CoreModule Unity.Collections.NativeArray<UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle>,mscorlib System.Int32):mscorlib System.Int32", 644669234990381867},
                {"GetParticles(UnityEngine.CoreModule Unity.Collections.NativeArray<UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle>):mscorlib System.Int32", 589486407553139680},
                {"SetCustomParticleData(mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemCustomData):mscorlib System.Void", 904570232862737637},
                {"GetCustomParticleData(mscorlib System.Collections.Generic.List<UnityEngine.CoreModule UnityEngine.Vector4>,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemCustomData):mscorlib System.Int32", 225726167075070196},
                {"GetPlaybackState():UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+PlaybackState", 201815353482549255},
                {"SetPlaybackState(UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+PlaybackState):mscorlib System.Void", 528830325678263143},
                {"GetTrails():UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Trails", 621036278442681467},
                {"GetTrails(UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Trails&):mscorlib System.Int32", 837059977411930662},
                {"SetTrails(UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Trails):mscorlib System.Void", 548156297309789630},
                {"Simulate(mscorlib System.Single,mscorlib System.Boolean,mscorlib System.Boolean,mscorlib System.Boolean):mscorlib System.Void", 657106975650292992},
                {"Simulate(mscorlib System.Single,mscorlib System.Boolean,mscorlib System.Boolean):mscorlib System.Void", 867186166873220900},
                {"Simulate(mscorlib System.Single,mscorlib System.Boolean):mscorlib System.Void", 624128063611679787},
                {"Simulate(mscorlib System.Single):mscorlib System.Void", 357004009242029136},
                {"Play(mscorlib System.Boolean):mscorlib System.Void", 863902254519344265},
                {"Play():mscorlib System.Void", 442911871058154291},
                {"Pause(mscorlib System.Boolean):mscorlib System.Void", 434485948989886478},
                {"Pause():mscorlib System.Void", 475112019636515283},
                {"Stop(mscorlib System.Boolean,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemStopBehavior):mscorlib System.Void", 185706289227277681},
                {"Stop(mscorlib System.Boolean):mscorlib System.Void", 611147521247469339},
                {"Stop():mscorlib System.Void", 367893379146983366},
                {"Clear(mscorlib System.Boolean):mscorlib System.Void", 294818503175893731},
                {"Clear():mscorlib System.Void", 582876838525410314},
                {"IsAlive(mscorlib System.Boolean):mscorlib System.Boolean", 544938920335215187},
                {"IsAlive():mscorlib System.Boolean", 431797998876548929},
                {"Emit(mscorlib System.Int32):mscorlib System.Void", 754495640320022297},
                {"Emit(UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+EmitParams,mscorlib System.Int32):mscorlib System.Void", 374693904388739543},
                {"TriggerSubEmitter(mscorlib System.Int32):mscorlib System.Void", 394504767885425064},
                {"TriggerSubEmitter(mscorlib System.Int32,UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle&):mscorlib System.Void", 595148136169147890},
                {"TriggerSubEmitter(mscorlib System.Int32,mscorlib System.Collections.Generic.List<UnityEngine.ParticleSystemModule UnityEngine.ParticleSystem+Particle>):mscorlib System.Void", 377993612985366405},
                {"AllocateAxisOfRotationAttribute():mscorlib System.Void", 814647869773753578},
                {"AllocateMeshIndexAttribute():mscorlib System.Void", 750253514189264208},
                {"AllocateCustomDataAttribute(UnityEngine.ParticleSystemModule UnityEngine.ParticleSystemCustomData):mscorlib System.Void", 662912484912278864}
            };
            Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
            Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
            Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(UnityEngine.ParticleSystem);
        static Type _typeDefinition = typeof(UnityEngine.ParticleSystem);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                563826332231445504 => new Func<object, Delegate>((instance) => new Action<ParticleSystem.Particle[], System.Int32, System.Int32>(((UnityEngine.ParticleSystem)instance).SetParticles)),
                392984952240952324 => new Func<object, Delegate>((instance) => new Action<ParticleSystem.Particle[], System.Int32>(((UnityEngine.ParticleSystem)instance).SetParticles)),
                869422255723193055 => new Func<object, Delegate>((instance) => new Action<ParticleSystem.Particle[]>(((UnityEngine.ParticleSystem)instance).SetParticles)),
                257887419138902073 => new Func<object, Delegate>((instance) => new Action<Unity.Collections.NativeArray<UnityEngine.ParticleSystem.Particle>, System.Int32, System.Int32>(((UnityEngine.ParticleSystem)instance).SetParticles)),
                597073923815689616 => new Func<object, Delegate>((instance) => new Action<Unity.Collections.NativeArray<UnityEngine.ParticleSystem.Particle>, System.Int32>(((UnityEngine.ParticleSystem)instance).SetParticles)),
                797290202671102066 => new Func<object, Delegate>((instance) => new Action<Unity.Collections.NativeArray<UnityEngine.ParticleSystem.Particle>>(((UnityEngine.ParticleSystem)instance).SetParticles)),
                817496360454828537 => new Func<object, Delegate>((instance) => new Func<ParticleSystem.Particle[], System.Int32, System.Int32, System.Int32>(((UnityEngine.ParticleSystem)instance).GetParticles)),
                629677024160362389 => new Func<object, Delegate>((instance) => new Func<ParticleSystem.Particle[], System.Int32, System.Int32>(((UnityEngine.ParticleSystem)instance).GetParticles)),
                920183084785266443 => new Func<object, Delegate>((instance) => new Func<ParticleSystem.Particle[], System.Int32>(((UnityEngine.ParticleSystem)instance).GetParticles)),
                714036219919143960 => new Func<object, Delegate>((instance) => new Func<Unity.Collections.NativeArray<UnityEngine.ParticleSystem.Particle>, System.Int32, System.Int32, System.Int32>(((UnityEngine.ParticleSystem)instance).GetParticles)),
                644669234990381867 => new Func<object, Delegate>((instance) => new Func<Unity.Collections.NativeArray<UnityEngine.ParticleSystem.Particle>, System.Int32, System.Int32>(((UnityEngine.ParticleSystem)instance).GetParticles)),
                589486407553139680 => new Func<object, Delegate>((instance) => new Func<Unity.Collections.NativeArray<UnityEngine.ParticleSystem.Particle>, System.Int32>(((UnityEngine.ParticleSystem)instance).GetParticles)),
                904570232862737637 => new Func<object, Delegate>((instance) => new Action<System.Collections.Generic.List<UnityEngine.Vector4>, UnityEngine.ParticleSystemCustomData>(((UnityEngine.ParticleSystem)instance).SetCustomParticleData)),
                225726167075070196 => new Func<object, Delegate>((instance) => new Func<System.Collections.Generic.List<UnityEngine.Vector4>, UnityEngine.ParticleSystemCustomData, System.Int32>(((UnityEngine.ParticleSystem)instance).GetCustomParticleData)),
                201815353482549255 => new Func<object, Delegate>((instance) => new Func<UnityEngine.ParticleSystem.PlaybackState>(((UnityEngine.ParticleSystem)instance).GetPlaybackState)),
                528830325678263143 => new Func<object, Delegate>((instance) => new Action<UnityEngine.ParticleSystem.PlaybackState>(((UnityEngine.ParticleSystem)instance).SetPlaybackState)),
                621036278442681467 => new Func<object, Delegate>((instance) => new Func<UnityEngine.ParticleSystem.Trails>(((UnityEngine.ParticleSystem)instance).GetTrails)),
                548156297309789630 => new Func<object, Delegate>((instance) => new Action<UnityEngine.ParticleSystem.Trails>(((UnityEngine.ParticleSystem)instance).SetTrails)),
                657106975650292992 => new Func<object, Delegate>((instance) => new Action<System.Single, System.Boolean, System.Boolean, System.Boolean>(((UnityEngine.ParticleSystem)instance).Simulate)),
                867186166873220900 => new Func<object, Delegate>((instance) => new Action<System.Single, System.Boolean, System.Boolean>(((UnityEngine.ParticleSystem)instance).Simulate)),
                624128063611679787 => new Func<object, Delegate>((instance) => new Action<System.Single, System.Boolean>(((UnityEngine.ParticleSystem)instance).Simulate)),
                357004009242029136 => new Func<object, Delegate>((instance) => new Action<System.Single>(((UnityEngine.ParticleSystem)instance).Simulate)),
                863902254519344265 => new Func<object, Delegate>((instance) => new Action<System.Boolean>(((UnityEngine.ParticleSystem)instance).Play)),
                442911871058154291 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.ParticleSystem)instance).Play)),
                434485948989886478 => new Func<object, Delegate>((instance) => new Action<System.Boolean>(((UnityEngine.ParticleSystem)instance).Pause)),
                475112019636515283 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.ParticleSystem)instance).Pause)),
                185706289227277681 => new Func<object, Delegate>((instance) => new Action<System.Boolean, UnityEngine.ParticleSystemStopBehavior>(((UnityEngine.ParticleSystem)instance).Stop)),
                611147521247469339 => new Func<object, Delegate>((instance) => new Action<System.Boolean>(((UnityEngine.ParticleSystem)instance).Stop)),
                367893379146983366 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.ParticleSystem)instance).Stop)),
                294818503175893731 => new Func<object, Delegate>((instance) => new Action<System.Boolean>(((UnityEngine.ParticleSystem)instance).Clear)),
                582876838525410314 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.ParticleSystem)instance).Clear)),
                544938920335215187 => new Func<object, Delegate>((instance) => new Func<System.Boolean, System.Boolean>(((UnityEngine.ParticleSystem)instance).IsAlive)),
                431797998876548929 => new Func<object, Delegate>((instance) => new Func<System.Boolean>(((UnityEngine.ParticleSystem)instance).IsAlive)),
                754495640320022297 => new Func<object, Delegate>((instance) => new Action<System.Int32>(((UnityEngine.ParticleSystem)instance).Emit)),
                374693904388739543 => new Func<object, Delegate>((instance) => new Action<UnityEngine.ParticleSystem.EmitParams, System.Int32>(((UnityEngine.ParticleSystem)instance).Emit)),
                394504767885425064 => new Func<object, Delegate>((instance) => new Action<System.Int32>(((UnityEngine.ParticleSystem)instance).TriggerSubEmitter)),
                377993612985366405 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Collections.Generic.List<UnityEngine.ParticleSystem.Particle>>(((UnityEngine.ParticleSystem)instance).TriggerSubEmitter)),
                814647869773753578 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.ParticleSystem)instance).AllocateAxisOfRotationAttribute)),
                750253514189264208 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.ParticleSystem)instance).AllocateMeshIndexAttribute)),
                662912484912278864 => new Func<object, Delegate>((instance) => new Action<UnityEngine.ParticleSystemCustomData>(((UnityEngine.ParticleSystem)instance).AllocateCustomDataAttribute)),
                _ => Infra.Singleton.GetIdToMethodMapForType(_typeReference.BaseType)(id),
            };
            return method;
        }
        public static MethodInfo _idToMethodInfo(long id)
        {
            MethodInfo methodDef = id switch
            {
                837059977411930662 => typeof(UnityEngine.ParticleSystem).GetMethod(nameof(UnityEngine.ParticleSystem.GetTrails), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.ParticleSystem.Trails).MakeByRefType() }, null),
                595148136169147890 => typeof(UnityEngine.ParticleSystem).GetMethod(nameof(UnityEngine.ParticleSystem.TriggerSubEmitter), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(System.Int32), typeof(UnityEngine.ParticleSystem.Particle).MakeByRefType() }, null),
                _ => Infra.Singleton.GetMethodInfoIdToMethodMapForType(_typeReference.BaseType)(id),
            };
            return methodDef;
        }
    }


    public class ParticleSystemSaveData : MonoSaveDataBase
    {
        public System.Single time;
        public System.UInt32 randomSeed;
        public System.Boolean useAutoRandomSeed;
        public UnityEngine.HideFlags hideFlags;
    }


    public class StaticParticleSystemSubtitute : StaticSubtitute
    {
        public override Type SubtitutedType => typeof(UnityEngine.ParticleSystem);
    }

    [SaveHandler(789916589031912030, "StaticParticleSystemSubtitute", typeof(StaticParticleSystemSubtitute), generationMode: SaveHandlerGenerationMode.FullAutomata, staticHandlerOf: typeof(UnityEngine.ParticleSystem))]
    public class StaticParticleSystemSaveHandler : StaticSaveHandlerBase<StaticParticleSystemSubtitute, StaticParticleSystemSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();

        }

        public override void LoadReferences()
        {
            base.LoadReferences();

        }
        static StaticParticleSystemSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
                {"ResetPreMappedBufferMemory():mscorlib System.Void", 366472277692266564},
                {"SetMaximumPreMappedBufferCounts(mscorlib System.Int32,mscorlib System.Int32):mscorlib System.Void", 684638109844378711}
            };
            Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
            Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
            Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(UnityEngine.ParticleSystem);
        static Type _typeDefinition = typeof(UnityEngine.ParticleSystem);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                366472277692266564 => new Func<object, Delegate>((instance) => new Action(UnityEngine.ParticleSystem.ResetPreMappedBufferMemory)),
                684638109844378711 => new Func<object, Delegate>((instance) => new Action<System.Int32, System.Int32>(UnityEngine.ParticleSystem.SetMaximumPreMappedBufferCounts)),
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


    public class StaticParticleSystemSaveData : StaticSaveDataBase
    {

    }

}