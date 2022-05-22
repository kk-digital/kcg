using Enums;
using System.Collections.Generic;
using TileProperties;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace PlanetTileMap
{
    /// <summary> Builds array of sprite quads (2 triangles) for specified layer </summary>
    //TODO: WTF is difference between mesh builder and quad builder?
    class QuadsBuilder
    {
        public int PixelsPerUnit = 100; //NO. 32 pixels = 1.0f
        public int TileWidth = 16;  // Tiles are 32x32 pixels
        public int TileHeight = 16; // Tiles are 32x32 pixel

        public Quad[] BuildQuads(PlanetMapInfo info, PlanetTileLayer layer, float depth)
        {
            var atlas = info.GetAtlas(layer); //WTF is atlas
            if (atlas == null)
                return new Quad[0];

            var texW = atlas.GetLength(0);
            var texH = atlas.GetLength(1);
            var quads = new List<Quad>();

            for (int iCol = 0; iCol < info.Map.Xsize; iCol++)
            for (int iRow = 0; iRow < info.Map.Ysize; iRow++)
            { 
                var tile = info.Map.Tiles[iCol, iRow];
                
                //TODO: Why is there a for loop here?
                //TODO: Tiles mostly have 1 sprite, some tiles have 2
                foreach(var spriteId in GetSpriteIds(tile))
                {
                    if (spriteId == 0)
                        continue;//empty sprite

                    var sprite = info.SpritesById[spriteId];
                    if (sprite.Height == 0)
                        continue;//sprite is not found :(

                    //calc position
                    var k = 1/(float)PixelsPerUnit;
                    var w = sprite.Width * k;//quad width
                    var h = sprite.Height * k;//quad height
                    var x = iCol * TileWidth * k;
                    var y = iRow * TileHeight * k;

                    //calc UVs
                    var u0 = (float)sprite.Left / texW;
                    var u1 = u0 + (float)sprite.Width / texW;
                    var v1 = 1 - (float)sprite.Top / texH;
                    var v0 = v1 - (float)sprite.Height / texH;

                    //create quad
                    var p0 = new Vector3(x, y, depth);
                    var p1 = new Vector3(x + w, y + h, depth);
                    var uv0 = new Vector2(u0, v0);
                    var uv1 = new Vector2(u1, v1);
                    var quad = new Quad(p0, p1, uv0, uv1);
                    quads.Add(quad);
                }
            }

            return quads.ToArray();

            //TODO: Do we even need this?
            //TODO: Move function to TilePropertiesManager or TileSpriteManager
            //TODO: DONT RETURN IEnumerable<int>
            //TODO: return pointer to TileProperty
            IEnumerable<int> GetSpriteIds(PlanetTile tile)
            {
                TileProperties.TileProperties tileProperty;

                switch (layer)
                {
                    case PlanetTileLayer.TileLayerBack: tileProperty = TilePropertiesManager.Instance.TileProperties[tile.BackTileId]; break;
                    case PlanetTileLayer.TileLayerMiddle: tileProperty = TilePropertiesManager.Instance.TileProperties[tile.MidTileId]; break;
                    case PlanetTileLayer.TileLayerFront: tileProperty = TilePropertiesManager.Instance.TileProperties[tile.FrontTileId]; break;
                    case PlanetTileLayer.TileLayerFurniture: tileProperty = TilePropertiesManager.Instance.TileProperties[tile.FurnitureTileId]; break;
                    default: yield break;
                }
                //line 71-74 changed from info.TileProperties to TilePropertiesManager.Instance.TileProperties
                if (tileProperty.SpriteId != 0) yield return tileProperty.SpriteId;
                if (tileProperty.SpriteId2 != 0) yield return tileProperty.SpriteId2;
            }
        }
    }

    /// <summary>Represents sprite quad in mesh</summary>
    struct Quad
    {
        public Vector3 P0;
        public Vector3 P1;
        public Vector2 UV0;
        public Vector2 UV1;

        public Quad(Vector3 p0, Vector3 p1, Vector2 uV0, Vector2 uV1)
        {
            P0 = p0;
            P1 = p1;
            UV0 = uV0;
            UV1 = uV1;
        }
    }
}