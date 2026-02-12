
using Assets._Project.Scripts.UtilScripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.InfraScripts
{
    [CreateAssetMenu(fileName = "ObjectDescriptor", menuName = "Scriptable Objects/SaveAndLoad/ObjectDescriptor")]
    public class ObjectDescriptor : ScriptableObject
    {
        public bool _ensureMemberIdsAreUnique;
        public bool _generateNewMemberIds;
        public List<ObjectMemberSettings> membersSettings;


        private void OnValidate()
        {
            if (_ensureMemberIdsAreUnique)
            {
                _ensureMemberIdsAreUnique = false;
                EnsureMemberIdsAreUnique();
            }
            if (_generateNewMemberIds)
            {
                _generateNewMemberIds = false;
                GenerateNewMemberIds();
            }
            if (membersSettings != null)
            {
                void Traverse(ObjectMemberSettings member)
                {
                    if (member.memberIndexV2 is GameObjectMembers.ChildrenRecursive or GameObjectMembers.AllChildrenAndComponentsRecursive)
                    {
                        var isNullOrEmpty = member.arrayElementMembers == null || member.arrayElementMembers.Count == 0;

                        if (!isNullOrEmpty)
                        {
                            Debug.LogWarning($"Recursive members do not support having a specific set of array element members. Clearing the array element members for memberId: {member.memberId}");
                            member.arrayElementMembers?.Clear();
                        }
                    }

                    if (member.IsArrayMember)
                    {
                        foreach (var arrayElementMember in member.arrayElementMembers)
                        {
                            Traverse(arrayElementMember);
                        }
                    }
                }

                foreach (var settings in membersSettings)
                {
                    Traverse(settings);
                }
            }
        }

        public void GenerateNewMemberIds()
        {
            if (membersSettings != null)
            {
                foreach (var settings in membersSettings)
                {
                    settings.memberId = RandomId.New;

                    if (settings.IsArrayMember)
                    {
                        foreach (var arrayElementMember in settings.arrayElementMembers)
                        {
                            arrayElementMember.memberId = RandomId.New;
                        }
                    }
                }
            }
        }

        private void EnsureMemberIdsAreUnique()
        {
            if (membersSettings != null)
            {
                var ids = new HashSet<RandomId>();

                foreach (var settings in membersSettings)
                {
                    if (settings.memberId.IsDefault || ids.Contains(settings.memberId))
                    {
                        settings.memberId = RandomId.New;
                    }

                    ids.Add(settings.memberId);


                    if (settings.IsArrayMember)
                    {
                        foreach (var arrayElementMember in settings.arrayElementMembers)
                        {
                            if (arrayElementMember.memberId.IsDefault || ids.Contains(arrayElementMember.memberId))
                            {
                                arrayElementMember.memberId = RandomId.New;
                            }
                            ids.Add(arrayElementMember.memberId);
                        }
                    }
                }
            }
        }
    }

    [Serializable]
    public class ObjectMemberSettings
    {
        public RandomId memberId;
        public int memberIndex => (int)memberIndexV2;
        public GameObjectMembers memberIndexV2;
        public int arrayIndex;
        public bool allArrayElementMembers;
        public ObjectDescriptor descriptor;
        public List<ObjectMemberSettings> arrayElementMembers;

        public bool IsArrayMember => arrayElementMembers != null && arrayElementMembers.Count > 0;
        public bool ShouldVisitMember => descriptor != null;
    }


    public enum GameObjectMembers
    {
        ArrayElement,
        GameObject,
        Components,
        ImmediateChildren,
        ChildrenRecursive,
        AllChildrenAndComponentsRecursive,
    }
}

