using System.Collections.Generic;
using UnityEngine;

namespace Tiles.PlanetMap.Unity
{
    //TODO: Improve naming PlanetTileMapRender ?
    static class GLDrawPlanetTileMap
    {
        //TODO: USE GOOD NAMES
        //TODO: Does anything even call this!?
        //TODO: This class has no variables? Should this be a method?
        public static void DrawPlanetTileMap(Rect rect, List<Quad> quads, Material mat)
        {
            GL.PushMatrix();
            mat.SetPass(0);
            GL.Begin(GL.QUADS);

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
                var p0 = quad.P0;
                var p1 = quad.P1;
                var p2 = p0; p2.y = p1.y;
                var p3 = p1; p3.y = p0.y;
                var uv0 = quad.UV0;
                var uv1 = quad.UV1;
                var uv2 = uv0; uv2.y = uv1.y;
                var uv3 = uv1; uv3.y = uv0.y;

                GL.TexCoord(uv0);
                GL.Vertex(p0);

                GL.TexCoord(uv2);
                GL.Vertex(p2);

                GL.TexCoord(uv1);
                GL.Vertex(p1);

                GL.TexCoord(uv3);
                GL.Vertex(p3);
            }

            GL.End();
            GL.PopMatrix();
            GL.Flush();
        }
    }
}