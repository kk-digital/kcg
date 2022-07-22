using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KGUI.Elements
{
    public class Image
    {
        // Image Gameobject
        private GameObject iconCanvas;

        // Constructor
        public Image(string imageName, Sprite image)
        {
            // Create Gameobject
            iconCanvas = new GameObject(imageName);

            // Set Object Parent To Canvas
            iconCanvas.transform.parent = GameObject.Find("Canvas").transform;

            // Add Rect Transform to Manage UI Scaling
            iconCanvas.AddComponent<RectTransform>();

            // Add Image Component to Render the Sprite
            iconCanvas.AddComponent<UnityEngine.UI.Image>();

            // Set Image Sprite
            iconCanvas.GetComponent<UnityEngine.UI.Image>().sprite = image;

            // Set Anchor Min
            iconCanvas.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);

            // Set Anchor Max
            iconCanvas.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);

            // Set Pivot
            iconCanvas.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        }

        public void SetPosition(Vector3 newPos)
        {
            // Set Local Rect Position
            iconCanvas.GetComponent<RectTransform>().localPosition = newPos;
        }

        public void SetRotation(Quaternion newRot)
        {
            // Set Local Rect Rotation
            iconCanvas.GetComponent<RectTransform>().localRotation = newRot;
        }

        public void SetScale(Vector3 newScale)
        {
            // Set Local Rect Scale
            iconCanvas.GetComponent<RectTransform>().localScale = newScale;
        }

        public Transform GetTransform()
        {
            // Return Transform
            return iconCanvas.transform;
        }
    }
}
