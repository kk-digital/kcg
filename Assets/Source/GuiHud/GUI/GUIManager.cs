using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KGUI
{
    public class GUIManager
    {
        static PlayerStatusUIManager playerStatus = new PlayerStatusUIManager();
        static List<GUIManager> UIList = new List<GUIManager>();

        public virtual void Initialize(Contexts contexts, AgentEntity agentEntity)
        {
            UIList.Add(playerStatus);

            for(int i = 0; i < UIList.Count; i++)
            {
                UIList[i].Initialize(contexts, agentEntity);
            }
        }

        public virtual void Update(AgentEntity agentEntity)
        {
            for (int i = 0; i < UIList.Count; i++)
            {
                UIList[i].Update(agentEntity);
            }
        }
    }
}
