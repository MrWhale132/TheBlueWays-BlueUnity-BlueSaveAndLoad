using Assets._Project.Scripts.UtilScripts;

namespace Assets._Project.Scripts.SaveAndLoad
{
    //these names must be unique because other types inherit from this and they can have members with same name which would hide these members
    public class SaveDataBase
    {
        public ObjectMetaData _MetaData_;
        public RandomId _ObjectId_;
        public string _AssemblyQualifiedName_;
        public string _DataGroupId_;
        public bool _isRootObject_;
    }
}
