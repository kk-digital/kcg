namespace Tiles
{
    struct PlanetMap 
    {
        public readonly int Xsize;
        public readonly int Ysize;
        public readonly PlanetTile[,] Tiles;

        public PlanetMap(int xsize, int ysize) : this()
        {
            Xsize = xsize;
            Ysize = ysize;
            Tiles = new PlanetTile[Xsize, Ysize];
        }
    }
}