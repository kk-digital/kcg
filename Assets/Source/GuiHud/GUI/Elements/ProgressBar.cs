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

        public ProgressBar(string barName, Transform parent, Sprite barTexture, UnityEngine.UI.Image.FillMethod fillMethod, float fillValue, AgentEntity agentEntity)
        {
            _fillValue = fillValue;

            // Bar Initializon
            Bar = new GameObject(barName);
            Bar.transform.parent = parent;
            Bar.AddComponent<RectTransform>();
            Bar.AddComponent<UnityEngine.UI.Image>();
            Bar.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            Bar.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            Bar.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);

            Bar.GetComponent<UnityEngine.UI.Image>().sprite = barTexture;
            Bar.GetComponent<UnityEngine.UI.Image>().raycastTarget = true;
            Bar.GetComponent<UnityEngine.UI.Image>().maskable = true;
            Bar.GetComponent<UnityEngine.UI.Image>().type = UnityEngine.UI.Image.Type.Filled;
            Bar.GetComponent<UnityEngine.UI.Image>().fillMethod = fillMethod;
            Bar.GetComponent<UnityEngine.UI.Image>().fillOrigin = 0;
            Bar.GetComponent<UnityEngine.UI.Image>().fillAmount = fillValue;
            Bar.GetComponent<UnityEngine.UI.Image>().fillClockwise = true;
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
            Bar.GetComponent<UnityEngine.UI.Image>().fillAmount = fillValue;
        }
    }
}
