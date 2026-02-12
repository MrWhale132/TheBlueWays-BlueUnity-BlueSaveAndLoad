using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RecursiveTree))]
public class RecursiveTreeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RecursiveTree tree = (RecursiveTree)target;
        serializedObject.Update();

        SerializedProperty nodesProp = serializedObject.FindProperty("serializedNodes");

        // Global "Add Root" button
        if (GUILayout.Button("Add New Root Node"))
        {
            Undo.RecordObject(tree, "Add Root Node");
            tree.rootNodes.Add(new Node { name = "New Root" });
            tree.SerializeTree();
        }

        EditorGUILayout.Space(10);

        for (int i = 0; i < nodesProp.arraySize; i++)
        {
            SerializedProperty wrapperProp = nodesProp.GetArrayElementAtIndex(i);
            SerializedProperty nodeProp = wrapperProp.FindPropertyRelative("node");
            int depth = wrapperProp.FindPropertyRelative("depth").intValue;
            int parentIndex = wrapperProp.FindPropertyRelative("parentIndex").intValue;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(depth * 20);
            // [+] Add Child
            if (GUILayout.Button("+", GUILayout.Width(25)))
            {
                Undo.RecordObject(tree, "Add Child");
                tree.GetParent(i).children.Add(new Node { name = "New Child" });
                tree.SerializeTree();
                break;
            }

            // [X] Remove Node (Works for Roots and Children)
            if (GUILayout.Button("x", GUILayout.Width(25)))
            {
                Undo.RecordObject(tree, "Remove Node");
                if (parentIndex == -1)
                    tree.rootNodes.Remove(tree.GetParent(i));
                else
                    tree.GetParent(parentIndex).children.Remove(tree.GetParent(i));

                tree.SerializeTree();
                break;
            }

            GUILayout.Space(10);

            // Draw Fields automatically
            EditorGUILayout.PropertyField(nodeProp, new GUIContent(nodeProp.FindPropertyRelative("name").stringValue), true);

            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}







//[CustomEditor(typeof(RecursiveTree))]
//public class RecursiveTreeEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        RecursiveTree tree = (RecursiveTree)target;
//        serializedObject.Update();

//        SerializedProperty nodesProp = serializedObject.FindProperty("serializedNodes");

//        if (GUILayout.Button("Reset/Initialize Tree"))
//        {
//            Undo.RecordObject(tree, "Reset Tree");
//            tree.rootNode = new Node { name = "Root" };
//            tree.SerializeTree();
//        }

//        EditorGUILayout.Space(10);

//        for (int i = 0; i < nodesProp.arraySize; i++)
//        {
//            SerializedProperty wrapperProp = nodesProp.GetArrayElementAtIndex(i);
//            SerializedProperty nodeProp = wrapperProp.FindPropertyRelative("node");
//            int depth = wrapperProp.FindPropertyRelative("depth").intValue;
//            int parentIndex = wrapperProp.FindPropertyRelative("parentIndex").intValue;

//            EditorGUILayout.BeginHorizontal();
//            GUILayout.Space(depth * 20);

//            // 1. ADD BUTTON [+]
//            if (GUILayout.Button("+", GUILayout.Width(25)))
//            {
//                Undo.RecordObject(tree, "Add Child");
//                // Get the actual node object from the serialized list
//                var parentNode = tree.GetParent(i);
//                parentNode.children.Add(new Node { name = "New Child" });
//                tree.SerializeTree(); // Force immediate refresh
//                break; // Break loop to avoid collection modified errors
//            }

//            // 2. REMOVE BUTTON [X] (Don't allow removing root)
//            if (parentIndex != -1 && GUILayout.Button("X", GUILayout.Width(25)))
//            {
//                Undo.RecordObject(tree, "Remove Node");
//                var parentNode = tree.GetParent(parentIndex);
//                var nodeToRemove = tree.GetParent(i);
//                parentNode.children.Remove(nodeToRemove);
//                tree.SerializeTree();
//                break;
//            }

