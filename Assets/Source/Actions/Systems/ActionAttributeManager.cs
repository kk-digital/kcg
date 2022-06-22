using Entitas;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;

namespace Action
{
    public class ActionAttributeManager
    {
        public GameEntity ActionAttributeEntity = null;

        public void CreateActionAttributeType(Enums.ActionType actionType, string name)
        {
            ActionAttributeEntity = Contexts.sharedInstance.game.CreateEntity();
            ActionAttributeEntity.AddActionAttribute((int)actionType);
            ActionAttributeEntity.AddActionAttributeName(name);
        }

        public void CreateActionAttributeType(Enums.ActionType actionType)
        {
            ActionAttributeEntity = Contexts.sharedInstance.game.CreateEntity();
            ActionAttributeEntity.AddActionAttribute((int)actionType);
            ActionAttributeEntity.AddActionAttributeName(actionType.ToString());
        }

        public int GetTypeID(string name)
{
            GameEntity entity = Contexts.sharedInstance.game.GetEntityWithActionAttributeName(name);
            return entity.actionAttribute.TypeID;
        }

        public void SetLogicFactory(Action.ActionCreator actionFactory)
        {
            ActionAttributeEntity.AddActionAttributeFactory(actionFactory);
        }

        public void SetTime(float duration)
        {
            ActionAttributeEntity.AddActionAttributeTime(duration);
        }

        public void SetCoolDown(float coolDown)
        {
            ActionAttributeEntity.AddActionAttributeCoolDown(coolDown);
        }

        public void SetData(object data)
        {
            ActionAttributeEntity.AddActionAttributeData(data);
        }

        public void Animation()
        { 
            ActionAttributeEntity.isActionAttributeAnimation = true;
        }

        /// <summary>
        /// Todo: Data should be in component instead of attributes. This should be an empty component.
        /// ActionAttribute that are or causes movement. Exemple: take cover.
        /// </summary>
        public void SetMoveTo(Vector2Int goalPosition)
        {
            ActionAttributeEntity.AddActionAttributeMoveTo(goalPosition);
        }

        public void SetGoap(AI.GoapState preCondition, AI.GoapState effects, int cost)
        {
            ActionAttributeEntity.AddActionAttributeGoap(preCondition, effects, cost);
        }

        public void EndActionAttributeType()
        {
            ActionAttributeEntity = null;
        }
    }
}
