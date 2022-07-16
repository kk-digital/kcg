using System;
using System.Collections.Generic;
using KMath;
using PlanetTileMap;
using UnityEngine;

namespace AI.Movement
{
    internal struct Node
    {
        public int id;
        public int parentID;
        public Vec2f pos;

        public void SetPos(Vec2f newPos, Vec2f end)
        {
            pos = newPos;
            heuristicCost = (int)Heuristics.euclidean_distance(newPos, end) * 100;
        }

        public int pathCost;
        public int heuristicCost
        {
            get { return heuristicCost; }
            private set
            {
                heuristicCost = value;
                totalCost = heuristicCost + pathCost;
            }
        }
        public int totalCost { get; private set; }

        public bool IsCheaper(ref Node node)
        {
            return totalCost < node.totalCost ? true : false;
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

        int openLenght;
        int closedLenght;

        Node[] openList;
        Node[] closedList;
        Node firstNode;

        HashSet<Vec2f> openSet;
        HashSet<Vec2f> closedSet;

        bool Passable()
        {
            return true;
        }

        /// <summary>
        /// Uses insertion sort. It's the fastest algorithm for almost sorted data.
        /// startPos is an optimization. Every element before startPos is sorted.
        /// </summary>
        void SortOpenList(int startPos)
        {
            for (int i = startPos; i < openLenght; i++)
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

        void Initialize()
        {
            openLenght = 0;
            closedLenght = 0;

            openList = new Node[MAX_NUM_NODES];
            closedList = new Node[MAX_NUM_NODES];
            firstNode = new Node();

            openSet.EnsureCapacity(MAX_NUM_NODES);
            closedSet.EnsureCapacity(MAX_NUM_NODES);
        }

        void AddNodeOpenList(ref Node node)
        {
            openList[openLenght++] = node;
            openSet.Add(firstNode.pos);
        }

        void AddNodeCloseList(ref Node node)
        {
            closedList[closedLenght++] = node;
            closedSet.Add(firstNode.pos);
        }

        void SetFirstNode(Vec2f start, Vec2f end)
        {
            firstNode.parentID = -1;
            firstNode.id = 1;
            firstNode.pathCost = 0;
            firstNode.SetPos(start, end);
            AddNodeOpenList(ref firstNode);
        }

        public Vec2f[] getPath(ref TileMap tileMap, Vec2f start, Vec2f end)
        {
            Initialize();

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
                if (openLenght >= MAX_NUM_NODES || closedLenght >= MAX_NUM_NODES)
                {
                    Debug.Log("The path is taking too long. Giving up.");
                    break;
                }

                // We failed to find a path if open list is empty.
                if (openSet.Count == 0)
                {
                    Debug.Log("Couldn't find a path to the destination.");
                    break;
                }

                if(sortStartPost > 0)
                {
                    SortOpenList(sortStartPost);
                    sortStartPost = 0;
                }

                // Move to closed list.
                ref Node current = ref openList[--openLenght];
                openSet.Remove(current.pos);

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

                    if (!Passable())
                    { }

                    if (closedSet.Contains(node.pos))
                        continue;

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

                    openSet.Add(node.pos);
                    sortStartPost = openLenght - 1;
                }
            }

            return new Vec2f[10];
        }
    }
}
