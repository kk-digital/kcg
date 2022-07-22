using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMath;

namespace KGUI
{
    public class GUIManager
    {
        // Food Bar
        static KGUI.PlayerStatus.FoodBarUI foodBarUI = new PlayerStatus.FoodBarUI();

        // Water Bar
        static KGUI.PlayerStatus.WaterBarUI waterBarUI = new PlayerStatus.WaterBarUI();

        // Oxygen Bar
        static KGUI.PlayerStatus.OxygenBarUI oxygenBarUI = new PlayerStatus.OxygenBarUI();

        // Fuel Bar
        static KGUI.PlayerStatus.FuelBarUI fuelBarUI = new PlayerStatus.FuelBarUI();

        // Health Bar
        static KGUI.PlayerStatus.HealthBarUI healthBarUI = new PlayerStatus.HealthBarUI();

        // GUI Elements List
        static List<GUIManager> UIList = new List<GUIManager>();

        // Cursor Screen Position from Unity Input Class
        public Vec2f CursorPosition;

        // Object Screen Position
        public Vec2f ObjectPosition;

        // Run Various Functions One Time
        private bool CanRun = true;

        // Initialize
        public virtual void Initialize(Contexts contexts, AgentEntity agentEntity)
        {
            // Add Food Bar To Draw Array
            UIList.Add(foodBarUI);

            // Add Water Bar To Draw Array
            UIList.Add(waterBarUI);

            // Add Oxygen Bar To Draw Array
            UIList.Add(oxygenBarUI);

            // Add Fuel Bar To Draw Array
            UIList.Add(fuelBarUI);

            // Add Health Bar To Draw Array
            healthBarUI.Initialize(agentEntity);

            // Init Elements
            for (int i = 0; i < UIList.Count; i++)
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

            // Assign New Cursor Position
            CursorPosition = new Vec2f(Input.mousePosition.x, Input.mousePosition.y);

            // Draw Health Bar
            healthBarUI.Draw(agentEntity);

            // Handle Inputs
            HandleInputs(agentEntity);
        }

        public void HandleInputs(AgentEntity agentEntity)
        {
            // On Mouse Click Event
            OnMouseClick(agentEntity);

            // On Mouse Stay Event
            OnMouseStay();
        }

        public virtual void OnMouseClick(AgentEntity agentEntity)
        {
            // If Mosue 0 Button Down
            if (Input.GetMouseButtonDown(0))
            {
                // Handle Inputs
                for (int i = 0; i < UIList.Count; i++)
                {
                    Debug.LogWarning("Cursor X: " + CursorPosition.X + ", Y: " + CursorPosition.Y);
                    Debug.LogWarning("Object X: " + UIList[i].ObjectPosition.X + ", Y: " + UIList[i].ObjectPosition.Y);

                    if (Vector2.Distance(new Vector2(CursorPosition.X, CursorPosition.Y), new Vector2(UIList[i].ObjectPosition.X, UIList[i].ObjectPosition.Y)) < 20.0f)
                    {
                        UIList[i].OnMouseClick(agentEntity);
                    }
                }
            }
        }

        public virtual void OnMouseEnter()
        {
            // Handle Inputs
            for (int i = 0; i < UIList.Count; i++)
            {
                // If Condition Is True
                if(UIList[i].CanRun)
                {
                    // Set Condition Bool
                    UIList[i].CanRun = false;

                    // If Cursor Is In UI Element
                    if (Vector2.Distance(new Vector2(CursorPosition.X, CursorPosition.Y), new Vector2(UIList[i].ObjectPosition.X, UIList[i].ObjectPosition.Y)) < 20.0f)
                    {
                        // Run Mouse Enter Event
                        UIList[i].OnMouseEnter();
                    }
                }
            }
        }

        public virtual void OnMouseStay()
        {
            // Handle Inputs
            for (int i = 0; i < UIList.Count; i++)
            {
                // Check If Cursor Is On UI Element
                if (Vector2.Distance(new Vector2(CursorPosition.X, CursorPosition.Y), new Vector2(UIList[i].ObjectPosition.X, UIList[i].ObjectPosition.Y)) < 20.0f)
                {
                    // Run Mouse Enter Event
                    OnMouseEnter();

                    // Call Mouse Stay Event For Each
                    UIList[i].OnMouseStay();
                }
                else if (Vector2.Distance(new Vector2(CursorPosition.X, CursorPosition.Y), new Vector2(UIList[i].ObjectPosition.X, UIList[i].ObjectPosition.Y)) > 30.0f)
                {
                    // Reset When Out Of the UI Element
                    UIList[i].CanRun = true;
                }
            }
        }

        public virtual void OnMouseExit()
        {
            // Handle Inputs
            for (int i = 0; i < UIList.Count; i++)
            {
                // If Condition Is False
                if (!UIList[i].CanRun)
                {
                    // Set Condition to True
                    UIList[i].CanRun = true;

                    // If Cursor is Out of UI Element
                    if (Vector2.Distance(new Vector2(CursorPosition.X, CursorPosition.Y), new Vector2(UIList[i].ObjectPosition.X, UIList[i].ObjectPosition.Y)) > 30.0f)
                    {
                        // On Mouse Exit
                        UIList[i].OnMouseExit();
                    }
                }
            }
        }
    }
}
