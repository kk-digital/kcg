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

    class TileSpriteAtlasTest : MonoBehaviour
    {
        //public string TileMap = "Moonbunker/Moon Bunker.tmx";
        [SerializeField] Material Material;

        public static string BaseDir => Application.streamingAssetsPath;

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
            DrawSprite(2, 1, 1.0f, 1.0f, 0);
            DrawSprite(2, -1, 1.0f, 1.0f, 3);
        }

        // create the sprite atlas for testing purposes
        public void LoadSprites()
        {
            int MetalSlabsTileSheet = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Tiles\\Blocks\\BuildingBlocks\\Metal\\Slabs\\Tiles_metal_slabs.png", 16, 16);
            int StoneBulkheads = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Tiles\\Blocks\\BuildingBlocks\\Stone\\Bulkheads\\Tiles_stone_bulkheads.png", 16, 16);
            int TilesMoon = 
                        GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Tiles\\Terrains\\Tiles_Moon.png", 16, 16);
            int OreTileSheet = 
            GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\Ores\\Gems\\Hexagon\\gem_hexagon_1.png", 16, 16);


            GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(MetalSlabsTileSheet, 0, 0, 0);
            GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(StoneBulkheads, 1, 0, 0);
            GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(TilesMoon, 4, 0, 0);
            GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(OreTileSheet, 5, 0, 0);
        }

        // drawing the sprite atlas
        void DrawSpriteAtlas()
        {
            ref Sprites.SpriteAtlas atlas = ref GameState.TileSpriteAtlasManager.GetSpriteAtlas(0);
            Sprites.Sprite sprite = new Sprites.Sprite();
            sprite.Texture = atlas.Texture;
            sprite.TextureCoords = new Vector4(0, 0, 1, 1);
            
            Utility.Render.DrawSpriteNow(-3, -1, 
                  atlas.Width, atlas.Height, sprite, Material);
        }

        void DrawSprite(float x, float y, float w, float h, int spriteId)
        {
            var sprite = GameState.TileSpriteAtlasManager.GetSprite(spriteId);

            Utility.Render.DrawSpriteNow(x, y, w, h, sprite, Material);
        }
    }
}

