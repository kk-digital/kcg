using Enums;

namespace PlanetTileMap
{
    class PlanetTileProperties
    {
        public string Name;
        public int SpriteId;
        public int SecondarySpriteId;
        public PlanetTileCategory TileCategory;
        public PlanetTileLayer Layer;
        public PlanetTileCollisionType TileCollisionType;
        public byte MaxHealth;
    }
}