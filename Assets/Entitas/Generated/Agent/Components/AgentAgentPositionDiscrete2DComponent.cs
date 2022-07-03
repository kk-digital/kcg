//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class AgentEntity {

    public Agent.PositionDiscrete2DComponent agentPositionDiscrete2D { get { return (Agent.PositionDiscrete2DComponent)GetComponent(AgentComponentsLookup.AgentPositionDiscrete2D); } }
    public bool hasAgentPositionDiscrete2D { get { return HasComponent(AgentComponentsLookup.AgentPositionDiscrete2D); } }

    public void AddAgentPositionDiscrete2D(KMath.Vec2i newValue, KMath.Vec2i newPreviousValue) {
        var index = AgentComponentsLookup.AgentPositionDiscrete2D;
        var component = (Agent.PositionDiscrete2DComponent)CreateComponent(index, typeof(Agent.PositionDiscrete2DComponent));
        component.Value = newValue;
        component.PreviousValue = newPreviousValue;
        AddComponent(index, component);
    }

    public void ReplaceAgentPositionDiscrete2D(KMath.Vec2i newValue, KMath.Vec2i newPreviousValue) {
        var index = AgentComponentsLookup.AgentPositionDiscrete2D;
        var component = (Agent.PositionDiscrete2DComponent)CreateComponent(index, typeof(Agent.PositionDiscrete2DComponent));
        component.Value = newValue;
        component.PreviousValue = newPreviousValue;
        ReplaceComponent(index, component);
    }

    public void RemoveAgentPositionDiscrete2D() {
        RemoveComponent(AgentComponentsLookup.AgentPositionDiscrete2D);
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

    static Entitas.IMatcher<AgentEntity> _matcherAgentPositionDiscrete2D;

    public static Entitas.IMatcher<AgentEntity> AgentPositionDiscrete2D {
        get {
            if (_matcherAgentPositionDiscrete2D == null) {
                var matcher = (Entitas.Matcher<AgentEntity>)Entitas.Matcher<AgentEntity>.AllOf(AgentComponentsLookup.AgentPositionDiscrete2D);
                matcher.componentNames = AgentComponentsLookup.componentNames;
                _matcherAgentPositionDiscrete2D = matcher;
            }

            return _matcherAgentPositionDiscrete2D;
        }
    }
}
