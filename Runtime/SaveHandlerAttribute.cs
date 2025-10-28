
using System;
using UnityEngine;

namespace Assets._Project.Scripts.SaveAndLoad
{
    public enum SaveHandlerGenerationMode
    {
        Manual,
        Mixed,
        FullAutomata,
    }

    public class SaveHandlerAttribute: System.Attribute
    {
        //These param names hardcoded in the codegen logic
        public SaveHandlerAttribute(long id, string dataGroupName, Type handledType, bool isStatic = false, int arrayDimension = 0
            , SaveHandlerGenerationMode generationMode = SaveHandlerGenerationMode.Manual
            , int order = 0 /*higher comes later, can be negative too*/
            , Type[] dependsOn = null,Type staticHandlerOf = null
            )
        {
            Id = id;
            DataGroupName = dataGroupName;
            HandledType = handledType;
            _IsStatic = isStatic;
            ArrayDimension = arrayDimension;
            GenerationMode = generationMode;
            Order = order;
            LoadDependencies = dependsOn;
            StaticHandlerOf = staticHandlerOf;
        }
        public long Id { get; private set; }
        public string DataGroupName { get;private set; }
        public Type HandledType { get; private set; }
        public bool _IsStatic;
        public bool IsStatic => _IsStatic || StaticHandlerOf != null;
        public int ArrayDimension { get; private set; }
        public SaveHandlerGenerationMode GenerationMode { get; }
        public int Order { get; }
        public Type[] LoadDependencies { get; }
        public Type StaticHandlerOf { get; }
        public bool RequiresManualAttributeCreation { get; init; }
    }
}
