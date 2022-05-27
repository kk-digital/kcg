using System;
using UnityEngine;
using System.Collections.Generic;

namespace SystemView
{
    public class SystemViewTest : MonoBehaviour
    {
        public SystemState State;

        private void Start()
        {
            GameLoop gl = GetComponent<GameLoop>();

            State = gl.CurrentSystemState;

            System.Random rnd = new System.Random();

            SystemPlanetRenderer[] testRenderers = new SystemPlanetRenderer[6];

            State.Star = new SystemStar();
            State.Star.PosX = (float)rnd.NextDouble() * 8.0f - 4.0f;
            State.Star.PosY = (float)rnd.NextDouble() * 8.0f - 4.0f;

            var StarObject = new GameObject();
            StarObject.name = "Star Renderer";

            SystemStarRenderer starRenderer = StarObject.AddComponent<SystemStarRenderer>();
            starRenderer.Star = State.Star;

            for (int i = 1; i <= 3; i++)
            {
                SystemPlanet testPlanet = new SystemPlanet();

                testPlanet.Descriptor.CenterX = State.Star.PosX;
                testPlanet.Descriptor.CenterY = State.Star.PosY;

                testPlanet.Descriptor.SemiMinorAxis = 1.0f + 3.0f * (float)rnd.NextDouble() * i;
                testPlanet.Descriptor.SemiMajorAxis = testPlanet.Descriptor.SemiMinorAxis + (float)rnd.NextDouble() / 8.0f;

                testPlanet.Descriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                var child = new GameObject();
                child.name = "Planet Renderer " + i;

                testRenderers[i - 1] = child.AddComponent<SystemPlanetRenderer>();
                testRenderers[i - 1].planet = testPlanet;

                State.Planets.Add(testPlanet);
            }

            SystemAsteroidBelt testBelt = new SystemAsteroidBelt();

            testBelt.Descriptor.CenterX = State.Star.PosX;
            testBelt.Descriptor.CenterY = State.Star.PosY;

            testBelt.Descriptor.SemiMinorAxis = State.Planets[2].Descriptor.SemiMajorAxis + 4.0f + (float)rnd.NextDouble();
            testBelt.Descriptor.SemiMajorAxis = testBelt.Descriptor.SemiMinorAxis + (float)rnd.NextDouble() / 4.0f;

            testBelt.BeltWidth = (float)rnd.NextDouble() * 4.0f;

            for (int i = 0; i < 4096; i++)
            {
                SystemAsteroid testAsteroid = new SystemAsteroid();
                
                testAsteroid.RotationalPosition = (float)i * 2.0f * 3.1415926f / 4096.0f;

                testBelt.Asteroids.Add(testAsteroid);
            }

            State.AsteroidBelts.Add(testBelt);

            var AsteroidBeltObject = new GameObject();
            AsteroidBeltObject.name = "Asteroid Belt Renderer";

            SystemAsteroidBeltRenderer asteroidBeltRenderer = AsteroidBeltObject.AddComponent<SystemAsteroidBeltRenderer>();
            asteroidBeltRenderer.belt = testBelt;

            for (int i = 0; i < 3; i++)
            {
                SystemPlanet testPlanet = new SystemPlanet();

                testPlanet.Descriptor.CenterX = State.Star.PosX;
                testPlanet.Descriptor.CenterY = State.Star.PosY;

                testPlanet.Descriptor.SemiMinorAxis = testBelt.Descriptor.SemiMajorAxis + testBelt.BeltWidth + 8.0f * (float)rnd.NextDouble() * (i + 1);
                testPlanet.Descriptor.SemiMajorAxis = testPlanet.Descriptor.SemiMinorAxis + (float)rnd.NextDouble() * (i + 1);

                testPlanet.Descriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                var child = new GameObject();
                child.name = "Planet Renderer " + (i + 4);

                testRenderers[i + 3] = child.AddComponent<SystemPlanetRenderer>();
                testRenderers[i + 3].planet = testPlanet;

                State.Planets.Add(testPlanet);
            }
        }

        void Update()
        {
            foreach(SystemPlanet p in State.Planets)
            {
                p.UpdatePosition(0.05f);
            }
            
            foreach(SystemAsteroidBelt b in State.AsteroidBelts)
            {
                b.UpdatePositions(0.05f);
            }
        }
    }
}
