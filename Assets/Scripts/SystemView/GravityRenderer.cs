using System;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class GravityRenderer : MonoBehaviour {
            public const int         samples        = 1024;
            public const float       line_thickness = 0.15f;
            public const float       line_length    = 0.75f;
            public const float       color_factor   = 2.0f;

            private struct ArrowInfo {
                public float         x;
                public float         y;
                public float         dirx;
                public float         diry;
                public GameObject    obj;
                public LineRenderer  line;
                public Vector3[]     vertices;
            }

            public  CameraController camera;

            public  int              width;
            public  int              height;
            public  Shader           shader;
            public  Material         mat;
            public  SystemState      state;

            private ArrowInfo[,]     arrows;
            public  bool             n_body_gravity = true;

            public void cleanup() {
                if(arrows != null)
                    for(int x = 0; x < width; x++)
                        for(int y = 0; y < height; y++) {
                            GameObject.Destroy(arrows[x, y].line);
                            GameObject.Destroy(arrows[x, y].obj);
                        }
                
            }

            public void reinit() {
                cleanup();

                height = (int)Math.Sqrt(samples / camera.get_aspect_ratio());
                width  = (int)         (height  * camera.get_aspect_ratio());

                arrows = new ArrowInfo[width, height];

                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++) {
                        arrows[x, y]                      = new ArrowInfo();
                        arrows[x, y].obj                  = new GameObject();
                        arrows[x, y].obj.name             = "Gravity direction renderer (" + x + ", " + y + ")";
                        arrows[x, y].obj.transform.parent = transform;
                        arrows[x, y].line                 = arrows[x, y].obj.AddComponent<LineRenderer>();
                        arrows[x, y].line.material        = mat;
                        arrows[x, y].line.useWorldSpace   = true;
                        arrows[x, y].line.startColor      =
                        arrows[x, y].line.endColor        = Color.white;
                        arrows[x, y].vertices             = new Vector3[5];
                        arrows[x, y].vertices[0]          = new Vector3();
                        arrows[x, y].vertices[1]          = new Vector3();
                        arrows[x, y].vertices[2]          = new Vector3();
                        arrows[x, y].vertices[3]          = new Vector3();
                        arrows[x, y].vertices[4]          = new Vector3();
                    }
            }

            private void Start() {
                shader        = Shader.Find("Hidden/Internal-Colored");
                mat           = new Material(shader);
                mat.hideFlags = HideFlags.HideAndDontSave;

                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

                // Turn off backface culling, depth writes, depth test.
                mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                mat.SetInt("_ZWrite", 0);
                mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

                reinit();
            }

            private void Update() {
                if(camera.size_changed()) reinit();

                float camera_width                   = camera.get_width();
                float camera_height                  = camera.get_height();

                float length;

                float scale;

                if(camera_height < camera_width) {
                    length                           = camera_height / (height - 1) * line_length;
                    scale                            = 1.0f / (height - 1);
                } else {
                    length                           = camera_width  / (width  - 1) * line_length;
                    scale                            = 1.0f / (width  - 1);
                }

                float actual_length = length / camera.scale * 0.25f * scale;

                for(int x = 0; x < width; x++)
                    for(int y = 0; y < height; y++) {
                        Vector3 absolute = camera.get_abs_pos(new Vector3(
                            x * length / line_length,
                            y * length / line_length,
                            0.0f
                        ));

                        arrows[x, y].x               = absolute.x;
                        arrows[x, y].y               = absolute.y;
                        arrows[x, y].dirx            = 0.0f;
                        arrows[x, y].diry            = 0.0f;
                        arrows[x, y].line.startWidth =
                        arrows[x, y].line.endWidth   = line_thickness / camera.scale;

                        float maxg = 0.0f;

                        foreach(SpaceObject Body in state.objects) {

                            float dx                 = Body.posx - absolute.x;
                            float dy                 = Body.posy - absolute.y;

                            float d2                 = dx * dx + dy * dy;
                            float d                  = (float)Math.Sqrt(d2);

                            float g                  = Tools.gravitational_constant * Body.mass / d2;

                            if(n_body_gravity) {

                                arrows[x, y].dirx       += g * dx / d;
                                arrows[x, y].diry       += g * dy / d;

                            } else {

                                if(g > maxg) {
                                    maxg = g;

                                    arrows[x, y].dirx    = g * dx / d;
                                    arrows[x, y].diry    = g * dy / d;
                                }

                            }

                        }

                        float magnitude              = Tools.magnitude(arrows[x, y].dirx, arrows[x, y].diry);
                        float angle                  = Tools.get_angle(arrows[x, y].dirx, arrows[x, y].diry);

                        arrows[x, y].dirx            = arrows[x, y].dirx * length / magnitude;
                        arrows[x, y].diry            = arrows[x, y].diry * length / magnitude;

                        arrows[x, y].vertices[0].x   = arrows[x, y].x;
                        arrows[x, y].vertices[0].y   = arrows[x, y].y;
                        arrows[x, y].vertices[0].z   = 1.0f;

                        arrows[x, y].vertices[1].x   = arrows[x, y].x + arrows[x, y].dirx / camera.scale * scale;
                        arrows[x, y].vertices[1].y   = arrows[x, y].y + arrows[x, y].diry / camera.scale * scale;
                        arrows[x, y].vertices[1].z   = 1.0f;

                        arrows[x, y].vertices[2].x   = arrows[x, y].vertices[1].x + actual_length * (float)Math.Cos(angle + Tools.pi - Tools.sixthpi);                                                                       
                        arrows[x, y].vertices[2].y   = arrows[x, y].vertices[1].y + actual_length * (float)Math.Sin(angle + Tools.pi - Tools.sixthpi);
                        arrows[x, y].vertices[2].z   = 1.0f;

                        arrows[x, y].vertices[3].x   = arrows[x, y].vertices[1].x + actual_length * (float)Math.Cos(angle + Tools.pi + Tools.sixthpi);                                                                       
                        arrows[x, y].vertices[3].y   = arrows[x, y].vertices[1].y + actual_length * (float)Math.Sin(angle + Tools.pi + Tools.sixthpi);
                        arrows[x, y].vertices[3].z   = 1.0f;

                        arrows[x, y].vertices[4].x   = arrows[x, y].vertices[1].x;
                        arrows[x, y].vertices[4].y   = arrows[x, y].vertices[1].y;
                        arrows[x, y].vertices[4].z   = 1.0f;

                        arrows[x, y].line.SetPositions(arrows[x, y].vertices);
                        arrows[x, y].line.positionCount = 5;

                        arrows[x, y].line.startColor =
                        arrows[x, y].line.endColor   = new Color(2.0f * (1.0f - 1.0f / (color_factor * magnitude)),
                                                                 2.0f * (       1.0f / (color_factor * magnitude)),
                                                                 0.0f, 1.0f);
                    }
            }

            private void OnDestroy() {
                cleanup();
            }
        }
    }
}
