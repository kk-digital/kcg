//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Projectile.TypeComponent projectileType { get { return (Projectile.TypeComponent)GetComponent(GameComponentsLookup.ProjectileType); } }
    public bool hasProjectileType { get { return HasComponent(GameComponentsLookup.ProjectileType); } }

    public void AddProjectileType(Enums.ProjectileType newType, Enums.ProjectileDrawType newDrawType) {
        var index = GameComponentsLookup.ProjectileType;
        var component = (Projectile.TypeComponent)CreateComponent(index, typeof(Projectile.TypeComponent));
        component.Type = newType;
        component.DrawType = newDrawType;
        AddComponent(index, component);
    }

    public void ReplaceProjectileType(Enums.ProjectileType newType, Enums.ProjectileDrawType newDrawType) {
        var index = GameComponentsLookup.ProjectileType;
        var component = (Projectile.TypeComponent)CreateComponent(index, typeof(Projectile.TypeComponent));
        component.Type = newType;
        component.DrawType = newDrawType;
        ReplaceComponent(index, component);
    }

    public void RemoveProjectileType() {
        RemoveComponent(GameComponentsLookup.ProjectileType);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherProjectileType;

    public static Entitas.IMatcher<GameEntity> ProjectileType {
        get {
            if (_matcherProjectileType == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ProjectileType);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherProjectileType = matcher;
            }

            return _matcherProjectileType;
        }
    }
}