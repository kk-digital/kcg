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

        public Node                 Parent;

        public readonly GoapState   WorldState;
        public int                  LinkedActionID;

        public int                  PathCost;
        public int                  HeuristicCost;
        public int                  TotalCost;
    }
    // Todo: update this for new action system.

    /*
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
                if (!ActionsList[i].actionGoap.Effects.MatchCondition(Parent.WorldState))
                {
                    continue;
                }

                NeighborActions.Add(ActionsList[i]);
            }

            // Add nodes to OpenLsit.
            foreach (GameEntity action in NeighborActions)
            {
                GoapState NewWorldState = new GoapState(new Dictionary<string, object>());
                NewWorldState = GoapState.ApplyEffect(Parent.WorldState, action.actionGoap.PreConditions);

                // Check If goal was reached.
                if (GoalState.MatchCondition(NewWorldState))
                {
                    ListofActions.Enqueue(action.actionID.ID);
                    Node node = Parent;
                    while (node != RootNode)
                    {
                        ListofActions.Enqueue(node.LinkedActionID);
                        node = node.Parent;
                    }
                    return true;
                }

                int NewPathCost = Parent.PathCost + action.actionGoap.Cost;

                if (IsNodeOnList(OpenList, NewWorldState, NewPathCost, action.actionID.ID))
                    continue;
                if (IsNodeOnList(ClosedList, NewWorldState, NewPathCost, action.actionID.ID))
                    continue;

                // Add new node to OpenList.
                OpenList.Add(new Node(Parent, NewWorldState, action.actionID.ID,
                        NewPathCost, GetHeuristicWeight(NewWorldState, GoalState)));
            }

            // Choose cheapest node in OpenList
            Node NextNode = null;
            int Cost = int.MaxValue;
            int NodeIndex = -1;
            for (int i = 0; i < OpenList.Count; i++)
            {
                // Choose Cheapest Node in OpenList.
                Node NodeIt = OpenList[i];
                if (NodeIt.TotalCost < Cost)
                {
                    NextNode = NodeIt;
                    Cost = NodeIt.TotalCost;
                    NodeIndex = i;
                }
            }

            if (NextNode != null)
            {
                OpenList.RemoveAt(NodeIndex);
                ClosedList.Add(NextNode);
                return CreateActionPath(GoalState, NextNode, ListofActions);
            }
            
            return false;
        }

        private bool IsNodeOnList(List<Node> List, GoapState WorldState, int PathCost, int ActionID)
        {
            foreach (Node NodeIt in OpenList)
            {
                if (NodeIt.WorldState == WorldState)
                {
                    if (NodeIt.PathCost > PathCost)
                    {
                        NodeIt.PathCost = PathCost;
                        NodeIt.TotalCost = PathCost + NodeIt.HeuristicCost;
                        NodeIt.LinkedActionID = ActionID;
                    }
                    return true;
                }

            }
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
    }  */
}
