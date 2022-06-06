using System;
using Enums;
using UnityEngine;

namespace TileMap
{
    public class GenerateSystem
    {
        public static readonly GenerateSystem Instance;
        public readonly GameContext Context;

        static GenerateSystem()
        {
            Instance = new GenerateSystem();
        }

        private GenerateSystem()
        {
            Context = Contexts.sharedInstance.game;
        }

        private GameEntity CreateTileMap(Vector2Int chunkSize)
        {
            var entity = Context.CreateEntity();

            int layersCount = Enum.GetNames(typeof(PlanetLayer)).Length;
            
            entity.isTileMapEntity = true;
            var chunks = new ChunkList(chunkSize, PlanetWrapBehavior.NoWrapAround, new NaturalLayer(chunkSize, chunkSize));
            var topMap = new Top(chunkSize.x);
            var layerTextures = new Texture2D[layersCount];
            
            entity.AddTileMapData(chunks, topMap, layerTextures);
            entity.tileMapData.Chunks.MakeAllChunksExplored();

            return entity;
        }

        public GameEntity GenerateTileMap()
        {
            // Generating the map
            Vector2Int mapSize = new Vector2Int(16, 16);

            var tileMap = CreateTileMap(mapSize);

            for (int j = 0; j < mapSize.y; j++)
            {
                for (int i = 0; i < mapSize.x; i++)
                {
                    var tile = Tile.Component.EmptyTile();
                    tile.FrontTilePropertiesId = 9;


                    if (i % 10 == 0)
                    {
                        //tile.FrontTilePropertiesId = 7;
                        tile.OreTilePropertiesId = 8;
                    }

                    if (j % 2 == 0)
                    {
                        // tile.FrontTilePropertiesId = 2;
                    }

                    if (j % 3 == 0)
                    {
                        // tile.FrontTilePropertiesId = 9;

                    }

                    if ((j > 1 && j < 6) || (j > (8 + i)))
                    {
                        tile.FrontTilePropertiesId = -1;
                        tile.OreTilePropertiesId = -1;
                    }


                    ManagerSystem.Instance.SetTile(ref tileMap, i, j, tile);
                }
            }

            tileMap.tileMapData.Top.UpdateTopTilesMap(ref tileMap);
            ManagerSystem.Instance.UpdateAllTileVariants(ref tileMap, PlanetLayer.Front);
            ManagerSystem.Instance.UpdateAllTileVariants(ref tileMap, PlanetLayer.Ore);
            ManagerSystem.Instance.BuildLayerTexture(ref tileMap, PlanetLayer.Front);
            ManagerSystem.Instance.BuildLayerTexture(ref tileMap, PlanetLayer.Ore);

            return tileMap;
        }
    }
}

