using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KGUI.Elements
{
    public class Image
    {
        private GameObject iconCanvas;

        public Image(string imageName, Sprite image)
        {
            // Food Bar Initializon
            iconCanvas = new GameObject(imageName);
            iconCanvas.transform.parent = GameObject.Find("Canvas").transform;
            iconCanvas.AddComponent<RectTransform>();
            iconCanvas.AddComponent<UnityEngine.UI.Image>();
            iconCanvas.GetComponent<UnityEngine.UI.Image>().sprite = image;
            iconCanvas.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            iconCanvas.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            iconCanvas.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        }

        public void SetPosition(Vector3 newPos)
        {
            iconCanvas.GetComponent<RectTransform>().localPosition = newPos;
        }

        public void SetRotation(Quaternion newRot)
        {
            iconCanvas.GetComponent<RectTransform>().localRotation = newRot;
        }

        public void SetScale(Vector3 newScale)
        {
            iconCanvas.GetComponent<RectTransform>().localScale = newScale;
        }

        public Transform GetTransform()
        {
            return iconCanvas.transform;
        }
    }
}
