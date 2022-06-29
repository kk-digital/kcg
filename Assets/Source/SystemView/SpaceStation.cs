using System.Collections.Generic;

namespace Source {
    namespace SystemView {
        public class SpaceStation {
            public List<SystemShip>         LandedShips = new();
            public List<SystemShip>         OwnedShips  = new();

            public SpaceObject              Self        = new();
            public OrbitingObjectDescriptor descriptor;

            public int health, max_health;
            public int shield, max_shield;

            public int shield_regeneration_rate;

            public SpaceStation() {
                descriptor = new(Self);
            }

            public void dock(SystemShip ship) {
                // todo: check if ship has permission to dock? or if docking bay is full?
                //       should docking bay have a maximum capacity?

                LandedShips.Add(ship);
            }

            public void undock(SystemShip ship) {
                LandedShips.Remove(ship);
            }
        }
    }
}
