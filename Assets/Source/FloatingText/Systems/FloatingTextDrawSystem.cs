using System.Collections.Generic;
using UnityEngine;
using KMath;

namespace FloatingText
{
    public class FloatingTextDrawSystem
    {
        public void Draw(GameContext gameContext, Transform transform, int drawOrder)
        {
            var entities = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.FloatingTextState));

            foreach (var entity in entities)
            {
                var movable = entity.floatingTextMovable;
                var state = entity.floatingTextState;
                var sprite = entity.floatingTextSprite;

                Utility.Render.DrawString(sprite.GameObject, movable.Position.X, movable.Position.Y,
                     0.35f, state.Text, 18, new Color(255, 0, 0, 255), drawOrder);
            }
        }
    }
}

