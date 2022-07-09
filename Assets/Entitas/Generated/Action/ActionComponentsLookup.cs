//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentLookupGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public static class ActionComponentsLookup {

    public const int ActionExecution = 0;
    public const int ActionID = 1;
    public const int ActionInterrupt = 2;
    public const int ActionOwner = 3;
    public const int ActionTime = 4;
    public const int ActionTool = 5;

    public const int TotalComponents = 6;

    public static readonly string[] componentNames = {
        "ActionExecution",
        "ActionID",
        "ActionInterrupt",
        "ActionOwner",
        "ActionTime",
        "ActionTool"
    };

    public static readonly System.Type[] componentTypes = {
        typeof(Action.ExecutionComponent),
        typeof(Action.IDComponent),
        typeof(Action.InterruptComponent),
        typeof(Action.OwnerComponent),
        typeof(Action.TimeComponent),
        typeof(Action.ToolComponent)
    };
}
