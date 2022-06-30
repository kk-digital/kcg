using UnityEngine;
using System.Collections.Generic;
using KMath;

namespace Utility
{

    internal static class Render
    {
        public static void DrawSprite(float x, float y, float w, float h,
            Sprites.Sprite sprite, Material material, Transform transform, 
                    int drawOrder = 0, float rotation = 0)
        {
            var tex = sprite.Texture;
            var mat = Material.Instantiate(material);
            mat.SetTexture("_MainTex", tex);
            //FIX: Do UnityEngine.CreateMesh, not using UnityEngine
            var mesh = CreateMesh(transform, "sprite", drawOrder, mat, new Vec2f(x, y), rotation);

            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();


            var p0 = new Vector3(0, 0, 0);
            var p1 = new Vector3((w), (h), 0);
            var p2 = p0; p2.y = p1.y;
            var p3 = p1; p3.y = p0.y;

            vertices.Add(p0);
            vertices.Add(p1);
            vertices.Add(p2);
            vertices.Add(p3);

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


            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
        }

        public static void DrawQuadColor(float x, float y, float w, float h,
            Color color, Material material, Transform transform, int drawOrder = 0, float rotation = 0)
        {
            var mat = material;
            mat.color = color;
            //FIX: Do UnityEngine.CreateMesh, not using UnityEngine
            var mesh = CreateMesh(transform, "colorQuad", drawOrder, mat, new Vec2f(x, y), rotation);

            List<int> triangles = new List<int>();
            List<Vector3> verticies = new List<Vector3>();

            var p0 = new Vector3(0, 0, 0);
            var p1 = new Vector3((w), (h), 0);
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

            mesh.SetVertices(verticies);
            mesh.SetTriangles(triangles, 0);
        }

        public static void DrawString(float x, float y, float characterSize, string label, int fontSize, Color color, Transform parent, int sortOrder)
        {
            var textMesh = CreateText(parent, new Vector2(x, y), "text", sortOrder);
            textMesh.text = label; 
            textMesh.characterSize = characterSize;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
        }

        private static Mesh CreateMesh(Transform parent, string name,
                             int sortingOrder, Material material, Vec2f position, float rotation)
        {
            var go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
            go.transform.SetParent(parent);
            go.transform.position = new Vector2(position.X, position.Y);
            go.transform.Rotate(0.0f, 0.0f, rotation, Space.Self);

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

        private static TextMesh CreateText(Transform parent, Vector2 pos, string name, int sortingOrder)
        {
            var go = new GameObject(name, typeof(TextMesh));
            go.transform.SetParent(parent);
            go.GetComponent<Transform>().position = pos;

            var textMesh = go.GetComponent<TextMesh>();
            textMesh.anchor = TextAnchor.LowerLeft;
            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            textMesh.font = ArialFont;

            var mr = go.GetComponent<MeshRenderer>();
            mr.sharedMaterial = ArialFont.material;
            mr.sortingOrder = sortingOrder;

            return textMesh;
        }

    }
}
