using Agent;
using Vehicle;
using Projectile;
using FloatingText;
using KMath;
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
        public FloatingTextList FloatingTextList;


        public GameContext GameContext;


        public PlanetState(Vec2i mapSize, GameContext gameContext)
        {
            TileMap = new TileMap(mapSize);
            AgentList = new AgentList();
            VehicleList = new VehicleList();
            ProjectileList = new ProjectileList();
            FloatingTextList = new FloatingTextList();

            GameContext = gameContext;
        }


        public AgentEntity AddPlayer(UnityEngine.Material material, int spriteId,
                                int width, int height, Vec2f position, int startingAnimation)
        {
            ref AgentEntity newEntity = ref AgentList.Add();
            GameEntity gameEntity = GameState.AgentSpawnerSystem.SpawnPlayer(material, spriteId, width, height, position, newEntity.AgentId,
                    startingAnimation);
            newEntity.Entity = gameEntity;

            return newEntity;
        }

        public AgentEntity AddAgent(UnityEngine.Material material, int spriteId, int width,
                     int height, Vec2f position, int startingAnimation)
        {
            ref AgentEntity newEntity = ref AgentList.Add();
            GameEntity entity = GameState.AgentSpawnerSystem.SpawnAgent(material, spriteId, width, height, position,
                                                                    newEntity.AgentId, startingAnimation);
            newEntity.Entity = entity;


            return newEntity;
        }

        public AgentEntity AddEnemy(UnityEngine.Material material, int spriteId,
                        int width, int height, Vec2f position, int startingAnimation)
        {
            ref AgentEntity newEntity = ref AgentList.Add();
            GameEntity entity = GameState.AgentSpawnerSystem.SpawnEnemy(material, spriteId, width, height, position,
            newEntity.AgentId, startingAnimation);

            newEntity.Entity = entity;


            return newEntity;
        }

        public void RemoveAgent(int Index)
        {
            ref AgentEntity entity = ref AgentList.Get(Index);
            entity.Entity.Destroy();
            AgentList.Remove(entity);
        }

        public FloatingTextEntity AddFloatingText(string text, float timeToLive, Vec2f velocity, Vec2f position)
        {
            ref FloatingTextEntity newEntity = ref FloatingTextList.Add();
            GameEntity entity = GameState.FloatingTextSpawnerSystem.SpawnFloatingText(GameContext, text, timeToLive, velocity, position,
                         newEntity.FloatingTextId);

            newEntity.Entity = entity;

            return newEntity;
        }

        public void RemoveFloatingText(int Index)
        {
            ref FloatingTextEntity entity = ref FloatingTextList.Get(Index);
            entity.Entity.Destroy();
            FloatingTextList.Remove(entity);
        }

        public void AddParticle()
        {

        }

        public void RemoveParticle()
        {

        }

        public ProjectileEntity AddProjectile(UnityEngine.Material material, Vector2 position)
        {
            return new ProjectileEntity();
        }

        public void RemoveProjectile(ProjectileEntity entity)
        {
            ProjectileList.Remove(entity);
        }

        public VehicleEntity AddVehicle(UnityEngine.Material material, Vector2 position)
        {
            return new VehicleEntity();
        }

        public void RemoveVehicle(VehicleEntity entity)
        {
            VehicleList.Remove(entity);
        }

        // used to place a tile into the tile map
        // x, y is the position in the tile map
        public void PlaceTile(int x, int y, int tileType, Enums.Tile.MapLayerType layer)
        {
            Tile.Tile tile = Tile.Tile.Empty;
            tile.Type = tileType;
            TileMap.AddTile(x, y, tile, layer);
        }


        // updates the entities, must call the systems and so on ..
        public void Update(float deltaTime, Material material, Transform transform)
        {
            float targetFps = 30.0f;
            float frameTime = 1.0f / targetFps;

            TimeState.Deficit += deltaTime;

            while (TimeState.Deficit >= frameTime)
            {
                TimeState.Deficit -= frameTime;
                // do a server/client tick right here
                {
                    TimeState.TickTime++;





                    for (int index = 0; index < ProjectileList.Capacity; index++)
                    {
                        ref ProjectileEntity projectile = ref ProjectileList.List[index];
                        if (projectile.IsInitialized)
                        {
                            var position = projectile.Entity.projectilePhysicsState2D;
                        }
                    }


                }

            }

            // check if the sprite atlas textures needs to be updated
            for(int type = 0; type < GameState.SpriteAtlasManager.Length; type++)
            {
                GameState.SpriteAtlasManager.UpdateAtlasTexture(type);
            }

            // check if the tile sprite atlas textures needs to be updated
            for(int type = 0; type < GameState.TileSpriteAtlasManager.Length; type++)
            {
                GameState.TileSpriteAtlasManager.UpdateAtlasTexture(type);
            }

            // calling all the systems we have

            GameState.InputProcessSystem.Update();
            GameState.PhysicsMovableSystem.Update();
            GameState.PhysicsProcessCollisionSystem.Update(TileMap);
            GameState.EnemyAiSystem.Update(this);
            GameState.FloatingTextUpdateSystem.Update(this, frameTime);
            GameState.AnimationUpdateSystem.Update(frameTime);
            GameState.ActionSchedulerSystem.Update(frameTime);
            GameState.ItemPickUpSystem.Update(GameContext);


            TileMap.Layers.DrawLayer(TileMap, Enums.Tile.MapLayerType.Mid, Object.Instantiate(material), transform, 9);
            TileMap.Layers.DrawLayer(TileMap, Enums.Tile.MapLayerType.Front, Object.Instantiate(material), transform, 10);
            TileMap.Layers.DrawLayer(TileMap, Enums.Tile.MapLayerType.Ore, Object.Instantiate(material), transform, 11);
            GameState.AgentDrawSystem.Draw(Object.Instantiate(material), transform, 12);
            GameState.ItemDrawSystem.Draw(GameContext, Material.Instantiate(material), transform, 13);
            GameState.FloatingTextDrawSystem.Draw(transform, 10000);

            #region Gui drawing systems
            //GameState.InventoryDrawSystem.Draw(material, transform, 1000);
            #endregion
        }
    }
}
