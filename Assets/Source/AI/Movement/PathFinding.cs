using System;
using System.Collections.Generic;
using System.Reflection;
using Enums.Tile;
using KMath;
using PlanetTileMap;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

namespace AI.Movement
{
    struct Node
    {
        public int id;  // Is  equal closedList index. 
        public int parentID;
        public Vec2i pos;

        public float pathCost;
        public float heuristicCost;
        public float totalCost;
        
        public int jumpValue;

        public bool IsCheaper(ref Node node)
        {
            return totalCost < node.totalCost ? true : false;
        }

        public void UpdateCost(Vec2i end)
        {
            heuristicCost = Heuristics.euclidean_distance(pos, end) * 100f;
            totalCost = heuristicCost + pathCost;
        }

        // Used by Array.indexOF
        public override bool Equals(object obj)
        {
            if (obj is Node)
                return pos == ((Node)obj).pos ? true : false;
            return false;
        }
    }

    internal struct PathAdjacency
    {
        public Vec2i dir;
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
        static public float euclidean_distance(Vec2i firstPos, Vec2i secondPos)
        {
            float x = firstPos.X - secondPos.X;
            float y = firstPos.Y - secondPos.Y;
            return System.MathF.Sqrt(x * x + y * y);
        }

        static public float manhattan_distance(Vec2i firstPos, Vec2i secondPos)
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
            {   new PathAdjacency() { dir = new Vec2i(1, 0),  cost = 100 },   // Right
                new PathAdjacency() { dir = new Vec2i(-1, 0), cost = 100 },   // Left
                new PathAdjacency() { dir = new Vec2i(0, 1),  cost = 100 },   // Up
                new PathAdjacency() { dir = new Vec2i(1, 1),  cost = 144 },   // Right Up 
                new PathAdjacency() { dir = new Vec2i(-1, 1), cost = 144 },   // Left Up
                new PathAdjacency() { dir = new Vec2i(0, -1), cost = 100 },   // Down
                new PathAdjacency() { dir = new Vec2i(1, -1), cost = 144 },   // Right Down
                new PathAdjacency() { dir = new Vec2i(-1,-1), cost = 144 }};  // Left Down

        Node[] openList;
        Node[] closedList;
        Node firstNode;

        // Used to check if node exists. 
        // Todo: Use a binary grid to do the testing. 
        HashSet<Vec2i> openSet;
        HashSet<Vec2i> closedSet;

        public void Initialize()
        {
            openList = new Node[MAX_NUM_NODES];
            closedList = new Node[MAX_NUM_NODES];
            firstNode = new Node();

            openSet = new HashSet<Vec2i>();
            closedSet = new HashSet<Vec2i>();
            openSet.EnsureCapacity(MAX_NUM_NODES);
            closedSet.EnsureCapacity(MAX_NUM_NODES);
        }

