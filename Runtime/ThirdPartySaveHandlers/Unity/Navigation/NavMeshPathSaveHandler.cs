
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using System.Collections.Generic;
using System;
using UnityEngine.AI;
using System.Reflection;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.UnitySHs.Navigation
{
    //[SaveHandler(748707611468881488, "NavMeshPath", typeof(UnityEngine.AI.NavMeshPath), order: -4, dependsOn: new[] { typeof(NavMeshAgent) })]
    public class NavMeshPathSaveHandler : UnmanagedSaveHandler<NavMeshPath, NavMeshPathSaveData>
    {
        public override void _AssignInstance()
        {
            __instance = GetObjectById<NavMeshPath>(__saveData._ObjectId_);
        }

        static NavMeshPathSaveHandler()
        {
            Dictionary<string, long> methodToId = new()
            {
            };
            Infra.Singleton.__methodIdsByMethodSignaturePerType.Add(_typeReference, methodToId);
            Infra.Singleton.__methodGetterFactoryPerType.Add(_typeReference, _idToMethod);
            Infra.Singleton.__methodInfoGettersPerType.Add(_typeReference, _idToMethodInfo);
        }
        static Type _typeReference = typeof(NavMeshPath);
        static Type _typeDefinition = typeof(UnityEngine.AI.NavMeshPath);
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

    public class NavMeshPathSaveData : SaveDataBase
    {
    }
}
