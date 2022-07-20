using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KGUI.Elements
{
    public class ProgressBar
    {
        private GameObject Bar;
        public float _fillValue;

        public ProgressBar(string barName, Transform parent, Sprite barTexture, Image.FillMethod fillMethod, float fillValue, AgentEntity agentEntity)
        {
            _fillValue = fillValue;

            // Bar Initializon
            Bar = new GameObject(barName);
            Bar.transform.parent = parent;
            Bar.AddComponent<RectTransform>();
            Bar.AddComponent<Image>();

            Bar.GetComponent<Image>().sprite = barTexture;
            Bar.GetComponent<Image>().raycastTarget = true;
            Bar.GetComponent<Image>().maskable = true;
            Bar.GetComponent<Image>().type = Image.Type.Filled;
            Bar.GetComponent<Image>().fillMethod = fillMethod;
            Bar.GetComponent<Image>().fillOrigin = 0;
            Bar.GetComponent<Image>().fillAmount = fillValue;
            Bar.GetComponent<Image>().fillClockwise = true;
        }

        public void SetPosition(Vector3 newPos)
        {
            Bar.GetComponent<RectTransform>().localPosition = newPos;
        }

        public void SetRotation(Quaternion newRot)
        {
            Bar.GetComponent<RectTransform>().localRotation = newRot;
        }

        public void SetScale(Vector3 newScale)
        {
            Bar.GetComponent<RectTransform>().localScale = newScale;
        }

        public void Update(float fillValue)
        {
            _fillValue = fillValue;
            Bar.GetComponent<Image>().fillAmount = fillValue;
        }
    }
}
