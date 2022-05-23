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

        public PlanetTile GetTile(int x, int y)
        {
            return Tiles[x, y];
        }

        /*public ... GetTileProperty(int x, int y)
        {
            return Tiles[x, y];
        }*/


        //Take in PlanetTileMap, and map a horizonal line
        public void GenerateFlatPlanet()
        {
            //default size = X...

            //make a single line horizonally across planet
            //from left to right

            //int TileId = GetTileId("default-tile")
            //for x = 0 to x = Planet.Size.X
            //Planet.SetTile(TileId, x, 10)


        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> cb502b2eec04ffb93752bc7706e5dc38777a12d7
