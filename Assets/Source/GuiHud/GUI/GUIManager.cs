using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HUD;

namespace KGUI
{
    public class GUIManager
    {
        static PlayerStatusUIManager playerStatus = new PlayerStatusUIManager();
        static List<GUIManager> UIList = new List<GUIManager>();

        public virtual void Initialize(Contexts contexts, AgentEntity agentEntity)
        {
            // Add Elements
            UIList.Add(playerStatus);

            // Init Elements
            for(int i = 0; i < UIList.Count; i++)
            {
                UIList[i].Initialize(contexts, agentEntity);
            }
        }

        public virtual void Update(AgentEntity agentEntity)
        {
            // Update Elements
            for (int i = 0; i < UIList.Count; i++)
            {
                UIList[i].Update(agentEntity);
            }
        }
    }
}
