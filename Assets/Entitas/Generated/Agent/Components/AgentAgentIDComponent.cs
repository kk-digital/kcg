//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class AgentEntity {

    public Agent.IDComponent agentID { get { return (Agent.IDComponent)GetComponent(AgentComponentsLookup.AgentID); } }
    public bool hasAgentID { get { return HasComponent(AgentComponentsLookup.AgentID); } }

    public void AddAgentID(int newID) {
        var index = AgentComponentsLookup.AgentID;
        var component = (Agent.IDComponent)CreateComponent(index, typeof(Agent.IDComponent));
        component.ID = newID;
        AddComponent(index, component);
    }

    public void ReplaceAgentID(int newID) {
        var index = AgentComponentsLookup.AgentID;
        var component = (Agent.IDComponent)CreateComponent(index, typeof(Agent.IDComponent));
        component.ID = newID;
        ReplaceComponent(index, component);
    }

    public void RemoveAgentID() {
        RemoveComponent(AgentComponentsLookup.AgentID);
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

    static Entitas.IMatcher<AgentEntity> _matcherAgentID;

    public static Entitas.IMatcher<AgentEntity> AgentID {
        get {
            if (_matcherAgentID == null) {
                var matcher = (Entitas.Matcher<AgentEntity>)Entitas.Matcher<AgentEntity>.AllOf(AgentComponentsLookup.AgentID);
                matcher.componentNames = AgentComponentsLookup.componentNames;
                _matcherAgentID = matcher;
            }

            return _matcherAgentID;
        }
    }
}
