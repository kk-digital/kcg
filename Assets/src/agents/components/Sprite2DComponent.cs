using Entitas;
using UnityEngine;

namespace Agent
{
    [Agent]
    public struct Sprite2DComponent : IComponent
    {
        public int SpriteID;
        /// <summary>
        /// blit the player sprite sheet to the atlas
        /// </summary>
        public int BlitSpriteAtlasID;
        public Vector2Int Size;

        public void InitSprite()
        {
            SpriteID = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png", Size.x, Size.y);
            BlitSpriteAtlasID = GameState.SpriteAtlasManager.Blit(SpriteID, 0, 0);
        }
    }
}
