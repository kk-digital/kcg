using UnityEngine;
using System.Collections.Generic;
using Entitas;
using Enums;
using Enums.Tile;
using KMath;

namespace Projectile
{
    public class SpawnerSystem
    {
        // Projectile ID
        private static int projectileID;

        public Entity SpawnProjectile(int spriteID, int witdh, int height, Vec2f startPos,
            ProjectileType projectileType, ProjectileDrawType projectileDrawType)
        {
            // Create Entity
            var entity = Contexts.sharedInstance.game.CreateEntity();

            // Increase ID per object statically
            projectileID++;

            // Set Png Size
            var pngSize = new Vector2Int(witdh, height);

            // Set Sprite ID from Sprite Atlas
            var spriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteID, 0, 0, Enums.AtlasType.Agent);

            // Set Sprite Size
            var spriteSize = new Vec2f(pngSize.x / 32f, pngSize.y / 32f);

            // Add ID Component
            entity.AddProjectileID(projectileID);

            // Add Sprite Component
            entity.AddProjectileSprite2D(spriteId, spriteSize);

            // Add Physics State 2D Component
            entity.AddProjectilePhysicsState2D(startPos, startPos, Vec2f.Zero, 1.0f, 1.0f, 0.5f,
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
