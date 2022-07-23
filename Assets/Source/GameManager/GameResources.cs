using Enums.Tile;
using KMath;
using UnityEngine;
using System;
using PlanetTileMap;

public class GameResources
{
    // sprite sheets ids
    public static int LoadingTilePlaceholderSpriteSheet;
    public static int BackgroundSpriteSheet;
    public static int MoonSpriteSheet;
    public static int OreSpriteSheet;
    public static int Ore2SpriteSheet;
    public static int Ore3SpriteSheet;
    public static int GunSpriteSheet;
    public static int RockSpriteSheet;
    public static int RockDustSpriteSheet;
    public static int SlimeSpriteSheet;
    public static int CharacterSpriteSheet;
    public static int LaserSpriteSheet;
    public static int PipeSpriteSheet;

    public static int pipeIconSpriteSheet;
    public static int DustSpriteSheet;
    public static int GrenadeSpriteSheet;
    public static int SwordSpriteSheet;


    public static int OreSprite;
    public static int Ore2Sprite;
    public static int Ore3Sprite;

    //agent sprite ids
    public static int SlimeMoveLeftBaseSpriteId;
    public static int CharacterSpriteId;

    public static int GrenadeSpriteId;
    public static int SwordSpriteId;


    // particle sprite ids used for icons
    // TODO(): create icons atlas
    public static int DustBaseSpriteId;

    public static int OreIcon;
    public static int PistolIcon;
    public static int PulseIcon;
    public static int ShotgunIcon;
    public static int LongRifleIcon;
    public static int SniperRifleIcon;
    public static int RPGIcon;
    public static int SMGIcon;
    public static int SlimeIcon;
    public static int PlacementToolIcon;
    public static int RemoveToolIcon;
    public static int MiningLaserToolIcon;
    public static int PipePlacementToolIcon;



    public static int LoadingTilePlaceholderSpriteId;
    public static int LoadingTilePlaceholderTileId;

    private static bool IsInitialized = false; 


