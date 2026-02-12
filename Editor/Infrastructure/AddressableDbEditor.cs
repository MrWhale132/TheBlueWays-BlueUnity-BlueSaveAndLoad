using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AddressableDb))]
public class AddressableDbEditor:Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AddressableDb myTarget = (AddressableDb)target;

        if (GUILayout.Button("Refresh"))
        {
            myTarget.Refresh();
        }
        if (GUILayout.Button("Save To Disk"))
        {
            myTarget.SaveToDisk();
        }
    }
}
