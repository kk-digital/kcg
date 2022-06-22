using Entitas;

namespace Action.Attribute
{
    public struct GoapComponent: IComponent
    {
        public AI.GoapState PreConditions;
        public AI.GoapState Effects;

        public int Cost;
    }
}
