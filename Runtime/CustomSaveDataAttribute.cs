
using System;

namespace Assets._Project.Scripts.SaveAndLoad
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class CustomSaveDataAttribute : System.Attribute
    {
        public CustomSaveDataAttribute(long id)
        {
            Id = id;
        }

        public CustomSaveDataAttribute(long id,int version, int appVersion, Type handledType): this(id)
        {
            IsPastVersion = true;
            Version = version;
            AppVersion = appVersion;
            HandledType = handledType;
        }

        public long Id { get; }
        public Type SaveHandlerType { get; private set; }
        public void SetHandlerType(Type type)
        {
            SaveHandlerType = type;
        }
        public Type HandledType { get; set; }
        public SaveHandlerGenerationMode GenerationMode { get; init; } = SaveHandlerGenerationMode.Manual;
        public bool IsPastVersion { get;private set; }

        public int Version { get; }
        public int AppVersion { get; }
    }
}
