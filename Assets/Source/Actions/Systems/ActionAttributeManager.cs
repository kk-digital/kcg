using Entitas;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;

namespace Action
{
    public class ActionPropertyManager
    {
        public ActionPropertiesEntity ActionPropertyEntity = null;

        public void CreateActionPropertyType(Contexts contexts, Enums.ActionType actionType, string name)
        {
            ActionPropertyEntity = contexts.actionProperties.CreateEntity();
            ActionPropertyEntity.AddActionProperty(actionType);
            ActionPropertyEntity.AddActionPropertyName(name);
        }

        public void CreateActionPropertyType(Contexts contexts, Enums.ActionType actionType)
        {
            ActionPropertyEntity = contexts.actionProperties.CreateEntity();
            ActionPropertyEntity.AddActionProperty(actionType);
            ActionPropertyEntity.AddActionPropertyName(actionType.ToString());
        }

        public Enums.ActionType GetTypeID(Contexts contexts, string name)
{
            ActionPropertiesEntity entity = contexts.actionProperties.GetEntityWithActionPropertyName(name);
            return entity.actionProperty.TypeID;
        }

        public void SetLogicFactory(Action.ActionCreator actionFactory)
        {
            ActionPropertyEntity.AddActionPropertyFactory(actionFactory);
        }

        public void SetTime(float duration)
        {
            ActionPropertyEntity.AddActionPropertyTime(duration);
        }

        public void SetCoolDown(float coolDown)
        {
            ActionPropertyEntity.AddActionPropertyCoolDown(coolDown);
        }

        public void SetShieldActive(bool ShieldActive)
        {
            ActionPropertyEntity.AddActionPropertyShield(ShieldActive);
        }

        public void SetData(object data)
        {
            ActionPropertyEntity.AddActionPropertyData(data);
        }

        public void Animation()
        { 
            ActionPropertyEntity.isActionPropertyAnimation = true;
        }

        /// <summary>
        /// Todo: Data should be in component instead of attributes. This should be an empty component.
        /// ActionProperty that are or causes movement. Exemple: take cover.
        /// </summary>
        public void Movement()
        {
            ActionPropertyEntity.isActionPropertyMovement = true;
        }

        public void SetGoap(AI.GoapState preCondition, AI.GoapState effects, int cost)
        {
            ActionPropertyEntity.AddActionPropertyGoap(preCondition, effects, cost);
        }

        public void EndActionPropertyType()
        {
            ActionPropertyEntity = null;
        }
    }
}
