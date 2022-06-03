using UnityEngine;
using System.Collections.Generic;

namespace Utility
{

    internal static class RenderUtils
    {

        public static void DrawSprite(float x, float y, float w, float h, 
                Render.Sprite sprite, Material material, Transform transform, int drawOrder = 0)
        {
            var tex = sprite.Texture;
            var mat = material;
            mat.SetTexture("_MainTex", tex);
            var mesh = CreateMesh(transform, "sprite", drawOrder, mat);

            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> verticies = new List<Vector3>();


            var p0 = new Vector3(x, y, 0);
            var p1 = new Vector3((x + w), (y + h), 0);
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

            var uv0 = new Vector2(sprite.TextureCoords.x, sprite.TextureCoords.y + sprite.TextureCoords.w);
            var uv1 = new Vector2(sprite.TextureCoords.x + sprite.TextureCoords.z, sprite.TextureCoords.y);
            var uv2 = uv0; uv2.y = uv1.y;
            var uv3 = uv1; uv3.y = uv0.y;


            uvs.Add(uv0);
            uvs.Add(uv1);
            uvs.Add(uv2);
            uvs.Add(uv3);
    

            mesh.SetVertices(verticies);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
        }


        private static Mesh CreateMesh(Transform parent, string name, int sortingOrder, Material material)
        {
            var go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
            go.transform.SetParent(parent);

            var mesh = new Mesh
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };

            var mf = go.GetComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            var mr = go.GetComponent<MeshRenderer>();
            mr.sharedMaterial = material;
            mr.sortingOrder = sortingOrder;

            return mesh;
        }
    }
}