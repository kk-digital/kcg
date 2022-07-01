using System.Collections.Generic;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class SystemState : MonoBehaviour {
            public List<SystemPlanet>       planets       = new();
            //public List<SystemAsteroidBelt> AsteroidBelts = new();
            public List<SystemShip>         ships         = new();
            public List<SpaceStation>       stations      = new();
            public List<LaserTower>         laser_towers  = new();
            public List<SpaceObject>        objects       = new();
            public List<SystemStar>         stars         = new();

            public PlayerShip player;

            public void load(string file_name) {

            }

            public void save(string file_name) {

            }
        }
    }
}
