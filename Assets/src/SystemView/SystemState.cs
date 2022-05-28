using System.Collections;
using System.Collections.Generic;

namespace SystemView
{
    public class SystemState
    {
        public List<SystemPlanet> Planets = new List<SystemPlanet>();
        public List<SystemAsteroidBelt> AsteroidBelts = new List<SystemAsteroidBelt>();
        public List<SystemShip> Ships = new List<SystemShip>();
        public SystemStar Star;
    }
}
