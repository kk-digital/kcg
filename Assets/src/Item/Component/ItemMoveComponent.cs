using Entitas;
using UnityEngine;

namespace Components
{
    public class ItemMoveComponent : IComponent // Make item Movable.
    {
        public float PreviuosPosX;
        public float PreviuosPosY;
    }
}
