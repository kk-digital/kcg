namespace Action
{ 
    public class DefaultActions
    {
        // Should be called before any other action is created.
        public DefaultActions() 
        {
            // Pick Up action.
            GameState.ActionManager.CreateAction();
            GameState.ActionManager.SetExecution(new PickUpAction());
            GameState.ActionManager.EndAction();

            // Drop Action.
            GameState.ActionManager.CreateAction();
            GameState.ActionManager.SetExecution(new DropAction());
            GameState.ActionManager.EndAction();
        }


    }
}
