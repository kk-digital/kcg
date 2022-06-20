using System.Collections.Generic;
using UnityEngine;

namespace Projectile
{
    public class DrawSystem
    {
        List<int> triangles = new();
        List<Vector2> uvs = new();
        List<Vector3> verticies = new();

        public void Draw(Material material, Transform transform, int drawOrder)
        {
            var VehiclesWithSprite = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.ProjectileSprite2D));
            foreach (var entity in VehiclesWithSprite)
            {
                var sprite = new Sprites.Sprite
                {
                    Texture = entity.projectileSprite2D.Texture,
                    TextureCoords = new Vector4(0, 0, 1, 1)
                };

                var x = entity.projectilePhysicsState2D.Position.X;
                var y = entity.projectilePhysicsState2D.Position.Y;
                var width = entity.projectileSprite2D.Size.X;
                var height = entity.projectileSprite2D.Size.Y;

                Utility.Render.DrawSprite(x, y, width, height, sprite, material, transform, drawOrder++);
            }
        }
    }
}

