using Entitas;
using System.Windows.Forms;

namespace Action
{ 
    public class ActionBase
    {
        protected Contexts EntitasContext;
        protected ActionEntity ActionEntity;
        protected ActionPropertiesEntity ActionPropertyEntity;
        protected AgentEntity AgentEntity;

        public ActionBase(Contexts entitasContext, int actionID)
        {   
            EntitasContext = entitasContext;
            ActionEntity = entitasContext.action.GetEntityWithActionIDID(actionID);
            ActionPropertyEntity = entitasContext.actionProperties.GetEntityWithActionProperty(
                ActionEntity.actionID.TypeID);
            AgentEntity = entitasContext.agent.GetEntityWithAgentID(ActionEntity.actionOwner.AgentID);    
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
        public virtual ActionBase CreateAction(Contexts entitasContext, int actionID)
        { 
            return new ActionBase(entitasContext, actionID);
        }
    }
}
