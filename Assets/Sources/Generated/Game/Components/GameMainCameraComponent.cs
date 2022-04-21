//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity mainCameraEntity { get { return GetGroup(GameMatcher.MainCamera).GetSingleEntity(); } }

    public bool isMainCamera {
        get { return mainCameraEntity != null; }
        set {
            var entity = mainCameraEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isMainCamera = true;
                } else {
                    entity.Destroy();
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly MainCameraComponent mainCameraComponent = new MainCameraComponent();

    public bool isMainCamera {
        get { return HasComponent(GameComponentsLookup.MainCamera); }
        set {
            if (value != isMainCamera) {
                var index = GameComponentsLookup.MainCamera;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : mainCameraComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherMainCamera;

    public static Entitas.IMatcher<GameEntity> MainCamera {
        get {
            if (_matcherMainCamera == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.MainCamera);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherMainCamera = matcher;
            }

            return _matcherMainCamera;
        }
    }
}
