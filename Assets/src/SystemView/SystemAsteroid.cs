namespace SystemView
{
    public class SystemAsteroid
    {
        public float RotationalPosition;
        public int Layer;

        // todo: add more properties

        public SystemAsteroid()
        {

        }

        public void UpdatePosition(float dt)
        {
            RotationalPosition += dt;
        }
    }
}
