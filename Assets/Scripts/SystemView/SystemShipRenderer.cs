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

        public Material mat;

        public Color orbitColor = new Color(1.0f, 0.7f, 0.5f, 1.0f);
        public Color shieldColor = new Color(0.4f, 0.7f, 1.0f, 0.5f);
        public Color shipColor = Color.white;

        public GameObject ShieldObject;

        public CameraController Camera;

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
        }

        // Update is called once per frame
        void Update()
        {
            ShipRender.transform.position     = new Vector3(ship.PosX, ship.PosY, -0.1f);
            ShipRender.transform.localScale   = new Vector3(5.0f / Camera.scale, 5.0f / Camera.scale, 1.0f);

            ShieldRender.transform.position   = new Vector3(ship.PosX, ship.PosY, -0.05f);
            ShieldRender.transform.localScale = new Vector3(15.0f / Camera.scale, 15.0f / Camera.scale, 1.0f);

            ShipRender.color   = shipColor;
            OrbitRender.color  = orbitColor;

            if (ship.MaxShield == 0)
                ShieldRender.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            else
                ShieldRender.color = new Color(shieldColor.r, shieldColor.g, shieldColor.b, shieldColor.a * ship.Shield / ship.MaxShield);

            if (!ship.PathPlanned) OrbitRender.descriptor = null;
            else OrbitRender.descriptor = ship.Descriptor;

            OrbitRender.UpdateRenderer(128);
        }
    }
}
