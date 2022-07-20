using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KGUI;
using Entitas;

namespace HUD
{
    public class HUDManager
    {
        private GUIManager guiManager;

        public HUDManager()
        {
        }

        public HUDManager(Contexts contexts, AgentEntity agentEntity)
        {
            guiManager = new GUIManager();

            guiManager.Initialize(contexts, agentEntity);
        }

        public void Update(AgentEntity agentEntity)
        {
            guiManager.Update(agentEntity);
        }
    }
}
