using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KGUI
{
    public class FoodBarUI
    {
        // Init Condition
        private static bool Init;

        // Food Bar Icon Position
        public Rect iconPosition = new Rect(7, 140, 60, -60);

        // Food Bar Icon Sprite
        Sprites.Sprite icon;
        Sprites.Sprite fill;

        // Image
        private GameObject foodBar;

        public void Initialize(Transform transform)
        {
            // Set Width and Height
            int IconWidth = 19;
            int IconHeight = 19;
            Vector2Int iconPngSize = new Vector2Int(IconWidth, IconHeight);

            // Load image from file
            var iconSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\interface\\hud_status_food.png", IconWidth, IconHeight);

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
            int FillWidth = 19;
            int FillHeight = 19;
            Vector2Int FillPngSize = new Vector2Int(FillWidth, FillHeight);

            // Load image from file
            var FillSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\interface\\hud_status_fill.png", FillWidth, FillHeight);

            // Set Sprite ID from Sprite Atlas
            int FillID = GameState.SpriteAtlasManager.CopySpriteToAtlas(FillSheet, 0, 0, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] FillSpriteData = new byte[FillPngSize.x * FillPngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(FillID, FillSpriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D FillTex = Utility.Texture.CreateTextureFromRGBA(FillSpriteData, FillPngSize.x, FillPngSize.y);

            // Create the sprite
            fill = new Sprites.Sprite
            {
                Texture = FillTex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };


            foodBar = new GameObject("Food Bar");
            foodBar.transform.parent = transform;
            foodBar.AddComponent<RectTransform>();
            foodBar.AddComponent<Image>();

            Sprite bar = Sprite.Create(fill.Texture, new Rect(0.0f, 0.0f, FillWidth, FillHeight), new Vector2(0.5f, 0.5f));
            foodBar.GetComponent<Image>().sprite = bar;
            foodBar.GetComponent<Image>().raycastTarget = true;
            foodBar.GetComponent<Image>().maskable = true;
            foodBar.GetComponent<Image>().type = Image.Type.Filled;
            foodBar.GetComponent<Image>().fillMethod = Image.FillMethod.Radial360;
            foodBar.GetComponent<Image>().fillOrigin = 2;
            foodBar.GetComponent<Image>().fillAmount = 0.5f;
            foodBar.GetComponent<Image>().fillClockwise = false;
            foodBar.GetComponent<RectTransform>().position = new Vector3(iconPosition.x, iconPosition.y, 0.6831958f);
            foodBar.GetComponent<RectTransform>().localScale = new Vector3(iconPosition.width, iconPosition.height, 0.5203559f);

            Init = true;
        }

        public void Draw()
        {
            if(Init)
            {
                GUI.DrawTexture(iconPosition, icon.Texture);
            }
        }
    }
}
