//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiInterfaceGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial interface IPhysicsMovableEntity {

    Physics.MovableComponent physicsMovable { get; }
    bool hasPhysicsMovable { get; }

    void AddPhysicsMovable(float newSpeed, KMath.Vec2f newVelocity, KMath.Vec2f newAcceleration, bool newLanded);
    void ReplacePhysicsMovable(float newSpeed, KMath.Vec2f newVelocity, KMath.Vec2f newAcceleration, bool newLanded);
    void RemovePhysicsMovable();
}
