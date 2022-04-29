using System.Collections.Generic;
using TiledCS;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Rect = UnityEngine.Rect;

namespace Tiles
{
    class MeshBuilder
    {
        public List<int> triangles = new List<int>();
        public List<Vector2> uvs = new List<Vector2>();
        public List<Vector3> verticies = new List<Vector3>();
        public int PixelsPerUnit = 100;

        /// <summary> All quads of map </summary>
        public List<Quad> quads = new List<Quad>();

        public void Clear()
        {
            triangles.Clear();
            uvs.Clear();
            verticies.Clear();
            quads.Clear();
        }

        public void BuildQauds()
        {
            var map = Assets.Map;
            for (int i =- 0 ; i <  map.Layers.Length; i++)
                BuildQauds(i, 0);
        }

        public void BuildQauds(int iLayer, float depth)
        {
            var map = Assets.Map;
            var layer = map.Layers[iLayer];
            var texW = Assets.AtlasTexture.GetLength(0);
            var texH = Assets.AtlasTexture.GetLength(1);
            var parallax = new Vector2(layer.parallaxX, layer.parallaxY);

            if (layer.chunks != null)
            foreach (var chunk in layer.chunks)
                Build(chunk);

            void Build(TiledChunk chunk)
            {
                for (int i = 0; i < chunk.data.Length; i++)
                {
                    var gid = chunk.data[i];
                    if (gid == 0)
                        continue;//empty sprite

                    var sprite = Assets.SpritesById[gid];
                    if (sprite.Height == 0)
                        continue;//sprite is not found :(

                    //calc position
                    var iCol = i % chunk.width;
                    var iRow = i / chunk.width;
                    var k = 1/(float)PixelsPerUnit;
                    var w = sprite.Width * k;//quad width
                    var h = sprite.Height * k;//quad height
                    var x = (layer.offsetX + (chunk.x + iCol) * map.TileWidth) * k;
                    var y = (layer.offsetY - (chunk.y + iRow) * map.TileHeight) * k;

                    //calc UVs
                    var u0 = (float)sprite.Left / texW;
                    var u1 = u0 + (float)sprite.Width / texW;
                    var v1 = 1 - (float)sprite.Top / texH;
                    var v0 = v1 - (float)sprite.Height / texH;

                    //create quad
                    var p0 = new Vector3(x, y, depth);
                    var p1 = new Vector3(x + w, y + h, depth);
                    var uv0 = new Vector2(u0, v0);
                    var uv1 = new Vector2(u1, v1);
                    var quad = new Quad(p0, p1, uv0, uv1, parallax);
                    quads.Add(quad);
                }
            }
        }

        public void BuildMesh(Rect rect)
        {
            triangles.Clear();
            uvs.Clear();
            verticies.Clear();

            var fromX = rect.xMin;
            var toX = rect.xMax;
            var fromY = rect.yMin;
            var toY = rect.yMax;

            for (int i = 0 ; i < quads.Count; i++)
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
        }
    }

    /// <summary>Represents sprite quad in mesh</summary>
    struct Quad
    {
        public Vector3 P0;
        public Vector3 P1;
        public Vector2 UV0;
        public Vector2 UV1;
        public Vector2 Parallax;

        public Quad(Vector3 p0, Vector3 p1, Vector2 uV0, Vector2 uV1, Vector2 parallax)
        {
            P0 = p0;
            P1 = p1;
            UV0 = uV0;
            UV1 = uV1;
            Parallax = parallax;
        }
    }
}