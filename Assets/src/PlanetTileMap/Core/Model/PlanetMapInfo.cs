using Enums;
using System;


namespace PlanetTileMap
{
    /// <summary> All info to render PlanetMap </summary>
    //TODO: delete PlanetTileMap, because its not doing anything
    class PlanetMapInfo
    {
        public PlanetTileMap Map;

        //Todo: Remove these. they have to do with SpriteStorage
        //TODO: Move these to TileSpriteManager
        public Sprite[] SpritesById;
        public int[][,] AtlasTextures;
        //public PlanetTileProperties[] TileProperties = TilePropertiesManager.Instance.TileProperties;
        //line 13 is moved to TilePropertiesManager

        public PlanetMapInfo()
        {
            AtlasTextures = new int[Enum.GetValues(typeof(PlanetTileLayer)).Length][,];
        }

        public int[,] GetAtlas(PlanetTileLayer layer)
        {
            return AtlasTextures[(int)layer];
        }

        public void SetAtlas(PlanetTileLayer layer, int[,] atlasTexture)
        {
            AtlasTextures[(int)layer] = atlasTexture;
        }
    }
}