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
                Sprites.Sprite sprite = GameState.SpriteAtlasManager.GetSprite(entity.projectileSprite2D.SpriteId, Enums.AtlasType.Particle);

                var x = entity.projectilePosition2D.Value.X;
                var y = entity.projectilePosition2D.Value.Y; 
                var width = entity.projectileSprite2D.Size.X;
                var height = entity.projectileSprite2D.Size.Y;

                Utility.Render.DrawSprite(x, y, width, height, sprite, material, transform, drawOrder++);
            }
        }

        public void DrawEx(Material material, int drawOrder)
        {
            var VehiclesWithSprite = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.ProjectileSprite2D));
            foreach (var entity in VehiclesWithSprite)
            {
                Sprites.Sprite sprite = GameState.SpriteAtlasManager.GetSprite(entity.projectileSprite2D.SpriteId, Enums.AtlasType.Particle);

                var x = entity.projectilePosition2D.Value.X;
                var y = entity.projectilePosition2D.Value.Y; 
                var width = entity.projectileSprite2D.Size.X;
                var height = entity.projectileSprite2D.Size.Y;

                Utility.Render.DrawSpriteEx(entity.projectileSprite2D.GameObject, x, y, width, 
                                    height, sprite, material, drawOrder++);
            }
        }
    }
}

