using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class SystemEnemy : MonoBehaviour {
            public SystemShip         ship;

            public GameObject         obj;
            public SystemShipRenderer renderer;

            public System.Random      rand;

            public int                last_time;

            private void Start() {
                last_time                      = (int)(Time.time * 1000);

                rand                          = new System.Random();

                ship                          = new SystemShip();

                ship.descriptor               = new OrbitingObjectDescriptor(ship.self);

                ship.descriptor.semiminoraxis = (float)rand.NextDouble() * 5.0f + 6.0f;
                ship.descriptor.semimajoraxis = (float)rand.NextDouble() * 2.0f + ship.descriptor.semiminoraxis;

                ship.descriptor.rotation      = (float)rand.NextDouble() * 2.0f * 3.1415926f;
                ship.descriptor.mean_anomaly  = (float)rand.NextDouble() * 2.0f * 3.1415926f;

                GameManager gl                = GetComponent<GameManager>();

                SystemState state             = gl.CurrentSystemState;

                ship.descriptor.central_body  = state.stars[0].obj.self;

                ship.start                    =
                ship.destination              = ship.descriptor;

                ship.path_planned             = true;

                obj                        = new GameObject();
                obj.name                   = "Enemy ship";

                ship.descriptor.compute();

                renderer                      = obj.AddComponent<SystemShipRenderer>();
                renderer.ship                 = ship;
                renderer.shipColor            = Color.red;
                renderer.width                = 3.0f;

                ship.health                   =
                ship.max_health               = 25000;

                ship.shield                   =
                ship.max_shield               = 50000;

                ship.shield_regeneration_rate = 2;

                ShipWeapon weapon             = new ShipWeapon();

                weapon.color                  = Color.white;

                weapon.range                  = 20.0f;
                weapon.shield_penetration     = 0.1f;
                weapon.projectile_velocity    = 5.0f;
                weapon.damage                 = 250;
                weapon.attack_speed           = 40;
                weapon.cooldown               = 0;
                weapon.self                   = ship;

                ship.weapons.Add(weapon);
            }

            private void Update() {
                int current_millis            = (int)(Time.time * 1000) - last_time;
                last_time                     = (int)(Time.time * 1000);

                ship.descriptor.update_position(current_millis);

                renderer.shipColor.r          = (float)ship.health / ship.max_health;
            }

            void OnDestroy() {
                GameObject.Destroy(renderer);
                GameObject.Destroy(obj);
            }
        }
    }
}
