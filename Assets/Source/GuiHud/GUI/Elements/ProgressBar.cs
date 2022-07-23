using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KGUI.Elements
{
    public class ProgressBar
    {
        // Bar Gameobject
        private GameObject Bar;

        // Bar Fill Value
        public float _fillValue;

        // Constructor
        public ProgressBar(string barName, Transform parent, Sprite barTexture, UnityEngine.UI.Image.FillMethod fillMethod, float fillValue, AgentEntity agentEntity)
        {
            // Set Fill Value
            _fillValue = fillValue;

            // Create Gameobject
            Bar = new GameObject(barName);

            // Set Parent
            Bar.transform.parent = parent;

            // Add Rect Transform Component
            Bar.AddComponent<RectTransform>();

            // Add Image Component
            Bar.AddComponent<UnityEngine.UI.Image>();

            // Set Anchor Min
            Bar.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);

            // Set Anchor Max
            Bar.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);

            // Set Pivot
            Bar.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

            // Set Sprite
            Bar.GetComponent<UnityEngine.UI.Image>().sprite = barTexture;

            // Set Raycast Target
            Bar.GetComponent<UnityEngine.UI.Image>().raycastTarget = true;

            // Set Maskable
            Bar.GetComponent<UnityEngine.UI.Image>().maskable = true;

            // Set Image Type
            Bar.GetComponent<UnityEngine.UI.Image>().type = UnityEngine.UI.Image.Type.Filled;

            // Set Fill Method
            Bar.GetComponent<UnityEngine.UI.Image>().fillMethod = fillMethod;

            // Set Fill Origin
            Bar.GetComponent<UnityEngine.UI.Image>().fillOrigin = 0;

            // Set Fill Value
            Bar.GetComponent<UnityEngine.UI.Image>().fillAmount = fillValue;

            // Set Fil Clockwise
            Bar.GetComponent<UnityEngine.UI.Image>().fillClockwise = true;
        }

        public void SetPosition(Vector3 newPos)
        {
            // Set Position
            Bar.GetComponent<RectTransform>().localPosition = newPos;
        }

        public void SetRotation(Quaternion newRot)
        {
            // Set Rotation
            Bar.GetComponent<RectTransform>().localRotation = newRot;
        }

        public void SetScale(Vector3 newScale)
        {
            // Set Scale
            Bar.GetComponent<RectTransform>().localScale = newScale;
        }

        public void Update(float fillValue)
        {
            // Update Fill Value
            _fillValue = fillValue;

            // Update Fill Value
            Bar.GetComponent<UnityEngine.UI.Image>().fillAmount = fillValue;
        }
    }
}
