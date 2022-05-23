using Enums;

//TODO: Move all sprite structs to TileSpriteManager, out of PlanetTileMap

//TODO: Delete TileSprite information from PlanetTileMap, move to TileSpriteManager
namespace SpriteAtlas
{
    //TODO:Destroy, use SpriteAtlas datastructuer to replace
    //Each sprite is just (SpriteAtlasId, Index)
    struct SpriteAtlas
    {
        public int id;
        public int Left;
        public int Top;

        public int Width;
        public int Height;

        public int[,] PixelArray;

        public PlanetTileLayer Layer;
    }
}