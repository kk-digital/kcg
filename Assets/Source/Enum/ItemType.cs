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
        LongRifle,
        PulseWeapon,
        SMG,
        Shotgun,
        PumpShotgun,
        Pistol,
        AutoCannon,
        GrenadeLauncher,
        RPG,
        Bow,
        Sword,
        StunBaton,

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
