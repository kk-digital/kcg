using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMath;

namespace KGUI
{
    public class GUIManager
    {
        static KGUI.PlayerStatus.FoodBarUI foodBarUI = new PlayerStatus.FoodBarUI();
        static KGUI.PlayerStatus.WaterBarUI waterBarUI = new PlayerStatus.WaterBarUI();
        static KGUI.PlayerStatus.OxygenBarUI oxygenBarUI = new PlayerStatus.OxygenBarUI();
        static KGUI.PlayerStatus.FuelBarUI fuelBarUI = new PlayerStatus.FuelBarUI();
        static KGUI.PlayerStatus.HealthBarUI healthBarUI = new PlayerStatus.HealthBarUI();

        // GUI Elements List
        static List<GUIManager> UIList = new List<GUIManager>();

        // Cursor Screen Position from Unity Input Class
        public Vec2f CursorPosition;

        // Object Screen Position
        public Vec2f ObjectPosition;

        // Run Various Functions One Time
        private bool CanRun = true;

        public virtual void Initialize(Contexts contexts, AgentEntity agentEntity)
        {
            // Add Elements
            UIList.Add(foodBarUI);
            UIList.Add(waterBarUI);
            UIList.Add(oxygenBarUI);
            UIList.Add(fuelBarUI);

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
            healthBarUI.Draw(agentEntity);
            HandleInputs(agentEntity);
        }

        public void HandleInputs(AgentEntity agentEntity)
        {
            OnMouseClick(agentEntity);
            OnMouseStay();
        }

        public virtual void OnMouseClick(AgentEntity agentEntity)
        {
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
                if(UIList[i].CanRun)
                {
                    UIList[i].CanRun = false;
                    if (Vector2.Distance(new Vector2(CursorPosition.X, CursorPosition.Y), new Vector2(UIList[i].ObjectPosition.X, UIList[i].ObjectPosition.Y)) < 20.0f)
                    {
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
                if (Vector2.Distance(new Vector2(CursorPosition.X, CursorPosition.Y), new Vector2(UIList[i].ObjectPosition.X, UIList[i].ObjectPosition.Y)) < 20.0f)
                {
                    OnMouseEnter();
                    UIList[i].OnMouseStay();
                }
                else if (Vector2.Distance(new Vector2(CursorPosition.X, CursorPosition.Y), new Vector2(UIList[i].ObjectPosition.X, UIList[i].ObjectPosition.Y)) > 30.0f)
                {
                    UIList[i].CanRun = true;
                }

            }
        }

        public virtual void OnMouseExit()
        {
            // Handle Inputs
            for (int i = 0; i < UIList.Count; i++)
            {
                if (!UIList[i].CanRun)
                {
                    UIList[i].CanRun = true;
                    if (Vector2.Distance(new Vector2(CursorPosition.X, CursorPosition.Y), new Vector2(UIList[i].ObjectPosition.X, UIList[i].ObjectPosition.Y)) > 1.0f)
                    {
                        UIList[i].OnMouseExit();
                    }
                }
            }
        }
    }
}
