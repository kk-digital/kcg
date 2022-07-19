using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KGUI
{
    public static class PlayerStatusUIManager
    {
        // Health Bar
        private static KGUI.HealthBarUI healthBarUI;

        // Food Bar
        private static KGUI.FoodBarUI foodBarUI;

        // Water Bar
        private static KGUI.WaterBarUI waterBarUI;

        // Oxygen Bar
        private static KGUI.OxygenBarUI oxygenBarUI;

        // Fuel Bar
        private static KGUI.FuelBarUI fuelBarUI;

        // Entitas Contexts
        private static Contexts _contexts;

        // Agent Entity
        private static AgentEntity _agentEntity;

        public static void Initialize(Contexts contexts, AgentEntity agentEntity)
        {
            _contexts = contexts;
            _agentEntity = agentEntity;

            // Health Bar Initialize
            healthBarUI = new KGUI.HealthBarUI();
            healthBarUI.Initialize();

            // Food Bar Initialize
            foodBarUI = new KGUI.FoodBarUI();
            foodBarUI.Initialize(_contexts);

            // Water Bar Initialize
            waterBarUI = new KGUI.WaterBarUI();
            waterBarUI.Initialize(_contexts);

            // Oxygen Bar Initialize
            oxygenBarUI = new KGUI.OxygenBarUI();
            oxygenBarUI.Initialize(_contexts);

            // Oxygen Bar Initialize
            fuelBarUI = new KGUI.FuelBarUI();
            fuelBarUI.Initialize(_contexts);
        }

        public static void Update()
        {
            //Health Bar Draw
            healthBarUI.Draw(_contexts);

            // Food Bar Update
            foodBarUI.Update();

            // Water Bar Update
            waterBarUI.Update();

            // Fuel Bar Update
            fuelBarUI.Update(_agentEntity);

            // Oxygen Bar Update
            oxygenBarUI.Update();

            // Water Bar UI Indicator Check
            if (healthBarUI.playerHealth < 50)
                Debug.Log("Health Indicator On.");
            else
                Debug.Log("Health Indicator Off.");

            // Food Bar UI Indicator Check
            if (foodBarUI.foodBar.GetComponent<Image>().fillAmount < 0.5f)
                Debug.Log("Food Indicator On.");
            else
                Debug.Log("Food Indicator Off.");

            // Water Bar UI Indicator Check
            if (waterBarUI.waterBar.GetComponent<Image>().fillAmount < 0.5f)
                Debug.Log("Water Indicator On.");
            else
                Debug.Log("Water Indicator Off.");

            // Fuel Bar UI Indicator Check
            if (fuelBarUI.fuelBar.GetComponent<Image>().fillAmount < 0.5f)
                Debug.Log("Fuel Indicator On.");
            else
                Debug.Log("Fuel Indicator Off.");

            // Oxygen Bar UI Indicator Check
            if (oxygenBarUI.oxygenBar.GetComponent<Image>().fillAmount < 0.5f)
                Debug.Log("Oxygen Indicator On.");
            else
                Debug.Log("Oxygen Indicator Off.");
        }
    }
}
