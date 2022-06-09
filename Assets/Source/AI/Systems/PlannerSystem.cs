using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;


namespace AI
{
    public class PlannerSystem
    {
        private static PlannerSystem instance;
        public static PlannerSystem Instance => instance ??= new PlannerSystem();

        public Contexts context;

        public void Initialize()
        { 
            context = Contexts.sharedInstance;
        }

        public void Update() // Todo: This should not be running every frame.
        {
            var group = context.game.GetGroup(GameMatcher.AIAgentPlanner);
            foreach (GameEntity entity in group)
            {
                // Get action.
                Queue<int> Actions = entity.aIAgentPlanner.ActionIDs;
                if (Actions.Count == 0)
                {
                    GameEntity NextGoalEntity = GetNextGoal(entity);
                    if (NextGoalEntity == null)
                        continue;
                    
                    MakePlan(entity, NextGoalEntity.aIGoal.GoalState);
                }
            }
        }

        // Todo: Very Expensive call. Precalculate some node connections.
        private void MakePlan(GameEntity entity, GoapState GoalState)
        {
            // Get List of all possible Actions.
            GameEntity[] Actions = context.game.GetGroup(GameMatcher.AIAction).GetEntities();

            GoapAStar goapAStar = new GoapAStar();
            if (!goapAStar.CreateActionPath(GoalState, entity.aIAgentPlanner.CurrentWorldState, Actions, entity.aIAgentPlanner.ActionIDs))
                Debug.Log("No available Plan");
        }

        private GameEntity GetNextGoal(GameEntity entity)
        {
            List<int> Goals = entity.aIAgentPlanner.GoalIDs;
            if (Goals.Count == 0)
                return null;

            GameEntity NextGoalEntity = context.game.GetEntityWithAIGoal(Goals[0]); ;
            foreach (int GoalID in Goals)
            {
                GameEntity GoalEntity = context.game.GetEntityWithAIGoal(GoalID);
                if (NextGoalEntity.aIGoal.Priority < GoalEntity.aIGoal.Priority)
                {
                    NextGoalEntity = GoalEntity;
                }
            }
            Goals.Remove(NextGoalEntity.aIGoal.GoalID);
            return NextGoalEntity;
        }
    }
}
