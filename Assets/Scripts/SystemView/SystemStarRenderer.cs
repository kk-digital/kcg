using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class SystemStarRenderer : MonoBehaviour
    {
        public SystemStar Star;

        public SpriteRenderer sr;

        public Color color = Color.white;

        // Start is called before the first frame update
        void Start()
        {
            sr = gameObject.AddComponent<SpriteRenderer>();

            // Temporary circular sprite
            sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        }

        // Update is called once per frame
        void Update()
        {
            sr.transform.position = new Vector3(Star.PosX, Star.PosY, -0.1f);
            sr.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);

            sr.color = color;
        }
    }
}
