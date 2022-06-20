namespace Source {
    namespace SystemView {
        public class SystemPlanet {
            public SpaceObject              Self;
            public OrbitingObjectDescriptor descriptor;

            // todo: add more properties

            public SystemPlanet() {
                Self       = new SpaceObject();
                descriptor = new OrbitingObjectDescriptor(Self);
            }
        }
    }
}
