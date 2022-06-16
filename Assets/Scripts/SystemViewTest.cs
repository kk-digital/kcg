using System;
using UnityEngine;
using UnityEngine.UI;
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
        public  int   SpaceStations    =                   15;

        public  float SystemScale      =                25.0f;

        public  float SunMass          = 50000000000000000.0f;
        public  float PlanetMass       =   100000000000000.0f;
        public  float MoonMass         =    20000000000000.0f;
        public  float StationMass      =        1000000000.0f;

        public  float TimeScale        =                 1.0f;
        
        public  float Acceleration     =               250.0f;
        public  float DragFactor       =             10000.0f;
        public  float SailingFactor    =                20.0f;

        private float CachedSunMass    = 50000000000000000.0f;
        private float CachedPlanetMass =   100000000000000.0f;
        private float CachedMoonMass   =    20000000000000.0f;

        private int   CurrentCycle     =                    0;

        public  CameraController Camera;

        public void setInnerPlanets(float f)    { InnerPlanets    =        (int)f; }
        public void setOuterPlanets(float f)    { OuterPlanets    =        (int)f; }
        public void setFarOrbitPlanets(float f) { FarOrbitPlanets =        (int)f; }

        public void setSystemScale(float f)     { SystemScale     =             f; }

        public void setSunMass(float f)         { SunMass         =             f; }
        public void setPlanetMass(float f)      { PlanetMass      =             f; }
        public void setMoonMass(float f)        { MoonMass        =             f; }

        public void setTimeScale(float f)       { TimeScale       =             f; }

        public void setAcceleration(float f)    { Acceleration    =             f; }
        public void setDragFactor(float f)      { DragFactor      = 100000.0f - f; }
        public void setSailingFactor(float f)   { SailingFactor   =   1000.0f - f; }

        public Dropdown DockingTargetSelector;
        private SpaceStation DockingTarget;

        private void Start()
        {
            RegenerateSystem();

            var StarObject = new GameObject();
            StarObject.name = "Star Renderer";

            SystemStarRenderer starRenderer = StarObject.AddComponent<SystemStarRenderer>();
            starRenderer.Star = State.Star;

            Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
        }

        public void CenterCamera()
        {
            Camera.setPosition(-State.Player.ship.self.posx, -State.Player.ship.self.posy, 0.25f / SystemScale);
        }

        public void RegenerateSystem()
        {
            LastTime = Time.time;

            State.Star.mass = SunMass;
            State.Star.posx = ((float)rnd.NextDouble() * 8.0f - 64.0f) * SystemScale;
            State.Star.posy = ((float)rnd.NextDouble() * 8.0f - 4.0f)  * SystemScale;

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

            while (Stations.Count > 0)
            {
                GameObject.Destroy(Stations.ElementAt(0).Value.Renderer);
                GameObject.Destroy(Stations.ElementAt(0).Value.Object);
                Stations.Remove(Stations.ElementAt(0).Key);
            }

            State.Stations.Clear();

            if (State.Player != null)
            {
                GameObject.Destroy(State.Player);
            }

            for (int i = 0; i < InnerPlanets; i++)
            {
                SystemPlanet Planet = new SystemPlanet();

                Planet.Descriptor.central_body = State.Star;

                Planet.Descriptor.semiminoraxis = (30.0f + (i + 1) * (i + 1) * 10) * SystemScale;
                Planet.Descriptor.semimajoraxis = Planet.Descriptor.semiminoraxis + ((float)rnd.NextDouble() * (i + 5) * SystemScale);

                Planet.Descriptor.rotation     = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                Planet.Descriptor.mean_anomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                Planet.Descriptor.compute();

                Planet.Descriptor.self.mass = PlanetMass;

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

            InnerAsteroidBeltDescriptor.CenterX = State.Star.posx;
            InnerAsteroidBeltDescriptor.CenterY = State.Star.posy;

            InnerAsteroidBeltDescriptor.semiminoraxis = State.Planets[InnerPlanets - 1].Descriptor.semimajoraxis + 6.0f;
            InnerAsteroidBeltDescriptor.semimajoraxis = InnerAsteroidBeltDescriptor.semiminoraxis + (float)rnd.NextDouble() / 4.0f;

            InnerAsteroidBeltDescriptor.rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

            SystemAsteroidBelt InnerAsteroidBelt = new(32, InnerAsteroidBeltDescriptor);

            for (int Layer = 0; Layer < 32; Layer++)
            {
                for (int i = 0; i < 32 + 4 * Layer; i++)
                {
                    SystemAsteroid Asteroid = new();

                    Asteroid.mean_anomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
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

                Planet.Descriptor.central_body = State.Star;

                //Planet.Descriptor.semiminoraxis = InnerAsteroidBeltDescriptor.semimajoraxis + (i + 3) * (i + 3);
                //Planet.Descriptor.semimajoraxis = Planet.Descriptor.semiminoraxis + (float)rnd.NextDouble() * i / 2.0f;

                Planet.Descriptor.semiminoraxis = State.Planets[InnerPlanets - 1].Descriptor.semimajoraxis + ((i + 3) * (i + 3) * 10 * SystemScale);
                Planet.Descriptor.semimajoraxis = Planet.Descriptor.semiminoraxis + ((float)rnd.NextDouble() * i / 20.0f) * SystemScale;

                Planet.Descriptor.rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                Planet.Descriptor.mean_anomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                Planet.Descriptor.compute();

                Planet.Descriptor.self.mass = PlanetMass;

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

                    Moon.Descriptor.self.mass = MoonMass;

                    Moon.Descriptor.central_body = Planet.Descriptor.self;

                    Moon.Descriptor.semiminoraxis = ((float)rnd.NextDouble() * (j + 1) + 5.0f) * SystemScale;
                    Moon.Descriptor.semimajoraxis = Moon.Descriptor.semiminoraxis + ((float)rnd.NextDouble() * 2.0f) * SystemScale;

                    Moon.Descriptor.rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                    Moon.Descriptor.mean_anomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                    Moon.Descriptor.compute();

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

            OuterAsteroidBeltDescriptor.CenterX = State.Star.posx;
            OuterAsteroidBeltDescriptor.CenterY = State.Star.posy;

            OuterAsteroidBeltDescriptor.semiminoraxis = State.Planets[InnerPlanets + OuterPlanets - 1].Descriptor.semimajoraxis + 24.0f;
            OuterAsteroidBeltDescriptor.semimajoraxis = OuterAsteroidBeltDescriptor.semiminoraxis + (float)rnd.NextDouble() * 6.0f;

            OuterAsteroidBeltDescriptor.rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

            SystemAsteroidBelt OuterAsteroidBelt = new(128, OuterAsteroidBeltDescriptor);

            for (int Layer = 0; Layer < 128; Layer++)
            {
                for (int i = 0; i < 96 + 8 * Layer; i++)
                {
                    SystemAsteroid Asteroid = new();

                    Asteroid.mean_anomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
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

                Planet.Descriptor.central_body = State.Star;

                //Planet.Descriptor.semiminoraxis = InnerAsteroidBeltDescriptor.semimajoraxis + (i + 3) * (i + 3);
                //Planet.Descriptor.semimajoraxis = Planet.Descriptor.semiminoraxis + (float)rnd.NextDouble() * i / 2.0f;

                Planet.Descriptor.semiminoraxis = State.Planets[InnerPlanets + OuterPlanets - 1].Descriptor.semimajoraxis + ((i + 3) * (i + 3) * 31 * SystemScale);
                Planet.Descriptor.semimajoraxis = Planet.Descriptor.semiminoraxis + (float)rnd.NextDouble() * (i + 1) * 82 * SystemScale;

                Planet.Descriptor.rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                Planet.Descriptor.mean_anomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                Planet.Descriptor.compute();

                Planet.Descriptor.self.mass = PlanetMass;

                ObjectInfo<SystemPlanetRenderer> PlanetInfo = new();

                PlanetInfo.Object = new();
                PlanetInfo.Object.name = "Planet renderer #" + (i + InnerPlanets + OuterPlanets);

                PlanetInfo.Renderer = PlanetInfo.Object.AddComponent<SystemPlanetRenderer>();
                PlanetInfo.Renderer.planet = Planet;

                State.Planets.Add(Planet);
                Planets.Add(Planet, PlanetInfo);
            }

            foreach (SystemPlanet Planet in State.Planets)
            {
                State.Objects.Add(Planet.Descriptor.self);
            }
            State.Objects.Add(State.Star);

            for (int i = 0; i < SpaceStations; i++)
            {
                SpaceStation Station = new();

                Station.Descriptor.central_body   = State.Star;

                Station.Descriptor.semiminoraxis = ((float)rnd.NextDouble() * State.Planets[InnerPlanets + OuterPlanets - 1].Descriptor.semimajoraxis + 4.0f);
                Station.Descriptor.semimajoraxis =  (float)rnd.NextDouble() * SystemScale + Station.Descriptor.semiminoraxis;

                Station.Descriptor.rotation      =  (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                Station.Descriptor.mean_anomaly   =  (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                Station.Descriptor.compute();

                Station.Descriptor.self.mass = StationMass;

                ObjectInfo<SpaceStationRenderer> StationInfo = new();

                StationInfo.Object = new();
                StationInfo.Object.name = "Space station renderer #" + i;

                StationInfo.Renderer = StationInfo.Object.AddComponent<SpaceStationRenderer>();
                StationInfo.Renderer.Station = Station;

                State.Stations.Add(Station);
                Stations.Add(Station, StationInfo);
            }

            for (int i = 0; i < State.Planets.Count; i++)
                if (State.Planets[i].Descriptor.central_body == State.Star)
                    for (int j = 0; j < State.Planets.Count; j++)
                        if (i != j && State.Planets[j].Descriptor.central_body == State.Star)
                        {
                            SystemShip ship = new SystemShip();
                            ship.Start = State.Planets[i].Descriptor;
                            ship.Destination = State.Planets[j].Descriptor;
                            ship.Descriptor = new OrbitingObjectDescriptor(ship.Start, ship.self);

                            State.Ships.Add(ship);

                            ObjectInfo<SystemShipRenderer> ShipInfo = new();

                            ShipInfo.Object = new GameObject();
                            ShipInfo.Object.name = "ship renderer";

                            ShipInfo.Renderer = ShipInfo.Object.AddComponent<SystemShipRenderer>();
                            ShipInfo.Renderer.ship = ship;

                            Ships.Add(ship, ShipInfo);
                        }

            State.Player = gameObject.AddComponent<PlayerShip>();
            State.Player.SystemScale = SystemScale;
        }

        void Update()
        {
            float CurrentTime = (Time.time - LastTime) * TimeScale;
            LastTime = Time.time;
            int UpdatesCompleted = 0;

            if (CachedSunMass != SunMass)
            {
                State.Star.mass = CachedSunMass = SunMass;

                for (int i = 0; i < Planets.Count; i++)
                {
                    Planets.ElementAt(i).Key.Descriptor.compute();
                }

                foreach (SystemShip ship in State.Ships)
                {
                    ship.Descriptor.compute();
                }
            }

            if (CachedPlanetMass != PlanetMass)
            {
                CachedPlanetMass = PlanetMass;

                for (int i = 0; i < Planets.Count; i++)
                {
                    Planets.ElementAt(i).Key.Descriptor.self.mass = PlanetMass;
                }

                for (int i = 0; i < Moons.Count; i++)
                {
                    Moons.ElementAt(i).Key.Descriptor.compute();
                }
            }

            if (CachedMoonMass != MoonMass)
            {
                CachedPlanetMass = MoonMass;

                for (int i = 0; i < Moons.Count; i++)
                {
                    Moons.ElementAt(i).Key.Descriptor.self.mass = MoonMass;
                }
            }

            foreach (SystemPlanet p in State.Planets)
            {
                //if (Planets[p].LastCycle == CurrentCycle) continue;

                p.Descriptor.update_position(CurrentTime);
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

            foreach (SpaceStation s in State.Stations)
            {
                s.Descriptor.update_position(CurrentTime);
            }

            foreach (SystemShip s in State.Ships)
            {
                if (!s.PathPlanned && !s.Reached)
                    s.PathPlanned = s.Descriptor.plan_path(s.Destination, 0.1f * SystemScale);
                else if (!s.Reached && s.Descriptor.get_distance_from(s.Destination) < SystemScale)
                {
                    s.Descriptor.copy(s.Destination);
                    s.PathPlanned = false;
                    (s.Start, s.Destination) = (s.Destination, s.Start);
                }

                s.Descriptor.update_position(CurrentTime);
            }

            float GravVelX = 0.0f;
            float GravVelY = 0.0f;

            // this behaves weird when getting really close to central body --- is float too inaccurate?
            foreach (SpaceObject Body in State.Objects)
            {
                float dx = Body.posx - State.Player.ship.self.posx;
                float dy = Body.posy - State.Player.ship.self.posy;

                float d2 = dx * dx + dy * dy;
                float d = (float)Math.Sqrt(d2);

                float g = 6.67408E-11f * Body.mass / d2;

                float Velocity = g * CurrentTime;

                GravVelX += Velocity * dx / d;
                GravVelY += Velocity * dy / d;
            }

            State.Player.GravitationalStrength = (float)Math.Sqrt(GravVelX * GravVelX + GravVelY * GravVelY) * 0.4f / CurrentTime;

            State.Player.ship.self.velx   += GravVelX;
            State.Player.ship.self.vely   += GravVelY;

            // For some reason this messes stuff up?!

            //State.Player.ship.self.posx   += GravVelX * CurrentTime * 0.5f;
            //State.Player.ship.self.posy   += GravVelY * CurrentTime * 0.5f;

            State.Player.ship.Acceleration = Acceleration;
            State.Player.DragFactor        = DragFactor;
            State.Player.SailingFactor     = SailingFactor;
            State.Player.TimeScale         = TimeScale;

            UpdateDropdownMenu();

            CurrentCycle++;
        }

        private void UpdateDropdownMenu()
        {
            DockingTargetSelector.ClearOptions();

            List<string> Options = new();

            if (Stations.Count == 0)
            {
                Options.Add("-- No stations --");

                DockingTargetSelector.interactable = false;
            }
            else
            {
                Options.Add("-- Select a station --");

                for (int i = 0; i < Stations.Count;)
                    Options.Add("Station " + ++i);

                DockingTargetSelector.interactable = true;
            }

            DockingTargetSelector.AddOptions(Options);

            DockingTargetSelector.value = 0;

            for (int i = 0; i < Stations.Count; i++)
                if (Stations.ElementAt(i).Key == DockingTarget)
                {
                    DockingTargetSelector.value = i + 1;
                    break;
                }
        }

        public void SelectDockingTarget(int i)
        {
            if (i == 0 || i > Stations.Count)
            {
                DockingTarget = null;
                State.Player.ship.DisengageDockingAutopilot();
            }
            else if (DockingTarget != Stations.ElementAt(i - 1).Key)
                State.Player.ship.EngageDockingAutopilot(DockingTarget = Stations.ElementAt(i - 1).Key);
        }
    }
}
