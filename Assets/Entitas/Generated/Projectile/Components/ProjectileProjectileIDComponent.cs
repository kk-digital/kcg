//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ProjectileEntity {

    public Projectile.IDComponent projectileID { get { return (Projectile.IDComponent)GetComponent(ProjectileComponentsLookup.ProjectileID); } }
    public bool hasProjectileID { get { return HasComponent(ProjectileComponentsLookup.ProjectileID); } }

    public void AddProjectileID(int newID) {
        var index = ProjectileComponentsLookup.ProjectileID;
        var component = (Projectile.IDComponent)CreateComponent(index, typeof(Projectile.IDComponent));
        component.ID = newID;
        AddComponent(index, component);
    }

    public void ReplaceProjectileID(int newID) {
        var index = ProjectileComponentsLookup.ProjectileID;
        var component = (Projectile.IDComponent)CreateComponent(index, typeof(Projectile.IDComponent));
        component.ID = newID;
        ReplaceComponent(index, component);
    }

    public void RemoveProjectileID() {
        RemoveComponent(ProjectileComponentsLookup.ProjectileID);
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

    static Entitas.IMatcher<ProjectileEntity> _matcherProjectileID;

    public static Entitas.IMatcher<ProjectileEntity> ProjectileID {
        get {
            if (_matcherProjectileID == null) {
                var matcher = (Entitas.Matcher<ProjectileEntity>)Entitas.Matcher<ProjectileEntity>.AllOf(ProjectileComponentsLookup.ProjectileID);
                matcher.componentNames = ProjectileComponentsLookup.componentNames;
                _matcherProjectileID = matcher;
            }

            return _matcherProjectileID;
        }
    }
}
