
using UnityEngine;
using UnityEditor;
using Assets._Project.Scripts.SaveAndLoad;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

namespace Packages.com.theblueway.saveandload.Editor.SaveAndLoad.HandledTypeNameSearchFeature
{
    [CustomPropertyDrawer(typeof(HandledTypeSaveHandlerIdAttribute))]
    public class SearchableStringDrawer : PropertyDrawer
    {
        public const string StaticPrefix = "(static) ";


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //var att = (SearchableStringAttribute)attribute;

            //EditorGUI.BeginProperty(position, label, property);

            //Rect fieldRect = position;
            //fieldRect.width -= 25;

            //Rect buttonRect = position;
            //buttonRect.x += fieldRect.width + 5;
            //buttonRect.width = 20;

            //EditorGUI.PropertyField(fieldRect, property, label);

            //if (GUI.Button(buttonRect, "⋯"))
            //{
            //    SearchableStringPopup.Open(
            //        att.Options,
            //        property.stringValue,
            //        newValue =>
            //        {
            //            property.stringValue = newValue;
            //            property.serializedObject.ApplyModifiedProperties();
            //        });
            //}

            //EditorGUI.EndProperty();

            var lookup = SaveAndLoadManager.Service_.GetHandledTypeNameByHandlerIdLookup();

            var options = new List<string>();

            var instanceHandlers = new List<string>();
            var staticHandlers = new List<string>();


            foreach((long handlerId,(string typeName,bool isStatic) val) in lookup)
            {
                string option = val.typeName;

                if(val.isStatic)
                    option = StaticPrefix + option;

                option = handlerId.ToString()+"  " + option;
                
                if(val.isStatic)
                    staticHandlers.Add(option);
                else
                    instanceHandlers.Add(option);
            }
            
            options.AddRange(instanceHandlers);
            options.AddRange(staticHandlers);

            
            EditorGUI.BeginProperty(position, label, property);

            Rect fieldRect = EditorGUI.PrefixLabel(position, label);

            // Detect click BEFORE drawing anything that may consume the event
            if (Event.current.type == EventType.MouseDown && fieldRect.Contains(Event.current.mousePosition))
            {
                SearchableStringPopup.Open(
                    options,
                    property.longValue.ToString(),
                    newValue =>
                    {
                        var parts = newValue.Split(' ');
                        long id = long.Parse(parts[0]);
                        property.longValue = id;
                        property.serializedObject.ApplyModifiedProperties();
                    });

                Event.current.Use(); // swallow the click
            }

            // Draw a non-editable field (but NOT a SelectableLabel)
            using (new EditorGUI.DisabledScope(true))
            {
                var handledTypeName = lookup.ContainsKey(property.longValue)
                    ? lookup[property.longValue].handledTypeName
                    : "<unassigned>";

                EditorGUI.TextField(fieldRect, handledTypeName, EditorStyles.popup);
            }

            EditorGUI.EndProperty();
        }
    }

}
