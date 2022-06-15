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

        public bool Reverse = false;

        public bool RenderOrbit = true;

        public float GravitationalStrength = 0.0f;

        public float TimeScale = 1.0f;
        public float DragFactor = 10000.0f;
        public float SailingFactor = 200.0f;
        public float SystemScale = 1.0f;

        public bool  MouseSteering = false;
        public CameraController Camera;

        public SystemState State;

        private void Start()
        {
            Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

            GameLoop gl = GetComponent<GameLoop>();

            State = gl.CurrentSystemState;

            LastTime = Time.time * 1000.0f;

            Ship = new SystemShip();

            Object = new GameObject();
            Object.name = "Player ship";

            Ship.Self.PosX = 0.0f;
            Ship.Self.PosY = 0.0f;
            Ship.Acceleration = 5.0f;

            Ship.Self.Mass = 1.0f;

            Renderer = Object.AddComponent<SystemShipRenderer>();
            Renderer.ship = Ship;
            Renderer.shipColor = Color.blue;
            Renderer.width = 3.0f;

            Ship.Health = Ship.MaxHealth = 25000;
            Ship.Shield = Ship.MaxShield = 50000;

            Ship.ShieldRegenerationRate = 3;

            ShipWeapon Cannon = new ShipWeapon();

            Cannon.ProjectileColor = new Color(0.7f, 0.9f, 1.0f, 1.0f);

            Cannon.Range = 45.0f;
            Cannon.ShieldPenetration = 0.2f;
            Cannon.ProjectileVelocity = 50.0f;
            Cannon.Damage = 6000;
            Cannon.AttackSpeed = 2500;
            Cannon.Cooldown = 0;
            Cannon.Self = Ship;

            Cannon.flags = WeaponFlags.WEAPON_PROJECTILE;

            ShipWeapon Weapon = new ShipWeapon();

            Weapon.ProjectileColor = Color.white;

            Weapon.Range = 20.0f;
            Weapon.ShieldPenetration = 0.1f;
            Weapon.ProjectileVelocity = 8.0f;
            Weapon.Damage = 200;
            Weapon.AttackSpeed = 50;
            Weapon.Cooldown = 0;
            Weapon.Self = Ship;

            Weapon.flags = WeaponFlags.WEAPON_PROJECTILE;

            Ship.Weapons.Add(Weapon);
            Ship.Weapons.Add(Cannon);
        }

        private void Update()
        {
            float CurrentTime = Time.time - LastTime;

            if (CurrentTime == 0.0f) return;

            CurrentTime *= TimeScale;

            LastTime = Time.time;

            if (Ship.DockingAutopilotLoop(CurrentTime, 0.1f * SystemScale)) return;

            if (Input.GetKeyDown("tab")) MouseSteering = !MouseSteering;

            if (!MouseSteering) Ship.Rotation -= Input.GetAxis("Horizontal") * CurrentTime * Ship.RotationSpeedModifier;
            else
            {
                Vector3 RelPos = Camera.GetRelPos(new Vector3(Ship.Self.PosX, Ship.Self.PosY, 0.0f));

                float dx = Input.mousePosition.x - RelPos.x;
                float dy = Input.mousePosition.y - RelPos.y;

                float d  = (float)Math.Sqrt(dx * dx + dy * dy);

                float angle = (float)Math.Acos(dx / d);

                if (dy < 0.0f) angle = 2.0f * 3.1415926f - angle;

                Ship.RotateTo(angle, CurrentTime);
            }

            float Movement = Input.GetAxis("Vertical");
            if (Movement == 0.0f && Input.GetKey("w")) Movement =  1.0f;
            if (Movement == 0.0f && Input.GetKey("s")) Movement = -1.0f;

            if (Movement > 0.0f && Ship.Self.VelX * Ship.Self.VelX + Ship.Self.VelY * Ship.Self.VelY < 0.1f) Reverse = false;
            if (Movement < 0.0f && Ship.Self.VelX * Ship.Self.VelX + Ship.Self.VelY * Ship.Self.VelY < 0.1f) Reverse = true;

            float AccX = (float)Math.Cos(Ship.Rotation) * Ship.Acceleration;
            float AccY = (float)Math.Sin(Ship.Rotation) * Ship.Acceleration;

            AccX *= CurrentTime * Movement;
            AccY *= CurrentTime * Movement;
            
            Ship.Self.PosX += Ship.Self.VelX * CurrentTime + AccX / 2.0f * CurrentTime;
            Ship.Self.PosY += Ship.Self.VelY * CurrentTime + AccY / 2.0f * CurrentTime;

            Ship.Self.VelX += AccX;
            Ship.Self.VelY += AccY;

            // "Sailing" and "air resistance" effects are dampened the closer the player is to a massive object
            // This is to make gravity and slingshotting more realistic and easier for the player to use.

            if (GravitationalStrength < 1.0f)
            {
                float GravitationalFactor = 1.0f / (1.0f - GravitationalStrength);

                // "Air resistance" effect
                float DragX = Ship.Self.VelX * -CurrentTime / (GravitationalFactor + DragFactor);
                float DragY = Ship.Self.VelY * -CurrentTime / (GravitationalFactor + DragFactor);

                Ship.Self.VelX *= 1.0f + DragX;
                Ship.Self.VelY *= 1.0f + DragY;

                // "Sailing" effect
                float EffectiveAccX = AccX + DragX;
                float EffectiveAccY = AccY + DragY;
                
                float SpeedMagnitude = (float)Math.Sqrt(Ship.Self.VelX * Ship.Self.VelX + Ship.Self.VelY * Ship.Self.VelY);
                float AccMagnitude   = (float)Math.Sqrt(EffectiveAccX * EffectiveAccX + EffectiveAccY * EffectiveAccY);

                Ship.Self.VelX = ((SailingFactor + GravitationalFactor) * Ship.Self.VelX + (float)Math.Cos(Ship.Rotation) * SpeedMagnitude) / (1.0f + SailingFactor + GravitationalFactor);
                Ship.Self.VelY = ((SailingFactor + GravitationalFactor) * Ship.Self.VelY + (float)Math.Sin(Ship.Rotation) * SpeedMagnitude) / (1.0f + SailingFactor + GravitationalFactor);
            }

            Renderer.shipColor.b = (float) Ship.Health / Ship.MaxHealth;

            foreach (ShipWeapon Weapon in Ship.Weapons)
            {
                Weapon.Cooldown -= (int)(CurrentTime * 1000.0f);
                if (Weapon.Cooldown < 0) Weapon.Cooldown = 0;
            }

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

            if (Input.GetKey("space"))
                foreach (ShipWeapon Weapon in Ship.Weapons)
                    Weapon.Fire(Ship.Self.PosX + (float)Math.Cos(Ship.Rotation), Ship.Self.PosY + (float)Math.Sin(Ship.Rotation));
        }

        void OnDestroy()
        {
            GameObject.Destroy(Renderer);
            GameObject.Destroy(Object);
        }
    }   
}
