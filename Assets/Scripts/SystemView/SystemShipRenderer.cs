using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class SystemShipRenderer : MonoBehaviour
    {
        public SystemShip ship;

        public SpriteRenderer ShipRender;
        public SpriteRenderer ShieldRender;
        public OrbitRenderer OrbitRender;
        public LineRenderer DirectionRenderer;
        public LineRenderer VelocityRenderer;

        public Color orbitColor     = new Color(1.0f, 0.7f, 0.5f, 1.0f);
        public Color shieldColor    = new Color(0.4f, 0.7f, 1.0f, 0.5f);
        public Color directionColor = new Color(1.0f, 0.7f, 0.5f, 0.4f);
        public Color velocityColor  = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        public Color shipColor      = Color.white;

        public float width = 1.0f;

        public GameObject ShieldObject;
        public GameObject DirectionObject;
        public GameObject VelocityObject;

        public CameraController Camera;

        public float LastRotation;

        // Start is called before the first frame update
        void Start()
        {
            OrbitRender = gameObject.AddComponent<OrbitRenderer>();
            ShipRender = gameObject.AddComponent<SpriteRenderer>();

            ShieldObject = new GameObject();
            ShieldObject.name = "Shield Renderer";

            ShieldRender = ShieldObject.AddComponent<SpriteRenderer>();

            OrbitRender.descriptor = ship.Descriptor;

            Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

            // Temporary sprites
            ShipRender.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            ShieldRender.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

            Shader shader = Shader.Find("Hidden/Internal-Colored");
            Material mat = new Material(shader);
            mat.hideFlags = HideFlags.HideAndDontSave;

            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            // Turn off backface culling, depth writes, depth test.
            mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            mat.SetInt("_ZWrite", 0);
            mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);

            DirectionObject = new GameObject();
            DirectionObject.name = "Ship direction renderer";

            VelocityObject = new GameObject();
            VelocityObject.name = "Ship velocity renderer";

            DirectionRenderer = DirectionObject.AddComponent<LineRenderer>();
            DirectionRenderer.material = mat;
            DirectionRenderer.useWorldSpace = true;

            VelocityRenderer = VelocityObject.AddComponent<LineRenderer>();
            VelocityRenderer.material = mat;
            VelocityRenderer.useWorldSpace = true;
        }

        // Update is called once per frame
        void Update()
        {
            ShipRender.transform.position     = new Vector3(ship.self.posx, ship.self.posy, -0.1f);
            ShipRender.transform.localScale   = new Vector3(5.0f * width / Camera.scale, 5.0f / Camera.scale, 1.0f);

            ShipRender.transform.Rotate(new Vector3(0.0f, 0.0f, (ship.Rotation - LastRotation) * 180.0f / 3.1415926f));

            ShieldRender.transform.position   = new Vector3(ship.self.posx, ship.self.posy, -0.05f);
            ShieldRender.transform.localScale = new Vector3(20.0f / Camera.scale, 15.0f / Camera.scale, 1.0f);

            ShieldRender.transform.Rotate(new Vector3(0.0f, 0.0f, (ship.Rotation - LastRotation) * 180.0f / 3.1415926f));

            LastRotation       = ship.Rotation;
            ShipRender.color   = shipColor;
            OrbitRender.color  = orbitColor;

            if (ship.MaxShield == 0)
                ShieldRender.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            else
                ShieldRender.color = new Color(shieldColor.r, shieldColor.g, shieldColor.b, shieldColor.a * ship.Shield / ship.MaxShield);

            float v = (float)Math.Sqrt(ship.self.velx * ship.self.velx + ship.self.vely * ship.self.vely);
            if (ship.Weapons.Count > 0 && v > 0.0f)
            {
                Vector3[] vertices = new Vector3[2];
                vertices[0] = new Vector3(ship.self.posx, ship.self.posy, -0.075f);
                vertices[1] = new Vector3(ship.self.posx + (float)Math.Cos(ship.Rotation) * 10.0f / Camera.scale, ship.self.posy + (float)Math.Sin(ship.Rotation) * 10.0f / Camera.scale, -0.075f);
                DirectionRenderer.SetPositions(vertices);
                DirectionRenderer.positionCount = 2;
                DirectionRenderer.startColor = DirectionRenderer.endColor = directionColor;
                DirectionRenderer.startWidth = DirectionRenderer.endWidth = 0.2f / Camera.scale;

                Vector3[] vertices2 = new Vector3[2];
                vertices2[0] = new Vector3(ship.self.posx, ship.self.posy, -0.075f);
                vertices2[1] = new Vector3(ship.self.posx + ship.self.velx / v * 10.0f / Camera.scale, ship.self.posy + ship.self.vely / v * 10.0f / Camera.scale, -0.075f);
                VelocityRenderer.SetPositions(vertices2);
                VelocityRenderer.positionCount = 2;
                VelocityRenderer.startColor = VelocityRenderer.endColor = velocityColor;
                VelocityRenderer.startWidth = VelocityRenderer.endWidth = 0.2f / Camera.scale;
            }
            else
            {
                DirectionRenderer.positionCount = VelocityRenderer.positionCount = 0;
            }

            if (!ship.PathPlanned) OrbitRender.descriptor = null;
            else OrbitRender.descriptor = ship.Descriptor;

            OrbitRender.UpdateRenderer(128);
        }

        void OnDestroy()
        {
            GameObject.Destroy(ShipRender);
            GameObject.Destroy(ShieldRender);
            GameObject.Destroy(OrbitRender);
            GameObject.Destroy(DirectionRenderer);
            GameObject.Destroy(VelocityRenderer);
            GameObject.Destroy(ShieldObject);
            GameObject.Destroy(DirectionObject);
            GameObject.Destroy(VelocityObject);
        }
    }
}
