using System;
using System.Collections.Generic;
using Agent;
using Entitas;
using UnityEngine;

namespace Action
{
    // 
    public class ActionSchedulerSystem
    {
        public void Update(Contexts contexts, float deltaTime, ref Planet.PlanetState planet)
        {
            var group = contexts.agent.GetGroup(AgentMatcher.AgentActionScheduler);

            foreach (AgentEntity entity in group)
            {
                ExcuteActions(entity.agentID.ID, deltaTime, ref planet);
            }
        }

        private void ExcuteActions(int agentID, float deltaTime, ref Planet.PlanetState planet)
        {
            var actions = Contexts.sharedInstance.action.GetEntitiesWithActionOwner(agentID);

            foreach (var action in actions)
            {
                if (action.hasActionExecution)
                {
                    switch (action.actionExecution.State)
                    {
                        case Enums.ActionState.Entry:
                            action.actionExecution.Logic.OnEnter(ref planet);
                            break;
                        case Enums.ActionState.Running:
                            action.actionExecution.Logic.OnUpdate(deltaTime, ref planet);
                            break;
                        case Enums.ActionState.Success:
                            action.actionExecution.Logic.OnExit(ref planet);
                            break;
                        case Enums.ActionState.Fail:
                            action.actionExecution.Logic.OnExit(ref planet);
                            break;
                        default:
                            Debug.Log("Not valid Action state.");
                            break;
                    }
                }

                /* Code to test AIGridWorld
                 * Move this inside an action.
                if (actionDeltaTime > ActionEntity.actionTime.Duration && actionDeltaTime != -1f)
                {
                    var Effects = ActionEntity.actionGoap.Effects;

                    // Todo: Create state class with get method.
                    if (Effects.states.ContainsKey("pos"))
                        ActorEntity.ReplaceAgentPositionDiscrete2D((Vector2Int)ActionEntity.actionGoap.Effects.states["pos"], Vector2Int.zero);
                    else
                        Debug.Log("There is no key called pos.");

                    // Action Complete remove it from list.
                    ActorEntity.agentActionScheduler.ActiveActionIDs.RemoveAt(i);
                }
                */
            }
        }
        
        // Todo: find better way to get next planned actions for the AI. maybe an NextAction component?
        private void ScheduleAction(GameEntity AgentEntity)
        {
            int actionID = AgentEntity.agentAIController.ActionIDs.Dequeue();  // Get Next Action.
            ScheduleAction(actionID, AgentEntity.agentID.ID);
        }

        public void ScheduleAction(int actionID, int agentID)
        {
            ActionEntity actionEntity = Contexts.sharedInstance.action.GetEntityWithActionID(actionID);
            actionEntity.AddActionOwner(agentID);
        }

        public void ScheduleAction(Enums.ActionType type, int agentID)
        {
            ScheduleAction(GameState.ActionCreationSystem.CreateAction(type), agentID);
        }
    }
}
