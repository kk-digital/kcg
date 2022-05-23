//using Entitas;

using TileProperties;

namespace PlanetTileMap
{
    //public struct PlanetMap : IComponent
    public struct PlanetTileMap
    {
        public int Xsize; //Size.X
        public int Ysize; //Size.Y
        public PlanetTile[,] Tiles;

        public PlanetTileMap(int xsize, int ysize) : this()
        {
            Xsize = xsize;
            Ysize = ysize;
            Tiles = new PlanetTile[Xsize, Ysize];
        }
    }
}