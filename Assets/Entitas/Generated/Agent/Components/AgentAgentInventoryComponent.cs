//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class AgentEntity {

    public Agent.InventoryComponent agentInventory { get { return (Agent.InventoryComponent)GetComponent(AgentComponentsLookup.AgentInventory); } }
    public bool hasAgentInventory { get { return HasComponent(AgentComponentsLookup.AgentInventory); } }

    public void AddAgentInventory(int newInventoryID) {
        var index = AgentComponentsLookup.AgentInventory;
        var component = (Agent.InventoryComponent)CreateComponent(index, typeof(Agent.InventoryComponent));
        component.InventoryID = newInventoryID;
        AddComponent(index, component);
    }

    public void ReplaceAgentInventory(int newInventoryID) {
        var index = AgentComponentsLookup.AgentInventory;
        var component = (Agent.InventoryComponent)CreateComponent(index, typeof(Agent.InventoryComponent));
        component.InventoryID = newInventoryID;
        ReplaceComponent(index, component);
    }

    public void RemoveAgentInventory() {
        RemoveComponent(AgentComponentsLookup.AgentInventory);
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

    static Entitas.IMatcher<AgentEntity> _matcherAgentInventory;

    public static Entitas.IMatcher<AgentEntity> AgentInventory {
        get {
            if (_matcherAgentInventory == null) {
                var matcher = (Entitas.Matcher<AgentEntity>)Entitas.Matcher<AgentEntity>.AllOf(AgentComponentsLookup.AgentInventory);
                matcher.componentNames = AgentComponentsLookup.componentNames;
                _matcherAgentInventory = matcher;
            }

            return _matcherAgentInventory;
        }
    }
}
