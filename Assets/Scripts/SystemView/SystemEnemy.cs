using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class SystemEnemy : MonoBehaviour {
            public SystemShip ship;

            public GameObject Object;
            public SystemShipRenderer Renderer;

            public System.Random Rand;

            public int LastTime;

            private void Start() {
                LastTime = (int)(Time.time * 1000);

                Rand = new System.Random();

                ship = new SystemShip();

                ship.descriptor = new OrbitingObjectDescriptor(ship.self);

                ship.descriptor.semiminoraxis = (float)Rand.NextDouble() * 5.0f + 6.0f;
                ship.descriptor.semimajoraxis = (float)Rand.NextDouble() * 2.0f + ship.descriptor.semiminoraxis;

                ship.descriptor.rotation      = (float)Rand.NextDouble() * 2.0f * 3.1415926f;
                ship.descriptor.mean_anomaly   = (float)Rand.NextDouble() * 2.0f * 3.1415926f;

                GameManager gl = GetComponent<GameManager>();

                SystemState State = gl.CurrentSystemState;

                ship.descriptor.central_body = State.stars[0].self;

                ship.start = ship.destination = ship.descriptor;

                ship.path_planned = true;

                Object = new GameObject();
                Object.name = "Enemy ship";

                ship.descriptor.compute();

                Renderer = Object.AddComponent<SystemShipRenderer>();
                Renderer.ship = ship;
                Renderer.shipColor = Color.red;
                Renderer.width = 3.0f;

                ship.health = ship.max_health = 25000;
                ship.shield = ship.max_shield = 50000;

                ship.shield_regeneration_rate = 2;

                ShipWeapon Weapon = new ShipWeapon();

                Weapon.color = Color.white;

                Weapon.range = 20.0f;
                Weapon.shield_penetration = 0.1f;
                Weapon.projectile_velocity = 5.0f;
                Weapon.damage = 250;
                Weapon.attack_speed = 40;
                Weapon.cooldown = 0;
                Weapon.self = ship;

                ship.weapons.Add(Weapon);
            }

            private void Update() {
                int CurrentMillis = (int)(Time.time * 1000) - LastTime;
                LastTime = (int)(Time.time * 1000);

                ship.descriptor.update_position(CurrentMillis);

                Renderer.shipColor.r = (float)ship.health / ship.max_health;
            }

            void OnDestroy() {
                GameObject.Destroy(Renderer);
                GameObject.Destroy(Object);
            }
        }
    }
}
