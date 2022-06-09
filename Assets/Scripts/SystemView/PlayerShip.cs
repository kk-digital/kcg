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

        public float RotationSpeedModifier = 2.0f;

        public bool Reverse = false;

        public bool RenderOrbit = true;

        public float GravitationalStrength = 0.0f;

        public SystemState State;

        private void Start()
        {
            GameLoop gl = GetComponent<GameLoop>();

            State = gl.CurrentSystemState;

            LastTime = Time.time * 1000.0f;

            Ship = new SystemShip();

            Object = new GameObject();
            Object.name = "Player ship";

            Ship.Self.PosX = 0.0f;
            Ship.Self.PosY = 0.0f;
            Ship.Acceleration = 250.0f;

            Ship.Self.Mass = 1.0f;

            Renderer = Object.AddComponent<SystemShipRenderer>();
            Renderer.ship = Ship;
            Renderer.shipColor = Color.blue;
            Renderer.width = 3.0f;

            Ship.Health = Ship.MaxHealth = 25000;
            Ship.Shield = Ship.MaxShield = 50000;

            Ship.ShieldRegenerationRate = 2;

            ShipWeapon Weapon = new ShipWeapon();

            Weapon.ProjectileColor = Color.white;

            Weapon.Range = 15.0f;
            Weapon.ShieldPenetration = 0.1f;
            Weapon.ProjectileVelocity = 8.0f;
            Weapon.Damage = 400;
            Weapon.AttackSpeed = 30;
            Weapon.Cooldown = 0;
            Weapon.Self = Ship;

            Ship.Weapons.Add(Weapon);
        }

        private void Update()
        {
            float CurrentTime = Time.time - LastTime;

            if (CurrentTime == 0.0f) return;

            LastTime = Time.time;

            Ship.Rotation -= Input.GetAxis("Horizontal") * CurrentTime * RotationSpeedModifier;

            float Movement = Input.GetAxis("Vertical");
            if (Movement == 0.0f && Input.GetKey("w")) Movement =  1.0f;
            if (Movement == 0.0f && Input.GetKey("s")) Movement = -1.0f;

            if (Movement > 0.0f && Ship.Self.VelX * Ship.Self.VelX + Ship.Self.VelY * Ship.Self.VelY < 0.1f) Reverse = false;
            if (Movement < 0.0f && Ship.Self.VelX * Ship.Self.VelX + Ship.Self.VelY * Ship.Self.VelY < 0.1f) Reverse = true;

            float AccX = (float)Math.Cos(Ship.Rotation);
            float AccY = (float)Math.Sin(Ship.Rotation);

            float Magnitude = (float)Math.Sqrt(AccX * AccX + AccY * AccY);

            AccX = AccX / Magnitude * Ship.Acceleration * CurrentTime * Movement;
            AccY = AccY / Magnitude * Ship.Acceleration * CurrentTime * Movement;

            Ship.Self.PosX += Ship.Self.VelX * CurrentTime + AccX / 2.0f * CurrentTime * CurrentTime;
            Ship.Self.PosY += Ship.Self.VelY * CurrentTime + AccY / 2.0f * CurrentTime * CurrentTime;

            Ship.Self.VelX += AccX * CurrentTime;
            Ship.Self.VelY += AccY * CurrentTime;

            // "Sailing" and "air resistance" effects are dampened the closer the player is to a massive object
            // This is to make gravity and slingshotting more realistic and easier for the player to use.

            if (GravitationalStrength < 1.0f)
            {
                float GravitationalFactor = 1.0f / (1.0f - GravitationalStrength);

                // "Air resistance" effect
                Ship.Self.VelX *= 1.0f - 1.0f / (GravitationalFactor + 1000.0f);
                Ship.Self.VelY *= 1.0f - 1.0f / (GravitationalFactor + 1000.0f);

                // "Sailing" effect
                if (Input.GetAxis("Horizontal") != 0.0f)
                {
                    Magnitude = (float)Math.Sqrt(Ship.Self.VelX * Ship.Self.VelX + Ship.Self.VelY * Ship.Self.VelY);

                    Ship.Self.VelX = ((5.0f + GravitationalFactor) * Ship.Self.VelX + (float)Math.Cos(Ship.Rotation) * Magnitude * (Reverse ? -1.0f : 1.0f)) / (6.0f + GravitationalFactor);
                    Ship.Self.VelY = ((5.0f + GravitationalFactor) * Ship.Self.VelY + (float)Math.Sin(Ship.Rotation) * Magnitude * (Reverse ? -1.0f : 1.0f)) / (6.0f + GravitationalFactor);
                }
            }

            Renderer.shipColor.b = (float) Ship.Health / Ship.MaxHealth;

            Ship.Weapons[0].Cooldown -= (int)(CurrentTime * 1000.0f);
            if (Ship.Weapons[0].Cooldown < 0) Ship.Weapons[0].Cooldown = 0;

            if (RenderOrbit)
            {
                if (Ship.Descriptor.CentralBody == null)
                    Ship.Descriptor.CentralBody = State.Star;

                SystemViewBody StrongestGravityBody = null;
                float g = 0.0f;

                foreach (SystemViewBody Body in State.Bodies)
                {
                    float dx = Body.PosX - State.Player.Ship.Self.PosX;
                    float dy = Body.PosY - State.Player.Ship.Self.PosY;

                    float d2 = dx * dx + dy * dy;

                    float curg = 6.67408E-11f * Body.Mass / d2;

                    if (curg > g)
                    {
                        g = curg;
                        StrongestGravityBody = Body;
                    }
                }

                if (StrongestGravityBody != null)
                    Ship.Descriptor.ChangeFrameOfReference(StrongestGravityBody);

                if (Ship.Descriptor.Eccentricity <= 1.0f)
                    Ship.PathPlanned = true;
                else
                    Ship.PathPlanned = false;
            }

            if (Input.GetKey("space")) Ship.Weapons[0].Fire();
        }
    }   
}
