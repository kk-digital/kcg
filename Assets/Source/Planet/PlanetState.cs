 using Agent;
using Enums.Tile;
using Vehicle;
using Projectile;
using FloatingText;
using Particle;
using KMath;
using UnityEngine;

namespace Planet
{
    public struct PlanetState
    {
        public int Index;
        public TimeState TimeState;

        public PlanetTileMap.TileMap TileMap;
        public AgentList AgentList;
        public VehicleList VehicleList;
        public ProjectileList ProjectileList;
        public FloatingTextList FloatingTextList;
        public ParticleEmitterList ParticleEmitterList;


        public GameContext GameContext;
        public ParticleContext ParticleContext;


        public PlanetState(Vec2i mapSize, GameContext gameContext, ParticleContext particleContext) : this()
        {
            TileMap = new PlanetTileMap.TileMap(mapSize);
            AgentList = new AgentList();
            VehicleList = new VehicleList();
            ProjectileList = new ProjectileList();
            FloatingTextList = new FloatingTextList();
            ParticleEmitterList = new ParticleEmitterList();

            GameContext = gameContext;
            ParticleContext = particleContext;
        }


        public AgentEntity AddPlayer(int spriteId, int width, int height, Vec2f position, int startingAnimation)
        {
            ref AgentEntity newEntity = ref AgentList.Add();
            GameEntity entity = GameState.AgentSpawnerSystem.SpawnPlayer(spriteId, width, height, position, newEntity.AgentId,
                    startingAnimation);
            newEntity.Entity = entity;

            return newEntity;
        }

        public AgentEntity AddAgent(int spriteId, int width,
                     int height, Vec2f position, int startingAnimation)
        {
            ref AgentEntity newEntity = ref AgentList.Add();
            GameEntity entity = GameState.AgentSpawnerSystem.SpawnAgent(spriteId, width, height, position,
                                                                    newEntity.AgentId, startingAnimation);
            newEntity.Entity = entity;


            return newEntity;
        }

        public AgentEntity AddEnemy(int spriteId, int width, int height, Vec2f position, int startingAnimation)
        {
            ref AgentEntity newEntity = ref AgentList.Add();
            GameEntity entity = GameState.AgentSpawnerSystem.SpawnEnemy(spriteId, width, height, position,
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

        public ParticleEmitterEntity AddParticleEmitter(UnityEngine.Material material, 
                                    Vec2f position, Vec2f size, int spriteId)
        {
            ref ParticleEmitterEntity newEntity = ref ParticleEmitterList.Add();
            ParticleEntity entity = GameState.ParticleEmitterSpawnerSystem.Spawn(ParticleContext, material, position, 
                        size, spriteId, newEntity.ParticleEmitterId);
            newEntity.Entity = entity;


            return newEntity;
        }

        public void RemoveParticleEmitter()
        {
            ref ParticleEmitterEntity entity = ref ParticleEmitterList.Get(Index);
            entity.Entity.Destroy();
            ParticleEmitterList.Remove(entity);
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
            GameState.PhysicsProcessCollisionSystem.Update(ref TileMap);
            GameState.EnemyAiSystem.Update(this);
            GameState.FloatingTextUpdateSystem.Update(this, frameTime);
            GameState.AnimationUpdateSystem.Update(frameTime);
            GameState.ItemPickUpSystem.Update();
            GameState.ActionSchedulerSystem.Update(frameTime, ref this);
            GameState.ParticleEmitterUpdateSystem.Update(ParticleContext);
            GameState.ParticleUpdateSystem.Update(this, ParticleContext);
            GameState.ProjectileMovementSystem.Update();
            GameState.ProjectileCollisionSystem.Update(ref TileMap);

            TileMap.DrawLayer(MapLayerType.Mid, Object.Instantiate(material), transform, 9);
            TileMap.DrawLayer(MapLayerType.Front, Object.Instantiate(material), transform, 10);
            GameState.AgentDrawSystem.Draw(Object.Instantiate(material), transform, 12);
            GameState.ItemDrawSystem.Draw(GameContext, Material.Instantiate(material), transform, 13);
            GameState.ProjectileDrawSystem.Draw(Material.Instantiate(material), transform, 20);
            GameState.FloatingTextDrawSystem.Draw(transform, 10000);
            GameState.ParticleDrawSystem.Draw(ParticleContext, Material.Instantiate(material), transform, 50);
            #region Gui drawing systems
            //GameState.InventoryDrawSystem.Draw(material, transform, 1000);
            #endregion
        }
    }
}
