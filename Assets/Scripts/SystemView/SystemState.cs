using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class SystemState : MonoBehaviour
    {
        public List<SystemPlanet>       Planets       = new();
        //public List<SystemAsteroidBelt> AsteroidBelts = new();
        public List<SystemShip>         ships         = new();
        public List<SpaceStation>       Stations      = new();
        public List<LaserTower>         LaserTowers   = new();
        public List<SpaceObject>        Objects       = new();

        public PlayerShip Player;

        public SpaceObject Star = new();
    }
}