//            // 3. DRAW AUTOMATIC FIELDS
//            // Use 'false' in PropertyField to keep the node collapsed by default if it's large
//            EditorGUILayout.PropertyField(nodeProp, new GUIContent(nodeProp.FindPropertyRelative("name").stringValue), true);

//            EditorGUILayout.EndHorizontal();
//        }

//        serializedObject.ApplyModifiedProperties();
//    }
//}


















//[CustomEditor(typeof(RecursiveTree))]
//public class RecursiveTreeEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        // Get the "serializedNodes" list property
//        SerializedProperty nodesProp = serializedObject.FindProperty("serializedNodes");

//        SerializedProperty nodesListProp = serializedObject.FindProperty("serializedListNodes");

//        EditorGUILayout.LabelField("Recursive Tree (Flat-Serialized)", EditorStyles.boldLabel);

//        DrawRoot(nodesProp);


//        //Debug.Log(nodesListProp == null);
//        //for(int i = 0; i < nodesListProp.arraySize; i++)
//        //{

//        //   SerializedProperty listWrapperProp = nodesListProp.GetArrayElementAtIndex(i);
//        //    //SerializedProperty listNodesProp = listWrapperProp.FindPropertyRelative("nodes");

//        //    EditorGUILayout.Space(10);
//        //    EditorGUILayout.LabelField($"Root Node List {i}", EditorStyles.boldLabel);
//        //    DrawRoot(listWrapperProp);
//        //}

//        serializedObject.ApplyModifiedProperties();
//    }

//    public void DrawRoot(SerializedProperty nodesProp)
//    {
//        if (nodesProp.arraySize == 0)
//        {
//            if (GUILayout.Button("Initialize Tree"))
//            {
//                // Logic to add the first node via code...
//            }
//        }

//        for (int i = 0; i < nodesProp.arraySize; i++)
//        {
//            SerializedProperty wrapperProp = nodesProp.GetArrayElementAtIndex(i);
//            SerializedProperty nodeProp = wrapperProp.FindPropertyRelative("node");
//            int depth = wrapperProp.FindPropertyRelative("depth").intValue;

//            // Start a horizontal layout to handle the indentation
//            EditorGUILayout.BeginHorizontal();

//            // Apply indentation (20 pixels per depth level)
//            GUILayout.Space(depth * 20);

//            // Draw the entire "Node" object automatically! 
//            // The 'true' parameter tells Unity to include all its children fields (int, color, etc.)
//            EditorGUILayout.PropertyField(nodeProp, new GUIContent($"Node {i}"), true);

//            EditorGUILayout.EndHorizontal();

//            // Add a little separator
//            if (nodeProp.isExpanded) EditorGUILayout.Space(5);
//        }
//    }
//}


























//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(RecursiveTree))]
//public class RecursiveTreeEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        RecursiveTree tree = (RecursiveTree)target;

//        serializedObject.Update();

//        EditorGUILayout.LabelField("Tree Hierarchy", EditorStyles.boldLabel);

//        if (tree.rootNode == null)
//        {
//            if (GUILayout.Button("Initialize Root"))
//            {
//                tree.rootNode = new Node { name = "Root" };
//            }
//        }
//        else
//        {
//            DrawNode(tree.rootNode, 0);
//        }

//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(tree);
//        }

//        serializedObject.ApplyModifiedProperties();
//    }

//    private void DrawNode(Node node, int indent)
//    {
//        EditorGUILayout.BeginHorizontal();
//        GUILayout.Space(indent * 20); // Create visual nesting

//        node.name = EditorGUILayout.TextField(node.name);
//        node.number = EditorGUILayout.IntField(node.number);


//        if (GUILayout.Button("+", GUILayout.Width(25)))
//        {
//            node.children.Add(new Node { name = "New Child" });
//        }

//        if (indent > 0 && GUILayout.Button("-", GUILayout.Width(25)))
//        {
//            // Note: Removing nodes requires a bit more logic to find the parent, 
//            // but for this example, we'll keep it simple.
//        }
//        EditorGUILayout.EndHorizontal();

//        foreach (var child in node.children)
//        {
//            DrawNode(child, indent + 1);
//        }
//    }
//}