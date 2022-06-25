using Entitas;
using System.Windows.Forms;

namespace Action
{ 
    public class ActionBase
    {
        protected ActionEntity ActionEntity;
        protected ActionPropertiesEntity ActionPropertyEntity;
        protected GameEntity AgentEntity;

        public ActionBase(int actionID, int agentID)
        {   
            ActionEntity = Contexts.sharedInstance.action.GetEntityWithActionIDID(actionID);
            ActionPropertyEntity = Contexts.sharedInstance.actionProperties.GetEntityWithActionProperty(
                ActionEntity.actionID.TypeID);
            AgentEntity = Contexts.sharedInstance.game.GetEntityWithAgentID(agentID);    
        }

        public virtual void OnEnter(ref Planet.PlanetState planet)
        {
            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
        }

        public virtual void OnUpdate(float deltaTime, ref Planet.PlanetState planet)
        {      
        }

        /// <summary>
        /// We should alweys delete actions after executed.
        /// </summary>
        public virtual void OnExit(ref Planet.PlanetState planet)
        {
            ActionEntity.Destroy();
        }

        public virtual void CheckProceduralPrecondition(ref Planet.PlanetState planet)
        {

        }

        public virtual void ProceduralEffects(ref Planet.PlanetState planet)
        { 
        }
    }

    // Factory Method
    public class ActionCreator
    {
        public virtual ActionBase CreateAction(int actionID, int agentID)
        { 
            return new ActionBase(actionID, agentID);
        }
    }
}
