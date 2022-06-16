﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class PlayerShip : MonoBehaviour
    {
        public SystemShip ship;

        public GameObject o;
        public SystemShipRenderer renderer;

        public float LastTime;

        public bool RenderOrbit = true;

        public float GravitationalStrength = 0.0f;

        public float TimeScale = 1.0f;
        public float DragFactor = 10000.0f;
        public float SailingFactor = 200.0f;
        public float SystemScale = 1.0f;

        public bool  MouseSteering = false;
        public CameraController Camera;

        public SystemState State;

        public List<LineRenderer> LaserLineRenderers;
        public List<GameObject>   LaserLineObjects;

        private void Start()
        {
            Camera   = GameObject.Find("Main Camera").GetComponent<CameraController>();

            State    = GetComponent<GameLoop>().CurrentSystemState;

            LastTime = Time.time * 1000.0f;

            ship = new SystemShip();

            o = new GameObject();
            o.name = "Player ship";

            ship.self.posx = 0.0f;
            ship.self.posy = 0.0f;
            ship.Acceleration = 5.0f;

            ship.self.mass = 1.0f;

            renderer = o.AddComponent<SystemShipRenderer>();
            renderer.ship = ship;
            renderer.shipColor = Color.blue;
            renderer.width = 3.0f;

            ship.Health = ship.MaxHealth = 25000;
            ship.Shield = ship.MaxShield = 50000;

            ship.ShieldRegenerationRate = 3;

            ShipWeapon Cannon = new ShipWeapon();

            Cannon.ProjectileColor = new Color(0.7f, 0.9f, 1.0f, 1.0f);

            Cannon.Range = 45.0f;
            Cannon.ShieldPenetration = 0.2f;
            Cannon.ProjectileVelocity = 50.0f;
            Cannon.Damage = 6000;
            Cannon.AttackSpeed = 2500;
            Cannon.Cooldown = 0;
            Cannon.Self = ship;

            Cannon.flags = WeaponFlags.WEAPON_PROJECTILE;

            ShipWeapon Weapon = new ShipWeapon();

            Weapon.ProjectileColor = Color.white;

            Weapon.Range = 20.0f;
            Weapon.ShieldPenetration = 0.1f;
            Weapon.ProjectileVelocity = 8.0f;
            Weapon.Damage = 200;
            Weapon.AttackSpeed = 50;
            Weapon.Cooldown = 0;
            Weapon.Self = ship;

            Weapon.flags = WeaponFlags.WEAPON_PROJECTILE;

            ship.Weapons.Add(Weapon);
            ship.Weapons.Add(Cannon);
        }

        private void Update()
        {
            float CurrentTime = Time.time - LastTime;

            if (CurrentTime == 0.0f) return;

            CurrentTime *= TimeScale;

            LastTime = Time.time;

            if (ship.DockingAutopilotLoop(CurrentTime, 0.1f * SystemScale)) return;

            if (Input.GetKeyDown("tab")) MouseSteering = !MouseSteering;

            float HorizontalMovement = 0.0f;

            if (!MouseSteering)
            {
                if (Input.GetKey("left ctrl")) HorizontalMovement = Input.GetAxis("Horizontal");
                else ship.Rotation -= Input.GetAxis("Horizontal") * CurrentTime * ship.RotationSpeedModifier;
            }
            else
            {
                HorizontalMovement = -Input.GetAxis("Horizontal");
                Vector3 RelPos = Camera.GetRelPos(new Vector3(ship.self.posx, ship.self.posy, 0.0f));

                float dx = Input.mousePosition.x - RelPos.x;
                float dy = Input.mousePosition.y - RelPos.y;

                float d  = (float)Math.Sqrt(dx * dx + dy * dy);

                float angle = (float)Math.Acos(dx / d);

                if (dy < 0.0f) angle = 2.0f * 3.1415926f - angle;

                ship.RotateTo(angle, CurrentTime);
            }

            float Movement = Input.GetAxis("Vertical");
            if (Movement == 0.0f && Input.GetKey("w")) Movement =  1.0f;
            if (Movement == 0.0f && Input.GetKey("s")) Movement = -1.0f;

            float accx = (float)Math.Cos(ship.Rotation) * Movement - (float)Math.Sin(ship.Rotation) * HorizontalMovement;
            float accy = (float)Math.Sin(ship.Rotation) * Movement + (float)Math.Cos(ship.Rotation) * HorizontalMovement;

            accx *= CurrentTime * ship.Acceleration;
            accy *= CurrentTime * ship.Acceleration;
            
            if (HorizontalMovement != 0.0f && Movement != 0.0f)
            {
                                                 //  1
                const float rsqrt2 = 0.7071068f; // ---
                                                 // √ 2

                accx *= rsqrt2;
                accy *= rsqrt2;
            }

            ship.self.posx += ship.self.velx * CurrentTime + accx / 2.0f * CurrentTime;
            ship.self.posy += ship.self.vely * CurrentTime + accy / 2.0f * CurrentTime;

            ship.self.velx += accx;
            ship.self.vely += accy;

            // "Sailing" and "air resistance" effects are dampened the closer the player is to a massive object
            // This is to make gravity and slingshotting more realistic and easier for the player to use.

            if (GravitationalStrength < 1.0f)
            {
                float GravitationalFactor = 1.0f / (1.0f - GravitationalStrength);

                // "Air resistance" effect
                float DragX = ship.self.velx * -CurrentTime / (GravitationalFactor + DragFactor);
                float DragY = ship.self.vely * -CurrentTime / (GravitationalFactor + DragFactor);

                ship.self.velx *= 1.0f + DragX;
                ship.self.vely *= 1.0f + DragY;

                // "Sailing" effect
                float Effectiveaccx = accx + DragX;
                float Effectiveaccy = accy + DragY;
                
                float SpeedMagnitude = (float)Math.Sqrt(ship.self.velx * ship.self.velx + ship.self.vely * ship.self.vely);
                float AccMagnitude   = (float)Math.Sqrt(Effectiveaccx * Effectiveaccx + Effectiveaccy * Effectiveaccy);

                ship.self.velx = ((SailingFactor + GravitationalFactor) * ship.self.velx + (float)Math.Cos(ship.Rotation) * SpeedMagnitude) / (1.0f + SailingFactor + GravitationalFactor);
                ship.self.vely = ((SailingFactor + GravitationalFactor) * ship.self.vely + (float)Math.Sin(ship.Rotation) * SpeedMagnitude) / (1.0f + SailingFactor + GravitationalFactor);
            }

            renderer.shipColor.b = (float) ship.Health / ship.MaxHealth;

            foreach (ShipWeapon Weapon in ship.Weapons)
            {
                Weapon.Cooldown -= (int)(CurrentTime * 1000.0f);
                if (Weapon.Cooldown < 0) Weapon.Cooldown = 0;
            }

            if (RenderOrbit)
            {
                if (ship.Descriptor.central_body == null)
                    ship.Descriptor.central_body = State.Star;

                SpaceObject StrongestGravityBody = null;
                float g = 0.0f;

                foreach (SpaceObject obj in State.Objects)
                {
                    float dx = obj.posx - State.Player.ship.self.posx;
                    float dy = obj.posy - State.Player.ship.self.posy;

                    float d2 = dx * dx + dy * dy;

                    float curg = 6.67408E-11f * obj.mass / d2;

                    if (curg > g)
                    {
                        g = curg;
                        StrongestGravityBody = obj;
                    }
                }

                if (StrongestGravityBody != null)
                    ship.Descriptor.ChangeFrameOfReference(StrongestGravityBody);

                if (ship.Descriptor.eccentricity <= 1.0f)
                    ship.PathPlanned = true;
                else
                    ship.PathPlanned = false;
            }

            if (!MouseSteering)
            {
                if (Input.GetKey("space"))
                {
                    foreach (ShipWeapon Weapon in ship.Weapons)
                    {
                        float x = ship.self.posx + (float)Math.Cos(ship.Rotation) * Weapon.Range;
                        float y = ship.self.posy + (float)Math.Sin(ship.Rotation) * Weapon.Range;

                        if ((Weapon.flags & WeaponFlags.WEAPON_LASER) != 0)
                        {

                        }
                        else Weapon.Fire(x, y);
                    }
                }

                Camera.DisableDragging = false;
            }
            else
            {
                if (Input.GetKey("space") || Input.GetMouseButton(0))
                {

                    Vector3 MousePosition = Camera.GetAbsPos(Input.mousePosition);

                    foreach (ShipWeapon Weapon in ship.Weapons)
                    {
                        if ((Weapon.flags & WeaponFlags.WEAPON_LASER) != 0)
                        {

                        }
                        else Weapon.Fire(MousePosition.x, MousePosition.y);
                    }
                }

                Camera.DisableDragging = true;
            }
        }

        void OnDestroy()
        {
            GameObject.Destroy(renderer);
            GameObject.Destroy(o);
        }
    }   
}
