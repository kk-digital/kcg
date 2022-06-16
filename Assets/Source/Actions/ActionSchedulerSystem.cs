using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Action
{
    // 
    public class ActionSchedulerSystem
    {
        public float CurrentTime;

        public void Update(float deltaTime)
        {
            var group = Contexts.sharedInstance.game.GetGroup(GameMatcher.AgentActionScheduler);

            foreach (GameEntity entity in group)
            {
                if (entity.agentActionScheduler.ActiveActionIDs.Count == 0 && entity.hasAgentAIController)
                {
                    if (entity.agentAIController.ActionIDs.Count == 0)
                        continue;
                    ScheduleAction(entity);
                }
                ExcuteActions(entity, deltaTime);
            }
        }

        private void ExcuteActions(GameEntity ActorEntity, float deltaTime)
        {
            for (int i = 0; i < ActorEntity.agentActionScheduler.ActiveActionIDs.Count; i++)
            {
                int actionID = ActorEntity.agentActionScheduler.ActiveActionIDs[i];

                GameEntity ActionEntity = Contexts.sharedInstance.game.GetEntityWithActionID(actionID);

                CurrentTime += deltaTime;
                float actionDeltaTime = -1f;

                if (ActionEntity.hasActionExecution)
                {
                    switch (ActionEntity.actionExecution.State)
                    {
                        case Enums.ActionState.None:
                            ActionEntity.actionExecution.Logic.OnEnter(actionID, ActorEntity.agentID.ID);
                            break;
                        case Enums.ActionState.Success:
                            ActionEntity.actionExecution.Logic.OnExit(actionID, ActorEntity.agentID.ID);
                            ActorEntity.agentActionScheduler.ActiveActionIDs.RemoveAt(i);
                            break;
                        case Enums.ActionState.Fail:
                            ActionEntity.actionExecution.Logic.OnExit(actionID, ActorEntity.agentID.ID);
                            ActorEntity.agentActionScheduler.ActiveActionIDs.RemoveAt(i);
                            break;
                        default:
                            Debug.Log("Not valid Action state.");
                            break;
                    }

                    if (ActionEntity.actionExecution.State == Enums.ActionState.Active && ActionEntity.hasActionTime)
                    {
                        // Get delta time.
                        actionDeltaTime = CurrentTime - ActionEntity.actionTime.StartTime;
                        ActionEntity.actionExecution.Logic.OnUpdate(actionID, ActorEntity.agentID.ID, actionDeltaTime);
                    }
                }

                /* Code to test AIGridWorld
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
        private void ScheduleAction(GameEntity ActorEntity)
        {
            int actionID = ActorEntity.agentAIController.ActionIDs.Dequeue();  // Get Next Action.
            ActorEntity.agentActionScheduler.ActiveActionIDs.Add(actionID);
        }

        public void ScheduleAction(GameEntity ActorEntity, int actionID)
        {
            ActorEntity.agentActionScheduler.ActiveActionIDs.Add(actionID);
        }
    }
}
