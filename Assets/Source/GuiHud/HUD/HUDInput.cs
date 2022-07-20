using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMath;

namespace HUD
{
    public class HUDInput : HUDManager
    {
        // Cursor Screen Position from UNITY Input Class
        public Vec2f CursorPosition;

        public HUDInput(Contexts contexts, AgentEntity agentEntity)
        {

        }

        // Input Update Function
        public void Update()
        {
            // Has Mouse Moved?
            if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
            {
                // Assign New Cursor Position
                CursorPosition = new Vec2f(Input.mousePosition.x, Input.mousePosition.y);
            }
        }

        public virtual void OnMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Pressed primary button.");
            }

            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Pressed secondary button.");
            }

            if (Input.GetMouseButtonDown(2))
            {
                Debug.Log("Pressed middle click.");
            }
        }

        public virtual void OnMouseOverEnter()
        {
            
        }

        public virtual void OnMouseOverStay()
        {

        }

        public virtual void OnMouseOverExit()
        {

        }
    }
}
