using System.Collections.Generic;
using UnityEngine;
using KMath;

namespace FloatingText
{
    public class FloatingTextDrawSystem
    {
        public void Draw(Transform transform, int drawOrder)
        {
            var entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.FloatingTextState));

            foreach (var entity in entities)
            {
                var movable = entity.floatingTextMovable;
                var state = entity.floatingTextState;


                Utility.Render.DrawString(movable.Position.X, movable.Position.Y,
                     0.35f, state.Text, 18, new Color(255, 0, 0, 255),
                    transform, drawOrder);

            }
        }

        public void DrawEx(Transform transform, int drawOrder)
        {
            var entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.FloatingTextState));

            foreach (var entity in entities)
            {
                var movable = entity.floatingTextMovable;
                var state = entity.floatingTextState;
                var sprite = entity.floatingTextSprite;

                Utility.Render.DrawStringEx(sprite.GameObject, movable.Position.X, movable.Position.Y,
                     0.35f, state.Text, 18, new Color(255, 0, 0, 255),
                    transform, drawOrder);

            }
        }
    }
}