        /// <summary>
        /// Path is the shortest possible path.
        /// Vec2f[] has closest node at the end of the list.
        /// </summary>
        public Vec2f[] getPath(ref TileMap tileMap, Vec2f start, Vec2f end)
        {
            if (tileMap.GetFrontTile((int)end.X, (int)end.Y).MaterialType != TileMaterialType.Air)
            {
                Debug.Log("Not possible path. Endpoint is solid(unreacheable)");
            }

            Vec2i startPos = new Vec2i((int)start.X, (int)start.Y);
            Vec2i endPos = new Vec2i((int)end.X, (int)end.Y);

            // Check max distance here.
            SetFirstNode(startPos, endPos);
            SetFirstNodeJumpValue(startPos, ref tileMap);

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
                Node current = openList[openSet.Count - 1];
                openSet.Remove(current.pos);

                current.id = closedSet.Count;
                AddNodeCloseList(ref current);

                if (current.pos == endPos)
                {
                    break;
                }

                for (int i = 0; i < directions.Length; i++)
                {
                    Node node = current;
                    node.parentID = current.id;

                    if (!PassableJump(ref tileMap, ref node, i))
                        continue;

                    node.UpdateCost(endPos);

                    if (openSet.Contains(node.pos))
                    {
                        // Todo(Performace): Only search among nodes with cheaper cost.
                        int index = Array.IndexOf(openList, node);
                        if (node.IsCheaper(ref openList[index]))
                        {
                            openList[index] = node;
                            if (sortStartPost > index)
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

                    openList[openSet.Count] = node;
                    openSet.Add(node.pos);
                    if (sortStartPost == 0)
                        sortStartPost = openSet.Count - 1;
                }
            }

            // Todo filter nodes befores returning path.
            return constructPath(endPos);
        }

        Vec2f[] constructPath(Vec2i end)
        {
            int first = closedList[closedSet.Count - 1].parentID;
            int length = 0;

            // Get path lenth.
            while (first >= 0)
            {
                first = closedList[first].parentID;
                length++;
            }

            Vec2f[] path = new Vec2f[length];

            // Add to path array starting form last element.
            first = closedList[closedSet.Count - 1].id;
            for (int i = 0; i < length; i++)
            {
                path[i] = new Vec2f(closedList[first].pos.X, closedList[first].pos.Y);
                first = closedList[first].parentID;
            }

            // Todo: filter path.

            return path;
        }

        // todo: Deals with non square blocks.
        /*
        bool PassableFly(ref TileMap tileMap, ref Node current, int indDir)
        {
            // TODO -- parameterized
            Vec2i CHARACTER_SIZE = new Vec2i(1, 1); // How many blocks character takes.

            Vec2i exitPos = current.pos + directions[indDir].dir;

            // Check if character can move to this tile
            Vec2i tilePos =
                new Vec2i((int)(exitPos.X - 0.5f) + (CHARACTER_SIZE.X - 1),
                    (int)(exitPos.Y - 0.5f) + (CHARACTER_SIZE.Y - 1)); // Get block character wasn't occupying before.

            // If solid return false.
            if (tileMap.GetFrontTile(tilePos.X, tilePos.Y).MaterialType != TileMaterialType.Air)
                return false;

            // if Diagonal directions.
            if (indDir > 3)
            {
                // Adjacent blocks needs to be free to go in a diagonal directions. 
                // (This is a simplification) This should not be true for small agents.
                Vec2i verTilePos = tilePos;
                verTilePos.X -= (int)directions[indDir].dir.X;

                if (tileMap.GetFrontTile(verTilePos.X, verTilePos.Y).MaterialType != TileMaterialType.Air)
                    return false;

                Vec2i horTilePos = tilePos;
                horTilePos.Y -= (int)directions[indDir].dir.Y;

                if (tileMap.GetFrontTile(horTilePos.X, horTilePos.Y).MaterialType != TileMaterialType.Air)
                    return false;
            }

            current.pos = exitPos;
            current.pathCost += directions[indDir].cost;
            return true;
        }*/

        /// <summary>
        /// Check if player can reach the space. 
        /// Return false if tile is either too high or two low. 
        /// Return false if block is solid.
        /// </summary>
        bool PassableJump(ref TileMap tileMap, ref Node current, int indDir)
        {
            // TODO -- parameterized
            const int MAX_UP = 3; // Maximum number of blocks down.
            const int MAX_DOWN = 9;

            // Todo: deals with diagonals.
            // Algorithm consider that character can move one block to the right for each one up.
            const int maxJump = MAX_UP; // Todo: check agent speed and deals with max number of blocks moved at x direction.
            const int maxDown = maxJump + MAX_DOWN;

            current.pos = current.pos + directions[indDir].dir;
            current.pathCost += directions[indDir].cost;

            // Check if tile is inside the map.
            if (current.pos.X < 0 || current.pos.X > tileMap.MapSize.X ||
                current.pos.Y < 0 || current.pos.Y > tileMap.MapSize.Y)
                return false;

            // If solid return false.
            if (tileMap.GetFrontTile(current.pos.X, current.pos.Y).MaterialType != TileMaterialType.Air)
                return false;

            // Jump and falling paths:
            if (indDir > 1)
            {
                if (indDir < 5) // UP
                {
                    if (current.jumpValue >= 3)
                        return false;
                    current.jumpValue++;
                }
                else // Dowm
                {
                    // Todo deals with falling speed. When falling speed it too high diagonals may be impossible.
                    if (current.jumpValue >= maxDown)
                        return false;
                    current.jumpValue = (current.jumpValue < 8) ? 8 : current.jumpValue + 1;
                }
                // Set jump to zero when tile it on ground.
                if (tileMap.GetFrontTile(current.pos.X, current.pos.Y - 1).MaterialType != TileMaterialType.Air)
                    current.jumpValue = 0;  
            }
            else
            {
                // Is tile on ground. // Deals with corners.
                if (tileMap.GetFrontTile(current.pos.X, current.pos.Y - 1).MaterialType == TileMaterialType.Air)
                    current.jumpValue = maxDown; // Make sure character can't jump if falling.   
            }

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
            closedSet.Add(node.pos);
        }

        void SetFirstNode(Vec2i start, Vec2i end)
        {
            firstNode.parentID = -1;
            firstNode.id = 0;
            firstNode.pathCost = 0;
            firstNode.pos = start;
            firstNode.UpdateCost(end);
            AddNodeOpenList(ref firstNode);
        }
        void SetFirstNodeJumpValue(Vec2i start, ref TileMap tileMap)
        {
            // TODO -- parameterized
            const int MAX_UP = 3;
            const int MAX_DOWN = 9;
            const int MAX_JUMP = MAX_UP * 2 + 1;

            // Is tile on ground.
            if (tileMap.GetFrontTile(start.X, start.Y - 1).MaterialType != TileMaterialType.Air)
            {
                firstNode.jumpValue = 0;
                return;
            }

            // If its on the ceiling
            if (tileMap.GetFrontTile(start.X, start.Y + 1).MaterialType != TileMaterialType.Air)
            {
                firstNode.jumpValue = MAX_JUMP; // Next node needs to be down.
                return;
            }
        }
    }
}
