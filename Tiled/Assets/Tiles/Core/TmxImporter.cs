using BigGustave;
using System;
using System.Collections.Generic;
using System.IO;
using TiledCS;

namespace Tiles
{
    public static class TmxImporter
    {
        public static void LoadMap(string filePath)
        {
            var map = Assets.Map = new TiledMap(filePath);
            var dir = Path.GetDirectoryName(filePath);
            var tileSets = new List<TiledTileset>();

            foreach (var ts in map.Tilesets)
                tileSets.Add(new TiledTileset(Path.Combine(dir, ts.source)));
                
            //calc max gid
            var maxGid = -1;
            for (int  i= 0 ; i < map.Tilesets.Length; i++)
            {
                var gid = map.Tilesets[i].firstgid + tileSets[i].TileCount - 1;
                if (gid > maxGid)
                    maxGid = gid;
            }

            //create sprite array
            var gidToSprite = Assets.SpritesById = new Sprite[maxGid + 1];

            //create sprites
            for (int  iTileSet = 0 ; iTileSet < map.Tilesets.Length; iTileSet++)
            {
                var link = map.Tilesets[iTileSet];
                var ts = tileSets[iTileSet];
                
                if (ts.TileCount == 0 || ts.TileWidth == 0 || ts.TileHeight == 0)
                    continue;

                if (ts.Tiles != null)
                foreach(var t in ts.Tiles)
                if (t.image != null)
                {
                    //make fictive tile for image
                    var tile = new TiledTileset();
                    tile.Image = t.image;
                    tile.Columns = 1;
                    tile.TileCount = 1;
                    tile.TileHeight = t.image.height;
                    tile.TileWidth = t.image.width;

                    LoadSprite(dir, link.firstgid + t.id, gidToSprite, link, tile);
                }

                if (ts.Image != null)
                    LoadSprite(dir, link.firstgid, gidToSprite, link, ts);
            }

            //build atlas
            Assets.AtlasTexture = AtlasBuilder.Build(gidToSprite);
        }

        private static void LoadSprite(string dir, int startGid, Sprite[] gidToSprite, TiledMapTileset link, TiledTileset ts)
        {
            var tsxDir = Path.GetDirectoryName(Path.Combine(dir, link.source));
            var imgPath = Path.Combine(tsxDir, ts.Image.source);
            var png = Png.Open(imgPath);

            for (int i = 0; i < ts.TileCount; i++)
            {
                var gid = startGid + i;
                var iColumn = i % ts.Columns;
                var iRow = i / ts.Columns;
                var x = ts.Margin + iColumn * ts.TileWidth + Math.Max(0, iColumn - 1) * ts.Spacing;
                var y = ts.Margin + iRow * ts.TileHeight + Math.Max(0, iRow - 1) * ts.Spacing;
                var sprite = new Sprite { Width = ts.TileWidth, Height = ts.TileHeight, Left = x, Top = y };
                sprite.Texture = PngToRGBA(png, x, y, sprite.Width, sprite.Height);

                gidToSprite[gid] = sprite;
            }
        }

        static int[,] PngToRGBA(Png png, int left, int top, int w, int h)
        {
            var res = new int[w, h];
            for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                res[x, y] = png.GetPixel(x + left, y + top).ToRGBA();
            return res;
        }
    }
}