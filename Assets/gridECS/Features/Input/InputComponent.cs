using Entitas;
using Entitas.CodeGenerator;

[SingleEntity]
public class InputComponent : IComponent {
    public InputIntent intent;
    public object[] data;
}
