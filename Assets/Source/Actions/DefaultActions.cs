using Entitas;

namespace Action
{ 
    public struct DefaultActions
    {

        static public int CreatePickUpAction(int agentID, int itemID)
        {
            // Pick Up action.
            int actionTypeID = GameState.ActionAttributeManager.GetTypeID("PickUp");
            int actionID = GameState.ActionCreationSystem.CreateAction(actionTypeID, agentID);
            GameState.ActionCreationSystem.SetItem(actionID, itemID);
            return actionID;
        }

        static public int CreateDropAction(int agentID)
        {
            // Drop Action.
            int actionTypeID = GameState.ActionAttributeManager.GetTypeID("Drop");
            int actionID = GameState.ActionCreationSystem.CreateAction(actionTypeID, agentID);
            return actionID;
        }

        static public void InitializeActionsAttributes()
        { 
            GameState.ActionAttributeManager.CreateActionAttributeType("PickUp");
            GameState.ActionAttributeManager.SetLogicFactory(new PickUpActionCreator());
            GameState.ActionAttributeManager.EndActionAttributeType();

            GameState.ActionAttributeManager.CreateActionAttributeType("Drop");
            GameState.ActionAttributeManager.SetLogicFactory(new DropActionCreator());
            GameState.ActionAttributeManager.SetTime(2.0f);
            GameState.ActionAttributeManager.EndActionAttributeType();

        }
    }
}
