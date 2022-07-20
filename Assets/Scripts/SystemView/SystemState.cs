using System.Collections.Generic;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class ObjectInfo<ObjectT, RendererT> where ObjectT : new() {
            public GameObject UnityObject;
            public ObjectT    Object = new();
            public RendererT  Renderer;
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

            public void generate_renderers() {
                for(int i = 0; i < planets.Count; i++) {
                    var planet               = planets[i];

                    planet.Object.descriptor.compute();

                    planet.UnityObject       = new GameObject("Planet #" + (i + 1));
                    planet.Renderer          = planet.UnityObject.AddComponent<SystemPlanetRenderer>();
                    planet.Renderer.planet   = planet.Object;

                    objects.Add(planet.Object.self);
                }

                for(int i = 0; i < ships.Count; i++) {
                    var ship                 = ships[i];

                    ship.Object.descriptor.compute();

                    ship.UnityObject         = new GameObject("Ship #" + (i + 1));
                    ship.Renderer            = ship.UnityObject.AddComponent<SystemShipRenderer>();
                    ship.Renderer.ship       = ship.Object;
                }

                for(int i = 0; i < stations.Count; i++) {
                    var station              = stations[i];

                    station.Object.descriptor.compute();

                    station.UnityObject      = new GameObject("Station #" + (i + 1));
                    station.Renderer         = station.UnityObject.AddComponent<SpaceStationRenderer>();
                    station.Renderer.Station = station.Object;
                }

                for(int i = 0; i < stars.Count; i++) {
                    var star                 = stars[i];

                    if(stars.Count > 1)
                        star.Object.descriptor.compute();

                    star.UnityObject         = new GameObject("Star #" + (i + 1));
                    star.Renderer            = star.UnityObject.AddComponent<SystemStarRenderer>();
                    star.Renderer.star       = star.Object;

                    objects.Add(star.Object.self);
                }
            }

            public void cleanup() {
                while(stars.Count > 0) {
                    GameObject.Destroy(stars[0].Renderer);
                    GameObject.Destroy(stars[0].UnityObject);
                    stars.Remove(stars[0]);
                }

                stars.Clear();

                while(ships.Count > 0) {
                    GameObject.Destroy(ships[0].Renderer);
                    GameObject.Destroy(ships[0].UnityObject);
                    ships.Remove(ships[0]);
                }

                ships.Clear();

                while(planets.Count > 0) {
                    GameObject.Destroy(planets[0].Renderer);
                    GameObject.Destroy(planets[0].UnityObject);
                    planets.Remove(planets[0]);
                }

                planets.Clear();

                while(stations.Count > 0) {
                    GameObject.Destroy(stations[0].Renderer);
                    GameObject.Destroy(stations[0].UnityObject);
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
