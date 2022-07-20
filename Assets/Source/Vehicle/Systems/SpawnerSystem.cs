using UnityEngine;
using System.Collections.Generic;
using Entitas;
using KMath;

namespace Vehicle
{
    public class SpawnerSystem
    {
        private static int vehicleID;

        public Entity SpawnVehicle(Material material, int spriteID, int width, int height, Vec2f position)
        {
            // Create Entity
            var entity = Contexts.sharedInstance.vehicle.CreateEntity();

            // Increase ID per object statically
            vehicleID++;

            // Set Png Size
            var pngSize = new Vec2i(width, height);

            // Set Sprite ID from Sprite Atlas
            var spriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteID, 0, 0, Enums.AtlasType.Agent);

            // Set Sprite Size
            var spriteSize = new Vec2f(pngSize.X / 32f, pngSize.Y / 32f);

            // Add ID Component
            entity.AddVehicleID(vehicleID);

            // Add Sprite Component
            entity.AddVehicleSprite2D(spriteId, spriteSize);

            // Add Physics State 2D Component
            entity.AddVehiclePhysicsState2D(position, position, Vec2f.One, Vec2f.One, Vec2f.Zero, 1.0f, 1.0f, 1.5f,
                Vec2f.Zero);

            // Add Physics Box Collider Component
            entity.AddPhysicsBox2DCollider(spriteSize, Vec2f.Zero);

            // Return projectile entity
            return entity;
        }
    }
}
