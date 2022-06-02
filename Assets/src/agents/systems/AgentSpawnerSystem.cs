using System.Drawing;
using SpriteAtlas;
using UnityEngine;

namespace Agent
{
    public class AgentSpawnerSystem
    {
        public static readonly AgentSpawnerSystem Instance;
        public GameContext GameContext;

        private static int playerID;

        static AgentSpawnerSystem()
        {
            Instance = new AgentSpawnerSystem();
        }

        public AgentSpawnerSystem()
        {
            GameContext = Contexts.sharedInstance.game;
        }

        public GameEntity SpawnPlayer(Material material)
        {
            var entity = GameContext.CreateEntity();

            playerID++;

            entity.isPlayer = true;
            
            entity.AddAgentID(1);

            var spriteSize = new Vector2Int(32, 48);
            var spritePath = "Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png";
            var spriteID = GameState.SpriteLoader.GetSpriteSheetID(spritePath, spriteSize.x, spriteSize.y);

            entity.AddAgentSprite2D(spriteID, spritePath, spriteSize, material, BuildMesh(spriteID, material, spriteSize));

            entity.AddAgentPosition2D(new Vector2(3f, 2f), default);

            return entity;
        }
        
        private Mesh BuildMesh(int spriteID, Material material, Vector2Int spriteSize)
        {
            var atlasIndex = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteID, 0, 0, AtlasType.Agent);
            
            byte[] spriteBytes = new byte[spriteSize.x * spriteSize.y * 4];
            GameState.SpriteAtlasManager.GetSpriteBytes(atlasIndex, spriteBytes, AtlasType.Agent);
            var mat = UnityEngine.Object.Instantiate(material);
            var tex = CreateTextureFromRGBA(spriteBytes, spriteSize.x, spriteSize.y);
            mat.SetTexture("_MainTex", tex);

            return InstantiateMesh("Agent", 0, mat);
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
        
        private Mesh InstantiateMesh(string name, int sortingOrder, Material material)
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

