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

            public bool  constant_rate_turning  = false;

            public bool  mouse_steering         = false;

            public const string main_weapon_key = "1";
            public const string broadsides_key  = "2";
            public const string turrets_key     = "3";

            private float rotating_to;
            private bool  rotating;

            public float periapsis;
            public float apoapsis;
            public float rotation;
            public bool  circularizing;

            public bool  turn_towards_mouse     = true;
            public bool  stations_orbiting;

            private SelectedWeaponType selectedWeapon = SelectedWeaponType.MAIN_WEAPONS;

            public CameraController camera_controller;
            public SystemState state;

            public LineRenderer rudder_renderer;
            public LineRenderer angular_velocity_in_direction_of_movement;
            public LineRenderer angular_velocity_perpendicular_to_movement;

            public GameObject   angular_velocity_direction_renderer;
            public GameObject   angular_velocity_perpendicular_renderer;

            public void rotate_to(float angle) {
                rotating_to = angle;
                rotating    = true;
            }

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

                ship.self.mass     = 100.0f;

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

                rudder_renderer.material                                 = mat;
                rudder_renderer.useWorldSpace                            = true;
                rudder_renderer.startColor                               =
                rudder_renderer.endColor                                 = Color.white;
                
                angular_velocity_direction_renderer                      = new GameObject();
                angular_velocity_in_direction_of_movement                = angular_velocity_direction_renderer.AddComponent<LineRenderer>();

                angular_velocity_perpendicular_renderer                  = new GameObject();
                angular_velocity_perpendicular_to_movement               = angular_velocity_perpendicular_renderer.AddComponent<LineRenderer>();

                angular_velocity_in_direction_of_movement.material       = mat;
                angular_velocity_in_direction_of_movement.useWorldSpace  = true;
                angular_velocity_in_direction_of_movement.startColor     =
                angular_velocity_in_direction_of_movement.endColor       = Color.blue;

                angular_velocity_perpendicular_to_movement.material      = mat;
                angular_velocity_perpendicular_to_movement.useWorldSpace = true;
                angular_velocity_perpendicular_to_movement.startColor    =
                angular_velocity_perpendicular_to_movement.endColor      = Color.green;
            }

            private void Update() {
                float current_time = Time.time - last_time;

                if (current_time == 0.0f) return;

                current_time *= time_scale;

                last_time = Time.time;
                Vector3[] vertices = new Vector3[2];

                if(ship.descriptor.central_body != null) {
                    float angular_velocity_x     = ship.self.velx - ship.descriptor.central_body.velx;
                    float angular_velocity_y     = ship.self.vely - ship.descriptor.central_body.vely;

                    float velocity_angle         = Tools.get_angle(angular_velocity_x,
                                                                   angular_velocity_y);

                    float angular_velocity_angle = Tools.get_angle(ship.descriptor.central_body.posx - ship.self.posx,
                                                                   ship.descriptor.central_body.posy - ship.self.posy);

                    // theta = angle between hypothetical circular orbit through ship's position

                    float theta                  = angular_velocity_angle - velocity_angle;

                    float costheta               = (float)Math.Cos(theta);
                    float sintheta               = (float)Math.Sin(theta);

                    (angular_velocity_x,
                     angular_velocity_y)         = (costheta * angular_velocity_x - sintheta * angular_velocity_y,
                                                    sintheta * angular_velocity_x + costheta * angular_velocity_x);

                    float in_direction           = Tools.magnitude(sintheta * angular_velocity_x, sintheta * angular_velocity_y);
                    float perpendicular          = Tools.magnitude(costheta * angular_velocity_x, costheta * angular_velocity_y);

                    Vector3[] vertices1          = new Vector3[2];
                    Vector3[] vertices2          = new Vector3[2];

                    vertices1[0] = new Vector3(ship.self.posx, ship.self.posy, 0.0f);
                    vertices1[1] = new Vector3(ship.self.posx + (float)Math.Cos(velocity_angle)
                                                              * 0.1f * in_direction / camera_controller.scale,
                                               ship.self.posy + (float)Math.Sin(velocity_angle)
                                                              * 0.1f * in_direction / camera_controller.scale,
                                               0.0f);

                    angular_velocity_in_direction_of_movement.SetPositions(vertices1);
                    angular_velocity_in_direction_of_movement.positionCount  = 2;

                    angular_velocity_in_direction_of_movement.startWidth     =
                    angular_velocity_in_direction_of_movement.endWidth       = 0.25f / camera_controller.scale;



                    vertices2[0] = new Vector3(ship.self.posx, ship.self.posy, 0.0f);
                    vertices2[1] = new Vector3(ship.self.posx + (float)Math.Cos(velocity_angle + Tools.halfpi)
                                                              * 0.1f * perpendicular / camera_controller.scale,
                                               ship.self.posy + (float)Math.Sin(velocity_angle + Tools.halfpi)
                                                              * 0.1f * perpendicular / camera_controller.scale,
                                               0.0f);

                    angular_velocity_perpendicular_to_movement.SetPositions(vertices2);
                    angular_velocity_perpendicular_to_movement.positionCount = 2;

                    angular_velocity_perpendicular_to_movement.startWidth    =
                    angular_velocity_perpendicular_to_movement.endWidth      = 0.25f / camera_controller.scale;

                } else {

                    Vector3[] vertices1 = new Vector3[2];

                    vertices1[0]        = new Vector3(ship.self.posx, ship.self.posy, 0.0f);
                    vertices1[1]        = new Vector3(ship.self.posx, ship.self.posy, 0.0f);

                    angular_velocity_in_direction_of_movement.SetPositions(vertices1);
                    angular_velocity_in_direction_of_movement.positionCount  = 0;

                    angular_velocity_perpendicular_to_movement.SetPositions(vertices1);
                    angular_velocity_perpendicular_to_movement.positionCount = 0;

                }

                if(rudder_enabled) {
                    vertices[0] = new Vector3(ship.self.posx, ship.self.posy, 0.0f);
                    vertices[1] = new Vector3(ship.self.posx + (float)Math.Cos(ship.rotation + sail_angle) * 5.0f / camera_controller.scale,
                                              ship.self.posy + (float)Math.Sin(ship.rotation + sail_angle) * 5.0f / camera_controller.scale,
                                              0.0f);

                    rudder_renderer.SetPositions(vertices);
                    rudder_renderer.positionCount = 2;

                    rudder_renderer.startWidth    =
                    rudder_renderer.endWidth      = 0.1f / camera_controller.scale;
                } else {
                    vertices[0] = new Vector3(ship.self.posx, ship.self.posy, 0.0f);
                    vertices[1] = new Vector3(ship.self.posx, ship.self.posy, 0.0f);

                    rudder_renderer.SetPositions(vertices);
                    rudder_renderer.positionCount = 0;
                }

                if(ship.docking_autopilot_tick(current_time, 0.1f * system_scale, stations_orbiting)) return;
                if(ship.orbital_autopilot_tick(periapsis, apoapsis, rotation, current_time))          return;
                if(circularizing) { circularizing = !ship.circularize(current_time);                  return; }

                if (Input.GetKeyDown("tab")) mouse_steering = !mouse_steering;

                float horizontal_movement = 0.0f;

                float rotation_change = ship.rotation;

                float direction     = ship.rotation;
                bool  move_to_mouse = false;
                float movement      = Input.GetAxis("Vertical");

                if(rotating && ship.rotation != rotating_to) ship.rotate_to(rotating_to, current_time);
                else rotating = false;

                if (!mouse_steering) {
                    ship.rotation         += ship.self.angular_vel * current_time;
                    if(Input.GetKey("left ctrl")) horizontal_movement = -Input.GetAxis("Horizontal");
                    else if(!rotating) {
                        float acc              = (float)Math.Sqrt(ship.torque / ship.self.angular_inertia) * -Input.GetAxis("Horizontal");
                        ship.rotation         += 0.5f * acc * current_time * current_time;
                        if(constant_rate_turning)
                            ship.self.angular_vel = acc;
                        else {
                            ship.self.angular_vel += acc * current_time;
                            ship.self.angular_vel *= 0.99f;
                        }
                    }
                } else {
                    horizontal_movement = -Input.GetAxis("Horizontal");
                    Vector3 RelPos = camera_controller.get_rel_pos(new Vector3(ship.self.posx, ship.self.posy, 0.0f));

                    float dx    = Input.mousePosition.x - RelPos.x;
                    float dy    = Input.mousePosition.y - RelPos.y;

                    float angle = Tools.get_angle(dx, dy);

                    if(turn_towards_mouse) { if(!rotating) ship.rotate_to(angle, current_time); }
                    else if(Input.GetMouseButton(0)) {
                        direction     = angle;
                        move_to_mouse = true;
                    }
                }

                if(Input.GetKey("q")) sail_angle += sail_speed * current_time;
                if(Input.GetKey("e")) sail_angle -= sail_speed * current_time;

                rotation_change -= ship.rotation;

                if (movement == 0.0f && Input.GetKey("w")) movement =  1.0f;
                if (movement == 0.0f && Input.GetKey("s")) movement = -1.0f;

                float cos  = (float)Math.Cos(ship.rotation);
                float sin  = (float)Math.Sin(ship.rotation);

                if(move_to_mouse) {

                    //  ax
                    // --- = cos(dir)
                    //  a 

                    //  ay
                    // --- = sin(dir)
                    //  a

                    // a = √(ax² + ay²)

                    // ax = t * (cos(rot) * a1 * m1 - sin(rot) * a2 * m2)

                    // ay = t * (sin(rot) * a1 * m1 + cos(rot) * a2 * m2)

                    // m1² + m2² = 1

                    // m2 = √(1 - m1²)

                    //                                 t * cos(rot) * a1 * m1 - sin(rot) * a2 * √(1 - m1²)
                    // --------------------------------------------------------------------------------------------------------------------- = cos(dir)
                    // √((t * (cos(rot) * a1 * m1 - sin(rot) * a2 * √(1 - m1²)))² + (t * (sin(rot) * a1 * m1 + cos(rot) * a2 * √(1 - m1²)))²)

                    //                        a2 * √(cos(2dir ± 2rot) + 1)
                    // m1 = ± --------------------------------------------------------------
                    //        √(a1² * (1 - cos(2dir ± 2rot)) + a2² * (1 + cos(2dir ± 2rot)))

                    // todo: solve this optimized version later
                    //       for now using version that only uses main thruster

                    /*float dirrot = 2 * direction + 2 * ship.rotation; // could be - too
                    float cosdirrot = (float)Math.Cos(dirrot);

                    movement  = ship.horizontal_acceleration * (float)Math.Sqrt(cosdirrot + 1);
                    movement /= (float)Math.Sqrt(ship.acceleration            * ship.acceleration            * (1 - cosdirrot)
                             +                   ship.horizontal_acceleration * ship.horizontal_acceleration * (1 + cosdirrot));

                    horizontal_movement = (float)Math.Sqrt(1 - movement * movement);*/

                    movement = 1.0f;

                    cos = (float)Math.Cos(direction);
                    sin = (float)Math.Sin(direction);

                }

                float accx = (cos * movement * ship.acceleration - sin * horizontal_movement * ship.horizontal_acceleration) * current_time;
                float accy = (sin * movement * ship.acceleration + cos * horizontal_movement * ship.horizontal_acceleration) * current_time;

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
                        ship.descriptor.central_body = state.stars[0].obj.self;

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

                    if(strongest_gravity_object != null)
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
                GameObject.Destroy(angular_velocity_perpendicular_renderer);
                GameObject.Destroy(angular_velocity_direction_renderer);
                GameObject.Destroy(renderer);
                GameObject.Destroy(o);
            }
        }   
    }
}
