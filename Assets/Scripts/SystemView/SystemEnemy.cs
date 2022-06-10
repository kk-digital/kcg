using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class SystemEnemy : MonoBehaviour
    {
        public SystemShip Ship;

        public GameObject Object;
        public SystemShipRenderer Renderer;

        public System.Random Rand;

        public int LastTime;

        private void Start()
        {
            LastTime = (int)(Time.time * 1000);

            Rand = new System.Random();

            Ship = new SystemShip();

            Ship.Descriptor = new OrbitingObjectDescriptor(Ship.Self);

            Ship.Descriptor.SemiMinorAxis = (float)Rand.NextDouble() * 5.0f + 6.0f;
            Ship.Descriptor.SemiMajorAxis = (float)Rand.NextDouble() * 2.0f + Ship.Descriptor.SemiMinorAxis;

            Ship.Descriptor.Rotation      = (float)Rand.NextDouble() * 2.0f * 3.1415926f;
            Ship.Descriptor.MeanAnomaly   = (float)Rand.NextDouble() * 2.0f * 3.1415926f;

            GameLoop gl = GetComponent<GameLoop>();

            SystemState State = gl.CurrentSystemState;

            Ship.Descriptor.CentralBody = State.Star;

            Ship.Start = Ship.Destination = Ship.Descriptor;

            Ship.PathPlanned = true;

            Object = new GameObject();
            Object.name = "Enemy ship";

            Ship.Descriptor.Compute();

            Renderer = Object.AddComponent<SystemShipRenderer>();
            Renderer.ship = Ship;
            Renderer.shipColor = Color.red;
            Renderer.width = 3.0f;

            Ship.Health = Ship.MaxHealth = 25000;
            Ship.Shield = Ship.MaxShield = 50000;

            Ship.ShieldRegenerationRate = 2;

            ShipWeapon Weapon = new ShipWeapon();

            Weapon.ProjectileColor = Color.white;

            Weapon.Range = 20.0f;
            Weapon.ShieldPenetration = 0.1f;
            Weapon.ProjectileVelocity = 5.0f;
            Weapon.Damage = 250;
            Weapon.AttackSpeed = 40;
            Weapon.Cooldown = 0;
            Weapon.Self = Ship;

            Ship.Weapons.Add(Weapon);
        }

        private void Update()
        {
            int CurrentMillis = (int)(Time.time * 1000) - LastTime;
            LastTime = (int)(Time.time * 1000);

            Ship.Descriptor.UpdatePosition(CurrentMillis);

            Renderer.shipColor.r = (float) Ship.Health / Ship.MaxHealth;
        }
    }
}
