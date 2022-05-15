using Enums;
using System;


namespace PlanetTileMap
{
    /// <summary> All info to render PlanetMap </summary>
    class PlanetMapInfo
    {
        public PlanetMap Map;
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