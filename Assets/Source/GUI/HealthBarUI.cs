using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KGUI
{
    public class HealthBarUI
    {
        // Init Condition
        private static bool Init;

        // Icon
        Sprites.Sprite icon;

        // Bar
        Sprites.Sprite bar;

        public void Initialize()
        {
            // Set Width and Height
            int IconWidth = 19;
            int IconHeight = 19;
            Vector2Int iconPngSize = new Vector2Int(IconWidth, IconHeight);

            // Load image from file
            var iconSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\interface\\hud_hp_icon.png", IconWidth, IconHeight);

            // Set Sprite ID from Sprite Atlas
            int iconID = GameState.SpriteAtlasManager.CopySpriteToAtlas(iconSheet, 0, 0, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] iconSpriteData = new byte[iconPngSize.x * iconPngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(iconID, iconSpriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D iconTex = Utility.Texture.CreateTextureFromRGBA(iconSpriteData, iconPngSize.x, iconPngSize.y);

            // Create the sprite
            icon = new Sprites.Sprite
            {
                Texture = iconTex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };

            // Set Width and Height
            int BarWidth = 5;
            int BarHeight = 5;
            Vector2Int BarPngSize = new Vector2Int(BarWidth, BarHeight);

            // Load image from file
            var BarSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\interface\\hud_hp_bar_fill.png", BarWidth, BarHeight);

            // Set Sprite ID from Sprite Atlas
            int BarID = GameState.SpriteAtlasManager.CopySpriteToAtlas(BarSheet, 0, 0, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] BarSpriteData = new byte[BarPngSize.x * BarPngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(BarID, BarSpriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D BarTex = Utility.Texture.CreateTextureFromRGBA(BarSpriteData, BarPngSize.x, BarPngSize.y);

            // Create the sprite
            bar = new Sprites.Sprite
            {
                Texture = BarTex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };

            Init = true;
        }   

        public void Draw(Material material, Transform transform, int drawOrder)
        {
            if(Init)
            {
                // Get Initial Positon.
                float Iconx = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
                float Icony = Camera.main.ScreenToWorldPoint(Vector3.zero).y + 6.0f;

                float Barx = Camera.main.ScreenToWorldPoint(Vector3.zero).x + 0.5f;
                float Bary = Camera.main.ScreenToWorldPoint(Vector3.zero).y + 6.3f;

                Utility.Render.DrawSprite(Iconx, Icony, 1, 1, icon, material, transform, drawOrder);

                Utility.Render.DrawSprite(Barx, Bary, 3.52453136f, 0.225585938f, bar, material, transform, drawOrder);
            }
        }
    }
}
