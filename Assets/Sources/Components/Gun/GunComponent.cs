using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class GunComponent : IComponent
{
    public float minimumShotInterval;
    public float timeSinceLastShot;
}
