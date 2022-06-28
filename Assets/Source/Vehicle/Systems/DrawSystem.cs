using System.Collections.Generic;
using UnityEngine;

namespace Vehicle
{
    public class DrawSystem
    {
        public void Draw(Material material, Transform transform, int drawOrder)
        {
            var VehiclesWithSprite = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.VehicleSprite2D));
            foreach (var entity in VehiclesWithSprite)
            {
                Sprites.Sprite sprite = GameState.SpriteAtlasManager.GetSprite(entity.vehicleSprite2D.SpriteId, Enums.AtlasType.Particle);


                var x = entity.vehiclePhysicsState2D.Position.X;
                var y = entity.vehiclePhysicsState2D.Position.Y;
                var width = entity.vehicleSprite2D.Size.X;
                var height = entity.vehicleSprite2D.Size.Y;

                Utility.Render.DrawSprite(x, y, width, height, sprite, material, transform, drawOrder++);
            }
        }
    }
}

