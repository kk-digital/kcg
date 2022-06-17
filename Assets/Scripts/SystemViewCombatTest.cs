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

        public Slider   SemiMajorAxisSlider;
        public Slider   SemiMinorAxisSlider;
        public Slider   RotationSlider;
        public Slider   MaxHealthSlider;
        public Slider   MaxShieldSlider;
        public Slider   ShieldRegenerationSlider;
        public Slider   WeaponCooldownSlider;
        public Slider   WeaponRangeSlider;
        public Slider   WeaponDamageSlider;
        public Slider   ShieldPenetrationSlider;
        public Slider   ProjectileVelocitySlider;

        public Button   UpdateEnemyButton;
        public Button   DeleteEnemyButton;

        void Start()
        {
            LastTime = (int)(Time.time * 1000.0f);

            State.Star.mass = 5000000.0f;
            State.Star.posx = -5.0f;
            State.Star.posy = 0.0f;

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
                State.Ships.Add(Player.ship);
            }

            while (PendingEnemies.Count > 0)
            {
                State.Ships.Add(PendingEnemies[0].ship);
                Enemies.Add(PendingEnemies[0]);
                PendingEnemies.RemoveAt(0);
            }

            while (PendingLasers.Count > 0)
            {
                State.LaserTowers.Add(PendingLasers[0]);
                PendingLasers[0].State = State;
                PendingLasers.RemoveAt(0);
            }

            if (SelectedEnemy != null && !Enemies.Contains(SelectedEnemy))
            {
                SelectEnemy(0);
            }

            if (SelectedEnemy != null)
            {
                if (SemiMajorAxisSlider.value < SemiMinorAxisSlider.value)
                    SemiMajorAxisSlider.value = SemiMinorAxisSlider.value;
                SemiMajorAxisSlider.minValue = SemiMinorAxisSlider.value;
            }

            int current_millis = (int)(Time.time * 1000) - LastTime;
            LastTime = (int)(Time.time * 1000);

            for (int i = 0; i < ProjectileRenderers.Count; i++)
            {
                KeyValuePair<ShipWeaponProjectile, GameObject> ProjectileRenderer = ProjectileRenderers.ElementAt(i);
                ShipWeaponProjectile Projectile = ProjectileRenderer.Key;
                GameObject Renderer = ProjectileRenderer.Value;

                if (Projectile.UpdatePosition(current_millis / 1000.0f))
                {
                    foreach (SystemShip ship in State.Ships)
                    {
                        if (ship == Projectile.Self) continue;

                        if (Projectile.InRangeOf(ship, 1.0f))
                        {
                            Projectile.DoDamage(ship);
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
                SystemShip ship = State.Ships[i];

                foreach (ShipWeapon Weapon in ship.weapons)
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

                if (ship.destroyed)
                {
                    State.Ships.Remove(ship);
                    if (ship == Player.ship)
                    {
                        GameObject.Destroy(Player);
                        Player = null;
                    }
                    else
                    {
                        for (int j = 0; j < Enemies.Count; j++)
                        {
                            if (Enemies[j].ship == ship)
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

                ship.shield += ship.shield_regeneration_rate * current_millis;
                if (ship.shield > ship.max_shield) ship.shield = ship.max_shield;
            }

            foreach (SystemEnemy Enemy in Enemies)
            {
                Enemy.ship.rotation = Enemy.ship.descriptor.true_anomaly + Enemy.ship.descriptor.rotation + Tools.halfpi;

                foreach (ShipWeapon Weapon in Enemy.ship.weapons)
                {
                    Weapon.TryFiringAt(Player.ship, current_millis);
                }
            }

            UpdateDropdownMenu();
        }

        public void RespawnPlayer()
        {
            if (Player != null)
            {
                State.Ships.Remove(Player.ship);
                GameObject.Destroy(Player);
                State.Player = null;
            }

            Player = gameObject.AddComponent<PlayerShip>();

            // todo: ship can't fire after being respawned. Fix it
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

            SelectEnemy(0);
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
            EnemySelectorMenu.value = 0;

            for (int i = 0; i < Enemies.Count; i++)
                if (Enemies[i] == SelectedEnemy)
                {
                    EnemySelectorMenu.value = i + 1;
                    break;
                }
        }

        public void SelectEnemy(int i)
        {
            if (i == 0 || i > Enemies.Count)
            {
                SelectedEnemy                         = null;

                SemiMajorAxisSlider.interactable      = false;
                SemiMinorAxisSlider.interactable      = false;
                RotationSlider.interactable           = false;
                MaxHealthSlider.interactable          = false;
                MaxShieldSlider.interactable          = false;
                ShieldRegenerationSlider.interactable = false;
                WeaponCooldownSlider.interactable     = false;
                WeaponRangeSlider.interactable        = false;
                WeaponDamageSlider.interactable       = false;
                ShieldPenetrationSlider.interactable  = false;
                ProjectileVelocitySlider.interactable = false;

                UpdateEnemyButton.interactable        = false;
                DeleteEnemyButton.interactable        = false;
            }
            else if (i <= Enemies.Count && SelectedEnemy != Enemies.ElementAt(i - 1))
            {
                SelectedEnemy                         = Enemies.ElementAt(i - 1);

                SemiMajorAxisSlider.interactable      = true;
                SemiMinorAxisSlider.interactable      = true;
                RotationSlider.interactable           = true;
                MaxHealthSlider.interactable          = true;
                MaxShieldSlider.interactable          = true;
                ShieldRegenerationSlider.interactable = true;
                WeaponCooldownSlider.interactable     = true;
                WeaponRangeSlider.interactable        = true;
                WeaponDamageSlider.interactable       = true;
                ShieldPenetrationSlider.interactable  = true;
                ProjectileVelocitySlider.interactable = true;

                UpdateEnemyButton.interactable        = true;
                DeleteEnemyButton.interactable        = true;

                SemiMajorAxisSlider.value             = SelectedEnemy.ship.descriptor.semimajoraxis;
                SemiMinorAxisSlider.value             = SelectedEnemy.ship.descriptor.semiminoraxis;
                RotationSlider.value                  = SelectedEnemy.ship.descriptor.rotation;
                MaxHealthSlider.value                 = SelectedEnemy.ship.max_health;
                MaxShieldSlider.value                 = SelectedEnemy.ship.max_shield;
                ShieldRegenerationSlider.value        = SelectedEnemy.ship.shield_regeneration_rate;
                WeaponCooldownSlider.value            = SelectedEnemy.ship.weapons[0].AttackSpeed;
                WeaponRangeSlider.value               = SelectedEnemy.ship.weapons[0].Range;
                WeaponDamageSlider.value              = SelectedEnemy.ship.weapons[0].Damage;
                ShieldPenetrationSlider.value         = SelectedEnemy.ship.weapons[0].ShieldPenetration;
                ProjectileVelocitySlider.value        = SelectedEnemy.ship.weapons[0].ProjectileVelocity;
            }
        }

        public void UpdateEnemy()
        {
            if (SelectedEnemy != null && Enemies.Contains(SelectedEnemy))
            {
                SelectedEnemy.ship.descriptor.semimajoraxis      =      SemiMajorAxisSlider.value;
                SelectedEnemy.ship.descriptor.semiminoraxis      =      SemiMinorAxisSlider.value;
                SelectedEnemy.ship.descriptor.rotation           =      RotationSlider.value;
                SelectedEnemy.ship.max_health                    = (int)MaxHealthSlider.value;
                SelectedEnemy.ship.max_shield                    = (int)MaxShieldSlider.value;
                SelectedEnemy.ship.shield_regeneration_rate      = (int)ShieldRegenerationSlider.value;
                SelectedEnemy.ship.weapons[0].AttackSpeed        = (int)WeaponCooldownSlider.value;
                SelectedEnemy.ship.weapons[0].Range              =      WeaponRangeSlider.value;
                SelectedEnemy.ship.weapons[0].Damage             = (int)WeaponDamageSlider.value;
                SelectedEnemy.ship.weapons[0].ShieldPenetration  =      ShieldPenetrationSlider.value;
                SelectedEnemy.ship.weapons[0].ProjectileVelocity =      ProjectileVelocitySlider.value;

                if (SelectedEnemy.ship.health > SelectedEnemy.ship.max_health)
                    SelectedEnemy.ship.health = SelectedEnemy.ship.max_health;

                SelectedEnemy.ship.descriptor.compute();
            }
        }

        public void DeleteEnemy()
        {
            if (SelectedEnemy != null && Enemies.Contains(SelectedEnemy))
            {
                State.Ships.Remove(SelectedEnemy.ship);
                Enemies.Remove(SelectedEnemy);
                GameObject.Destroy(SelectedEnemy);
                SelectEnemy(0);
            }
        }
    }
}
