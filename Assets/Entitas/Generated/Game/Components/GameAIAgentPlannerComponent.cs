//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public AI.AgentPlannerComponent aIAgentPlanner { get { return (AI.AgentPlannerComponent)GetComponent(GameComponentsLookup.AIAgentPlanner); } }
    public bool hasAIAgentPlanner { get { return HasComponent(GameComponentsLookup.AIAgentPlanner); } }

    public void AddAIAgentPlanner(int newAgentPlannerID, System.Collections.Generic.Queue<int> newActionIDs, System.Collections.Generic.List<AI.ActionInfo> newActiveActionIDs, System.Collections.Generic.List<int> newGoalIDs, AI.GoapState newCurrentWorldState) {
        var index = GameComponentsLookup.AIAgentPlanner;
        var component = (AI.AgentPlannerComponent)CreateComponent(index, typeof(AI.AgentPlannerComponent));
        component.AgentPlannerID = newAgentPlannerID;
        component.ActionIDs = newActionIDs;
        component.ActiveActionIDs = newActiveActionIDs;
        component.GoalIDs = newGoalIDs;
        component.CurrentWorldState = newCurrentWorldState;
        AddComponent(index, component);
    }

    public void ReplaceAIAgentPlanner(int newAgentPlannerID, System.Collections.Generic.Queue<int> newActionIDs, System.Collections.Generic.List<AI.ActionInfo> newActiveActionIDs, System.Collections.Generic.List<int> newGoalIDs, AI.GoapState newCurrentWorldState) {
        var index = GameComponentsLookup.AIAgentPlanner;
        var component = (AI.AgentPlannerComponent)CreateComponent(index, typeof(AI.AgentPlannerComponent));
        component.AgentPlannerID = newAgentPlannerID;
        component.ActionIDs = newActionIDs;
        component.ActiveActionIDs = newActiveActionIDs;
        component.GoalIDs = newGoalIDs;
        component.CurrentWorldState = newCurrentWorldState;
        ReplaceComponent(index, component);
    }

    public void RemoveAIAgentPlanner() {
        RemoveComponent(GameComponentsLookup.AIAgentPlanner);
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

    static Entitas.IMatcher<GameEntity> _matcherAIAgentPlanner;

    public static Entitas.IMatcher<GameEntity> AIAgentPlanner {
        get {
            if (_matcherAIAgentPlanner == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.AIAgentPlanner);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAIAgentPlanner = matcher;
            }

            return _matcherAIAgentPlanner;
        }
    }
}
