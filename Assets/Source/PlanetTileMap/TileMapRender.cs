using Enums.Tile;
using System;
using UnityEngine;
using Utility;

namespace PlanetTileMap
{

    public class TileMapRenderer
    {
        FrameMesh[] LayerMeshes;
        public static readonly int LayerCount = Enum.GetNames(typeof(MapLayerType)).Length;

        public void Initialize(Material material, Transform transform, int drawOrder)
        {
            LayerMeshes = new FrameMesh[LayerCount];

            for (int i = 0; i < LayerCount; i++)
            {
                LayerMeshes[i] = new FrameMesh(("layerMesh" + i), material, transform,
                    GameState.TileSpriteAtlasManager.GetSpriteAtlas(0), drawOrder + i);
            }
        }

        public void UpdateMidLayerMesh(TileMap tileMap)
        {
            if (Camera.main == null) { Debug.LogError("Camera.main not found, failed to create edge colliders"); return; }

            var cam = Camera.main;
            if (!cam.orthographic) { Debug.LogError("Camera.main is not Orthographic, failed to create edge colliders"); return; }

            var bottomLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
            var topLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
            var topRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
            var bottomRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane));

            LayerMeshes[(int)MapLayerType.Mid].Clear();

            int index = 0;
            for (int y = (int)(bottomLeft.y - 10); y < tileMap.MapSize.Y && y <= (topRight.y + 10); y++)
            {
                for (int x = (int)(bottomLeft.x - 10); x < tileMap.MapSize.X && x <= (bottomRight.x + 10); x++)
                {
                    if (x >= 0 && y >= 0)
                    {
                        var tile = tileMap.GetTile(x, y);

                        var spriteId = tile.MidTileSpriteID;

                        if (spriteId >= 0)
                        {
                            Vector4 textureCoords = GameState.TileSpriteAtlasManager.GetSprite(spriteId).TextureCoords;

                            const float width = 1;
                            const float height = 1;

                            if (!ObjectMesh.isOnScreen(x, y))
                                continue;

                            // Update UVs
                            LayerMeshes[(int)MapLayerType.Mid].UpdateUV(textureCoords, (index) * 4);
                            // Update Vertices
                            LayerMeshes[(int)MapLayerType.Mid].UpdateVertex((index * 4), x, y, width, height);
                            index++;
                        }

                        var tileProperty = GameState.TileCreationApi.GetTileProperty(tile.MidTileID);
                        
                        if (tileProperty.DrawType == TileDrawType.Composited)
                        {
                            Vector4 textureCoords = GameState.TileSpriteAtlasManager.GetSprite(spriteId).TextureCoords;

                            const float width = 1;
                            const float height = 1;

                            if (!Utility.ObjectMesh.isOnScreen(x, y))
                                continue;

                            // Update UVs
                            LayerMeshes[(int)MapLayerType.Front].UpdateUV(textureCoords, (index) * 4);
                            // Update Vertices
                            LayerMeshes[(int)MapLayerType.Front].UpdateVertex((index * 4), x, y, width, height);
                            index++;
                        }
                    }
                }
            }
        }

        public void UpdateFrontLayerMesh(TileMap tileMap)
        {
            if (Camera.main == null) { Debug.LogError("Camera.main not found, failed to create edge colliders"); return; }

            var cam = Camera.main;
            if (!cam.orthographic) { Debug.LogError("Camera.main is not Orthographic, failed to create edge colliders"); return; }

            var bottomLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
            var topLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
            var topRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
            var bottomRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane));

            LayerMeshes[(int)MapLayerType.Front].Clear();

            int index = 0;
            for (int y = (int)(bottomLeft.y - 10); y < tileMap.MapSize.Y && y <= (topRight.y + 10); y++)
            {
                for (int x = (int)(bottomLeft.x - 10); x < tileMap.MapSize.X && x <= (bottomRight.x + 10); x++)
                {
                    if (x >= 0 && y >= 0)
                    {
                        ref var tile = ref tileMap.GetTile(x, y);

                        var spriteId = tile.FrontTileSpriteID;

                        if (spriteId >= 0)
                        {
                            Vector4 textureCoords = GameState.TileSpriteAtlasManager.GetSprite(spriteId).TextureCoords;

                            const float width = 1;
                            const float height = 1;

                            if (!ObjectMesh.isOnScreen(x, y))
                                continue;

                            // Update UVs
                            LayerMeshes[(int)MapLayerType.Front].UpdateUV(textureCoords, (index) * 4);
                            // Update Vertices
                            LayerMeshes[(int)MapLayerType.Front].UpdateVertex((index * 4), x, y, width, height);
                            index++;
                        }
                        
                        var tileProperty = GameState.TileCreationApi.GetTileProperty(tile.MidTileID);

                        if (tileProperty.DrawType == TileDrawType.Composited)
                        {
                            Vector4 textureCoords = GameState.TileSpriteAtlasManager.GetSprite(spriteId).TextureCoords;

                            const float width = 1;
                            const float height = 1;

                            if (!Utility.ObjectMesh.isOnScreen(x, y))
                                continue;

                            // Update UVs
                            LayerMeshes[(int)MapLayerType.Front].UpdateUV(textureCoords, (index) * 4);
                            // Update Vertices
                            LayerMeshes[(int)MapLayerType.Front].UpdateVertex((index * 4), x, y, width, height);
                            index++;
                        }
                    }
                }
            }
        }
        
        public void UpdateBackLayerMesh(TileMap tileMap)
        {
            if (Camera.main == null) { Debug.LogError("Camera.main not found, failed to create edge colliders"); return; }

            var cam = Camera.main;
            if (!cam.orthographic) { Debug.LogError("Camera.main is not Orthographic, failed to create edge colliders"); return; }

            var bottomLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
            var topLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
            var topRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
            var bottomRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane));

            LayerMeshes[(int)MapLayerType.Back].Clear();

            int index = 0;
            for (int y = (int)(bottomLeft.y - 10); y < tileMap.MapSize.Y && y <= (topRight.y + 10); y++)
            {
                for (int x = (int)(bottomLeft.x - 10); x < tileMap.MapSize.X && x <= (bottomRight.x + 10); x++)
                {
                    if (x >= 0 && y >= 0)
                    {
                        ref var tile = ref tileMap.GetTile(x, y);

                        var spriteId = tile.BackTileSpriteID;

                        if (spriteId >= 0)
                        {
                            Vector4 textureCoords = GameState.TileSpriteAtlasManager.GetSprite(spriteId).TextureCoords;

                            const float width = 1;
                            const float height = 1;

                            if (!ObjectMesh.isOnScreen(x, y))
                                continue;

                            // Update UVs
                            LayerMeshes[(int)MapLayerType.Back].UpdateUV(textureCoords, (index) * 4);
                            // Update Vertices
                            LayerMeshes[(int)MapLayerType.Back].UpdateVertex((index * 4), x, y, width, height);
                            index++;
                        }

                        var tileProperty = GameState.TileCreationApi.GetTileProperty(tile.MidTileID);
                        
                        if (tileProperty.DrawType == TileDrawType.Composited)
                        {
                            Vector4 textureCoords = GameState.TileSpriteAtlasManager.GetSprite(spriteId).TextureCoords;

                            const float width = 1;
                            const float height = 1;

                            if (!Utility.ObjectMesh.isOnScreen(x, y))
                                continue;

                            // Update UVs
                            LayerMeshes[(int)MapLayerType.Front].UpdateUV(textureCoords, (index) * 4);
                            // Update Vertices
                            LayerMeshes[(int)MapLayerType.Front].UpdateVertex((index * 4), x, y, width, height);
                            index++;
                        }
                    }
                }
            }
        }

        public void DrawLayer(MapLayerType planetLayer)
        {
            Render.DrawFrame(ref LayerMeshes[(int)planetLayer], GameState.TileSpriteAtlasManager.GetSpriteAtlas(0));
        }
    }
}
