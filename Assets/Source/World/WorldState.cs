using System.Collections;
using System.Collections.Generic;
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
            PlanetState newPlanet = new PlanetState(new Vector2Int(16, 16));
            PlanetList.Add(newPlanet);

            newPlanet.Index = PlanetList.Count - 1;

            return newPlanet;
        }
    }
}
