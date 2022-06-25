using UnityEngine;
using System.Collections.Generic;

using Enums;
using KMath;
using Entitas;

namespace Action
{
    public class ActionCreationSystem
    {
        public Contexts EntitasContext;

        private static int ActionID;

        public ActionCreationSystem()
        {
            EntitasContext = Contexts.sharedInstance;
        }

        public int CreateAction(int actionTypeID, int agentID)
        {
            var entityAttribute = EntitasContext.actionProperties.GetEntityWithActionProperty(actionTypeID);

            ActionEntity actionEntity = EntitasContext.action.CreateEntity();
            actionEntity.AddActionID(ActionID, actionTypeID);
            actionEntity.AddActionExecution(
                entityAttribute.actionPropertyFactory.ActionFactory.CreateAction(ActionID, agentID), 
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

        public void SetItem(int actionID, int itemID)
        {
            ActionEntity actionEntity = EntitasContext.action.GetEntityWithActionIDID(actionID);
            actionEntity.AddActionItem(itemID);
        }
    }
}

