using Entitas;
using UnityEngine;

namespace Agent
{
    [Agent]
    public struct Sprite2DComponent : IComponent
    {
        public int SpriteID;
        public string SpritePath;
        
        public Vector2Int Size;
        public Material Material;
        
        // TODO: ADD Mesh component
        public Mesh Mesh;

        public void Init()
        {
            var atlasIndex = GameState.SpriteAtlasManager.CopySpriteToAtlas(SpriteID, 0, 0, 0);
            
            byte[] spriteBytes = new byte[Size.x * Size.y * 4];
            GameState.SpriteAtlasManager.GetSpriteBytes(atlasIndex, spriteBytes);
            var mat = UnityEngine.Object.Instantiate(Material);
            var tex = CreateTextureFromRGBA(spriteBytes, Size.x, Size.y);
            mat.SetTexture("_MainTex", tex);
           
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
        
        private Mesh CreateMesh(string name, int sortingOrder, Material material)
        {
            var go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));

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
