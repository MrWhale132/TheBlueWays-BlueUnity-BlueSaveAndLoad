
using System;

namespace Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class MigrationsAttribute:Attribute
    {
        public MigrationsAttribute(long typeId)
        {
            SaveHandlerId = typeId;
        }
        public long SaveHandlerId { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class MigrationAttribute : Attribute
    {
        public MigrationAttribute(int dataVersion, int appVersion, Type dataType)
        {
            DataVersion = dataVersion;
            AppVersion = appVersion;
            DataType = dataType;
        }
        public int DataVersion { get; private set; }
        public int AppVersion { get; private set; }
        public Type DataType { get; private set; }
    }
}
