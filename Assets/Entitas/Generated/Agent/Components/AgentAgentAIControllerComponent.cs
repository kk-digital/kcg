//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class AgentEntity {

    public Agent.AIController agentAIController { get { return (Agent.AIController)GetComponent(AgentComponentsLookup.AgentAIController); } }
    public bool hasAgentAIController { get { return HasComponent(AgentComponentsLookup.AgentAIController); } }

    public void AddAgentAIController(int newAgentPlannerID, System.Collections.Generic.Queue<int> newActionIDs, System.Collections.Generic.List<int> newGoalIDs, AI.GoapState newCurrentWorldState) {
        var index = AgentComponentsLookup.AgentAIController;
        var component = (Agent.AIController)CreateComponent(index, typeof(Agent.AIController));
        component.AgentPlannerID = newAgentPlannerID;
        component.ActionIDs = newActionIDs;
        component.GoalIDs = newGoalIDs;
        component.CurrentWorldState = newCurrentWorldState;
        AddComponent(index, component);
    }

    public void ReplaceAgentAIController(int newAgentPlannerID, System.Collections.Generic.Queue<int> newActionIDs, System.Collections.Generic.List<int> newGoalIDs, AI.GoapState newCurrentWorldState) {
        var index = AgentComponentsLookup.AgentAIController;
        var component = (Agent.AIController)CreateComponent(index, typeof(Agent.AIController));
        component.AgentPlannerID = newAgentPlannerID;
        component.ActionIDs = newActionIDs;
        component.GoalIDs = newGoalIDs;
        component.CurrentWorldState = newCurrentWorldState;
        ReplaceComponent(index, component);
    }

    public void RemoveAgentAIController() {
        RemoveComponent(AgentComponentsLookup.AgentAIController);
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

    static Entitas.IMatcher<AgentEntity> _matcherAgentAIController;

    public static Entitas.IMatcher<AgentEntity> AgentAIController {
        get {
            if (_matcherAgentAIController == null) {
                var matcher = (Entitas.Matcher<AgentEntity>)Entitas.Matcher<AgentEntity>.AllOf(AgentComponentsLookup.AgentAIController);
                matcher.componentNames = AgentComponentsLookup.componentNames;
                _matcherAgentAIController = matcher;
            }

            return _matcherAgentAIController;
        }
    }
}
