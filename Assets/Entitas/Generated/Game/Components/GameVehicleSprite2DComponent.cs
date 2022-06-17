//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Vehicle.Sprite2DComponent vehicleSprite2D { get { return (Vehicle.Sprite2DComponent)GetComponent(GameComponentsLookup.VehicleSprite2D); } }
    public bool hasVehicleSprite2D { get { return HasComponent(GameComponentsLookup.VehicleSprite2D); } }

    public void AddVehicleSprite2D(UnityEngine.Texture2D newTexture, UnityEngine.Vector2 newSize) {
        var index = GameComponentsLookup.VehicleSprite2D;
        var component = (Vehicle.Sprite2DComponent)CreateComponent(index, typeof(Vehicle.Sprite2DComponent));
        component.Texture = newTexture;
        component.Size = newSize;
        AddComponent(index, component);
    }

    public void ReplaceVehicleSprite2D(UnityEngine.Texture2D newTexture, UnityEngine.Vector2 newSize) {
        var index = GameComponentsLookup.VehicleSprite2D;
        var component = (Vehicle.Sprite2DComponent)CreateComponent(index, typeof(Vehicle.Sprite2DComponent));
        component.Texture = newTexture;
        component.Size = newSize;
        ReplaceComponent(index, component);
    }

    public void RemoveVehicleSprite2D() {
        RemoveComponent(GameComponentsLookup.VehicleSprite2D);
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

    static Entitas.IMatcher<GameEntity> _matcherVehicleSprite2D;

    public static Entitas.IMatcher<GameEntity> VehicleSprite2D {
        get {
            if (_matcherVehicleSprite2D == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.VehicleSprite2D);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherVehicleSprite2D = matcher;
            }

            return _matcherVehicleSprite2D;
        }
    }
}
