using Entitas;

namespace PlanetTileMap
{
    //public struct PlanetMap : IComponent
    public struct PlanetMap
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