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

        public void Update()
        {
            var group = context.agent.GetGroup(AgentMatcher.Planner);
            foreach (AgentEntity entity in group)
            {
                // Get action.
                Queue<int> Actions = entity.planner.ActionIDs;
                if (Actions.Count == 0)
                {
                    GameEntity NextGoalEntity = GetNextGoal(entity);
                    if (NextGoalEntity == null)
                        continue;
                    
                    MakePlan(entity, NextGoalEntity.goal.GoalState);
                }

                //int CurrentActionID = entity.planner.RunningActionID;
                //if (CurrentActionID != -1)
                //{
                //    // Check if Action has finished.
                //    if ((DateTime.Now.Millisecond - entity.planner.ActionStartTime) >= context.game.GetEntityWithAction(CurrentActionID).action.Duration)
                //    {
                //        CurrentActionID = -1;
                //    }
                //    else
                //    {
                //        return;
                //    }
                //}

                // Get Next Action.
                int ActionID = entity.planner.ActionIDs.Dequeue();
                //entity.planner.RunningActionID = ActionID;
                //entity.planner.ActionStartTime = DateTime.Now.Millisecond;
                GameEntity ActionEntity = context.game.GetEntityWithAction(ActionID);

                // Update Agent(Testing purpose) Todo: Move this code out of planner System.
                var Effects = ActionEntity.action.Effects;
                // Todo: State class with get method.
                if (Effects.states.ContainsKey("pos"))
                    // Todo: This doen't look good. Look into how I should do this in c#.
                    entity.positionDiscrete2D.Value = (Vector2Int)ActionEntity.action.Effects.states["pos"];
                else
                    Debug.Log("There is no key called pos.");
            }
        }

        // Todo: Very Expensive call. Precalculate some node connections.
        private void MakePlan(AgentEntity entity, GoapState GoalState)
        {
            // Get List of all possible Actions.
            GameEntity[] Actions = context.game.GetGroup(GameMatcher.Action).GetEntities();

            GoapAStar goapAStar = new GoapAStar();
            if (!goapAStar.CreateActionPath(GoalState, entity.planner.CurrentWorldState, Actions, entity.planner.ActionIDs))
                Debug.Log("No available Plan");
        }

        private GameEntity GetNextGoal(AgentEntity entity)
        {
            List<int> Goals = entity.planner.GoalIDs;
            if (Goals.Count == 0)
                return null;

            GameEntity NextGoalEntity = context.game.GetEntityWithGoal(Goals[0]); ;
            foreach (int GoalID in Goals)
            {
                GameEntity GoalEntity = context.game.GetEntityWithGoal(GoalID);
                if (NextGoalEntity.goal.Priority < GoalEntity.goal.Priority)
                {
                    NextGoalEntity = GoalEntity;
                }
            }
            Goals.Remove(NextGoalEntity.goal.GoalID);
            return NextGoalEntity;
        }
    }
}
