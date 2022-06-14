using Entitas;

namespace AI
{ 
    public class ActionBase
    {
        public GameEntity CreateActionEntity()
        {
            return Contexts.sharedInstance.game.CreateEntity();
        }
        public virtual void OnEnter()
        {  
        }

        public virtual void OnUpdate(float deltaTime)
        {      
        }

        public virtual void OnExit()
        { 
        
        }

        public virtual bool CheckProceduralPrecondition()
        {
            return true;
        }

        public virtual void ProceduralEffects()
        { 
        }
    }
}
