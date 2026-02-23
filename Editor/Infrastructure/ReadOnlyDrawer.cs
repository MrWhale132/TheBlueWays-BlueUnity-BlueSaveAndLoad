using Assets._Project.Scripts.UtilScripts;
using Theblueway.SaveAndLoad;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        //todo: handle arrays and lists
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(property?.boxedValue?.GetType().Name == typeof(RandomId).Name)
            {
                return;
            }

            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }

}