using System.Collections.Generic;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class ObjectInfo<ObjectT, RendererT> where ObjectT : new() {
            public GameObject unity_object;
            public ObjectT    obj = new();
            public RendererT  renderer;
        }

        public class SystemState : MonoBehaviour {
            public List<ObjectInfo<SystemPlanet, SystemPlanetRenderer>> planets      = new();
            public List<ObjectInfo<SystemShip,   SystemShipRenderer  >> ships        = new();
            public List<ObjectInfo<SpaceStation, SpaceStationRenderer>> stations     = new();
            public List<ObjectInfo<SystemStar,   SystemStarRenderer  >> stars        = new();
            public List<LaserTower                                    > laser_towers = new();
            public List<SpaceObject                                   > objects      = new();

            public PlayerShip player;

            public void load(string file_name) {

            }

            public void save(string file_name) {

            }

            public void create_renderers() {
                for(int i = 0; i < planets.Count; i++) {
                    var planet               = planets[i];
                    planet.unity_object      = new GameObject("Planet #" + (i + 1));
                    planet.renderer          = planet.unity_object.AddComponent<SystemPlanetRenderer>();
                }

                for(int i = 0; i < ships.Count; i++) {
                    var ship                 = ships[i];
                    ship.unity_object        = new GameObject("Ship #" + (i + 1));
                    ship.renderer            = ship.unity_object.AddComponent<SystemShipRenderer>();
                }

                for(int i = 0; i < stations.Count; i++) {
                    var station              = stations[i];
                    station.unity_object     = new GameObject("Station #" + (i + 1));
                    station.renderer         = station.unity_object.AddComponent<SpaceStationRenderer>();
                }

                for(int i = 0; i < stars.Count; i++) {
                    var star                 = stars[i];
                    star.unity_object        = new GameObject("Star #" + (i + 1));
                    star.renderer            = star.unity_object.AddComponent<SystemStarRenderer>();
                }
            }

            public void generate_renderers() {
                for(int i = 0; i < planets.Count; i++) {
                    var planet               = planets[i];

                    planet.obj.descriptor.compute();

                    planet.renderer.planet   = planet.obj;
                    planet.renderer.init();

                    objects.Add(planet.obj.self);
                }

                for(int i = 0; i < ships.Count; i++) {
                    var ship                 = ships[i];

                    ship.obj.descriptor.compute();

                    ship.renderer.ship       = ship.obj;
                }

                for(int i = 0; i < stations.Count; i++) {
                    var station              = stations[i];

                    station.obj.descriptor.compute();

                    station.renderer.Station = station.obj;
                }

                for(int i = 0; i < stars.Count; i++) {
                    var star                 = stars[i];

                    if(stars.Count > 1)
                        star.obj.descriptor.compute();

                    star.renderer.star       = star.obj;

                    objects.Add(star.obj.self);
                }
            }

            public void cleanup() {
                while(stars.Count > 0) {
                    GameObject.Destroy(stars[0].renderer);
                    GameObject.Destroy(stars[0].unity_object);
                    stars.Remove(stars[0]);
                }

                stars.Clear();

                while(ships.Count > 0) {
                    GameObject.Destroy(ships[0].renderer);
                    GameObject.Destroy(ships[0].unity_object);
                    ships.Remove(ships[0]);
                }

                ships.Clear();

                while(planets.Count > 0) {
                    GameObject.Destroy(planets[0].renderer);
                    GameObject.Destroy(planets[0].unity_object);
                    planets.Remove(planets[0]);
                }

                planets.Clear();

                while(stations.Count > 0) {
                    GameObject.Destroy(stations[0].renderer);
                    GameObject.Destroy(stations[0].unity_object);
                    stations.Remove(stations[0]);
                }

                stations.Clear();

                if(player != null) {
                    GameObject.Destroy(player);
                    player = null;
                }
            }
        }
    }
}
