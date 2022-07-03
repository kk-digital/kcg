using Entitas;

namespace Projectile
{
    public class List
    {
        private readonly ProjectileContext projectileContext;

        public IGroup<ProjectileEntity> ProjectilesWithSprite;
        public IGroup<ProjectileEntity> ProjectilesWithInput;
        public IGroup<ProjectileEntity> ProjectilesWithPhysics;

        // List of projectiles
        public List()
        {
            projectileContext = Contexts.sharedInstance.projectile;
            ProjectilesWithSprite = projectileContext.GetGroup(ProjectileMatcher.AllOf(ProjectileMatcher.ProjectileID, ProjectileMatcher.ProjectileSprite2D));
            ProjectilesWithInput = projectileContext.GetGroup(ProjectileMatcher.AllOf(ProjectileMatcher.ProjectileID, ProjectileMatcher.ECSInput));
            ProjectilesWithPhysics = projectileContext.GetGroup(ProjectileMatcher.AllOf(ProjectileMatcher.ProjectileID, ProjectileMatcher.ProjectilePhysicsState2D, ProjectileMatcher.ProjectilePhysicsState2D));
        }
    }
}
