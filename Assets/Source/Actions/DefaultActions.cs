namespace Action
{ 
    public struct DefaultActions
    {
        static public int CreatePickUpAction(int agentID, int itemID)
        {
            // Pick Up action.
            GameState.ActionManager.CreateAction();
            int actionID = GameState.ActionManager.GetCurrentID();
            GameState.ActionManager.SetExecution(new PickUpAction(actionID, agentID, itemID));
            GameState.ActionManager.EndAction();
            return actionID;
        }

        static public int CreateDropAction(int agentID)
        {
            // Drop Action.
            GameState.ActionManager.CreateAction();
            int actionID = GameState.ActionManager.GetCurrentID();
            GameState.ActionManager.SetExecution(new DropAction(actionID, agentID));
            GameState.ActionManager.EndAction();
            return actionID;
        }
    }
}
