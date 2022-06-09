namespace SystemView
{
    public class SystemPlanet
    {
        public SystemViewBody           Self;
        public OrbitingObjectDescriptor Descriptor;

        // todo: add more properties

        public SystemPlanet()
        {
            Self       = new SystemViewBody();
            Self.Mass  = 5000.0f;
            Descriptor = new OrbitingObjectDescriptor(Self);
        }
    }
}
