using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class SystemStarRenderer : MonoBehaviour {
            public SystemStar star;

            public SpriteRenderer sr;
            public OrbitRenderer  or;

            public Color orbit_color = new Color(0.7f, 0.92f, 1.0f, 1.0f);
            public Color star_color  = Color.white;

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
                sr.transform.localScale = new Vector3(10.0f / Camera.scale, 10.0f / Camera.scale, 1.0f);

                sr.color = star_color;
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
