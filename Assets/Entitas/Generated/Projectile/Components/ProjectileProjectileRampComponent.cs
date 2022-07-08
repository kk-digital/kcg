//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ProjectileEntity {

    public Projectile.RampComponent projectileRamp { get { return (Projectile.RampComponent)GetComponent(ProjectileComponentsLookup.ProjectileRamp); } }
    public bool hasProjectileRamp { get { return HasComponent(ProjectileComponentsLookup.ProjectileRamp); } }

    public void AddProjectileRamp(bool newCanRamp, float newStartVelocity, float newMaxVelocity, float newRampTime) {
        var index = ProjectileComponentsLookup.ProjectileRamp;
        var component = (Projectile.RampComponent)CreateComponent(index, typeof(Projectile.RampComponent));
        component.canRamp = newCanRamp;
        component.startVelocity = newStartVelocity;
        component.maxVelocity = newMaxVelocity;
        component.rampTime = newRampTime;
        AddComponent(index, component);
    }

    public void ReplaceProjectileRamp(bool newCanRamp, float newStartVelocity, float newMaxVelocity, float newRampTime) {
        var index = ProjectileComponentsLookup.ProjectileRamp;
        var component = (Projectile.RampComponent)CreateComponent(index, typeof(Projectile.RampComponent));
        component.canRamp = newCanRamp;
        component.startVelocity = newStartVelocity;
        component.maxVelocity = newMaxVelocity;
        component.rampTime = newRampTime;
        ReplaceComponent(index, component);
    }

    public void RemoveProjectileRamp() {
        RemoveComponent(ProjectileComponentsLookup.ProjectileRamp);
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
public sealed partial class ProjectileMatcher {

    static Entitas.IMatcher<ProjectileEntity> _matcherProjectileRamp;

    public static Entitas.IMatcher<ProjectileEntity> ProjectileRamp {
        get {
            if (_matcherProjectileRamp == null) {
                var matcher = (Entitas.Matcher<ProjectileEntity>)Entitas.Matcher<ProjectileEntity>.AllOf(ProjectileComponentsLookup.ProjectileRamp);
                matcher.componentNames = ProjectileComponentsLookup.componentNames;
                _matcherProjectileRamp = matcher;
            }

            return _matcherProjectileRamp;
        }
    }
}
