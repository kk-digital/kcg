using Entitas;
using UnityEngine;
using KMath;

namespace FloatingText
{
    public class StateComponent : IComponent
    {
        public float TimeToLive;
        public string Text;
    }
}
