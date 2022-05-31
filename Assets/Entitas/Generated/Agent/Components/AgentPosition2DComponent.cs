//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class AgentEntity {

    public Components.Agent.Position2DComponent position2D { get { return (Components.Agent.Position2DComponent)GetComponent(AgentComponentsLookup.Position2D); } }
    public bool hasPosition2D { get { return HasComponent(AgentComponentsLookup.Position2D); } }

    public void AddPosition2D(UnityEngine.Vector2 newPosition, UnityEngine.Vector2 newPreviousPosition) {
        var index = AgentComponentsLookup.Position2D;
        var component = (Components.Agent.Position2DComponent)CreateComponent(index, typeof(Components.Agent.Position2DComponent));
        component.Position = newPosition;
        component.PreviousPosition = newPreviousPosition;
        AddComponent(index, component);
    }

    public void ReplacePosition2D(UnityEngine.Vector2 newPosition, UnityEngine.Vector2 newPreviousPosition) {
        var index = AgentComponentsLookup.Position2D;
        var component = (Components.Agent.Position2DComponent)CreateComponent(index, typeof(Components.Agent.Position2DComponent));
        component.Position = newPosition;
        component.PreviousPosition = newPreviousPosition;
        ReplaceComponent(index, component);
    }

    public void RemovePosition2D() {
        RemoveComponent(AgentComponentsLookup.Position2D);
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
public sealed partial class AgentMatcher {

    static Entitas.IMatcher<AgentEntity> _matcherPosition2D;

    public static Entitas.IMatcher<AgentEntity> Position2D {
        get {
            if (_matcherPosition2D == null) {
                var matcher = (Entitas.Matcher<AgentEntity>)Entitas.Matcher<AgentEntity>.AllOf(AgentComponentsLookup.Position2D);
                matcher.componentNames = AgentComponentsLookup.componentNames;
                _matcherPosition2D = matcher;
            }

            return _matcherPosition2D;
        }
    }
}
