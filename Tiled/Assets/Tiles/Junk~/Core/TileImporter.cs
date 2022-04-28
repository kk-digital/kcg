using System.IO;
using Tiles.Utils;
using UnityEngine;

namespace Tiles.Junk
{
    public static class TileImporter
    {
        public static string BaseDir => Application.streamingAssetsPath;

        public static void LoadTileTypes()
        {
            Assets.TileTypes.Clear();

            var path = Path.Combine(BaseDir, "assets/tile/");
            foreach (var file in Directory.GetFiles(path, "*.yaml",SearchOption.AllDirectories))
            {
                var yaml = YamlParser.Parse(file);
                foreach (var tileConfig in yaml.Children)
                {
                    var tileType = new TileType();
                    tileType.Name = tileConfig.Name;

                    Assets.RegisterTileType(tileType);
                }
            }
        }
    }
}