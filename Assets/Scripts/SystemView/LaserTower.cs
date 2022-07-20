using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class LaserTower : MonoBehaviour {
            public  float posx;
            public  float posy;

            public  float angular_velocity;
            public  float rotation;
            private float last_rotation;
            public  float FOV;
            public  float range;

            public  int damage;
            public  int firing_rate;
            private int last_millis;
            public  int cooldown;

            public  float shield_penetration;

            public  float shield_damage_multiplier;
            public  float hull_damage_multiplier;

            public  System.Random rand;

            public  SpriteRenderer sr;
            public  CameraController camera;

            private GameObject debug_line_object1;
            private GameObject debug_line_object2;
            private GameObject laser_line_object;

            private LineRenderer debug_line_renderer1;
            private LineRenderer debug_line_renderer2;

            private Color debug_cone_color         = new Color(0.8f, 0.6f, 0.1f, 0.4f);

            public LineRenderer laser_renderer;
            public Color laser_color               = new Color(0.2f, 0.8f, 0.3f, 0.7f);

            public float laser_charging_time;
            public float laser_duration;
            public float laser_width;

            public SystemState state;

            public SystemShip target;

            // todo apply gravity to laser tower

            void Start() {
                rand                               = new System.Random();

                posx                               = (float)rand.NextDouble() * 50.0f - 25.0f;
                posy                               = (float)rand.NextDouble() * 50.0f - 25.0f;

                angular_velocity                   = 0.4f;
                rotation                           = 0.0f;
                last_rotation                      = 0.0f;
                FOV                                = Tools.pi / 6.0f;
                range                              = 20.0f;
                laser_width                        = 0.6f;
                laser_charging_time                = 0.125f;
                laser_duration                     = 0.3f;

                firing_rate                        = 3250;
                damage                             = 3000;
                shield_damage_multiplier           = 3.0f;
                hull_damage_multiplier             = 0.2f;
                shield_penetration                 = 0.02f;

                sr                                 = gameObject.AddComponent<SpriteRenderer>();
                sr.sprite                          = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");

                camera                             = GameObject.Find("Main Camera").GetComponent<CameraController>();

                debug_line_object1                 = new GameObject();
                debug_line_object2                 = new GameObject();
                laser_line_object                  = new GameObject();

                debug_line_renderer1               = debug_line_object1.AddComponent<LineRenderer>();
                debug_line_renderer2               = debug_line_object2.AddComponent<LineRenderer>();
                laser_renderer                     = laser_line_object.AddComponent<LineRenderer>();

                Shader shader                      = Shader.Find("Hidden/Internal-Colored");
                Material mat                       = new Material(shader);
                mat.hideFlags                      = HideFlags.HideAndDontSave;

                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

                // Turn off backface culling, depth writes, depth test.
                mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                mat.SetInt("_ZWrite", 0);
                mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

                debug_line_renderer1.material      =
                debug_line_renderer2.material      =
                laser_renderer.material            = mat;

                debug_line_renderer1.useWorldSpace =
                debug_line_renderer2.useWorldSpace =
                laser_renderer.useWorldSpace       = true;

                last_millis                        = (int)(Time.time * 1000.0f);

                laser_renderer.startWidth          =
                laser_renderer.endWidth            = 0.15f / camera.scale;

                laser_renderer.startColor          =
                laser_renderer.endColor            = laser_color;
            }

            void Update() {
                sr.transform.position              = new Vector3(posx, posy, -0.1f);
                sr.transform.localScale            = new Vector3(5.0f / camera.scale,
                                                                 5.0f / camera.scale,
                                                                 1.0f);

                sr.transform.Rotate(new Vector3(0.0f, 0.0f, (rotation - last_rotation) * 180.0f / Tools.pi));
                last_rotation                      = rotation;

                Vector3[] vertices1                = new Vector3[2];
                Vector3[] vertices2                = new Vector3[2];

                vertices1[0]                       = new Vector3(posx, posy, 0.0f);
                vertices1[1]                       = new Vector3(posx + (float)Math.Cos(rotation - FOV / 2.0f) * range,
                                                                 posy + (float)Math.Sin(rotation - FOV / 2.0f) * range,
                                                                 0.0f);

                vertices2[0]                       = new Vector3(posx, posy, 0.0f);
                vertices2[1]                       = new Vector3(posx + (float)Math.Cos(rotation + FOV / 2.0f) * range,
                                                                 posy + (float)Math.Sin(rotation + FOV / 2.0f) * range,
                                                                 0.0f);

                debug_line_renderer1.startWidth    =
                debug_line_renderer1.endWidth      =
                debug_line_renderer2.startWidth    =
                debug_line_renderer2.endWidth      = 0.1f / camera.scale;
                
                debug_line_renderer1.startColor    =
                debug_line_renderer1.endColor      =
                debug_line_renderer2.startColor    =
                debug_line_renderer2.endColor      = debug_cone_color;

                debug_line_renderer1.SetPositions(vertices1);
                debug_line_renderer2.SetPositions(vertices2);

                debug_line_renderer1.positionCount =
                debug_line_renderer2.positionCount = 2;

                int CurrentMillis                  = (int)(Time.time * 1000.0f) - last_millis;
                last_millis                        = (int)(Time.time * 1000.0f);

                int ChargingTime                   = (int)(firing_rate * laser_duration *         laser_charging_time );
                int LaserDurationTime              = (int)(firing_rate * laser_duration * (1.0f - laser_charging_time));

                int RemainingChargingTime          = cooldown - (firing_rate -      ChargingTime);
                int RemainingTime                  = cooldown - (firing_rate - LaserDurationTime);

                if(RemainingTime > 0) {
                    if(RemainingChargingTime > 0) {
                        float RemainingTimeAsPercentage = 1.0f - (float)RemainingChargingTime
                                                               / (float)LaserDurationTime;
                        laser_renderer.startWidth =
                        laser_renderer.endWidth   = laser_width * RemainingTimeAsPercentage / camera.scale;

                        laser_renderer.startColor = new Color(laser_color.r,
                                                              laser_color.g,
                                                              laser_color.b,
                                                              laser_color.a * RemainingTimeAsPercentage + 0.10f);

                        laser_renderer.endColor   = new Color(laser_color.r,
                                                              laser_color.g,
                                                              laser_color.b,
                                                              laser_color.a * RemainingTimeAsPercentage + 0.02f);
                    } else {
                        float RemainingTimeAsPercentage = (float)RemainingTime / (float)LaserDurationTime;
                        laser_renderer.startWidth =
                        laser_renderer.endWidth   = RemainingTimeAsPercentage * laser_width / camera.scale;

                        laser_renderer.startColor = new Color(laser_color.r,
                                                              laser_color.g,
                                                              laser_color.b,
                                                              laser_color.a * RemainingTimeAsPercentage + 0.10f);

                        laser_renderer.endColor   = new Color(laser_color.r,
                                                              laser_color.g,
                                                              laser_color.b,
                                                              laser_color.a * RemainingTimeAsPercentage + 0.02f);
                    }
                }

                cooldown                         -= CurrentMillis;
                if(cooldown < 0) cooldown         = 0;

                // Pick target
                if(target != null) {
                    float DistanceX               = target.self.posx - posx;
                    float DistanceY               = target.self.posy - posy;
                    float Distance                = Tools.magnitude(DistanceX, DistanceY);

                    if(Distance > range) target   = null;
                }

                if(target == null) {
                    if(state != null)
                        foreach(var s in state.ships) {
                            SystemShip ship = s.obj;

                            float DistanceX       = ship.self.posx - posx;
                            float DistanceY       = ship.self.posy - posy;
                            float Distance        = Tools.magnitude(DistanceX, DistanceY);

                            if(Distance <= range) { target = ship; break; }
                        }
                }

                if(target != null) {
                    if(try_targeting(target, CurrentMillis) && cooldown == 0) {
                        try_shooting();
                    }
                }
            }

            public bool try_targeting(SystemShip ship, int CurrentMillis) {
                float DistanceX                   = ship.self.posx - posx;
                float DistanceY                   = ship.self.posy - posy;
                float Distance                    = Tools.magnitude(DistanceX, DistanceY);

                while(rotation <        0.0f)
                    rotation                      = Tools.twopi + rotation;

                while(rotation > Tools.twopi)
                    rotation                     -= Tools.twopi;

                if(Distance > range) return false;

                float Angle                       = (float)Math.Acos(DistanceX / Distance);
                if(ship.self.posy < posy) Angle   = Tools.twopi - Angle;

                if(rotation == Angle) return true;

                float diff1                       = Angle - rotation;
                float diff2                       = rotation - Angle;

                if(diff1 < 0.0f) diff1            = Tools.twopi + diff1;
                if(diff2 < 0.0f) diff2            = Tools.twopi + diff2;

                if(diff2 < diff1) {
                    rotation                     -= angular_velocity / 1000.0f * CurrentMillis;

                    diff1                         = Angle - rotation;
                    diff2                         = rotation - Angle;

                    if(diff1 < 0.0f) diff1        = Tools.twopi + diff1;
                    if(diff2 < 0.0f) diff2        = Tools.twopi + diff2;

                    if(diff1 < diff2) rotation    = Angle;
                } else {
                    rotation                     += angular_velocity / 1000.0f * CurrentMillis;

                    diff1                         = Angle - rotation;
                    diff2                         = rotation - Angle;

                    if(diff1 < 0.0f) diff1        = Tools.twopi + diff1;
                    if(diff2 < 0.0f) diff2        = Tools.twopi + diff2;

                    if(diff2 < diff1) rotation    = Angle;
                }

                return true;
            }

            public bool try_shooting() {
                if(cooldown > 0) return false;

                float DistanceX                   = target.self.posx - posx;
                float DistanceY                   = target.self.posy - posy;
                float Distance                    = Tools.magnitude(DistanceX, DistanceY);

                if   (rotation <        0.0f)
                    rotation                      = Tools.twopi + rotation;

                while(rotation > Tools.twopi)
                    rotation                     -= Tools.twopi;

                if(Distance > range) return false;

                float Angle                       = (float)Math.Acos(DistanceX / Distance);
                if(target.self.posy < posy) Angle = Tools.twopi - Angle;

                if(Angle > rotation + FOV / 2.0f
                || Angle < rotation - FOV / 2.0f) return false;

                Vector3[] vertices                = new Vector3[2];
                vertices[0]                       = new Vector3(posx, posy, 0.0f);
                vertices[1]                       = new Vector3(target.self.posx, target.self.posy, 0.0f);

                laser_renderer.SetPositions(vertices);

                laser_renderer.positionCount      = 2;

                float ShieldDamage                = damage * shield_damage_multiplier * 1.0f - shield_penetration;
                float HullDamage                  = damage *   hull_damage_multiplier *        shield_penetration;

                target.shield                    -= (int)ShieldDamage;

                if(target.shield < 0) {
                    HullDamage                   -= (float)target.shield / shield_damage_multiplier * hull_damage_multiplier;
                    target.shield                 = 0;
                }

                target.health                    -= (int)HullDamage;

                if(target.health <= 0) {
                    target.destroy();
                    target                        = null;
                }

                cooldown                          = firing_rate;

                return true;
            }

            void OnDestroy() {
                GameObject.Destroy(debug_line_renderer1);
                GameObject.Destroy(debug_line_renderer2);
                GameObject.Destroy(laser_renderer);
                GameObject.Destroy(debug_line_object1);
                GameObject.Destroy(debug_line_object2);
                GameObject.Destroy(laser_line_object);
            }
        }
    }
}
