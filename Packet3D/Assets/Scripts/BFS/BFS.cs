using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BFS
{
    // static array, could also be non static.
    static Node[] nodes;
    // ctor, all nodes are found
    static BFS()
    {
        nodes = (Node[])MonoBehaviour.FindObjectsOfType(typeof(Node));
    }
    // Iterate and set
    static void ResetNode()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].used = false;
        }
    }

    public static Node[] BFSMethod(Node start, Node target)
    {
        Node final = null;
        if (start == target)
        {
            Node[] nodeArray = { start };
            return nodeArray;
        }

        // Set this one to null
        start.parent = null;
        Queue<Node> queue = new Queue<Node>();
        Node[] children = start.children;

        foreach (Node node in children)
        {
            // Set the parent for those node as the start node      
            node.parent = start;
            queue.Enqueue(node);
        }
        while (queue.Count > 0)
        {
            Node n = queue.Dequeue();
            if (n == target)
            {
                final = n;
                break;
            }
            Node[] c = n.children;
            foreach (Node nod in c)
            {
                if (nod.used) { continue; }
                // Set parent node
                nod.parent = n;
                queue.Enqueue(nod);
            }
        }
        // Same code below
        // If final is null we could not find a path
        if (final == null) return null;
        // Get the parent node of the final node
        Node parent = final.parent;
        List<Node> list = new List<Node>();

        // While the parent is not the start node
        // This will go back up parent of each node
        // and then define the parent as current node
        while (parent != null)
        {
            list.Add(final);
            final = parent;
            parent = final.parent;
        }
        // We add the last two remaining ones
        list.Add(final);
        // The list is from target to start
        // We reverse it
        list.Reverse();
        // And convert it to array
        return list.ToArray();
    }
    
}
