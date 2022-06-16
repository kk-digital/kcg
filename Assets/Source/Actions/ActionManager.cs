using Entitas;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Action
{
    public class ActionManager
    {
        public GameEntity ActionEntity = null;
        private int CurrentID = 0;

        public void CreateAction()
        {
            ActionEntity = Contexts.sharedInstance.game.CreateEntity();
            ActionEntity.AddActionID(CurrentID);
        }

        public int GetCurrentID()
        {
            return CurrentID;
        }
        public void SetExecution(ActionBase actionLogic)
        {
            ActionEntity.AddActionExecution(actionLogic, Enums.ActionState.None);
        }

        public void SetTime(float duration)
        {
            ActionEntity.AddActionTime(duration, -1);
        }

        public void SetCoolDown(float coolDown)
        {
            ActionEntity.AddActionCoolDown(coolDown);
        }

        public void Animation()
        { 
            ActionEntity.isActionAnimation = true;
        }

        /// <summary>
        /// Action that are or causes movement. Exemple: take cover.
        /// </summary>
        public void SetMoveTo(Vector2Int goalPosition)
        {
            ActionEntity.AddActionMoveTo(goalPosition);
        }

        public void SetGoap(AI.GoapState preCondition, AI.GoapState effects, int cost)
        {
            ActionEntity.AddActionGoap(preCondition, effects, cost);
        }

        public void EndAction()
        {
            ActionEntity = null;
            CurrentID++;
        }
    }
}
