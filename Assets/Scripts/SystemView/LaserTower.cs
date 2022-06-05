using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class LaserTower : MonoBehaviour
    {
        public float PosX;
        public float PosY;

        public float AngularVelocity;
        public float Rotation;
        public float LastRotation;

        public int Damage;
        public int FiringRate;
        public int LastMillis;

        public System.Random rand;

        public SpriteRenderer sr;
        public CameraController Camera;

        // Start is called before the first frame update
        void Start()
        {
            rand = new System.Random();

            PosX = (float)rand.NextDouble() * 30.0f - 15.0f;
            PosY = (float)rand.NextDouble() * 30.0f - 15.0f;

            AngularVelocity = 0.1f;
            Rotation = 0.0f;
            LastRotation = 0.0f;

            sr = gameObject.AddComponent<SpriteRenderer>();
            sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");

            Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

            LastMillis = (int)(Time.time * 1000.0f);
        }

        // Update is called once per frame
        void Update()
        {
            sr.transform.position = new Vector3(PosX, PosY, -0.1f);
            sr.transform.localScale = new Vector3(5.0f / Camera.scale, 5.0f / Camera.scale, 1.0f);

            sr.transform.Rotate(new Vector3(0.0f, 0.0f, (Rotation - LastRotation) * 180.0f / 3.1415926f));
        }
    }
}
