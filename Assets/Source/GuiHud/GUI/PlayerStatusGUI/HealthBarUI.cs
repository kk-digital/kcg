using UnityEngine;
using KGUI.Elements;

namespace KGUI.PlayerStatus
{
    public class HealthBarUI
    {
        // Init Condition
        private static bool Init;

        // Icon
        Sprites.Sprite icon;

        // Bar
        Sprites.Sprite barBorder;
        private Texture2D healthBar;

        Sprites.Sprite barDiv1;
        Sprites.Sprite barDiv2;

        // Player Health
        public float playerHealth;
        public Rect fillPosition = new Rect(78, 36, 226, 14);
        public Rect div1Position = new Rect(130, 52, 3, -17);
        public Rect div2Position = new Rect(190, 52, 3, -17);
        public Rect div3Position = new Rect(250, 52, 3, -17);
        public Rect borderPosition = new Rect(72, 54, 238, -20);
        public Rect IconPosition = new Rect(10, 75, 50, -50);
        public Rect TextPosition = new Rect(250, 60, 55, 22);
        public Color color = new Color(0.6f, 0, 0, 1.0f);

        Image Icon;

        public void Initialize(AgentEntity agentEntity)
        {
            // Set Width and Height
            int IconWidth = 19;
            int IconHeight = 19;
            Vector2Int iconPngSize = new Vector2Int(IconWidth, IconHeight);

            // Load image from file
            var iconSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\UserInterface\\Icons\\Health\\hud_hp_icon.png", IconWidth, IconHeight);

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
            int BarBorderWidth = 6;
            int BarBorderHeight = 8;
            Vector2Int BarBorderPngSize = new Vector2Int(BarBorderWidth, BarBorderHeight);

            // Load image from file
            var BarBorderSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\UserInterface\\Bars\\HealthBar\\hud_hp_bar_border.png", BarBorderWidth, BarBorderHeight);

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
            Vector2Int BarDiv1PngSize = new Vector2Int(BarDiv1Width, BarDiv1Height);

            // Load image from file
            var BarDiv1Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\UserInterface\\Bars\\HealthBar\\hud_hp_bar_div1.png", BarDiv1Width, BarDiv1Height);

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

            // Set Width and Height
            int BarDiv2Width = 1;
            int BarDiv2Height = 6;
            Vector2Int BarDiv2PngSize = new Vector2Int(BarDiv2Width, BarDiv2Height);

            // Load image from file
            var BarDiv2Sheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\UserInterface\\Bars\\HealthBar\\hud_hp_bar_div2.png", BarDiv2Width, BarDiv2Height);

            // Set Sprite ID from Sprite Atlas
            int BarDiv2ID = GameState.SpriteAtlasManager.CopySpriteToAtlas(BarDiv2Sheet, 0, 0, Enums.AtlasType.Particle);

            // Set Sprite Data
            byte[] BarDiv2SpriteData = new byte[BarDiv2PngSize.x * BarDiv2PngSize.y * 4];

            // Get Sprite Bytes
            GameState.SpriteAtlasManager.GetSpriteBytes(BarDiv2ID, BarDiv2SpriteData, Enums.AtlasType.Particle);

            // Set Texture
            Texture2D BarDiv2Tex = Utility.Texture.CreateTextureFromRGBA(BarDiv2SpriteData, BarDiv2PngSize.x, BarDiv2PngSize.y);

            // Create the sprite
            barDiv2 = new Sprites.Sprite
            {
                Texture = BarDiv2Tex,
                TextureCoords = new Vector4(0, 0, 1, 1)
            };

            // Add Components and setup agent object
            Sprite iconBar = Sprite.Create(icon.Texture, new Rect(0.0f, 0.0f, IconWidth, IconHeight), new Vector2(0.5f, 0.5f));

            
            Icon = new Image("Health Bar", iconBar);

            if (Camera.main.aspect >= 1.7f)
                Icon.SetPosition(new Vector3(-377.3f, 183.0f, 4.873917f));
            else if (Camera.main.aspect >= 1.5f)
                Icon.SetPosition(new Vector3(-335.6f, 9.6f, 4.873917f));
            else
                Icon.SetPosition(new Vector3(-364.8f, 255.3f, 4.873917f));

            Icon.SetScale(new Vector3(0.6f, -0.6f, 0.5203559f));

            healthBar = new Texture2D(100, 1);

            Init = true;
        }

        void DrawHealthBar(AgentEntity agentEntity)
        {
            playerHealth = agentEntity.agentStats.Health;
            
            ClearHealthBar();

            UpdateHealthBar((int)playerHealth);
        }

        private void ClearHealthBar()
        {
            // Clear Health Bar Texture
            healthBar = new Texture2D(100, 1);
        }

        public void UpdateHealthBar(int percantage)
        {
            int j = 0;
            for(; j < percantage; j++)
            {
                healthBar.SetPixel(j, 0, color);
            }
            healthBar.Apply();
            GUI.skin.box.normal.background = healthBar;
            GUI.backgroundColor = Color.white;
            GUI.Box(fillPosition, GUIContent.none);
        }

        public void Draw(AgentEntity agentEntity)
        {
            if(Init)
            {
                GUI.DrawTexture(borderPosition, barBorder.Texture);

                DrawHealthBar(agentEntity);

                // Update Div Textures for under 25
                if (playerHealth < 25)
                {
                    GUI.DrawTexture(div1Position, barDiv2.Texture);
                }
                else
                {
                    GUI.DrawTexture(div1Position, barDiv1.Texture);
                }

                // Update Div Textures for under 50
                if (playerHealth < 50)
                {
                    GUI.DrawTexture(div2Position, barDiv2.Texture);
                }
                else
                {
                    GUI.DrawTexture(div2Position, barDiv1.Texture);
                }

                // Update Div Textures for under 75
                if (playerHealth < 75)
                {
                    GUI.DrawTexture(div3Position, barDiv2.Texture);
                }
                else
                {
                    GUI.DrawTexture(div3Position, barDiv1.Texture);
                }

                if (Camera.main.aspect >= 1.7f)
                    Icon.SetPosition(new Vector3(-377.3f, 183.0f, 4.873917f));
                else if (Camera.main.aspect >= 1.5f)
                    Icon.SetPosition(new Vector3(-335.6f, 9.6f, 4.873917f));
                else
                    Icon.SetPosition(new Vector3(-364.8f, 255.3f, 4.873917f));
            }

            // Show Health With Text
            GUI.TextArea(TextPosition, playerHealth + "/100");
        }
    }
}
