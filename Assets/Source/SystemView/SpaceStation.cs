using System.Collections.Generic;

namespace Source {
    namespace SystemView {
        public class SpaceStation {
            public List<SystemShip>         LandedShips = new();
            public List<SystemShip>         OwnedShips  = new();

            public SpaceObject              Self        = new();
            public OrbitingObjectDescriptor descriptor;

            public SpaceStation() {
                descriptor = new(Self);
            }
        }
    }
}
