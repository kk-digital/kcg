using Entitas;
using System.Collections.Generic;
using UnityEngine;

public sealed class ForceComponent : IComponent
{
    public List<Vector2> relativeForces;
    public float torque;
}
