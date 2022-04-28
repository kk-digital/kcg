using System.Collections.Generic;
using TiledCS;
using UnityEngine;

namespace Tiles
{
    class MeshBuilder
    {
        public List<int> triangles = new List<int>();
        public List<Vector2> uvs = new List<Vector2>();
        public List<Vector3> verticies = new List<Vector3>();
        public int PixelsPerUnit = 100;

        public void Clear()
        {
            triangles.Clear();
            uvs.Clear();
            verticies.Clear();
        }

        public void Build(int iLayer, float depth)
        {
            var map = Assets.Map;
            var layer = map.Layers[iLayer];
            var texW = Assets.AtlasTexture.GetLength(0);
            var texH = Assets.AtlasTexture.GetLength(1);

            if (layer.chunks != null)
            foreach (var chunk in layer.chunks)
            { 
                Build(chunk);
            }

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
                    var p1 = new Vector3(x, y + h, depth);
                    var p2 = new Vector3(x + w, y + h, depth);
                    var p3 = new Vector3(x + w, y, depth);
                    var uv0 = new Vector2(u0, v0);
                    var uv1 = new Vector2(u0, v1);
                    var uv2 = new Vector2(u1, v1);
                    var uv3 = new Vector2(u1, v0);

                    var startVert = verticies.Count;

                    verticies.Add(p0);
                    verticies.Add(p1);
                    verticies.Add(p2);
                    verticies.Add(p3);

                    uvs.Add(uv0);
                    uvs.Add(uv1);
                    uvs.Add(uv2);
                    uvs.Add(uv3);

                    triangles.Add(startVert + 0);
                    triangles.Add(startVert + 1);
                    triangles.Add(startVert + 2);

                    triangles.Add(startVert + 0);
                    triangles.Add(startVert + 2);
                    triangles.Add(startVert + 3);
                }

            }
        }
    }
}