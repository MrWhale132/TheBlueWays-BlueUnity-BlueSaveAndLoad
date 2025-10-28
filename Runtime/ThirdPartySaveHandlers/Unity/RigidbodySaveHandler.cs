
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity
{
    [SaveHandler(146976697126058400, "Rigidbody", typeof(UnityEngine.Rigidbody))]
    public class RigidbodySaveHandler : MonoSaveHandler<UnityEngine.Rigidbody, RigidbodySaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.linearVelocity = __instance.linearVelocity;
            __saveData.angularVelocity = __instance.angularVelocity;
            __saveData.linearDamping = __instance.linearDamping;
            __saveData.angularDamping = __instance.angularDamping;
            __saveData.mass = __instance.mass;
            __saveData.useGravity = __instance.useGravity;
            __saveData.maxDepenetrationVelocity = __instance.maxDepenetrationVelocity;
            __saveData.isKinematic = __instance.isKinematic;
            __saveData.constraints = __instance.constraints;
            __saveData.collisionDetectionMode = __instance.collisionDetectionMode;
            __saveData.automaticCenterOfMass = __instance.automaticCenterOfMass;
            __saveData.centerOfMass = __instance.centerOfMass;
            __saveData.automaticInertiaTensor = __instance.automaticInertiaTensor;
            __saveData.inertiaTensorRotation = __instance.inertiaTensorRotation;
            __saveData.inertiaTensor = __instance.inertiaTensor;
            __saveData.detectCollisions = __instance.detectCollisions;
            __saveData.position = __instance.position;
            __saveData.rotation = __instance.rotation;
            __saveData.interpolation = __instance.interpolation;
            __saveData.solverIterations = __instance.solverIterations;
            __saveData.sleepThreshold = __instance.sleepThreshold;
            __saveData.maxAngularVelocity = __instance.maxAngularVelocity;
            __saveData.maxLinearVelocity = __instance.maxLinearVelocity;
            __saveData.solverVelocityIterations = __instance.solverVelocityIterations;
            __saveData.excludeLayers = __instance.excludeLayers;
            __saveData.includeLayers = __instance.includeLayers;
            __saveData.hideFlags = __instance.hideFlags;
        }
        public override void LoadReferences()
        {
            base.LoadReferences();
            __instance.linearVelocity = __saveData.linearVelocity;
            __instance.angularVelocity = __saveData.angularVelocity;
            __instance.linearDamping = __saveData.linearDamping;
            __instance.angularDamping = __saveData.angularDamping;
            __instance.mass = __saveData.mass;
            __instance.useGravity = __saveData.useGravity;
            __instance.maxDepenetrationVelocity = __saveData.maxDepenetrationVelocity;
            __instance.isKinematic = __saveData.isKinematic;
            __instance.constraints = __saveData.constraints;
            __instance.collisionDetectionMode = __saveData.collisionDetectionMode;
            __instance.automaticCenterOfMass = __saveData.automaticCenterOfMass;
            __instance.centerOfMass = __saveData.centerOfMass;
            __instance.automaticInertiaTensor = __saveData.automaticInertiaTensor;
            __instance.inertiaTensorRotation = __saveData.inertiaTensorRotation;
            __instance.inertiaTensor = __saveData.inertiaTensor;
            __instance.detectCollisions = __saveData.detectCollisions;
            __instance.position = __saveData.position;
            __instance.rotation = __saveData.rotation;
            __instance.interpolation = __saveData.interpolation;
            __instance.solverIterations = __saveData.solverIterations;
            __instance.sleepThreshold = __saveData.sleepThreshold;
            __instance.maxAngularVelocity = __saveData.maxAngularVelocity;
            __instance.maxLinearVelocity = __saveData.maxLinearVelocity;
            __instance.solverVelocityIterations = __saveData.solverVelocityIterations;
            __instance.excludeLayers = __saveData.excludeLayers;
            __instance.includeLayers = __saveData.includeLayers;
            __instance.hideFlags = __saveData.hideFlags;
        }
        static RigidbodySaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
                {"SetDensity(mscorlib System.Single):mscorlib System.Void", 448843109521375247},
                {"MovePosition(UnityEngine.CoreModule UnityEngine.Vector3):mscorlib System.Void", 207627375648227905},
                {"MoveRotation(UnityEngine.CoreModule UnityEngine.Quaternion):mscorlib System.Void", 529287607575170039},
                {"Move(UnityEngine.CoreModule UnityEngine.Vector3,UnityEngine.CoreModule UnityEngine.Quaternion):mscorlib System.Void", 371046845130447972},
                {"Sleep():mscorlib System.Void", 882002080957818762},
                {"IsSleeping():mscorlib System.Boolean", 447243156713820724},
                {"WakeUp():mscorlib System.Void", 727906688822177552},
                {"ResetCenterOfMass():mscorlib System.Void", 544884526613618393},
                {"ResetInertiaTensor():mscorlib System.Void", 443165432464627727},
                {"GetRelativePointVelocity(UnityEngine.CoreModule UnityEngine.Vector3):UnityEngine.CoreModule UnityEngine.Vector3", 984338997511377300},
                {"GetPointVelocity(UnityEngine.CoreModule UnityEngine.Vector3):UnityEngine.CoreModule UnityEngine.Vector3", 938935216208698131},
                {"PublishTransform():mscorlib System.Void", 911516781108622689},
                {"GetAccumulatedForce(mscorlib System.Single):UnityEngine.CoreModule UnityEngine.Vector3", 904901179891223649},
                {"GetAccumulatedForce():UnityEngine.CoreModule UnityEngine.Vector3", 931215310757691913},
                {"GetAccumulatedTorque(mscorlib System.Single):UnityEngine.CoreModule UnityEngine.Vector3", 690934689988508274},
                {"GetAccumulatedTorque():UnityEngine.CoreModule UnityEngine.Vector3", 778969141120030004},
                {"AddForce(UnityEngine.CoreModule UnityEngine.Vector3,UnityEngine.PhysicsModule UnityEngine.ForceMode):mscorlib System.Void", 990652632225932614},
                {"AddForce(UnityEngine.CoreModule UnityEngine.Vector3):mscorlib System.Void", 563089346895850437},
                {"AddForce(mscorlib System.Single,mscorlib System.Single,mscorlib System.Single,UnityEngine.PhysicsModule UnityEngine.ForceMode):mscorlib System.Void", 402532791528841553},
                {"AddForce(mscorlib System.Single,mscorlib System.Single,mscorlib System.Single):mscorlib System.Void", 181030674737440420},
                {"AddRelativeForce(UnityEngine.CoreModule UnityEngine.Vector3,UnityEngine.PhysicsModule UnityEngine.ForceMode):mscorlib System.Void", 392039872704320735},
                {"AddRelativeForce(UnityEngine.CoreModule UnityEngine.Vector3):mscorlib System.Void", 627196895317831135},
                {"AddRelativeForce(mscorlib System.Single,mscorlib System.Single,mscorlib System.Single,UnityEngine.PhysicsModule UnityEngine.ForceMode):mscorlib System.Void", 195225886941770589},
                {"AddRelativeForce(mscorlib System.Single,mscorlib System.Single,mscorlib System.Single):mscorlib System.Void", 309623346497328464},
                {"AddTorque(UnityEngine.CoreModule UnityEngine.Vector3,UnityEngine.PhysicsModule UnityEngine.ForceMode):mscorlib System.Void", 930230995363634182},
                {"AddTorque(UnityEngine.CoreModule UnityEngine.Vector3):mscorlib System.Void", 637198949951230655},
                {"AddTorque(mscorlib System.Single,mscorlib System.Single,mscorlib System.Single,UnityEngine.PhysicsModule UnityEngine.ForceMode):mscorlib System.Void", 907962798275214582},
                {"AddTorque(mscorlib System.Single,mscorlib System.Single,mscorlib System.Single):mscorlib System.Void", 774139103412208917},
                {"AddRelativeTorque(UnityEngine.CoreModule UnityEngine.Vector3,UnityEngine.PhysicsModule UnityEngine.ForceMode):mscorlib System.Void", 567449066374765887},
                {"AddRelativeTorque(UnityEngine.CoreModule UnityEngine.Vector3):mscorlib System.Void", 287876938506739601},
                {"AddRelativeTorque(mscorlib System.Single,mscorlib System.Single,mscorlib System.Single,UnityEngine.PhysicsModule UnityEngine.ForceMode):mscorlib System.Void", 633416283964741173},
                {"AddRelativeTorque(mscorlib System.Single,mscorlib System.Single,mscorlib System.Single):mscorlib System.Void", 979041055054406325},
                {"AddForceAtPosition(UnityEngine.CoreModule UnityEngine.Vector3,UnityEngine.CoreModule UnityEngine.Vector3,UnityEngine.PhysicsModule UnityEngine.ForceMode):mscorlib System.Void", 466059306475465673},
                {"AddForceAtPosition(UnityEngine.CoreModule UnityEngine.Vector3,UnityEngine.CoreModule UnityEngine.Vector3):mscorlib System.Void", 348245112955741570},
                {"AddExplosionForce(mscorlib System.Single,UnityEngine.CoreModule UnityEngine.Vector3,mscorlib System.Single,mscorlib System.Single,UnityEngine.PhysicsModule UnityEngine.ForceMode):mscorlib System.Void", 781974468959883317},
                {"AddExplosionForce(mscorlib System.Single,UnityEngine.CoreModule UnityEngine.Vector3,mscorlib System.Single,mscorlib System.Single):mscorlib System.Void", 712093745063398598},
                {"AddExplosionForce(mscorlib System.Single,UnityEngine.CoreModule UnityEngine.Vector3,mscorlib System.Single):mscorlib System.Void", 227377716206243486},
                {"ClosestPointOnBounds(UnityEngine.CoreModule UnityEngine.Vector3):UnityEngine.CoreModule UnityEngine.Vector3", 858723930566038418},
                {"SweepTest(UnityEngine.CoreModule UnityEngine.Vector3,UnityEngine.PhysicsModule UnityEngine.RaycastHit&,mscorlib System.Single,UnityEngine.PhysicsModule UnityEngine.QueryTriggerInteraction):mscorlib System.Boolean", 133538743881990273},
                {"SweepTest(UnityEngine.CoreModule UnityEngine.Vector3,UnityEngine.PhysicsModule UnityEngine.RaycastHit&,mscorlib System.Single):mscorlib System.Boolean", 940444725027782434},
                {"SweepTest(UnityEngine.CoreModule UnityEngine.Vector3,UnityEngine.PhysicsModule UnityEngine.RaycastHit&):mscorlib System.Boolean", 322007914856576547},
                {"SweepTestAll(UnityEngine.CoreModule UnityEngine.Vector3,mscorlib System.Single,UnityEngine.PhysicsModule UnityEngine.QueryTriggerInteraction):UnityEngine.PhysicsModule UnityEngine.RaycastHit[]", 769870372500856504},
                {"SweepTestAll(UnityEngine.CoreModule UnityEngine.Vector3,mscorlib System.Single):UnityEngine.PhysicsModule UnityEngine.RaycastHit[]", 193243987844123577},
                {"SweepTestAll(UnityEngine.CoreModule UnityEngine.Vector3):UnityEngine.PhysicsModule UnityEngine.RaycastHit[]", 534263530490907154}
            };
            Infra.Singleton.__methodIdsByMethodSignaturePerType.Add(_typeReference, methodToId);
            Infra.Singleton.__methodGetterFactoryPerType.Add(_typeReference, _idToMethod);
            Infra.Singleton.__methodInfoGettersPerType.Add(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(UnityEngine.Rigidbody);
        static Type _typeDefinition = typeof(UnityEngine.Rigidbody);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                448843109521375247 => new Func<object, Delegate>((instance) => new Action<System.Single>(((UnityEngine.Rigidbody)instance).SetDensity)),
                207627375648227905 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3>(((UnityEngine.Rigidbody)instance).MovePosition)),
                529287607575170039 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Quaternion>(((UnityEngine.Rigidbody)instance).MoveRotation)),
                371046845130447972 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3, UnityEngine.Quaternion>(((UnityEngine.Rigidbody)instance).Move)),
                882002080957818762 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.Rigidbody)instance).Sleep)),
                447243156713820724 => new Func<object, Delegate>((instance) => new Func<System.Boolean>(((UnityEngine.Rigidbody)instance).IsSleeping)),
                727906688822177552 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.Rigidbody)instance).WakeUp)),
                544884526613618393 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.Rigidbody)instance).ResetCenterOfMass)),
                443165432464627727 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.Rigidbody)instance).ResetInertiaTensor)),
                984338997511377300 => new Func<object, Delegate>((instance) => new Func<UnityEngine.Vector3, UnityEngine.Vector3>(((UnityEngine.Rigidbody)instance).GetRelativePointVelocity)),
                938935216208698131 => new Func<object, Delegate>((instance) => new Func<UnityEngine.Vector3, UnityEngine.Vector3>(((UnityEngine.Rigidbody)instance).GetPointVelocity)),
                911516781108622689 => new Func<object, Delegate>((instance) => new Action(((UnityEngine.Rigidbody)instance).PublishTransform)),
                904901179891223649 => new Func<object, Delegate>((instance) => new Func<System.Single, UnityEngine.Vector3>(((UnityEngine.Rigidbody)instance).GetAccumulatedForce)),
                931215310757691913 => new Func<object, Delegate>((instance) => new Func<UnityEngine.Vector3>(((UnityEngine.Rigidbody)instance).GetAccumulatedForce)),
                690934689988508274 => new Func<object, Delegate>((instance) => new Func<System.Single, UnityEngine.Vector3>(((UnityEngine.Rigidbody)instance).GetAccumulatedTorque)),
                778969141120030004 => new Func<object, Delegate>((instance) => new Func<UnityEngine.Vector3>(((UnityEngine.Rigidbody)instance).GetAccumulatedTorque)),
                990652632225932614 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3, UnityEngine.ForceMode>(((UnityEngine.Rigidbody)instance).AddForce)),
                563089346895850437 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3>(((UnityEngine.Rigidbody)instance).AddForce)),
                402532791528841553 => new Func<object, Delegate>((instance) => new Action<System.Single, System.Single, System.Single, UnityEngine.ForceMode>(((UnityEngine.Rigidbody)instance).AddForce)),
                181030674737440420 => new Func<object, Delegate>((instance) => new Action<System.Single, System.Single, System.Single>(((UnityEngine.Rigidbody)instance).AddForce)),
                392039872704320735 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3, UnityEngine.ForceMode>(((UnityEngine.Rigidbody)instance).AddRelativeForce)),
                627196895317831135 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3>(((UnityEngine.Rigidbody)instance).AddRelativeForce)),
                195225886941770589 => new Func<object, Delegate>((instance) => new Action<System.Single, System.Single, System.Single, UnityEngine.ForceMode>(((UnityEngine.Rigidbody)instance).AddRelativeForce)),
                309623346497328464 => new Func<object, Delegate>((instance) => new Action<System.Single, System.Single, System.Single>(((UnityEngine.Rigidbody)instance).AddRelativeForce)),
                930230995363634182 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3, UnityEngine.ForceMode>(((UnityEngine.Rigidbody)instance).AddTorque)),
                637198949951230655 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3>(((UnityEngine.Rigidbody)instance).AddTorque)),
                907962798275214582 => new Func<object, Delegate>((instance) => new Action<System.Single, System.Single, System.Single, UnityEngine.ForceMode>(((UnityEngine.Rigidbody)instance).AddTorque)),
                774139103412208917 => new Func<object, Delegate>((instance) => new Action<System.Single, System.Single, System.Single>(((UnityEngine.Rigidbody)instance).AddTorque)),
                567449066374765887 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3, UnityEngine.ForceMode>(((UnityEngine.Rigidbody)instance).AddRelativeTorque)),
                287876938506739601 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3>(((UnityEngine.Rigidbody)instance).AddRelativeTorque)),
                633416283964741173 => new Func<object, Delegate>((instance) => new Action<System.Single, System.Single, System.Single, UnityEngine.ForceMode>(((UnityEngine.Rigidbody)instance).AddRelativeTorque)),
                979041055054406325 => new Func<object, Delegate>((instance) => new Action<System.Single, System.Single, System.Single>(((UnityEngine.Rigidbody)instance).AddRelativeTorque)),
                466059306475465673 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3, UnityEngine.Vector3, UnityEngine.ForceMode>(((UnityEngine.Rigidbody)instance).AddForceAtPosition)),
                348245112955741570 => new Func<object, Delegate>((instance) => new Action<UnityEngine.Vector3, UnityEngine.Vector3>(((UnityEngine.Rigidbody)instance).AddForceAtPosition)),
                781974468959883317 => new Func<object, Delegate>((instance) => new Action<System.Single, UnityEngine.Vector3, System.Single, System.Single, UnityEngine.ForceMode>(((UnityEngine.Rigidbody)instance).AddExplosionForce)),
                712093745063398598 => new Func<object, Delegate>((instance) => new Action<System.Single, UnityEngine.Vector3, System.Single, System.Single>(((UnityEngine.Rigidbody)instance).AddExplosionForce)),
                227377716206243486 => new Func<object, Delegate>((instance) => new Action<System.Single, UnityEngine.Vector3, System.Single>(((UnityEngine.Rigidbody)instance).AddExplosionForce)),
                858723930566038418 => new Func<object, Delegate>((instance) => new Func<UnityEngine.Vector3, UnityEngine.Vector3>(((UnityEngine.Rigidbody)instance).ClosestPointOnBounds)),
                769870372500856504 => new Func<object, Delegate>((instance) => new Func<UnityEngine.Vector3, System.Single, UnityEngine.QueryTriggerInteraction, UnityEngine.RaycastHit[]>(((UnityEngine.Rigidbody)instance).SweepTestAll)),
                193243987844123577 => new Func<object, Delegate>((instance) => new Func<UnityEngine.Vector3, System.Single, UnityEngine.RaycastHit[]>(((UnityEngine.Rigidbody)instance).SweepTestAll)),
                534263530490907154 => new Func<object, Delegate>((instance) => new Func<UnityEngine.Vector3, UnityEngine.RaycastHit[]>(((UnityEngine.Rigidbody)instance).SweepTestAll)),
                _ => Infra.Singleton.GetIdToMethodMapForType(_typeReference.BaseType)(id),
            };
            return method;
        }
        public static MethodInfo _idToMethodInfo(long id)
        {
            MethodInfo methodDef = id switch
            {
                133538743881990273 => typeof(UnityEngine.Rigidbody).GetMethod(nameof(UnityEngine.Rigidbody.SweepTest), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Vector3), typeof(UnityEngine.RaycastHit).MakeByRefType(), typeof(System.Single), typeof(UnityEngine.QueryTriggerInteraction) }, null),
                940444725027782434 => typeof(UnityEngine.Rigidbody).GetMethod(nameof(UnityEngine.Rigidbody.SweepTest), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Vector3), typeof(UnityEngine.RaycastHit).MakeByRefType(), typeof(System.Single) }, null),
                322007914856576547 => typeof(UnityEngine.Rigidbody).GetMethod(nameof(UnityEngine.Rigidbody.SweepTest), BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(UnityEngine.Vector3), typeof(UnityEngine.RaycastHit).MakeByRefType() }, null),
                _ => Infra.Singleton.GetMethodInfoIdToMethodMapForType(_typeReference.BaseType)(id),
            };
            return methodDef;
        }
    }
    public class RigidbodySaveData : MonoSaveDataBase
    {
        public UnityEngine.Vector3 linearVelocity;
        public UnityEngine.Vector3 angularVelocity;
        public System.Single linearDamping;
        public System.Single angularDamping;
        public System.Single mass;
        public System.Boolean useGravity;
        public System.Single maxDepenetrationVelocity;
        public System.Boolean isKinematic;
        public UnityEngine.RigidbodyConstraints constraints;
        public UnityEngine.CollisionDetectionMode collisionDetectionMode;
        public System.Boolean automaticCenterOfMass;
        public UnityEngine.Vector3 centerOfMass;
        public System.Boolean automaticInertiaTensor;
        public UnityEngine.Quaternion inertiaTensorRotation;
        public UnityEngine.Vector3 inertiaTensor;
        public System.Boolean detectCollisions;
        public UnityEngine.Vector3 position;
        public UnityEngine.Quaternion rotation;
        public UnityEngine.RigidbodyInterpolation interpolation;
        public System.Int32 solverIterations;
        public System.Single sleepThreshold;
        public System.Single maxAngularVelocity;
        public System.Single maxLinearVelocity;
        public System.Int32 solverVelocityIterations;
        public UnityEngine.LayerMask excludeLayers;
        public UnityEngine.LayerMask includeLayers;
        public UnityEngine.HideFlags hideFlags;
    }

    //[SaveHandler(34234236665310,nameof(Rigidbody),typeof(Rigidbody))]
    //public class RigidbodySaveHandler:MonoSaveHandler<Rigidbody,RigidBodySaveData>
    //{
    //    public RigidbodySaveHandler()
    //    {

    //    }

    //    public override void WriteSaveData()
    //    {
    //        base.WriteSaveData();

    //        __saveData.Mass = __instance.mass;
    //        __saveData.LinearDamping = __instance.linearDamping;
    //        __saveData.AngularDamping = __instance.angularDamping;
    //        __saveData.AutomaticCenterOfMass = __instance.automaticCenterOfMass;
    //        __saveData.AutomaticTensor = __instance.automaticInertiaTensor;
    //        __saveData.UseGravity = __instance.useGravity;
    //        __saveData.IsKinematic = __instance.isKinematic;
    //        __saveData.Interpolate = (int)__instance.interpolation;
    //        __saveData.CollisionDetection = (int)__instance.collisionDetectionMode;
    //        __saveData.IncludeLayers = __instance.includeLayers;
    //        __saveData.ExcludeLayers = __instance.excludeLayers;
    //        __saveData.Constraints = (int)__instance.constraints;

    //    }


    //    public override void LoadValues()
    //    {
    //        base.LoadValues();

    //        __instance.mass = __saveData.Mass;
    //        __instance.linearDamping = __saveData.LinearDamping;
    //        __instance.angularDamping = __saveData.AngularDamping;
    //        __instance.automaticCenterOfMass = __saveData.AutomaticCenterOfMass;
    //        __instance.automaticInertiaTensor = __saveData.AutomaticTensor;
    //        __instance.useGravity = __saveData.UseGravity;
    //        __instance.isKinematic = __saveData.IsKinematic;
    //        __instance.interpolation = (RigidbodyInterpolation)__saveData.Interpolate;
    //        __instance.collisionDetectionMode = (CollisionDetectionMode)__saveData.CollisionDetection;
    //        __instance.includeLayers = __saveData.IncludeLayers;
    //        __instance.excludeLayers = __saveData.ExcludeLayers;
    //        __instance.constraints = (RigidbodyConstraints)__saveData.Constraints;
    //    }
    //}

    //public class RigidBodySaveData: MonoSaveDataBase
    //{
    //    public float Mass;
    //    public float LinearDamping;
    //    public float AngularDamping;
    //    public bool AutomaticCenterOfMass;
    //    public bool AutomaticTensor;
    //    public bool UseGravity;
    //    public bool IsKinematic;
    //    public int Interpolate;
    //    public int CollisionDetection;
    //    public int IncludeLayers;
    //    public int ExcludeLayers;
    //    public int Constraints;
    //}
}
