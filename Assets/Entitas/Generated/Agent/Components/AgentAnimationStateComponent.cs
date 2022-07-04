//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class AgentEntity {

    public Animation.StateComponent animationState { get { return (Animation.StateComponent)GetComponent(AgentComponentsLookup.AnimationState); } }
    public bool hasAnimationState { get { return HasComponent(AgentComponentsLookup.AnimationState); } }

    public void AddAnimationState(float newAnimationSpeed, Animation.Animation newState) {
        var index = AgentComponentsLookup.AnimationState;
        var component = (Animation.StateComponent)CreateComponent(index, typeof(Animation.StateComponent));
        component.AnimationSpeed = newAnimationSpeed;
        component.State = newState;
        AddComponent(index, component);
    }

    public void ReplaceAnimationState(float newAnimationSpeed, Animation.Animation newState) {
        var index = AgentComponentsLookup.AnimationState;
        var component = (Animation.StateComponent)CreateComponent(index, typeof(Animation.StateComponent));
        component.AnimationSpeed = newAnimationSpeed;
        component.State = newState;
        ReplaceComponent(index, component);
    }

    public void RemoveAnimationState() {
        RemoveComponent(AgentComponentsLookup.AnimationState);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiInterfaceGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class AgentEntity : IAnimationStateEntity { }

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class AgentMatcher {

    static Entitas.IMatcher<AgentEntity> _matcherAnimationState;

    public static Entitas.IMatcher<AgentEntity> AnimationState {
        get {
            if (_matcherAnimationState == null) {
                var matcher = (Entitas.Matcher<AgentEntity>)Entitas.Matcher<AgentEntity>.AllOf(AgentComponentsLookup.AnimationState);
                matcher.componentNames = AgentComponentsLookup.componentNames;
                _matcherAnimationState = matcher;
            }

            return _matcherAnimationState;
        }
    }
}
