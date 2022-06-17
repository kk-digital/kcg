using Entitas;
using System.Windows.Forms;

namespace Action
{ 
    public class ActionBase
    {
        protected GameEntity ActionEntity;
        protected GameEntity AgentEntity;

        public ActionBase(int actionID, int agentID)
{   
            ActionEntity = Contexts.sharedInstance.game.GetEntityWithAgentID(agentID);
            AgentEntity = Contexts.sharedInstance.game.GetEntityWithActionID(actionID);    
        }

        public virtual void OnEnter()
        {  
        }

        public virtual void OnUpdate(float deltaTime)
        {      
        }

        public virtual void OnExit()
        {
            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.None);
            ActionEntity.Destroy();
        }

        public virtual void CheckProceduralPrecondition(Planet.PlanetState planetState)
        {
        }

        public virtual void ProceduralEffects()
        { 
        }
    }
}
