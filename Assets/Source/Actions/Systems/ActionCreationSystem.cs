using UnityEngine;
using System.Collections.Generic;

using Enums;
using KMath;
using Entitas;

namespace Action
{
    public class ActionCreationSystem
    {

        private static int ActionID;

        public int CreateAction(Contexts entitasContext, int actionTypeID, int agentID)
        {
            var entityAttribute = entitasContext.actionProperties.GetEntityWithActionProperty(actionTypeID);

            ActionEntity actionEntity = entitasContext.action.CreateEntity();
            actionEntity.AddActionID(ActionID, actionTypeID);
            actionEntity.AddActionExecution(
                entityAttribute.actionPropertyFactory.ActionFactory.CreateAction(entitasContext, ActionID, agentID), 
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

            //if()

            return ActionID++;
        }

        public void SetItem(Contexts entitasContext, int actionID, int itemID)
        {
            ActionEntity actionEntity = entitasContext.action.GetEntityWithActionIDID(actionID);
            actionEntity.AddActionItem(itemID);
        }
    }
}

