using BigGustave;
using System.IO;
using Tiles.Utils;
using UnityEngine;

namespace Tiles.Junk
{
    public static class SpriteImporter
    {
        public static string BaseDir => Application.streamingAssetsPath;

        public static int DefaultCellWidth = 16;
        public static int DefaultCellHeight = 16;

        public static void LoadSprites()
        {
            Assets.Sprites.Clear();

            var path = Path.Combine(BaseDir, "assets/sprite/");
            foreach (var file in Directory.GetFiles(path, "*.yaml",SearchOption.AllDirectories))
            {
                var node = YamlParser.Parse(file);
                //load img
                var imgPath = Path.ChangeExtension(file, ".png");
                var png = Png.Open(imgPath);

                var cw = CellWidth(node);
                var ch = CellHeight(node);

                //create sprites
                if(node["sprites"] != null)
                { 
                    foreach (var spriteNode in node["sprites"].Children)
                    {
                        var sprite = new Sprite();
                        sprite.Name = spriteNode.Name;
                        sprite.Left = spriteNode.GetInt("left", 0) * cw;
                        sprite.Top = spriteNode.GetInt("top", 0) * ch;
                        sprite.Width = spriteNode.GetInt("width", 1) * cw;
                        sprite.Height = spriteNode.GetInt("height", 1) * ch;
                        sprite.Texture = PngToRGBA(png, sprite.Left, sprite.Top, sprite.Width, sprite.Height);

                        Assets.RegisterSprite(sprite);  
                    }
                    continue;
                }

                if (node["autoname"]?.ValueAsString == "index")
                {
                    var x = 0;
                    var y = 0;

                    for (int i=0; y < png.Height; i++)
                    {
                        var sprite = new Sprite();
                        sprite.Top = y;
                        sprite.Left = x;
                        sprite.Width = cw;
                        sprite.Height = ch;
                        sprite.Name = $"{node["name"].ValueAsString}:{i}";
                        sprite.Texture = PngToRGBA(png, sprite.Left, sprite.Top, cw, ch);
                        Assets.RegisterSprite(sprite);

                        x += cw;
                        if (x >= png.Width)
                        {
                            x = 0; y += ch;
                        }
                    }
                }
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

        static int CellWidth(YamlNode node)
        {
            return node["cellwidth"] == null ? DefaultCellWidth : node["cellwidth"].ValueAsInt;
        }

        static int CellHeight(YamlNode node)
        {
            return node["cellheight"] == null ? DefaultCellHeight : node["cellheight"].ValueAsInt;
        }
    }
}