using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace AI
{
    // 
    public class ActionControllerSystem
    {
        private static PlannerSystem instance;
        public static PlannerSystem Instance => instance ??= new PlannerSystem();

        public void EmmitActions()
        { 
        
        }

        public void Update()
        {
            var group = Contexts.sharedInstance.game.GetGroup(GameMatcher.AIAgentPlanner);

            foreach (GameEntity entity in group)
            {
                if (entity.aIAgentPlanner.ActiveActionIDs.Count == 0)
                {
                    if (entity.aIAgentPlanner.ActionIDs.Count == 0)
                        continue;
                    ScheduleActions(entity);
                }

                ExcuteActions(entity);
            }
        }

        private void ExcuteActions(GameEntity ActorEntity)
        {
            for (int i = 0; i < ActorEntity.aIAgentPlanner.ActiveActionIDs.Count; i++)
            {
                ActionInfo actionInfo = ActorEntity.aIAgentPlanner.ActiveActionIDs[i];

                GameEntity ActionEntity = Contexts.sharedInstance.game.GetEntityWithAIAction(actionInfo.ActionID);

                DateTime CurrentTime = DateTime.Now;

                if ((CurrentTime - actionInfo.StartTime).TotalMilliseconds > ActionEntity.aIAction.Duration)
                {
                    var Effects = ActionEntity.aIAction.Effects;

                    // Todo: Create state class with get method.
                    if (Effects.states.ContainsKey("pos"))
                        ActorEntity.ReplaceAgentPositionDiscrete2D((Vector2Int)ActionEntity.aIAction.Effects.states["pos"], Vector2Int.zero);
                    else
                        Debug.Log("There is no key called pos.");

                    // Action Complete remove it from list.
                    ActorEntity.aIAgentPlanner.ActiveActionIDs.RemoveAt(i);
                }
            }
        }
        private void ScheduleActions(GameEntity ActorEntity)
        {
            // Get Next Action.
            int ActionID = ActorEntity.aIAgentPlanner.ActionIDs.Dequeue();
            ActorEntity.aIAgentPlanner.ActiveActionIDs.Add(new ActionInfo(ActionID));
        }
    }
}
