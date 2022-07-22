using Source.SystemView;
using UnityEngine;

namespace Scripts {
    namespace SystemView {
        public class ProtostarTest : MonoBehaviour {
            public SystemState state;
            public int         inner_planets;
            public int         outer_planets;

            public float       solar_mass;
            public float       min_inner_planet_mass;
            public float       max_inner_planet_mass;
            public float       min_outer_planet_mass;
            public float       max_outer_planet_mass;

            private float      last_time;

            private void Start() {
                System.Random rng = new();

                state.stars.Add(new());

                state.stars[0].obj.self.mass    = solar_mass;
                state.stars[0].obj.self.posx    = 0.0f;
                state.stars[0].obj.self.posy    = 0.0f;
                state.stars[0].obj.render_orbit = false;
                state.stars[0].obj.brightness   = 0.08f;

                for(int i = 0; i < inner_planets; i++) {
                    SystemPlanet planet = new SystemPlanet();

                    planet.descriptor.central_body  = state.stars[0].obj.self;
                    planet.descriptor.semiminoraxis = 30.0f + (i + 1) * (i + 1) * 10.0f;
                    planet.descriptor.semimajoraxis = planet.descriptor.semiminoraxis + (float)rng.NextDouble() * (i + 3);
                    planet.descriptor.rotation      = (float)rng.NextDouble() * Tools.twopi;
                    planet.descriptor.mean_anomaly  = (float)rng.NextDouble() * Tools.twopi;
                    planet.descriptor.self.mass     = min_inner_planet_mass + (float)rng.NextDouble() * (max_inner_planet_mass - min_inner_planet_mass);

                    state.planets.Add(new());
                    var p = state.planets[state.planets.Count - 1];
                    p.obj = planet;
                }

                for(int i = 0; i < outer_planets; i++) {
                    SystemPlanet planet = new SystemPlanet();

                    planet.descriptor.central_body  = state.stars[0].obj.self;
                    planet.descriptor.semiminoraxis = 450.0f + (i + 1) * (i + 1) * 40.0f;
                    planet.descriptor.semimajoraxis = planet.descriptor.semiminoraxis + (float)rng.NextDouble() * (i + 10);
                    planet.descriptor.rotation      = (float)rng.NextDouble() * Tools.twopi;
                    planet.descriptor.mean_anomaly  = (float)rng.NextDouble() * Tools.twopi;
                    planet.descriptor.self.mass     = min_outer_planet_mass + (float)rng.NextDouble() * (max_outer_planet_mass - min_outer_planet_mass);

                    state.planets.Add(new());
                    var p = state.planets[state.planets.Count - 1];
                    p.obj = planet;
                }

                state.create_renderers();
                state.generate_renderers();
                last_time = Time.time;
            }

            private void Update() {
                float current_time = Time.time - last_time;
                      last_time    = Time.time;

                foreach(var planet in state.planets)
                    planet.obj.descriptor.update_position(-current_time);
            }
        }
    }
}
