using Entitas;
using KMath;
using UnityEngine;

namespace Agent
{
    [Agent]
    public class Sprite2DComponent : IComponent
    {
        public int SpriteId;
        public Vec2f Size;
    }
}
