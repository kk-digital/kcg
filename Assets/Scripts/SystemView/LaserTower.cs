using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class LaserTower : MonoBehaviour
    {
        public float posx;
        public float posy;

        public float AngularVelocity;
        public float Rotation;
        public float LastRotation;
        public float FOV;
        public float Range;

        public int Damage;
        public int FiringRate;
        public int LastMillis;
        public int Cooldown;

        public float ShieldPenetration;

        public float ShieldDamageMultiplier;
        public float HullDamageMultiplier;

        public System.Random rand;

        public SpriteRenderer sr;
        public CameraController Camera;

        private GameObject DebugLineObject1;
        private GameObject DebugLineObject2;
        private GameObject LaserLineObject;

        private LineRenderer DebugLineRenderer1;
        private LineRenderer DebugLineRenderer2;

        private Color DebugConeColor = new Color(0.8f, 0.6f, 0.1f, 0.4f);

        public LineRenderer LaserLineRenderer;
        public Color LaserColor = new Color(0.2f, 0.8f, 0.3f, 0.7f);

        public float LaserChargingDuration;
        public float LaserDuration;
        public float LaserWidth;

        public SystemState State;

        public SystemShip Target;

        // todo apply gravity to laser tower

        // Start is called before the first frame update
        void Start()
        {
            rand = new System.Random();

            posx = (float)rand.NextDouble() * 50.0f - 25.0f;
            posy = (float)rand.NextDouble() * 50.0f - 25.0f;

            AngularVelocity = 0.4f;
            Rotation = 0.0f;
            LastRotation = 0.0f;
            FOV = 3.1415926f / 6.0f;
            Range = 20.0f;
            LaserWidth = 0.6f;
            LaserChargingDuration = 0.125f;
            LaserDuration = 0.3f;

            FiringRate = 3250;
            Damage = 3000;
            ShieldDamageMultiplier = 3.0f;
            HullDamageMultiplier = 0.2f;
            ShieldPenetration = 0.02f;

            sr = gameObject.AddComponent<SpriteRenderer>();
            sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");

            Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

            DebugLineObject1   = new GameObject();
            DebugLineObject2   = new GameObject();
            LaserLineObject    = new GameObject();

            DebugLineRenderer1 = DebugLineObject1.AddComponent<LineRenderer>();
            DebugLineRenderer2 = DebugLineObject2.AddComponent<LineRenderer>();
            LaserLineRenderer  = LaserLineObject.AddComponent<LineRenderer>();

            Shader shader = Shader.Find("Hidden/Internal-Colored");
            Material mat = new Material(shader);
            mat.hideFlags = HideFlags.HideAndDontSave;

            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            // Turn off backface culling, depth writes, depth test.
            mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            mat.SetInt("_ZWrite", 0);
            mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

            DebugLineRenderer1.material = DebugLineRenderer2.material = LaserLineRenderer.material = mat;

            DebugLineRenderer1.useWorldSpace = DebugLineRenderer2.useWorldSpace = LaserLineRenderer.useWorldSpace = true;

            LastMillis = (int)(Time.time * 1000.0f);

            LaserLineRenderer.startWidth = LaserLineRenderer.endWidth = 0.15f / Camera.scale;
            LaserLineRenderer.startColor = LaserLineRenderer.endColor = LaserColor;
        }

        // Update is called once per frame
        void Update()
        {
            sr.transform.position = new Vector3(posx, posy, -0.1f);
            sr.transform.localScale = new Vector3(5.0f / Camera.scale, 5.0f / Camera.scale, 1.0f);

            sr.transform.Rotate(new Vector3(0.0f, 0.0f, (Rotation - LastRotation) * 180.0f / 3.1415926f));
            LastRotation = Rotation;

            Vector3[] vertices1 = new Vector3[2];
            Vector3[] vertices2 = new Vector3[2];

            vertices1[0] = new Vector3(posx, posy, 0.0f);
            vertices1[1] = new Vector3(posx + (float)Math.Cos(Rotation - FOV / 2.0f) * Range, posy + (float)Math.Sin(Rotation - FOV / 2.0f) * Range, 0.0f);

            vertices2[0] = new Vector3(posx, posy, 0.0f);
            vertices2[1] = new Vector3(posx + (float)Math.Cos(Rotation + FOV / 2.0f) * Range, posy + (float)Math.Sin(Rotation + FOV / 2.0f) * Range, 0.0f);

            DebugLineRenderer1.startWidth = DebugLineRenderer1.endWidth = DebugLineRenderer2.startWidth = DebugLineRenderer2.endWidth = 0.1f / Camera.scale;
            DebugLineRenderer1.startColor = DebugLineRenderer1.endColor = DebugLineRenderer2.startColor = DebugLineRenderer2.endColor = DebugConeColor;
            DebugLineRenderer1.SetPositions(vertices1);
            DebugLineRenderer2.SetPositions(vertices2);
            DebugLineRenderer1.positionCount = DebugLineRenderer2.positionCount = 2;

            int CurrentMillis = (int)(Time.time * 1000.0f) - LastMillis;
            LastMillis = (int)(Time.time * 1000.0f);

            int ChargingTime = (int)(FiringRate * LaserDuration * LaserChargingDuration);
            int LaserDurationTime = (int)(FiringRate * LaserDuration * (1.0f - LaserChargingDuration));
            int RemainingChargingTime = Cooldown - (FiringRate - ChargingTime);
            int RemainingTime = Cooldown - (FiringRate - LaserDurationTime);
            if (RemainingTime > 0)
            {
                if (RemainingChargingTime > 0)
                {
                    float RemainingTimeAsPercentage = 1.0f - (float)RemainingChargingTime / (float)LaserDurationTime;
                    LaserLineRenderer.startWidth = LaserLineRenderer.endWidth = LaserWidth * RemainingTimeAsPercentage / Camera.scale;
                    LaserLineRenderer.startColor = new Color(LaserColor.r, LaserColor.g, LaserColor.b, LaserColor.a * RemainingTimeAsPercentage + 0.10f);
                    LaserLineRenderer.endColor   = new Color(LaserColor.r, LaserColor.g, LaserColor.b, LaserColor.a * RemainingTimeAsPercentage + 0.02f);
                }
                else
                { 
                    float RemainingTimeAsPercentage = (float)RemainingTime / (float)LaserDurationTime;
                    LaserLineRenderer.startWidth = LaserLineRenderer.endWidth = RemainingTimeAsPercentage * LaserWidth / Camera.scale;
                    LaserLineRenderer.startColor = new Color(LaserColor.r, LaserColor.g, LaserColor.b, LaserColor.a * RemainingTimeAsPercentage + 0.10f);
                    LaserLineRenderer.endColor   = new Color(LaserColor.r, LaserColor.g, LaserColor.b, LaserColor.a * RemainingTimeAsPercentage + 0.02f);
                }
            }

            Cooldown -= CurrentMillis;
            if (Cooldown < 0) Cooldown = 0;

            // Pick target
            if (Target != null)
            {
                float DistanceX = Target.self.posx - posx;
                float DistanceY = Target.self.posy - posy;
                float Distance  = (float)Math.Sqrt(DistanceX * DistanceX + DistanceY * DistanceY);

                if (Distance > Range) Target = null;
            }

            if (Target == null)
            {
                if (State != null) foreach (SystemShip Ship in State.ships)
                {

                    float DistanceX = Ship.self.posx - posx;
                    float DistanceY = Ship.self.posy - posy;
                    float Distance = (float)Math.Sqrt(DistanceX * DistanceX + DistanceY * DistanceY);

                    if (Distance <= Range) { Target = Ship; break; }
                }
            }

            if (Target != null)
            {
                if (TryTargeting(Target, CurrentMillis) && Cooldown == 0)
                {
                    TryShooting();
                }
            }
        }

        public bool TryTargeting(SystemShip ship, int CurrentMillis)
        {
            float DistanceX = ship.self.posx - posx;
            float DistanceY = ship.self.posy - posy;
            float Distance = (float)Math.Sqrt(DistanceX * DistanceX + DistanceY * DistanceY);

            while (Rotation < 0.0f) Rotation = 2.0f * 3.1415926f + Rotation;
            while (Rotation > 2.0f * 3.1415926f) Rotation -= 2.0f * 3.1415926f;

            if (Distance > Range) return false;

            float Angle = (float)Math.Acos(DistanceX / Distance);
            if (ship.self.posy < posy) Angle = 2.0f * 3.1415926f - Angle;

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

        public bool TryShooting()
        {
            if (Cooldown > 0) return false;

            float DistanceX = Target.self.posx - posx;
            float DistanceY = Target.self.posy - posy;
            float Distance = (float)Math.Sqrt(DistanceX * DistanceX + DistanceY * DistanceY);

            if (Rotation < 0.0f) Rotation = 2.0f * 3.1415926f + Rotation;
            while (Rotation > 2.0f * 3.1415926f) Rotation -= 2.0f * 3.1415926f;

            if (Distance > Range) return false;

            float Angle = (float)Math.Acos(DistanceX / Distance);
            if (Target.self.posy < posy) Angle = 2.0f * 3.1415926f - Angle;

            if (Angle > Rotation + FOV / 2.0f || Angle < Rotation - FOV / 2.0f) return false;

            Vector3[] vertices = new Vector3[2];
            vertices[0] = new Vector3(posx, posy, 0.0f);
            vertices[1] = new Vector3(Target.self.posx, Target.self.posy, 0.0f);
            LaserLineRenderer.SetPositions(vertices);
            LaserLineRenderer.positionCount = 2;

            float ShieldDamage = Damage * ShieldDamageMultiplier * 1.0f - ShieldPenetration;
            float HullDamage   = Damage * HullDamageMultiplier   *        ShieldPenetration;

            Target.shield -= (int)ShieldDamage;

            if (Target.shield < 0)
            {
                HullDamage -= (float)Target.shield / ShieldDamageMultiplier * HullDamageMultiplier;
                Target.shield = 0;
            }

            Target.health -= (int)HullDamage;

            if (Target.health <= 0)
            {
                Target.destroy();
                Target = null;
            }

            Cooldown = FiringRate;

            return true;
        }

        void OnDestroy()
        {
            GameObject.Destroy(DebugLineRenderer1);
            GameObject.Destroy(DebugLineRenderer2);
            GameObject.Destroy(LaserLineRenderer);
            GameObject.Destroy(DebugLineObject1);
            GameObject.Destroy(DebugLineObject2);
            GameObject.Destroy(LaserLineObject);
        }
    }
}
