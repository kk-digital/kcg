using Entitas;
using UnityEngine;

namespace Item
{
    public class MoveComponent : IComponent // Make item Movable.
    {
        public float PreviuosPosX;
        public float PreviuosPosY;
    }
}
