using System.Collections.Generic;
using UnityEngine;

namespace Agent
{
    public class AgentDrawSystem
    {
        public static readonly AgentDrawSystem Instance;

        public readonly AgentContext AgentContext;
        
        List<int> triangles = new();
        List<Vector2> uvs = new();
        List<Vector3> verticies = new();

        static AgentDrawSystem()
        {
            Instance = new AgentDrawSystem();
        }
        
        public AgentDrawSystem()
        {
            AgentContext = Contexts.sharedInstance.agent;
        }
        
        public void Draw()
        {
            var agents = AgentContext.GetGroup(AgentMatcher.Sprite2D);
            
            foreach (var agent in agents)
            {
                byte[] spriteBytes = new byte[agent.sprite2D.Size.x * agent.sprite2D.Size.y * 4];
                GameState.SpriteAtlasManager.GetSpriteBytes(agent.sprite2D.AtlasIndex, spriteBytes);
                
                var tex = CreateTextureFromRGBA(spriteBytes, agent.sprite2D.Size.x, agent.sprite2D.Size.y);
                var mat = Object.Instantiate(agent.sprite2D.Material);
                mat.SetTexture("_MainTex", tex);
                var mesh = CreateMesh(agent.sprite2D.Parent, "Agent", 0, mat);
                
                triangles.Clear();
                uvs.Clear();
                verticies.Clear();

                var x = agent.position2D.Value.x;
                var y = agent.position2D.Value.y;
                var width = 1.0f;
                var height = agent.sprite2D.Size.y / (float)agent.sprite2D.Size.x;
                
                var p0 = new Vector3(x, y, 0);
                var p1 = new Vector3((x + width), (y + height), 0);
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
            }
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

        private Mesh CreateMesh(Transform parent, string name, int sortingOrder, Material material)
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

