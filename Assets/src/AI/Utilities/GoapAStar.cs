using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace AI
{
    internal class Node
    {
        public Node(Node parent, GoapState states, int ActionID, int PCost, int HCost)
        {
            Parent = parent;
            WorldState = states;
            LinkedActionID = ActionID;
            PathCost = PCost;
            HeuristicCost = HCost;
            TotalCost = PathCost + HeuristicCost;
        }

        public Node Parent;

        public readonly GoapState WorldState;
        public int LinkedActionID;

        public int PathCost;
        public int HeuristicCost;
        public int TotalCost;
    }
    public class GoapAStar
    {
        List<Node> OpenList = new List<Node>();
        List<Node> ClosedList = new List<Node>();
        Node RootNode;
        GameEntity[] ActionsList;

        public bool CreateActionPath(GoapState WorldState, GoapState GoalState, GameEntity[] Actions, 
            Queue<int> ListofActions)
        {
            ActionsList = Actions;
            RootNode = new Node(null, WorldState, -1, 0, 0);
            return CreateActionPath(GoalState, RootNode, ListofActions);
        }

        private bool CreateActionPath(GoapState GoalState, Node Parent, Queue<int> ListofActions)
        {
            // Get List of connections to Neighbor states.
            List<GameEntity> NeighborActions = new List<GameEntity>();

            for (int i = 0; i < ActionsList.Length; i++)
            {
                if (!ActionsList[i].action.Effects.MatchCondition(Parent.WorldState))
                {
                    continue;
                }

                NeighborActions.Add(ActionsList[i]);
            }

            // Add nodes to OpenLsit.
            foreach (GameEntity action in NeighborActions)
            {
                GoapState NewWorldState = new GoapState(new Dictionary<string, object>());
                NewWorldState = GoapState.ApplyEffect(Parent.WorldState, action.action.PreConditions);
                if (GoalState.MatchCondition(NewWorldState)) // Check If goal was reached.
                {
                    ListofActions.Enqueue(action.action.ActionID);
                    Node node = Parent;
                    while (node != RootNode)
                    {
                        ListofActions.Enqueue(node.LinkedActionID);
                        node = node.Parent;
                    }
                    return true;
                }

                int NewPathCost = Parent.PathCost + action.action.Cost;

                // Check if node in this position Exist.
                bool HasNode = false;
                foreach (Node NodeIt in OpenList)
                {
                    if (NodeIt.WorldState == NewWorldState)
                    {
                        HasNode = true;
                        if (NodeIt.PathCost > NewPathCost)
                        {
                            NodeIt.PathCost = NewPathCost;
                            NodeIt.TotalCost = NewPathCost + NodeIt.HeuristicCost;
                            NodeIt.LinkedActionID = action.action.ActionID;
                        }
                        break;
                    }

                }

                foreach (Node NodeIt in ClosedList)
                {
                    if (NodeIt.WorldState == NewWorldState)
                    {
                        HasNode = true;
                        if (NodeIt.PathCost > NewPathCost)
                        {
                            NodeIt.PathCost = NewPathCost;
                            NodeIt.TotalCost = NewPathCost + NodeIt.HeuristicCost;
                            NodeIt.LinkedActionID = action.action.ActionID;
                        }
                        break;
                    }
                }

                // Add List in OpenList
                if (!HasNode)
                {
                    Node node = new Node(Parent, NewWorldState, action.action.ActionID, 
                        NewPathCost, GetHeuristicWeight(NewWorldState, GoalState));
                    OpenList.Add(node);
                }
            }

            // Choose cheapest node in OpenList
            Node NextNode = null;
            int Cost = int.MaxValue;
            foreach (Node NodeIt in OpenList)
            {
                // Choose Cheapest Node in OpenList.
                if (NodeIt.TotalCost < Cost)
                {
                    NextNode = NodeIt;
                    Cost = NodeIt.TotalCost;
                }
            }

            OpenList.Remove(NextNode);
            ClosedList.Add(NextNode);

            if (NextNode != null)
                CreateActionPath(GoalState, NextNode, ListofActions);
            

            return false;
        }

        private int GetHeuristicWeight(GoapState WorldState, GoapState GoalState)
        {
            GoapState MissingStates = GoalState.GetMissing(WorldState);

            int cost = 0;
            foreach (var state in MissingStates.states)
            {
                if (state.Value is Vector2Int) // Todo: Expensive temporary solution.
                {
                    Vector2Int tempSolution = new Vector2Int();
                    tempSolution = (Vector2Int)state.Value - (Vector2Int)WorldState.states[state.Key];
                    cost = Math.Abs(tempSolution.x) + Math.Abs(tempSolution.y);
                }
                else
                {
                    cost++;
                }
            }
            return cost;
        }
    }   
}
