
using System;
using System.Reflection;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Dotnet
{
    public class ObjectSaveHandler
    {
        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                _ => null,
            };
            return method;
        }


        static Type _type = typeof(System.Object);
        static Type[] _args = _type.IsGenericType ? _type.GetGenericArguments() : null;


        public static MethodInfo _idToGenMethodDef(long id)
        {
            MethodInfo methodDef = id switch
            {
                _ => null,
            };

            return methodDef;
        }
    }
}
