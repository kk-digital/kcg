using Entitas;
using Entitas.CodeGeneration.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    // todo: This should be unique entitas context.
    // We need every system to have a separate context for that.
    //[ Unique Inventory, Particle, Item particle, Agent, Projectiles, ...]
    public struct FrameMesh
    {
        public FrameMesh(Material material, Transform transform, int drawOrder = 0)
        {
            obj = new GameObject("ItemGameObject", typeof(MeshFilter), typeof(MeshRenderer));
            obj.transform.SetParent(transform);

            var mesh = new Mesh
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };

            var mf = obj.GetComponent<MeshFilter>();
            mf.sharedMesh = mesh;
            var mr = obj.GetComponent<MeshRenderer>();
            mr.sharedMaterial = material;
            mr.sortingOrder = drawOrder;

            // Todo: prealocate lists.
            vertices = new List<Vector3>();
            triangles = new List<int>();
            uvs = new List<Vector2>();
        }

        public GameObject obj;
        public List<Vector3> vertices;
        public List<Vector2> uvs;
        public List<int> triangles;

        public void UpdateVertex(int index, float x, float y, float w, float h)
        {
            var p0 = new Vector3(x, y, 0);
            var p1 = new Vector3((x + w), (y + h), 0);
            var p2 = p0; p2.y = p1.y;
            var p3 = p1; p3.y = p0.y;

            if (uvs.Count >= index)
            {
                AddVertex(p0, p1, p2, p3);
            }
            else
            {
                vertices[index++] = p0;
                vertices[index++] = p1;
                vertices[index++] = p2;
                vertices[index] = p3;
            }
        }

        public void UpdateUV(Vector4 textureCoords, int index)
        {
            var uv0 = new Vector2(textureCoords.x, textureCoords.y + textureCoords.w);
            var uv1 = new Vector2(textureCoords.x + textureCoords.z, textureCoords.y);
            var uv2 = uv0; uv2.y = uv1.y;
            var uv3 = uv1; uv3.y = uv0.y;

            if (uvs.Count >= index)
            {
                AddUV(uv0, uv1, uv2, uv3);
            }
            else
            {
                uvs[index++] = uv0;
                uvs[index++] = uv1;
                uvs[index++] = uv2;
                uvs[index] = uv3;
            }
        }

        private void AddVertex(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
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

        void AddUV(Vector2 uv0, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            uvs.Add(uv0);
            uvs.Add(uv1);
            uvs.Add(uv2);
            uvs.Add(uv3);
        }
    }
}
