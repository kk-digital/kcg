using System.Collections.Generic;
using UnityEngine;

namespace Planet
{



    public struct PlanetState
    {
        public int Index;


        public PlanetTileMap.PlanetTileMap TileMap;
        public List < int > AgentIdList;
        public List < int > ParticleIdList;
        public List < int > ProjectileIdList;
        public List < int > VehicleIdList;
        public List < int > ItemParticlesList;


        public void Init(Vector2Int mapSize)
        {
            TileMap = new PlanetTileMap.PlanetTileMap(mapSize);
            AgentIdList = new List<int>();
            ParticleIdList = new List<int>();
            ProjectileIdList = new List<int>();
            VehicleIdList = new List<int>();
        }

        public void AddAgent()
        {

        }

        public void RemoveAgent()
        {

        }

        public void AddParticle()
        {

        }

        public void RemoveParticle()
        {

        }

        public void AddProjectile()
        {

        }

        public void RemoveProjectile()
        {

        }

        public void AddVehicle()
        {

        }

        public void RemoveVehicle()
        {

        }
    }
}