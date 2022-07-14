namespace Enums
{
    public enum ItemType
    {
        Error = -1,     // Not inialized type.
        Rock,
        RockDust,
        Ore,

        /// <summary>
        /// Weapons
        /// </summary>
        Pistol,
        PumpShotgun,
        Shotgun,
        LongRifle,
        AutoCannon,
        RPG,
        SMG,
        Sword,

        /// <summary>
        /// Throwable Explosive Weapons
        /// </summary>
        Grenade,

        /// <summary>
        /// Tools
        /// </summary>
        PlacementTool,
        RemoveTileTool,
        MiningLaserTool,
        SpawnEnemySlimeTool,
        PipePlacementTool,
        ParticleEmitterPlacementTool
    }
}
