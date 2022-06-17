using System.Collections.Generic;
using UnityEngine;

namespace Vehicle
{
    public class DrawSystem
    {
        List<int> triangles = new();
        List<Vector2> uvs = new();
        List<Vector3> verticies = new();

        public void Draw(Material material, Transform transform, int drawOrder)
        {
            var VehiclesWithSprite = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.VehicleSprite2D));
            foreach (var entity in VehiclesWithSprite)
            {
                var sprite = new Sprites.Sprite
                {
                    Texture = entity.vehicleSprite2D.Texture,
                    TextureCoords = new Vector4(0, 0, 1, 1)
                };

                var x = entity.vehiclePhysicsState2D.Position.x;
                var y = entity.vehiclePhysicsState2D.Position.y;
                var width = entity.vehicleSprite2D.Size.x;
                var height = entity.vehicleSprite2D.Size.y;

                Utility.Render.DrawSprite(x, y, width, height, sprite, material, transform, drawOrder++);
            }
        }
    }
}

