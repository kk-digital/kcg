using Enums;
using RectpackSharp;
using System;
using System.Collections.Generic;
using System.Linq;


//TODO: Delete, is doing sprite stuff

namespace PlanetTileMap
{
    //TODO: NO COMMENTS!?!?!?
    //TODO: Delete
    //TODO: Replace with /src/SpriteAtlas
    static class AtlasBuilder
    {
        public static int[,] Build(IList<Sprite> sprites, PlanetTileLayer layer, bool clearTextureInSprite = true)
        {
            //pack sprites into one rect\
            //WTF: Why do we have rectangle? Every tile is 32x32 pixels; their squares
            var rectsList = new List<PackingRectangle>(sprites.Count);
            for (int  i = 0 ; i <  sprites.Count; i++)
            if (sprites[i].Texture != null && sprites[i].Layer == layer)
            { 
                rectsList.Add(new PackingRectangle(0, 0, (uint)sprites[i].Width, (uint)sprites[i].Height, i));
            }

            var rects = rectsList.ToArray();

            RectanglePacker.Pack(rects, out var bounds);
            //round texture size to POT
            var w2 = (int)Math.Pow(2, Math.Ceiling(Math.Log(bounds.Width, 2))); //w2 = 32 always
            var h2 = (int)Math.Pow(2, Math.Ceiling(Math.Log(bounds.Height, 2))); //h2 = 32 always

            //create atlas
            var atlas = new int[w2, h2];
            //copy textures to atlas
            for (int  i = 0 ; i < rects.Length; i++)
            {
                var targetRect = rects[i];
                var sprite = sprites[targetRect.Id];
                CopyRect(sprite.Texture, targetRect);
                sprite.Left = (int)targetRect.X;
                sprite.Top = (int)targetRect.Y;
                if (clearTextureInSprite)
                    sprite.Texture = null;
                sprites[targetRect.Id] = sprite;
            }

            return atlas;

            //TODO: Delete this, should be done by TileSpriteManager
            //TODO: tiles are allways 32x32 pixels and are fixed size, not rects
            void CopyRect(int[,] texture, PackingRectangle targetRect)
            {
                for (int x = 0; x < targetRect.Width; x++)
                for (int y = 0; y < targetRect.Height; y++)
                    atlas[targetRect.X + x,  targetRect.Y + y] = texture[x, y];
            }
        }
    }
}