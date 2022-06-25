//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Agent.EnemyComponent agentEnemy { get { return (Agent.EnemyComponent)GetComponent(GameComponentsLookup.AgentEnemy); } }
    public bool hasAgentEnemy { get { return HasComponent(GameComponentsLookup.AgentEnemy); } }

    public void AddAgentEnemy(int newBehaviour, float newDetectionRadius) {
        var index = GameComponentsLookup.AgentEnemy;
        var component = (Agent.EnemyComponent)CreateComponent(index, typeof(Agent.EnemyComponent));
        component.Behaviour = newBehaviour;
        component.DetectionRadius = newDetectionRadius;
        AddComponent(index, component);
    }

    public void ReplaceAgentEnemy(int newBehaviour, float newDetectionRadius) {
        var index = GameComponentsLookup.AgentEnemy;
        var component = (Agent.EnemyComponent)CreateComponent(index, typeof(Agent.EnemyComponent));
        component.Behaviour = newBehaviour;
        component.DetectionRadius = newDetectionRadius;
        ReplaceComponent(index, component);
    }

    public void RemoveAgentEnemy() {
        RemoveComponent(GameComponentsLookup.AgentEnemy);
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

    static Entitas.IMatcher<GameEntity> _matcherAgentEnemy;

    public static Entitas.IMatcher<GameEntity> AgentEnemy {
        get {
            if (_matcherAgentEnemy == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.AgentEnemy);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAgentEnemy = matcher;
            }

            return _matcherAgentEnemy;
        }
    }
}
