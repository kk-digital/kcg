using System;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class OrbitRenderer : MonoBehaviour {
            private LineRenderer linerenderer;

            public OrbitingObjectDescriptor descriptor;

            public Material mat;

            public Color color = new Color(0.5f, 0.7f, 1.0f, 1.0f);

            public float LineWidth = 0.1f;

            public CameraController Camera;

            public void UpdateRenderer(int segments) {
                if(linerenderer == null) return;
                if(descriptor == null) {
                    linerenderer.startWidth = linerenderer.endWidth = 0.0f;
                    return;
                }

                Vector3[] vertices = new Vector3[segments];

                float angle = 2.0f * 3.1415926f / (float)segments;

                // sine and cosine of the relative angle between each segment
                float sin = (float)Math.Sin(angle);
                float cos = (float)Math.Cos(angle);

                // sine and cosine of the rotation
                float rotsin = (float)Math.Sin(descriptor.rotation);
                float rotcos = (float)Math.Cos(descriptor.rotation);

                // eccentricity
                float c = descriptor.linear_eccentricity;

                float x = 1.0f;
                float y = 0.0f;

                for(int i = 0; i < segments; i++) {
                    float vx = x * descriptor.semimajoraxis - c;
                    float vy = y * descriptor.semiminoraxis;

                    vertices[i] = new Vector3(
                        rotcos * vx - rotsin * vy + descriptor.central_body.posx,
                        rotsin * vx + rotcos * vy + descriptor.central_body.posy,
                        0.0f
                    );

                    (x, y) = (cos * x - sin * y, sin * x + cos * y);
                }

                linerenderer.startWidth = linerenderer.endWidth = LineWidth == 0.1f ? LineWidth / Camera.scale : LineWidth;
                linerenderer.startColor = linerenderer.endColor = color;
                linerenderer.SetPositions(vertices);
                linerenderer.positionCount = segments;
            }

            // Start is called before the first frame update
            void Start() {
                Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

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

                linerenderer.useWorldSpace = true;

                linerenderer.loop = true;
            }

            void OnDestroy() {
                GameObject.Destroy(linerenderer);
            }
        }
    }
}
