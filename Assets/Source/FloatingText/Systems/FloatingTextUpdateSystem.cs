using System.Collections.Generic;
using UnityEngine;
using KMath;

namespace FloatingText
{
    public class FloatingTextUpdateSystem
    {
        List<GameEntity> ToRemoveEntities = new List<GameEntity>();

        public void Update(Planet.PlanetState planetState, float deltaTime)
        {
            var entities = planetState.GameContext.GetGroup(GameMatcher.AllOf(GameMatcher.FloatingTextState));

            foreach (var entity in entities)
            {
                var state = entity.floatingTextState;

                // reduce the time to live of the floating text
                float NewTimeToLive = state.TimeToLive - deltaTime;
                entity.ReplaceFloatingTextState(NewTimeToLive, state.Text);

                // maybe move this to its own system for movement
                if (entity.hasFloatingTextMovable)
                {
                    var movable = entity.floatingTextMovable;

                    Vec2f newPosition = new Vec2f(movable.Position.X + movable.Velocity.X,
                                                    movable.Position.Y + movable.Velocity.Y);
                    entity.ReplaceFloatingTextMovable(movable.Velocity, newPosition);
                }

                // if time to live hits zero we remove the entity
                if (NewTimeToLive <= 0.0f)
                {
                    ToRemoveEntities.Add(entity);
                }
            }

            foreach(var entity in ToRemoveEntities)
            {
                planetState.RemoveFloatingText(entity.floatingTextID.Index);
            }
            ToRemoveEntities.Clear();
        }
    }
}

