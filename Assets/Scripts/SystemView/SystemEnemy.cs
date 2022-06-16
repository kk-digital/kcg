using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class SystemEnemy : MonoBehaviour
    {
        public SystemShip ship;

        public GameObject Object;
        public SystemShipRenderer Renderer;

        public System.Random Rand;

        public int LastTime;

        private void Start()
        {
            LastTime = (int)(Time.time * 1000);

            Rand = new System.Random();

            ship = new SystemShip();

            ship.Descriptor = new OrbitingObjectDescriptor(ship.self);

            ship.Descriptor.semiminoraxis = (float)Rand.NextDouble() * 5.0f + 6.0f;
            ship.Descriptor.semimajoraxis = (float)Rand.NextDouble() * 2.0f + ship.Descriptor.semiminoraxis;

            ship.Descriptor.rotation      = (float)Rand.NextDouble() * 2.0f * 3.1415926f;
            ship.Descriptor.mean_anomaly   = (float)Rand.NextDouble() * 2.0f * 3.1415926f;

            GameLoop gl = GetComponent<GameLoop>();

            SystemState State = gl.CurrentSystemState;

            ship.Descriptor.central_body = State.Star;

            ship.Start = ship.Destination = ship.Descriptor;

            ship.PathPlanned = true;

            Object = new GameObject();
            Object.name = "Enemy ship";

            ship.Descriptor.compute();

            Renderer = Object.AddComponent<SystemShipRenderer>();
            Renderer.ship = ship;
            Renderer.shipColor = Color.red;
            Renderer.width = 3.0f;

            ship.Health = ship.MaxHealth = 25000;
            ship.Shield = ship.MaxShield = 50000;

            ship.ShieldRegenerationRate = 2;

            ShipWeapon Weapon = new ShipWeapon();

            Weapon.ProjectileColor = Color.white;

            Weapon.Range = 20.0f;
            Weapon.ShieldPenetration = 0.1f;
            Weapon.ProjectileVelocity = 5.0f;
            Weapon.Damage = 250;
            Weapon.AttackSpeed = 40;
            Weapon.Cooldown = 0;
            Weapon.Self = ship;

            ship.Weapons.Add(Weapon);
        }

        private void Update()
        {
            int CurrentMillis = (int)(Time.time * 1000) - LastTime;
            LastTime = (int)(Time.time * 1000);

            ship.Descriptor.update_position(CurrentMillis);

            Renderer.shipColor.r = (float) ship.Health / ship.MaxHealth;
        }

        void OnDestroy()
        {
            GameObject.Destroy(Renderer);
            GameObject.Destroy(Object);
        }
    }
}
