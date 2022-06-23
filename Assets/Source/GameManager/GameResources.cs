using KMath;


public class GameResources
{

    // sprite sheets ids
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


    public static int OreSprite;
    public static int Ore2Sprite;
    public static int Ore3Sprite;

    //agent sprite ids
    public static int SlimeMoveLeftBaseSpriteId;
    public static int CharacterSpriteId;


    // particle sprite ids used for icons
    // TODO(): create icons atlas
    public static int OreIcon;
    public static int GunIcon;
    public static int SlimeIcon;
    public static int PlacementToolIcon;
    public static int RemoveToolIcon;
    public static int MiningLaserToolIcon;
    public static int PipePlacementToolIcon;

    private static bool IsInitialized = false; 


    public static void Initialize()
    {
        if (!IsInitialized)
        {
            IsInitialized = true;
            // loading the sprite sheets
            MoonSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\tiles_moon\\Tiles_Moon.png", 16, 16);
            OreSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\gem_hexagon_1.png", 16, 16);
            Ore2SpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\ore_copper_1.png", 16, 16);
            Ore3SpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\ores\\ore_adamantine_1.png", 16, 16);
            GunSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\gun-temp.png", 44, 25);
            RockSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1.png", 16, 16);
            RockDustSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\rock1_dust.png", 16, 16);
            SlimeSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\slime.png", 32, 32);
            CharacterSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png", 32, 48);
            LaserSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\item\\lasergun-temp.png", 195, 79);
            PipeSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\pipesim\\pipe.png", 16, 16);
            pipeIconSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\sprite\\item\\admin_icon_pipesim.png", 16, 16);


            OreSprite = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(OreSpriteSheet, 0, 0, 0);
            Ore2Sprite = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(Ore2SpriteSheet, 0, 0, 0);
            Ore3Sprite = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(Ore3SpriteSheet, 0, 0, 0);

            // agent sprite atlas
            SlimeMoveLeftBaseSpriteId = GameState.SpriteAtlasManager.CopySpritesToAtlas(SlimeSpriteSheet, 0, 0, 3, 0, Enums.AtlasType.Agent);
            CharacterSpriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(CharacterSpriteSheet, 0, 0, Enums.AtlasType.Agent);

            // particle sprite atlas
            OreIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(OreSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            GunIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(GunSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            SlimeIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(SlimeSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            PlacementToolIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(RockSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            RemoveToolIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(Ore2SpriteSheet, 0, 0, Enums.AtlasType.Particle);
            MiningLaserToolIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(LaserSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            PipePlacementToolIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(pipeIconSpriteSheet, 0, 0, Enums.AtlasType.Particle);


            CreateTiles();
            CreateAnimations();
            CreateItems();
        }
    }


    private static void CreateTiles()
    {
        GameState.TileCreationApi.CreateTile((int)Tile.TileEnum.Ore1);
        GameState.TileCreationApi.SetTileName("ore_1");
        GameState.TileCreationApi.SetTileTexture16(OreSpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTile();

        GameState.TileCreationApi.CreateTile((int)Tile.TileEnum.Glass);
        GameState.TileCreationApi.SetTileName("glass");
        GameState.TileCreationApi.SetTileSpriteSheet16(MoonSpriteSheet, 11, 10);
        GameState.TileCreationApi.EndTile();

        GameState.TileCreationApi.CreateTile((int)Tile.TileEnum.Moon);
        GameState.TileCreationApi.SetTileName("moon");
        GameState.TileCreationApi.SetTileSpriteSheet16(MoonSpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTile();

        GameState.TileCreationApi.CreateTile((int)Tile.TileEnum.Ore2);
        GameState.TileCreationApi.SetTileName("ore_2");
        GameState.TileCreationApi.SetTileTexture16(Ore2SpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTile();

        GameState.TileCreationApi.CreateTile((int)Tile.TileEnum.Ore3);
        GameState.TileCreationApi.SetTileName("ore_3");
        GameState.TileCreationApi.SetTileTexture16(Ore3SpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTile();

        GameState.TileCreationApi.CreateTile((int)Tile.TileEnum.Pipe);
        GameState.TileCreationApi.SetTileName("pipe");
        GameState.TileCreationApi.SetTileSpriteSheet16(PipeSpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTile();
    }

    private static void CreateAnimations()
    {
        GameState.AnimationManager.CreateAnimation(0);
        GameState.AnimationManager.SetName("character-move-left");
        GameState.AnimationManager.SetTimePerFrame(0.15f);
        GameState.AnimationManager.SetBaseSpriteID(GameResources.CharacterSpriteId);
        GameState.AnimationManager.SetFrameCount(1);
        GameState.AnimationManager.EndAnimation();

        GameState.AnimationManager.CreateAnimation(1);
        GameState.AnimationManager.SetName("character-move-right");
        GameState.AnimationManager.SetTimePerFrame(0.15f);
        GameState.AnimationManager.SetBaseSpriteID(GameResources.CharacterSpriteId);
        GameState.AnimationManager.SetFrameCount(1);
        GameState.AnimationManager.EndAnimation();

        GameState.AnimationManager.CreateAnimation(2);
        GameState.AnimationManager.SetName("slime-move-left");
        GameState.AnimationManager.SetTimePerFrame(0.35f);
        GameState.AnimationManager.SetBaseSpriteID(GameResources.SlimeMoveLeftBaseSpriteId);
        GameState.AnimationManager.SetFrameCount(4);
        GameState.AnimationManager.EndAnimation();
    }

    private static void CreateItems()
    {
        
        Item.CreationApi.Instance.CreateItem(Enums.ItemType.Gun, "Gun");
        Item.CreationApi.Instance.SetTexture(GunIcon);
        Item.CreationApi.Instance.SetInventoryTexture(GunIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.Ore, "Ore");
        Item.CreationApi.Instance.SetTexture(OreIcon);
        Item.CreationApi.Instance.SetInventoryTexture(OreIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetStackable(99);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.PlacementTool, "PlacementTool");
        Item.CreationApi.Instance.SetTexture(PlacementToolIcon);
        Item.CreationApi.Instance.SetInventoryTexture(PlacementToolIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetAction((int)Enums.ActionType.PlaceTilMoonAction);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.RemoveTileTool, "RemoveTileTool");
        Item.CreationApi.Instance.SetTexture(RemoveToolIcon);
        Item.CreationApi.Instance.SetInventoryTexture(RemoveToolIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetAction((int)Enums.ActionType.RemoveTileAction);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.SpawnEnemySlimeTool, "SpawnSlimeTool");
        Item.CreationApi.Instance.SetTexture(SlimeIcon);
        Item.CreationApi.Instance.SetInventoryTexture(SlimeIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetAction((int)Enums.ActionType.EnemySpawnAction);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.MiningLaserTool, "MiningLaserTool");
        Item.CreationApi.Instance.SetTexture(MiningLaserToolIcon);
        Item.CreationApi.Instance.SetInventoryTexture(MiningLaserToolIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetAction((int)Enums.ActionType.MiningLaserAction);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.PipePlacementTool, "PipePlacementTool");
        Item.CreationApi.Instance.SetTexture(PipePlacementToolIcon);
        Item.CreationApi.Instance.SetInventoryTexture(PipePlacementToolIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetAction((int)Enums.ActionType.PlaceTilPipeAction);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(Enums.ItemType.ParticleEmitterPlacementTool, "ParticleEmitterPlacementTool");
        Item.CreationApi.Instance.SetTexture(OreIcon);
        Item.CreationApi.Instance.SetInventoryTexture(OreIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetAction((int)Enums.ActionType.PlaceParticleEmitterAction);
        Item.CreationApi.Instance.EndItem();
    }


}