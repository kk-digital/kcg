using Entitas;

public sealed class InputComponent : IComponent
{
    public bool accelerate;
    public bool decelerate;
    public bool turnLeft;
    public bool turnRight;
    public bool strafeLeft;
    public bool strafeRight;
    public bool fire;

}
