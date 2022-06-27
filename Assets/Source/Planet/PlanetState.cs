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


        // Note(Mahdi): Deprecated will be removed soon
        public AgentEntity AddPlayer(int spriteId,
                                int width, int height, Vec2f position, int startingAnimation)
        {
            if (AgentList.Size >= PlanetEntityLimits.AgentLimit)
            {
                return new AgentEntity();
            }

            ref AgentEntity newEntity = ref AgentList.Add();
            GameEntity entity = GameState.AgentSpawnerSystem.SpawnPlayer(spriteId, width, height, position, newEntity.AgentId,
                    startingAnimation);
            newEntity.Entity = entity;

            return newEntity;
        }

        public AgentEntity AddPlayer(Vec2f position)
        {
            if (AgentList.Size >= PlanetEntityLimits.AgentLimit)
            {
                return new AgentEntity();
            }

            ref AgentEntity newEntity = ref AgentList.Add();
            GameEntity entity = GameState.AgentSpawnerSystem.Spawn(position,
                    newEntity.AgentId,
                    Agent.AgentType.Player);
            newEntity.Entity = entity;

            return newEntity;
        }

        // Note(Mahdi): Deprecated will be removed soon
        public AgentEntity AddAgent(int spriteId, int width,
                     int height, Vec2f position, int startingAnimation)
        {
            if (AgentList.Size >= PlanetEntityLimits.AgentLimit)
            {
                return new AgentEntity();
            }

            ref AgentEntity newEntity = ref AgentList.Add();
            GameEntity entity = GameState.AgentSpawnerSystem.SpawnAgent(spriteId, width, height, position,
                                                                    newEntity.AgentId, startingAnimation);
            newEntity.Entity = entity;


            return newEntity;
        }

        public AgentEntity AddAgent(Vec2f position)
        {
            if (AgentList.Size >= PlanetEntityLimits.AgentLimit)
            {
                return new AgentEntity();
            }

            ref AgentEntity newEntity = ref AgentList.Add();
            GameEntity entity = GameState.AgentSpawnerSystem.Spawn(position,
                    newEntity.AgentId,
                    Agent.AgentType.Agent);
            newEntity.Entity = entity;

            return newEntity;
        }

        // Note(Mahdi): Deprecated will be removed soon
        public AgentEntity AddEnemy(int spriteId,
                        int width, int height, Vec2f position, int startingAnimation)
        {
            if (AgentList.Size >= PlanetEntityLimits.AgentLimit)
            {
                return new AgentEntity();
            }

            ref AgentEntity newEntity = ref AgentList.Add();
            GameEntity entity = GameState.AgentSpawnerSystem.SpawnEnemy(spriteId, width, height, position,
            newEntity.AgentId, startingAnimation);

            newEntity.Entity = entity;


            return newEntity;
        }

        public AgentEntity AddEnemy(Vec2f position)
        {
            if (AgentList.Size >= PlanetEntityLimits.AgentLimit)
            {
                return new AgentEntity();
            }
            
            ref AgentEntity newEntity = ref AgentList.Add();
            GameEntity entity = GameState.AgentSpawnerSystem.Spawn(position,
                    newEntity.AgentId,
                    Agent.AgentType.Enemy);
            newEntity.Entity = entity;

            return newEntity;
        }

        public void RemoveAgent(int Index)
        {
            ref AgentEntity entity = ref AgentList.Get(Index);
            if (entity.Entity.hasAgentSprite2D)
            {
                GameObject.Destroy(entity.Entity.agentSprite2D.GameObject);
            }
            entity.Entity.Destroy();
            AgentList.Remove(entity.AgentId);
        }

        public FloatingTextEntity AddFloatingText(string text, float timeToLive, Vec2f velocity, Vec2f position)
        {
            ref FloatingTextEntity newEntity = ref FloatingTextList.Add();
            GameEntity entity = GameState.FloatingTextSpawnerSystem.SpawnFloatingText(GameContext, text, timeToLive, velocity, position,
                         newEntity.FloatingTextId);

            newEntity.Entity = entity;

            return newEntity;
        }

        public void RemoveFloatingText(int floatingTextId)
        {
            ref FloatingTextEntity entity = ref FloatingTextList.Get(floatingTextId);
            if (entity.Entity.hasFloatingTextSprite)
            {
                GameObject.Destroy(entity.Entity.floatingTextSprite.GameObject);
            }
            entity.Entity.Destroy();
            FloatingTextList.Remove(entity);
        }

        public ParticleEmitterEntity AddParticleEmitter(Vec2f position, Particle.ParticleEmitterType type)
        {
            ref ParticleEmitterEntity newEntity = ref ParticleEmitterList.Add();
            ParticleEntity entity = GameState.ParticleEmitterSpawnerSystem.Spawn(ParticleContext, type, position, 
                        newEntity.ParticleEmitterId);
            newEntity.Entity = entity;


            return newEntity;
        }

        public void RemoveParticleEmitter(int particleEmitterId)
        {
            ref ParticleEmitterEntity entity = ref ParticleEmitterList.Get(particleEmitterId);
            entity.Entity.Destroy();
            ParticleEmitterList.Remove(entity);
        }

        public ProjectileEntity AddProjectile(int spriteID, int width, int height, Vec2f startPos,
            Vec2f velocity, Vec2f acceleration, Enums.ProjectileType projectileType, 
            Enums.ProjectileDrawType projectileDrawType)
        {
            ref ProjectileEntity newEntity = ref ProjectileList.Add();
            GameEntity entity = GameState.ProjectileSpawnerSystem.SpawnBullet(spriteID, width, height, startPos,
                            velocity, acceleration, projectileType, projectileDrawType, newEntity.ProjectileId);
            newEntity.Entity = entity;

            return newEntity;
        }

        public void RemoveProjectile(int projectileId)
        {
            ref ProjectileEntity entity = ref ProjectileList.Get(projectileId);
            if (entity.Entity.hasProjectileSprite2D)
            {
                GameObject.Destroy(entity.Entity.projectileSprite2D.GameObject);
            }
            entity.Entity.Destroy();
            ProjectileList.Remove(entity.ProjectileId);
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
            GameState.ParticleEmitterUpdateSystem.Update(this);
            GameState.ParticleUpdateSystem.Update(this, ParticleContext);
            GameState.ProjectileMovementSystem.Update();
            GameState.ProjectileCollisionSystem.Update(ref this);

            TileMap.DrawLayer(MapLayerType.Mid, Object.Instantiate(material), transform, 9);
            TileMap.DrawLayer(MapLayerType.Front, Object.Instantiate(material), transform, 10);
            GameState.AgentDrawSystem.Draw(Object.Instantiate(material), transform, 12);
            GameState.ItemDrawSystem.Draw(Contexts.sharedInstance, Material.Instantiate(material), transform, 13);
            GameState.ProjectileDrawSystem.Draw(Material.Instantiate(material), transform, 20);
            GameState.FloatingTextDrawSystem.Draw(transform, 10000);
            GameState.ParticleDrawSystem.Draw(ParticleContext, Material.Instantiate(material), transform, 50);
            #region Gui drawing systems
            //GameState.InventoryDrawSystem.Draw(material, transform, 1000);
            #endregion
        }

        // updates the entities, must call the systems and so on ..
        public void UpdateNew(float deltaTime, Material material, Transform transform)
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
                           // var position = projectile.Entity.projectilePhysicsState2D;
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
            GameState.ParticleEmitterUpdateSystem.Update(this);
            GameState.ParticleUpdateSystem.Update(this, ParticleContext);
            GameState.ProjectileMovementSystem.Update();
            GameState.ProjectileCollisionSystem.Update(ref this);

            TileMap.DrawLayerEx(MapLayerType.Mid, Object.Instantiate(material), transform, 9);
            TileMap.DrawLayerEx(MapLayerType.Front, Object.Instantiate(material), transform, 10);
            GameState.AgentDrawSystem.DrawEx(Object.Instantiate(material), transform, 12);
            GameState.ItemDrawSystem.DrawEx(Contexts.sharedInstance, Material.Instantiate(material), 13);
            GameState.ProjectileDrawSystem.DrawEx(Material.Instantiate(material), 20);
            GameState.FloatingTextDrawSystem.DrawEx(transform, 10000);
            GameState.ParticleDrawSystem.DrawEx(ParticleContext, Material.Instantiate(material), 50);
            #region Gui drawing systems
            //GameState.InventoryDrawSystem.Draw(material, transform, 1000);
            #endregion
        }
    }
}
