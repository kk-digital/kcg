using UnityEngine;

namespace Scripts {
    namespace SystemView {
        public class ProtostarTest : MonoBehaviour {
            public SystemState State;

            private void Start() {
                State.stars.Add(new());

                State.stars[0].Object.self.mass    = 1E9f;
                State.stars[0].Object.self.posx    = 0.0f;
                State.stars[0].Object.self.posy    = 0.0f;
                State.stars[0].Object.render_orbit = false;
                State.stars[0].Object.brightness   = 0.08f;

                State.generate_renderers();
            }
        }
    }
}
