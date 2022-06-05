using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class SystemAsteroidBeltRenderer : MonoBehaviour
    {
        public CameraController cameraController;

        public SystemAsteroidBelt belt;

        public SpriteRenderer[] SpriteRenderers = new SpriteRenderer[0];
        public GameObject[] GameObjects = new GameObject[0];
        public OrbitRenderer or;

        public Color orbitColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        public Color asteroidColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        void Start()
        {
            cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();

            or = gameObject.AddComponent<OrbitRenderer>();

            or.descriptor = belt.CentralDescriptor;
        }

        void LateUpdate()
        {
            if (cameraController.scale > 2.0f)
            {
                orbitColor.a = 0.5f * (3.0f - (cameraController.scale > 2.9f ? 2.9f : cameraController.scale));
                asteroidColor.a = (cameraController.scale > 3.0 ? 3.0f : cameraController.scale) - 3.0f;

                if (SpriteRenderers.Length != belt.Asteroids.Count)
                {
                    Array.Resize(ref SpriteRenderers, belt.Asteroids.Count);
                    Array.Resize(ref GameObjects, belt.Asteroids.Count);
                    for (int i = 0; i < SpriteRenderers.Length; i++)
                        if (SpriteRenderers[i] == null)
                        {
                            GameObjects[i] = new GameObject();
                            GameObjects[i].name = "Asteroid renderer " + (i + 1);
                            SpriteRenderers[i] = GameObjects[i].AddComponent<SpriteRenderer>();
                            SpriteRenderers[i].sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
                        }
                }

                for (int i = 0; i < belt.Asteroids.Count; i++)
                {
                    float[] pos = belt.Descriptor[belt.Asteroids[i].Layer].GetPositionAt(belt.Asteroids[i].RotationalPosition + belt.RotationalPos[belt.Asteroids[i].Layer]);

                    GameObjects[i].transform.position = new Vector3(pos[0], pos[1], -0.1f);
                    GameObjects[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
            }
            else
            {
                orbitColor.a = 0.5f;

                for (int i = 0; i < GameObjects.Length; i++)
                    Destroy(GameObjects[i]);

                Array.Resize(ref SpriteRenderers, 0);
                Array.Resize(ref GameObjects, 0);
            }

            or.color = orbitColor;
            or.LineWidth = belt.BeltWidth;
            or.UpdateRenderer(128);
        }
    }
}
