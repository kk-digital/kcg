using BigGustave;
using Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TiledCS;
using TileProperties;
//Todo: Remove unity dependency
using PlanetTileMap;
using SpriteAtlas;

//Note:
//TMX files are created by map editor "Tiled"
//Tiled can be downloaded at mapeditor.org
//Tiled saves maps in TMX format

namespace TmxMapFileLoader
{
    static class TmxImporter
    {
        //Todo: Break into stages
        //First stage, iterates over map and gets all sprite sheets used (loads them)
        //with ImageAssetManager
        //Second stage, iterates over map and gets all sprites/tiles used
        //register the tiles
        //Third stage copies the map data into the internal format
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
            var idToSprite = new Deprecate_Sprite[maxGid + 1];

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

        //Tile Properties should be set in an earlier stage

        //TODO: Make it return a PlanetTileMap, not a PlanetMapInfo


        //TODO: Move sprite/image loading operations from ConvertToInternalStructures
        public static void LoadMapSpritesFromMapFile(TiledMap map, Deprecate_Sprite[] spritesById)
        {
            //
            return;
        }

        public static PlanetMapInfo ConvertToInternalStructures(TiledMap map, Deprecate_Sprite[] spritesById)
        {
            var PlanetMap = new PlanetMapInfo();
            PlanetMap.SpritesById = spritesById;

            //calc bounds
            var mapBounds = CalcBounds(map);
            var mapWidth = mapBounds.maxX - mapBounds.minX + 1;
            var mapHeight = mapBounds.maxY - mapBounds.minY + 1;
            
            //A grid of tiles
            PlanetMap.Map = new PlanetTileMap.PlanetTileMap(mapWidth, mapHeight);
            //var planetMap = new PlanetTileMap.PlanetTileMap(mapWidth, mapHeight);

            //temp array to collect info about tiles

            //TODO: Replace wth PlanetMap
            var tileInfos = new TileProperties.TileProperties[PlanetMap.Map.Xsize, PlanetMap.Map.Ysize];

            //load layers
            foreach (var layer in map.Layers)
            { 
                //get PlanetTileLayer
                var planetLayer = GetPlanetTileLayer(layer);
                if (planetLayer == PlanetTileLayer.TileLayerError)
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
                        tileInfos[x, y].Layer = planetLayer;  
                        if (tileInfos[x, y].SpriteId == 0)
                        {
                            tileInfos[x, y].SpriteId = spriteId;
                        }
                        else
                        {
                            tileInfos[x, y].SpriteId2 = spriteId;
                        }
                    }
                }
            }
            //TODO: The Tile Properties should be set earlier, before this stage
            //now look in tile info array and generate TileProperty for each unique combinations of primary and secondary spriteId
            var spriteIdsToTilePropertyId = new Dictionary<(int, int), int>();
           
            //TODO: Fix
            var tileProperties = new List<TileProperties.TileProperties>();

            Generate(PlanetTileLayer.TileLayerBack);
            Generate(PlanetTileLayer.TileLayerMiddle);
            Generate(PlanetTileLayer.TileLayerFront);
            Generate(PlanetTileLayer.TileLayerFurniture);

            TilePropertiesManager.Instance.TileProperties = tileProperties.ToArray(); //changed from res.TileProperties to use singleton

            //build atlases
            PlanetMap.SetAtlas(PlanetTileLayer.TileLayerBack, SpriteAtlasBuilder.Build(spritesById, PlanetTileLayer.TileLayerBack));
            PlanetMap.SetAtlas(PlanetTileLayer.TileLayerMiddle, SpriteAtlasBuilder.Build(spritesById, PlanetTileLayer.TileLayerMiddle));
            PlanetMap.SetAtlas(PlanetTileLayer.TileLayerFront, SpriteAtlasBuilder.Build(spritesById, PlanetTileLayer.TileLayerFront));
            PlanetMap.SetAtlas(PlanetTileLayer.TileLayerFurniture, SpriteAtlasBuilder.Build(spritesById, PlanetTileLayer.TileLayerFurniture));

            return PlanetMap;

            //TODO: What does this function actually do?
            void Generate(PlanetTileLayer layer)
            {
                spriteIdsToTilePropertyId.Clear();

                for (int x = 0; x < PlanetMap.Map.Xsize; x++)
                for (int y = 0; y < PlanetMap.Map.Ysize; y++)
                {
                    var tileInfo = tileInfos[x, y];
                    var key = (0, 0);
                    if (tileInfo.Layer == layer)
                    {
                        key = (tileInfo.SpriteId, tileInfo.SpriteId2);
                    }

                    //get tileProperty index or create new tileProperty
                    if (!spriteIdsToTilePropertyId.TryGetValue(key, out var tilePropertyId))
                    {
                        //move to global
                        tilePropertyId = tileProperties.Count;
                        var tileProperty = new TileProperties.TileProperties(){ Layer = layer, SpriteId = key.Item1, SpriteId2 = key.Item2};
                        tileProperties.Add(tileProperty);
                        int tilePropertiesLength = TilePropertiesManager.Instance.TileProperties.Length;
                        Array.Resize(ref TilePropertiesManager.Instance.TileProperties, 
                                         tilePropertiesLength != 1 ? tilePropertiesLength + 1 :
                                         tilePropertiesLength);
                        TilePropertiesManager.Instance.TileProperties[tilePropertiesLength - 1] = tileProperty;
                    }

                    //assign layer to sprite
                    if (key.Item1 != 0) spritesById[key.Item1].Layer = layer;
                    if (key.Item2 != 0) spritesById[key.Item2].Layer = layer;

                    //assign tile property index to tile in planetMap.Tiles
                    switch (layer)
                    {
                        case PlanetTileLayer.TileLayerBack: PlanetMap.Map.Tiles[x, y].BackTileId = tilePropertyId; break;
                        case PlanetTileLayer.TileLayerMiddle: PlanetMap.Map.Tiles[x, y].MidTileId = tilePropertyId; break;
                        case PlanetTileLayer.TileLayerFront: PlanetMap.Map.Tiles[x, y].FrontTileId = tilePropertyId; break;
                        case PlanetTileLayer.TileLayerFurniture: PlanetMap.Map.Tiles[x, y].FurnitureTileId = tilePropertyId; break;
                    }
                }
            }
        }

/*
       DELETE
        /// <summary> Temp struct to collect info about tile </summary>
        
        //TODO: Delete and replace
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
*/

        private static PlanetTileLayer GetPlanetTileLayer(TiledLayer layer)
        {
            if (layer.properties == null) return PlanetTileLayer.TileLayerError;

            var layerProperty = layer.properties.FirstOrDefault(p => p.name == "Layer");

            switch (layerProperty?.value)
            {
                case "Front" : return PlanetTileLayer.TileLayerFront;
                case "Back" : return PlanetTileLayer.TileLayerBack;
                case "Middle" : return PlanetTileLayer.TileLayerMiddle;
                case "Furniture" : return PlanetTileLayer.TileLayerFurniture;
            }

            return PlanetTileLayer.TileLayerError;
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

        //DELETE AMD USE OUR LOADER
        private static void LoadSprite(string dir, int startGid, Deprecate_Sprite[] gidToSprite, TiledMapTileset link, TiledTileset ts)
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
                var sprite = new Deprecate_Sprite { Width = ts.TileWidth, Height = ts.TileHeight, Left = x, Top = y };
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