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
            Descriptor.RotationalPosition += 0.05f / Descriptor.GetDistanceFromCenter();
        }
    }
}
