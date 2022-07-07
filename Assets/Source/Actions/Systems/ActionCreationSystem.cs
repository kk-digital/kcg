using UnityEngine;
using System.Collections.Generic;

using Enums;
using KMath;
using Entitas;
using Unity.VisualScripting;

namespace Action
{
    public class ActionCreationSystem
    {

        private static int ActionID;

        /// <summary>
        /// Create action and schedule it. Later we will be able to create action without scheduling immediately.
        /// If actions is in cool down returns -1. 
        /// </summary>
        public int CreateAction(Contexts entitasContext, Enums.ActionType actionTypeID, int agentID)
        {
            var entityAttribute = entitasContext.actionProperties.GetEntityWithActionProperty(actionTypeID);

            ActionEntity actionEntity = entitasContext.action.CreateEntity();
            if (GameState.ActionCoolDownSystem.InCoolDown(entitasContext, actionTypeID, agentID))
            {
                return -1;
            }

            actionEntity.AddActionID(ActionID, actionTypeID);
            actionEntity.AddActionOwner(agentID);
            actionEntity.AddActionExecution(
                entityAttribute.actionPropertyFactory.ActionFactory.CreateAction(entitasContext, ActionID), 
                ActionState.Entry);

            // Maybe we should deal with time and CoolDown inside onEntry?
            if (entityAttribute.hasActionPropertyTime)
            {
                actionEntity.AddActionTime(0f);
            }
            if (entityAttribute.hasActionPropertyCoolDown)
            {
                actionEntity.AddActionBeginCoolDown(0f);
            }

            return ActionID++;
        }

        public int CreateAction(Contexts entitasContext, Enums.ActionType actionTypeID, int agentID, int itemID)
        {
            int actionID = CreateAction(entitasContext, actionTypeID, agentID);
            if (actionID != -1)
            {
                ActionEntity actionEntity = entitasContext.action.GetEntityWithActionIDID(actionID);
                actionEntity.AddActionTool(itemID);
            }
            return actionID;
        }
    }
}

