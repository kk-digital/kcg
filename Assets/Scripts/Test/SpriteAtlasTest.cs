using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Planet.Unity
{
    //Note: TileMap should be mostly controlled by GameManager


    //Note(Mahdi): we are just testing and making sure everything is working
    // before we move things out of here
    // there will be things like rendering, collision, TileMap
    // that are not supposed to be here.

    class SpriteAtlasTest : MonoBehaviour
    {
        //public string TileMap = "Moonbunker/Moon Bunker.tmx";
        [SerializeField] Material Material;

        public static string BaseDir => Application.streamingAssetsPath;

        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> verticies = new List<Vector3>();

        Vector2 MapOffset = new Vector2(-3.0f, 4.0f);

        static bool InitTiles = false;
        

        public void Start()
        {
            if (!InitTiles)
            {
                LoadSprites();
                InitTiles = true;
            }
        }

        public void OnRenderObject()
        {
            DrawSpriteAtlas();
            DrawSprite(2, 1, 1.0f, 2.0f, 3);
            DrawSprite(2, -1, 1.0f, 1.5f, 2);
        }

        // create the sprite atlas for testing purposes
        public void LoadSprites()
        {
            // we load the sprite sheets here
            int SomeObjectTileSheet = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Furnitures\\PowerMachines\\Tanks\\Algae\\algaeTank1.png", 32, 64);
            int PlayerTileSheet = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Characters\\Player\\character.png", 32, 48);


            // bit the sprites into the sprite atlas
            // we can blit the same sprite
            // but its only for testing purpose
            // we should remove that in the future
            GameState.SpriteAtlasManager.CopySpriteToAtlas(SomeObjectTileSheet, 0, 0, Enums.AtlasType.Generic);
            GameState.SpriteAtlasManager.CopySpriteToAtlas(PlayerTileSheet, 0, 0, Enums.AtlasType.Generic);;
            GameState.SpriteAtlasManager.CopySpriteToAtlas(PlayerTileSheet, 0, 0, Enums.AtlasType.Generic);
            GameState.SpriteAtlasManager.CopySpriteToAtlas(SomeObjectTileSheet, 0, 0, Enums.AtlasType.Generic);
            GameState.SpriteAtlasManager.CopySpriteToAtlas(PlayerTileSheet, 0, 0, Enums.AtlasType.Generic);
            GameState.SpriteAtlasManager.CopySpriteToAtlas(PlayerTileSheet, 0, 0, Enums.AtlasType.Generic);
        }

        // drawing the sprite atlas
        void DrawSpriteAtlas()
        {
            // check if the sprite atlas textures needs to be updated
            for(int type = 0; type < GameState.SpriteAtlasManager.Length; type++)
            {
                GameState.SpriteAtlasManager.UpdateAtlasTexture(type);
            }

            // check if the tile sprite atlas textures needs to be updated
            for(int type = 0; type < GameState.TileSpriteAtlasManager.Length; type++)
            {
                GameState.TileSpriteAtlasManager.UpdateAtlasTexture(type);
            }
            
            ref Sprites.SpriteAtlas atlas = ref GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.Generic);
            Sprites.Sprite sprite = new Sprites.Sprite
            {
                Texture = atlas.Texture,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };
            Utility.Render.DrawSpriteNow(-3, -1, atlas.Width/32f, atlas.Height/32f, sprite, Material);
        }

        void DrawSprite(float x, float y, float w, float h, int spriteId)
        {
            var sprite = GameState.SpriteAtlasManager.GetSprite(spriteId, Enums.AtlasType.Generic);

            Utility.Render.DrawSpriteNow(x, y, w, h, sprite, Material);
        }
    }
}
