using UnityEngine.AI;
using Assets._Project.Scripts.UtilScripts;
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using Assets._Project.Scripts.SaveAndLoad.SavableDelegates;
using System.Collections.Generic;
using System;
using System.Reflection;
using Unity.AI.Navigation;
namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity.Navigation
{
    [SaveHandler(748707611466571488, "NavMeshData", typeof(UnityEngine.AI.NavMeshData), order:-6, dependsOn: new[] {typeof(NavMeshSurface)})]
    public class NavMeshDataSaveHandler : UnmanagedSaveHandler<NavMeshData, NavMeshDataSaveData>
    {
        public override void WriteSaveData()
        {
            base.WriteSaveData();
            __saveData.position = __instance.position;
            __saveData.rotation = __instance.rotation;
        }

        public override void CreateObject()
        {
            HandledObjectId = __saveData._ObjectId_;

            __instance = GetObjectById<NavMeshData>(__saveData._ObjectId_);
        }

        static NavMeshDataSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
            };
            Infra.Singleton.__methodIdsByMethodSignaturePerType.Add(_typeReference, methodToId);
            Infra.Singleton.__methodGetterFactoryPerType.Add(_typeReference, _idToMethod);
            Infra.Singleton.__methodInfoGettersPerType.Add(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(NavMeshData);
        static Type _typeDefinition = typeof(UnityEngine.AI.NavMeshData);
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
    public class NavMeshDataSaveData : SaveDataBase
    {
        public UnityEngine.Vector3 position;
        public UnityEngine.Quaternion rotation;
    }
}