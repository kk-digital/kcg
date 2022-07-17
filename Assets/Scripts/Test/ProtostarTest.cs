using UnityEngine;

namespace Scripts {
    namespace SystemView {
        public class ProtostarTest : MonoBehaviour {
            public SystemState state;
            public int         inner_planets;
            public int         outer_planets;

            private void Start() {
                state.stars.Add(new());

                state.stars[0].Object.self.mass    = 1E9f;
                state.stars[0].Object.self.posx    = 0.0f;
                state.stars[0].Object.self.posy    = 0.0f;
                state.stars[0].Object.render_orbit = false;
                state.stars[0].Object.brightness   = 0.08f;

                state.generate_renderers();
            }
        }
    }
}
