using Entitas;
using UnityEngine;
using KMath;

namespace FloatingText
{
    [FloatingText]
    public class StateComponent : IComponent
    {
        public float TimeToLive;
        public string Text;
    }
}
