using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class SystemState : MonoBehaviour
    {
        public List<SystemPlanet>       Planets       = new();
        //public List<SystemAsteroidBelt> AsteroidBelts = new();
        public List<SystemShip>         Ships         = new();
        public List<SpaceStation>       Stations      = new();
        public List<LaserTower>         LaserTowers   = new();
        public List<SystemViewBody>     Bodies        = new();

        public PlayerShip Player;

        public SystemViewBody Star = new();
    }
}
