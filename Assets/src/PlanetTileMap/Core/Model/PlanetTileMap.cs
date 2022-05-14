//using Entitas;

namespace PlanetTileMap
{
    //public struct PlanetMap : IComponent
    public struct PlanetTileMap
    {
        public int Xsize;
        public int Ysize;
        public PlanetTile[,] Tiles;

        public PlanetTileMap(int xsize, int ysize) : this()
        {
            Xsize = xsize;
            Ysize = ysize;
            Tiles = new PlanetTile[Xsize, Ysize];
        }
    }
}