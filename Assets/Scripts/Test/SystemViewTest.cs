using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public struct ObjectInfo<RendererT> {
            public GameObject Object;
            public RendererT Renderer;
        }

        public class SystemViewTest : MonoBehaviour {
            public SystemState State;

            private float LastTime;

            public Dictionary<SystemStar,         ObjectInfo<SystemStarRenderer>>         Stars     = new();
            public Dictionary<SystemPlanet,       ObjectInfo<SystemPlanetRenderer>>       Planets   = new();
            public Dictionary<SystemPlanet,       ObjectInfo<SystemPlanetRenderer>>       Moons     = new();
            //public Dictionary<SystemAsteroidBelt, ObjectInfo<SystemAsteroidBeltRenderer>> Asteroids = new();
            public Dictionary<SystemShip,         ObjectInfo<SystemShipRenderer>>         Ships     = new();
            public Dictionary<SpaceStation,       ObjectInfo<SpaceStationRenderer>>       Stations  = new();

            public System.Random rnd = new System.Random();

            public  int   StarCount        =                    1;
            public  int   InnerPlanets     =                    4;
            public  int   OuterPlanets     =                    6;
            public  int   FarOrbitPlanets  =                    2;
            public  int   SpaceStations    =                    0;

            public  float system_scale     =                25.0f;

            public  float SunMass          = 50000000000000000.0f;
            public  float PlanetMass       =   100000000000000.0f;
            public  float MoonMass         =    20000000000000.0f;
            public  float StationMass      =        1000000000.0f;

            public  float time_scale       =                 1.0f;

            public  float acceleration     =               250.0f;
            public  float drag_factor      =             10000.0f;
            public  float sailing_factor   =                20.0f;

            private float CachedSunMass    = 50000000000000000.0f;
            private float CachedPlanetMass =   100000000000000.0f;
            private float CachedMoonMass   =    20000000000000.0f;

            public  bool  TrackingPlayer   =                false;
            public  bool  planet_movement  =                 true;
            public  bool  n_body_gravity   =                 true;

            public  CameraController Camera;

            public void setInnerPlanets(float f)    { InnerPlanets    =        (int)f; }
            public void setOuterPlanets(float f)    { OuterPlanets    =        (int)f; }
            public void setFarOrbitPlanets(float f) { FarOrbitPlanets =        (int)f; }

            public void setSystemScale(float f)     { system_scale    =             f; }

            public void setSunMass(float f)         { SunMass         =             f; }
            public void setPlanetMass(float f)      { PlanetMass      =             f; }
            public void setMoonMass(float f)        { MoonMass        =             f; }

            public void setTimeScale(float f)       { time_scale      =             f; }

            public void setAcceleration(float f)    { acceleration    =             f; }
            public void setDragFactor(float f)      { drag_factor     = 100000.0f - f; }
            public void setSailingFactor(float f)   { sailing_factor  =   1000.0f - f; }

            public void toggle_planet_movement(bool b) {
                planet_movement = b;
            }

            public void toggle_n_body_gravity(bool b) {
                n_body_gravity = b;
                gravity_renderer.n_body_gravity = b;
            }
                
            public void engage_autopilot() {
                State.player.ship.engage_orbital_autopilot();
            }

            public void circularize() {
                State.player.circularizing = true;
            }

            public void set_periapsis(string s) {
                State.player.periapsis = float.Parse(s);
            }

            public void set_apoapsis(string s) {
                State.player.apoapsis = float.Parse(s);
            }

            public void set_rotation(string s) { 
                State.player.rotation = Tools.normalize_angle(float.Parse(s) * Tools.pi / 180.0f);
            }

            public  Dropdown DockingTargetSelector;
            private SpaceStation DockingTarget;
            public  GravityRenderer gravity_renderer;

            private void Start() {
                RegenerateSystem();

                var StarObject = new GameObject();
                StarObject.name = "Star Renderer";

                Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
            }

            public void CenterCamera() {
                Camera.set_position(-State.player.ship.self.posx, -State.player.ship.self.posy, 0.25f / system_scale);
            }

            public void TogglePlayerTracking() {
                if(TrackingPlayer = !TrackingPlayer) CenterCamera();                
            }

            public void RegenerateSystem() {
                LastTime = Time.time;

                // delete previous system

                while(Stars.Count > 0) {
                    GameObject.Destroy(Stars.ElementAt(0).Value.Renderer);
                    GameObject.Destroy(Stars.ElementAt(0).Value.Object);
                    Stars.Remove(Stars.ElementAt(0).Key);
                }

                State.stars.Clear();

                while(Ships.Count > 0) {
                    GameObject.Destroy(Ships.ElementAt(0).Value.Renderer);
                    GameObject.Destroy(Ships.ElementAt(0).Value.Object);
                    Ships.Remove(Ships.ElementAt(0).Key);
                }

                State.ships.Clear();

                while(Moons.Count > 0) {
                    GameObject.Destroy(Moons.ElementAt(0).Value.Renderer);
                    GameObject.Destroy(Moons.ElementAt(0).Value.Object);
                    Moons.Remove(Moons.ElementAt(0).Key);
                }

                while(Planets.Count > 0) {
                    GameObject.Destroy(Planets.ElementAt(0).Value.Renderer);
                    GameObject.Destroy(Planets.ElementAt(0).Value.Object);
                    Planets.Remove(Planets.ElementAt(0).Key);
                }

                State.planets.Clear();

                while(Stations.Count > 0) {
                    GameObject.Destroy(Stations.ElementAt(0).Value.Renderer);
                    GameObject.Destroy(Stations.ElementAt(0).Value.Object);
                    Stations.Remove(Stations.ElementAt(0).Key);
                }

                State.stations.Clear();

                if(State.player != null) {
                    GameObject.Destroy(State.player);
                }

                for(int i = 0; i < StarCount; i++) {

                    State.stars.Add(new SystemStar());

                    State.stars[i].self.mass                    = SunMass * (float)rnd.NextDouble() * (i + 1);
                    State.stars[i].self.posx                    = ((float)rnd.NextDouble() * 16.0f - 64.0f) * system_scale;
                    State.stars[i].self.posy                    = ((float)rnd.NextDouble() * 16.0f -  8.0f) * system_scale;
                    State.stars[i].render_orbit                 = StarCount > 1;

                    State.objects.Add(State.stars[i].self);

                    ObjectInfo<SystemStarRenderer> StarInfo     = new();
                    StarInfo.Object                             = new GameObject();
                    StarInfo.Object.name                        = "Star renderer " + (i + 1);
                    StarInfo.Renderer                           = StarInfo.Object.AddComponent<SystemStarRenderer>();
                    StarInfo.Renderer.star                      = State.stars[i];

                    Stars.Add(State.stars[i], StarInfo);

                }

                if(StarCount > 1)
                    for(int i = 0; i < StarCount; i++) {
                        State.stars[i].descriptor.semiminoraxis = (float)rnd.NextDouble() * 32.0f * system_scale;
                        State.stars[i].descriptor.semimajoraxis = (float)rnd.NextDouble() * 32.0f * system_scale + State.stars[i].descriptor.semiminoraxis;
                        State.stars[i].descriptor.rotation      = (float)rnd.NextDouble() * Tools.twopi;
                        State.stars[i].descriptor.rotation      = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                        State.stars[i].descriptor.mean_anomaly  = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                        int j;
                        do j = rnd.Next(StarCount);
                        while(j == i);

                        State.stars[i].descriptor.central_body  = State.stars[j].self;
                        State.stars[i].descriptor.compute();
                    }

                for(int i = 0; i < InnerPlanets; i++) {
                    SystemPlanet Planet = new SystemPlanet();

                    Planet.descriptor.central_body = State.stars[0].self;

                    Planet.descriptor.semiminoraxis = (30.0f + (i + 1) * (i + 1) * 10) * system_scale;
                    Planet.descriptor.semimajoraxis = Planet.descriptor.semiminoraxis + ((float)rnd.NextDouble() * (i + 5) * system_scale);

                    Planet.descriptor.rotation     = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                    Planet.descriptor.mean_anomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                    Planet.descriptor.compute();

                    Planet.descriptor.self.mass = PlanetMass;

                    ObjectInfo<SystemPlanetRenderer> PlanetInfo = new();

                    PlanetInfo.Object = new();
                    PlanetInfo.Object.name = "Planet renderer #" + (i + 1);

                    PlanetInfo.Renderer = PlanetInfo.Object.AddComponent<SystemPlanetRenderer>();
                    PlanetInfo.Renderer.planet = Planet;

                    State.planets.Add(Planet);
                    Planets.Add(Planet, PlanetInfo);
                }

                /*
                OrbitingObjectDescriptor InnerAsteroidBeltDescriptor = new();

                InnerAsteroidBeltDescriptor.CenterX = State.Star.posx;
                InnerAsteroidBeltDescriptor.CenterY = State.Star.posy;

                InnerAsteroidBeltDescriptor.semiminoraxis = State.Planets[InnerPlanets - 1].descriptor.semimajoraxis + 6.0f;
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

                for(int i = 0; i < OuterPlanets; i++) {
                    SystemPlanet Planet = new SystemPlanet();

                    Planet.descriptor.central_body = State.stars[0].self;

                    //Planet.descriptor.semiminoraxis = InnerAsteroidBeltDescriptor.semimajoraxis + (i + 3) * (i + 3);
                    //Planet.descriptor.semimajoraxis = Planet.descriptor.semiminoraxis + (float)rnd.NextDouble() * i / 2.0f;

                    Planet.descriptor.semiminoraxis = State.planets[InnerPlanets - 1].descriptor.semimajoraxis + ((i + 3) * (i + 3) * 10 * system_scale);
                    Planet.descriptor.semimajoraxis = Planet.descriptor.semiminoraxis + ((float)rnd.NextDouble() * i / 20.0f) * system_scale;

                    Planet.descriptor.rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                    Planet.descriptor.mean_anomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                    Planet.descriptor.compute();

                    Planet.descriptor.self.mass = PlanetMass;

                    ObjectInfo<SystemPlanetRenderer> PlanetInfo = new();

                    PlanetInfo.Object = new();
                    PlanetInfo.Object.name = "Planet renderer #" + (i + InnerPlanets);

                    PlanetInfo.Renderer = PlanetInfo.Object.AddComponent<SystemPlanetRenderer>();
                    PlanetInfo.Renderer.planet = Planet;

                    State.planets.Add(Planet);
                    Planets.Add(Planet, PlanetInfo);

                    for(int j = 0; j < rnd.Next(i + 1); j++) {
                        SystemPlanet Moon = new SystemPlanet();

                        Moon.descriptor.self.mass     = MoonMass;

                        Moon.descriptor.central_body  = Planet.descriptor.self;

                        Moon.descriptor.semiminoraxis = ((float)rnd.NextDouble() * (j + 1) + 5.0f) * system_scale;
                        Moon.descriptor.semimajoraxis = Moon.descriptor.semiminoraxis + ((float)rnd.NextDouble() * 2.0f) * system_scale;

                        Moon.descriptor.rotation      = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                        Moon.descriptor.mean_anomaly  = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                        Moon.descriptor.compute();

                        State.planets.Add(Moon);

                        ObjectInfo<SystemPlanetRenderer> MoonInfo = new();

                        MoonInfo.Object               = new();
                        MoonInfo.Object.name          = "Moon renderer";

                        MoonInfo.Renderer             = MoonInfo.Object.AddComponent<SystemPlanetRenderer>();
                        MoonInfo.Renderer.planet      = Moon;

                        Moons.Add(Moon, MoonInfo);
                    }
                }

                /*OrbitingObjectDescriptor OuterAsteroidBeltDescriptor = new();

                OuterAsteroidBeltDescriptor.CenterX = State.Star.posx;
                OuterAsteroidBeltDescriptor.CenterY = State.Star.posy;

                OuterAsteroidBeltDescriptor.semiminoraxis = State.Planets[InnerPlanets + OuterPlanets - 1].descriptor.semimajoraxis + 24.0f;
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

                for(int i = 0; i < FarOrbitPlanets; i++) {
                    SystemPlanet Planet = new SystemPlanet();

                    Planet.descriptor.central_body = State.stars[0].self;

                    //Planet.descriptor.semiminoraxis = InnerAsteroidBeltDescriptor.semimajoraxis + (i + 3) * (i + 3);
                    //Planet.descriptor.semimajoraxis = Planet.descriptor.semiminoraxis + (float)rnd.NextDouble() * i / 2.0f;

                    Planet.descriptor.semiminoraxis = State.planets[InnerPlanets + OuterPlanets - 1].descriptor.semimajoraxis + ((i + 3) * (i + 3) * 31 * system_scale);
                    Planet.descriptor.semimajoraxis = Planet.descriptor.semiminoraxis + (float)rnd.NextDouble() * (i + 1) * 82 * system_scale;

                    Planet.descriptor.rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                    Planet.descriptor.mean_anomaly = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                    Planet.descriptor.compute();

                    Planet.descriptor.self.mass = PlanetMass;

                    ObjectInfo<SystemPlanetRenderer> PlanetInfo = new();

                    PlanetInfo.Object = new();
                    PlanetInfo.Object.name = "Planet renderer #" + (i + InnerPlanets + OuterPlanets);

                    PlanetInfo.Renderer = PlanetInfo.Object.AddComponent<SystemPlanetRenderer>();
                    PlanetInfo.Renderer.planet = Planet;

                    State.planets.Add(Planet);
                    Planets.Add(Planet, PlanetInfo);
                }

                foreach(SystemPlanet Planet in State.planets) {
                    State.objects.Add(Planet.descriptor.self);
                }
                State.objects.Add(State.stars[0].self);

                for(int i = 0; i < SpaceStations; i++) {
                    SpaceStation Station = new();

                    Station.descriptor.central_body  = State.stars[0].self;

                    Station.descriptor.semiminoraxis = ((float)rnd.NextDouble() * State.planets[InnerPlanets + OuterPlanets - 1].descriptor.semimajoraxis + 4.0f);
                    Station.descriptor.semimajoraxis =  (float)rnd.NextDouble() * system_scale + Station.descriptor.semiminoraxis;

                    Station.descriptor.rotation      =  (float)rnd.NextDouble() * 2.0f * 3.1415926f;
                    Station.descriptor.mean_anomaly  =  (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                    Station.descriptor.compute();

                    Station.descriptor.self.mass = StationMass;

                    ObjectInfo<SpaceStationRenderer> StationInfo = new();

                    StationInfo.Object = new();
                    StationInfo.Object.name = "Space station renderer #" + i;

                    StationInfo.Renderer = StationInfo.Object.AddComponent<SpaceStationRenderer>();
                    StationInfo.Renderer.Station = Station;

                    State.stations.Add(Station);
                    Stations.Add(Station, StationInfo);
                }

                /*for(int i = 0; i < State.planets.Count; i++)
                    if(State.planets[i].descriptor.central_body == State.star)
                        for(int j = 0; j < State.planets.Count; j++)
                            if(i != j && State.planets[j].descriptor.central_body == State.star) {
                                SystemShip ship = new SystemShip();
                                ship.start = State.planets[i].descriptor;
                                ship.destination = State.planets[j].descriptor;
                                ship.descriptor = new OrbitingObjectDescriptor(ship.start, ship.self);

                                State.ships.Add(ship);

                                ObjectInfo<SystemShipRenderer> ShipInfo = new();

                                ShipInfo.Object = new GameObject();
                                ShipInfo.Object.name = "ship renderer";

                                ShipInfo.Renderer = ShipInfo.Object.AddComponent<SystemShipRenderer>();
                                ShipInfo.Renderer.ship = ship;

                                Ships.Add(ship, ShipInfo);
                            }*/

                foreach(SystemPlanet planet in State.planets) {
                    planet.descriptor.update_position(0.0f);

                    // Exactly as expected, it is impossible to orbit a planet when you make the planet not move around the star
                    // and when you disable the star's impact on the player when he's orbiting the planet

                    // While this was expected, this janky and ugly solution makes it at least kind of work. It would however
                    // be much easier and more intuitive to just have everything orbit, you know, as you would expect it. A much
                    // better way of achieving the same effect of "simplicity" would be to either make the system astronomically
                    // large, or to make the gravity of all objects a lot smaller.

                    planet.descriptor.self.velx =
                    planet.descriptor.self.vely = 0.0f;
                }

                foreach(SpaceStation station in State.stations) {
                    station.descriptor.update_position(0.0f);

                    // See above

                    station.descriptor.self.velx =
                    station.descriptor.self.vely = 0.0f;
                }

                State.player = gameObject.AddComponent<PlayerShip>();
                State.player.system_scale = system_scale;
            }

            private SpaceObject gravity_cycle(SpaceObject self, float current_time) {
                SpaceObject strongest_body = null;
                float       maxg           = 0.0f;
                float       grav_velx       = 0.0f;
                float       grav_vely       = 0.0f;

                foreach(SpaceObject body in State.objects) {

                    if(body == self) continue;

                    float dx = body.posx - self.posx;
                    float dy = body.posy - self.posy;

                    float d2 = dx * dx + dy * dy;
                    float d = (float)Math.Sqrt(d2);

                    float g = Tools.gravitational_constant * body.mass / d2;

                    if(g > maxg) strongest_body = body;

                    if(n_body_gravity) {

                        float Velocity = g * current_time;

                        grav_velx += Velocity * dx / d;
                        grav_vely += Velocity * dy / d;

                    } else {

                        if(g > maxg) {
                            maxg = g;
                            float vel = g * current_time;

                            grav_velx = vel * dx / d;
                            grav_vely = vel * dy / d;
                        }

                    }

                }

                self.posx += self.velx * current_time + 0.5f * grav_velx * current_time;
                self.posy += self.vely * current_time + 0.5f * grav_vely * current_time;

                self.velx += grav_velx;
                self.vely += grav_vely;

                return strongest_body;
            }
            void Update() {
                float current_time = (Time.time - LastTime) * time_scale;
                LastTime = Time.time;

                if(CachedSunMass != SunMass) {
                    State.stars[0].self.mass = CachedSunMass = SunMass;

                    for(int i = 0; i < Planets.Count; i++)
                        Planets.ElementAt(i).Key.descriptor.compute();

                    foreach(SystemShip ship in State.ships)
                        ship.descriptor.compute();
                }

                if(CachedPlanetMass != PlanetMass) {
                    CachedPlanetMass = PlanetMass;

                    for(int i = 0; i < Planets.Count; i++)
                        Planets.ElementAt(i).Key.descriptor.self.mass = PlanetMass;

                    for(int i = 0; i < Moons.Count; i++)
                        Moons.ElementAt(i).Key.descriptor.compute();
                }

                if(CachedMoonMass != MoonMass) {
                    CachedPlanetMass = MoonMass;

                    for(int i = 0; i < Moons.Count; i++)
                        Moons.ElementAt(i).Key.descriptor.self.mass = MoonMass;
                }
                
                foreach(SystemShip s in State.ships) {
                    if(!s.path_planned)
                        s.path_planned = s.descriptor.plan_path(s.destination, 0.1f * system_scale);
                    else if(s.descriptor.get_distance_from(s.destination) < system_scale) {
                        s.descriptor.copy(s.destination);
                        s.path_planned = false;
                        (s.start, s.destination) = (s.destination, s.start);
                    }

                    s.descriptor.update_position(current_time);
                }

                State.player.stations_orbiting = planet_movement;

                if(planet_movement) {

                    if(StarCount <= 1 || !n_body_gravity) {

                        foreach(SystemPlanet p in State.planets)
                            p.descriptor.update_position(current_time);

                        foreach(SpaceStation s in State.stations)
                            s.descriptor.update_position(current_time);

                    } else {

                        foreach(SystemStar star in State.stars) {

                            SpaceObject strongest_body = gravity_cycle(star.self, current_time);

                            if(strongest_body != null)
                                star.descriptor.change_frame_of_reference(strongest_body);

                        }

                        foreach(SystemPlanet planet in State.planets) {

                            SpaceObject strongest_body = gravity_cycle(planet.self, current_time);

                            if(strongest_body != null)
                                planet.descriptor.change_frame_of_reference(strongest_body);

                        }

                        foreach(SpaceStation station in State.stations) {

                            SpaceObject strongest_body = gravity_cycle(station.self, current_time);

                            if(strongest_body != null)
                                station.descriptor.change_frame_of_reference(strongest_body);

                        }

                    }

                }

                if(!State.player.ship.ignore_gravity) {
                    float maxg = 0.0f;
                    float GravVelX = 0.0f;
                    float GravVelY = 0.0f;

                    // this behaves weird when getting really close to central body --- is float too inaccurate?
                    foreach(SpaceObject body in State.objects) {

                        float dx = body.posx - State.player.ship.self.posx;
                        float dy = body.posy - State.player.ship.self.posy;

                        float d2 = dx * dx + dy * dy;
                        float d = (float)Math.Sqrt(d2);

                        float g = Tools.gravitational_constant * body.mass / d2;

                        if(n_body_gravity) {

                            float Velocity = g * current_time;

                            GravVelX += Velocity * dx / d;
                            GravVelY += Velocity * dy / d;

                        } else {

                            if(g > maxg) {
                                maxg = g;
                                float vel = g * current_time;

                                GravVelX = vel * dx / d;
                                GravVelY = vel * dy / d;
                            }

                        }

                    }

                    State.player.gravitational_strength = (float)Math.Sqrt(GravVelX * GravVelX + GravVelY * GravVelY) * 0.4f / current_time;

                    State.player.ship.self.velx   += GravVelX;
                    State.player.ship.self.vely   += GravVelY;

                    // For some reason this messes stuff up?!

                    //State.Player.ship.self.posx   += GravVelX * CurrentTime * 0.5f;
                    //State.Player.ship.self.posy   += GravVelY * CurrentTime * 0.5f;
                }

                State.player.ship.acceleration = acceleration;
                State.player.drag_factor       = drag_factor;
                State.player.sailing_factor    = sailing_factor;
                State.player.time_scale        = time_scale;

                UpdateDropdownMenu();

                if(TrackingPlayer) {
                    if(Input.GetMouseButton(1)) TrackingPlayer = false; // Disable camera tracking if user is manually moving camera
                    else
                        Camera.set_position(-State.player.ship.self.posx, -State.player.ship.self.posy, Camera.scale);
                }
            }

            private void UpdateDropdownMenu() {
                DockingTargetSelector.ClearOptions();

                List<string> Options = new();

                if(Stations.Count == 0) {
                    Options.Add("-- No stations --");

                    DockingTargetSelector.interactable = false;
                } else {
                    Options.Add("-- Select a station --");

                    for(int i = 0; i < Stations.Count;)
                        Options.Add("Station " + ++i);

                    DockingTargetSelector.interactable = true;
                }

                DockingTargetSelector.AddOptions(Options);

                DockingTargetSelector.value = 0;

                for(int i = 0; i < Stations.Count; i++)
                    if(Stations.ElementAt(i).Key == DockingTarget) {
                        DockingTargetSelector.value = i + 1;
                        break;
                    }
            }

            public void SelectDockingTarget(int i) {
                if(i == 0 || i > Stations.Count) {
                    DockingTarget = null;
                    State.player.ship.disengage_docking_autopilot();
                } else if(DockingTarget != Stations.ElementAt(i - 1).Key)
                    State.player.ship.engage_docking_autopilot(DockingTarget = Stations.ElementAt(i - 1).Key);
            }
        }
    }
}
