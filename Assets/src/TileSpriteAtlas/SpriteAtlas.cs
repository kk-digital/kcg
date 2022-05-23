using Enums;

//TODO: Move all sprite structs to TileSpriteManager, out of PlanetTileMap

//TODO: Delete TileSprite information from PlanetTileMap, move to TileSpriteManager
namespace SpriteAtlas
{

    //For now, array of 1 element, one texture
    //API: *SpriteAtlas GetSpriteAtlas(id)
    //API: int GetGlTextureId(SpriteAtlasId)
    //All SpriteAtlases are named by an int enum
    //TODO: Put SpriteAtlases in Enums
    //ex. SpriteAtlasTileMap

    class SpriteAtlasManager
    {
        //TODO: Add array
        //TODO: Make SpriteAtlas struct


    }

    /*
     struct SpriteAtlas
        {
            public int id;

            public int Width; //Size.x, Size.y
            public int Height;

            public int[,] PixelArray;
        }
    */

    //TODO: int GlTexId = AtlasIntsance.GlTextureId()
    //

    //TODO: BLIT
    // BLIT COPIES SPRITES FROM TileSpriteLoader to TileSpriteAtlas
    //Blit Takes in: TileSpriteId (the sprite sheet), x,y position of tile to copy; and returns a AtlasSpriteID

    //Example:
    //if Atlas is 16x16, 32x32 pixel tiles, then first tile is Id = 0

    //AtlasSpriteID to position on the AtlasTexture
    // (0,0 ) = index ~= 0
    // (15,0), index=15
    // (0,1), index=16
    // (x,y), index=  x + width*y

    //Blit-8-to-32
    //8x8 -> 32x32 (1x1 -> 4x4)

    //Blit-16-to-32
    //16x16 -> 32x32 (1x1 -> 2x2)

    //Blit-32-to-32
    //32x32 -> 32x32

    //TODO: DELETE
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