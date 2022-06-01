using Entitas;
using UnityEngine;

namespace Agent
{
    [Agent]
    public struct Sprite2DComponent : IComponent
    {
        public int AtlasIndex;
        
        public Transform Parent;
        public Material Material;
        public Vector2Int Size;
    }
}
