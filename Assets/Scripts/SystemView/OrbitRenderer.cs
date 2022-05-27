using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class OrbitRenderer : MonoBehaviour
    {
        private LineRenderer linerenderer;

        public OrbitingObjectDescriptor descriptor;

        public Material mat;

        public Color color = new Color(0.5f, 0.7f, 1.0f, 1.0f);

        public void UpdateRenderer(int segments)
        {
            Vector3[] vertices = new Vector3[segments];

            float angle = 2.0f * 3.1415926f / (float)segments;

            // sine and cosine of the relative angle between each segment
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            // sine and cosine of the rotation
            float rotsin = (float)Math.Sin(descriptor.Rotation);
            float rotcos = (float)Math.Cos(descriptor.Rotation);

            // eccentricity
            float c = descriptor.GetEccentricDistance();

            float x = 1.0f;
            float y = 0.0f;

            for (int i = 0; i < segments; i++)
            {
                float vx = x * descriptor.SemiMajorAxis - c;
                float vy = y * descriptor.SemiMinorAxis;

                vertices[i] = new Vector3(
                    rotcos * vx - rotsin * vy + descriptor.CenterX,
                    rotsin * vx + rotcos * vy + descriptor.CenterY,
                    0.0f
                );

                (x, y) = (cos * x - sin * y, sin * x + cos * y);
            }

            linerenderer.startColor = linerenderer.endColor = color;
            linerenderer.SetPositions(vertices);
            linerenderer.positionCount = segments;
        }

        // Start is called before the first frame update
        void Start()
        {
            linerenderer = gameObject.AddComponent<LineRenderer>();

            // Load unity test shader
            // this could alternatively also be our GLTestShader

            Shader shader = Shader.Find("Hidden/Internal-Colored");
            mat = new Material(shader);
            mat.hideFlags = HideFlags.HideAndDontSave;

            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            // Turn off backface culling, depth writes, depth test.
            mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            mat.SetInt("_ZWrite", 0);
            mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

            linerenderer.material = mat;

            linerenderer.startWidth = 0.1f;
            linerenderer.endWidth = 0.1f;

            linerenderer.useWorldSpace = true;

            linerenderer.loop = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
