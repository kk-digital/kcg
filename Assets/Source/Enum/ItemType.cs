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
        Gun,
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
