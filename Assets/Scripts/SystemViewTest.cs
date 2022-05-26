using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemViewTest : MonoBehaviour
{
    private Planet[] testPlanets;
    private PlanetRenderer[] testRenderers;

    void Start()
    {
        System.Random rnd = new System.Random();

        testPlanets = new Planet[6];
        testRenderers = new PlanetRenderer[6];

        for(int i = 0; i < 6; i++)
        {
            testPlanets[i] = new Planet();

            testPlanets[i].CenterX = 0;
            testPlanets[i].CenterY = 0;

            testPlanets[i].SemiMinorAxis = 2.0f + i + 2.0f * (float)rnd.NextDouble();
            testPlanets[i].SemiMajorAxis = testPlanets[i].SemiMinorAxis + 4.0f * (float)rnd.NextDouble();

            testPlanets[i].Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

            var child = new GameObject();
            child.name = "Planet Renderer " + (i + 1);

            testRenderers[i] = child.AddComponent<PlanetRenderer>();
            testRenderers[i].planet = testPlanets[i];
            testRenderers[i].color = new Color((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), 1.0f);
        }        
    }

    void Update()
    {

    }
}
