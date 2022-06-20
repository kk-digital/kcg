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
            ActionEntity = Contexts.sharedInstance.game.GetEntityWithActionID(actionID);
            AgentEntity = Contexts.sharedInstance.game.GetEntityWithAgentID(agentID);    
        }

        public virtual void OnEnter()
        {
            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
        }

        public virtual void OnUpdate(float deltaTime)
        {      
        }


        /// <summary>
        /// We should alweys delete action after executed.
        /// </summary>
        public virtual void OnExit()
        {
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
