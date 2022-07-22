using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {

        public class SystemViewTest : MonoBehaviour {
            public SystemState       State;

            private float            LastTime;
            public System.Random     rnd = new System.Random();

            public  int              StarCount        =                    1;
            public  int              InnerPlanets     =                    4;
            public  int              OuterPlanets     =                    6;
            public  int              FarOrbitPlanets  =                    2;
            public  int              SpaceStations    =                    0;

            public  float            system_scale     =                25.0f;

            public  float            SunMass          = 50000000000000000.0f;
            public  float            PlanetMass       =   100000000000000.0f;
            public  float            MoonMass         =    20000000000000.0f;
            public  float            StationMass      =        1000000000.0f;

            public  float            time_scale       =                 1.0f;

            public  float            acceleration     =               250.0f;
            public  float            drag_factor      =             10000.0f;
            public  float            sailing_factor   =                20.0f;

            private float            CachedSunMass    = 50000000000000000.0f;
            private float            CachedPlanetMass =   100000000000000.0f;
            private float            CachedMoonMass   =    20000000000000.0f;

            public  bool             TrackingPlayer   =                false;
            public  bool             planet_movement  =                 true;
            public  bool             n_body_gravity   =                 true;

            public  ComputeShader    blur_noise_shader;
            public  ComputeShader    scale_noise_shader;
            public  ComputeShader    exponential_filter_shader;
            public  ComputeShader    distortion_shader;
            public  ComputeShader    circular_blur_shader;
            public  ComputeShader    circular_mask_shader;

            public  Color            rocky_planet_base_color;
            public  Color            gas_giant_base_color;
            public  Color            moon_base_color;

            public  int              rocky_planet_radius;
            public  int              gas_giant_radius;
            public  int              moon_radius;

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

                State.cleanup();

                for(int i = 0; i < StarCount; i++) {

                    State.stars.Add(new());

                    State.stars[i].obj.self.mass                    = SunMass * (float)rnd.NextDouble() * (i + 1);
                    State.stars[i].obj.self.posx                    = ((float)rnd.NextDouble() * 16.0f - 64.0f) * system_scale;
                    State.stars[i].obj.self.posy                    = ((float)rnd.NextDouble() * 16.0f -  8.0f) * system_scale;
                    State.stars[i].obj.render_orbit                 = StarCount > 1;

                }

                if(StarCount > 1)
                    for(int i = 0; i < StarCount; i++) {

                        int j;
                        do j = rnd.Next(StarCount);
                        while(j == i);

                        State.stars[i].obj.descriptor.semiminoraxis = (float)rnd.NextDouble() * 32.0f * system_scale;
                        State.stars[i].obj.descriptor.semimajoraxis = (float)rnd.NextDouble() * 32.0f * system_scale + State.stars[i].obj.descriptor.semiminoraxis;
                        State.stars[i].obj.descriptor.rotation      = (float)rnd.NextDouble() * Tools.twopi;
                        State.stars[i].obj.descriptor.rotation      = (float)rnd.NextDouble() * Tools.twopi;
                        State.stars[i].obj.descriptor.mean_anomaly  = (float)rnd.NextDouble() * Tools.twopi;
                        State.stars[i].obj.descriptor.central_body  = State.stars[j].obj.self;

                    }

                for(int i = 0; i < InnerPlanets; i++) {

                    SystemPlanet Planet = new SystemPlanet();

                    Planet.descriptor.central_body  = State.stars[0].obj.self;
                    Planet.descriptor.semiminoraxis = (30.0f + (i + 1) * (i + 1) * 10) * system_scale;
                    Planet.descriptor.semimajoraxis = Planet.descriptor.semiminoraxis + ((float)rnd.NextDouble() * (i + 5) * system_scale);
                    Planet.descriptor.rotation      = (float)rnd.NextDouble() * Tools.twopi;
                    Planet.descriptor.mean_anomaly  = (float)rnd.NextDouble() * Tools.twopi;
                    Planet.descriptor.self.mass     = PlanetMass;
                    Planet.type                     = PlanetType.PLANET_ROCKY;

                    State.planets.Add(new());
                    var p = State.planets[State.planets.Count - 1];
                    p.obj = Planet;

                }

                for(int i = 0; i < OuterPlanets; i++) {

                    SystemPlanet Planet = new SystemPlanet();

                    Planet.descriptor.central_body  = State.stars[0].obj.self;
                    Planet.descriptor.semiminoraxis = State.planets[InnerPlanets - 1].obj.descriptor.semimajoraxis + ((i + 3) * (i + 3) * 10 * system_scale);
                    Planet.descriptor.semimajoraxis = Planet.descriptor.semiminoraxis + ((float)rnd.NextDouble() * i / 20.0f) * system_scale;
                    Planet.descriptor.rotation      = (float)rnd.NextDouble() * Tools.twopi;
                    Planet.descriptor.mean_anomaly  = (float)rnd.NextDouble() * Tools.twopi;
                    Planet.descriptor.self.mass     = PlanetMass;
                    Planet.type                     = PlanetType.PLANET_GAS_GIANT;

                    State.planets.Add(new());
                    var p = State.planets[State.planets.Count - 1];
                    p.obj = Planet;

                    for(int j = 0; j < rnd.Next(i + 1); j++) {

                        SystemPlanet Moon = new SystemPlanet();

                        Moon.descriptor.self.mass     = MoonMass;
                        Moon.descriptor.central_body  = Planet.descriptor.self;
                        Moon.descriptor.semiminoraxis = ((float)rnd.NextDouble() * (j + 1) + 5.0f) * system_scale;
                        Moon.descriptor.semimajoraxis = Moon.descriptor.semiminoraxis + ((float)rnd.NextDouble() * 2.0f) * system_scale;
                        Moon.descriptor.rotation      = (float)rnd.NextDouble() * Tools.twopi;
                        Moon.descriptor.mean_anomaly  = (float)rnd.NextDouble() * Tools.twopi;
                        Moon.type                     = PlanetType.PLANET_MOON;

                        State.planets.Add(new());
                        var m = State.planets[State.planets.Count - 1];
                        m.obj = Moon;

                    }

                }

                for(int i = 0; i < FarOrbitPlanets; i++) {

                    SystemPlanet Planet = new SystemPlanet();

                    Planet.descriptor.central_body  = State.stars[0].obj.self;
                    Planet.descriptor.semiminoraxis = State.planets[InnerPlanets + OuterPlanets - 1].obj.descriptor.semimajoraxis
                                                    + ((i + 3) * (i + 3) * 31 * system_scale);
                    Planet.descriptor.semimajoraxis = Planet.descriptor.semiminoraxis + (float)rnd.NextDouble() * (i + 1) * 82 * system_scale;
                    Planet.descriptor.rotation      = (float)rnd.NextDouble() * Tools.twopi;
                    Planet.descriptor.mean_anomaly  = (float)rnd.NextDouble() * Tools.twopi;
                    Planet.descriptor.self.mass     = PlanetMass;
                    Planet.type                     = PlanetType.PLANET_ROCKY;

                    State.planets.Add(new());
                    var p = State.planets[State.planets.Count - 1];
                    p.obj = Planet;

                }

                for(int i = 0; i < SpaceStations; i++) {

                    State.stations.Add(new());

                    State.stations[i].obj.descriptor.central_body  =  State.stars[0].obj.self;
                    State.stations[i].obj.descriptor.semiminoraxis = ((float)rnd.NextDouble()
                                                                   *  State.planets[InnerPlanets + OuterPlanets - 1].obj.descriptor.semimajoraxis + 4.0f);
                    State.stations[i].obj.descriptor.semimajoraxis =  (float)rnd.NextDouble() * system_scale
                                                                   +  State.stations[i].obj.descriptor.semiminoraxis;
                    State.stations[i].obj.descriptor.rotation      =  (float)rnd.NextDouble() * Tools.twopi;
                    State.stations[i].obj.descriptor.mean_anomaly  =  (float)rnd.NextDouble() * Tools.twopi;
                    State.stations[i].obj.descriptor.self.mass     =  StationMass;

                }

                State.create_renderers();

                foreach(var Planet in State.planets) {
                    Planet.renderer.blur_noise_shader         = blur_noise_shader;
                    Planet.renderer.scale_noise_shader        = scale_noise_shader;
                    Planet.renderer.exponential_filter_shader = exponential_filter_shader;
                    Planet.renderer.distortion_shader         = distortion_shader;
                    Planet.renderer.circular_blur_shader      = circular_blur_shader;
                    Planet.renderer.circular_mask_shader      = circular_mask_shader;

                    switch(Planet.obj.type) {
                        case PlanetType.PLANET_ROCKY:
                            Planet.renderer.radius    = rocky_planet_radius;
                            Planet.renderer.basecolor = rocky_planet_base_color;
                            break;
                        case PlanetType.PLANET_GAS_GIANT:
                            Planet.renderer.radius    = gas_giant_radius;
                            Planet.renderer.basecolor = gas_giant_base_color;
                            break;
                        case PlanetType.PLANET_MOON:
                            Planet.renderer.radius    = moon_radius;
                            Planet.renderer.basecolor = moon_base_color;
                            break;
                    }
                }

                State.generate_renderers();

                State.player              = gameObject.AddComponent<PlayerShip>();
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

                /*if(CachedSunMass != SunMass) {
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
                }*/
                
                foreach(var ship in State.ships) {
                    SystemShip s = ship.obj;

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

                        foreach(var p in State.planets)
                            p.obj.descriptor.update_position(current_time);

                        foreach(var s in State.stations)
                            s.obj.descriptor.update_position(current_time);

                    } else {

                        foreach(var star in State.stars) {

                            SpaceObject strongest_body = gravity_cycle(star.obj.self, current_time);

                            if(strongest_body != null)
                                star.obj.descriptor.change_frame_of_reference(strongest_body);

                        }

                        foreach(var planet in State.planets) {

                            SpaceObject strongest_body = gravity_cycle(planet.obj.self, current_time);

                            if(strongest_body != null)
                                planet.obj.descriptor.change_frame_of_reference(strongest_body);

                        }

                        foreach(var station in State.stations) {

                            SpaceObject strongest_body = gravity_cycle(station.obj.self, current_time);

                            if(strongest_body != null)
                                station.obj.descriptor.change_frame_of_reference(strongest_body);

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

                if(State.stations.Count == 0) {
                    Options.Add("-- No stations --");

                    DockingTargetSelector.interactable = false;
                } else {
                    Options.Add("-- Select a station --");

                    for(int i = 0; i < State.stations.Count;)
                        Options.Add("Station " + ++i);

                    DockingTargetSelector.interactable = true;
                }

                DockingTargetSelector.AddOptions(Options);

                DockingTargetSelector.value = 0;

                for(int i = 0; i < State.stations.Count; i++)
                    if(State.stations[i].obj == DockingTarget) {
                        DockingTargetSelector.value = i + 1;
                        break;
                    }
            }

            public void SelectDockingTarget(int i) {
                if(i == 0 || i > State.stations.Count) {
                    DockingTarget = null;
                    State.player.ship.disengage_docking_autopilot();
                } else if(DockingTarget != State.stations[i - 1].obj)
                    State.player.ship.engage_docking_autopilot(DockingTarget = State.stations[i - 1].obj);
            }
        }
    }
}
