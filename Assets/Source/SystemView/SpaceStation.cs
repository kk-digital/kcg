using System.Collections.Generic;

namespace SystemView
{
    public class SpaceStation
    {
        public List<SystemShip> LandedShips = new();
        public List<SystemShip> OwnedShips  = new();

        public OrbitingObjectDescriptor Descriptor = new();
    }
}
