using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SystemView
{
    // this is pretty messy but its just a quick and dirty test script
    public class SystemViewCombatTest : MonoBehaviour
    {
        public PlayerShip        Player;
        public List<SystemEnemy> Enemies = new();

        public Dictionary<ShipWeaponProjectile, GameObject> ProjectileRenderers = new();

        public SystemState State;

        public int LastTime;

        void Start()
        {
            LastTime = (int)(Time.time * 1000.0f);

            GameLoop gl = GetComponent<GameLoop>();

            State = gl.CurrentSystemState;

            State.Star.Mass = 5000000.0f;
            State.Star.PosX = 0.0f;
            State.Star.PosY = 0.0f;

            SystemEnemy Enemy = gameObject.AddComponent<SystemEnemy>();

            Enemies.Add(Enemy);

            Player = gameObject.AddComponent<PlayerShip>();

            var StarObject = new GameObject();
            StarObject.name = "Star Renderer";

            SystemStarRenderer starRenderer = StarObject.AddComponent<SystemStarRenderer>();
            starRenderer.Star = State.Star;

            Invoke("ReadyShips", 1);

            State.LaserTowers.Add((new GameObject()).AddComponent<LaserTower>());
            State.LaserTowers.Add((new GameObject()).AddComponent<LaserTower>());
            State.LaserTowers.Add((new GameObject()).AddComponent<LaserTower>());
            State.LaserTowers.Add((new GameObject()).AddComponent<LaserTower>());
        }

        void ReadyShips()
        {
            State.Ships.Add(Player.Ship);
            State.Ships.Add(Enemies[0].Ship);

            foreach (LaserTower Laser in State.LaserTowers)
                Laser.State = State;
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

                if (Projectile.UpdatePosition(CurrentMillis / 1000.0f))
                {
                    foreach (SystemShip Ship in State.Ships)
                    {
                        if (Ship == Projectile.Self) continue;

                        if (Projectile.InRangeOf(Ship, 1.0f))
                        {
                            Projectile.DoDamage(Ship);
                            Projectile.Weapon.ProjectilesFired.Remove(Projectile);

                            GameObject.Destroy(ProjectileRenderers[Projectile]);
                            ProjectileRenderers.Remove(Projectile);
                            i--;

                            break;
                        }
                    }
                }
                else
                {
                    Projectile.Weapon.ProjectilesFired.Remove(Projectile);

                    GameObject.Destroy(ProjectileRenderers[Projectile]);
                    ProjectileRenderers.Remove(Projectile);
                    i--;
                }
            }

            for (int i = 0; i < State.Ships.Count; i++)
            {
                SystemShip Ship = State.Ships[i];

                foreach (ShipWeapon Weapon in Ship.Weapons)
                {
                    foreach (ShipWeaponProjectile Projectile in Weapon.ProjectilesFired)
                    {
                        if (!ProjectileRenderers.ContainsKey(Projectile))
                        {
                            GameObject ProjectileRenderer = new GameObject();
                            ProjectileRenderer.name = "Projectile renderer";

                            ShipWeaponProjectileRenderer Renderer = ProjectileRenderer.AddComponent<ShipWeaponProjectileRenderer>();    
                            Renderer.Projectile = Projectile;

                            ProjectileRenderers.Add(Projectile, ProjectileRenderer);
                        }
                    }
                }

                if (Ship.Destroyed)
                {
                    State.Ships.Remove(Ship);
                    if (Ship == Player.Ship)
                    {
                        GameObject.Destroy(Player.Renderer.ShieldObject);
                        GameObject.Destroy(Player.Object);
                        GameObject.Destroy(Player);
                    }
                    else
                    {
                        for (int j = 0; j < Enemies.Count; j++)
                        {
                            if (Enemies[j].Ship == Ship)
                            {
                                SystemEnemy Enemy = Enemies[j];
                                Enemies.Remove(Enemy);

                                GameObject.Destroy(Enemy.Renderer.ShieldObject);
                                GameObject.Destroy(Enemy.Object);
                                GameObject.Destroy(Enemy);
                                break;
                            }
                        }
                    }

                    i--;
                    continue;
                }

                Ship.Shield += Ship.ShieldRegenerationRate * CurrentMillis;
                if (Ship.Shield > Ship.MaxShield) Ship.Shield = Ship.MaxShield;
            }

            foreach (SystemEnemy Enemy in Enemies)
            {
                Enemy.Ship.Rotation = Enemy.Ship.Descriptor.TrueAnomaly + Enemy.Ship.Descriptor.Rotation + 3.1415926f * 0.5f;

                foreach (ShipWeapon Weapon in Enemy.Ship.Weapons)
                {
                    Weapon.TryFiringAt(Player.Ship, CurrentMillis);
                }
            }
        }
    }
}
