using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SystemView
{
    public struct ShipInfo
    {
        public GameObject Object;
        public SystemShipRenderer Renderer;
    }

    public class SystemViewTest : MonoBehaviour
    {
        public SystemState State;

        public int LastTime;

        public Dictionary<SystemShip, ShipInfo> Fighters = new Dictionary<SystemShip, ShipInfo>();
        public Dictionary<ShipWeaponProjectile, GameObject> ProjectileRenderers = new Dictionary<ShipWeaponProjectile, GameObject>();

        public System.Random rnd = new System.Random();

        private void Start()
        {
            GameLoop gl = GetComponent<GameLoop>();

            State = gl.CurrentSystemState;

            SystemPlanetRenderer[] testRenderers = new SystemPlanetRenderer[6];

            State.Star = new SystemStar();
            State.Star.PosX = (float)rnd.NextDouble() * 8.0f - 4.0f;
            State.Star.PosY = (float)rnd.NextDouble() * 8.0f - 4.0f;

            var StarObject = new GameObject();
            StarObject.name = "Star Renderer";

            SystemStarRenderer starRenderer = StarObject.AddComponent<SystemStarRenderer>();
            starRenderer.Star = State.Star;

            for (int i = 1; i <= 3; i++)
            {
                SystemPlanet testPlanet = new SystemPlanet();

                testPlanet.Descriptor.CenterX = State.Star.PosX;
                testPlanet.Descriptor.CenterY = State.Star.PosY;

                testPlanet.Descriptor.SemiMinorAxis = 1.0f + 2.0f * (float)rnd.NextDouble() * i;
                testPlanet.Descriptor.SemiMajorAxis = testPlanet.Descriptor.SemiMinorAxis + (float)rnd.NextDouble() / 8.0f;

                testPlanet.Descriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                testPlanet.Descriptor.RotationalPosition = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                var child = new GameObject();
                child.name = "Planet Renderer " + i;

                testRenderers[i - 1] = child.AddComponent<SystemPlanetRenderer>();
                testRenderers[i - 1].planet = testPlanet;

                State.Planets.Add(testPlanet);
            }

            OrbitingObjectDescriptor testBeltDescriptor = new OrbitingObjectDescriptor();

            testBeltDescriptor.CenterX = State.Star.PosX;
            testBeltDescriptor.CenterY = State.Star.PosY;

            testBeltDescriptor.SemiMinorAxis = State.Planets[2].Descriptor.SemiMajorAxis + 4.0f + (float)rnd.NextDouble();
            testBeltDescriptor.SemiMajorAxis = testBeltDescriptor.SemiMinorAxis + (float)rnd.NextDouble() / 4.0f;

            SystemAsteroidBelt testBelt = new SystemAsteroidBelt(16, testBeltDescriptor);

            for (int Layer = 0; Layer < 16; Layer++)
            {
                for (int i = 0; i < 192 + 8 * Layer; i++)
                {
                    SystemAsteroid testAsteroid = new SystemAsteroid();

                    testAsteroid.RotationalPosition = (float)i * 3.1415926f / (96.0f + 4.0f * Layer);
                    testAsteroid.Layer = Layer;

                    testBelt.Asteroids.Add(testAsteroid);
                }
            }

            State.AsteroidBelts.Add(testBelt);

            var AsteroidBeltObject = new GameObject();
            AsteroidBeltObject.name = "Asteroid Belt Renderer";

            SystemAsteroidBeltRenderer asteroidBeltRenderer = AsteroidBeltObject.AddComponent<SystemAsteroidBeltRenderer>();
            asteroidBeltRenderer.belt = testBelt;

            for (int i = 0; i < 3; i++)
            {
                SystemPlanet testPlanet = new SystemPlanet();

                testPlanet.Descriptor.CenterX = State.Star.PosX;
                testPlanet.Descriptor.CenterY = State.Star.PosY;

                testPlanet.Descriptor.SemiMinorAxis = testBeltDescriptor.SemiMajorAxis + testBelt.BeltWidth + 4.0f * (float)rnd.NextDouble() * (i + 1);
                testPlanet.Descriptor.SemiMajorAxis = testPlanet.Descriptor.SemiMinorAxis + (float)rnd.NextDouble() * (i + 1) / 4.0f;

                testPlanet.Descriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                var child = new GameObject();
                child.name = "Planet Renderer " + (i + 4);

                testRenderers[i + 3] = child.AddComponent<SystemPlanetRenderer>();
                testRenderers[i + 3].planet = testPlanet;

                State.Planets.Add(testPlanet);
            }

            int shipnr = 1;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (j == i) continue;

                    SystemShip testShip = new SystemShip();

                    State.Ships.Add(testShip);

                    var shipRendererObject = new GameObject();
                    shipRendererObject.name = "Ship Renderer " + shipnr++;

                    testShip.Start = State.Planets[i].Descriptor;
                    testShip.Destination = State.Planets[j].Descriptor;

                    SystemShipRenderer shipRenderer = shipRendererObject.AddComponent<SystemShipRenderer>();
                    shipRenderer.ship = testShip;
                }
            }

            for (int i = 0; i < 32; i++)
            {
                SystemShip Fighter = new SystemShip();
                ShipInfo Info = new ShipInfo();

                State.Ships.Add(Fighter);

                Fighter.Health = Fighter.MaxHealth = Fighter.Shield = Fighter.MaxShield = 10000;
                Fighter.ShieldRegenerationRate = 1;

                Fighter.Descriptor.CenterX = State.Star.PosX;
                Fighter.Descriptor.CenterY = State.Star.PosY;

                Fighter.Descriptor.SemiMajorAxis = 20.0f;
                Fighter.Descriptor.SemiMinorAxis = 6.0f;

                Fighter.Descriptor.RotationalPosition = 3.1415926f / 16.0f * i;

                ShipWeapon Weapon = new ShipWeapon();

                Weapon.ProjectileColor = new Color((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), 1.0f);
                Weapon.Range = 50.0f;
                Weapon.ShieldPenetration = (float)rnd.NextDouble() * 0.3f;
                Weapon.Damage = rnd.Next(200, 800);
                Weapon.AttackSpeed = rnd.Next(200, 800);
                Weapon.Cooldown = 0;
                Weapon.Self = Fighter;
                Weapon.ProjectileVelocity = (float)rnd.NextDouble() * 1.5f + 1.0f;

                Fighter.Weapons.Add(Weapon);

                Info.Object = new GameObject();
                Info.Object.name = "Fighter " + i;

                Info.Renderer = Info.Object.AddComponent<SystemShipRenderer>();
                Info.Renderer.ship = Fighter;
                Info.Renderer.shipColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);

                Fighter.PathPlanned = true;
                Fighter.Reached = true;

                Fighters.Add(Fighter, Info);
            }

            for (int i = 0; i < 16; i++)
            {
                SystemShip Fighter = new SystemShip();
                ShipInfo Info = new ShipInfo();

                State.Ships.Add(Fighter);

                Fighter.Health = Fighter.MaxHealth = Fighter.Shield = Fighter.MaxShield = 10000;
                Fighter.ShieldRegenerationRate = 1;

                Fighter.Descriptor.CenterX = State.Star.PosX;
                Fighter.Descriptor.CenterY = State.Star.PosY;

                Fighter.Descriptor.SemiMajorAxis = 16.0f;
                Fighter.Descriptor.SemiMinorAxis = 10.0f;

                Fighter.Descriptor.Rotation = 1.0f;

                Fighter.Descriptor.RotationalPosition = 3.1415926f / 8.0f * i;

                ShipWeapon Weapon = new ShipWeapon();

                Weapon.ProjectileColor = new Color((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), 1.0f);
                Weapon.Range = 50.0f;
                Weapon.ShieldPenetration = (float)rnd.NextDouble() * 0.3f;
                Weapon.Damage = rnd.Next(200, 800);
                Weapon.AttackSpeed = rnd.Next(200, 800);
                Weapon.Cooldown = 0;
                Weapon.Self = Fighter;
                Weapon.ProjectileVelocity = (float)rnd.NextDouble() * 1.5f + 1.0f;

                Fighter.Weapons.Add(Weapon);

                Info.Object = new GameObject();
                Info.Object.name = "Fighter " + i;

                Info.Renderer = Info.Object.AddComponent<SystemShipRenderer>();
                Info.Renderer.ship = Fighter;
                Info.Renderer.shipColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);

                Fighter.PathPlanned = true;
                Fighter.Reached = true;

                Fighters.Add(Fighter, Info);
            }

            LastTime = (int)(Time.time * 1000);
        }

        void Update()
        {
            int CurrentMillis = (int)(Time.time * 1000) - LastTime;
            LastTime = (int)(Time.time * 1000);

            foreach (SystemPlanet p in State.Planets)
            {
                p.UpdatePosition(CurrentMillis / 200.0f);
            }
            
            foreach (SystemAsteroidBelt b in State.AsteroidBelts)
            {
                b.UpdatePositions(CurrentMillis / 200.0f);
            }

            foreach (SystemShip b in State.Ships)
            {
                if (!b.PathPlanned && !b.Reached)
                    b.PlanPath(b.Start, b.Destination, 0.1f);
                else if (!b.Reached && b.Descriptor.GetDistanceFrom(b.Destination) < 1.0f)
                {
                    b.Descriptor = new OrbitingObjectDescriptor(b.Destination);
                    b.PathPlanned = false;
                    (b.Start, b.Destination) = (b.Destination, b.Start);
                }

                b.UpdatePosition(CurrentMillis / 200.0f);
            }

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

            for (int i = 30; i < State.Ships.Count; i++)
            {
                SystemShip Ship = State.Ships[i];

                if (Ship.Destroyed)
                {
                    GameObject.Destroy(Fighters[Ship].Renderer.ShieldObject);
                    GameObject.Destroy(Fighters[Ship].Object);

                    Fighters.Remove(Ship);
                    State.Ships.Remove(Ship);

                    i--;
                    continue;
                }

                if (Fighters.Count > 1) foreach (ShipWeapon Weapon in Ship.Weapons)
                {
                    SystemShip Target = null;

                    do
                    {
                        Target = State.Ships[rnd.Next(Fighters.Count) + 30];
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

                Fighters[Ship].Renderer.shipColor.g = (float)Ship.Health / Ship.MaxHealth;
                Fighters[Ship].Renderer.shipColor.r = 1.0f - Fighters[Ship].Renderer.shipColor.g;
            }
        }
    }
}
