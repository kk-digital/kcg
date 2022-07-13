using System;
using Enums.Tile;
using System.Collections.Generic;

namespace PlanetTileMap
{
    public struct UpdateTile
    {
        public int XPos;
        public int YPos;
        public MapLayerType Layer;

        public UpdateTile(int xPos, int yPos, MapLayerType layer)
        {
            XPos = xPos;
            YPos = yPos;
            Layer = layer;
        }
    }


    public class TileSpriteUpdateQueue
    {

        private List<UpdateTile> ToUpdateTiles;


        public TileSpriteUpdateQueue()
        {
            ToUpdateTiles = new List<UpdateTile>();
        }

        public void Add(int x, int y, MapLayerType layer)
        {
            ToUpdateTiles.Add(new UpdateTile(x, y, layer));
        }



        public void UpdateTileSprites(ref TileMap tileMap)
        {
            for(int i = 0; i < 1024 * 32 && i < ToUpdateTiles.Count; i++)
            {
                UpdateTile updateTile = ToUpdateTiles[i];
                tileMap.UpdateTile(updateTile.XPos, updateTile.YPos, updateTile.Layer);
            }
            ToUpdateTiles.RemoveRange(0, Math.Min(1024 * 32, ToUpdateTiles.Count));
        }
    }
}
