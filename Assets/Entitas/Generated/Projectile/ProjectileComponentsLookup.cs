//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentLookupGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public static class ProjectileComponentsLookup {

    public const int AnimationState = 0;
    public const int ECSInput = 1;
    public const int PhysicsBox2DCollider = 2;
    public const int PhysicsSphere2DCollider = 3;
    public const int ProjectileCollider = 4;
    public const int ProjectileID = 5;
    public const int ProjectileLinearDrag = 6;
    public const int ProjectileMovable = 7;
    public const int ProjectilePhysicsState2D = 8;
    public const int ProjectilePosition2D = 9;
    public const int ProjectileRamp = 10;
    public const int ProjectileSprite2D = 11;
    public const int ProjectileType = 12;

    public const int TotalComponents = 13;

    public static readonly string[] componentNames = {
        "AnimationState",
        "ECSInput",
        "PhysicsBox2DCollider",
        "PhysicsSphere2DCollider",
        "ProjectileCollider",
        "ProjectileID",
        "ProjectileLinearDrag",
        "ProjectileMovable",
        "ProjectilePhysicsState2D",
        "ProjectilePosition2D",
        "ProjectileRamp",
        "ProjectileSprite2D",
        "ProjectileType"
    };

    public static readonly System.Type[] componentTypes = {
        typeof(Animation.StateComponent),
        typeof(ECSInput.Component),
        typeof(Physics.Box2DColliderComponent),
        typeof(Physics.Sphere2DColliderComponent),
        typeof(Projectile.ColliderComponent),
        typeof(Projectile.IDComponent),
        typeof(Projectile.LinearDragComponent),
        typeof(Projectile.MovableComponent),
        typeof(Projectile.PhysicsState2DComponent),
        typeof(Projectile.Position2DComponent),
        typeof(Projectile.RampComponent),
        typeof(Projectile.Sprite2DComponent),
        typeof(Projectile.TypeComponent)
    };
}
