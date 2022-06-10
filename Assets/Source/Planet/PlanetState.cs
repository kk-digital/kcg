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


        public PlanetState(UnityEngine.Vector2Int mapSize)
        {
            TileMap = new Planet.TileMap(mapSize);
            AgentList = new AgentList();
            VehicleList = new VehicleList();
            ProjectileList = new ProjectileList();
        }

        public void AddPlayer(UnityEngine.Material material, Vector2 position)
        {
            GameEntity entity = GameState.SpawnerSystem.SpawnPlayer(material, position);
            AgentList.Add(entity);
        }

        public void AddAgent(UnityEngine.Material material, Vector2 position)
        {
            GameEntity entity = GameState.SpawnerSystem.SpawnAgent(material, position);
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

        // used to place a tile into the tile map
        // x, y is the position in the tile map
        public void PlaceTile(int x, int y, int tileType, Enums.Tile.MapLayerType layer)
        {
            Tile.Tile tile = Tile.Tile.EmptyTile;
            tile.Type = tileType;
            TileMap.PlaceTile(x, y, tile, layer);
        }


        // updates the entities, must call the systems and so on ..
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


                    for(int index = 0; index < ProjectileList.Capacity; index++)
                    {
                        ref ProjectileEntity projectile = ref ProjectileList.List[index];

                        var position = projectile.Entity.projectilePosition2D;
                    }

                }

            }
        }
    }
}