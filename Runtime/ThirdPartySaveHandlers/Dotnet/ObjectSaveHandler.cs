
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.SaveAndLoad;
using Assets._Project.Scripts.SaveAndLoad.SaveHandlerBases;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.ThirdPartySaveHandlers.Dotnet
{
    [SaveHandler(98234000982345, nameof(Object), typeof(Object))]
    public class ObjectSaveHandler: UnmanagedSaveHandler<Object, ObjectSaveData>
    {
        static ObjectSaveHandler()
        {
            var methodToId = new Dictionary<string, long>();

            Infra.Singleton.AddMethodSignatureToMethodIdMap(_typeReference, methodToId);
            Infra.Singleton.AddMethodIdToMethodMap(_typeReference, _idToMethod);
            Infra.Singleton.AddMethodIdToMethodInfoMap(_typeReference, _idToMethodInfo);
        }

        static Type _typeReference = typeof(Object);
        static Type _typeDefinition = typeof(Object);
        static Type[] _args = _typeReference.IsGenericType ? _typeReference.GetGenericArguments() : null;
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                _ => null,
            };
            return method;
        }
        public static MethodInfo _idToMethodInfo(long id)
        {
            MethodInfo methodDef = id switch
            {
                _ => null,
            };
            return methodDef;
        }
    }

    public class ObjectSaveData : SaveDataBase
    {
    }
}
