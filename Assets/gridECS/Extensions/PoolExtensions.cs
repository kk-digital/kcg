using Entitas;
using System.Collections.Generic;
using UnityEngine;

public static class PoolExtensions {

    public static float GetDistanceBetweenNodes(Entity a, Entity b)
    {
        float result = 1;

        return result;
    }

    public static List<Entity> RetracePath(Entity startNode, Entity endNode)
    {
        List<Entity> path = new List<Entity>();
        Entity currentNode = endNode;
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.node.parent;
        }

        path.Reverse();

        return path;
    }
}
