using System.Collections.Generic;
using KMath;
using Planet;
using UnityEngine;

namespace World
{
    public class WorldState
    {
        int planetId;

        List<PlanetState> PlanetList;



        public WorldState()
        {
            PlanetList = new List<PlanetState>();
        }


        public PlanetState AddPlanet()
        {
            PlanetState newPlanet = new PlanetState();
            newPlanet.Init(new Vec2i(16, 16));
            PlanetList.Add(newPlanet);

            newPlanet.Index = PlanetList.Count - 1;

            return newPlanet;
        }
    }
}
