using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SystemView
{
    public struct ObjectInfo<RendererT>
    {
        public GameObject Object;
        public RendererT Renderer;
        public int LastUpdateTime;
        public int LastCycle;
    }

    public class SystemViewTest : MonoBehaviour
    {
        public SystemState State;

        private float LastTime;

        public Dictionary<SystemPlanet,       ObjectInfo<SystemPlanetRenderer>>       Planets   = new();
        public Dictionary<SystemPlanet,       ObjectInfo<SystemPlanetRenderer>>       Moons     = new();
        //public Dictionary<SystemAsteroidBelt, ObjectInfo<SystemAsteroidBeltRenderer>> Asteroids = new();
        public Dictionary<SystemShip,         ObjectInfo<SystemShipRenderer>>         Ships     = new();
        public Dictionary<SpaceStation,       ObjectInfo<SpaceStationRenderer>>       Stations  = new();

        //public const int UpdatesPerTick = 64;

        public System.Random rnd = new System.Random();

        public  int   InnerPlanets     =                    4;
        public  int   OuterPlanets     =                    6;
        public  int   FarOrbitPlanets  =                    2;

        public  float SystemScale      =                25.0f;

        public  float SunMass          = 50000000000000000.0f;
        public  float PlanetMass       =   100000000000000.0f;
        public  float MoonMass         =    20000000000000.0f;

        public  float TimeScale        =                 1.0f;
        
        public  float Acceleration     =               250.0f;
        public  float DragFactor       =             10000.0f;
        public  float SailingFactor    =                20.0f;

        private float CachedSunMass    = 50000000000000000.0f;
        private float CachedPlanetMass =   100000000000000.0f;
        private float CachedMoonMass   =    20000000000000.0f;

        private int   CurrentCycle     =                    0;

        public  CameraController Camera;

        public void setInnerPlanets(float f)    { InnerPlanets    = (int)f; }
        public void setOuterPlanets(float f)    { OuterPlanets    = (int)f; }
        public void setFarOrbitPlanets(float f) { FarOrbitPlanets = (int)f; }

        public void setSystemScale(float f)     { SystemScale     =      f; }

        public void setSunMass(float f)         { SunMass         =      f; }
        public void setPlanetMass(float f)      { PlanetMass      =      f; }
        public void setMoonMass(float f)        { MoonMass        =      f; }

        public void setTimeScale(float f)       { TimeScale       =      f; }

        public void setAcceleration(float f)    { Acceleration    =      f; }
        public void setDragFactor(float f)      { DragFactor      =      f; }
        public void setSailingFactor(float f)   { SailingFactor   =      f; }

        private void Start()
        {
            GameLoop gl = GetComponent<GameLoop>();

            State = gl.CurrentSystemState;

            RegenerateSystem();

            var StarObject = new GameObject();
            StarObject.name = "Star Renderer";

            SystemStarRenderer starRenderer = StarObject.AddComponent<SystemStarRenderer>();
            starRenderer.Star = State.Star;

            Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
        }

        public void CenterCamera()
        {
            Camera.setPosition(-State.Player.Ship.Self.PosX, -State.Player.Ship.Self.PosY, 0.25f / SystemScale);
        }

        public void RegenerateSystem()
        {
            LastTime = Time.time;

            State.Star.Mass = SunMass;
            State.Star.PosX = ((float)rnd.NextDouble() * 8.0f - 64.0f) * SystemScale;
            State.Star.PosY = ((float)rnd.NextDouble() * 8.0f - 4.0f)  * SystemScale;

            // delete previous system

            // while (Stations.Count > 0) { } todo

            while (Ships.Count > 0)
            {
                GameObject.Destroy(Ships.ElementAt(0).Value.Renderer);
                GameObject.Destroy(Ships.ElementAt(0).Value.Object);
                Ships.Remove(Ships.ElementAt(0).Key);
            }

            State.Ships.Clear();

            while (Moons.Count > 0)
            {
                GameObject.Destroy(Moons.ElementAt(0).Value.Renderer);
                GameObject.Destroy(Moons.ElementAt(0).Value.Object);
                Moons.Remove(Moons.ElementAt(0).Key);
            }

            while (Planets.Count > 0)
            {
                GameObject.Destroy(Planets.ElementAt(0).Value.Renderer);
                GameObject.Destroy(Planets.ElementAt(0).Value.Object);
                Planets.Remove(Planets.ElementAt(0).Key);
            }

            State.Planets.Clear();

            if (State.Player != null)
            {
                GameObject.Destroy(State.Player);
            }

            for (int i = 0; i < InnerPlanets; i++)
            {
                SystemPlanet Planet = new SystemPlanet();

                Planet.Descriptor.CentralBody = State.Star;

                Planet.Descriptor.SemiMinorAxis = (30.0f + (i + 1) * (i + 1) * 10) * SystemScale;
                Planet.Descriptor.SemiMajorAxis = Planet.Descriptor.SemiMinorAxis + ((float)rnd.NextDouble() * (i + 5) * SystemScale);

                Planet.Descriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                Planet.Descriptor.MeanAnomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                Planet.Descriptor.Compute();

                Planet.Descriptor.Self.Mass = PlanetMass;

                ObjectInfo<SystemPlanetRenderer> PlanetInfo = new();

                PlanetInfo.Object = new();
                PlanetInfo.Object.name = "Planet renderer #" + (i + 1);

                PlanetInfo.Renderer = PlanetInfo.Object.AddComponent<SystemPlanetRenderer>();
                PlanetInfo.Renderer.planet = Planet;

                State.Planets.Add(Planet);
                Planets.Add(Planet, PlanetInfo);
            }

            /*
            OrbitingObjectDescriptor InnerAsteroidBeltDescriptor = new();

            InnerAsteroidBeltDescriptor.CenterX = State.Star.PosX;
            InnerAsteroidBeltDescriptor.CenterY = State.Star.PosY;

            InnerAsteroidBeltDescriptor.SemiMinorAxis = State.Planets[InnerPlanets - 1].Descriptor.SemiMajorAxis + 6.0f;
            InnerAsteroidBeltDescriptor.SemiMajorAxis = InnerAsteroidBeltDescriptor.SemiMinorAxis + (float)rnd.NextDouble() / 4.0f;

            InnerAsteroidBeltDescriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

            SystemAsteroidBelt InnerAsteroidBelt = new(32, InnerAsteroidBeltDescriptor);

            for (int Layer = 0; Layer < 32; Layer++)
            {
                for (int i = 0; i < 32 + 4 * Layer; i++)
                {
                    SystemAsteroid Asteroid = new();

                    Asteroid.MeanAnomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                    Asteroid.Layer = Layer;

                    InnerAsteroidBelt.Asteroids.Add(Asteroid);
                }
            }

            ObjectInfo<SystemAsteroidBeltRenderer> InnerAsteroidBeltInfo = new();

            InnerAsteroidBeltInfo.Object = new();
            InnerAsteroidBeltInfo.Object.name = "Inner asteroid belt renderer";

            InnerAsteroidBeltInfo.Renderer = InnerAsteroidBeltInfo.Object.AddComponent<SystemAsteroidBeltRenderer>();
            InnerAsteroidBeltInfo.Renderer.belt = InnerAsteroidBelt;

            Asteroids.Add(InnerAsteroidBelt, InnerAsteroidBeltInfo);
            */

            for (int i = 0; i < OuterPlanets; i++)
            {
                SystemPlanet Planet = new SystemPlanet();

                Planet.Descriptor.CentralBody = State.Star;

                //Planet.Descriptor.SemiMinorAxis = InnerAsteroidBeltDescriptor.SemiMajorAxis + (i + 3) * (i + 3);
                //Planet.Descriptor.SemiMajorAxis = Planet.Descriptor.SemiMinorAxis + (float)rnd.NextDouble() * i / 2.0f;

                Planet.Descriptor.SemiMinorAxis = State.Planets[InnerPlanets - 1].Descriptor.SemiMajorAxis + ((i + 3) * (i + 3) * 10 * SystemScale);
                Planet.Descriptor.SemiMajorAxis = Planet.Descriptor.SemiMinorAxis + ((float)rnd.NextDouble() * i / 20.0f) * SystemScale;

                Planet.Descriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                Planet.Descriptor.MeanAnomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                Planet.Descriptor.Compute();

                Planet.Descriptor.Self.Mass = PlanetMass;

                ObjectInfo<SystemPlanetRenderer> PlanetInfo = new();

                PlanetInfo.Object = new();
                PlanetInfo.Object.name = "Planet renderer #" + (i + InnerPlanets);

                PlanetInfo.Renderer = PlanetInfo.Object.AddComponent<SystemPlanetRenderer>();
                PlanetInfo.Renderer.planet = Planet;

                State.Planets.Add(Planet);
                Planets.Add(Planet, PlanetInfo);

                for (int j = 0; j < rnd.Next(i + 1); j++)
                {
                    SystemPlanet Moon = new SystemPlanet();

                    Moon.Descriptor.Self.Mass = MoonMass;

                    Moon.Descriptor.CentralBody = Planet.Descriptor.Self;

                    Moon.Descriptor.SemiMinorAxis = ((float)rnd.NextDouble() * (j + 1) + 5.0f) * SystemScale;
                    Moon.Descriptor.SemiMajorAxis = Moon.Descriptor.SemiMinorAxis + ((float)rnd.NextDouble() * 2.0f) * SystemScale;

                    Moon.Descriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                    Moon.Descriptor.MeanAnomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                    Moon.Descriptor.Compute();

                    State.Planets.Add(Moon);

                    ObjectInfo<SystemPlanetRenderer> MoonInfo = new();

                    MoonInfo.Object = new();
                    MoonInfo.Object.name = "Moon renderer";

                    MoonInfo.Renderer = MoonInfo.Object.AddComponent<SystemPlanetRenderer>();
                    MoonInfo.Renderer.planet = Moon;

                    Moons.Add(Moon, MoonInfo);
                }
            }

            /*OrbitingObjectDescriptor OuterAsteroidBeltDescriptor = new();

            OuterAsteroidBeltDescriptor.CenterX = State.Star.PosX;
            OuterAsteroidBeltDescriptor.CenterY = State.Star.PosY;

            OuterAsteroidBeltDescriptor.SemiMinorAxis = State.Planets[InnerPlanets + OuterPlanets - 1].Descriptor.SemiMajorAxis + 24.0f;
            OuterAsteroidBeltDescriptor.SemiMajorAxis = OuterAsteroidBeltDescriptor.SemiMinorAxis + (float)rnd.NextDouble() * 6.0f;

            OuterAsteroidBeltDescriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

            SystemAsteroidBelt OuterAsteroidBelt = new(128, OuterAsteroidBeltDescriptor);

            for (int Layer = 0; Layer < 128; Layer++)
            {
                for (int i = 0; i < 96 + 8 * Layer; i++)
                {
                    SystemAsteroid Asteroid = new();

                    Asteroid.MeanAnomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                    Asteroid.Layer = Layer;

                    OuterAsteroidBelt.Asteroids.Add(Asteroid);
                }
            }

            ObjectInfo<SystemAsteroidBeltRenderer> OuterAsteroidBeltInfo = new();

            OuterAsteroidBeltInfo.Object = new();
            OuterAsteroidBeltInfo.Object.name = "Outer asteroid belt renderer";

            OuterAsteroidBeltInfo.Renderer = OuterAsteroidBeltInfo.Object.AddComponent<SystemAsteroidBeltRenderer>();
            OuterAsteroidBeltInfo.Renderer.belt = OuterAsteroidBelt;

            Asteroids.Add(OuterAsteroidBelt, OuterAsteroidBeltInfo);

            State.AsteroidBelts.Add(InnerAsteroidBelt);
            State.AsteroidBelts.Add(OuterAsteroidBelt);*/

            for (int i = 0; i < FarOrbitPlanets; i++)
            {
                SystemPlanet Planet = new SystemPlanet();

                Planet.Descriptor.CentralBody = State.Star;

                //Planet.Descriptor.SemiMinorAxis = InnerAsteroidBeltDescriptor.SemiMajorAxis + (i + 3) * (i + 3);
                //Planet.Descriptor.SemiMajorAxis = Planet.Descriptor.SemiMinorAxis + (float)rnd.NextDouble() * i / 2.0f;

                Planet.Descriptor.SemiMinorAxis = State.Planets[InnerPlanets + OuterPlanets - 1].Descriptor.SemiMajorAxis + ((i + 3) * (i + 3) * 31 * SystemScale);
                Planet.Descriptor.SemiMajorAxis = Planet.Descriptor.SemiMinorAxis + (float)rnd.NextDouble() * (i + 1) * 82 * SystemScale;

                Planet.Descriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                Planet.Descriptor.MeanAnomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                Planet.Descriptor.Compute();

                Planet.Descriptor.Self.Mass = PlanetMass;

                ObjectInfo<SystemPlanetRenderer> PlanetInfo = new();

                PlanetInfo.Object = new();
                PlanetInfo.Object.name = "Planet renderer #" + (i + InnerPlanets + OuterPlanets);

                PlanetInfo.Renderer = PlanetInfo.Object.AddComponent<SystemPlanetRenderer>();
                PlanetInfo.Renderer.planet = Planet;

                State.Planets.Add(Planet);
                Planets.Add(Planet, PlanetInfo);
            }

            for (int i = 0; i < State.Planets.Count; i++)
                if (State.Planets[i].Descriptor.CentralBody == State.Star)
                    for (int j = 0; j < State.Planets.Count; j++)
                        if (i != j && State.Planets[j].Descriptor.CentralBody == State.Star)
                        {
                            SystemShip Ship = new SystemShip();
                            Ship.Start = State.Planets[i].Descriptor;
                            Ship.Destination = State.Planets[j].Descriptor;
                            Ship.Descriptor = new OrbitingObjectDescriptor(Ship.Start, Ship.Self);

                            State.Ships.Add(Ship);

                            ObjectInfo<SystemShipRenderer> ShipInfo = new();

                            ShipInfo.Object = new GameObject();
                            ShipInfo.Object.name = "Ship renderer";

                            ShipInfo.Renderer = ShipInfo.Object.AddComponent<SystemShipRenderer>();
                            ShipInfo.Renderer.ship = Ship;

                            Ships.Add(Ship, ShipInfo);
                        }

            foreach (SystemPlanet Planet in State.Planets)
            {
                State.Bodies.Add(Planet.Descriptor.Self);
            }
            State.Bodies.Add(State.Star);

            State.Player = gameObject.AddComponent<PlayerShip>();
        }

        void Update()
        {
            float CurrentTime = (Time.time - LastTime) * TimeScale;
            LastTime = Time.time;
            int UpdatesCompleted = 0;

            if (CachedSunMass != SunMass)
            {
                State.Star.Mass = CachedSunMass = SunMass;

                for (int i = 0; i < Planets.Count; i++)
                {
                    Planets.ElementAt(i).Key.Descriptor.Compute();
                }

                foreach (SystemShip Ship in State.Ships)
                {
                    Ship.Descriptor.Compute();
                }
            }

            if (CachedPlanetMass != PlanetMass)
            {
                CachedPlanetMass = PlanetMass;

                for (int i = 0; i < Planets.Count; i++)
                {
                    Planets.ElementAt(i).Key.Descriptor.Self.Mass = PlanetMass;
                }

                for (int i = 0; i < Moons.Count; i++)
                {
                    Moons.ElementAt(i).Key.Descriptor.Compute();
                }
            }

            if (CachedMoonMass != MoonMass)
            {
                CachedPlanetMass = MoonMass;

                for (int i = 0; i < Moons.Count; i++)
                {
                    Moons.ElementAt(i).Key.Descriptor.Self.Mass = MoonMass;
                }
            }

            foreach (SystemPlanet p in State.Planets)
            {
                //if (Planets[p].LastCycle == CurrentCycle) continue;

                p.Descriptor.UpdatePosition(CurrentTime);
                //Planets[p].LastCycle = CurrentCycle;

                //if (++UpdatesCompleted == UpdatesPerTick) return;
            }

            /*foreach (SystemAsteroidBelt b in State.AsteroidBelts)
            {
                //if (Asteroids[b].LastCycle == CurrentCycle) continue;

                b.UpdatePositions(CurrentMillis / 200.0f);
                //Asteroids[b].LastCycle = CurrentCycle;

                //if (++UpdatesCompleted == UpdatesPerTick) return;
            }*/

            foreach (SystemShip s in State.Ships)
            {
                if (!s.PathPlanned && !s.Reached)
                    s.PathPlanned = s.Descriptor.PlanPath(s.Destination, 0.1f);
                else if (!s.Reached && s.Descriptor.GetDistanceFrom(s.Destination) < 5.0f)
                {
                    s.Descriptor.Copy(s.Destination);
                    s.PathPlanned = false;
                    (s.Start, s.Destination) = (s.Destination, s.Start);
                }

                s.Descriptor.UpdatePosition(CurrentTime);
            }

            float GravVelX = 0.0f;
            float GravVelY = 0.0f;

            // this behaves weird when getting really close to central body --- float too inaccurate?
            foreach (SystemViewBody Body in State.Bodies)
            {
                float dx = Body.PosX - State.Player.Ship.Self.PosX;
                float dy = Body.PosY - State.Player.Ship.Self.PosY;

                float d2 = dx * dx + dy * dy;
                float d = (float)Math.Sqrt(d2);

                float g = 6.67408E-11f * Body.Mass / d2;

                float Velocity = g * CurrentTime;

                GravVelX += Velocity * dx / d;
                GravVelY += Velocity * dy / d;
            }

            State.Player.GravitationalStrength = (float)Math.Sqrt(GravVelX * GravVelX + GravVelY * GravVelY) * 5.0f / CurrentTime;

            State.Player.Ship.Self.VelX   += GravVelX;
            State.Player.Ship.Self.VelY   += GravVelY;

            // For some reason this messes stuff up?!

            //State.Player.Ship.Self.PosX   += GravVelX * CurrentTime * 0.5f;
            //State.Player.Ship.Self.PosY   += GravVelY * CurrentTime * 0.5f;

            State.Player.Ship.Acceleration = Acceleration;
            State.Player.DragFactor        = DragFactor;
            State.Player.SailingFactor     = SailingFactor;
            State.Player.TimeScale         = TimeScale;

            CurrentCycle++;
        }
    }
}
