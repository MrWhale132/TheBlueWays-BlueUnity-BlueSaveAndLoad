using Assets._Project.Scripts.UtilScripts;
using UnityEngine.Scripting;

namespace Assets._Project.Scripts.SaveAndLoad
{
    [Preserve]
    //these names must be unique because other types inherit from this and they can have members with same name which would hide these members
    public class SaveDataBase
    {
        public ObjectMetaData _MetaData_;
        public RandomId _ObjectId_;
        public bool _isRootObject_;
    }
}
