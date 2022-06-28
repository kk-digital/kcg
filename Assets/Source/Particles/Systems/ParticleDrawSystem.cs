using System.Collections.Generic;
using UnityEngine;

namespace Particle
{
    public class ParticleDrawSystem
    {
        
        public void Draw(ParticleContext context, Material material, Transform transform, int drawOrder)
        {
            var entities = context.GetGroup(ParticleMatcher.AllOf(ParticleMatcher.ParticleSprite2D));

            foreach (var entity in entities)
            {
                int spriteId = entity.particleSprite2D.SpriteId;

                /*if (entity.hasAnimationState)
                {
                    var animation = entity.animationState;
                    spriteId = animation.State.GetSpriteId();
                }*/

                Sprites.Sprite sprite = GameState.SpriteAtlasManager.GetSprite(spriteId, Enums.AtlasType.Particle);

                var pos = entity.particlePosition2D.Position;

                var x = pos.x;
                var y = pos.y;
                var width = entity.particleSprite2D.Size.X;
                var height = entity.particleSprite2D.Size.Y;

                Utility.Render.DrawSprite(x, y, width, height, sprite, Material.Instantiate(material), transform, drawOrder++, entity.particlePosition2D.Rotation);
            }
        }
    }
}

