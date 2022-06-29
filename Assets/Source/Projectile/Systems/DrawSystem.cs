using Sprites;
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
                int spriteId = entity.projectileSprite2D.SpriteId;

                if (entity.hasAnimationState)
                {
                    var animation = entity.animationState;
                    spriteId = animation.State.GetSpriteId();
                }

                Sprites.Sprite sprite = GameState.SpriteAtlasManager.GetSprite(spriteId, Enums.AtlasType.Particle);

                var x = entity.projectilePosition2D.Value.X;
                var y = entity.projectilePosition2D.Value.Y; 
                var width = entity.projectileSprite2D.Size.X;
                var height = entity.projectileSprite2D.Size.Y;

                Utility.Render.DrawSprite(x, y, width, height, sprite, material, transform, drawOrder++, entity.projectilePosition2D.Rotation);
            }
        }
    }
}

