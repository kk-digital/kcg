using UnityEngine;
using System.Collections.Generic;
using Entitas;
using KMath;

namespace Vehicle
{
    public class SpawnerSystem
    {
        private static int vehicleID;

        public Entity SpawnVehicle(Material material, int spriteID, int width, int height, Vec2f position)
        {
            // Create Entity
            var entity = Contexts.sharedInstance.game.CreateEntity();

            // Increase ID per object statically
            vehicleID++;

            // Set Png Size
            var pngSize = new Vec2i(width, height);

            // Set Sprite ID from Sprite Atlas
            var spriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteID, 0, 0, Enums.AtlasType.Agent);

            // Set Sprite Size
            var spriteSize = new Vec2f(pngSize.X / 32f, pngSize.Y / 32f);

            // Add ID Component
            entity.AddVehicleID(vehicleID);

            // Add Sprite Component
            entity.AddVehicleSprite2D(spriteId, spriteSize);

            // Add Physics State 2D Component
            entity.AddVehiclePhysicsState2D(position, position, Vec2f.One, Vec2f.One, Vec2f.Zero, 1.0f, 1.0f, 1.5f,
                Vec2f.Zero);

            // Add Physics Box Collider Component
            entity.AddPhysicsBox2DCollider(spriteSize, Vec2f.Zero);

            // Return projectile entity
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

            return InstantiateGameObject("Vehicle", 0, mat, box2dCollider);
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

