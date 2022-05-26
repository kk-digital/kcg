using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Planet
{
    public float CenterX;
    public float CenterY;

    public float SemiMinorAxis;
    public float SemiMajorAxis;

    public float Rotation;

    public float MeanAnomaly; // https://en.wikipedia.org/wiki/Mean_anomaly - This is basically the planet's position as a degree of its total orbit relative to its periapsis
}
