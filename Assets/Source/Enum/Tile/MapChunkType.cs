namespace Enums.Tile
{
    /// <param name="Error">Not initialized chunk</param>
    /// <param name="Empty">Chunk with only Air tiles</param>
    /// <param name="NotEmpty">At least one non air tile exist</param>
    public enum MapChunkType
    {
        /// <summary>
        /// Not initialized chunk
        /// </summary>
        Error = 0,
        /// <summary>
        /// Chunk with only Air tiles
        /// </summary>
        Empty,
        /// <summary>
        /// At least one non air tile exist
        /// </summary>
        NotEmpty
    }
}
