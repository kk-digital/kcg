using RectpackSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tiles
{
    static class AtlasBuilder
    {
        public static int[,] Build(IList<Sprite> sprites, bool replaceSpritesInAssets = true, bool clearTextureInSprite = true)
        {
            //pack sprites into one rect
            var rects = new PackingRectangle[sprites.Count];
            for (int  i = 0 ; i <  sprites.Count; i++)
                rects[i] = new PackingRectangle(0, 0, (uint)sprites[i].Width, (uint)sprites[i].Height, i);
            
            RectanglePacker.Pack(rects, out var bounds);
            //round texture size to POT
            var w2 = (int)Math.Pow(2, Math.Ceiling(Math.Log(bounds.Width, 2)));
            var h2 = (int)Math.Pow(2, Math.Ceiling(Math.Log(bounds.Height, 2)));

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
            }

            //clear texture in sprite
            if (clearTextureInSprite)
            for (int  i = 0; i < sprites.Count; i++)
            {
                var s = sprites[i];
                s.Texture = null;
                sprites[i] = s;
            }

            //replace sprite in Assets
            if (replaceSpritesInAssets)
            foreach (var sprite in sprites)
                Assets.Sprites[sprite.Name] = sprite;

            return atlas;

            void CopyRect(int[,] texture, PackingRectangle targetRect)
            {
                for (int x = 0; x < targetRect.Width; x++)
                for (int y = 0; y < targetRect.Height; y++)
                    atlas[targetRect.X + x,  targetRect.Y + y] = texture[x, y];
            }
        }
    }
}