namespace SystemView
{
    public class SystemPlanet
    {
        public SpaceObject              Self;
        public OrbitingObjectDescriptor Descriptor;

        // todo: add more properties

        public SystemPlanet()
        {
            Self       = new SpaceObject();
            Descriptor = new OrbitingObjectDescriptor(Self);
        }
    }
}
