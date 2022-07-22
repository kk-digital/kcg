using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KGUI;
using Entitas;

namespace HUD
{
    public class HUDManager
    {
        // GUI Manager
        private GUIManager guiManager;

        // Constructor
        public HUDManager()
        {
        }

        // Constructor
        public HUDManager(Contexts contexts, AgentEntity agentEntity)
        {
            // Create GUI Manager
            guiManager = new GUIManager();

            // Initialize GUI Manager
            guiManager.Initialize(contexts, agentEntity);
        }

        public void Update(AgentEntity agentEntity)
        {
            // Update GUI
            guiManager.Update(agentEntity);
        }
    }
}
