using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class TestObject
{
    public bool someValue;
    public float anotherValue;
}

[Serializable]
public class Node
{
    public string name;
    public int number;
    public Color nodeColor;
    public Vector3 position; // Add as many fields as you want!
    public TestObject testObject;

    // This is the recursive part Unity hates. 
    // We mark it [NonSerialized] so the engine ignores it.
    [NonSerialized]
    public List<Node> children = new List<Node>();
}


[Serializable]
public struct SerializableNodeWrapper
{
    public Node node;
    public int parentIndex;
    public int depth; // We'll use this for the Inspector indent
}




//[CreateAssetMenu(fileName = "RecursiveTree", menuName = "Scriptable Objects/RecursiveTree")]
public class RecursiveTree : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private List<SerializableNodeWrapper> serializedNodes = new List<SerializableNodeWrapper>();

    // Now a list of entry points
    public List<Node> rootNodes = new List<Node>();

    public void OnBeforeSerialize() => SerializeTree();
    public void OnAfterDeserialize() => DeserializeTree();

    public void SerializeTree()
    {
        serializedNodes.Clear();
        if (rootNodes == null) return;

        foreach (var root in rootNodes)
        {
            if (root != null) Flatten(root, -1, 0);
        }
    }

    private void Flatten(Node node, int parentIndex, int depth)
    {
        int currentIndex = serializedNodes.Count;
        serializedNodes.Add(new SerializableNodeWrapper { node = node, parentIndex = parentIndex, depth = depth });
        foreach (var child in node.children) Flatten(child, currentIndex, depth + 1);
    }

    private void DeserializeTree()
    {
        if (serializedNodes.Count == 0) return;

        rootNodes.Clear();
        foreach (var wrapper in serializedNodes) wrapper.node.children.Clear();

        for (int i = 0; i < serializedNodes.Count; i++)
        {
            var current = serializedNodes[i];
            if (current.parentIndex == -1)
            {
                // It's a root node
                rootNodes.Add(current.node);
            }
            else
            {
                // It's a child node
                serializedNodes[current.parentIndex].node.children.Add(current.node);
            }
        }
    }

    public Node GetParent(int parentIndex)
    {
        if (parentIndex >= 0 && parentIndex < serializedNodes.Count)
            return serializedNodes[parentIndex].node;
        return null;
    }
}








//public class RecursiveTree : ScriptableObject, ISerializationCallbackReceiver
//{
//    [SerializeField] private List<SerializableNodeWrapper> serializedNodes = new List<SerializableNodeWrapper>();
//    public Node rootNode;

//    public void OnBeforeSerialize() => SerializeTree();
//    public void OnAfterDeserialize() => DeserializeTree();

//    public void SerializeTree()
//    {
//        serializedNodes.Clear();
//        if (rootNode != null) Flatten(rootNode, -1, 0);
//    }

//    private void Flatten(Node node, int parentIndex, int depth)
//    {
//        int currentIndex = serializedNodes.Count;
//        serializedNodes.Add(new SerializableNodeWrapper { node = node, parentIndex = parentIndex, depth = depth });
//        foreach (var child in node.children) Flatten(child, currentIndex, depth + 1);
//    }

//    private void DeserializeTree()
//    {
//        if (serializedNodes.Count == 0) return;
//        foreach (var wrapper in serializedNodes) wrapper.node.children.Clear();

//        for (int i = 0; i < serializedNodes.Count; i++)
//        {
//            var current = serializedNodes[i];
//            if (current.parentIndex != -1)
//                serializedNodes[current.parentIndex].node.children.Add(current.node);
//        }
//        rootNode = serializedNodes[0].node;
//    }

//    // Helper for the Editor to find a parent easily
//    public Node GetParent(int parentIndex)
//    {
//        if (parentIndex >= 0 && parentIndex < serializedNodes.Count)
//            return serializedNodes[parentIndex].node;
//        return null;
//    }
//}


//[CreateAssetMenu(fileName = "RecursiveTree", menuName = "Scriptable Objects/RecursiveTree")]
//public class RecursiveTree : ScriptableObject,  ISerializationCallbackReceiver
//{
//    // This is what Unity WILL serialize.
//    [SerializeField] private List<SerializableNodeWrapper> serializedNodes = new List<SerializableNodeWrapper>();
//    [SerializeField] private List<List<SerializableNodeWrapper>> serializedListNodes = new ();

//    public Node rootNode;
//    public List<Node> rootNodes;

//    // 1. Flatten the tree into a list before Unity saves
//    public void OnBeforeSerialize()
//    {
//        serializedNodes.Clear();
//        Flatten(rootNode, -1,0);

//        foreach (var root in rootNodes)
//        {
//            var tempList = new List<SerializableNodeWrapper>();
//            Flatten(root, -1,0);
//            serializedListNodes.Add(tempList);
//        }
//    }

//    // 2. Rebuild the tree from the list after Unity loads
//    public void OnAfterDeserialize()
//    {
//        Build(serializedNodes,out rootNode);

//        rootNodes = new List<Node>();
//        foreach (var serializedList in serializedListNodes)
//        {
//            Build(serializedList, out Node root);
//            rootNodes.Add(root);
//        }
//    }

//    public void Build(List<SerializableNodeWrapper> serializedNodes, out Node rootNode)
//    {
//        if (serializedNodes.Count == 0)
//        {
//            rootNode = null;
//            return;
//        }

//        // Reconstruct the actual recursive references
//        for (int i = 0; i < serializedNodes.Count; i++)
//        {
//            var current = serializedNodes[i];
//            if (current.parentIndex != -1)
//            {
//                serializedNodes[current.parentIndex].node.children.Add(current.node);
//            }
//        }
//        rootNode = serializedNodes[0].node;
//    }


//    private void Flatten(Node node, int parentIndex, int depth)
//    {
//        if (node == null) return;

//        int currentIndex = serializedNodes.Count;
//        serializedNodes.Add(new SerializableNodeWrapper
//        {
//            node = node,
//            parentIndex = parentIndex,
//            depth = depth
//        });

//        foreach (var child in node.children)
//        {
//            Flatten(child, currentIndex, depth + 1);
//        }
//    }

//    // Update the call in OnBeforeSerialize:
//    // Flatten(rootNode, -1, 0);

//}

