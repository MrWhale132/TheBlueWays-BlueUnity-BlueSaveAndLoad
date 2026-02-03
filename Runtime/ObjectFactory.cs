
using Assets._Project.Scripts.UtilScripts.CodeGen;
using System;
using System.Reflection;
using UnityEngine;

namespace Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime
{
    public class ObjectFactory
    {
        public static object CreateInstance(Type type)
        {
            //var ctor = type.GetConstructor(System.Type.EmptyTypes);

            //if (ctor == null)
            //{
            //    Debug.LogError($"{nameof(ObjectFactory)} cant create instance of type {type.CleanAssemblyQualifiedName()}, " +
            //        $"it does not have a parameterless constructor.");
            //    return default;
            //}

            //var instance = ctor.Invoke(null);
            //return instance;

            try
            {
                var instance = Activator.CreateInstance(type, nonPublic: true);
                return instance;
            }
            catch (Exception e)
            {
                Debug.LogError($"{nameof(ObjectFactory)} cant create instance of type {type.CleanAssemblyQualifiedName()}.\n" +
                    $"Exception: {e}");
                throw;
            }
        }

        public static T CreateInstance<T>(Type type)
        {
            var instance = CreateInstance(type);
            return (T)instance;
        }
        public static T CreateInstance<T>()
        {
            return CreateInstance<T>(typeof(T));
        }






        public static object CreateUsingFirstConstructor(Type type)
        {
            if (type.IsAbstract)
                throw new InvalidOperationException($"Cannot instantiate abstract type {type}");

            var ctors = type.GetConstructors(
                BindingFlags.Public | BindingFlags.Instance);

            if (ctors.Length == 0)
                throw new InvalidOperationException($"Type {type} has no public constructors");

            var ctor = ctors[0]; // "first" ctor
            var args = CreateDefaultArguments(ctor);

            return ctor.Invoke(args);
        }



        public static object[] CreateDefaultArguments(ConstructorInfo ctor)
        {
            var parameters = ctor.GetParameters();
            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                args[i] = GetDefaultValue(p);
            }

            return args;
        }


        static object GetDefaultValue(ParameterInfo p)
        {
            if (p.HasDefaultValue)
                return p.DefaultValue;

            var type = p.ParameterType;

            if (!type.IsValueType || Nullable.GetUnderlyingType(type) != null)
                return null;

            return Activator.CreateInstance(type);
        }
    }
}
