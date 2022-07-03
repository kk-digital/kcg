using Entitas;

namespace Action.Property
{
    [ActionProperties]
    public class GoapComponent: IComponent
    {
        public AI.GoapState PreConditions;
        public AI.GoapState Effects;

        public int Cost;
    }
}
