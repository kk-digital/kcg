﻿using System.Collections.Generic;
using TiledCS;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Rect = UnityEngine.Rect;
using Mesh = UnityEngine.Mesh;
using System;

namespace PlanetTileMap
{
    class MeshBuilder
    {
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> verticies = new List<Vector3>();

        Quad[] quads;
        Mesh mesh;

        public MeshBuilder(Quad[] quads, Mesh mesh)
        {
            this.quads = quads;
            this.mesh = mesh;
        }

        public void Clear()
        {
            triangles.Clear();
            uvs.Clear();
            verticies.Clear();
        }

        public void BuildMesh(Rect visibleRect)
        {
            triangles.Clear();
            uvs.Clear();
            verticies.Clear();

            var fromX = visibleRect.xMin;
            var toX = visibleRect.xMax;
            var fromY = visibleRect.yMin;
            var toY = visibleRect.yMax;

            for (int i = 0 ; i < quads.Length; i++)
            {
                var quad = quads[i];
                if (quad.P1.x < fromX) continue;
                if (quad.P0.x > toX) continue;
                if (quad.P1.y < fromY) continue;
                if (quad.P0.y > toY) continue;

                //flush quad
                // P2  P1
                // P0  P3
                var startVert = verticies.Count;
                var p0 = quad.P0;
                var p1 = quad.P1;
                var p2 = p0; p2.y = p1.y;
                var p3 = p1; p3.y = p0.y;
                var uv0 = quad.UV0;
                var uv1 = quad.UV1;
                var uv2 = uv0; uv2.y = uv1.y;
                var uv3 = uv1; uv3.y = uv0.y;

                verticies.Add(p0);
                verticies.Add(p1);
                verticies.Add(p2);
                verticies.Add(p3);

                uvs.Add(uv0);
                uvs.Add(uv1);
                uvs.Add(uv2);
                uvs.Add(uv3);

                triangles.Add(startVert + 0);
                triangles.Add(startVert + 2);
                triangles.Add(startVert + 1);

                triangles.Add(startVert + 0);
                triangles.Add(startVert + 1);
                triangles.Add(startVert + 3);
            }

            mesh.Clear(true);
            mesh.SetVertices(verticies);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
        }
    }
}