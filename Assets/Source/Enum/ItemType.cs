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
        PulseWeapon,
        PumpShotgun,
        Shotgun,
        LongRifle,
        AutoCannon,
        RPG,
        GrenadeLauncher,
        SMG,
        Sword,
        Bow,

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
