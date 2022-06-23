using Enums.Tile;
using KMath;
using UnityEngine;

namespace PlanetTileMap
{
    public struct Tile
    {
        public static Tile Air = new() {ID = TileID.Air, SpriteID = -1};
        
        public TileID ID;
        public int SpriteID;
    }
}
