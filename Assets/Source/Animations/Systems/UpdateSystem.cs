using System.Collections.Generic;
using UnityEngine;
using KMath;

namespace Animation
{
    public class UpdateSystem
    {

        public void Update(float deltaTime)
        {
            var entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.AnimationState));

            foreach (var entity in entities)
            {
                var state = entity.animationState;
                state.State.Update(deltaTime, state.AnimationSpeed);

                entity.ReplaceAnimationState(state.AnimationSpeed, state.State);
            }

        }
    }
}

