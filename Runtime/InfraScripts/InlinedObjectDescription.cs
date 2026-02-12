
using Assets._Project.Scripts.UtilScripts;
using System;
using System.Collections.Generic;
using Thebluway.Core;
using Object = UnityEngine.Object;

namespace Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.InfraScripts
{
    [Serializable]
    public class InlinedObjectDescription
    {
#if UNITY_EDITOR
        public bool _collectSelfAndChildGameObjectsAndComponents;
#endif

        public MemberExclusionSettings memberExclusionSettings;

        public List<ObjectMember> members;

        [Serializable]
        public class ObjectMember
        {
            public Object member;
            [AutoGenerate, AllowEdit(RandomIdEditMode = RandomIdEditMode.Generate)]
            public RandomId memberId;
        }
    }

    [Serializable]
    public class MemberExclusionSettings
    {
        public Object memberToExclude;

        [ReadOnly]
        public List<Object> excludedMembers;
    }
}
