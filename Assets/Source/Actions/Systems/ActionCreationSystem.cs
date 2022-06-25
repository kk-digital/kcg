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
            var entityAttribute = EntitasContext.game.GetEntityWithActionAttribute(actionTypeID);

            var actionEntity = EntitasContext.game.CreateEntity();
            actionEntity.AddActionID(ActionID, actionTypeID);
            actionEntity.AddActionExecution(
                entityAttribute.actionAttributeFactory.ActionFactory.CreateAction(ActionID, agentID), 
                ActionState.Entry);

            // Maybe we should deal with time and CoolDown inside onEntry?
            if (entityAttribute.hasActionAttributeTime)
            {
                actionEntity.AddActionTime(0f);
            }
            if (entityAttribute.hasActionAttributeCoolDown)
            {
                actionEntity.AddActionBeginCoolDown(0f);
            }

            return ActionID++;
        }

        public void SetItem(int actionID, int itemID)
        {
            GameEntity actionEntity = EntitasContext.game.GetEntityWithActionIDID(actionID);
            actionEntity.AddActionItem(itemID);
        }
    }
}

