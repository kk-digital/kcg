using TileSpriteLoader;

namespace TileSpriteAtlas
{
    public class TileSpriteAtlasManager
    {
        private TileSpriteAtlas[] SpritesArray;
        private int[] Count;

        public TileSpriteAtlasManager()
        {
            SpritesArray = new TileSpriteAtlas[1];
            Count = new int[1];

            TileSpriteAtlas atlas = new TileSpriteAtlas();
            atlas.Width = 128;
            atlas.Height = 128;
            atlas.Data = new byte[4 * 32 * 32 * atlas.Width * atlas.Height]; // 4 * 32 * 32 = 4096

            SpritesArray[0] = atlas;
        }
        
        public ref TileSpriteAtlas GetSpriteAtlas(int id)
        {
            return ref SpritesArray[id];
        }

        public int GetGlTextureId(int id)
        {
            TileSpriteAtlas atlas = GetSpriteAtlas(id);
            return atlas.GLTextureID;
        }

        public void GetSpriteBytes(int id, byte[] data)
        {
            ref TileSpriteAtlas atlas = ref SpritesArray[0];

            int xOffset = (id % atlas.Width) * 32;
            int yOffset = (id / atlas.Height) * 32;

            for(int y = 0; y < 32; y++)
            {
                for(int x = 0; x < 32; x++)
                {
                    int index = 4 * (x + y * 32);
                    int atlasindex = 4 * ((yOffset + y) * (atlas.Width * 32) +
                                         (xOffset + x));
                    

                    data[index + 0] = atlas.Data[atlasindex + 0];
                    data[index + 1] = atlas.Data[atlasindex + 1];
                    data[index + 2] = atlas.Data[atlasindex + 2];
                    data[index + 3] = atlas.Data[atlasindex + 3];
                }
            }
        }

        // Copies a tile sprite from the Tile sprite sheet
        // to the Tile Sprite Atlas
        // returns an id that can be used later to get texture coordinates
        public int CopyTileSpriteToAtlas(int spriteSheetID, int row, int column, int atlasId)
        {
            SpriteSheet sheet = GameState.TileSpriteLoader.SpriteSheets[spriteSheetID];
            ref TileSpriteAtlas atlas = ref SpritesArray[atlasId];
            ref int count = ref Count[atlasId];
            
            int xOffset = (count % atlas.Width) * 32;
            int yOffset = (count / atlas.Height) * 32;

            for (int y = 0; y < 32; y++)
                for (int x = 0; x < 32; x++)
                {

                    int atlasIndex = 4 * ((yOffset + y)  * (atlas.Width * 32) + (xOffset + x));
                    int sheetIndex = 4 * ((x + row * 32) + ((y + column * 32) * sheet.Width));

                    atlas.Data[atlasIndex + 0] = sheet.Data[sheetIndex + 0];
                    atlas.Data[atlasIndex + 1] = sheet.Data[sheetIndex + 1];
                    atlas.Data[atlasIndex + 2] = sheet.Data[sheetIndex + 2];
                    atlas.Data[atlasIndex + 3] = sheet.Data[sheetIndex + 3];
                }

            // todo: upload texture to open gl

            count++;

            return count - 1;
        }

        // Copies a 16x16 tile sprite from the Tile sprite sheet
        // to the Tile Sprite Atlas
        // returns an id that can be used later to get texture coordinates
         public int CopyTileSpriteToAtlas16To32(int spriteSheetID, int row, int column, int atlasId)
        {
            SpriteSheet sheet = GameState.TileSpriteLoader.SpriteSheets[spriteSheetID];
            ref TileSpriteAtlas atlas = ref SpritesArray[atlasId];
            ref int count = ref Count[atlasId];
            
            int xOffset = (count % atlas.Width) * 32;
            int yOffset = (count / atlas.Height) * 32;

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    int sheetindex = 4 * ((x + row * 16) + ((y + column * 16) * sheet.Width));

                    for (int j = 0; j < 2; j++)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            int atlasIndex = 4 * ((yOffset + (y * 2) + j) * (atlas.Width * 32) + (xOffset + (x * 2) + i));

                            atlas.Data[atlasIndex + 0] = sheet.Data[sheetindex + 0];
                            atlas.Data[atlasIndex + 1] = sheet.Data[sheetindex + 1];
                            atlas.Data[atlasIndex + 2] = sheet.Data[sheetindex + 2];
                            atlas.Data[atlasIndex + 3] = sheet.Data[sheetindex + 3];
                        }
                    }
                }
            }

            // todo: upload texture to open gl

            count++;

            return count - 1;
        }
        

        // Copies a 8x8 tile sprite from the Tile sprite sheet
        // to the Tile Sprite Atlas
        // returns an id that can be used later to get texture coordinates
        public int CopyTileSpriteToAtlas8To32(int spriteSheetID, int row, int column, int atlasId)
        {
            SpriteSheet sheet = GameState.TileSpriteLoader.SpriteSheets[spriteSheetID];
            ref TileSpriteAtlas atlas = ref SpritesArray[atlasId];
            ref int count = ref Count[atlasId];
            
            int xOffset = (count % atlas.Width) * 32;
            int yOffset = (count / atlas.Height) * 32;

            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    int sheetindex = 4 * ((x + row * 8) + ( (y + column * 8) * sheet.Width));

                    for(int j = 0; j < 4; j++)
                    {
                        for(int i = 0; i < 4; i++)
                        {
                            int atlasIndex = 4 * ((yOffset + (y * 4) + j) * (atlas.Width * 32) + (xOffset + (x * 4) + i));
                    
                            atlas.Data[atlasIndex + 0] = sheet.Data[sheetindex + 0];
                            atlas.Data[atlasIndex + 1] = sheet.Data[sheetindex + 1];
                            atlas.Data[atlasIndex + 2] = sheet.Data[sheetindex + 2];
                            atlas.Data[atlasIndex + 3] = sheet.Data[sheetindex + 3];
                        }
                    }

                }

            // todo: upload texture to open gl

            count++;

            return count - 1;
        }
    }
}