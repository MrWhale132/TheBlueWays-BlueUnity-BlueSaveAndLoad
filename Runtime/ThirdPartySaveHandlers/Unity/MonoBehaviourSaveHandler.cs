using System;
using System.Reflection;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad.ThirdPartySaveHandlers.Unity
{
    public class MonoBehaviourSaveHandler// : MonoSaveHandler<MonoBehaviour, MonoBehaviourSaveData>
    {

        public static Func<object, Delegate> _idToMethod(long id)
        {
            Func<object, Delegate> method = id switch
            {
                345466577698 => new Func<object, Delegate>((instance) => new Func<string>(instance.ToString)),
                987856746535 => new Func<object, Delegate>((instance) => new Func<int>(instance.GetHashCode)),
                768745674645 => new Func<object, Delegate>((instance) => new Func<Type>(instance.GetType)),
                232536765856 => new Func<object, Delegate>((instance) => new Func<object, bool>(instance.Equals)),
                _ => null,
            };
            return method;
        }


        static Type _type = typeof(MonoBehaviour);
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


    public class MonoBehaviourSaveData :MonoSaveDataBase
    {

    }
}
