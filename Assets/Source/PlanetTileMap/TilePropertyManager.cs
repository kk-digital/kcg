using Enums.Tile;

namespace PlanetTileMap
{
    public struct TilePropertyManager
    {
        /// <summary>
        /// Array that maps and index value to a struct. Storing tile property information for each tile
        /// </summary>
        public TileProperty[] TilePropertyArray;
        
        public void InitStage1()
        {
            var tilePropertyArray = new TileProperty[4096];

            for (int i = 0; i < tilePropertyArray.Length; i++)
            {
                tilePropertyArray[i].TileID = TileID.Empty;
                tilePropertyArray[i].SpriteId = TileSpriteID.Error;
            }

            TilePropertyArray = tilePropertyArray;
        }

        public void InitStage2()
        {
            
        }
        
        public TileProperty GetTileProperty(TileID tileId)
        {
            return TilePropertyArray[(int) tileId];
        }
    }
}
