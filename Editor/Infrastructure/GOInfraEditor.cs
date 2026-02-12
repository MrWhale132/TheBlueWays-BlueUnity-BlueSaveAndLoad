using Assets._Project.Scripts.UtilScripts;
using System.Collections.Generic;
using System.Linq;
using Theblueway.SaveAndLoad.Packages.com.theblueway.saveandload.Runtime.InfraScripts;
using UnityEditor;
using UnityEngine;
using static PlasticGui.WorkspaceWindow.Merge.MergeInProgress;

namespace Assets._Project.Scripts.Infrastructure
{
    [CustomEditor(typeof(GOInfra))]
    public class GOInfraEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GOInfra infra = (GOInfra)target;

            bool didChange = false;


            bool CheckInlinedDescription(InlinedObjectDescription description)
            {
                if (description._collectSelfAndChildGameObjectsAndComponents)
                {
                    description._collectSelfAndChildGameObjectsAndComponents = false;

                    PopulateInlinedDescription(infra, description);

                    Debug.Log("Successfully collected members.");

                    return true;
                }
                else if (description.memberExclusionSettings?.memberToExclude != null)
                {
                    ApplyMemberExclusion(infra, description);

                    return true;
                }
                else return false;
            }


            if (infra.HasInlinedPrefabParts)
            {
                var description = infra.InlinedPrefabDescription;

                didChange = CheckInlinedDescription(description);
            }

            if (infra.HasInlinedSceneParts)
            {
                var description = infra.InlinedScenePlacedDescription;

                didChange = CheckInlinedDescription(description);
            }




            if (GUILayout.Button("Refresh Asset References"))
            {
                infra.RefreshReferencedAssets();
                didChange = true;
            }

            //else if (GUILayout.Button("Add infra to all child"))
            //{
            //    infra.AddInfraToAllChildren();
            //    didChange = true;
            //}

            else if (GUILayout.Button("RemoveInfraFromAllChildren"))
            {
                infra.RemoveInfraFromAllChildren();
                didChange = true;
            }

            //note: feature is not fully developed
            //else if (GUILayout.Button("Cache components in children and self"))
            //{
            //    myTarget.CacheComponentsInChildrenAndSelf();
            //}

            if (didChange)
            {
                EditorUtility.SetDirty(infra); // Mark component as dirty
                EditorUtility.SetDirty(infra.gameObject); // Mark component as dirty
                PrefabUtility.RecordPrefabInstancePropertyModifications(infra); // For prefab instance
            }
        }



        public void PopulateInlinedDescription(GOInfra infra, InlinedObjectDescription description)
        {
            HashSet<Object> ignoredMembers = description.memberExclusionSettings.excludedMembers.ToHashSet();


            description.members.Clear();

            var components = new List<Component>();

            void Traverse(Transform parent)
            {
                void AddMember(Object member)
                {
                    if (ignoredMembers.Contains(member))
                    {
                        return;
                    }

                    var objectMember = new InlinedObjectDescription.ObjectMember()
                    {
                        member = member,
                        memberId = RandomId.New,
                    };

                    description.members.Add(objectMember);
                }

                AddMember(parent.gameObject);

                components.Clear();
                parent.GetComponents<Component>(components);

                foreach (var component in components) AddMember(component);

                for (int i = 0; i < parent.childCount; i++) Traverse(parent.GetChild(i));
            }

            Traverse(infra.transform);


        }




        public void ApplyMemberExclusion(GOInfra infra, InlinedObjectDescription description)
        {

            HashSet<Object> alreadyExcludedMembers = description.memberExclusionSettings.excludedMembers.ToHashSet();


            var memberToExclude = description.memberExclusionSettings.memberToExclude;

            List<Object> relatedMembers = new()
                    {
                        memberToExclude
                    };

            if (memberToExclude is GameObject go)
            {
                var comps = go.GetComponents<Component>();
                relatedMembers.AddRange(comps);
            }
            else if (memberToExclude is ParticleSystem ps)
            {
                relatedMembers.Add(ps.gameObject.GetComponent<ParticleSystemRenderer>());
            }


            relatedMembers = relatedMembers.Except(alreadyExcludedMembers).ToList();


            description.memberExclusionSettings.excludedMembers.AddRange(relatedMembers);

            PopulateInlinedDescription(infra, description);

            Debug.Log($"Updated member list to exclude members:\n" +
                $"{string.Join("\n", relatedMembers.Select(m => m == null ? "null" : m.name))}");


            description.memberExclusionSettings.memberToExclude = null;
        }
    }
}
