using Entitas;
using UnityEngine;

public sealed class UpdateInputPointerSystem : IExecuteSystem
{
    private readonly Contexts _contexts;

    public KeyCode mouseRightClick = KeyCode.Mouse0;

    public UpdateInputPointerSystem(Contexts contexts)
    {
        _contexts = contexts;
       
    }

    public void Execute()
    {
        var Clicked = Input.GetKey(mouseRightClick);

        if (Clicked)
        {
            var e = _contexts.game.CreateEntity();

            e.isPointerClicked = true;
            e.isPointer = true;
            e.AddPointerPosition(Camera.main.ScreenPointToRay(Input.mousePosition).origin);
           
        }

    }
}
