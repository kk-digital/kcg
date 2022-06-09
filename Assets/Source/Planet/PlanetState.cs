using System.Collections.Generic;
using Agent;
using Vehicle;
using Projectile;
using Entitas;
using UnityEngine;

namespace Planet
{
    public class PlanetState
    {
        public int Index;
        TimeState TimeState;

        public Planet.TileMap TileMap;
        public AgentList AgentList;
        public VehicleList VehicleList;
        public ProjectileList ProjectileList;
        public List < int > ParticleIdList;
        public List < int > ItemParticlesList;


        public PlanetState(UnityEngine.Vector2Int mapSize)
        {
            TileMap = new Planet.TileMap(mapSize);
            AgentList = new AgentList();
            VehicleList = new VehicleList();
            ProjectileList = new ProjectileList();
            ParticleIdList = new List<int>();
        }

        public void AddPlayer(UnityEngine.Material material, Vector2 position)
        {
            Entity entity = GameState.SpawnerSystem.SpawnPlayer(material, position);
            AgentList.Add(entity);
        }

        public void AddAgent(UnityEngine.Material material, Vector2 position)
        {
            Entity entity = GameState.SpawnerSystem.SpawnAgent(material, position);
            AgentList.Add(entity);
        }

        public void RemoveAgent(AgentEntity entity)
        {
            AgentList.Remove(entity);
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

        public void AddVehicle(UnityEngine.Material material, Vector2 position)
        {

        }

        public void RemoveVehicle()
        {

        }

        public void Update(float deltaTime)
        {
            float targetFps = 30.0f;
            float frameTime = 1.0f / targetFps;

            TimeState.Deficit += deltaTime;

            while(TimeState.Deficit >= frameTime)
            {
                TimeState.Deficit -= frameTime;
                // do a server/client tick right here
                {
                    TimeState.TickTime++;

                }

            }
        }
    }
}