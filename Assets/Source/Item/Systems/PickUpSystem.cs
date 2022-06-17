using Action;
using Entitas;
using UnityEngine;

namespace Item
{
    public class PickUpSystem
    {
        Contexts EntitasContext;

        public PickUpSystem(Contexts entitasContext)
        {
            EntitasContext = entitasContext;
        }

        // Todo:
        //  Hash entities by their position.
        //  Only call this after an item or an agent has changed position. 
        public void Update()
        {
            // Get agents able to pick an object.
            var agents = EntitasContext.game.GetGroup(
                GameMatcher.AllOf(GameMatcher.AgentActionScheduler, GameMatcher.PhysicsPosition2D).AnyOf(GameMatcher.AgentInventory, GameMatcher.AgentToolBar));

            // Get all pickable items.
            var pickableItems = EntitasContext.game.GetGroup(
                GameMatcher.AllOf(GameMatcher.ItemID, GameMatcher.PhysicsPosition2D).NoneOf(GameMatcher.ItemDrawPosition2D));


            foreach (var item in pickableItems)
            {
                // Get item ceter position.
                var itemAttribute = EntitasContext.game.GetEntityWithItemAttributes(item.itemID.ItemType);
                Vector2 centerPos = item.physicsPosition2D.Value + itemAttribute.itemAttributeSize.Size / 2.0f;
                foreach (var agent in agents)
                {
                    // Todo: Use action center Position.
                    if ((agent.physicsPosition2D.Value - centerPos).magnitude <= 2.0f)
                    {
                        GameState.ActionSchedulerSystem.ScheduleAction(agent, DefaultActions.CreatePickUpAction(agent.agentID.ID, item.itemID.ID));
                    }
                }    
            }
        }
    }
}
