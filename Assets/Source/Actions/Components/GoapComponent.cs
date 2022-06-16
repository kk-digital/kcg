using Entitas;

namespace Action
{
    public struct GoapComponent: IComponent
    {
        public AI.GoapState PreConditions;
        public AI.GoapState Effects;

        public int Cost;
    }
}
