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

        public int CreateAction(Contexts entitasContext, Enums.ActionType actionTypeID)
        {
            var entityAttribute = Contexts.sharedInstance.actionProperties.GetEntityWithActionProperty(actionTypeID);

            ActionEntity actionEntity = Contexts.sharedInstance.action.CreateEntity();
            actionEntity.AddActionExecution(
                entityAttribute.actionPropertyFactory.ActionFactory.CreateAction(ActionID), 
                ActionState.Entry);

            if (entityAttribute.hasActionPropertyTime)
            {
                actionEntity.AddActionTime(0f);
            }

            return ActionID++;
        }

        private void SetItem(int actionID, int itemID)
        {
            ActionEntity actionEntity = Contexts.sharedInstance.action.GetEntityWithActionID(actionID);
            actionEntity.AddActionItem(itemID);
        }
    }
}

