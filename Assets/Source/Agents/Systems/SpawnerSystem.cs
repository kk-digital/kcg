using UnityEngine;
using System.Collections.Generic;
using Entitas;
using Physics;

namespace Agent
{
    public class SpawnerSystem
    {
        private static int playerID;

        public GameEntity SpawnPlayer(Material material, Vector2 position)
        {
            var entity = Contexts.sharedInstance.game.CreateEntity();

            playerID++;

            var spritePath = "Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png";
            var pngSize = new Vector2Int(32, 48);
            var spriteID = GameState.SpriteLoader.GetSpriteSheetID(spritePath);
            var spriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteID, 0, 0, Enums.AtlasType.Agent);
            byte[] spriteData = new byte[pngSize.x * pngSize.y * 4];
            GameState.SpriteAtlasManager.GetSpriteBytes(spriteId, spriteData, Enums.AtlasType.Agent);
            var texture = Utility.Texture.CreateTextureFromRGBA(spriteData, pngSize.x, pngSize.y);
            var spriteSize = new Vector2(pngSize.x / 32f, pngSize.y / 32f);

            entity.isAgentPlayer = true;
            entity.isECSInput = true;
            entity.AddECSInputXY(new Vector2(0, 0), false);

            entity.AddAgentID(playerID);

            entity.AddAgentSprite2D(texture, spriteSize);
            entity.AddAgentPosition2D(position, newPreviousValue: default);
            Vector2 box2dCollider = new Vector2(0.5f, 1.5f);
            entity.AddPhysicsBox2DCollider(box2dCollider, new Vector2(0.25f, 0.0f));
            entity.AddAgentMovable(newSpeed: 1f, newVelocity: Vector2.zero, newAcceleration: Vector2.zero, newAccelerationTime: 2f);

            return entity;
        }

        public GameEntity SpawnAgent(Material material, Vector2 position)
        {
            var entity = Contexts.sharedInstance.game.CreateEntity();

            playerID++;

            var spritePath = "Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png";
            var pngSize = new Vector2Int(32, 48);
            var spriteID = GameState.SpriteLoader.GetSpriteSheetID(spritePath);
            var spriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteID, 0, 0, Enums.AtlasType.Agent);
            byte[] spriteData = new byte[pngSize.x * pngSize.y * 4];
            GameState.SpriteAtlasManager.GetSpriteBytes(spriteId, spriteData, Enums.AtlasType.Agent);
            var texture = Utility.Texture.CreateTextureFromRGBA(spriteData, pngSize.x, pngSize.y);
            var spriteSize = new Vector2(pngSize.x / 32f, pngSize.y / 32f);

            entity.AddAgentID(playerID);

            Vector2 box2dCollider = new Vector2(0.5f, 1.5f);
            entity.AddPhysicsBox2DCollider(box2dCollider, new Vector2(0.25f, 0.0f));
            entity.AddAgentSprite2D(texture, spriteSize);
            entity.AddAgentPosition2D(position, newPreviousValue: default);
            entity.AddAgentMovable(newSpeed: 1f, newVelocity: Vector2.zero, newAcceleration: Vector2.zero, newAccelerationTime: 2f);

            return entity;
        }

        public GameEntity SpawnEnemy(Material material, Vector2 position)
        {
            var entity = Contexts.sharedInstance.game.CreateEntity();

            playerID++;
            
            var spritePath = "Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png";
            var pngSize = new Vector2Int(32, 48);
            var spriteID = GameState.SpriteLoader.GetSpriteSheetID(spritePath);
            var spriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteID, 0, 0, Enums.AtlasType.Agent);
            byte[] spriteData = new byte[pngSize.x * pngSize.y * 4];
            GameState.SpriteAtlasManager.GetSpriteBytes(spriteId, spriteData, Enums.AtlasType.Agent);
            var texture = Utility.Texture.CreateTextureFromRGBA(spriteData, pngSize.x, pngSize.y);
            var spriteSize = new Vector2(pngSize.x / 32f, pngSize.y / 32f);
            
            entity.AddAgentID(playerID);

            Vector2 box2dCollider = new Vector2(0.5f, 1.5f);
            entity.AddPhysicsBox2DCollider(box2dCollider, new Vector2(0.25f, 0.0f));
            entity.AddAgentSprite2D(texture, spriteSize);
            entity.AddAgentPosition2D(position, newPreviousValue: default);
            entity.AddAgentMovable(newSpeed: 1f, newVelocity: Vector2.zero, newAcceleration: Vector2.zero, newAccelerationTime: 2f);
            entity.isAgentEnemy = true;

            return entity;
        }

        private GameObject BuildGameObject(int spriteID, Material material, Vector2Int spriteSize, Vector2 box2dCollider)
        {
            var atlasIndex = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteID, 0, 0, Enums.AtlasType.Agent);

            byte[] spriteBytes = new byte[spriteSize.x * spriteSize.y * 4];
            GameState.SpriteAtlasManager.GetSpriteBytes(atlasIndex, spriteBytes, Enums.AtlasType.Agent);
            var mat = UnityEngine.Object.Instantiate(material);
            var tex = CreateTextureFromRGBA(spriteBytes, spriteSize.x, spriteSize.y);
            mat.SetTexture("_MainTex", tex);

            return InstantiateGameObject("Agent", 0, mat, box2dCollider);
        }

        private Texture2D CreateTextureFromRGBA(byte[] rgba, int w, int h)
        {

            var res = new Texture2D(w, h, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point
            };

            var pixels = new Color32[w * h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int index = (x + y * w) * 4;
                    var r = rgba[index];
                    var g = rgba[index + 1];
                    var b = rgba[index + 2];
                    var a = rgba[index + 3];

                    pixels[x + y * w] = new Color32(r, g, b, a);
                }
            }

            res.SetPixels32(pixels);
            res.Apply();

            return res;
        }

        private GameObject InstantiateGameObject(string name, int sortingOrder, Material material, Vector2 size)
        {
            var go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));

            var mesh = new Mesh
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };

            var mf = go.GetComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            var mr = go.GetComponent<MeshRenderer>();
            mr.sharedMaterial = material;
            mr.sortingOrder = sortingOrder;

            List<Vector3> verticies = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            float width = size.x;
            float height = size.y;

            var p0 = new Vector3(0, 0, 0);
            var p1 = new Vector3((width), (height), 0);
            var p2 = p0; p2.y = p1.y;
            var p3 = p1; p3.y = p0.y;

            verticies.Add(p0);
            verticies.Add(p1);
            verticies.Add(p2);
            verticies.Add(p3);

            triangles.Add(0);
            triangles.Add(2);
            triangles.Add(1);
            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(3);

            var u0 = 0;
            var u1 = 1;
            var v1 = -1;
            var v0 = 0;

            var uv0 = new Vector2(u0, v0);
            var uv1 = new Vector2(u1, v1);
            var uv2 = uv0; uv2.y = uv1.y;
            var uv3 = uv1; uv3.y = uv0.y;


            uvs.Add(uv0);
            uvs.Add(uv1);
            uvs.Add(uv2);
            uvs.Add(uv3);


            mesh.SetVertices(verticies);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);

            return go;
        }
    }
}

