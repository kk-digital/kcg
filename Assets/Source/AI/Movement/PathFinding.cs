using System;
using System.Collections.Generic;
using Enums.Tile;
using KMath;
using PlanetTileMap;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace AI.Movement
{
    internal struct Node
    {
        public int id;  // Is  equal closedList index. 
        public int parentID;
        public Vec2f pos;

        public int pathCost;
        public int heuristicCost;
        public int totalCost;

        public bool IsCheaper(ref Node node)
        {
            return totalCost < node.totalCost ? true : false;
        }

        public void UpdateCost(Vec2f end)
        {
            heuristicCost = (int)Heuristics.euclidean_distance(pos, end) * 100;
            totalCost = heuristicCost + pathCost;
        }
    }

    internal struct PathAdjacency
    {
        public Vec2f dir;
        public int cost;
    }

    internal struct CharacterMovement
    {
        public float JumpMaxDistance;
        public float MaxJumpNumber;
        public float DownMaxDistance;
    }

    internal static class Heuristics
    {
        static public float euclidean_distance(Vec2f firstPos, Vec2f secondPos)
        {
            float x = firstPos.X - secondPos.X;
            float y = firstPos.Y - secondPos.Y;
            return System.MathF.Sqrt(x * x + y * y);
        }

        static public float manhattan_distance(Vec2f firstPos, Vec2f secondPos)
        {
            float x = firstPos.X - secondPos.X;
            float y = firstPos.Y - secondPos.Y;
            return System.MathF.Abs(x) + System.MathF.Abs(y);
        }
    }

    public class PathFinding
    {
        const int MAX_NUM_NODES = 256; // Maximum size of open/closed Map.

        readonly PathAdjacency[] directions = new PathAdjacency[8] 
            {   new PathAdjacency() { dir = new Vec2f(1f, 0f),  cost = 100 },   // Right
                new PathAdjacency() { dir = new Vec2f(-1f, 0f), cost = 100 },   // Left
                new PathAdjacency() { dir = new Vec2f(0f, 1f),  cost = 100 },   // Up
                new PathAdjacency() { dir = new Vec2f(0f, -1f), cost = 100 },   // Down
                new PathAdjacency() { dir = new Vec2f(1f, 1f),  cost = 144 },   // Right Up 
                new PathAdjacency() { dir = new Vec2f(1f, -1f), cost = 144 },   // Right Down
                new PathAdjacency() { dir = new Vec2f(-1f, 1f), cost = 144 },   // Left Up
                new PathAdjacency() { dir = new Vec2f(-1f,-1f), cost = 144 }};  // Left Down

        Node[] openList;
        Node[] closedList;
        Node firstNode;

        HashSet<Vec2f> openSet;
        HashSet<Vec2f> closedSet;

        public void Initialize()
        {
            openList = new Node[MAX_NUM_NODES];
            closedList = new Node[MAX_NUM_NODES];
            firstNode = new Node();

            openSet = new HashSet<Vec2f>();
            closedSet = new HashSet<Vec2f>();
            openSet.EnsureCapacity(MAX_NUM_NODES);
            closedSet.EnsureCapacity(MAX_NUM_NODES);
        }

        public Vec2f[] getPath(ref TileMap tileMap, Vec2f start, Vec2f end)
        {
            if (tileMap.GetFrontTile((int)end.X, (int)end.Y).ID != Enums.Tile.TileID.Air)
            {
                Debug.Log("Not possible path. Endpoint is solid(unreacheable)");
            }

            // Check max distance here.
            SetFirstNode(start, end);

            // Todo: Profile sorting against searching, Gnomescroll sort the nodes. I am not sure its the fatest way.
            // It's possible that 
            int sortStartPost = 0; // If 0 no sorting needed.
            while (true)
            {
                if (openSet.Count >= MAX_NUM_NODES || closedSet.Count >= MAX_NUM_NODES)
                {
                    Debug.Log("The path is taking too long. Giving up.");
                    return null;
                }

                // We failed to find a path if open list is empty.
                if (openSet.Count == 0)
                {
                    Debug.Log("Couldn't find a path to the destination.");
                    return null;
                }

                if (sortStartPost > 0)
                {
                    SortOpenList(sortStartPost);
                    sortStartPost = 0;
                }

                // Move to closed list.
                ref Node current = ref openList[openSet.Count];
                openSet.Remove(current.pos);

                current.id = closedSet.Count;
                AddNodeCloseList(ref current);

                if (current.pos == end)
                {
                    Debug.Log("Path found.");
                    break;
                }

                for (int i = 0; i < directions.Length; i++)
                {
                    Node node = current;
                    node.parentID = current.id;

                    if (!PassableJump(ref tileMap, ref node, i))
                        continue;

                    node.UpdateCost(end);

                    if (openSet.Contains(node.pos))
                    {
                        // Todo(Performace): Only search among nodes with cheaper cost.
                        int index = Array.IndexOf(openList, node);
                        if (node.IsCheaper(ref openList[index]))
                        {
                            openList[index] = node;
                            if(sortStartPost > index)
                                sortStartPost = index;
                        }
                        continue;
                    }

                    if (closedSet.Contains(node.pos))
                    {
                        int index = Array.IndexOf(closedList, node);
                        if (!node.IsCheaper(ref closedList[index]))
                        {
                            continue;
                        }
                    }

                    openSet.Add(node.pos);
                    sortStartPost = openSet.Count - 1;
                }
            }

            return constructPath(end);
        }

        Vec2f[] constructPath(Vec2f end)
        {
            int first = closedList[closedSet.Count - 1].parentID;
            int len = 1;

            // Get path lenth.
            while (first >= 0)
            {
                first = closedList[first].parentID;
                len++;
            }

            Vec2f[] path = new Vec2f[len];

            // Add to path array starting form last element.
            first = closedList[closedSet.Count - 1].parentID;
            while (first >= 0)
            {
                path[--len] = closedList[first].pos;
                first = closedList[first].parentID;
            }

            // Todo: filter path.

            return path;
        }

        // todo: Deals with non square blocks.
        bool PassableFly(ref TileMap tileMap, ref Node current, int indDir)
        {
            // TODO -- parameterized
            Vec2i CHARACTER_SIZE = new Vec2i(1, 1); // How many blocks character takes.

            Vec2f exitPos = current.pos + directions[indDir].dir;

            // Check if character can move to this tile
            Vec2i tilePos =
                new Vec2i((int)(exitPos.X - 0.5f) + (CHARACTER_SIZE.X - 1),
                    (int)(exitPos.Y - 0.5f) + (CHARACTER_SIZE.Y - 1)); // Get block character wasn't occupying before.

            // If solid return false.
            if (tileMap.GetFrontTile(tilePos.X, tilePos.Y).ID != TileID.Air)
                return false;

            // if Diagonal directions.
            if (indDir > 3)
            {
                // Adjacent blocks needs to be free to go in a diagonal directions. 
                // (This is a simplification) This should not be true for small agents.
                Vec2i verTilePos = tilePos;
                verTilePos.X -= (int)directions[indDir].dir.X;

                if (tileMap.GetFrontTile(verTilePos.X, verTilePos.Y).ID != TileID.Air)
                    return false;

                Vec2i horTilePos = tilePos;
                horTilePos.Y -= (int)directions[indDir].dir.Y;

                if (tileMap.GetFrontTile(horTilePos.X, horTilePos.Y).ID != TileID.Air)
                    return false;
            }

            current.pos = exitPos;
            current.pathCost += directions[indDir].cost;
            return true;
        }

        /// <summary>
        /// Check if player can reach the space. 
        /// Return false if tile is either too high or two low. 
        /// Return false if block is solid.
        /// </summary>
        bool PassableJump(ref TileMap tileMap, ref Node current, int indDir)
        {
            // TODO -- parameterized
            Vec2i CHARACTER_SIZE = new Vec2i(1, 1); // How many blocks character takes.
            const int MAX_UP = 3;
            const int MAX_DOWN = 9;

            current.pos = current.pos + directions[indDir].dir;
            
            // Check if tile is inside the map.
            if (current.pos.X < 0 || current.pos.X > tileMap.MapSize.X ||
                current.pos.Y < 0 || current.pos.Y > tileMap.MapSize.Y)
                return false;

            current.pathCost += directions[indDir].cost;

            // Todo deals with jumping.
            if (indDir > 2)
            {
                return false;
            }

            // Check if character can move to this tile
            Vec2i tilePos =
                new Vec2i((int)(current.pos.X - 0.5f) + (CHARACTER_SIZE.X - 1),
                    (int)(current.pos.Y - 0.5f) + (CHARACTER_SIZE.Y - 1)); // Get block character wasn't occupying before.

            // Check if tile is inside the map.
            if (tilePos.X < 0 || tilePos.X > tileMap.MapSize.X ||
                tilePos.Y < 0 || tilePos.Y > tileMap.MapSize.Y)
                return false;

            // If solid return false.
            if (tileMap.GetFrontTile(tilePos.X, tilePos.Y).ID != TileID.Air)
                return false;

            // Is tile at ground.
            Vec2i GroundPos = new Vec2i((int)(current.pos.X - 0.5f), (int)(current.pos.Y - 0.5f) - 1);
            if (tileMap.GetFrontTile(GroundPos.X, GroundPos.Y).ID == TileID.Air)
                return false;

            return true;
        }

        /// <summary>
        /// Uses insertion sort. It's the fastest algorithm for almost sorted data.
        /// startPos is an optimization. Every element before startPos is sorted.
        /// </summary>
        void SortOpenList(int startPos)
        {
            for (int i = startPos; i < openSet.Count; i++)
            {
                Node current = openList[i];
                int j = i - 1;
                while (j >= 0 && !current.IsCheaper(ref openList[j]))
                {
                    openList[j + 1] = openList[j];
                    j--;
                }
                openList[j + 1] = current;
            }
        }

        void AddNodeOpenList(ref Node node)
        {
            openList[openSet.Count] = node;
            openSet.Add(firstNode.pos);
        }

        void AddNodeCloseList(ref Node node)
        {
            closedList[closedSet.Count] = node;
            closedSet.Add(firstNode.pos);
        }

        void SetFirstNode(Vec2f start, Vec2f end)
        {
            firstNode.parentID = -1;
            firstNode.id = 0;
            firstNode.pathCost = 0;
            firstNode.pos = start;
            firstNode.UpdateCost(end);
            AddNodeOpenList(ref firstNode);
        }
    }
}
