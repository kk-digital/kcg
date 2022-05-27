using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class SystemPlanetRenderer : MonoBehaviour
    {
        public SystemPlanet planet;

        public Sprite circle;
        public SpriteRenderer sr;
        public OrbitRenderer or;

        public Material mat;

        public Color orbitColor  = new Color(0.5f, 0.7f, 1.0f, 1.0f);
        public Color planetColor = Color.white;

        // Start is called before the first frame update
        void Start()
        {
            or = gameObject.AddComponent<OrbitRenderer>();
            sr = gameObject.AddComponent<SpriteRenderer>();

            or.descriptor = planet.Descriptor;

            // Temporary circular sprite
            circle = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        }

        // LateUpdate is called once per frame
        void LateUpdate()
        {
            float[] pos = planet.Descriptor.GetPosition();

            sr.sprite = circle;

            sr.transform.position = new Vector3(pos[0], pos[1], -0.1f);
            sr.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);

            sr.color = planetColor;
            or.color = orbitColor;

            or.UpdateRenderer(128);
        }
    }
}
