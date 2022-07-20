namespace Source {
    namespace SystemView {
        public class SystemPlanet {
            public SpaceObject              self;
            public OrbitingObjectDescriptor descriptor;

            // todo: add more properties

            public SystemPlanet() {
                self       = new SpaceObject();
                descriptor = new OrbitingObjectDescriptor(self);
            }
        }
    }
}
