namespace Source {
    namespace SystemView {
        public enum PlanetType {
            PLANET_ROCKY,
            PLANET_GAS_GIANT,
            PLANET_MOON
        };

        public class SystemPlanet {
            public SpaceObject              self;
            public OrbitingObjectDescriptor descriptor;
            public PlanetType               type;

            // todo: add more properties

            public SystemPlanet() {
                self       = new SpaceObject();
                descriptor = new OrbitingObjectDescriptor(self);
            }
        }
    }
}
