
using System;

namespace Assets._Project.Scripts.SaveAndLoad
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class SaveDataAttribute:System.Attribute
    {
        public SaveDataAttribute(long saveHandlerId)
        {
            SaveHandlerId = saveHandlerId;
        }

        public long SaveHandlerId {  get; private set; }
    }
}
