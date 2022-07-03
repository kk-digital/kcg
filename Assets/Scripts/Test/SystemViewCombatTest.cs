using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        // this is pretty messy but its just a quick and dirty test script
        public class SystemViewCombatTest : MonoBehaviour {
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

            public float    rudder_speed;
            public float    rudder_strength;
            public bool     rudder_enabled = true;

            public void set_rudder_speed(float f) {
                rudder_speed = f;
                if(Player != null) Player.sail_speed = f;
            }

            public void set_rudder_strength(float f) {
                rudder_strength = f;
                if(Player != null) Player.sailing_factor = 50 - f;
            }

            public void toggle_rudder(bool b) {
                rudder_enabled = b;
                if(Player != null) Player.rudder_enabled = rudder_enabled;
            }

            void Start() {
                LastTime = (int)(Time.time * 1000.0f);

                State.star.mass = 5000000.0f;
                State.star.posx = -5.0f;
                State.star.posy = 0.0f;

                RespawnPlayer();

                var StarObject = new GameObject();
                StarObject.name = "Star Renderer";

                SystemStarRenderer starRenderer = StarObject.AddComponent<SystemStarRenderer>();
                starRenderer.Star = State.star;
            }

            void LateUpdate() {
                if(Player != null && State.player == null) {
                    State.player = Player;
                    State.ships.Add(Player.ship);
                    UpdatePlayerWeapons();
                }

                while(PendingEnemies.Count > 0) {
                    State.ships.Add(PendingEnemies[0].ship);
                    Enemies.Add(PendingEnemies[0]);
                    PendingEnemies.RemoveAt(0);
                }

                while(PendingLasers.Count > 0) {
                    State.laser_towers.Add(PendingLasers[0]);
                    PendingLasers[0].state = State;
                    PendingLasers.RemoveAt(0);
                }

                if(SelectedEnemy != null && !Enemies.Contains(SelectedEnemy)) {
                    SelectEnemy(0);
                }

                if(SelectedEnemy != null) {
                    if(SemiMajorAxisSlider.value < SemiMinorAxisSlider.value)
                        SemiMajorAxisSlider.value = SemiMinorAxisSlider.value;
                    SemiMajorAxisSlider.minValue = SemiMinorAxisSlider.value;
                }

                int current_millis = (int)(Time.time * 1000) - LastTime;
                LastTime = (int)(Time.time * 1000);

                for(int i = 0; i < ProjectileRenderers.Count; i++) {
                    KeyValuePair<ShipWeaponProjectile, GameObject> ProjectileRenderer = ProjectileRenderers.ElementAt(i);
                    ShipWeaponProjectile Projectile = ProjectileRenderer.Key;
                    GameObject Renderer = ProjectileRenderer.Value;

                    if(Projectile.UpdatePosition(current_millis / 1000.0f)) {
                        foreach(SystemShip ship in State.ships) {
                            if(ship == Projectile.Self) continue;

                            if(Projectile.InRangeOf(ship, 1.0f)) {
                                Projectile.DoDamage(ship);
                                Projectile.Weapon.projectiles_fired.Remove(Projectile);

                                GameObject.Destroy(ProjectileRenderers[Projectile]);
                                ProjectileRenderers.Remove(Projectile);
                                i--;

                                break;
                            }
                        }
                    } else {
                        Projectile.Weapon.projectiles_fired.Remove(Projectile);

                        GameObject.Destroy(ProjectileRenderers[Projectile]);
                        ProjectileRenderers.Remove(Projectile);
                        i--;
                    }
                }

                for(int i = 0; i < State.ships.Count; i++) {
                    SystemShip ship = State.ships[i];

                    foreach(ShipWeapon Weapon in ship.weapons) {
                        foreach(ShipWeaponProjectile Projectile in Weapon.projectiles_fired) {
                            if(!ProjectileRenderers.ContainsKey(Projectile)) {
                                GameObject ProjectileRenderer = new GameObject();
                                ProjectileRenderer.name = "Projectile renderer";

                                ShipWeaponProjectileRenderer Renderer = ProjectileRenderer.AddComponent<ShipWeaponProjectileRenderer>();
                                Renderer.Projectile = Projectile;

                                ProjectileRenderers.Add(Projectile, ProjectileRenderer);
                            }
                        }
                    }

                    if(ship.destroyed) {
                        State.ships.Remove(ship);
                        if(ship == Player.ship) {
                            GameObject.Destroy(Player);
                            Player = null;
                        } else {
                            for(int j = 0; j < Enemies.Count; j++) {
                                if(Enemies[j].ship == ship) {
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
                    if(ship.shield > ship.max_shield) ship.shield = ship.max_shield;
                }

                foreach(SystemEnemy Enemy in Enemies) {
                    Enemy.ship.rotation = Enemy.ship.descriptor.true_anomaly + Enemy.ship.descriptor.rotation + Tools.halfpi;

                    foreach(ShipWeapon Weapon in Enemy.ship.weapons) {
                        Weapon.TryFiringAt(Player.ship, current_millis);
                    }
                }

                UpdateDropdownMenu();
            }

            public void RespawnPlayer() {
                if(Player != null) {
                    Player.ship.destroy();
                    State.ships.Remove(Player.ship);
                    GameObject.Destroy(Player);
                    State.player = null;
                }

                Player = gameObject.AddComponent<PlayerShip>();
            }

            public void UpdatePlayerWeapons() {
                ShipWeapon left_cannon           = new ShipWeapon();

                left_cannon.color                = new Color(0.7f, 0.9f, 1.0f, 1.0f);

                left_cannon.range                = 45.0f;
                left_cannon.shield_penetration   = 0.2f;
                left_cannon.projectile_velocity  = 50.0f;
                left_cannon.damage               = 6000;
                left_cannon.attack_speed         = 1250;
                left_cannon.cooldown             = 0;
                left_cannon.self                 = Player.ship;
                left_cannon.FOV                  = Tools.quarterpi;

                left_cannon.flags                = (int)WeaponFlags.WEAPON_PROJECTILE
                                                 | (int)WeaponFlags.WEAPON_BROADSIDE
                                                 | (int)WeaponFlags.WEAPON_POSX;


                ShipWeapon left_gun              = new ShipWeapon();

                left_gun.color                   = new Color(0.4f, 0.8f, 1.0f, 1.0f);

                left_gun.range                   = 30.0f;
                left_gun.shield_penetration      = 0.02f;
                left_gun.projectile_velocity     = 25.0f;
                left_gun.damage                  = 2000;
                left_gun.attack_speed            = 400;
                left_gun.cooldown                = 0;
                left_gun.self                    = Player.ship;
                left_gun.FOV                     = Tools.halfpi;

                left_gun.flags                   = (int)WeaponFlags.WEAPON_PROJECTILE
                                                 | (int)WeaponFlags.WEAPON_BROADSIDE
                                                 | (int)WeaponFlags.WEAPON_POSX;

                ShipWeapon right_cannon          = new ShipWeapon();

                right_cannon.color               = new Color(0.7f, 0.9f, 1.0f, 1.0f);

                right_cannon.range               = 45.0f;
                right_cannon.shield_penetration  = 0.2f;
                right_cannon.projectile_velocity = 50.0f;
                right_cannon.damage              = 6000;
                right_cannon.attack_speed        = 1250;
                right_cannon.cooldown            = 0;
                right_cannon.self                = Player.ship;
                right_cannon.FOV                 = Tools.quarterpi;

                right_cannon.flags               = (int)WeaponFlags.WEAPON_PROJECTILE
                                                 | (int)WeaponFlags.WEAPON_BROADSIDE;

                ShipWeapon right_gun             = new ShipWeapon();

                right_gun.color                  = new Color(0.4f, 0.8f, 1.0f, 1.0f);

                right_gun.range                  = 30.0f;
                right_gun.shield_penetration     = 0.02f;
                right_gun.projectile_velocity    = 25.0f;
                right_gun.damage                 = 2000;
                right_gun.attack_speed           = 400;
                right_gun.cooldown               = 0;
                right_gun.self                   = Player.ship;
                right_gun.FOV                    = Tools.halfpi;

                right_gun.flags                  = (int)WeaponFlags.WEAPON_PROJECTILE
                                                 | (int)WeaponFlags.WEAPON_BROADSIDE;

                ShipWeapon turret                = new ShipWeapon();

                turret.color                     = Color.white;

                turret.range                     = 20.0f;
                turret.shield_penetration        = 0.1f;
                turret.projectile_velocity       = 8.0f;
                turret.damage                    = 200;
                turret.attack_speed              = 50;
                turret.cooldown                  = 0;
                turret.FOV                       = Tools.eigthpi;
                turret.rotation                  = Tools.pi;
                turret.rotation_rate             = 2.0f;
                turret.self                      = Player.ship;

                turret.flags                     = (int)WeaponFlags.WEAPON_PROJECTILE
                                                 | (int)WeaponFlags.WEAPON_TURRET;

                ShipWeapon laser                 = new ShipWeapon();

                laser.color                      = new Color(1.0f, 0.0f, 0.0f, 1.0f);

                laser.range                      = 50.0f;
                laser.shield_penetration         = 0.1f;
                laser.projectile_velocity        = 8.0f;
                laser.damage                     = 7500;
                laser.attack_speed               = 1800;
                laser.cooldown                   = 0;
                laser.shield_damage_multiplier   = 2.0f;
                laser.hull_damage_multiplier     = 0.01f;
                laser.self                       = Player.ship;
                laser.state                      = State;
                laser.camera                     = GameObject.Find("Main Camera").GetComponent<CameraController>();

                laser.flags                      = (int)WeaponFlags.WEAPON_LASER;

                Player.ship.weapons.Add(turret);
                Player.ship.weapons.Add(laser);
                Player.ship.weapons.Add(left_cannon);
                Player.ship.weapons.Add(left_gun);
                Player.ship.weapons.Add(right_cannon);
                Player.ship.weapons.Add(right_gun);
            }

            public void AddEnemy() {
                PendingEnemies.Add(gameObject.AddComponent<SystemEnemy>());
            }

            public void AddLaserTower() {
                GameObject O = new GameObject();
                PendingLasers.Add(O.AddComponent<LaserTower>());
                LaserObjects.Add(O);
            }

            public void Reset() {
                while(Enemies.Count > 0) {
                    GameObject.Destroy(Enemies[0]);
                    Enemies.RemoveAt(0);
                }

                State.ships.Clear();

                while(State.laser_towers.Count > 0) {
                    GameObject.Destroy(State.laser_towers[0]);
                    GameObject.Destroy(LaserObjects[0]);
                    State.laser_towers.RemoveAt(0);
                    LaserObjects.RemoveAt(0);
                }

                while(ProjectileRenderers.Count > 0) {
                    KeyValuePair<ShipWeaponProjectile, GameObject> ProjectileRenderer = ProjectileRenderers.ElementAt(0);
                    ShipWeaponProjectile Projectile = ProjectileRenderer.Key;
                    GameObject Renderer = ProjectileRenderer.Value;

                    GameObject.Destroy(Renderer);
                    ProjectileRenderers.Remove(Projectile);
                }

                SelectEnemy(0);
                RespawnPlayer();
            }

            private void UpdateDropdownMenu() {
                EnemySelectorMenu.ClearOptions();

                List<string> Options = new();

                if(Enemies.Count == 0) {
                    Options.Add("-- No enemies --");

                    EnemySelectorMenu.interactable = false;
                } else {
                    Options.Add("-- Select an enemy --");

                    for(int i = 0; i < Enemies.Count;)
                        Options.Add("Enemy " + ++i);

                    EnemySelectorMenu.interactable = true;
                }

                EnemySelectorMenu.AddOptions(Options);
                EnemySelectorMenu.value = 0;

                for(int i = 0; i < Enemies.Count; i++)
                    if(Enemies[i] == SelectedEnemy) {
                        EnemySelectorMenu.value = i + 1;
                        break;
                    }
            }

            public void SelectEnemy(int i) {
                if(i == 0 || i > Enemies.Count) {
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
                } else if(i <= Enemies.Count && SelectedEnemy != Enemies.ElementAt(i - 1)) {
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
                    WeaponCooldownSlider.value            = SelectedEnemy.ship.weapons[0].attack_speed;
                    WeaponRangeSlider.value               = SelectedEnemy.ship.weapons[0].range;
                    WeaponDamageSlider.value              = SelectedEnemy.ship.weapons[0].damage;
                    ShieldPenetrationSlider.value         = SelectedEnemy.ship.weapons[0].shield_penetration;
                    ProjectileVelocitySlider.value        = SelectedEnemy.ship.weapons[0].projectile_velocity;
                }
            }

            public void UpdateEnemy() {
                if(SelectedEnemy != null && Enemies.Contains(SelectedEnemy)) {
                    SelectedEnemy.ship.descriptor.semimajoraxis       =      SemiMajorAxisSlider.value;
                    SelectedEnemy.ship.descriptor.semiminoraxis       =      SemiMinorAxisSlider.value;
                    SelectedEnemy.ship.descriptor.rotation            =      RotationSlider.value;
                    SelectedEnemy.ship.max_health                     = (int)MaxHealthSlider.value;
                    SelectedEnemy.ship.max_shield                     = (int)MaxShieldSlider.value;
                    SelectedEnemy.ship.shield_regeneration_rate       = (int)ShieldRegenerationSlider.value;
                    SelectedEnemy.ship.weapons[0].attack_speed        = (int)WeaponCooldownSlider.value;
                    SelectedEnemy.ship.weapons[0].range               =      WeaponRangeSlider.value;
                    SelectedEnemy.ship.weapons[0].damage              = (int)WeaponDamageSlider.value;
                    SelectedEnemy.ship.weapons[0].shield_penetration  =      ShieldPenetrationSlider.value;
                    SelectedEnemy.ship.weapons[0].projectile_velocity =      ProjectileVelocitySlider.value;

                    if(SelectedEnemy.ship.health > SelectedEnemy.ship.max_health)
                        SelectedEnemy.ship.health = SelectedEnemy.ship.max_health;

                    SelectedEnemy.ship.descriptor.compute();
                }
            }

            public void DeleteEnemy() {
                if(SelectedEnemy != null && Enemies.Contains(SelectedEnemy)) {
                    State.ships.Remove(SelectedEnemy.ship);
                    Enemies.Remove(SelectedEnemy);
                    GameObject.Destroy(SelectedEnemy);
                    SelectEnemy(0);
                }
            }
        }
    }
}
