using Entitas;
using Entitas.CodeGeneration.Attributes;
using KMath;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public struct FrameMesh
    {
        public GameObject obj;
        public List<Vector3> vertices;
        public List<Vector2> uvs;
        public List<int> triangles;

        public FrameMesh(string name, Material material, Transform transform, Sprites.SpriteAtlas Atlassprite, int drawOrder = 0)
        {
            obj = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
            obj.transform.SetParent(transform);

            var mat = Material.Instantiate(material);
            mat.SetTexture("MainTex", Atlassprite.Texture);

            var mesh = new Mesh
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };

            var mf = obj.GetComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            var mr = obj.GetComponent<MeshRenderer>();
            mr.sharedMaterial = mat;
            mr.sortingOrder = drawOrder;

            // Todo: prealocate lists.
            vertices = new List<Vector3>();
            triangles = new List<int>();
            uvs = new List<Vector2>();
        }

        public void Clear()
        {
            vertices.Clear();
            uvs.Clear();
            triangles.Clear();
        }

        public void UpdateVertex(int index, float x, float y, float w, float h)
        {
            var p0 = new Vector3(x, y, 0);
            var p1 = new Vector3((x + w), (y + h), 0);
            var p2 = p0; p2.y = p1.y;
            var p3 = p1; p3.y = p0.y;

            int triangleIndex = vertices.Count;

            vertices.Add(p0);
            vertices.Add(p1);
            vertices.Add(p2);
            vertices.Add(p3);

            triangles.Add(triangleIndex);
            triangles.Add(triangleIndex + 2);
            triangles.Add(triangleIndex + 1);
            triangles.Add(triangleIndex);
            triangles.Add(triangleIndex + 1);
            triangles.Add(triangleIndex + 3);
        }

        public void UpdateUV(Vector4 textureCoords, int index)
        {
            var uv0 = new Vector2(textureCoords.x, textureCoords.y + textureCoords.w);
            var uv1 = new Vector2(textureCoords.x + textureCoords.z, textureCoords.y);
            var uv2 = uv0; uv2.y = uv1.y;
            var uv3 = uv1; uv3.y = uv0.y;

            uvs.Add(uv0);
            uvs.Add(uv1);
            uvs.Add(uv2);
            uvs.Add(uv3);
        }
    }


    internal static class ObjectMesh
    {
        private static GameObject CreateObjectMesh(Transform parent, string name,
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

            return go;
        }

        public static GameObject CreateEmptyObjectMesh()
        {
            var go = new GameObject("sprite", typeof(MeshFilter), typeof(MeshRenderer));

            var mesh = new Mesh
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };

            var mf = go.GetComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            var mr = go.GetComponent<MeshRenderer>();

            return go;
        }

        public static GameObject CreateObjectText(Transform parent, Vector2 pos, string name, int sortingOrder)
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

            return go;
        }

        public static GameObject CreateEmptyTextGameObject(string name = "sprite")
        {
            var go = new GameObject(name, typeof(TextMesh));

            var textMesh = go.GetComponent<TextMesh>();
            textMesh.anchor = TextAnchor.LowerLeft;
            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            textMesh.font = ArialFont;

            var mr = go.GetComponent<MeshRenderer>();
            mr.sharedMaterial = ArialFont.material;

            return go;
        }
    }
}
