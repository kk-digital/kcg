using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class PlayerShip : MonoBehaviour
    {
        public SystemShip Ship;

        public GameObject Object;
        public SystemShipRenderer Renderer;

        public float LastTime;

        public float rotation = 0.0f;

        public float RotationSpeedModifier = 1.0f;

        public bool Reverse = false;

        private void Start()
        {
            LastTime = Time.time * 1000.0f;

            Ship = new SystemShip();

            Object = new GameObject();
            Object.name = "Player ship";

            Ship.UpdatePosition(0.01f);
            Ship.PosX = 10.0f;
            Ship.PosY = 10.0f;
            Ship.Acceleration = 250.0f;

            Renderer = Object.AddComponent<SystemShipRenderer>();
            Renderer.ship = Ship;
            Renderer.shipColor = Color.blue;

            Ship.Health = Ship.MaxHealth = 25000;
            Ship.Shield = Ship.MaxShield = 50000;

            Ship.ShieldRegenerationRate = 2;
        }

        private void Update()
        {
            float CurrentTime = Time.time - LastTime;

            if (CurrentTime == 0.0f) return;

            LastTime = Time.time;

            Ship.Rotation -= Input.GetAxis("Horizontal") * CurrentTime * RotationSpeedModifier;

            if (Input.GetAxis("Vertical") > 0.0f && Ship.VelX * Ship.VelX + Ship.VelY * Ship.VelY < 0.1f) Reverse = false;
            if (Input.GetAxis("Vertical") < 0.0f && Ship.VelX * Ship.VelX + Ship.VelY * Ship.VelY < 0.1f) Reverse = true;

            float AccX = (float)Math.Cos(Ship.Rotation);
            float AccY = (float)Math.Sin(Ship.Rotation);

            float Magnitude = (float)Math.Sqrt(AccX * AccX + AccY * AccY);

            AccX = AccX / Magnitude * Ship.Acceleration * CurrentTime * Input.GetAxis("Vertical");
            AccY = AccY / Magnitude * Ship.Acceleration * CurrentTime * Input.GetAxis("Vertical");

            Ship.PosX += Ship.VelX * CurrentTime + AccX / 2.0f * CurrentTime * CurrentTime;
            Ship.PosY += Ship.VelY * CurrentTime + AccY / 2.0f * CurrentTime * CurrentTime;

            Ship.VelX += AccX * CurrentTime;
            Ship.VelY += AccY * CurrentTime;

            // "Air resistance" effect
            Ship.VelX *= 0.995f;
            Ship.VelY *= 0.995f;

            // "Sailing" effect
            Magnitude = (float)Math.Sqrt(Ship.VelX * Ship.VelX + Ship.VelY * Ship.VelY);

            Ship.VelX = (3.0f * Ship.VelX + (float)Math.Cos(Ship.Rotation) * Magnitude * (Reverse ? -1.0f : 1.0f)) / 4.0f;
            Ship.VelY = (3.0f * Ship.VelY + (float)Math.Sin(Ship.Rotation) * Magnitude * (Reverse ? -1.0f : 1.0f)) / 4.0f;

            Renderer.shipColor.b = Ship.Health / Ship.MaxHealth;
        }
    }
}
