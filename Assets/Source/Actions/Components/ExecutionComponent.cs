using Entitas;

namespace Action
{
    public struct ExecutionComponent: IComponent
    {
        public ActionBase           Logic;
        public Enums.ActionState    State;

    }   
}
