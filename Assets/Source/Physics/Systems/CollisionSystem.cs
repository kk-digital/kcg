using KMath;

namespace Physics
{
    public class CollisionSystem
    {
        private struct CollisionChecker
        {
            public Tile.Tile[] Tiles;
            public Vertices Vertices;
        }

        private CollisionChecker BroadPhase(ref Vertices vertices)
        {
            return new CollisionChecker();
        }
        
        private void NarrowPhase(ref CollisionChecker tiles)
        {
            
        }
    }
}

