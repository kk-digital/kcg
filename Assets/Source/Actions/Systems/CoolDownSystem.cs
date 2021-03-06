using Entitas;
using UnityEngine;

namespace Action
{
    public class CoolDownSystem
    {
        float currentTime;

        public void SetCoolDown(Contexts contexts, Enums.ActionType type, int agentID, float time)
        {
            var entity = contexts.actionCoolDown.CreateEntity();
            entity.AddActionCoolDown(type, agentID);
            entity.AddActionCoolDownTime(currentTime + time);
        }

        public void SetCoolDown(Contexts contexts, Enums.ActionType type, int agentID)
        {
            float time = contexts.actionProperties.GetEntityWithActionProperty(type).actionPropertyCoolDown.CoolDownTime;
            SetCoolDown(contexts, type, agentID, time);
        }

        public bool InCoolDown(Contexts contexts, Enums.ActionType type, int agentID)
        {
            var coolDownList = contexts.actionCoolDown.GetEntitiesWithActionCoolDownAgentID(agentID);
            foreach (var coolDown in coolDownList)
            {
                if (coolDown.actionCoolDown.TypeID == type)
                    return true;
            }

            return false;
        }

        public void Update(Contexts contexts, float deltaTime)
        {
            currentTime += deltaTime;

            ActionCoolDownEntity[] coolDownList = contexts.actionCoolDown.GetEntities();
            for (int i = 0; i < coolDownList.Length; i++)
            {
                if (coolDownList[i].actionCoolDownTime.EndTime < currentTime)
                {
                    coolDownList[i].Destroy();
                }
            }

        }
    }
}
