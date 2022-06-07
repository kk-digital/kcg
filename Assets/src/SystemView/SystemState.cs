using System.Collections;
using System.Collections.Generic;

namespace SystemView
{
    public class SystemState
    {
        public List<SystemPlanet>       Planets       = new();
        //public List<SystemAsteroidBelt> AsteroidBelts = new();
        public List<SystemShip>         Ships         = new();
        public List<SpaceStation>       Stations      = new();
        public List<LaserTower>         LaserTowers   = new();

        public SystemViewBody Star = new();
    }
}
