using System;
using UnityEngine;
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

        public int LastTime;

        public Dictionary<SystemPlanet,       ObjectInfo<SystemPlanetRenderer>>       Planets   = new();
        //public Dictionary<SystemAsteroidBelt, ObjectInfo<SystemAsteroidBeltRenderer>> Asteroids = new();
        public Dictionary<SystemShip,         ObjectInfo<SystemShipRenderer>>         Ships     = new();
        public Dictionary<SpaceStation,       ObjectInfo<SpaceStationRenderer>>       Stations  = new();

        public const int UpdatesPerTick = 64;

        public System.Random rnd = new System.Random();

        public const int InnerPlanets    = 4;
        public const int OuterPlanets    = 6;
        public const int FarOrbitPlanets = 2;

        public int CurrentCycle = 0;

        private void Start()
        {
            LastTime = (int)(Time.time * 1000.0f);
            
            GameLoop gl = GetComponent<GameLoop>();

            State = gl.CurrentSystemState;

            State.Star.Mass = 5000000000.0f;
            State.Star.PosX = (float)rnd.NextDouble() * 8.0f - 4.0f;
            State.Star.PosY = (float)rnd.NextDouble() * 8.0f - 4.0f;

            var StarObject = new GameObject();
            StarObject.name = "Star Renderer";

            SystemStarRenderer starRenderer = StarObject.AddComponent<SystemStarRenderer>();
            starRenderer.Star = State.Star;

            for (int i = 0; i < InnerPlanets; i++)
            {
                SystemPlanet Planet = new SystemPlanet();

                Planet.Descriptor.CentralBody = State.Star;

                Planet.Descriptor.SemiMinorAxis = 30.0f + (i + 1) * (i + 1) * 10;
                Planet.Descriptor.SemiMajorAxis = Planet.Descriptor.SemiMinorAxis + (float)rnd.NextDouble() * (i + 5);

                Planet.Descriptor.Rotation    = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                Planet.Descriptor.MeanAnomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                Planet.Descriptor.Compute();

                Planet.Descriptor.Self.Mass = 100000000;

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

                Planet.Descriptor.SemiMinorAxis = State.Planets[InnerPlanets - 1].Descriptor.SemiMajorAxis + (i + 3) * (i + 3) * 10;
                Planet.Descriptor.SemiMajorAxis = Planet.Descriptor.SemiMinorAxis + (float)rnd.NextDouble() * i / 20.0f;

                Planet.Descriptor.Rotation      = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                Planet.Descriptor.MeanAnomaly   = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                Planet.Descriptor.Compute();

                Planet.Descriptor.Self.Mass = 100000000;

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

                    Moon.Descriptor.Self.Mass = 20000000;

                    Moon.Descriptor.CentralBody = Planet.Descriptor.Self;

                    Moon.Descriptor.SemiMinorAxis = (float)rnd.NextDouble() * (j + 1) + 5.0f;
                    Moon.Descriptor.SemiMajorAxis = Moon.Descriptor.SemiMinorAxis + (float)rnd.NextDouble() * 2.0f;

                    Moon.Descriptor.Rotation      = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                    Moon.Descriptor.MeanAnomaly   = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                    Moon.Descriptor.Compute();

                    State.Planets.Add(Moon);

                    ObjectInfo<SystemPlanetRenderer> MoonInfo = new();

                    MoonInfo.Object = new();
                    MoonInfo.Object.name = "Moon renderer";

                    MoonInfo.Renderer = MoonInfo.Object.AddComponent<SystemPlanetRenderer>();
                    MoonInfo.Renderer.planet = Moon;

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

                            GameObject ShipObject = new GameObject();
                            ShipObject.name = "Ship renderer";

                            SystemShipRenderer ShipRenderer = ShipObject.AddComponent<SystemShipRenderer>();
                            ShipRenderer.ship = Ship;
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
            int CurrentMillis = (int)(Time.time * 1000) - LastTime;
            LastTime = (int)(Time.time * 1000);
            int UpdatesCompleted = 0;

            foreach (SystemPlanet p in State.Planets)
            {
                //if (Planets[p].LastCycle == CurrentCycle) continue;

                p.Descriptor.UpdatePosition(CurrentMillis);
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

                s.Descriptor.UpdatePosition(CurrentMillis);
            }

            float GravVelX = 0.0f;
            float GravVelY = 0.0f;

            float GravPosX = 0.0f;
            float GravPosY = 0.0f;

            foreach (SystemViewBody Body in State.Bodies)
            {
                float dx = Body.PosX - State.Player.Ship.Self.PosX;
                float dy = Body.PosY - State.Player.Ship.Self.PosY;

                float d2 = dx * dx + dy * dy;
                float d = (float)Math.Sqrt(d2);

                float g = 6.67408E-11f * Body.Mass / d2;

                GravVelX += g * CurrentMillis * dx / d;
                GravVelY += g * CurrentMillis * dy / d;

                GravPosX += g * g * CurrentMillis * dx * 0.5f / d;
                GravPosY += g * g * CurrentMillis * dy * 0.5f / d;
            }

            /* todo: fix
            float ShipMagnitude = (float)Math.Sqrt(State.Player.Ship.Self.VelX * State.Player.Ship.Self.VelX + State.Player.Ship.Self.VelY * State.Player.Ship.Self.VelY);
            float GravMagnitude = (float)Math.Sqrt(GravVelX * GravVelX + GravVelY * GravVelY);

            if (GravMagnitude != 0.0f && GravMagnitude + ShipMagnitude != 0.0f && GravMagnitude != float.NaN && ShipMagnitude != float.NaN)
            {
                float GravRotation = 0.0f;

                if (GravVelY > 0.0f) GravRotation = -(float)Math.Acos(GravVelX / GravMagnitude);
                else                 GravRotation =  (float)Math.Acos(GravVelX / GravMagnitude);
            
                State.Player.Ship.Rotation  = GravMagnitude * GravRotation + State.Player.Ship.Rotation * ShipMagnitude;
                State.Player.Ship.Rotation /= GravMagnitude + 1.0f;
            }*/

            State.Player.Ship.Self.VelX += GravVelX;
            State.Player.Ship.Self.VelY += GravVelY;

            State.Player.Ship.Self.PosX += GravPosX;
            State.Player.Ship.Self.PosY += GravPosY;

            CurrentCycle++;
        }
    }
}
