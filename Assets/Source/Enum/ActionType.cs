namespace Enums
{
    // Todo: Procedural generate enums and Intialize functions.
    public enum ActionType
    {
        /// <summary>
        /// General Actions
        /// </summary>
        DropAction,
        PickUpAction,
        ReloadAction,

        /// <summary>
        /// PlaceTileTool
        /// One for each type of tile
        /// </summary>
        PlaceTilOre1Action,
        PlaceTilOre2Action,
        PlaceTilOre3Action,
        PlaceTilGlassAction,
        PlaceTilMoonAction,
        PlaceTilPipeAction,

        /// <summary>
        /// Others tools actions
        /// </summary>
        ToolActionFireWeapon = 9,
        ToolActionPlaceParticle,
        ToolActionEnemySpawn,
        ToolActionMiningLaser,
        ToolActionRemoveTile
    }
}
