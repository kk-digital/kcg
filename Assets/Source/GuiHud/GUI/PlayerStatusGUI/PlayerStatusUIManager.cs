using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KGUI
{
    public class PlayerStatusUIManager : GUIManager
    {
        // Health Bar
        private KGUI.PlayerStatus.HealthBarUI healthBarUI;

        // Food Bar
        private KGUI.PlayerStatus.FoodBarUI foodBarUI;

        // Water Bar
        private KGUI.PlayerStatus.WaterBarUI waterBarUI;

        // Oxygen Bar
        private KGUI.PlayerStatus.OxygenBarUI oxygenBarUI;

        // Fuel Bar
        private KGUI.PlayerStatus.FuelBarUI fuelBarUI;

        public override void Initialize(Contexts contexts, AgentEntity agentEntity)
        {
            // Health Bar Initialize
            healthBarUI = new KGUI.PlayerStatus.HealthBarUI();
            healthBarUI.Initialize(agentEntity);

            // Food Bar Initialize
            foodBarUI = new KGUI.PlayerStatus.FoodBarUI();
            foodBarUI.Initialize(agentEntity);

            // Water Bar Initialize
            waterBarUI = new KGUI.PlayerStatus.WaterBarUI();
            waterBarUI.Initialize(agentEntity);

            // Oxygen Bar Initialize
            oxygenBarUI = new KGUI.PlayerStatus.OxygenBarUI();
            oxygenBarUI.Initialize(agentEntity);

            // Oxygen Bar Initialize
            fuelBarUI = new KGUI.PlayerStatus.FuelBarUI();
            fuelBarUI.Initialize(agentEntity);
        }

        public override void Update(AgentEntity agentEntity)
        {
            //Health Bar Draw
            healthBarUI.Draw(agentEntity);

            // Food Bar Update
            foodBarUI.Update(agentEntity);

            // Water Bar Update
            waterBarUI.Update(agentEntity);

            // Fuel Bar Update
            fuelBarUI.Update(agentEntity);

            // Oxygen Bar Update
            oxygenBarUI.Update(agentEntity);

            // Water Bar UI Indicator Check
           /* if (healthBarUI.playerHealth < 50)
                Debug.Log("Health Indicator On.");
            else
                Debug.Log("Health Indicator Off.");

            // Food Bar UI Indicator Check
            if (foodBarUI.foodBar._fillValue < 0.5f)
                Debug.Log("Food Indicator On.");
            else
                Debug.Log("Food Indicator Off.");

            // Water Bar UI Indicator Check
            if (waterBarUI.waterBar._fillValue < 0.5f)
                Debug.Log("Water Indicator On.");
            else
                Debug.Log("Water Indicator Off.");

            // Fuel Bar UI Indicator Check
            if (fuelBarUI.progressBar._fillValue < 0.5f)
                Debug.Log("Fuel Indicator On.");
            else
                Debug.Log("Fuel Indicator Off.");

            // Oxygen Bar UI Indicator Check
            if (oxygenBarUI.oxygenBar._fillValue < 0.5f)
                Debug.Log("Oxygen Indicator On.");
            else
                Debug.Log("Oxygen Indicator Off.");*/
        }
    }
}
