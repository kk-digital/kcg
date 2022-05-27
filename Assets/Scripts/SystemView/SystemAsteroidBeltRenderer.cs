using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class SystemAsteroidBeltRenderer : MonoBehaviour
    {
        public SystemAsteroidBelt belt;

        public OrbitRenderer or;

        public Material mat;

        public Color orbitColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

        // Start is called before the first frame update
        void Start()
        {
            or = gameObject.AddComponent<OrbitRenderer>();

            or.descriptor = belt.Descriptor;
        }

        // LateUpdate is called once per frame
        void LateUpdate()
        {
            or.color = orbitColor;
            or.LineWidth = belt.BeltWidth;
            or.UpdateRenderer(128);
        }
    }
}
