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

        public AgentEntity AddPlayer(UnityEngine.Material material, Vector2 position)
        {
            GameEntity entity = GameState.SpawnerSystem.SpawnPlayer(material, position);
            AgentEntity newEntity = AgentList.Add(entity);

            return newEntity;
        }

        public AgentEntity AddAgent(UnityEngine.Material material, Vector2 position)
        {
            GameEntity entity = GameState.SpawnerSystem.SpawnAgent(material, position);
            AgentEntity newEntity = AgentList.Add(entity);

            return newEntity;
        }

        public AgentEntity AddEnemy(UnityEngine.Material material, Vector2 position)
        {
            GameEntity entity = GameState.SpawnerSystem.SpawnEnemy(material, position);
            AgentEntity newEntity = AgentList.Add(entity);

            return newEntity;
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
        public void Update(float deltaTime, Material material, Transform transform)
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
                        if (projectile.IsInitialized)
                        {
                            var position = projectile.Entity.projectilePhysicsState2D;
                        }
                    }


                }

            }


            

            GameState.ProcessSystem.Update();
            GameState.MovableSystem.Update();
            GameState.ProcessCollisionSystem.Update(TileMap);
            GameState.EnemyAiSystem.Update();
            
            TileMap.Layers.DrawLayer(Enums.Tile.MapLayerType.Front, Object.Instantiate(material), transform, 10);
            TileMap.Layers.DrawLayer(Enums.Tile.MapLayerType.Ore, Object.Instantiate(material), transform, 11);
            GameState.DrawSystem.Draw(Object.Instantiate(material), transform, 12);


           
        }
    }
}