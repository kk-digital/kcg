using System;
using UnityEngine;

namespace Planet
{
    public struct Layers
    {
        public static readonly int Count = Enum.GetNames(typeof(Enums.Tile.MapLayerType)).Length;
        
        public Vector2Int MapSize;
        public Texture2D[] LayerTextures;
        
        public void DrawLayer(Enums.Tile.MapLayerType planetLayer, Material material, Transform transform, int DrawOrder)
        {
            var sprite = new Sprites.Sprite(LayerTextures[(int) planetLayer]);

            Utility.Render.DrawSprite(0, 0, 1.0f * MapSize.x, 1.0f * MapSize.y, sprite, material, transform, DrawOrder);
        }
        
        public void BuildLayerTexture(TileMap tileMap, Enums.Tile.MapLayerType planetLayer)
        {
            byte[] bytes = new byte[32 * 32 * 4];
            byte[] data = new byte[MapSize.x * MapSize.y * 32 * 32 * 4];

            for(int y = 0; y < MapSize.y; y++)
            {
                for(int x = 0; x < MapSize.x; x++)
                {
                    ref Tile.Tile tile = ref tileMap.GetTileRef(x, y, planetLayer);

                    int spriteId = tile.SpriteId;

                    if (spriteId >= 0)
                    {
                        
                        GameState.TileSpriteAtlasManager.GetSpriteBytes(spriteId, bytes);

                        int tileX = (x * 32);
                        int tileY = (y * 32);

                        for (int j = 0; j <  32; j++)
                        {
                            for(int i = 0; i < 32; i++)
                            {
                                int index = 4 * (((i + tileX)) + ((j + tileY)) * (MapSize.x * 32));
                                int bytesIndex = 4 * (i + (32 - j - 1) * 32);
                                data[index] = 
                                    bytes[bytesIndex];
                                data[index + 1] = 
                                    bytes[bytesIndex + 1];
                                data[index + 2] = 
                                    bytes[bytesIndex + 2];
                                data[index + 3] = 
                                    bytes[bytesIndex + 3];
                            }
                        }
                    }
                }
            }
            
            LayerTextures[(int)planetLayer] = Utility.Texture.CreateTextureFromRGBA(data, MapSize.x * 32, MapSize.y * 32);
        }
    }
}

