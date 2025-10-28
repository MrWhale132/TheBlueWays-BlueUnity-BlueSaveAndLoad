using Assets._Project.Scripts.Infrastructure.AddressableInfra;
using Assets._Project.Scripts.UtilScripts.Misc;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RandomIdGenerator))]
public class RandomIdGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        RandomIdGenerator myTarget = (RandomIdGenerator)target;

        if (GUILayout.Button("Generate"))
        {
            myTarget.Generate();
        }
    }
}
