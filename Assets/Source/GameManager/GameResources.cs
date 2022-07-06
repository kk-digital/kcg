using Enums.Tile;
using KMath;
using UnityEngine;

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
    public static int DustSpriteSheet;
    public static int GrenadeSpriteSheet;


    public static int OreSprite;
    public static int Ore2Sprite;
    public static int Ore3Sprite;

    //agent sprite ids
    public static int SlimeMoveLeftBaseSpriteId;
    public static int CharacterSpriteId;

    public static int GrenadeSpriteId;


    // particle sprite ids used for icons
    // TODO(): create icons atlas
    public static int DustBaseSpriteId;

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
            MoonSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Tiles\\Terrains\\Tiles_Moon.png", 16, 16);
            OreSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\Ores\\Gems\\Hexagon\\gem_hexagon_1.png", 16, 16);
            Ore2SpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\Ores\\Copper\\ore_copper_1.png", 16, 16);
            Ore3SpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\Ores\\Adamantine\\ore_adamantine_1.png", 16, 16);
            GunSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\Pistol\\gun-temp.png", 44, 25);
            RockSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\MaterialIcons\\Rock\\rock1.png", 16, 16);
            RockDustSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\Rock\\rock1_dust.png", 16, 16);
            SlimeSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Enemies\\Slime\\slime.png", 32, 32);
            CharacterSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Characters\\Player\\character.png", 32, 48);
            LaserSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\RailGun\\lasergun-temp.png", 195, 79);
            PipeSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Furnitures\\Pipesim\\pipesim.png", 16, 16);
            pipeIconSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Items\\AdminIcon\\Pipesim\\admin_icon_pipesim.png", 16, 16);
            DustSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Particles\\Dust\\dust1.png", 16, 16);
            GrenadeSpriteSheet = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Projectiles\\Grenades\\Grenade\\Grenades1.png", 16, 16);


            OreSprite = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(OreSpriteSheet, 0, 0, 0);
            Ore2Sprite = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(Ore2SpriteSheet, 0, 0, 0);
            Ore3Sprite = GameState.TileSpriteAtlasManager.CopyTileSpriteToAtlas16To32(Ore3SpriteSheet, 0, 0, 0);

            // agent sprite atlas
            SlimeMoveLeftBaseSpriteId = GameState.SpriteAtlasManager.CopySpritesToAtlas(SlimeSpriteSheet, 0, 0, 3, 0, Enums.AtlasType.Agent);
            CharacterSpriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(CharacterSpriteSheet, 0, 0, Enums.AtlasType.Agent);

            GrenadeSpriteId = GameState.SpriteAtlasManager.CopySpriteToAtlas(GrenadeSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            // particle sprite atlas
            OreIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(OreSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            GunIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(GunSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            SlimeIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(SlimeSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            PlacementToolIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(RockSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            RemoveToolIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(Ore2SpriteSheet, 0, 0, Enums.AtlasType.Particle);
            MiningLaserToolIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(LaserSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            PipePlacementToolIcon = GameState.SpriteAtlasManager.CopySpriteToAtlas(pipeIconSpriteSheet, 0, 0, Enums.AtlasType.Particle);
            DustBaseSpriteId = GameState.SpriteAtlasManager.CopySpritesToAtlas(DustSpriteSheet, 0, 0, 5, 0, Enums.AtlasType.Particle);


            CreateTiles();
            CreateAnimations();
            CreateAgents();
            CreateParticles();
            CreateParticleEmitters();
            CreateProjectiles();
        }
    }


    private static void CreateTiles()
    {
        GameState.TileCreationApi.CreateTileProperty(TileID.Ore1);
        GameState.TileCreationApi.SetTilePropertyName("ore_1");
        GameState.TileCreationApi.SetTilePropertyTexture16(OreSpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTileProperty();

        GameState.TileCreationApi.CreateTileProperty(TileID.Glass);
        GameState.TileCreationApi.SetTilePropertyName("glass");
        GameState.TileCreationApi.SetTilePropertySpriteSheet16(MoonSpriteSheet, 11, 10);
        GameState.TileCreationApi.EndTileProperty();

        GameState.TileCreationApi.CreateTileProperty(TileID.Moon);
        GameState.TileCreationApi.SetTilePropertyName("moon");
        GameState.TileCreationApi.SetTilePropertySpriteSheet16(MoonSpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTileProperty();

        GameState.TileCreationApi.CreateTileProperty(TileID.Ore2);
        GameState.TileCreationApi.SetTilePropertyName("ore_2");
        GameState.TileCreationApi.SetTilePropertyTexture16(Ore2SpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTileProperty();

        GameState.TileCreationApi.CreateTileProperty(TileID.Ore3);
        GameState.TileCreationApi.SetTilePropertyName("ore_3");
        GameState.TileCreationApi.SetTilePropertyTexture16(Ore3SpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTileProperty();

        GameState.TileCreationApi.CreateTileProperty(TileID.Pipe);
        GameState.TileCreationApi.SetTilePropertyName("pipe");
        GameState.TileCreationApi.SetTilePropertySpriteSheet16(PipeSpriteSheet, 0, 0);
        GameState.TileCreationApi.EndTileProperty();
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

    public static void CreateItems(Contexts entitasContext)
    {
        
        Item.CreationApi.Instance.CreateItem(entitasContext, Enums.ItemType.Gun, "Gun");
        Item.CreationApi.Instance.SetTexture(GunIcon);
        Item.CreationApi.Instance.SetInventoryTexture(GunIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetAction(Enums.ActionType.FireWeaponAction);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(entitasContext, Enums.ItemType.Ore, "Ore");
        Item.CreationApi.Instance.SetTexture(OreIcon);
        Item.CreationApi.Instance.SetInventoryTexture(OreIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetStackable(99);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(entitasContext, Enums.ItemType.PlacementTool, "PlacementTool");
        Item.CreationApi.Instance.SetTexture(PlacementToolIcon);
        Item.CreationApi.Instance.SetInventoryTexture(PlacementToolIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetAction(Enums.ActionType.PlaceTilMoonAction);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(entitasContext, Enums.ItemType.RemoveTileTool, "RemoveTileTool");
        Item.CreationApi.Instance.SetTexture(RemoveToolIcon);
        Item.CreationApi.Instance.SetInventoryTexture(RemoveToolIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetAction(Enums.ActionType.RemoveTileAction);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(entitasContext, Enums.ItemType.SpawnEnemySlimeTool, "SpawnSlimeTool");
        Item.CreationApi.Instance.SetTexture(SlimeIcon);
        Item.CreationApi.Instance.SetInventoryTexture(SlimeIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetAction(Enums.ActionType.EnemySpawnAction);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(entitasContext, Enums.ItemType.MiningLaserTool, "MiningLaserTool");
        Item.CreationApi.Instance.SetTexture(MiningLaserToolIcon);
        Item.CreationApi.Instance.SetInventoryTexture(MiningLaserToolIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetAction(Enums.ActionType.MiningLaserAction);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(entitasContext, Enums.ItemType.PipePlacementTool, "PipePlacementTool");
        Item.CreationApi.Instance.SetTexture(PipePlacementToolIcon);
        Item.CreationApi.Instance.SetInventoryTexture(PipePlacementToolIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetAction(Enums.ActionType.PlaceTilPipeAction);
        Item.CreationApi.Instance.EndItem();

        Item.CreationApi.Instance.CreateItem(entitasContext, Enums.ItemType.ParticleEmitterPlacementTool, "ParticleEmitterPlacementTool");
        Item.CreationApi.Instance.SetTexture(OreIcon);
        Item.CreationApi.Instance.SetInventoryTexture(OreIcon);
        Item.CreationApi.Instance.SetSize(new Vec2f(0.5f, 0.5f));
        Item.CreationApi.Instance.SetAction(Enums.ActionType.PlaceParticleEmitterAction);
        Item.CreationApi.Instance.EndItem();
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
    }



}
