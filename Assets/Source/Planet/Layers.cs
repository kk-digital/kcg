using System;
using KMath;
using UnityEngine;

namespace Planet
{
    public struct Layers
    {
        public static readonly int Count = Enum.GetNames(typeof(Enums.Tile.MapLayerType)).Length;
        
        public Vec2i MapSize;
        public Texture2D[] LayerTextures;
        public Tile.Tile[][] Tiles;
        public bool[] NeedsUpdate;
        
        public void DrawLayer(TileMap tileMap, Enums.Tile.MapLayerType planetLayer, Material material, Transform transform, int DrawOrder)
        {

            BuildLayerTexture(tileMap, planetLayer);

            var sprite = new Sprites.Sprite(LayerTextures[(int) planetLayer]);

            Utility.Render.DrawSprite(0, 0, 1.0f * MapSize.X, 1.0f * MapSize.Y, sprite, material, transform, DrawOrder);
        }
        
        private void BuildLayerTexture(TileMap tileMap, Enums.Tile.MapLayerType planetLayer)
        {
            if (NeedsUpdate[(int)planetLayer])
            {

                NeedsUpdate[(int)planetLayer] = false;

                byte[] bytes = new byte[32 * 32 * 4];
                byte[] data = new byte[MapSize.X * MapSize.Y * 32 * 32 * 4];

                for(int y = 0; y < MapSize.Y; y++)
                {
                    for(int x = 0; x < MapSize.X; x++)
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
                                    int index = 4 * (((i + tileX)) + ((j + tileY)) * (MapSize.X * 32));
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
                
                LayerTextures[(int)planetLayer] = Utility.Texture.CreateTextureFromRGBA(data, MapSize.X * 32, MapSize.Y * 32);
            }
        }
    }
}

