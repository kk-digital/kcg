using Entitas;

namespace Action
{
    [Action]
    public struct ExecutionComponent: IComponent
    {
        public ActionBase           Logic;
        public Enums.ActionState    State;
    }   
}