    public static void Initialize()
    {
        if (!IsInitialized)
        {
            long beginTime = DateTime.Now.Ticks;
            

            IsInitialized = true;
            // loading the sprite sheets
            LoadingTilePlaceholderSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Tiles\\Terrains\\placeholder_loadingSprite.png", 32, 32);
            BackgroundSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Tiles\\Terrains\\test - Copy.png", 16, 16);
            MoonSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Tiles\\Terrains\\Tiles_Moon.png", 16, 16);
            OreSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\Ores\\Gems\\Hexagon\\gem_hexagon_1.png", 16, 16);
            Ore2SpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\Ores\\Copper\\ore_copper_1.png", 16, 16);
            Ore3SpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\Ores\\Adamantine\\ore_adamantine_1.png", 16, 16);
            GunSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\Pistol\\gun-temp.png", 44, 25);
            ShotgunIcon = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Weapons\\Guns\\Pistol\\Guns\\Gun13.png", 48, 16);
            LongRifleIcon = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Weapons\\Guns\\Pistol\\Guns\\Gun10.png", 48, 16);
            SniperRifleIcon = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Weapons\\Guns\\Pistol\\Guns\\Gun8.png", 48, 16);
            PulseIcon = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Weapons\\Guns\\Pistol\\Guns\\Gun17.png", 48, 16);
            RPGIcon = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Weapons\\Guns\\Pistol\\Guns\\Gun18.png", 48, 16);
            SMGIcon = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Weapons\\Guns\\Pistol\\Guns\\Gun6.png", 48, 16);
            RockSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\MaterialIcons\\Rock\\rock1.png", 16, 16);
            RockDustSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\Rock\\rock1_dust.png", 16, 16);
            SlimeSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Enemies\\Slime\\slime.png", 32, 32);
            CharacterSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Characters\\Player\\character.png", 32, 48);
            LaserSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\RailGun\\lasergun-temp.png", 195, 79);
            PipeSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Furnitures\\Pipesim\\pipesim.png", 16, 16);
            pipeIconSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\AdminIcon\\Pipesim\\admin_icon_pipesim.png", 16, 16);
            DustSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Particles\\Dust\\dust1.png", 16, 16);
            GrenadeSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Projectiles\\Grenades\\Grenade\\Grenades1.png", 16, 16);
            SwordSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Weapons\\Swords\\Sword1.png", 16, 48);


            OreSprite = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(OreSpriteSheet, 0, 0, 0);
            Ore2Sprite = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(Ore2SpriteSheet, 0, 0, 0);
            Ore3Sprite = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(Ore3SpriteSheet, 0, 0, 0);

            // agent sprite atlas
            SlimeMoveLeftBaseSpriteId = GameState.SpriteAtlasManager.CopySpritesToAtlas(SlimeSpriteSheet, 0, 0, 3, 0, Enums.AtlasType.Agent);
            CharacterSpriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(CharacterSpriteSheet, 0, 0, Enums.AtlasType.Agent);

            GrenadeSpriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(GrenadeSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            SwordSpriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(SwordSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            // particle sprite atlas
            OreIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(OreSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            PistolIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(GunSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            ShotgunIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(ShotgunIcon, 0, 0, Enums.AtlasType.Particle);
            LongRifleIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(LongRifleIcon, 0, 0, Enums.AtlasType.Particle);
            SniperRifleIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(SniperRifleIcon, 0, 0, Enums.AtlasType.Particle);
            PulseIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(PulseIcon, 0, 0, Enums.AtlasType.Particle);
            RPGIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(RPGIcon, 0, 0, Enums.AtlasType.Particle);
            SMGIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(SMGIcon, 0, 0, Enums.AtlasType.Particle);
            SlimeIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(SlimeSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            PlacementToolIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(RockSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            RemoveToolIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(Ore2SpriteSheet, 0, 0, Enums.AtlasType.Particle);
            MiningLaserToolIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(LaserSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            PipePlacementToolIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(pipeIconSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            DustBaseSpriteId = GameState.SpriteAtlasManager.CopySpritesToAtlas(DustSpriteSheet, 0, 0, 5, 0, Enums.AtlasType.Particle);


            CreateTiles();
            CreateAnimations();
            CreateItems();
            CreateAgents();
            CreateParticles();
            CreateParticleEmitters();
            CreateProjectiles();



            Debug.Log("2d Assets Loading Time: " + (DateTime.Now.Ticks - beginTime) / TimeSpan.TicksPerMillisecond + " miliseconds");
        }
    }


    private static void CreateTiles()
    {
        LoadingTilePlaceholderSpriteId = 
                            GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas(LoadingTilePlaceholderSpriteSheet, 0, 0, 0);
        
        GameState.TileCreationApi.BeginMaterial(TileMaterialType.Placeholder);
        GameState.TileCreationApi.SetMaterialName("placeholder");

        LoadingTilePlaceholderTileId = GameState.TileCreationApi.CreateTileProperty();
        GameState.TileCreationApi.SetTilePropertyTexture16(LoadingTilePlaceholderSpriteId, 0, 0);
        GameState.TileCreationApi.EndTileProperty();

        GameState.TileCreationApi.EndMaterial();

        /*GameState.TileCreationApi.CreateTileProperty(TileID.Ore1);
        GameState.TileCreationApi.SetTilePropertyName("ore_1");
        GameState.TileCreationApi.SetTilePropertyShape(TileShape.FullBlock, TileShapeAndRotation.FB);
        GameState.TileCreationApi.SetTilePropertyTexture16(OreSpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTileProperty();*/


        GameState.TileCreationApi.BeginMaterial(TileMaterialType.Glass);
        GameState.TileCreationApi.SetMaterialName("glass");
        GameState.TileCreationApi.SetMaterialSpriteRuleType(SpriteRuleType.R3);
        for(int j = 0; j < 5; j++)
        {
            for(int i = 0; i < 11; i++)
            {
                GameState.TileCreationApi.CreateTileProperty();
                GameState.TileCreationApi.SetTilePropertyShape(TileShape.FullBlock, TileShapeAndRotation.FB);
                GameState.TileCreationApi.SetTilePropertyTexture16(MoonSpriteSheet, i + 11, j + 10);
                GameState.TileCreationApi.EndTileProperty();
            }
        }

        GameState.TileCreationApi.EndMaterial();

        /*GameState.TileCreationApi.CreateTileProperty(TileID.Glass);
        GameState.TileCreationApi.SetTilePropertyName("glass");
        GameState.TileCreationApi.SetTilePropertyShape(TileShape.FullBlock, TileShapeAndRotation.FB);
        GameState.TileCreationApi.SetSpriteRuleType(PlanetTileMap.SpriteRuleType.R3);
        GameState.TileCreationApi.SetTilePropertySpriteSheet16(MoonSpriteSheet, 11, 10);
        GameState.TileCreationApi.EndTileProperty();*/

        GameState.TileCreationApi.BeginMaterial(TileMaterialType.Moon);
        GameState.TileCreationApi.SetMaterialName("moon");
        GameState.TileCreationApi.SetMaterialSpriteRuleType(SpriteRuleType.R3);
        for(int j = 0; j < 5; j++)
        {
            for(int i = 0; i < 11; i++)
            {

            GameState.TileCreationApi.CreateTileProperty();
            GameState.TileCreationApi.SetTilePropertyShape(TileShape.FullBlock, TileShapeAndRotation.FB);
            GameState.TileCreationApi.SetTilePropertyTexture16(MoonSpriteSheet, i, j);
            GameState.TileCreationApi.EndTileProperty();
            }
        }

        GameState.TileCreationApi.EndMaterial();

        /*GameState.TileCreationApi.CreateTileProperty(TileID.Moon);
        GameState.TileCreationApi.SetTilePropertyName("moon");
        GameState.TileCreationApi.SetTilePropertyShape(TileShape.FullBlock, TileShapeAndRotation.FB);
        GameState.TileCreationApi.SetSpriteRuleType(PlanetTileMap.SpriteRuleType.R3);
        GameState.TileCreationApi.SetTilePropertySpriteSheet16(MoonSpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTileProperty();*/

        GameState.TileCreationApi.BeginMaterial(TileMaterialType.Background);
        GameState.TileCreationApi.SetMaterialName("background");
        GameState.TileCreationApi.SetMaterialSpriteRuleType(SpriteRuleType.R3);
        for(int j = 0; j < 5; j++)
        {
            for(int i = 0; i < 11; i++)
            {
                GameState.TileCreationApi.CreateTileProperty();
                GameState.TileCreationApi.SetTilePropertyTexture16(BackgroundSpriteSheet, i, j);
                GameState.TileCreationApi.EndTileProperty();

                GameState.TileCreationApi.EndMaterial();
            }
        }


        GameState.TileCreationApi.BeginMaterial(TileMaterialType.Platform);
        GameState.TileCreationApi.SetMaterialName("platform");

        GameState.TileCreationApi.CreateTileProperty();
        GameState.TileCreationApi.SetTilePropertyTexture16(RockSpriteSheet, 0, 0);

        GameState.TileCreationApi.SetTilePropertyCollisionType(CollisionType.Platform);

        GameState.TileCreationApi.EndTileProperty();

        GameState.TileCreationApi.EndMaterial();

        /*GameState.TileCreationApi.CreateTileProperty(TileID.Background);
        GameState.TileCreationApi.SetTilePropertyName("background");
        GameState.TileCreationApi.SetSpriteRuleType(PlanetTileMap.SpriteRuleType.R3);
        GameState.TileCreationApi.SetTilePropertySpriteSheet16(BackgroundSpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTileProperty();*/


        /*GameState.TileCreationApi.CreateTileProperty(TileID.Ore2);
        GameState.TileCreationApi.SetTilePropertyName("ore_2");
        GameState.TileCreationApi.SetTilePropertyShape(TileShape.FullBlock, TileShapeAndRotation.FB);
        GameState.TileCreationApi.SetTilePropertyTexture16(Ore2SpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTileProperty();*/


        /*GameState.TileCreationApi.CreateTileProperty(TileID.Ore3);
        GameState.TileCreationApi.SetTilePropertyName("ore_3");
        GameState.TileCreationApi.SetTilePropertyShape(TileShape.FullBlock, TileShapeAndRotation.FB);
        GameState.TileCreationApi.SetTilePropertyTexture16(Ore3SpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTileProperty();*/

        GameState.TileCreationApi.BeginMaterial(TileMaterialType.Pipe);
        GameState.TileCreationApi.SetMaterialName("pipe");

        GameState.TileCreationApi.SetMaterialSpriteRuleType(SpriteRuleType.R2);
        for(int j = 0; j < 4; j++)
        {
            for(int i = 0; i < 4; i++)
            {
                GameState.TileCreationApi.CreateTileProperty();
                GameState.TileCreationApi.SetTilePropertyTexture16(PipeSpriteSheet, i, j);
                GameState.TileCreationApi.EndTileProperty();
            }
        }

        GameState.TileCreationApi.EndMaterial();

        /*GameState.TileCreationApi.CreateTileProperty(TileID.Pipe);
        GameState.TileCreationApi.SetTilePropertyName("pipe");
        GameState.TileCreationApi.SetTilePropertyShape(TileShape.FullBlock, TileShapeAndRotation.FB);
        GameState.TileCreationApi.SetSpriteRuleType(PlanetTileMap.SpriteRuleType.R2);
        GameState.TileCreationApi.SetTilePropertySpriteSheet16(PipeSpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTileProperty();*/

    
        GameState.TileCreationApi.BeginMaterial(TileMaterialType.Wire);
        GameState.TileCreationApi.SetMaterialName("wire");

        GameState.TileCreationApi.SetMaterialSpriteRuleType(SpriteRuleType.R2);
        for(int j = 0; j < 4; j++)
        {
            for(int i = 0; i < 4; i++)
            {
                GameState.TileCreationApi.CreateTileProperty();
                GameState.TileCreationApi.SetTilePropertyTexture16(PipeSpriteSheet, i + 4, j + 12);
                GameState.TileCreationApi.EndTileProperty();
            }
        }

        GameState.TileCreationApi.EndMaterial();

        /*GameState.TileCreationApi.CreateTileProperty(TileID.Wire);
        GameState.TileCreationApi.SetTilePropertyName("wire");
        GameState.TileCreationApi.SetSpriteRuleType(PlanetTileMap.SpriteRuleType.R2);
        GameState.TileCreationApi.SetTilePropertySpriteSheet16(PipeSpriteSheet, 4, 12);
        GameState.TileCreationApi.EndTileProperty();*/

        GameState.TileCreationApi.BeginMaterial(TileMaterialType.Bedrock);
        GameState.TileCreationApi.SetMaterialName("Bedrock");
        GameState.TileCreationApi.SetMaterialCannotBeRemoved(true);
        
        GameState.TileCreationApi.SetMaterialSpriteRuleType(SpriteRuleType.R3);
        for(int j = 0; j < 5; j++)
        {
            for(int i = 0; i < 11; i++)
            {
                GameState.TileCreationApi.CreateTileProperty();
                GameState.TileCreationApi.SetTilePropertyShape(TileShape.FullBlock, TileShapeAndRotation.FB);
                GameState.TileCreationApi.SetTilePropertyTexture16(MoonSpriteSheet, i, j + 10);
                GameState.TileCreationApi.EndTileProperty();
            }
        }

        GameState.TileCreationApi.EndMaterial();

        /*GameState.TileCreationApi.CreateTileProperty(TileID.Bedrock);
        GameState.TileCreationApi.SetTilePropertyName("Bedrock");
        GameState.TileCreationApi.SetCannotBeRemoved(true);
        GameState.TileCreationApi.SetTilePropertyShape(TileShape.FullBlock, TileShapeAndRotation.FB);
        GameState.TileCreationApi.SetSpriteRuleType(PlanetTileMap.SpriteRuleType.R3);
        GameState.TileCreationApi.SetTilePropertySpriteSheet16(MoonSpriteSheet, 0, 10);
        GameState.TileCreationApi.EndTileProperty();*/

        GameState.TileCreationApi.BeginMaterial(TileMaterialType.Platform);
        GameState.TileCreationApi.CreateTileProperty();
        GameState.TileCreationApi.SetMaterialName("Platform");
        GameState.TileCreationApi.SetTilePropertyShape(TileShape.FullBlock, TileShapeAndRotation.FB);
        GameState.TileCreationApi.SetMaterialSpriteRuleType(SpriteRuleType.R3);
        GameState.TileCreationApi.EndTileProperty();
        GameState.TileCreationApi.EndMaterial();

        
    }

    private static void CreateAnimations()
    {
        GameState.AnimationManager.CreateAnimation((int)Animation.AnimationType.CharacterMoveLeft);
        GameState.AnimationManager.SetName("character-move-left");
        GameState.AnimationManager.SetTimePerFrame(0.15f);
        GameState.AnimationManager.SetBaseSpriteID(CharacterSpriteId);
        GameState.AnimationManager.SetFrameCount(1);
        GameState.AnimationManager.EndAnimation();

        GameState.AnimationManager.CreateAnimation((int)Animation.AnimationType.CharacterMoveLeft);
        GameState.AnimationManager.SetName("character-move-right");
        GameState.AnimationManager.SetTimePerFrame(0.15f);
        GameState.AnimationManager.SetBaseSpriteID(CharacterSpriteId);
        GameState.AnimationManager.SetFrameCount(1);
        GameState.AnimationManager.EndAnimation();

        GameState.AnimationManager.CreateAnimation((int)Animation.AnimationType.SlimeMoveLeft);
        GameState.AnimationManager.SetName("slime-move-left");
        GameState.AnimationManager.SetTimePerFrame(0.35f);
        GameState.AnimationManager.SetBaseSpriteID(SlimeMoveLeftBaseSpriteId);
        GameState.AnimationManager.SetFrameCount(4);
        GameState.AnimationManager.EndAnimation();

        GameState.AnimationManager.CreateAnimation((int)Animation.AnimationType.Dust);
        GameState.AnimationManager.SetName("dust");
        GameState.AnimationManager.SetTimePerFrame(0.075f);
        GameState.AnimationManager.SetBaseSpriteID(DustBaseSpriteId);
        GameState.AnimationManager.SetFrameCount(6);
        GameState.AnimationManager.EndAnimation();
    }

    public static void CreateItems()
    {
        // Sniper Rifle Item Creation
        GameState.ItemCreationApi.CreateItem(Enums.ItemType.SniperRifle, "SniperRifle");
        GameState.ItemCreationApi.SetTexture(SniperRifleIcon);
        GameState.ItemCreationApi.SetInventoryTexture(SniperRifleIcon);
        GameState.ItemCreationApi.SetRangedWeapon(200.0f, 1f, 350.0f, 60.0f);
        GameState.ItemCreationApi.SetRangedWeaponClip(6, 1, 1.3f);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetBullet(OreIcon, new Vec2f(0.2f, 0.2f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionFireWeapon);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.LongRifle, "LongRifle");
        GameState.ItemCreationApi.SetTexture(LongRifleIcon);
        GameState.ItemCreationApi.SetInventoryTexture(LongRifleIcon);
        GameState.ItemCreationApi.SetRangedWeapon(50.0f, 1f, 20.0f, 40.0f);
        GameState.ItemCreationApi.SetRangedWeaponClip(25, 1, 2f);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetBullet(OreIcon, new Vec2f(0.2f, 0.2f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionFireWeapon);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.PulseWeapon, "PulseWeapon");
        GameState.ItemCreationApi.SetTexture(PulseIcon);
        GameState.ItemCreationApi.SetInventoryTexture(PulseIcon);
        GameState.ItemCreationApi.SetRangedWeapon(20.0f, 0.5f, 10.0f, false, 25.0f);
        GameState.ItemCreationApi.SetRangedWeaponClip(25, 4, 1, 1);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetBullet(OreIcon, new Vec2f(0.2f, 0.2f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionPulseWeapon);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.AutoCannon, "AutoCannon");
        GameState.ItemCreationApi.SetTexture(LongRifleIcon);
        GameState.ItemCreationApi.SetInventoryTexture(LongRifleIcon);
        GameState.ItemCreationApi.SetRangedWeapon(50.0f, 0.5f, 20.0f, 40.0f);
        GameState.ItemCreationApi.SetRangedWeaponClip(40, 3, 4f);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetBullet(OreIcon, new Vec2f(0.2f, 0.2f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionFireWeapon);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.SMG, "SMG");
        GameState.ItemCreationApi.SetTexture(SMGIcon);
        GameState.ItemCreationApi.SetInventoryTexture(SMGIcon);
        GameState.ItemCreationApi.SetRangedWeapon(50.0f, 0.2f, 20.0f, 15.0f);
        GameState.ItemCreationApi.SetRangedWeaponClip(30, 1, 1f);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetBullet(OreIcon, new Vec2f(0.2f, 0.2f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionFireWeapon);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.Shotgun, "Shotgun");
        GameState.ItemCreationApi.SetTexture(ShotgunIcon);
        GameState.ItemCreationApi.SetInventoryTexture(ShotgunIcon);
        GameState.ItemCreationApi.SetRangedWeapon(30.0f, 1f, 10.0f, 35.0f);
        GameState.ItemCreationApi.SetSpreadAngle(1.0f);
        GameState.ItemCreationApi.SetRangedWeaponClip(6, 2, 2.5f);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetBullet(OreIcon, new Vec2f(0.2f, 0.2f));
        GameState.ItemCreationApi.SetFlags(Item.FireWeaponPropreties.Flags.ShouldSpread);
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionFireWeapon);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.PumpShotgun, "PumpShotgun");
        GameState.ItemCreationApi.SetTexture(ShotgunIcon);
        GameState.ItemCreationApi.SetInventoryTexture(ShotgunIcon);
        GameState.ItemCreationApi.SetRangedWeapon(20.0f, 2f, 5.0f, 30.0f);
        GameState.ItemCreationApi.SetSpreadAngle(1.0f);
        GameState.ItemCreationApi.SetRangedWeaponClip(8, 4, 2.5f);
        GameState.ItemCreationApi.SetFlags(Item.FireWeaponPropreties.Flags.ShouldSpread);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetBullet(OreIcon, new Vec2f(0.2f, 0.2f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionFireWeapon);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.Pistol, "Pistol");
        GameState.ItemCreationApi.SetTexture(PistolIcon);
        GameState.ItemCreationApi.SetInventoryTexture(PistolIcon);
        GameState.ItemCreationApi.SetRangedWeapon(20.0f, 1f, 10.0f, 25.0f);
        GameState.ItemCreationApi.SetRangedWeaponClip(8, 1, 1f);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetBullet(OreIcon, new Vec2f(0.2f, 0.2f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionFireWeapon);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.RPG, "RPG");
        GameState.ItemCreationApi.SetTexture(RPGIcon);
        GameState.ItemCreationApi.SetInventoryTexture(RPGIcon);
        GameState.ItemCreationApi.SetRangedWeapon(50.0f, 3f, 50.0f, 100.0f);
        GameState.ItemCreationApi.SetRangedWeaponClip(2, 1, 3);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetBullet(GrenadeSpriteId, new Vec2f(0.2f, 0.2f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionThrowGrenade);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.GrenadeLauncher, "GrenadeLauncher");
        GameState.ItemCreationApi.SetTexture(GrenadeSpriteId);
        GameState.ItemCreationApi.SetInventoryTexture(GrenadeSpriteId);
        GameState.ItemCreationApi.SetRangedWeapon(20.0f, 1f, 20.0f, 25.0f);
        GameState.ItemCreationApi.SetRangedWeaponClip(4, 1, 2);
        GameState.ItemCreationApi.SetFlags(Item.FireWeaponPropreties.GrenadesFlags.Flame);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetBullet(GrenadeSpriteId, new Vec2f(0.2f, 0.2f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionThrowGrenade);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.Bow, "Bow");
        GameState.ItemCreationApi.SetTexture(PistolIcon);
        GameState.ItemCreationApi.SetInventoryTexture(PistolIcon);
        GameState.ItemCreationApi.SetRangedWeapon(70.0f, 3f, 100.0f, 30.0f);
        GameState.ItemCreationApi.SetRangedWeaponClip(1, 1, 2f);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetBullet(OreIcon, new Vec2f(0.2f, 0.2f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionFireWeapon);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.Sword, "Sword");
        GameState.ItemCreationApi.SetTexture(SwordSpriteId);
        GameState.ItemCreationApi.SetInventoryTexture(SwordSpriteId);
        GameState.ItemCreationApi.SetMeleeWeapon(1.0f, 2.0f, 0.5f, 1.0f, 10.0f);
        GameState.ItemCreationApi.SetFlags(Item.FireWeaponPropreties.MeleeFlags.Stab);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionMeleeAttack);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.StunBaton, "StunBaton");
        GameState.ItemCreationApi.SetTexture(SwordSpriteId);
        GameState.ItemCreationApi.SetInventoryTexture(SwordSpriteId);
        GameState.ItemCreationApi.SetMeleeWeapon(0.5f, 2.0f, 1.0f, 1.0f, 5.0f);
        GameState.ItemCreationApi.SetFlags(Item.FireWeaponPropreties.MeleeFlags.Slash);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionMeleeAttack);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.RiotShield, "RiotShield");
        GameState.ItemCreationApi.SetTexture(SwordSpriteId);
        GameState.ItemCreationApi.SetInventoryTexture(SwordSpriteId);
        GameState.ItemCreationApi.SetShield(false);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionShield);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.Ore, "Ore");
        GameState.ItemCreationApi.SetTexture(OreIcon);
        GameState.ItemCreationApi.SetInventoryTexture(OreIcon);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetStackable(99);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.PlacementTool, "PlacementTool");
        GameState.ItemCreationApi.SetTexture(PlacementToolIcon);
        GameState.ItemCreationApi.SetInventoryTexture(PlacementToolIcon);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.PlaceTilMoonAction);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.PlacementToolBack, "BackgroundPlacementTool");
        GameState.ItemCreationApi.SetTexture(PlacementToolIcon);
        GameState.ItemCreationApi.SetInventoryTexture(PlacementToolIcon);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.PlaceTilBackgroundAction);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.RemoveTileTool, "RemoveTileTool");
        GameState.ItemCreationApi.SetTexture(RemoveToolIcon);
        GameState.ItemCreationApi.SetInventoryTexture(RemoveToolIcon);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionRemoveTile);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.SpawnEnemySlimeTool, "SpawnSlimeTool");
        GameState.ItemCreationApi.SetTexture(SlimeIcon);
        GameState.ItemCreationApi.SetInventoryTexture(SlimeIcon);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionEnemySpawn);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.MiningLaserTool, "MiningLaserTool");
        GameState.ItemCreationApi.SetTexture(MiningLaserToolIcon);
        GameState.ItemCreationApi.SetInventoryTexture(MiningLaserToolIcon);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionMiningLaser);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.PipePlacementTool, "PipePlacementTool");
        GameState.ItemCreationApi.SetTexture(PipePlacementToolIcon);
        GameState.ItemCreationApi.SetInventoryTexture(PipePlacementToolIcon);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.PlaceTilPipeAction);
        GameState.ItemCreationApi.EndItem();

        GameState.ItemCreationApi.CreateItem(Enums.ItemType.ParticleEmitterPlacementTool, "ParticleEmitterPlacementTool");
        GameState.ItemCreationApi.SetTexture(OreIcon);
        GameState.ItemCreationApi.SetInventoryTexture(OreIcon);
        GameState.ItemCreationApi.SetSpriteSize(new Vec2f(0.5f, 0.5f));
        GameState.ItemCreationApi.SetAction(Enums.ActionType.ToolActionPlaceParticle);
        GameState.ItemCreationApi.EndItem();
    }

    private static void CreateAgents()
    {
        GameState.AgentCreationApi.Create((int)Agent.AgentType.Player);
        GameState.AgentCreationApi.SetName("player");
        GameState.AgentCreationApi.SetSpriteSize(new Vec2f(1.0f, 1.5f));
        GameState.AgentCreationApi.SetCollisionBox(new Vec2f(0.25f, 0.0f), new Vec2f(0.5f, 1.5f));
        GameState.AgentCreationApi.SetStartingAnimation((int)Animation.AnimationType.CharacterMoveLeft);
        GameState.AgentCreationApi.End();

        GameState.AgentCreationApi.Create((int)Agent.AgentType.Agent);
        GameState.AgentCreationApi.SetName("agent");
        GameState.AgentCreationApi.SetSpriteSize(new Vec2f(1.0f, 1.5f));
        GameState.AgentCreationApi.SetCollisionBox(new Vec2f(0.25f, 0.0f), new Vec2f(0.5f, 1.5f));
        GameState.AgentCreationApi.SetStartingAnimation((int)Animation.AnimationType.CharacterMoveLeft);
        GameState.AgentCreationApi.End();

        GameState.AgentCreationApi.Create((int)Agent.AgentType.Enemy);
        GameState.AgentCreationApi.SetName("enemy");
        GameState.AgentCreationApi.SetSpriteSize(new Vec2f(1.0f, 1.0f));
        GameState.AgentCreationApi.SetCollisionBox(new Vec2f(0.125f, 0.0f), new Vec2f(0.75f, 0.5f));
        GameState.AgentCreationApi.SetStartingAnimation((int)Animation.AnimationType.SlimeMoveLeft);
        GameState.AgentCreationApi.SetEnemyBehaviour(0);
        GameState.AgentCreationApi.SetDetectionRadius(4.0f);
        GameState.AgentCreationApi.SetHealth(100.0f);
        GameState.AgentCreationApi.SetAttackCooldown(0.8f);
        GameState.AgentCreationApi.End();
    }

    private static void CreateParticles()
    {
        GameState.ParticleCreationApi.Create((int)Particle.ParticleType.Ore);
        GameState.ParticleCreationApi.SetName("Ore");
        GameState.ParticleCreationApi.SetDecayRate(1.0f);
        GameState.ParticleCreationApi.SetAcceleration(new Vector2(0.0f, -20.0f));
        GameState.ParticleCreationApi.SetDeltaRotation(90.0f);
        GameState.ParticleCreationApi.SetDeltaScale(0.0f);
        GameState.ParticleCreationApi.SetSpriteId(OreIcon);
        GameState.ParticleCreationApi.SetSize(new Vec2f(0.5f, 0.5f));
        GameState.ParticleCreationApi.SetStartingVelocity(new Vector2(1.0f, 10.0f));
        GameState.ParticleCreationApi.SetStartingRotation(0.0f);
        GameState.ParticleCreationApi.SetStartingScale(1.0f);
        GameState.ParticleCreationApi.SetStartingColor(new Color(255.0f, 255.0f, 255.0f, 255.0f));
        GameState.ParticleCreationApi.End();

        GameState.ParticleCreationApi.Create((int)Particle.ParticleType.OreExplosionParticle);
        GameState.ParticleCreationApi.SetName("ore-explosion-particle");
        GameState.ParticleCreationApi.SetDecayRate(1.0f);
        GameState.ParticleCreationApi.SetAcceleration(new Vector2(0.0f, 0.0f));
        GameState.ParticleCreationApi.SetDeltaRotation(130.0f);
        GameState.ParticleCreationApi.SetDeltaScale(-1.0f);
        GameState.ParticleCreationApi.SetSpriteId(OreIcon);
        GameState.ParticleCreationApi.SetSize(new Vec2f(0.5f, 0.5f));
        GameState.ParticleCreationApi.SetStartingVelocity(new Vector2(0.0f, 0.0f));
        GameState.ParticleCreationApi.SetStartingRotation(0.0f);
        GameState.ParticleCreationApi.SetStartingScale(1.0f);
        GameState.ParticleCreationApi.SetStartingColor(new Color(255.0f, 255.0f, 255.0f, 255.0f));
        GameState.ParticleCreationApi.End();

        GameState.ParticleCreationApi.Create((int)Particle.ParticleType.DustParticle);
        GameState.ParticleCreationApi.SetName("dust-particle");
        GameState.ParticleCreationApi.SetDecayRate(4.0f);
        GameState.ParticleCreationApi.SetAcceleration(new Vector2(0.0f, 0.0f));
        GameState.ParticleCreationApi.SetDeltaRotation(0);
        GameState.ParticleCreationApi.SetDeltaScale(-1.0f);
        GameState.ParticleCreationApi.SetAnimationType(Animation.AnimationType.Dust);
        GameState.ParticleCreationApi.SetSize(new Vec2f(0.5f, 0.5f));
        GameState.ParticleCreationApi.SetStartingVelocity(new Vector2(0.0f, 0.0f));
        GameState.ParticleCreationApi.SetStartingRotation(0.0f);
        GameState.ParticleCreationApi.SetStartingScale(1.0f);
        GameState.ParticleCreationApi.SetStartingColor(new Color(255.0f, 255.0f, 255.0f, 255.0f));
        GameState.ParticleCreationApi.End();
    }

    private static void CreateParticleEmitters()
    {
        GameState.ParticleEmitterCreationApi.Create((int)Particle.ParticleEmitterType.OreFountain);
        GameState.ParticleEmitterCreationApi.SetName("ore-fountain");
        GameState.ParticleEmitterCreationApi.SetParticleType(Particle.ParticleType.Ore);
        GameState.ParticleEmitterCreationApi.SetDuration(0.5f);
        GameState.ParticleEmitterCreationApi.SetParticleCount(1);
        GameState.ParticleEmitterCreationApi.SetTimeBetweenEmissions(0.05f);
        GameState.ParticleEmitterCreationApi.SetVelocityInterval(new Vec2f(-1.0f, 0.0f), new Vec2f(1.0f, 0.0f));
        GameState.ParticleEmitterCreationApi.End();

        GameState.ParticleEmitterCreationApi.Create((int)Particle.ParticleEmitterType.OreExplosion);
        GameState.ParticleEmitterCreationApi.SetName("ore-explosion");
        GameState.ParticleEmitterCreationApi.SetParticleType(Particle.ParticleType.OreExplosionParticle);
        GameState.ParticleEmitterCreationApi.SetDuration(0.15f);
        GameState.ParticleEmitterCreationApi.SetSpawnRadius(0.1f);
        GameState.ParticleEmitterCreationApi.SetParticleCount(15);
        GameState.ParticleEmitterCreationApi.SetTimeBetweenEmissions(1.0f);
        GameState.ParticleEmitterCreationApi.SetVelocityInterval(new Vec2f(-10.0f, -10.0f), new Vec2f(10.0f, 10.0f));
        GameState.ParticleEmitterCreationApi.End();
        
        GameState.ParticleEmitterCreationApi.Create((int)Particle.ParticleEmitterType.DustEmitter);
        GameState.ParticleEmitterCreationApi.SetName("dust-emitter");
        GameState.ParticleEmitterCreationApi.SetParticleType(Particle.ParticleType.DustParticle);
        GameState.ParticleEmitterCreationApi.SetDuration(0.1f);
        GameState.ParticleEmitterCreationApi.SetSpawnRadius(0.1f);
        GameState.ParticleEmitterCreationApi.SetParticleCount(1);
        GameState.ParticleEmitterCreationApi.SetTimeBetweenEmissions(1.02f);
        GameState.ParticleEmitterCreationApi.SetVelocityInterval(new Vec2f(0.0f, 0), new Vec2f(0.0f, 0));
        GameState.ParticleEmitterCreationApi.End();
    }


    private static void CreateProjectiles()
    {
        GameState.ProjectileCreationApi.Create((int)Enums.ProjectileType.Bullet);
        GameState.ProjectileCreationApi.SetName("bullet");
        GameState.ProjectileCreationApi.SetSpriteId(OreIcon);
        GameState.ProjectileCreationApi.SetSize(new Vec2f(0.5f, 0.5f));
        GameState.ProjectileCreationApi.SetSpeed(20.0f);
        GameState.ProjectileCreationApi.SetRamp(false, 1f, 10f, 1.0f);
        GameState.ProjectileCreationApi.SetDragType(Enums.DragType.Linear);
        GameState.ProjectileCreationApi.SetLinearDrag(0.73f, 0.01f);
        GameState.ProjectileCreationApi.SetAffectedByGravity(true);
        GameState.ProjectileCreationApi.SetAcceleration(new Vec2f());
        GameState.ProjectileCreationApi.End();

        GameState.ProjectileCreationApi.Create((int)Enums.ProjectileType.Grenade);
        GameState.ProjectileCreationApi.SetName("grenade");
        GameState.ProjectileCreationApi.SetSpriteId(GrenadeSpriteId);
        GameState.ProjectileCreationApi.SetDeltaRotation(180.0f);
        GameState.ProjectileCreationApi.SetSize(new Vec2f(0.5f, 0.5f));
        GameState.ProjectileCreationApi.SetSpeed(10.0f);
        GameState.ProjectileCreationApi.SetAcceleration(new Vec2f(0.0f, -10.0f));
        GameState.ProjectileCreationApi.End();

        GameState.ProjectileCreationApi.Create((int)Enums.ProjectileType.Rocket);
        GameState.ProjectileCreationApi.SetName("rocket");
        GameState.ProjectileCreationApi.SetSpriteId(GrenadeSpriteId);
        GameState.ProjectileCreationApi.SetAffectedByGravity(true);
        GameState.ProjectileCreationApi.SetDeltaRotation(180.0f);
        GameState.ProjectileCreationApi.SetSize(new Vec2f(0.5f, 0.5f));
        GameState.ProjectileCreationApi.SetSpeed(20.0f);
        GameState.ProjectileCreationApi.End();

        GameState.ProjectileCreationApi.Create((int)Enums.ProjectileType.Arrow);
        GameState.ProjectileCreationApi.SetName("arrow");
        GameState.ProjectileCreationApi.SetSpriteId(OreIcon);
        GameState.ProjectileCreationApi.SetAffectedByGravity(false);
        GameState.ProjectileCreationApi.SetDeltaRotation(180.0f);
        GameState.ProjectileCreationApi.SetSize(new Vec2f(0.5f, 0.5f));
        GameState.ProjectileCreationApi.SetSpeed(20.0f);
        GameState.ProjectileCreationApi.End();
    }
}
