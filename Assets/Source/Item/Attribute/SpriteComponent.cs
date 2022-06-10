using Entitas;
using UnityEngine;

namespace Item.Attribute
{
    public struct SpriteComponent : IComponent
    {
        public Texture2D Texture;

        /// <summary>
        /// <p>x = PngSize.x / 32f</p>
        /// <p>y = PngSize.y / 32f</p>
        /// </summary>
        public Vector2 Size;

        /// <summary>
        /// Size defined by InventorySystem.
        /// </summary>
        public Texture2D TextureInventory;
    }
}
