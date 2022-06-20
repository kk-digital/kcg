using UnityEngine;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class SystemPlanetRenderer : MonoBehaviour {
            public SystemPlanet planet;

            public SpriteRenderer sr;
            public OrbitRenderer or;

            public Color orbitColor  = new Color(0.5f, 0.7f, 1.0f, 1.0f);
            public Color planetColor = Color.white;

            public CameraController Camera;

            // Start is called before the first frame update
            void Start() {
                or = gameObject.AddComponent<OrbitRenderer>();
                sr = gameObject.AddComponent<SpriteRenderer>();

                Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

                or.descriptor = planet.descriptor;

                // Temporary circular sprite
                sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
            }

            // LateUpdate is called once per frame
            void LateUpdate() {
                float[] pos = planet.descriptor.get_position();

                sr.transform.position = new Vector3(pos[0], pos[1], -0.1f);
                sr.transform.localScale = new Vector3(3.0f / Camera.scale, 3.0f / Camera.scale, 1.0f);

                sr.color = planetColor;
                or.color = orbitColor;

                or.UpdateRenderer(128);
            }

            void OnDestroy() {
                GameObject.Destroy(sr);
                GameObject.Destroy(or);
            }
        }
    }
}
