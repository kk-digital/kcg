using System;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class OrbitRenderer : MonoBehaviour {
            private LineRenderer line_renderer;

            public OrbitingObjectDescriptor descriptor;

            public Material mat;

            public Color color = new Color(0.5f, 0.7f, 1.0f, 1.0f);

            public float line_width = 0.1f;

            public CameraController camera;

            public void UpdateRenderer(int segments) {
                if(line_renderer == null) return;
                if(descriptor == null) {
                    line_renderer.startWidth = line_renderer.endWidth = 0.0f;
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

                line_renderer.startWidth = line_renderer.endWidth = line_width == 0.1f ? line_width / camera.scale : line_width;
                line_renderer.startColor = line_renderer.endColor = color;
                line_renderer.SetPositions(vertices);
                line_renderer.positionCount = segments;
            }

            // Start is called before the first frame update
            void Start() {
                camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

                line_renderer = gameObject.AddComponent<LineRenderer>();

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

                line_renderer.material = mat;

                line_renderer.useWorldSpace = true;

                line_renderer.loop = true;
            }

            void OnDestroy() {
                GameObject.Destroy(line_renderer);
            }
        }
    }
}
