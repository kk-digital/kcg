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

            SystemEnemy Enemy = gameObject.AddComponent<SystemEnemy>();

            Enemies.Add(Enemy);

            Player = gameObject.AddComponent<PlayerShip>();

            State.Star = new SystemStar();
            State.Star.PosX = 0.0f;
            State.Star.PosY = 0.0f;

            var StarObject = new GameObject();
            StarObject.name = "Star Renderer";

            SystemStarRenderer starRenderer = StarObject.AddComponent<SystemStarRenderer>();
            starRenderer.Star = State.Star;

            Invoke("ReadyShips", 1);
        }

        void ReadyShips()
        {
            State.Ships.Add(Player.Ship);
            State.Ships.Add(Enemies[0].Ship);
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
                }
            }
        }
    }
}
