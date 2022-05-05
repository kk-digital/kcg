using Entitas;

namespace Tiles
{
    public struct PlanetMap : IComponent
    {
        public int Xsize;
        public int Ysize;
        public PlanetTile[,] Tiles;

        public PlanetMap(int xsize, int ysize) : this()
        {
            Xsize = xsize;
            Ysize = ysize;
            Tiles = new PlanetTile[Xsize, Ysize];
        }
    }
}