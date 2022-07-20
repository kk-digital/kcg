namespace Source {
    namespace SystemView {
        public class SystemStar {
            public SpaceObject              self         = new();
            public OrbitingObjectDescriptor descriptor;
            public bool                     render_orbit;
            public float                    brightness   = 1.0f;
            public bool                     giant;

            public SystemStar() {
                descriptor = new(self);
            }
        };
    }
}