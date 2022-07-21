namespace PlanetTileMap
{
    public struct TileMaterial
    {
        public TileMaterialType MaterialType; // material type
        public string Name; // name of the material
        public byte Durability; //max health of tile

        public bool CannotBeRemoved; // bedrock cannot be removed

        public SpriteRuleType SpriteRuleType;

        public int StartTileIndex; // the first tile index 
    }
}