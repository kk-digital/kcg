using Enums;
using UnityEngine;
using TileSpriteLoader;
using System.Collections;
using System;

namespace SpriteAtlas
{
    class SpriteAtlasManager
    {
        public SpriteAtlas[] Sprites = new SpriteAtlas[0];
        public int Count = 0;

        public ref SpriteAtlas GetSpriteAtlas(int id)
        {
            return ref Sprites[id];
        }

        public int GetGlTextureId(int id)
        {
            SpriteAtlas atlas = GetSpriteAtlas(id);
            return atlas.glTextureID;
        }

        // Returns sprite sheet id
        public int Blit(int SpriteSheetID, int Row, int Column)
        {
            SpriteSheet sheet = TileSpriteLoader.TileSpriteLoader.Instance.SpriteSheets[SpriteSheetID];
            SpriteAtlas atlas = new SpriteAtlas();

            atlas.Data = new byte[4096]; // 4 * 32 * 32 = 4096
            int ScaleFactor = 32 / sheet.SpriteSize;

            for (int y = 0; y < 32; y++)
                for (int x = 0; x < 32; x++)
                {
                    int atlasindex = 4 * (y * 32 + x);
                    int sheetindex = 4 * (x + Column * sheet.SpriteSize + y * sheet.SpriteSize + Row * sheet.SpriteSize * sheet.Width);

                    atlas.Data[atlasindex + 0] = sheet.Data[sheetindex + 0];
                    atlas.Data[atlasindex + 1] = sheet.Data[sheetindex + 1];
                    atlas.Data[atlasindex + 2] = sheet.Data[sheetindex + 2];
                    atlas.Data[atlasindex + 3] = sheet.Data[sheetindex + 3];
                }

            // todo: upload texture to open gl

            atlas.id = Count++;
            Array.Resize(ref Sprites, Count);

            return Count - 1;
        }
    }
}
