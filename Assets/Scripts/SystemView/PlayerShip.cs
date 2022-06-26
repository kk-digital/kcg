using System;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class PlayerShip : MonoBehaviour {
            private enum SelectedWeaponType {
                MAIN_WEAPONS = 1 << 1,
                BROADSIDES   = 1 << 2,
                TURRETS      = 1 << 3
            };

            public SystemShip ship;

            public GameObject o;
            public SystemShipRenderer renderer;

            public float last_time;

            public bool  render_orbit           = true;

            public float gravitational_strength = 0.0f;

            public float time_scale             = 1.0f;
            public float drag_factor            = 10000.0f;
            public float drag_cutoff            = 5.0f;
            public bool  quadratic_drag         = false;
            public float sailing_factor         = 5.0f;
            public float system_scale           = 1.0f;
            public float sail_angle             = 0.0f;
            public float sail_speed             = 0.5f;
            public bool  rudder_enabled         = true;

            public bool  mouse_steering         = false;

            public const string main_weapon_key = "1";
            public const string broadsides_key  = "2";
            public const string turrets_key     = "3";

            public bool  turn_towards_mouse     = false;

            private SelectedWeaponType selectedWeapon = SelectedWeaponType.MAIN_WEAPONS;

            public CameraController camera_controller;
            public SystemState state;

            public LineRenderer rudder_renderer;

            private void Start() {
                camera_controller  = GameObject.Find("Main Camera").GetComponent<CameraController>();

                state              = GetComponent<GameManager>().CurrentSystemState;
                rudder_renderer    = gameObject.AddComponent<LineRenderer>();

                last_time          = Time.time * 1000.0f;

                ship               = new SystemShip();

                o                  = new GameObject();
                o.name             = "Player ship";

                ship.self.posx     = 0.0f;
                ship.self.posy     = 0.0f;
                ship.acceleration  = 5.0f;
                ship.horizontal_acceleration = 2.5f;

                ship.self.mass     = 1.0f;

                renderer           = o.AddComponent<SystemShipRenderer>();
                renderer.ship      = ship;
                renderer.shipColor = Color.blue;
                renderer.width     = 3.0f;

                ship.health        =
                ship.max_health    = 25000;
                ship.shield        =
                ship.max_shield    = 50000;

                ship.shield_regeneration_rate = 3;

                Shader shader = Shader.Find("Hidden/Internal-Colored");
                Material mat  = new Material(shader);
                mat.hideFlags = HideFlags.HideAndDontSave;

                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

                // Turn off backface culling, depth writes, depth test.
                mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                mat.SetInt("_ZWrite", 0);
                mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

                rudder_renderer.material      = mat;
                rudder_renderer.useWorldSpace = true;
                rudder_renderer.startColor    =
                rudder_renderer.endColor      = Color.white;
            }

            private void Update() {
                float current_time = Time.time - last_time;

                if (current_time == 0.0f) return;

                current_time *= time_scale;

                last_time = Time.time;

                if (ship.DockingAutopilotLoop(current_time, 0.1f * system_scale)) return;

                if (Input.GetKeyDown("tab")) mouse_steering = !mouse_steering;

                float horizontal_movement = 0.0f;

                float rotation_change = ship.rotation;

                Vector3[] vertices = new Vector3[2];

                if(rudder_enabled) {
                    vertices[0] = new Vector3(ship.self.posx, ship.self.posy, 0.0f);
                    vertices[1] = new Vector3(ship.self.posx + (float)Math.Cos(ship.rotation + sail_angle) * 5.0f,
                                              ship.self.posy + (float)Math.Sin(ship.rotation + sail_angle) * 5.0f,
                                              0.0f);

                    rudder_renderer.SetPositions(vertices);
                    rudder_renderer.positionCount  = 2;

                    rudder_renderer.startWidth     =
                    rudder_renderer.endWidth       = 0.1f / camera_controller.scale;
                } else {
                    vertices[0] = new Vector3(ship.self.posx, ship.self.posy, 0.0f);
                    vertices[1] = new Vector3(ship.self.posx, ship.self.posy, 0.0f);

                    rudder_renderer.SetPositions(vertices);
                    rudder_renderer.positionCount  = 0;
                }

                float direction = ship.rotation;
                float movement = Input.GetAxis("Vertical");

                if (!mouse_steering) {
                    ship.rotation         += ship.self.angular_vel * current_time;
                    if(Input.GetKey("left ctrl")) horizontal_movement = Input.GetAxis("Horizontal");
                    else {
                        float acc              = (float)Math.Sqrt(ship.torque / ship.self.angular_inertia) * -Input.GetAxis("Horizontal");
                        ship.rotation         += 0.5f * acc * current_time * current_time;
                        ship.self.angular_vel += acc * current_time;
                        ship.self.angular_vel *= 0.99f;
                    }
                } else {
                    horizontal_movement = -Input.GetAxis("Horizontal");
                    Vector3 RelPos = camera_controller.get_rel_pos(new Vector3(ship.self.posx, ship.self.posy, 0.0f));

                    float dx    = Input.mousePosition.x - RelPos.x;
                    float dy    = Input.mousePosition.y - RelPos.y;

                    float angle = Tools.get_angle(dx, dy);

                    if(turn_towards_mouse) ship.rotate_to(angle, current_time);
                    else if(Input.GetMouseButton(0)) {
                        direction = angle;
                        movement  = 1.0f;
                    }
                }

                if(Input.GetKey("q")) sail_angle += sail_speed * current_time;
                if(Input.GetKey("e")) sail_angle -= sail_speed * current_time;

                rotation_change -= ship.rotation;

                if (movement == 0.0f && Input.GetKey("w")) movement =  1.0f;
                if (movement == 0.0f && Input.GetKey("s")) movement = -1.0f;

                float cos    = (float)Math.Cos(ship.rotation);
                float sin    = (float)Math.Sin(ship.rotation);

                float dircos = (float)Math.Cos(direction);
                float dirsin = (float)Math.Sin(direction);

                float accx = (cos * movement * ship.acceleration + sin * horizontal_movement * ship.horizontal_acceleration) * current_time;
                float accy = (sin * movement * ship.acceleration + cos * horizontal_movement * ship.horizontal_acceleration) * current_time;

                (accx, accy) = (dircos * accx - dirsin * accy, dirsin * accx + dircos * accy);

                /*
                 * if (horizontal_movement != 0.0f && movement != 0.0f) {
                 *     accx *= Tools.rsqrt2;
                 *     accy *= Tools.rsqrt2;
                 * }
                */

                ship.self.posx += ship.self.velx * current_time + accx / 2.0f * current_time;
                ship.self.posy += ship.self.vely * current_time + accy / 2.0f * current_time;

                ship.self.velx += accx;
                ship.self.vely += accy;

                // "Sailing" and "air resistance" effects are dampened the closer the player is to a massive object
                // This is to make gravity and slingshotting more realistic and easier for the player to use.

                if (gravitational_strength < 1.0f) {
                    float gravitational_factor = 1.0f / (1.0f - gravitational_strength);

                    float magnitude = Tools.magnitude(ship.self.velx, ship.self.vely);

                    // "Air resistance" effect
                    if(magnitude > drag_cutoff) {
                        // Drag cutoff as a vector facing the same direction as velocity
                        float cutoff_x = drag_cutoff * ship.self.velx / magnitude;
                        float cutoff_y = drag_cutoff * ship.self.vely / magnitude;
                        
                        // Effective velocity = velocity - drag cutoff, this is the part of the velocity
                        // that we care about for calculating the drag;
                        float effective_vel_x = ship.self.velx - cutoff_x;
                        float effective_vel_y = ship.self.vely - cutoff_y;

                        float drag_x = effective_vel_x * -current_time / (gravitational_factor + drag_factor);
                        float drag_y = effective_vel_y * -current_time / (gravitational_factor + drag_factor);

                        if(quadratic_drag) {
                            drag_x *= effective_vel_x;
                            drag_y *= effective_vel_y;
                        }

                        ship.self.velx *= 1.0f + drag_x;
                        ship.self.vely *= 1.0f + drag_y;

                        magnitude = Tools.magnitude(ship.self.velx, ship.self.vely);
                    }

                    // "Sailing" effect
                    if(ship.self.velx != 0.0f && ship.self.vely != 0.0f && rudder_enabled) {
                        float sail_x = magnitude * current_time * (float)Math.Cos(ship.rotation + sail_angle);
                        float sail_y = magnitude * current_time * (float)Math.Sin(ship.rotation + sail_angle);

                        ship.self.velx = (sailing_factor * ship.self.velx + sail_x) / (sailing_factor + current_time);
                        ship.self.vely = (sailing_factor * ship.self.vely + sail_y) / (sailing_factor + current_time);
                    }
                }

                renderer.shipColor.b = (float) ship.health / ship.max_health;

                foreach (ShipWeapon weapon in ship.weapons) {
                    weapon.cooldown -= (int)(current_time * 1000.0f);
                    if (weapon.cooldown < 0) weapon.cooldown = 0;
                }

                if (render_orbit) {
                    if (ship.descriptor.central_body == null)
                        ship.descriptor.central_body = state.star;

                    SpaceObject strongest_gravity_object = null;
                    float g = 0.0f;

                    foreach (SpaceObject obj in state.objects) {
                        float dx = obj.posx - state.player.ship.self.posx;
                        float dy = obj.posy - state.player.ship.self.posy;

                        float d2 = dx * dx + dy * dy;

                        float curg = 6.67408E-11f * obj.mass / d2;

                        if (curg > g) {
                            g = curg;
                            strongest_gravity_object = obj;
                        }
                    }

                    if (strongest_gravity_object != null)
                        ship.descriptor.change_frame_of_reference(strongest_gravity_object);

                    ship.path_planned = true;
                }

                     if(Input.GetKey(main_weapon_key)) selectedWeapon = SelectedWeaponType.MAIN_WEAPONS;
                else if(Input.GetKey( broadsides_key)) selectedWeapon = SelectedWeaponType.BROADSIDES;
                else if(Input.GetKey(    turrets_key)) selectedWeapon = SelectedWeaponType.TURRETS;

                foreach(ShipWeapon weapon in ship.weapons) {
                    weapon.update();

                    if((weapon.flags & (int)WeaponFlags.WEAPON_TURRET) != 0)
                        weapon.rotation -= rotation_change;
                }

                if(!mouse_steering) {
                    if(Input.GetKey("space") || Input.GetMouseButton(0)) {
                        Vector3 mouse_position = camera_controller.get_abs_pos(Input.mousePosition);

                        foreach(ShipWeapon weapon in ship.weapons) {
                            if(((int)selectedWeapon & (int)weapon.flags) == 0) continue;

                            if(weapon.try_targeting(mouse_position.x, mouse_position.y, current_time))
                                weapon.fire(mouse_position.x, mouse_position.y);
                        }
                    }
                } else {
                    if(Input.GetKey("space"))
                        foreach(ShipWeapon weapon in ship.weapons) {
                            if(((int)selectedWeapon & (int)weapon.flags) == 0) continue;

                            float x = ship.self.posx + (float)Math.Cos(ship.rotation) * weapon.range;
                            float y = ship.self.posy + (float)Math.Sin(ship.rotation) * weapon.range;

                            if(weapon.try_targeting(x, y, current_time))
                                weapon.fire(x, y);
                        }
                }
            }

            void OnDestroy() {
                GameObject.Destroy(renderer);
                GameObject.Destroy(o);
            }
        }   
    }
}
