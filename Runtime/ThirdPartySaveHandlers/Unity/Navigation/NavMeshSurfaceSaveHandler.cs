using Unity.AI.Navigation;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine.AI;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity.Navigation
{
    [SaveHandler(520828485848231821, "NavMeshSurface", typeof(NavMeshSurface),order:-7)]
    public class NavMeshSurfaceSaveHandler : MonoSaveHandler<NavMeshSurface, NavMeshSurfaceSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.agentTypeID = __instance.agentTypeID;
            __saveData.collectObjects = __instance.collectObjects;
            __saveData.size = __instance.size;
            __saveData.center = __instance.center;
            __saveData.layerMask = __instance.layerMask;
            __saveData.useGeometry = __instance.useGeometry;
            __saveData.defaultArea = __instance.defaultArea;
            __saveData.ignoreNavMeshAgent = __instance.ignoreNavMeshAgent;
            __saveData.ignoreNavMeshObstacle = __instance.ignoreNavMeshObstacle;
            __saveData.overrideTileSize = __instance.overrideTileSize;
            __saveData.tileSize = __instance.tileSize;
            __saveData.overrideVoxelSize = __instance.overrideVoxelSize;
            __saveData.voxelSize = __instance.voxelSize;
            __saveData.minRegionArea = __instance.minRegionArea;
            __saveData.buildHeightMesh = __instance.buildHeightMesh;
            __saveData.navMeshData = GetObjectId(__instance.navMeshData);
        }

        public override void CreateObject()
        {
            base.CreateObject();

            __instance.agentTypeID = __saveData.agentTypeID;
            __instance.collectObjects = __saveData.collectObjects;
            __instance.size = __saveData.size;
            __instance.center = __saveData.center;
            __instance.layerMask = __saveData.layerMask;
            __instance.useGeometry = __saveData.useGeometry;
            __instance.defaultArea = __saveData.defaultArea;
            __instance.ignoreNavMeshAgent = __saveData.ignoreNavMeshAgent;
            __instance.ignoreNavMeshObstacle = __saveData.ignoreNavMeshObstacle;
            __instance.overrideTileSize = __saveData.overrideTileSize;
            __instance.tileSize = __saveData.tileSize;
            __instance.overrideVoxelSize = __saveData.overrideVoxelSize;
            __instance.voxelSize = __saveData.voxelSize;
            __instance.minRegionArea = __saveData.minRegionArea;
            __instance.buildHeightMesh = __saveData.buildHeightMesh;
            
            __instance.BuildNavMesh();

            Infra.Singleton.RegisterReference(__instance.navMeshData, __saveData.navMeshData);
        }
        static NavMeshSurfaceSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
                {"AddData():mscorlib System.Void", 867467563436705950},
                {"RemoveData():mscorlib System.Void", 513541784237073035},
                {"GetBuildSettings():UnityEngine.AIModule UnityEngine.AI.NavMeshBuildSettings", 106236495914237130},
                {"BuildNavMesh():mscorlib System.Void", 821577322546266095},
                {"UpdateNavMesh(UnityEngine.AIModule UnityEngine.AI.NavMeshData):UnityEngine.CoreModule UnityEngine.AsyncOperation", 427705677090094320}
            };
            Infra.Singleton.__methodIdsByMethodSignaturePerType.Add(_typeReference, methodToId);
            Infra.Singleton.__methodGetterFactoryPerType.Add(_typeReference, _idToMethod);
            Infra.Singleton.__methodInfoGettersPerType.Add(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(NavMeshSurface);
        static Type _typeDefinition = typeof(NavMeshSurface);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                867467563436705950 => new Func<object, Delegate>((instance) => new Action(((NavMeshSurface)instance).AddData)),
                513541784237073035 => new Func<object, Delegate>((instance) => new Action(((NavMeshSurface)instance).RemoveData)),
                106236495914237130 => new Func<object, Delegate>((instance) => new Func<UnityEngine.AI.NavMeshBuildSettings>(((NavMeshSurface)instance).GetBuildSettings)),
                821577322546266095 => new Func<object, Delegate>((instance) => new Action(((NavMeshSurface)instance).BuildNavMesh)),
                427705677090094320 => new Func<object, Delegate>((instance) => new Func<UnityEngine.AI.NavMeshData, UnityEngine.AsyncOperation>(((NavMeshSurface)instance).UpdateNavMesh)),
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
    public class NavMeshSurfaceSaveData : MonoSaveDataBase
    {
        public System.Int32 agentTypeID;
        public CollectObjects collectObjects;
        public UnityEngine.Vector3 size;
        public UnityEngine.Vector3 center;
        public UnityEngine.LayerMask layerMask;
        public UnityEngine.AI.NavMeshCollectGeometry useGeometry;
        public System.Int32 defaultArea;
        public System.Boolean ignoreNavMeshAgent;
        public System.Boolean ignoreNavMeshObstacle;
        public System.Boolean overrideTileSize;
        public System.Int32 tileSize;
        public System.Boolean overrideVoxelSize;
        public System.Single voxelSize;
        public System.Single minRegionArea;
        public System.Boolean buildHeightMesh;
        public RandomId navMeshData;
    }
}