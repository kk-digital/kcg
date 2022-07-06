using System.Collections.Generic;

namespace Source {
    namespace SystemView {
        public class SpaceStation {
            public List<SystemShip>         landed_ships = new();
            public List<SystemShip>         owned_ships  = new();

            public SpaceObject              self         = new();
            public OrbitingObjectDescriptor descriptor;

            public int health, max_health;
            public int shield, max_shield;

            public int shield_regeneration_rate;

            public float max_docking_range = 25.0f;

            public SpaceStation() {
                descriptor = new(self);
            }

            public bool dock(SystemShip ship) {
                // todo: check if ship has permission to dock? or if docking bay is full?
                //       should docking bay have a maximum capacity?

                if(ship.docked) return false;

                if(!landed_ships.Contains(ship) && Tools.get_distance(ship.self.posx, ship.self.posy, self.posx, self.posy) <= max_docking_range) {
                    landed_ships.Add(ship);
                    ship.docked = true;
                    ship.docked_at = this;
                    return true;
                }
                return false;
            }

            public void undock(SystemShip ship) {
                if(landed_ships.Contains(ship)) {
                    landed_ships.Remove(ship);
                    ship.docked = false;
                    ship.docked_at = null;
                }
            }
        }
    }
}
