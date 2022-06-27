using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Action
{
    public class CoolDownSystem
    {
        float currentTime;

        public void SetCoolDown(Enums.ActionType type, int agentID, float time)
        {
            var entity = Contexts.sharedInstance.actionCoolDown.CreateEntity();
            entity.AddActionCoolDown(type, agentID);
            entity.AddActionCoolDownTime(currentTime + time);
        }

        public void SetCoolDown(Enums.ActionType type, int agentID)
        {
            float time = Contexts.sharedInstance.actionProperties.GetEntityWithActionProperty(type).actionPropertyCoolDown.CoolDownTime;
            SetCoolDown(type, agentID, time);
        }

        public bool InCoolDown(Enums.ActionType type, int agentID)
        {
            var coolDownList = Contexts.sharedInstance.actionCoolDown.GetEntitiesWithActionCoolDownAgentID(agentID);
            foreach (var coolDown in coolDownList)
            {
                if (coolDown.actionCoolDown.TypeID == type)
                    return true;
            }

            return false;
        }

        public void Update(float deltaTime)
        {
            currentTime += deltaTime;

            var coolDownList = Contexts.sharedInstance.actionCoolDown.GetGroup(ActionCoolDownMatcher.ActionCoolDown);
            foreach (var coolDown in coolDownList)
            {
                if (coolDown.actionCoolDownTime.EndTime > currentTime)
                {
                    coolDown.Destroy();
                }
            }

        }
    }
}
