//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Projectile.Sprite2DComponent projectileSprite2D { get { return (Projectile.Sprite2DComponent)GetComponent(GameComponentsLookup.ProjectileSprite2D); } }
    public bool hasProjectileSprite2D { get { return HasComponent(GameComponentsLookup.ProjectileSprite2D); } }

    public void AddProjectileSprite2D(UnityEngine.Texture2D newTexture, UnityEngine.Vector2 newSize) {
        var index = GameComponentsLookup.ProjectileSprite2D;
        var component = (Projectile.Sprite2DComponent)CreateComponent(index, typeof(Projectile.Sprite2DComponent));
        component.Texture = newTexture;
        component.Size = newSize;
        AddComponent(index, component);
    }

    public void ReplaceProjectileSprite2D(UnityEngine.Texture2D newTexture, UnityEngine.Vector2 newSize) {
        var index = GameComponentsLookup.ProjectileSprite2D;
        var component = (Projectile.Sprite2DComponent)CreateComponent(index, typeof(Projectile.Sprite2DComponent));
        component.Texture = newTexture;
        component.Size = newSize;
        ReplaceComponent(index, component);
    }

    public void RemoveProjectileSprite2D() {
        RemoveComponent(GameComponentsLookup.ProjectileSprite2D);
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

    static Entitas.IMatcher<GameEntity> _matcherProjectileSprite2D;

    public static Entitas.IMatcher<GameEntity> ProjectileSprite2D {
        get {
            if (_matcherProjectileSprite2D == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ProjectileSprite2D);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherProjectileSprite2D = matcher;
            }

            return _matcherProjectileSprite2D;
        }
    }
}
