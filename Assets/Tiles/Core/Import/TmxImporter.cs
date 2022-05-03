using BigGustave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TiledCS;
using BoundsInt = UnityEngine.BoundsInt;

namespace Tiles
{
    static class TmxImporter
    {
        public static PlanetMapInfo LoadMap(string filePath)
        {
            var map = new TiledMap(filePath);
            var dir = Path.GetDirectoryName(filePath);
            var tileSets = new List<TiledTileset>();

            //load tile layers
            foreach (var ts in map.Tilesets)
                tileSets.Add(new TiledTileset(Path.Combine(dir, ts.source)));

            //calc max gid (sprite id)
            var maxGid = -1;
            for (int i = 0; i < map.Tilesets.Length; i++)
            {
                var gid = map.Tilesets[i].firstgid + tileSets[i].TileCount - 1;
                if (gid > maxGid)
                    maxGid = gid;
            }

            //create sprite array
            var idToSprite = new Sprite[maxGid + 1];

            //create sprites
            for (int iTileSet = 0; iTileSet < map.Tilesets.Length; iTileSet++)
            {
                var link = map.Tilesets[iTileSet];
                var ts = tileSets[iTileSet];

                if (ts.TileCount == 0 || ts.TileWidth == 0 || ts.TileHeight == 0)
                    continue;

                if (ts.Tiles != null)
                foreach (var t in ts.Tiles)
                if (t.image != null)
                {
                    //make fictive tile for image
                    var tile = new TiledTileset();
                    tile.Image = t.image;
                    tile.Columns = 1;
                    tile.TileCount = 1;
                    tile.TileHeight = t.image.height;
                    tile.TileWidth = t.image.width;

                    LoadSprite(dir, link.firstgid + t.id, idToSprite, link, tile);
                }

                if (ts.Image != null)
                    LoadSprite(dir, link.firstgid, idToSprite, link, ts);
            }

            //convert to internal structures
            return ConvertToInternalStructures(map, idToSprite);
        }

        private static PlanetMapInfo ConvertToInternalStructures(TiledMap map, Sprite[] spritesById)
        {
            var res = new PlanetMapInfo();
            res.SpritesById = spritesById;

            //build atlas
            res.AtlasTexture = AtlasBuilder.Build(spritesById);

            //calc bounds
            var mapBounds = CalcBounds(map);
            var mapWidth = mapBounds.maxX - mapBounds.minX + 1;
            var mapHeight = mapBounds.maxY - mapBounds.minY + 1;
            var planetMap = res.Map = new PlanetMap(mapWidth, mapHeight);
            var tiles = planetMap.Tiles;

            //load layers
            foreach (var layer in map.Layers)
            { 
                //get PlanetTileLayer
                var planetLayer = GetPlanetTileLayer(layer);
                if (planetLayer == PlanetTileLayer.Unknown)
                    continue; //do not import layers with unknown PlanetTileLayer

                //convert chunks of layer
                if (layer.chunks != null)
                foreach (var chunk in layer.chunks)
                { 
                    for (int i = 0; i < chunk.data.Length; i++)
                    {
                        var spriteId = chunk.data[i];
                        if (spriteId == 0)
                            continue;//empty sprite

                        var sprite = spritesById[spriteId];
                        if (sprite.Height == 0)
                            continue;//sprite is not found :(

                        //calc position
                        var iCol = i % chunk.width;
                        var iRow = i / chunk.width;
                        var x = (chunk.x + iCol) - mapBounds.minX;
                        var y = mapBounds.maxY - (chunk.y + iRow);

                        switch (planetLayer)
                        {
                            case PlanetTileLayer.Back: tiles[x, y].BackSpriteId = spriteId; break;
                            case PlanetTileLayer.Front: tiles[x, y].FrontSpriteId = spriteId; break;
                            case PlanetTileLayer.Middle: tiles[x, y].MidSpriteId = spriteId; break;
                            case PlanetTileLayer.Furniture: tiles[x, y].FurnitureSpriteId = spriteId; break;
                        }
                    }
                }
            }

            return res;
        }

        private static PlanetTileLayer GetPlanetTileLayer(TiledLayer layer)
        {
            if (layer.properties == null) return PlanetTileLayer.Unknown;

            var layerProperty = layer.properties.FirstOrDefault(p => p.name == "Layer");

            switch (layerProperty?.value)
            {
                case "Front" : return PlanetTileLayer.Front;
                case "Back" : return PlanetTileLayer.Back;
                case "Middle" : return PlanetTileLayer.Middle;
                case "Furniture" : return PlanetTileLayer.Furniture;
            }

            return PlanetTileLayer.Unknown;
        }

        private static (int minX, int minY, int maxX, int maxY) CalcBounds(TiledMap map)
        {
            int minX = 0;
            int minY = 0;
            int maxX = 0;
            int maxY = 0;

            foreach (var layer in map.Layers)
            if (layer.chunks != null)
            foreach (var chunk in layer.chunks)
            { 
                if(chunk.x < minX) minX = chunk.x;
                if(chunk.y < minY) minY = chunk.y;
                if(chunk.x + chunk.width > maxX) maxX = chunk.x + chunk.width;
                if(chunk.y + chunk.height > maxY) maxY = chunk.y + chunk.height;
            }

            return (minX, minY, maxX, maxY);
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