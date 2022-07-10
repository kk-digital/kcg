namespace Source {
    namespace SystemView {
        public class SystemStar {
            public SpaceObject              self         = new();
            public OrbitingObjectDescriptor descriptor;
            public bool                     render_orbit;

            public SystemStar() {
                descriptor = new(self);
            }
        };
    }
}