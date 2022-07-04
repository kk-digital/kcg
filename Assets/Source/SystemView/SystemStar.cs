namespace Source {
    namespace SystemView {
        // Some of these will probably go away in later versions
        public enum StarType {
            NEBULA,
            PROTOSTAR,
            MAIN_SEQUENCE_STAR,
            RED_GIANT,
            BLUE_GIANT,
            WHITE_DWARF,
            NEUTRON_STAR,
            BLACK_HOLE
        };

        public class SystemStar {
            public StarType    type;
            public SpaceObject self;

            public SystemStar() {
                self = new();
            }

            public void update(float current_time) {

            }
        };
    }
}