using UnityEditor;
using UnityEngine;

namespace Assets._Project.Scripts.Infrastructure
{
    [CustomEditor(typeof(GOInfra))]
    public class GOInfraEditor: UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GOInfra myTarget = (GOInfra)target;

            if (GUILayout.Button("Add infra to all child"))
            {
                myTarget.AddInfraToAllChild();


                EditorUtility.SetDirty(myTarget); // Mark component as dirty
                EditorUtility.SetDirty(myTarget.gameObject); // Mark component as dirty
                PrefabUtility.RecordPrefabInstancePropertyModifications(myTarget); // For prefab instance
            }

        }
    }
}
