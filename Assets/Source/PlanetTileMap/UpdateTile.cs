using Enums.Tile;
using KMath;

namespace PlanetTileMap
{
    public struct UpdateTile
    {
        public Vec2i Position;
        public MapLayerType Layer;

        public UpdateTile(Vec2i position, MapLayerType layer)
        {
            Position = position;
            Layer = layer;
        }
    }
}
