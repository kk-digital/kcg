 using Agent;
using Enums.Tile;
using Vehicle;
using Projectile;
using FloatingText;
using Particle;
using Enums;
using Item;
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
        public ParticleList ParticleList;
        public ItemParticleList ItemParticleList;

        public Contexts EntitasContext;

        public void Init(Vec2i mapSize)
        {
            TileMap = new PlanetTileMap.TileMap(mapSize);
            AgentList = new AgentList();
            VehicleList = new VehicleList();
            ProjectileList = new ProjectileList();
            FloatingTextList = new FloatingTextList();
            ParticleEmitterList = new ParticleEmitterList();
            ParticleList = new ParticleList();
            ItemParticleList = new ItemParticleList();

            EntitasContext = new Contexts();
        }

        public void InitializeSystems(Material material, Transform transform)
        {
            GameState.ActionInitializeSystem.Initialize(EntitasContext, material);

            // Mesh builders
            GameState.TileMapRenderer.Initialize(material, transform, 7);
            GameState.ItemMeshBuilderSystem.Initialize(material, transform, 11);
            GameState.AgentMeshBuilderSystem.Initialize(material, transform, 12);
            GameState.ProjectileMeshBuilderSystem.Initialize(material, transform, 13);
            GameState.ParticleMeshBuilderSystem.Initialize(material, transform, 20);
        }


        // Note(Mahdi): Deprecated will be removed soon
        public AgentEntity AddPlayer(int spriteId, int width, int height, Vec2f position, int startingAnimation, 
            int health, int food, int water, int oxygen, int fuel)
        {
            Utils.Assert(AgentList.Size < PlanetEntityLimits.AgentLimit);

            AgentEntity newEntity = AgentList.Add(GameState.AgentSpawnerSystem.SpawnPlayer(EntitasContext, spriteId, 
                width, height, position, -1, startingAnimation, health, food, water, oxygen, fuel, 0.2f));
            return newEntity;
        }

        public AgentEntity AddPlayer(Vec2f position)
        {
            Utils.Assert(AgentList.Size < PlanetEntityLimits.AgentLimit);

            AgentEntity newEntity = AgentList.Add(GameState.AgentSpawnerSystem.Spawn(EntitasContext, position,
                    -1, Agent.AgentType.Player));
            return newEntity;
        }

        // Note(Mahdi): Deprecated will be removed soon
        public AgentEntity AddAgent(int spriteId, int width,
                     int height, Vec2f position, int startingAnimation)
        {
            Utils.Assert(AgentList.Size < PlanetEntityLimits.AgentLimit);

            AgentEntity newEntity = AgentList.Add(GameState.AgentSpawnerSystem.SpawnAgent(EntitasContext, 
                spriteId, width, height, position, -1, startingAnimation));
            return newEntity;
        }

        public AgentEntity AddAgent(Vec2f position)
        {
            Utils.Assert(AgentList.Size < PlanetEntityLimits.AgentLimit);

            AgentEntity newEntity = AgentList.Add(GameState.AgentSpawnerSystem.Spawn(EntitasContext, position,
                    -1, Agent.AgentType.Agent));
            return newEntity;
        }

        // Note(Mahdi): Deprecated will be removed soon
        public AgentEntity AddEnemy(int spriteId,
                        int width, int height, Vec2f position, int startingAnimation)
        {
            Utils.Assert(AgentList.Size < PlanetEntityLimits.AgentLimit);

            AgentEntity newEntity = AgentList.Add(GameState.AgentSpawnerSystem.SpawnEnemy(spriteId, width, height, 
                position, -1, startingAnimation));
            return newEntity;
        }

        public AgentEntity AddEnemy(Vec2f position)
        {
            Utils.Assert(AgentList.Size < PlanetEntityLimits.AgentLimit);

            AgentEntity newEntity = AgentList.Add(GameState.AgentSpawnerSystem.Spawn(EntitasContext, position,
                    -1, Agent.AgentType.Enemy));
            return newEntity;
        }

        public void RemoveAgent(int agentId)
        {
            AgentEntity entity = AgentList.Get(agentId);
            Utils.Assert(entity.isEnabled);
            AgentList.Remove(agentId);
        }

        public FloatingTextEntity AddFloatingText(string text, float timeToLive, Vec2f velocity, Vec2f position)
        {
            FloatingTextEntity newEntity = FloatingTextList.Add(GameState.FloatingTextSpawnerSystem.SpawnFloatingText
                (EntitasContext.floatingText, text, timeToLive, velocity, position, -1));
            return newEntity;
        }

        public void RemoveFloatingText(int floatingTextId)
        {
            FloatingTextEntity entity = FloatingTextList.Get(floatingTextId);
            Utils.Assert(entity.isEnabled);
            GameObject.Destroy(entity.floatingTextSprite.GameObject);
            FloatingTextList.Remove(floatingTextId);
        }

        public ParticleEntity AddParticleEmitter(Vec2f position, Particle.ParticleEmitterType type)
        {
            ParticleEntity newEntity = ParticleEmitterList.Add(GameState.ParticleEmitterSpawnerSystem.Spawn(
                EntitasContext.particle, type, position, -1));
            return newEntity;
        }

        public void RemoveParticleEmitter(int particleEmitterId)
        {
            ParticleEntity entity = ParticleEmitterList.Get(particleEmitterId);
            Utils.Assert(entity.isEnabled);
            ParticleEmitterList.Remove(entity.particleEmitterID.ParticleEmitterId);
        }


        public ParticleEntity AddParticle(Vec2f position, Vec2f velocity, Particle.ParticleType type)
        {
            Utils.Assert(ParticleList.Size < PlanetEntityLimits.ParticleLimit);

            ParticleEntity newEntity = ParticleList.Add(GameState.ParticleSpawnerSystem.Spawn(
                EntitasContext.particle, type, position, velocity, -1));
            return newEntity;
        }

        public void RemoveParticle(int particleId)
        {
            ParticleList.Remove(particleId);
        }

        public ProjectileEntity AddProjectile(Vec2f position, Vec2f direction, Enums.ProjectileType projectileType)
        {
            Utils.Assert(ProjectileList.Size < PlanetEntityLimits.ProjectileLimit);
            ProjectileEntity newEntity = ProjectileList.Add(GameState.ProjectileSpawnerSystem.Spawn(EntitasContext.projectile,
                         position, direction, projectileType, -1));
            return newEntity;
        }

        public void RemoveProjectile(int projectileId)
        {
            ProjectileEntity entity = ProjectileList.Get(projectileId);
            Utils.Assert(entity.isEnabled);
            ProjectileList.Remove(entity.projectileID.ID);
        }

        public VehicleEntity AddVehicle(UnityEngine.Material material, Vector2 position)
        {
            Utils.Assert(VehicleList.Size < PlanetEntityLimits.VehicleLimit);

            VehicleEntity newEntity = VehicleList.Add(new VehicleEntity());
            return newEntity;
        }

        public void RemoveVehicle(int vehicleId)
        {
            VehicleList.Remove(vehicleId);
        }

        public ItemParticleEntity AddItemParticle(Vec2f position, ItemType itemType)
        {
            Utils.Assert(ItemParticleList.Size < PlanetEntityLimits.ItemParticlesLimit);

            ItemParticleEntity newEntity = ItemParticleList.Add(GameState.ItemSpawnSystem.SpawnItemParticle(EntitasContext, itemType, position));
            return newEntity;
        }

        public void RemoveItemParticle(int itemParticleId)
        {
            ItemParticleList.Remove(itemParticleId);

        }



        // updates the entities, must call the systems and so on ..
        public void Update(float deltaTime, Material material, Transform transform)
        {
            float targetFps = 30.0f;
            float frameTime = 1.0f / targetFps;

            /*TimeState.Deficit += deltaTime;

            while (TimeState.Deficit >= frameTime)
            {
                TimeState.Deficit -= frameTime;
                // do a server/client tick right here
                {
                    TimeState.TickTime++;


                }

            }*/

            // check if the sprite atlas teSetTilextures needs to be updated
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

            GameState.InputProcessSystem.Update(ref this);
            GameState.PhysicsMovableSystem.Update(EntitasContext.agent);
            GameState.PhysicsMovableSystem.Update(EntitasContext.itemParticle);
            GameState.PhysicsProcessCollisionSystem.Update(EntitasContext.agent, ref TileMap);
            GameState.PhysicsProcessCollisionSystem.Update(EntitasContext.itemParticle, ref TileMap);
            GameState.EnemyAiSystem.Update(ref this);
            GameState.FloatingTextUpdateSystem.Update(ref this, frameTime);
            GameState.AnimationUpdateSystem.Update(EntitasContext, frameTime);
            GameState.ItemPickUpSystem.Update(EntitasContext);
            GameState.ActionSchedulerSystem.Update(EntitasContext, frameTime, ref this);
            GameState.ActionCoolDownSystem.Update(EntitasContext, deltaTime);
            GameState.ParticleEmitterUpdateSystem.Update(ref this);
            GameState.ParticleUpdateSystem.Update(ref this, EntitasContext.particle);
            GameState.ProjectileMovementSystem.Update(EntitasContext.projectile);
            GameState.ProjectileCollisionSystem.UpdateEx(ref this);

            TileMap.UpdateTileSprites();
            
            // Update Meshes.
            GameState.TileMapRenderer.UpdateBackLayerMesh(ref TileMap);
            GameState.TileMapRenderer.UpdateMidLayerMesh(ref TileMap);
            GameState.TileMapRenderer.UpdateFrontLayerMesh(ref TileMap);
            GameState.ItemMeshBuilderSystem.UpdateMesh(EntitasContext);
            GameState.AgentMeshBuilderSystem.UpdateMesh(EntitasContext.agent);
            GameState.ProjectileMeshBuilderSystem.UpdateMesh(EntitasContext.projectile);
            GameState.ParticleMeshBuilderSystem.UpdateMesh(EntitasContext.particle);

            // Draw Frames.
            GameState.TileMapRenderer.DrawLayer(MapLayerType.Back);
            GameState.TileMapRenderer.DrawLayer(MapLayerType.Mid);
            GameState.TileMapRenderer.DrawLayer(MapLayerType.Front);
            Utility.Render.DrawFrame(ref GameState.ItemMeshBuilderSystem.Mesh, GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.Particle));
            Utility.Render.DrawFrame(ref GameState.AgentMeshBuilderSystem.Mesh, GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.Agent));
            Utility.Render.DrawFrame(ref GameState.ProjectileMeshBuilderSystem.Mesh, GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.Particle));
            Utility.Render.DrawFrame(ref GameState.ParticleMeshBuilderSystem.Mesh, GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.Particle));

            GameState.FloatingTextDrawSystem.Draw(EntitasContext.floatingText, transform, 10000);
        }
    }
}
