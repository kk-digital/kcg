namespace Source {
    namespace SystemView {
        public class SpaceObject {
            public float mass            = 1.0f;
            public float size            = 1.0f;
            public float posx            = 0.0f;
            public float posy            = 0.0f;
            public float velx            = 0.0f;
            public float vely            = 0.0f;
            public float angular_vel     = 0.0f;
    
            public float angular_inertia { get { return 0.5f * mass * size * size; } }
        }
    }
}
