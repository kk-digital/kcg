using System;
using UnityEngine;

namespace InteliMap
{
    [Serializable]
    public struct DirectionalBools
    {
        public DirectionalBools(bool top, bool bottom, bool left, bool right)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }

        [Tooltip("The same direction as positive y.")]
        [SerializeField] public bool top;
        [Tooltip("The same direction as negative y.")]
        [SerializeField] public bool bottom;
        [Tooltip("The same direction as negative x.")]
        [SerializeField] public bool left;
        [Tooltip("The same direction as positive x.")]
        [SerializeField] public bool right;

        public override string ToString()
        {
            return (top ? "[T " : "[_ ") + (bottom ? "B " : "_ ") + (left ? "L " : "_ ") + (right ? "R]" : "_]");
        }
    }
}