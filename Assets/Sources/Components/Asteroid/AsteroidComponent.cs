using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;


public enum AsteroidSize
{
    Large,
    Medium,
    Small,
    Tiny
}

public class AsteroidComponent : IComponent
{
    public AsteroidSize size;
}
