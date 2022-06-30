using UnityEngine;
using System.Collections.Generic;
using Entitas;
using Enums;
using Enums.Tile;
using KMath;
using Unity.VisualScripting;
using Sprites;

namespace Projectile
{
    public class SpawnerSystem
    {
        // Projectile ID
        private static int projectileID;
        ProjectileCreationApi ProjectileCreationApi;

        public SpawnerSystem(ProjectileCreationApi projectileCreationApi)
        {
            ProjectileCreationApi = projectileCreationApi;
        }

        public GameEntity Spawn(GameContext gameContext, Vec2f position, Vec2f direction, 
                                Enums.ProjectileType projectileType, 
                                int projectileId)
        {
            ProjectileProperties projectileProperties = 
                                    ProjectileCreationApi.GetRef((int)projectileType);

            GameEntity entity = gameContext.CreateEntity();
            // Add ID Component
            entity.AddProjectileID(projectileId);

            // Add Sprite Component
            entity.AddProjectileSprite2D(projectileProperties.SpriteId, projectileProperties.Size);
            // Add Position Component
            entity.AddProjectilePosition2D(position, position, 0.0f);
            // Add Movement Component
            entity.AddProjectileMovable(direction.Normalized * projectileProperties.Speed, 
                                    projectileProperties.Acceleration);
            
            // Add Physics Box Collider Component
            entity.AddPhysicsBox2DCollider(projectileProperties.Size, Vec2f.Zero);

            // Add Physics Collider Component
            entity.AddProjectileCollider(true, true);

            entity.AddProjectilePhysicsState2D(Vec2f.Zero, 1.0f, 1.0f, 0.5f, Vec2f.Zero);

            // Add Projectile Type
            entity.AddProjectileType(projectileType, Enums.ProjectileDrawType.Standard);

            if (projectileProperties.HasAnimation)
            {
                 entity.AddAnimationState(1.0f, new Animation.Animation{Type=(int)projectileProperties.AnimationType});
            }

            return entity;

        }

        public GameEntity SpawnBullet(Contexts entitasContext, int spriteID, int width, int height, Vec2f startPos,
            Vec2f velocity, Vec2f acceleration, ProjectileType projectileType, 
            ProjectileDrawType projectileDrawType)
        {
            GameEntity entity = entitasContext.game.CreateEntity();
            // Increase ID per object statically
            projectileID++;

            // Set Png Size
            var pngSize = new Vector2Int(width, height);
            var spriteSize = new Vec2f(pngSize.x / 32f, pngSize.y / 32f);
            
            // Add ID Component
            entity.AddProjectileID(projectileID);

            // Add Sprite Component
            entity.AddProjectileSprite2D(spriteID, spriteSize);

            // Add Position Component
            entity.AddProjectilePosition2D(startPos, startPos, 0.0f);
            // Add Moviment Component
            entity.AddProjectileMovable(velocity, acceleration);

            // Add Physics Box Collider Component
            entity.AddPhysicsBox2DCollider(spriteSize, Vec2f.Zero);

            // Add Physics Collider Component
            entity.AddProjectileCollider(true, true);
            entity.AddProjectilePhysicsState2D(Vec2f.Zero, 1.0f, 1.0f, 0.5f, Vec2f.Zero);

            // Add Projectile Type
            entity.AddProjectileType(projectileType, projectileDrawType);

            return entity;
        }

        public Entity SpawnProjectile(Contexts entitasContext, int spriteID, int width, int height, Vec2f startPos,
            Cell start, Cell end, ProjectileType projectileType, ProjectileDrawType projectileDrawType)
        {
            // Create Entity
            var entity = entitasContext.game.CreateEntity();

            // Increase ID per object statically
            projectileID++;

            // Set Png Size
            var pngSize = new Vector2Int(width, height);

            // Set Sprite ID from Sprite Atlas
            var spriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteID, 0, 0, Enums.AtlasType.Particle);

            // Set Sprite Size
            var spriteSize = new Vec2f(pngSize.x / 32f, pngSize.y / 32f);

            // Add ID Component
            entity.AddProjectileID(projectileID);

            // Add Sprite Component
            entity.AddProjectileSprite2D(spriteId, spriteSize);

            // Add Physics State 2D Component
            entity.AddProjectilePosition2D(startPos, startPos, 0.0f);
            entity.AddProjectilePhysicsState2D(Vec2f.Zero, 1.0f, 1.0f, 0.5f,
                Vec2f.Zero);

            // Add Physics Box Collider Component
            entity.AddPhysicsBox2DCollider(spriteSize, Vec2f.Zero);

            bool isFirstSolid = false;
            // Log Places Shooted Ray Go Through
            foreach (var cell in start.LineTo(end))
            {
                isFirstSolid = true;
            }

#if UNITY_EDITOR
            // Draw Debug Line to see shooted ray
            Debug.DrawLine(new Vector3(start.x, start.y, 0.0f), new Vector3(end.x, end.y), Color.red);
#endif

            // Add Physics Collider Component
            entity.AddProjectileCollider(isFirstSolid, false);

            // Add Projectile Type
            entity.AddProjectileType(projectileType, projectileDrawType);

            // Return projectile entity
            return entity;
        }
    }
}
