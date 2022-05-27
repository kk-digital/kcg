using System.Collections;
using System.Collections.Generic;

namespace SystemView
{
    public class SystemState
    {
        public List<SystemPlanet> Planets = new List<SystemPlanet>();
        public List<SystemAsteroidBelt> AsteroidBelts = new List<SystemAsteroidBelt>();
        public SystemStar Star;
    }
}
