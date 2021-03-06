//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class VehicleEntity {

    public Vehicle.Sprite2DComponent vehicleSprite2D { get { return (Vehicle.Sprite2DComponent)GetComponent(VehicleComponentsLookup.VehicleSprite2D); } }
    public bool hasVehicleSprite2D { get { return HasComponent(VehicleComponentsLookup.VehicleSprite2D); } }

    public void AddVehicleSprite2D(int newSpriteId, KMath.Vec2f newSize) {
        var index = VehicleComponentsLookup.VehicleSprite2D;
        var component = (Vehicle.Sprite2DComponent)CreateComponent(index, typeof(Vehicle.Sprite2DComponent));
        component.SpriteId = newSpriteId;
        component.Size = newSize;
        AddComponent(index, component);
    }

    public void ReplaceVehicleSprite2D(int newSpriteId, KMath.Vec2f newSize) {
        var index = VehicleComponentsLookup.VehicleSprite2D;
        var component = (Vehicle.Sprite2DComponent)CreateComponent(index, typeof(Vehicle.Sprite2DComponent));
        component.SpriteId = newSpriteId;
        component.Size = newSize;
        ReplaceComponent(index, component);
    }

    public void RemoveVehicleSprite2D() {
        RemoveComponent(VehicleComponentsLookup.VehicleSprite2D);
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
public sealed partial class VehicleMatcher {

    static Entitas.IMatcher<VehicleEntity> _matcherVehicleSprite2D;

    public static Entitas.IMatcher<VehicleEntity> VehicleSprite2D {
        get {
            if (_matcherVehicleSprite2D == null) {
                var matcher = (Entitas.Matcher<VehicleEntity>)Entitas.Matcher<VehicleEntity>.AllOf(VehicleComponentsLookup.VehicleSprite2D);
                matcher.componentNames = VehicleComponentsLookup.componentNames;
                _matcherVehicleSprite2D = matcher;
            }

            return _matcherVehicleSprite2D;
        }
    }
}
