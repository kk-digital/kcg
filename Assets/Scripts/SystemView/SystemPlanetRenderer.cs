using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class SystemPlanetRenderer : MonoBehaviour
    {
        public SystemPlanet planet;

        public LineRenderer linerenderer;
        public Sprite circle;
        public SpriteRenderer sr;

        public Material mat;

        public Color orbitColor = Color.white;
        public Color planetColor = Color.white;

        private void UpdateRenderer(int segments)
        {
            Vector3[] ellipsevertices = new Vector3[segments];

            float angle = 2.0f * 3.1415926f / (float)segments;

            // sine and cosine of the relative angle between each segment
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            // sine and cosine of the rotation
            float rotsin = (float)Math.Sin(planet.Rotation);
            float rotcos = (float)Math.Cos(planet.Rotation);

            // eccentricity
            float c = planet.GetEccentricDistance();

            float x = 1.0f;
            float y = 0.0f;

            for (int i = 0; i < segments; i++)
            {
                float vx = x * planet.SemiMajorAxis - c;
                float vy = y * planet.SemiMinorAxis;

                ellipsevertices[i] = new Vector3(
                    rotcos * vx - rotsin * vy + planet.CenterX,
                    rotsin * vx + rotcos * vy + planet.CenterY,
                    0.0f
                );

                (x, y) = (cos * x - sin * y, sin * x + cos * y);
            }

            linerenderer.startColor = linerenderer.endColor = orbitColor;
            linerenderer.SetPositions(ellipsevertices);
            linerenderer.positionCount = segments;

            float[] pos = planet.GetPosition();

            sr.sprite = circle;

            sr.transform.position = new Vector3(pos[0], pos[1], -0.1f);
            sr.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);

            sr.color = planetColor;
        }

        // Start is called before the first frame update
        void Start()
        {
            linerenderer = gameObject.AddComponent<LineRenderer>();
            sr = gameObject.AddComponent<SpriteRenderer>();

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

            circle = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        }

        // Update is called once per frame
        void Update()
        {
            UpdateRenderer(128);
        }
    }
}
