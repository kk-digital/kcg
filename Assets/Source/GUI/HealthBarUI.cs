using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using KMath;

namespace KGUI
{
    public class HealthBarUI
    {
        // Init Condition
        private static bool Init;

        // Icon
        Sprites.Sprite icon;

        // Bar
        Sprites.Sprite barFill;
        Sprites.Sprite barBorder;
        Sprites.Sprite barDiv1;

        // Player Health
        float playerHealth;

        public void Initialize(Material material, Transform transform)
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
            int BarFillWidth = 5;
            int BarFillHeight = 5;
            Vector2Int BarPngSize = new Vector2Int(BarFillWidth, BarFillHeight);

            // Load image from file
            var BarFillSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\interface\\hud_hp_bar_fill.png", BarFillWidth, BarFillHeight);

            // Set Sprite ID from Sprite Atlas
            int BarFillID = GameState.SpriteAtlasManager.CopySpriteToAtlas(BarFillSheet, 0, 0, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] BarFillSpriteData = new byte[BarPngSize.x * BarPngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(BarFillID, BarFillSpriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D BarFillTex = Utility.Texture.CreateTextureFromRGBA(BarFillSpriteData, BarPngSize.x, BarPngSize.y);

            // Create the sprite
            barFill = new Sprites.Sprite
            {
                Texture = BarFillTex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };

            // Set Width and Height
            int BarBorderWidth = 6;
            int BarBorderHeight = 8;
            Vector2Int BarBorderPngSize = new Vector2Int(BarBorderWidth, BarBorderHeight);

            // Load image from file
            var BarBorderSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\interface\\hud_hp_bar_border.png", BarBorderWidth, BarBorderHeight);

            // Set Sprite ID from Sprite Atlas
            int BarBorderID = GameState.SpriteAtlasManager.CopySpriteToAtlas(BarBorderSheet, 0, 0, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] BarBorderSpriteData = new byte[BarBorderPngSize.x * BarBorderPngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(BarBorderID, BarBorderSpriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D BarBorderTex = Utility.Texture.CreateTextureFromRGBA(BarBorderSpriteData, BarBorderPngSize.x, BarBorderPngSize.y);

            // Create the sprite
            barBorder = new Sprites.Sprite
            {
                Texture = BarBorderTex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };

            // Set Width and Height
            int BarDiv1Width = 1;
            int BarDiv1Height = 6;
            Vector2Int BarDiv1PngSize = new Vector2Int(BarBorderWidth, BarBorderHeight);

            // Load image from file
            var BarDiv1Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\interface\\hud_hp_bar_div1.png", BarDiv1Width, BarDiv1Height);

            // Set Sprite ID from Sprite Atlas
            int BarDiv1ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(BarDiv1Sheet, 0, 0, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] BarDiv1SpriteData = new byte[BarDiv1PngSize.x * BarDiv1PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(BarDiv1ID, BarDiv1SpriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D BarDiv1Tex = Utility.Texture.CreateTextureFromRGBA(BarDiv1SpriteData, BarDiv1PngSize.x, BarDiv1PngSize.y);

            // Create the sprite
            barDiv1 = new Sprites.Sprite
            {
                Texture = BarDiv1Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };

            // Get Player Health
            IGroup<GameEntity> entities =
            Contexts.sharedInstance.game.GetGroup(GameMatcher.AgentStats);
            foreach (var entity in entities)
            {
                playerHealth = entity.agentStats.Health;
            }

            Init = true;
        }   

        public void Draw(Material material, Transform transform, int drawOrder)
        {
            if(Init)
            {
                // Get Initial Positon.
                float Iconx = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0, Camera.main.nearClipPlane)).x;
                float Icony = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight - 60, Camera.main.nearClipPlane)).y;

                float BarFillx = Camera.main.ScreenToWorldPoint(new Vector3(50.0f, 0, Camera.main.nearClipPlane)).x;
                float BarFilly = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight - 50, Camera.main.nearClipPlane)).y;

                float BarBorderx = Camera.main.ScreenToWorldPoint(new Vector3(45.0f, 0, Camera.main.nearClipPlane)).x;
                float BarBordery = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight - 53.5f, Camera.main.nearClipPlane)).y;

                float BarDiv1x = Camera.main.ScreenToWorldPoint(new Vector3(115.0f, 0, Camera.main.nearClipPlane)).x;
                float BarDiv1y = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight - 156, Camera.main.nearClipPlane)).y;

                float BarDiv1_2x = Camera.main.ScreenToWorldPoint(new Vector3(215.0f, 0, Camera.main.nearClipPlane)).x;
                float BarDiv1_2y = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight - 156, Camera.main.nearClipPlane)).y;

                Utility.Render.DrawSprite(Iconx, Icony, 0.5f, 0.5f, icon, material, transform, drawOrder);
                
                // Health Bar Border Draw
                Utility.Render.DrawSprite(BarBorderx, BarBordery, 3.757031f, 0.309375f, barBorder, material, transform, 5000);

                // Health Bar Filled Draw
                Utility.Render.DrawSprite(BarFillx, BarFilly, 3.5941875f, 0.22574003f, barFill, material, transform, 5001);

                // Health Bar Div 1 Draw
                Utility.Render.DrawSprite(BarDiv1x, BarDiv1y, 1, 1, 270, barDiv1, material, transform, 5002);

                Utility.Render.DrawSprite(BarDiv1_2x, BarDiv1_2y, 1, 1, 270, barDiv1, material, transform, 5002);

            }
        }
    }
}
