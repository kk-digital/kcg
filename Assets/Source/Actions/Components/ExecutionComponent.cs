using Entitas;

namespace Action
{
    [Action]
    public class ExecutionComponent: IComponent
    {
        public ActionBase           Logic;
        public Enums.ActionState    State;
    }   
}
