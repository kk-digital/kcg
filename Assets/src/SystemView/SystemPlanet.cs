namespace SystemView
{
    public class SystemPlanet
    {
        public OrbitingObjectDescriptor Descriptor;

        // todo: add more properties

        public SystemPlanet()
        {
            Descriptor = new OrbitingObjectDescriptor();
        }

        public void UpdatePosition(float dt)
        {
            Descriptor.RotationalPosition += dt / Descriptor.GetDistanceFromCenter() / Descriptor.GetDistanceFromCenter();
        }
    }
}
