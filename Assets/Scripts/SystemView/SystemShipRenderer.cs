using System;
using System.Collections.Generic;
using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public struct DebugWeaponInfo {
            public ShipWeapon   weapon;
            public GameObject   obj1;
            public GameObject   obj2;
            public LineRenderer line1;
            public LineRenderer line2;
            public Vector3[]    vertices1;
            public Vector3[]    vertices2;

            public void delete() {
                GameObject.Destroy(line1);
                GameObject.Destroy(line2);
                GameObject.Destroy(obj1);
                GameObject.Destroy(obj2);
            }
        }

        public class SystemShipRenderer : MonoBehaviour {
            public SystemShip ship;

            public SpriteRenderer ShipRender;
            public SpriteRenderer ShieldRender;
            public OrbitRenderer OrbitRender;
            public LineRenderer DirectionRenderer;
            public LineRenderer VelocityRenderer;
            public SystemState State;

            public Color orbitColor     = new Color(1.0f, 0.7f, 0.5f, 1.0f);
            public Color shieldColor    = new Color(0.4f, 0.7f, 1.0f, 0.5f);
            public Color directionColor = new Color(1.0f, 0.7f, 0.5f, 0.4f);
            public Color velocityColor  = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            public Color debugLineColor = new Color(1.0f, 1.0f, 1.0f, 0.3f);
            public Color shipColor      = Color.white;

            public float width = 1.0f;

            public GameObject ShieldObject;
            public GameObject DirectionObject;
            public GameObject VelocityObject;

            public CameraController camera_controller;

            public List<DebugWeaponInfo> weapons = new();

            public float LastRotation;

            public Material mat;

            // Start is called before the first frame update
            void Start() {
                State = GameObject.FindWithTag("SystemController").GetComponent<GameLoop>().CurrentSystemState;
                OrbitRender = gameObject.AddComponent<OrbitRenderer>();
                ShipRender = gameObject.AddComponent<SpriteRenderer>();

                ShieldObject = new GameObject();
                ShieldObject.name = "Shield Renderer";

                ShieldRender = ShieldObject.AddComponent<SpriteRenderer>();

                OrbitRender.descriptor = ship.descriptor;

                camera_controller = GameObject.Find("Main Camera").GetComponent<CameraController>();

                // Temporary sprites
                ShipRender.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
                ShieldRender.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

                Shader shader = Shader.Find("Hidden/Internal-Colored");
                mat = new Material(shader);
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
            void Update() {
                ShipRender.transform.position     = new Vector3(ship.self.posx, ship.self.posy, -0.1f);
                ShipRender.transform.localScale   = new Vector3(5.0f * width / camera_controller.scale, 5.0f / camera_controller.scale, 1.0f);

                ShipRender.transform.Rotate(new Vector3(0.0f, 0.0f, (ship.rotation - LastRotation) * 180.0f / 3.1415926f));

                ShieldRender.transform.position   = new Vector3(ship.self.posx, ship.self.posy, -0.05f);
                ShieldRender.transform.localScale = new Vector3(20.0f / camera_controller.scale, 15.0f / camera_controller.scale, 1.0f);

                ShieldRender.transform.Rotate(new Vector3(0.0f, 0.0f, (ship.rotation - LastRotation) * 180.0f / 3.1415926f));

                LastRotation       = ship.rotation;
                ShipRender.color   = shipColor;
                OrbitRender.color  = orbitColor;

                if (ship.max_shield == 0)
                    ShieldRender.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                else
                    ShieldRender.color = new Color(shieldColor.r, shieldColor.g, shieldColor.b, shieldColor.a * ship.shield / ship.max_shield);

                float v = Tools.magnitude(ship.self.velx, ship.self.vely);
                if(ship.weapons.Count > 0 || ship == State.Player.ship) {
                    Vector3[] vertices = new Vector3[2];
                    vertices[0] = new Vector3(ship.self.posx, ship.self.posy, -0.075f);
                    vertices[1] = new Vector3(ship.self.posx + (float)Math.Cos(ship.rotation) * 10.0f / camera_controller.scale, ship.self.posy + (float)Math.Sin(ship.rotation) * 10.0f / camera_controller.scale, -0.075f);
                    DirectionRenderer.SetPositions(vertices);
                    DirectionRenderer.positionCount = 2;
                    DirectionRenderer.startColor = DirectionRenderer.endColor = directionColor;
                    DirectionRenderer.startWidth = DirectionRenderer.endWidth = 0.2f / camera_controller.scale;

                    if(v != 0.0f) {
                        Vector3[] vertices2 = new Vector3[2];
                        vertices2[0] = new Vector3(ship.self.posx, ship.self.posy, -0.075f);
                        vertices2[1] = new Vector3(ship.self.posx + ship.self.velx / v * 10.0f / camera_controller.scale, ship.self.posy + ship.self.vely / v * 10.0f / camera_controller.scale, -0.075f);
                        VelocityRenderer.SetPositions(vertices2);
                        VelocityRenderer.positionCount = 2;
                        VelocityRenderer.startColor = VelocityRenderer.endColor = velocityColor;
                        VelocityRenderer.startWidth = VelocityRenderer.endWidth = 0.2f / camera_controller.scale;
                    }
                } else
                    DirectionRenderer.positionCount = VelocityRenderer.positionCount = 0;

                if (!ship.path_planned) OrbitRender.descriptor = null;
                else OrbitRender.descriptor = ship.descriptor;

                OrbitRender.UpdateRenderer(128);

                if(Tools.debug) {
                    // Add new weapons
                    foreach(ShipWeapon weapon in ship.weapons) {
                        if(weapon.FOV == 0.0f) continue;

                        bool found = false;

                        foreach(DebugWeaponInfo info in weapons)
                            if(info.weapon.Equals(weapon)) {
                                found = true;
                                break;
                            }

                        if(!found) {
                            DebugWeaponInfo info     = new DebugWeaponInfo();
                            info.weapon              = weapon;

                            info.obj1                = new GameObject();
                            info.obj1.name           = "Ship weapon cone renderer";
                            info.line1               = info.obj1.AddComponent<LineRenderer>();
                            info.line1.material      = mat;
                            info.line1.useWorldSpace = true;
                            info.line1.positionCount = 2;
                            info.line1.startColor    = debugLineColor;
                            info.line1.endColor      = debugLineColor;
                            info.vertices1           = new Vector3[2];
                            info.vertices1[0]        = new Vector3(0.0f, 0.0f, 0.02f);
                            info.vertices1[1]        = new Vector3(0.0f, 0.0f, 0.02f);

                            info.obj2                = new GameObject();
                            info.obj2.name           = "Ship weapon cone renderer";
                            info.line2               = info.obj2.AddComponent<LineRenderer>();
                            info.line2.material      = mat;
                            info.line2.useWorldSpace = true;
                            info.line2.positionCount = 2;
                            info.line2.startColor    = debugLineColor;
                            info.line2.endColor      = debugLineColor;
                            info.vertices2           = new Vector3[2];
                            info.vertices2[0]        = new Vector3(0.0f, 0.0f, 0.02f);
                            info.vertices2[1]        = new Vector3(0.0f, 0.0f, 0.02f);

                            weapons.Add(info);
                        }
                    }

                    // Remove weapons that have been removed or replaced
                    for(int i = 0; i < weapons.Count; i++)
                        if(!ship.weapons.Contains(weapons[i].weapon)) {
                            weapons[i].delete();
                            weapons.RemoveAt(i);
                            i--;
                        }

                    // Update renderers
                    foreach(DebugWeaponInfo info in weapons) {
                        info.vertices1[0].x    = ship.self.posx;
                        info.vertices1[0].y    = ship.self.posy;

                        info.vertices1[1].x    = ship.self.posx + (float)Math.Cos(info.weapon.rotation - info.weapon.FOV / 2.0f) * info.weapon.range;
                        info.vertices1[1].y    = ship.self.posy + (float)Math.Sin(info.weapon.rotation - info.weapon.FOV / 2.0f) * info.weapon.range;

                        info.vertices2[0].x    = ship.self.posx;
                        info.vertices2[0].y    = ship.self.posy;

                        info.vertices2[1].x    = ship.self.posx + (float)Math.Cos(info.weapon.rotation + info.weapon.FOV / 2.0f) * info.weapon.range;
                        info.vertices2[1].y    = ship.self.posy + (float)Math.Sin(info.weapon.rotation + info.weapon.FOV / 2.0f) * info.weapon.range;

                        info.line1.startWidth  = 0.1f / camera_controller.scale;
                        info.line1.endWidth    = 0.1f / camera_controller.scale;
                        info.line1.SetPositions(info.vertices1);

                        info.line2.startWidth  = 0.1f / camera_controller.scale;
                        info.line2.endWidth    = 0.1f / camera_controller.scale;
                        info.line2.SetPositions(info.vertices2);
                    }
                }
            }

            void OnDestroy() {
                GameObject.Destroy(ShipRender);
                GameObject.Destroy(ShieldRender);
                GameObject.Destroy(OrbitRender);
                GameObject.Destroy(DirectionRenderer);
                GameObject.Destroy(VelocityRenderer);
                GameObject.Destroy(ShieldObject);
                GameObject.Destroy(DirectionObject);
                GameObject.Destroy(VelocityObject);

                foreach(DebugWeaponInfo weapon in weapons) {
                    GameObject.Destroy(weapon. obj1);
                    GameObject.Destroy(weapon. obj2);
                    GameObject.Destroy(weapon.line1);
                    GameObject.Destroy(weapon.line2);
                }
            }
        }
    }
}