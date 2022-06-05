using System;
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
        public float FOV;
        public float Range;

        public int Damage;
        public int FiringRate;
        public int LastMillis;
        public int Cooldown;

        public System.Random rand;

        public SpriteRenderer sr;
        public CameraController Camera;

        private LineRenderer DebugLineRenderer1;
        private LineRenderer DebugLineRenderer2;
        private Color DebugConeColor = new Color(0.8f, 0.6f, 0.1f, 0.4f);

        public SystemState State;

        public SystemShip Target;

        // Start is called before the first frame update
        void Start()
        {
            rand = new System.Random();

            PosX = (float)rand.NextDouble() * 50.0f - 25.0f;
            PosY = (float)rand.NextDouble() * 50.0f - 25.0f;

            AngularVelocity = 1.0f;
            Rotation = 0.0f;
            LastRotation = 0.0f;
            FOV = 3.1415926f / 4.0f;
            Range = 15.0f;

            sr = gameObject.AddComponent<SpriteRenderer>();
            sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");

            Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

            DebugLineRenderer1 = (new GameObject()).AddComponent<LineRenderer>();
            DebugLineRenderer2 = (new GameObject()).AddComponent<LineRenderer>();

            Shader shader = Shader.Find("Hidden/Internal-Colored");
            Material mat = new Material(shader);
            mat.hideFlags = HideFlags.HideAndDontSave;

            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            // Turn off backface culling, depth writes, depth test.
            mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            mat.SetInt("_ZWrite", 0);
            mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

            DebugLineRenderer1.material = DebugLineRenderer2.material = mat;

            DebugLineRenderer1.useWorldSpace = DebugLineRenderer2.useWorldSpace = true;

            LastMillis = (int)(Time.time * 1000.0f);
        }

        // Update is called once per frame
        void Update()
        {
            sr.transform.position = new Vector3(PosX, PosY, -0.1f);
            sr.transform.localScale = new Vector3(5.0f / Camera.scale, 5.0f / Camera.scale, 1.0f);

            sr.transform.Rotate(new Vector3(0.0f, 0.0f, (Rotation - LastRotation) * 180.0f / 3.1415926f));
            LastRotation = Rotation;

            Vector3[] vertices1 = new Vector3[2];
            Vector3[] vertices2 = new Vector3[2];

            vertices1[0] = new Vector3(PosX, PosY, 0.0f);
            vertices1[1] = new Vector3(PosX + (float)Math.Cos(Rotation - FOV / 2.0f) * Range, PosY + (float)Math.Sin(Rotation - FOV / 2.0f) * Range, 0.0f);

            vertices2[0] = new Vector3(PosX, PosY, 0.0f);
            vertices2[1] = new Vector3(PosX + (float)Math.Cos(Rotation + FOV / 2.0f) * Range, PosY + (float)Math.Sin(Rotation + FOV / 2.0f) * Range, 0.0f);

            DebugLineRenderer1.startWidth = DebugLineRenderer1.endWidth = DebugLineRenderer2.startWidth = DebugLineRenderer2.endWidth = 0.1f / Camera.scale;
            DebugLineRenderer1.startColor = DebugLineRenderer1.endColor = DebugLineRenderer2.startColor = DebugLineRenderer2.endColor = DebugConeColor;
            DebugLineRenderer1.SetPositions(vertices1);
            DebugLineRenderer2.SetPositions(vertices2);
            DebugLineRenderer1.positionCount = DebugLineRenderer2.positionCount = 2;

            int CurrentMillis = (int)(Time.time * 1000.0f) - LastMillis;
            LastMillis = (int)(Time.time * 1000.0f);

            // Pick target
            if (Target != null)
            {
                float DistanceX = Target.PosX - PosX;
                float DistanceY = Target.PosY - PosY;
                float Distance  = (float)Math.Sqrt(DistanceX * DistanceX + DistanceY * DistanceY);

                if (Distance > Range) Target = null;
            }

            if (Target == null)
            {
                if (State != null) foreach (SystemShip Ship in State.Ships)
                {

                    float DistanceX = Ship.PosX - PosX;
                    float DistanceY = Ship.PosY - PosY;
                    float Distance = (float)Math.Sqrt(DistanceX * DistanceX + DistanceY * DistanceY);

                    if (Distance <= Range) { Target = Ship; break; }
                }
            }

            if (Target != null)
            {
                if(TryTargeting(Target, CurrentMillis))
                {

                }
            }
        }

        public bool TryTargeting(SystemShip ship, int CurrentMillis)
        {
            float DistanceX = ship.PosX - PosX;
            float DistanceY = ship.PosY - PosY;
            float Distance = (float)Math.Sqrt(DistanceX * DistanceX + DistanceY * DistanceY);

            if (Rotation < 0.0f) Rotation = 2.0f * 3.1415926f + Rotation;
            while (Rotation > 2.0f * 3.1415926f) Rotation -= 2.0f * 3.1415926f;

            if (Cooldown > 0.0f || Distance > Range) return false;

            float Angle = (float)Math.Acos(DistanceX / Distance);
            if (ship.PosY < PosY) Angle = 2.0f * 3.1415926f - Angle;

            if (Rotation == Angle) return true;

            float diff1 = Angle - Rotation;
            float diff2 = Rotation - Angle;

            if (diff1 < 0.0f) diff1 = 2.0f * 3.1415926f + diff1;
            if (diff2 < 0.0f) diff2 = 2.0f * 3.1415926f + diff2;

            if (diff2 < diff1)
            {
                Rotation -= AngularVelocity / 1000.0f * CurrentMillis;

                diff1 = Angle - Rotation;
                diff2 = Rotation - Angle;

                if (diff1 < 0.0f) diff1 = 2.0f * 3.1415926f + diff1;
                if (diff2 < 0.0f) diff2 = 2.0f * 3.1415926f + diff2;

                if (diff1 < diff2) Rotation = Angle;
            }
            else
            {
                Rotation += AngularVelocity / 1000.0f * CurrentMillis;

                diff1 = Angle - Rotation;
                diff2 = Rotation - Angle;

                if (diff1 < 0.0f) diff1 = 2.0f * 3.1415926f + diff1;
                if (diff2 < 0.0f) diff2 = 2.0f * 3.1415926f + diff2;

                if (diff2 < diff1) Rotation = Angle;
            }

            return true;
        }

        public bool TryShooting(SystemShip ship)
        {
            if (Cooldown > 0.0f) return false;

            float DistanceX = ship.PosX - PosX;
            float DistanceY = ship.PosY - PosY;
            float Distance = (float)Math.Sqrt(DistanceX * DistanceX + DistanceY * DistanceY);

            float Angle = (float)Math.Acos(DistanceX / Distance);

            if (Angle > Rotation + FOV / 2.0f || Angle < Rotation - FOV / 2.0f) return false;

            

            return true;
        }
    }
}
