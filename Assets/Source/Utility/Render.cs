using UnityEngine;
using System.Collections.Generic;
using KMath;
using Sprites;
using UnityEditor;

namespace Utility
{

    internal static class Render
    {
       public static void DrawFrame(ref FrameMesh frameMesh, Sprites.SpriteAtlas Atlassprite)
        {
            var mesh = frameMesh.obj.GetComponent<MeshFilter>().sharedMesh;
            mesh.Clear(); // This makes sure you never have out of bounds data.

            var mr = frameMesh.obj.GetComponent<MeshRenderer>();
            mr.sharedMaterial.SetTexture("_MainTex", Atlassprite.Texture);

            mesh.SetVertices(frameMesh.vertices);
            mesh.SetUVs(0, frameMesh.uvs);
            mesh.SetTriangles(frameMesh.triangles, 0);
        }

        public static void DrawSprite(GameObject gameObject, float x, float y, float w, float h,
            Sprites.Sprite sprite)
        {
            var tex = sprite.Texture;
            var mr = gameObject.GetComponent<MeshRenderer>();
            mr.sharedMaterial.SetTexture("_MainTex", tex);

            var mf = gameObject.GetComponent<MeshFilter>();
            var mesh = mf.sharedMesh;

            gameObject.transform.position = new Vector3(x, y, 0.0f);

            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> vertices = new List<Vector3>();


            var p0 = new Vector3(x, y, 0);
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

        public static void DrawQuadColor(GameObject gameObject, float x, float y, float w, float h,
            Color color)
        {
            var mr = gameObject.GetComponent<MeshRenderer>();
            mr.sharedMaterial.color = color;

            var mf = gameObject.GetComponent<MeshFilter>();
            var mesh = mf.sharedMesh;

            gameObject.transform.position = new Vector3(x, y, 0.0f);

            List<int> triangles = new List<int>();
            List<Vector3> verticies = new List<Vector3>();

            var p0 = new Vector3(x, y, 0);
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


        public static void DrawString(GameObject gameObject, float x, float y, float characterSize, string label, int fontSize, Color color, int sortOrder)
        {
            var textMesh = gameObject.GetComponent<TextMesh>();
            var mr = gameObject.GetComponent<MeshRenderer>();
            mr.sortingOrder = sortOrder;

            gameObject.transform.position = new Vector2(x, y);
            textMesh.text = label;
            textMesh.characterSize = characterSize;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
        }

        /// <summary>
        /// These functions draw immediately on screen. 
        /// It doesn't work inside OnUpdate, because camera clear screen before drawing.
        /// </summary>
        public static void DrawSpriteNow(float x, float y, float w, float h,
            Sprites.Sprite sprite, Material material)
        {
            GL.PushMatrix();
            DrawGlSprite(x, y, w, h, sprite, material);
        }

        public static void DrawQuadColorNow(float x, float y, float w, float h,
            Color color, Material material)
        {
            GL.PushMatrix();
            DrawGlQuad(x, y, w, h, color, material);
        }

        /// <summary>
        /// [x, y] = (0, 0) is lower left coner of the screen.
        /// [x, y] = (1, 1) is upper right coner of the screen.
        /// </summary>
        public static void DrawSpriteColorGui(float x, float y, float w, float h,
            Sprites.Sprite sprite, Material material)
        {
            GL.PushMatrix();
            GL.LoadOrtho();
            DrawGlSprite(x, y, w, h, sprite, material);
        }

        public static void DrawQuadColorGui(float x, float y, float w, float h,
                Color color, Material material)
        {
            GL.PushMatrix();
            GL.LoadOrtho();
            DrawGlQuad(x, y, w, h, color, material);
        }


        /// <summary>
        /// Helper Functions.
        /// </summary>
        private static void DrawGlSprite(float x, float y, float w, float h,
            Sprites.Sprite sprite, Material material)
        {
            // Todo: Fix memory leak. Track and release Instantiate materials.

            Vector4 texCoord = sprite.TextureCoords;
            var uv0 = new Vector2(texCoord.x, texCoord.y + texCoord.w);
            var uv2 = new Vector2(texCoord.x + texCoord.z, texCoord.y);
            var uv1 = uv0; uv1.y = uv2.y;
            var uv3 = uv2; uv3.y = uv0.y;

            var mat = Material.Instantiate(material);

            mat.SetTexture("_MainTex", sprite.Texture);

            mat.SetPass(0);

            GL.Begin(GL.QUADS);

            GL.TexCoord2(uv0.x, uv0.y);
            GL.Vertex3(x, y, 0);

            GL.TexCoord2(uv1.x, uv1.y);
            GL.Vertex3(x, (y + h), 0);

            GL.TexCoord2(uv2.x, uv2.y);
            GL.Vertex3(x + w, y + h, 0);

            GL.TexCoord2(uv3.x, uv3.y);
            GL.Vertex3((x + w), y, 0);

            GL.End();
            GL.PopMatrix();
        }

        private static void DrawGlQuad(float x, float y, float w, float h,
                Color color, Material material)
        {
            var mat = Material.Instantiate(material);
            mat.SetPass(0);

            GL.Begin(GL.QUADS);
            GL.Color(color);

            GL.Vertex3(x, y, 0);
            GL.Vertex3(x, (y + h), 0);
            GL.Vertex3(x + w, y + h, 0);
            GL.Vertex3((x + w), y, 0);

            GL.End();
            GL.PopMatrix();
        }
    }
}
