using UnityEngine;
using UnityEngine.UI;
using Entitas;
using KGUI.Elements;

namespace KGUI.PlayerStatus
{
    public class FuelBarUI
    {
        // Init
        private static bool Init;

        // Fuel Bar Icon Position
        public Rect iconPosition = new Rect(7, 140, 60, -60);

        // Fuel Bar Icon Sprite
        Sprites.Sprite icon;
        Sprites.Sprite fill;

        // Progress Bar
        public ProgressBar progressBar;
        private GameObject iconCanvas;

        public void Initialize(AgentEntity agentEntity)
        {
            // Set Width and Height
            int IconWidth = 19;
            int IconHeight = 19;
            Vector2Int iconPngSize = new Vector2Int(IconWidth, IconHeight);

            // Load image from file
            var iconSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\UserInterface\\Icons\\Fuel\\hud_status_fuel.png", IconWidth, IconHeight);

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
            var FillSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\UserInterface\\Bars\\CircleBar\\hud_status_fill.png", FillWidth, FillHeight);

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

            // Fuel Bar Initializon
            iconCanvas = new GameObject("Fuel Icon");
            iconCanvas.transform.parent = GameObject.Find("Canvas").transform;
            iconCanvas.AddComponent<RectTransform>();
            iconCanvas.AddComponent<Image>();

            // Add Components and setup game object
            Sprite iconBar = Sprite.Create(icon.Texture, new Rect(0.0f, 0.0f, IconWidth, IconHeight), new Vector2(0.5f, 0.5f));
            iconCanvas.GetComponent<Image>().sprite = iconBar;

            // Calculate position using aspect ratio
            if (Camera.main.aspect >= 1.7f)
                iconCanvas.GetComponent<RectTransform>().localPosition = new Vector3(-377.3f, -52.6f, 4.873917f);
            else if (Camera.main.aspect >= 1.5f)
                iconCanvas.GetComponent<RectTransform>().localPosition = new Vector3(-335.6f, -49.2f, 4.873917f);
            else
                iconCanvas.GetComponent<RectTransform>().localPosition = new Vector3(-363.8f, 16.6f, 4.873917f);

            iconCanvas.GetComponent<RectTransform>().localScale = new Vector3(0.6f, -0.6f, 0.5203559f);

            // Add Components and setup game object
            Sprite bar = Sprite.Create(fill.Texture, new Rect(0.0f, 0.0f, FillWidth, FillHeight), new Vector2(0.5f, 0.5f));

            // Fuel Bar Initializon
            progressBar = new ProgressBar("Fuel Bar",iconCanvas.transform, bar, Image.FillMethod.Radial360, agentEntity.agentStats.Fuel / 100, agentEntity);
            progressBar.SetPosition(new Vector3(-0.4f, -0.1f, 4.873917f));
            progressBar.SetScale(new Vector3(0.8566527f, 0.8566527f, 0.3714702f));

            Init = true;
        }

        public void Update(AgentEntity agentEntity)
        {
            if(Init)
            {
                float fuelValue = agentEntity.agentStats.Fuel;
                if (fuelValue <= 0)
                {
                    fuelValue = 0;
                }
                progressBar.Update(fuelValue / 100);

                // Calculate position using aspect ratio
                if (Camera.main.aspect >= 1.7f)
                    iconCanvas.GetComponent<RectTransform>().localPosition = new Vector3(-377.3f, -52.6f, 4.873917f);
                else if (Camera.main.aspect >= 1.5f)
                    iconCanvas.GetComponent<RectTransform>().localPosition = new Vector3(-335.6f, -49.2f, 4.873917f);
                else
                    iconCanvas.GetComponent<RectTransform>().localPosition = new Vector3(-363.8f, 16.6f, 4.873917f);
            }
        }
    }
}
