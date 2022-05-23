using Enums;
using SpriteAtlas;
using System;


namespace PlanetTileMap
{
    class PlanetMapInfo
    {
        public PlanetTileMap Map;

        //Todo: Remove these. they have to do with SpriteStorage
        //TODO: Use datastructure in SrpiteAtlas
        public SpriteAtlas.SpriteAtlas[] SpritesById;
        //TODO: Use datastructure in SrpiteAtlas
        public int[][,] AtlasTextures;

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