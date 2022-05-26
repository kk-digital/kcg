using TileSpriteLoader;
using System.Numerics;
using System.Collections.Generic;

namespace SpriteAtlas
{
    public struct AtlasElement
    {
        public int OffsetX;
        public int OffsetY;
        public int SizeX;
        public int SizeY;

        public AtlasElement(int offsetX, int offsetY, int sizeX, int sizeY)
        {
            OffsetX = offsetX;
            OffsetY = offsetY;
            SizeX = sizeX;
            SizeY = sizeY;
        }
    }

    public struct SpriteAtlas
    {
        public int AtlasID;
        public int GLTextureID;

        public int Width;
        public int Height;

        public byte[] Data;
        public List<AtlasElement> Elements;
    }

    
    public class SpriteAtlasManager
    {
        private SpriteAtlas[] SpritesArray;

        public SpriteAtlasManager()
        {
            SpritesArray = new SpriteAtlas[1];

            SpriteAtlas atlas = new SpriteAtlas();
            atlas.Width = 128;
            atlas.Height = 128;
            atlas.Data = new byte[4 * 32 * 32 * atlas.Width * atlas.Height]; // 4 * 32 * 32 = 4096
            atlas.Elements = new List<AtlasElement>();

            SpritesArray[0] = atlas;
        }
        
        public ref SpriteAtlas GetSpriteAtlas(int id)
        {
            return ref SpritesArray[id];
        }

        public int GetGlTextureId(int id)
        {
            SpriteAtlas atlas = GetSpriteAtlas(id);
            return atlas.GLTextureID;
        }

        public void GetSpriteBytes(int id, byte[] data)
        {
            ref SpriteAtlas atlas = ref SpritesArray[0];
            if (id >= 0 && id < atlas.Elements.Count)
            {
                AtlasElement element = atlas.Elements[id];

                int xOffset = element.OffsetX;
                int yOffset = element.OffsetY;

                for(int y = 0; y < element.SizeY; y++)
                {
                    for(int x = 0; x < element.SizeX; x++)
                    {
                        int index = 4 * (x + y * element.SizeX);
                        int atlasindex = 4 * ((yOffset + y) * (atlas.Width * element.SizeX) +
                                            (xOffset + x));
                        

                        data[index + 0] = atlas.Data[atlasindex + 0];
                        data[index + 1] = atlas.Data[atlasindex + 1];
                        data[index + 2] = atlas.Data[atlasindex + 2];
                        data[index + 3] = atlas.Data[atlasindex + 3];
                    }
                }
            }
        }

        public int Blit(int spriteSheetID, int row, int column, int w, int h)
        {
           // Note(Mahdi) : Not Implemented


           // used packed rectangle to blit somehow

           return 0;
        }
    }
}