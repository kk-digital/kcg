using Entitas;

namespace Action
{ 
    public class ActionBase
    {
        public virtual void OnEnter(int actionID, int agentID)
        {  
        }

        public virtual void OnUpdate(int actionID, int agentID, float CurrentTime)
        {      
        }

        public virtual void OnExit(int actionID, int agentID)
        {
            GameEntity actionEntity = Contexts.sharedInstance.game.GetEntityWithActionID(actionID);
            actionEntity.ReplaceActionExecution(this, Enums.ActionState.None);
        }

        public virtual void CheckProceduralPrecondition(int actionID, int agentID, Planet.PlanetState planetState)
        {
        }

        public virtual void ProceduralEffects(int actionID, int agentID)
        { 
        }
    }
}
