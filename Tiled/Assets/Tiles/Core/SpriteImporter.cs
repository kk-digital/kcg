using BigGustave;
using System.Collections.Generic;
using System.IO;
using Tiles.Utils;
using UnityEngine;

namespace Tiles
{
    public static class SpriteImporter
    {
        public static string BaseDir => Application.streamingAssetsPath;

        public static int DefaultCellWidth = 16;
        public static int DefaultCellHeight = 16;

        public static void LoadSprites()
        {
            //List<>
            var path = Path.Combine(BaseDir, "assets/sprite/");
            foreach (var file in Directory.GetFiles(path, "*.yaml",SearchOption.AllDirectories))
            {
                var node = YamlParser.Parse(file);
                //load img
                var imgPath = Path.ChangeExtension(file, ".png");
                var png = Png.Open(imgPath);

                //create sprites
                if(node["sprites"] != null)
                { 
                    foreach (var spriteNode in node["sprites"].Children)
                    {
                        var sprite = new Sprite();
                        sprite.Name = spriteNode.Name;
                        Assets.RegisterSprite(sprite);   
                    }
                    continue;
                }

                if (node["autoname"]?.ValueAsString == "index")
                {
                    var cw = CellWidth(node);
                    var ch = CellHeight(node);

                    var x = 0;
                    var y = 0;

                    for (int i=0; y < png.Height; i++)
                    {
                        var sprite = new Sprite();
                        sprite.Top = y;
                        sprite.Left = x;
                        sprite.Name = $"{node.Name}:{i}";
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

        int[,] PngToRGBA(Png png)
        {
            var res = new int[png.Width, png.Height];
            png.GetPixel
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