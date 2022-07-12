using Action;
using Entitas;
using UnityEngine;
using KMath;
using Enums;

namespace Item
{
    public class PickUpSystem
    {
        // Todo:
        //  Hash entities by their position.
        //  Only call this after an item or an agent has changed position. 
        public void Update(Contexts contexts)
        {
            // Get agents able to pick an object.
            var agents = contexts.agent.GetGroup(
                AgentMatcher.AllOf(AgentMatcher.PhysicsPosition2D).AnyOf(AgentMatcher.AgentInventory, AgentMatcher.AgentToolBar));

            // Get all pickable items.
            var pickableItems = contexts.itemParticle.GetGroup(
                ItemParticleMatcher.AllOf(ItemParticleMatcher.ItemID, ItemParticleMatcher.PhysicsPosition2D).NoneOf(ItemParticleMatcher.ItemUnpickable));

            foreach (var item in pickableItems)
            {
                // Get item ceter position.
                var itemPropreties = GameState.ItemCreationApi.Get(item.itemType.Type);
                Vec2f centerPos = item.physicsPosition2D.Value + itemPropreties.SpriteSize / 2.0f;
                foreach (var agent in agents)
                {
                    // Todo: Use action center Position.
                    if ((agent.physicsPosition2D.Value - centerPos).Magnitude <= 1.25f)
                    {
                        GameState.ActionInitializeSystem.CreatePickUpAction(contexts, agent.agentID.ID, item.itemID.ID);
                    }
                }    
            }
        }
    }
}
