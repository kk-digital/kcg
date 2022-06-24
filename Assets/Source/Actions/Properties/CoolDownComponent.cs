using Entitas;

namespace Action.Property
{
    [ActionProperties]
    public struct CoolDownComponent : IComponent
    {
        public float CoolDownTime;
    }
}
