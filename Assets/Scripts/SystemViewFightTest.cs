using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class SystemViewFightTest : MonoBehaviour
    {
        public SystemState State;

        public SystemShip Fighter1;
        public SystemShip Fighter2;

        public GameObject Fighter1Object;
        public GameObject Fighter2Object;

        public SystemShipRenderer Fighter1Renderer;
        public SystemShipRenderer Fighter2Renderer;

        public int LastTime;

        public List<GameObject> ProjectileRenderers;

        void Start()
        {
            GameLoop gl = GetComponent<GameLoop>();

            State = gl.CurrentSystemState;

            Fighter1 = new SystemShip();
            Fighter1.Health = Fighter1.MaxHealth = Fighter1.Shield = Fighter1.MaxShield = 1000;
            Fighter1.ShieldRegenerationRate = 1;

            Fighter1.PosX = -5.0f;
            Fighter1.PosY = 0.0f;

            ShipWeapon Fighter1Weapon = new ShipWeapon();

            Fighter1Weapon.ProjectileColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            Fighter1Weapon.Range = 20.0f;
            Fighter1Weapon.ShieldPenetration = 0.1f;
            Fighter1Weapon.Damage = 600;
            Fighter1Weapon.AttackSpeed = 400;
            Fighter1Weapon.Cooldown = 0;

            Fighter1.Weapons.Add(Fighter1Weapon);

            Fighter2 = new SystemShip();
            Fighter2.Health = Fighter2.MaxHealth = Fighter2.Shield = Fighter2.MaxShield = 1000;
            Fighter1.ShieldRegenerationRate = 2;

            Fighter2.PosX = 5.0f;
            Fighter2.PosY = 0.0f;

            ShipWeapon Fighter2Weapon = new ShipWeapon();

            Fighter2Weapon.ProjectileColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            Fighter2Weapon.Range = 20.0f;
            Fighter2Weapon.ShieldPenetration = 0.2f;
            Fighter2Weapon.Damage = 1800;
            Fighter2Weapon.AttackSpeed = 1500;
            Fighter2Weapon.Cooldown = 0;

            Fighter2.Weapons.Add(Fighter2Weapon);

            State.Ships.Add(Fighter1);
            State.Ships.Add(Fighter2);

            Fighter1Object = new GameObject();
            Fighter2Object = new GameObject();

            Fighter1Object.name = "Fighter 1 Renderer";
            Fighter2Object.name = "Fighter 2 Renderer";

            Fighter1Renderer = Fighter1Object.AddComponent<SystemShipRenderer>();
            Fighter2Renderer = Fighter2Object.AddComponent<SystemShipRenderer>();

            Fighter1Renderer.ship = Fighter1;
            Fighter2Renderer.ship = Fighter2;

            LastTime = (int)(Time.time * 1000);
        }

        void Update()
        {
            int CurrentTime = (int)(Time.time * 1000) - LastTime;
            LastTime = (int)(Time.time * 1000);

            foreach (ShipWeapon Weapon in Fighter1.Weapons)
            {
                if (Weapon.TryFiringAt(Fighter2, CurrentTime))
                {
                    GameObject RendererObject = new GameObject();
                    RendererObject.name = "Fighter 1 Projectile " + Weapon.ProjectilesFired.Count;

                    ShipWeaponProjectileRenderer ProjectileRenderer = RendererObject.AddComponent<ShipWeaponProjectileRenderer>();

                    ProjectileRenderer.Projectile = Weapon.ProjectilesFired[Weapon.ProjectilesFired.Count - 1];

                    ProjectileRenderers.Add(RendererObject);
                }

                foreach (ShipWeaponProjectile Projectile in Weapon.ProjectilesFired)
                {
                    if(Projectile.UpdatePosition(0.1f))
                    {
                        if(Projectile.InRangeOf(Fighter2))
                        {
                            Projectile.DoDamage(Fighter2);

                            // todo: remove renderer for projectile
                        }
                    }
                    else
                    {
                        // todo: remove renderer for projectile
                    }
                }
            }

            foreach (ShipWeapon Weapon in Fighter2.Weapons)
            {
                if (Weapon.TryFiringAt(Fighter1, CurrentTime))
                {
                    GameObject RendererObject = new GameObject();
                    RendererObject.name = "Fighter 2 Projectile " + Weapon.ProjectilesFired.Count;

                    ShipWeaponProjectileRenderer ProjectileRenderer = RendererObject.AddComponent<ShipWeaponProjectileRenderer>();

                    ProjectileRenderer.Projectile = Weapon.ProjectilesFired[Weapon.ProjectilesFired.Count - 1];

                    ProjectileRenderers.Add(RendererObject);
                }

                foreach (ShipWeaponProjectile Projectile in Weapon.ProjectilesFired)
                {
                    if (Projectile.UpdatePosition(0.1f))
                    {
                        if (Projectile.InRangeOf(Fighter1))
                        {
                            Projectile.DoDamage(Fighter1);

                            // todo: remove renderer for projectile
                        }
                    }
                    else
                    {
                        // todo: remove renderer for projectile
                    }
                }
            }
        }
    }
}
