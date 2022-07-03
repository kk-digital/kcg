using Entitas;

namespace Vehicle
{
    public class List
    {
        private readonly VehicleContext Context;

        public IGroup<VehicleEntity> VehiclesWithSprite;
        public IGroup<VehicleEntity> VehiclesWithInput;
        public IGroup<VehicleEntity> VehiclesWithPhysics;

        // List of vehicles
        public List()
        {
            Context = Contexts.sharedInstance.vehicle;
            VehiclesWithSprite = Context.GetGroup(VehicleMatcher.AllOf(VehicleMatcher.VehicleID, VehicleMatcher.VehicleSprite2D));
            VehiclesWithInput = Context.GetGroup(VehicleMatcher.AllOf(VehicleMatcher.VehicleID, VehicleMatcher.ECSInput));
            VehiclesWithPhysics = Context.GetGroup(VehicleMatcher.AllOf(VehicleMatcher.VehicleID, VehicleMatcher.VehiclePhysicsState2D, VehicleMatcher.VehiclePhysicsState2D));
        }
    }
}

