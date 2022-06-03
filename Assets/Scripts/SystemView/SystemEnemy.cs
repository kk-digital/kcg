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

        public float LastTime;

        private void Start()
        {
            LastTime = Time.time * 1000.0f;

            Rand = new System.Random();

            Ship = new SystemShip();

            Ship.Descriptor = new OrbitingObjectDescriptor();

            Ship.Descriptor.SemiMinorAxis = (float)Rand.NextDouble() * 5.0f + 6.0f;
            Ship.Descriptor.SemiMajorAxis = (float)Rand.NextDouble() * 2.0f + Ship.Descriptor.SemiMinorAxis;

            Ship.Start = Ship.Destination = Ship.Descriptor;

            Ship.PathPlanned = true;

            Object = new GameObject();
            Object.name = "Enemy ship";

            Ship.UpdatePosition(0.01f);

            Renderer = Object.AddComponent<SystemShipRenderer>();
            Renderer.ship = Ship;
            Renderer.shipColor = Color.red;

            Ship.Health = Ship.MaxHealth = 25000;
            Ship.Shield = Ship.MaxShield = 50000;

            Ship.ShieldRegenerationRate = 2;
        }

        private void Update()
        {
            float CurrentTime = Time.time - LastTime;

            if (CurrentTime == 0.0f) return;

            Ship.UpdatePosition(CurrentTime / 100.0f);

            Renderer.shipColor.r = (float) Ship.Health / Ship.MaxHealth;
        }
    }
}
