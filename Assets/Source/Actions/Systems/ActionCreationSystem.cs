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
            var entityAttribute = EntitasContext.actionProperties.GetEntityWithActionProperty(actionTypeID);

            ActionEntity actionEntity = EntitasContext.action.CreateEntity();
            actionEntity.AddActionExecution(
                entityAttribute.actionPropertyFactory.ActionFactory.CreateAction(entitasContext, ActionID), 
                ActionState.Entry);

            if (entityAttribute.hasActionPropertyTime)
            {
                actionEntity.AddActionTime(0f);
            }

            if()

            return ActionID++;
        }

        private void SetItem(int actionID, int itemID)
        {
            ActionEntity actionEntity = EntitasContext.action.GetEntityWithActionID(actionID);
            actionEntity.AddActionItem(itemID);
        }
    }
}

