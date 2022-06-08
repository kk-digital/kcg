using System.Drawing;
using Enums;
using UnityEngine;

namespace Planet.TileMap
{
    public struct Layers
    {
        public Vector2Int MapSize;
        public Texture2D[] LayerTextures;
        
        public void DrawLayer(PlanetLayer planetLayer, Material material, Transform transform, int DrawOrder)
        {
            Render.Sprite sprite = new Render.Sprite();
            sprite.Texture = LayerTextures[(int)planetLayer];
            sprite.TextureCoords = new Vector4(0, 0, 1, -1);

            Utility.RenderUtils.DrawSprite(0, 0, 1.0f * MapSize.x, 1.0f * MapSize.y, sprite, material, transform, DrawOrder);
        }
        
        public void BuildLayerTexture(ref Model tileMap, PlanetLayer planetLayer)
        {
            byte[] Bytes = new byte[32 * 32 * 4];
            byte[] Data = new byte[MapSize.x * MapSize.y * 32 * 32 * 4];

            for(int y = 0; y < MapSize.y; y++)
            {
                for(int x = 0; x < MapSize.x; x++)
                {
                    ref Tile.Model tile = ref tileMap.GetTileRef(x, y, planetLayer);

                    int spriteId = tile.SpriteId;

                    if (spriteId >= 0)
                    {
                        
                        GameState.TileSpriteAtlasManager.GetSpriteBytes(spriteId, Bytes);

                        int tileX = (x * 32);
                        int tileY = (y * 32);

                        for (int j = 0; j <  32; j++)
                        {
                            for(int i = 0; i < 32; i++)
                            {
                                int index = 4 * (((i + tileX)) + ((j + tileY)) * (MapSize.x * 32));
                                int bytesIndex = 4 * (i + (32 - j - 1) * 32);
                                Data[index] = 
                                    Bytes[bytesIndex];
                                Data[index + 1] = 
                                    Bytes[bytesIndex + 1];
                                Data[index + 2] = 
                                    Bytes[bytesIndex + 2];
                                Data[index + 3] = 
                                    Bytes[bytesIndex + 3];
                            }
                        }
                    }
                }
            }
            
            LayerTextures[(int)planetLayer] = Utility.TextureUtils.CreateTextureFromRGBA(Data, MapSize.x * 32, MapSize.y * 32);
        }
    }
}

