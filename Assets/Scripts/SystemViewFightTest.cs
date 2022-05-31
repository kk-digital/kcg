using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public struct ShipInfo
    {
        public GameObject Object;
        public SystemShipRenderer Renderer;
    }

    public class SystemViewFightTest : MonoBehaviour
    {
        public SystemState State;

        public int LastTime;

        public Dictionary<SystemShip, ShipInfo> Ships = new Dictionary<SystemShip, ShipInfo>();
        public Dictionary<ShipWeaponProjectile, GameObject> ProjectileRenderers = new Dictionary<ShipWeaponProjectile, GameObject>();

        System.Random rand = new System.Random();

        void Start()
        {
            GameLoop gl = GetComponent<GameLoop>();

            State = gl.CurrentSystemState;

            for(int i = 0; i < 64; i++)
            {
                ShipInfo Info = new ShipInfo();

                SystemShip Ship = new SystemShip();

                Ship.Health = Ship.MaxHealth = Ship.Shield = Ship.MaxShield = 10000;
                Ship.ShieldRegenerationRate = 1;

                Ship.PosX = (float)rand.NextDouble() * 30.0f - 15.0f;
                Ship.PosY = (float)rand.NextDouble() * 30.0f - 15.0f;

                ShipWeapon Weapon = new ShipWeapon();

                Weapon.ProjectileColor = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1.0f);
                Weapon.Range = 50.0f;
                Weapon.ShieldPenetration = (float)rand.NextDouble() * 0.3f;
                Weapon.Damage = rand.Next(200, 800);
                Weapon.AttackSpeed = rand.Next(200, 800);
                Weapon.Cooldown = 0;
                Weapon.Self = Ship;
                Weapon.ProjectileVelocity = (float)rand.NextDouble() * 1.5f + 1.0f;

                Ship.Weapons.Add(Weapon);

                Info.Object = new GameObject();
                Info.Object.name = "Fighter " + (i + 1) + " Renderer";

                Info.Renderer = Info.Object.AddComponent<SystemShipRenderer>();
                Info.Renderer.ship = Ship;
                Info.Renderer.shipColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);

                Ships.Add(Ship, Info);
                State.Ships.Add(Ship);
            }

            LastTime = (int)(Time.time * 1000);
        }

        void Update()
        {
            int CurrentMillis = (int)(Time.time * 1000) - LastTime;
            LastTime = (int)(Time.time * 1000);

            for (int i = 0; i < ProjectileRenderers.Count; i++)
            {
                KeyValuePair<ShipWeaponProjectile, GameObject> ProjectileRenderer = ProjectileRenderers.ElementAt(i);
                ShipWeaponProjectile Projectile = ProjectileRenderer.Key;
                GameObject Renderer = ProjectileRenderer.Value;

                if (Projectile.UpdatePosition(CurrentMillis / 200.0f))
                {
                    foreach (SystemShip Ship in State.Ships)
                    {
                        if (Ship == Projectile.Self) continue;

                        if (Projectile.InRangeOf(Ship))
                        {
                            Projectile.DoDamage(Ship);

                            GameObject.Destroy(ProjectileRenderers[Projectile]);
                            ProjectileRenderers.Remove(Projectile);
                            i--;

                            break;
                        }
                    }
                }
                else
                {
                    GameObject.Destroy(ProjectileRenderers[Projectile]);
                    ProjectileRenderers.Remove(Projectile);
                    i--;
                }
            }

            for (int i = 0; i < State.Ships.Count; i++)
            {
                SystemShip Ship = State.Ships[i];

                if (Ship.Destroyed)
                {
                    GameObject.Destroy(Ships[Ship].Renderer.ShieldObject);
                    GameObject.Destroy(Ships[Ship].Object);

                    Ships.Remove(Ship);
                    State.Ships.Remove(Ship);

                    i--;
                    continue;
                }

                if(Ships.Count > 1) foreach (ShipWeapon Weapon in Ship.Weapons)
                {
                    SystemShip Target = null;

                    do
                    {
                        Target = State.Ships[rand.Next(Ships.Count)];
                    }
                    while (Target == Ship);

                    if (Weapon.TryFiringAt(Target, CurrentMillis))
                    {
                        GameObject RendererObject = new GameObject();
                        RendererObject.name = "Fighter Projectile";

                        ShipWeaponProjectileRenderer ProjectileRenderer = RendererObject.AddComponent<ShipWeaponProjectileRenderer>();

                        ProjectileRenderer.Projectile = Weapon.ProjectilesFired[Weapon.ProjectilesFired.Count - 1];

                        ProjectileRenderers.Add(Weapon.ProjectilesFired[Weapon.ProjectilesFired.Count - 1], RendererObject);
                    }
                }

                Ship.Shield += Ship.ShieldRegenerationRate * CurrentMillis;
                if (Ship.Shield > Ship.MaxShield) Ship.Shield = Ship.MaxShield;

                Ships[Ship].Renderer.shipColor.g = (float)Ship.Health / Ship.MaxHealth;
                Ships[Ship].Renderer.shipColor.r = 1.0f - Ships[Ship].Renderer.shipColor.g;
            }
        }
    }
}
