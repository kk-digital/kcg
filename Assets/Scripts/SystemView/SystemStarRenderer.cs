using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class SystemStarRenderer : MonoBehaviour {
            public SystemStar star;

            public SpriteRenderer sr;
            public OrbitRenderer  or;

            public Color orbit_color = new Color(0.7f, 0.92f, 1.0f, 1.0f);
            public Color star_color  = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            public CameraController Camera;

            // Start is called before the first frame update
            void Start() {
                or = gameObject.AddComponent<OrbitRenderer>();
                sr = gameObject.AddComponent<SpriteRenderer>();

                Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

                or.descriptor = star.descriptor;

                // Temporary circular sprite
                sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
            }

            // Update is called once per frame
            void Update() {
                sr.transform.position   = new Vector3(star.self.posx, star.self.posy, -0.1f);

                if(star.brightness < 0.08f) { // Brown dwarf
                    star_color.r = Tools.smootherstep(0.0f, 0.30f, star.brightness / 0.08f);
                    star_color.g = Tools.smootherstep(0.0f, 0.15f, star.brightness / 0.08f);
                    star_color.b = Tools.smootherstep(0.0f, 0.05f, star.brightness / 0.08f);

                    float scale  = Tools.smootherstep(0.0f, 4.00f, star.brightness / 0.08f);

                    sr.transform.localScale = new Vector3(scale / Camera.scale, scale / Camera.scale, 1.0f);
                } else if(star.brightness < 1.0f) {
                    float value = (star.brightness - 0.08f) / 0.92f;

                    if(!star.giant) { // Yellow main sequence star
                        star_color.r = Tools.smootherstep(0.30f, 1.00f, value);
                        star_color.g = Tools.smootherstep(0.15f, 1.00f, value);
                        star_color.b = Tools.smootherstep(0.05f, 0.85f, value);

                        float scale  = Tools.smootherstep(4.00f, 10.0f, value);

                        sr.transform.localScale = new Vector3(scale / Camera.scale, scale / Camera.scale, 1.0f);
                    } else { // Blue giant
                        star_color.r = Tools.smootherstep(0.30f, 0.81f, value);
                        star_color.g = Tools.smootherstep(0.15f, 0.92f, value);
                        star_color.b = Tools.smootherstep(0.05f, 1.00f, value);

                        float scale  = Tools.smootherstep(4.00f, 25.0f, value);

                        sr.transform.localScale = new Vector3(scale / Camera.scale, scale / Camera.scale, 1.0f);
                    }
                } else if(star.brightness < 2.0f) {
                    float value = star.brightness - 1.0f;
                    if(!star.giant) { // Red giant
                        star_color.r = 1.0f;
                        star_color.g = Tools.smootherstep(1.00f, 0.70f, value);
                        star_color.b = Tools.smootherstep(0.85f, 0.65f, value);

                        float scale  = Tools.smootherstep(10.0f, 40.0f, value);

                        sr.transform.localScale = new Vector3(scale / Camera.scale, scale / Camera.scale, 1.0f);
                    } else { // Red super giant
                        star_color.r = Tools.smootherstep(0.81f, 1.00f, value);
                        star_color.g = Tools.smootherstep(0.92f, 0.10f, value);
                        star_color.b = Tools.smootherstep(1.00f, 0.04f, value);

                        float scale  = Tools.smootherstep(25.0f, 75.0f, value);

                        sr.transform.localScale = new Vector3(scale / Camera.scale, scale / Camera.scale, 1.0f);
                    }
                } else { // White dwarf / neutron star
                    star_color.r = 1.0f;
                    star_color.g = 1.0f;
                    star_color.b = 1.0f;
                    
                    float scale  = Tools.smootherstep(star.giant ? 75.0f : 40.0f, 5.0f, star.brightness - 2.0f);

                    sr.transform.localScale = new Vector3(scale / Camera.scale, scale / Camera.scale, 1.0f);
                }

                sr.color = star_color; // DOESN'T WORK
                or.color = orbit_color;

                if(star.render_orbit)
                    or.update_renderer(128);
            }

            void OnDestroy() {
                GameObject.Destroy(sr);
            }
        }
    }
}
