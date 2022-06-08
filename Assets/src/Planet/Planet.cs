
using UnityEngine;
using TileProperties;
using PlanetTileMap;

namespace Planet
{
    public class Planet
    {


        public PlanetTileMap.PlanetTileMap TileMap;

        public Planet(Vector2Int mapSize)
        {
            TileMap = new PlanetTileMap.PlanetTileMap(mapSize);
        }




    }
}