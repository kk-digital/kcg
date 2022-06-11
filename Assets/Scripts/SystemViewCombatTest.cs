using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SystemView
{
    // this is pretty messy but its just a quick and dirty test script
    public class SystemViewCombatTest : MonoBehaviour
    {
        public PlayerShip        Player;
        public List<SystemEnemy> Enemies = new();

        public List<SystemEnemy> PendingEnemies = new();
        public List<LaserTower>  PendingLasers  = new();
        public List<GameObject>  LaserObjects   = new();

        public Dictionary<ShipWeaponProjectile, GameObject> ProjectileRenderers = new();

        public SystemEnemy SelectedEnemy;

        public SystemState State;

        public int LastTime;

        public Dropdown EnemySelectorMenu;

        void Start()
        {
            LastTime = (int)(Time.time * 1000.0f);

            GameLoop gl = GetComponent<GameLoop>();

            State = gl.CurrentSystemState;

            State.Star.Mass = 5000000.0f;
            State.Star.PosX = -5.0f;
            State.Star.PosY = 0.0f;

            RespawnPlayer();

            var StarObject = new GameObject();
            StarObject.name = "Star Renderer";

            SystemStarRenderer starRenderer = StarObject.AddComponent<SystemStarRenderer>();
            starRenderer.Star = State.Star;
        }

        void LateUpdate()
        {
            if (Player != null && State.Player == null)
            {
                State.Player = Player;
                State.Ships.Add(Player.Ship);
            }

            while (PendingEnemies.Count > 0)
            {
                State.Ships.Add(PendingEnemies[0].Ship);
                Enemies.Add(PendingEnemies[0]);
                PendingEnemies.RemoveAt(0);
            }

            while (PendingLasers.Count > 0)
            {
                State.LaserTowers.Add(PendingLasers[0]);
                PendingLasers[0].State = State;
                PendingLasers.RemoveAt(0);
            }

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
                        GameObject.Destroy(Player);
                        Player = null;
                    }
                    else
                    {
                        for (int j = 0; j < Enemies.Count; j++)
                        {
                            if (Enemies[j].Ship == Ship)
                            {
                                SystemEnemy Enemy = Enemies[j];
                                Enemies.Remove(Enemy);
                                
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

            UpdateDropdownMenu();
        }

        public void RespawnPlayer()
        {
            if (Player != null)
            {
                State.Ships.Remove(Player.Ship);
                GameObject.Destroy(Player);
                State.Player = null;
            }

            Player = gameObject.AddComponent<PlayerShip>();

            // todo: Ship can't fire after being respawned. Fix it
        }

        public void AddEnemy()
        {
            PendingEnemies.Add(gameObject.AddComponent<SystemEnemy>());
        }

        public void AddLaserTower()
        {
            GameObject O = new GameObject();
            PendingLasers.Add(O.AddComponent<LaserTower>());
            LaserObjects.Add(O);
        }

        public void Reset()
        {
            while (Enemies.Count > 0)
            {
                GameObject.Destroy(Enemies[0]);
                Enemies.RemoveAt(0);
            }

            State.Ships.Clear();

            while (State.LaserTowers.Count > 0)
            {
                GameObject.Destroy(State.LaserTowers[0]);
                GameObject.Destroy(LaserObjects[0]);
                State.LaserTowers.RemoveAt(0);
                LaserObjects.RemoveAt(0);
            }

            while (ProjectileRenderers.Count > 0)
            {
                KeyValuePair<ShipWeaponProjectile, GameObject> ProjectileRenderer = ProjectileRenderers.ElementAt(0);
                ShipWeaponProjectile Projectile = ProjectileRenderer.Key;
                GameObject Renderer = ProjectileRenderer.Value;

                GameObject.Destroy(Renderer);
                ProjectileRenderers.Remove(Projectile);
            }

            RespawnPlayer();
        }

        private void UpdateDropdownMenu()
        {
            EnemySelectorMenu.ClearOptions();

            List<string> Options = new();

            if (Enemies.Count == 0)
            {
                Options.Add("-- No enemies --");

                EnemySelectorMenu.interactable = false;
            }
            else
            {
                Options.Add("-- Select an enemy --");

                for (int i = 0; i < Enemies.Count;)
                    Options.Add("Enemy " + ++i);

                EnemySelectorMenu.interactable = true;
            }

            EnemySelectorMenu.AddOptions(Options);

            bool found = false;
            for (int i = 0; i < Enemies.Count; i++)
                if (Enemies[i] == SelectedEnemy)
                {
                    found = true;
                    EnemySelectorMenu.value = i + 1;
                    break;
                }

            if (!found)
                EnemySelectorMenu.value = 0;
        }

        public void SelectEnemy(int i)
        {
            if (i == 0)
            {
                SelectedEnemy = null;
            }
            else
            {
                SelectedEnemy = Enemies.ElementAt(i - 1);
            }
        }
    }
}
