
using System;

namespace Assets._Project.Scripts.SaveAndLoad
{
    public class CustomSaveDataAttribute:System.Attribute
    {
        public Type SaveHandlerType { get; set; }
        public Type HandledType { get; set; }
        public SaveHandlerGenerationMode GenerationMode { get; init; } = SaveHandlerGenerationMode.Manual;
    }
}
