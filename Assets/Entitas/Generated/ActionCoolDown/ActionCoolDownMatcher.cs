//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ContextMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class ActionCoolDownMatcher {

    public static Entitas.IAllOfMatcher<ActionCoolDownEntity> AllOf(params int[] indices) {
        return Entitas.Matcher<ActionCoolDownEntity>.AllOf(indices);
    }

    public static Entitas.IAllOfMatcher<ActionCoolDownEntity> AllOf(params Entitas.IMatcher<ActionCoolDownEntity>[] matchers) {
          return Entitas.Matcher<ActionCoolDownEntity>.AllOf(matchers);
    }

    public static Entitas.IAnyOfMatcher<ActionCoolDownEntity> AnyOf(params int[] indices) {
          return Entitas.Matcher<ActionCoolDownEntity>.AnyOf(indices);
    }

    public static Entitas.IAnyOfMatcher<ActionCoolDownEntity> AnyOf(params Entitas.IMatcher<ActionCoolDownEntity>[] matchers) {
          return Entitas.Matcher<ActionCoolDownEntity>.AnyOf(matchers);
    }
}