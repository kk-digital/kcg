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

            //calc bounds
            var mapBounds = CalcBounds(map);
            var mapWidth = mapBounds.maxX - mapBounds.minX + 1;
            var mapHeight = mapBounds.maxY - mapBounds.minY + 1;
            var planetMap = res.Map = new PlanetMap(mapWidth, mapHeight);

            //temp array to collect info about tiles
            var tileInfos = new PlanetTileInfo[planetMap.Xsize, planetMap.Ysize];

            //load layers
            foreach (var layer in map.Layers)
            { 
                //get PlanetTileLayer
                var planetLayer = GetPlanetTileLayer(layer);
                if (planetLayer == PlanetTileLayer.Unknown)
                    continue; //do not import layers with unknown PlanetTileLayer

                //enumerate layer chunks
                if (layer.chunks != null)
                foreach (var chunk in layer.chunks)
                { 
                    //enumerate spriteId of layer
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

                        //save spriteId into temp array
                        switch (planetLayer)
                        {
                            case PlanetTileLayer.Back: 
                                if (tileInfos[x, y].BackSpriteId == 0) 
                                    tileInfos[x, y].BackSpriteId = spriteId; else tileInfos[x, y].SecondaryBackSpriteId = spriteId;
                                break;
                            case PlanetTileLayer.Front:
                                 if (tileInfos[x, y].FrontSpriteId == 0) 
                                    tileInfos[x, y].FrontSpriteId = spriteId; else tileInfos[x, y].SecondaryFrontSpriteId = spriteId;
                                break;
                            case PlanetTileLayer.Middle:
                                 if (tileInfos[x, y].MidSpriteId == 0) 
                                    tileInfos[x, y].MidSpriteId = spriteId; else tileInfos[x, y].SecondaryMidSpriteId = spriteId;
                                break;
                            case PlanetTileLayer.Furniture:
                                 if (tileInfos[x, y].FurnitureSpriteId == 0) 
                                    tileInfos[x, y].FurnitureSpriteId = spriteId; else tileInfos[x, y].SecondaryFurnitureSpriteId = spriteId;
                                break;
                        }
                    }
                }
            }

            //now look in tile info array and generate TileProperty for each unique combinations of primary and secondary spriteId
            var spriteIdsToTilePropertyId = new Dictionary<(int, int), int>();
            var tileProperties = new List<PlanetTileProperties>();

            Generate(PlanetTileLayer.Back);
            Generate(PlanetTileLayer.Middle);
            Generate(PlanetTileLayer.Front);
            Generate(PlanetTileLayer.Furniture);

            res.TileProperties = tileProperties.ToArray();

            //build atlases
            res.SetAtlas(PlanetTileLayer.Back, AtlasBuilder.Build(spritesById, PlanetTileLayer.Back));
            res.SetAtlas(PlanetTileLayer.Middle, AtlasBuilder.Build(spritesById, PlanetTileLayer.Middle));
            res.SetAtlas(PlanetTileLayer.Front, AtlasBuilder.Build(spritesById, PlanetTileLayer.Front));
            res.SetAtlas(PlanetTileLayer.Furniture, AtlasBuilder.Build(spritesById, PlanetTileLayer.Furniture));

            return res;

            void Generate(PlanetTileLayer layer)
            {
                spriteIdsToTilePropertyId.Clear();

                for (int x = 0; x < planetMap.Xsize; x++)
                for (int y = 0; y < planetMap.Ysize; y++)
                {
                    var tileInfo = tileInfos[x, y];
                    var key = (0, 0);
                    switch (layer)
                    {
                        case PlanetTileLayer.Back: key = (tileInfo.BackSpriteId, tileInfo.SecondaryBackSpriteId); break;
                        case PlanetTileLayer.Middle: key = (tileInfo.MidSpriteId, tileInfo.SecondaryMidSpriteId); break;
                        case PlanetTileLayer.Front: key = (tileInfo.FrontSpriteId, tileInfo.SecondaryFrontSpriteId); break;
                        case PlanetTileLayer.Furniture: key = (tileInfo.FurnitureSpriteId, tileInfo.SecondaryFurnitureSpriteId); break;
                    }

                    //get tileProperty index or create new tileProperty
                    if (!spriteIdsToTilePropertyId.TryGetValue(key, out var tilePropertyId))
                    {
                        tilePropertyId = tileProperties.Count;
                        var tileProperty = new PlanetTileProperties(){Layer = layer, SpriteId = key.Item1, SecondarySpriteId = key.Item2};
                        tileProperties.Add(tileProperty);
                    }

                    //assign layer to sprite
                    if (key.Item1 != 0) spritesById[key.Item1].Layer = layer;
                    if (key.Item2 != 0) spritesById[key.Item2].Layer = layer;

                    //assign tile property index to tile in planetMap.Tiles
                    switch (layer)
                    {
                        case PlanetTileLayer.Back: planetMap.Tiles[x, y].BackTileId = tilePropertyId; break;
                        case PlanetTileLayer.Middle: planetMap.Tiles[x, y].MidTileId = tilePropertyId; break;
                        case PlanetTileLayer.Front: planetMap.Tiles[x, y].FrontTileId = tilePropertyId; break;
                        case PlanetTileLayer.Furniture: planetMap.Tiles[x, y].FurnitureTileId = tilePropertyId; break;
                    }
                }
            }
        }

        /// <summary> Temp struct to collect info about tile </summary>
        private struct PlanetTileInfo
        {
            //Back tile
            public int BackSpriteId;
            public int SecondaryBackSpriteId;
            public int BackTileId;

            //Mid tile
            public int MidSpriteId;
            public int SecondaryMidSpriteId;
            public int MidTileId;

            //Front tile
            public int FrontSpriteId;
            public int SecondaryFrontSpriteId;
            public int FrontTileId;
        
            //Furniture
            public int FurnitureSpriteId;
            public int SecondaryFurnitureSpriteId;
            public int FurnitureTileId;
            public sbyte FurnitureOffsetX;
            public sbyte FurnitureOffsetY;

            //Health
            public byte Durability;
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