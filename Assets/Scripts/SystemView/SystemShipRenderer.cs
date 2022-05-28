using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class SystemShipRenderer : MonoBehaviour
    {
        public SystemShip ship;

        public SpriteRenderer sr;
        public OrbitRenderer or;

        public Material mat;

        public Color orbitColor = new Color(1.0f, 0.7f, 0.5f, 1.0f);
        public Color shipColor = Color.white;

        // Start is called before the first frame update
        void Start()
        {
            or = gameObject.AddComponent<OrbitRenderer>();
            sr = gameObject.AddComponent<SpriteRenderer>();

            or.descriptor = ship.Descriptor;

            // Temporary circular sprite
            sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        }

        // Update is called once per frame
        void Update()
        {
            float[] pos = ship.Descriptor.GetPosition();

            sr.transform.position   = new Vector3(pos[0], pos[1], -0.1f);
            sr.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);

            sr.color = shipColor;
            or.color = orbitColor;

            or.UpdateRenderer(128);
        }
    }
}
