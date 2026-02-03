
using System;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad
{
    public enum SaveHandlerGenerationMode
    {
        Manual,
        Configured,
        FullAutomata,
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SaveHandlerAttribute : System.Attribute
    {
        //These param names are hardcoded in the codegen logic
        public SaveHandlerAttribute(long id, string dataGroupName="obsolete", Type handledType = null, int arrayDimension = 0
            , SaveHandlerGenerationMode generationMode = SaveHandlerGenerationMode.Manual
            , int order = 0 /*higher comes later, can be negative too*/
            , Type[] dependsOn = null, Type staticHandlerOf = null,bool singleton = false
            )
        {
            if (handledType != null)
            {
                if (handledType.IsGenericType && !handledType.IsGenericTypeDefinition)
                    throw new ArgumentException($"SaveHandlerAttribute: handledType {handledType.FullName} must be an open generic type definition if it is generic.");
            }

            Id = id;
            HandledType = handledType;
            StaticHandlerOf = staticHandlerOf;
            //_IsStatic = isStatic;
            ArrayDimension = arrayDimension;
            GenerationMode = generationMode;
            Order = order;
            LoadDependencies = dependsOn;
            IsSingleton = singleton;
        }
        public long Id { get; private set; }
        public Type HandledType { get; private set; }
        public void SetHandledType(Type type)
        {
            HandledType = type;
        }
        public Type HandlerType { get; set; }
        public bool _IsStatic;
        public bool IsStatic => _IsStatic || StaticHandlerOf != null;
        public bool IsGeneric => HandledType.IsGenericTypeDefinition;
        public int ArrayDimension { get; private set; }
        public SaveHandlerGenerationMode GenerationMode { get; }
        public int Order { get; }
        public Type[] LoadDependencies { get; }
        public Type StaticHandlerOf { get; }
        public bool IsSingleton { get; set; }
        public bool RequiresManualAttributeCreation { get; set; }
    }
}
