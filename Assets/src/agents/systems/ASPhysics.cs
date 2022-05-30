using Tiles.PlanetMap;

namespace Agents.Systems
{
    public class ASPhysics
    {
        private static ASPhysics instance;
        public static ASPhysics Instance => instance ??= new ASPhysics();

        public void UpdateAgents(ref Tiles.PlanetMap.TilesPlanetMap tilesPlanetMap)
        {
            
        }
    }
}
