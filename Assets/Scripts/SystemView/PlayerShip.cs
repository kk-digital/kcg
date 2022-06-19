using System;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class PlayerShip : MonoBehaviour {
            public SystemShip ship;

            public GameObject o;
            public SystemShipRenderer renderer;

            public float last_time;

            public bool render_orbit = true;

            public float gravitational_strength = 0.0f;

            public float time_scale = 1.0f;
            public float drag_factor = 10000.0f;
            public float sailing_factor = 20.0f;
            public float system_scale = 1.0f;

            public bool  mouse_steering = false;
            public CameraController camera_controller;

            public SystemState state;

            private void Start() {
                camera_controller  = GameObject.Find("Main Camera").GetComponent<CameraController>();

                state              = GetComponent<GameLoop>().CurrentSystemState;

                last_time          = Time.time * 1000.0f;

                ship               = new SystemShip();

                o                  = new GameObject();
                o.name             = "Player ship";

                ship.self.posx     = 0.0f;
                ship.self.posy     = 0.0f;
                ship.acceleration  = 5.0f;

                ship.self.mass     = 1.0f;

                renderer           = o.AddComponent<SystemShipRenderer>();
                renderer.ship      = ship;
                renderer.shipColor = Color.blue;
                renderer.width     = 3.0f;

                ship.health = ship.max_health = 25000;
                ship.shield = ship.max_shield = 50000;

                ship.shield_regeneration_rate = 3;
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

                if (!mouse_steering) {
                    if (Input.GetKey("left ctrl")) horizontal_movement = Input.GetAxis("Horizontal");
                    else ship.rotation -= Input.GetAxis("Horizontal") * current_time * ship.rotational_speed_modifier;
                } else {
                    horizontal_movement = -Input.GetAxis("Horizontal");
                    Vector3 RelPos = camera_controller.GetRelPos(new Vector3(ship.self.posx, ship.self.posy, 0.0f));

                    float dx = Input.mousePosition.x - RelPos.x;
                    float dy = Input.mousePosition.y - RelPos.y;

                    float d  = (float)Math.Sqrt(dx * dx + dy * dy);

                    float angle = (float)Math.Acos(dx / d);

                    if (dy < 0.0f) angle = 2.0f * 3.1415926f - angle;

                    ship.rotate_to(angle, current_time);
                }

                rotation_change -= ship.rotation;

                float movement = Input.GetAxis("Vertical");
                if (movement == 0.0f && Input.GetKey("w")) movement =  1.0f;
                if (movement == 0.0f && Input.GetKey("s")) movement = -1.0f;

                float accx = (float)Math.Cos(ship.rotation) * movement - (float)Math.Sin(ship.rotation) * horizontal_movement;
                float accy = (float)Math.Sin(ship.rotation) * movement + (float)Math.Cos(ship.rotation) * horizontal_movement;

                accx *= current_time * ship.acceleration;
                accy *= current_time * ship.acceleration;
            
                if (horizontal_movement != 0.0f && movement != 0.0f) {
                    accx *= Tools.rsqrt2;
                    accy *= Tools.rsqrt2;
                }

                ship.self.posx += ship.self.velx * current_time + accx / 2.0f * current_time;
                ship.self.posy += ship.self.vely * current_time + accy / 2.0f * current_time;

                ship.self.velx += accx;
                ship.self.vely += accy;

                // "Sailing" and "air resistance" effects are dampened the closer the player is to a massive object
                // This is to make gravity and slingshotting more realistic and easier for the player to use.

                if (gravitational_strength < 1.0f) {
                    float gravitational_factor = 1.0f / (1.0f - gravitational_strength);

                    // "Air resistance" effect
                    float drag_x = ship.self.velx * -current_time / (gravitational_factor + drag_factor);
                    float drag_y = ship.self.vely * -current_time / (gravitational_factor + drag_factor);

                    ship.self.velx *= 1.0f + drag_x;
                    ship.self.vely *= 1.0f + drag_y;

                    // "Sailing" effect
                    float effective_accx = accx + drag_x;
                    float effective_accy = accy + drag_y;
                
                    float magnitude = Tools.magnitude(ship.self.velx, ship.self.vely);

                    ship.self.velx = ((sailing_factor + gravitational_factor) * ship.self.velx + (float)Math.Cos(ship.rotation) * magnitude) / (1.0f + sailing_factor + gravitational_factor);
                    ship.self.vely = ((sailing_factor + gravitational_factor) * ship.self.vely + (float)Math.Sin(ship.rotation) * magnitude) / (1.0f + sailing_factor + gravitational_factor);
                }

                renderer.shipColor.b = (float) ship.health / ship.max_health;

                foreach (ShipWeapon weapon in ship.weapons) {
                    weapon.cooldown -= (int)(current_time * 1000.0f);
                    if (weapon.cooldown < 0) weapon.cooldown = 0;
                }

                if (render_orbit) {
                    if (ship.descriptor.central_body == null)
                        ship.descriptor.central_body = state.Star;

                    SpaceObject strongest_gravity_object = null;
                    float g = 0.0f;

                    foreach (SpaceObject obj in state.Objects) {
                        float dx = obj.posx - state.Player.ship.self.posx;
                        float dy = obj.posy - state.Player.ship.self.posy;

                        float d2 = dx * dx + dy * dy;

                        float curg = 6.67408E-11f * obj.mass / d2;

                        if (curg > g) {
                            g = curg;
                            strongest_gravity_object = obj;
                        }
                    }

                    if (strongest_gravity_object != null)
                        ship.descriptor.change_frame_of_reference(strongest_gravity_object);

                    if (ship.descriptor.eccentricity <= 1.0f)
                        ship.path_planned = true;
                    else
                        ship.path_planned = false;
                }

                foreach(ShipWeapon weapon in ship.weapons) {
                    weapon.update();

                    if((weapon.flags & (int)WeaponFlags.WEAPON_TURRET) != 0)
                        weapon.rotation -= rotation_change;
                }

                if(!mouse_steering) {
                    if(Input.GetKey("space") || Input.GetMouseButton(0)) {
                        Vector3 mouse_position = camera_controller.GetAbsPos(Input.mousePosition);

                        foreach(ShipWeapon weapon in ship.weapons)
                            if(weapon.try_targeting(mouse_position.x, mouse_position.y, current_time))
                                weapon.fire(mouse_position.x, mouse_position.y);
                    }
                } else {
                    if(Input.GetKey("space"))
                        foreach(ShipWeapon weapon in ship.weapons) {
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
