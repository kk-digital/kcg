namespace Components
{
    public struct CollisionInfo
    {
        public bool Above;
        public bool Below;
        public bool Left;
        public bool Right;

        public void Reset() => Above = Below = Left = Right = false;
    
        public bool Collided() => Above || Below || Left || Right;
        public bool Horizontal() => Left || Right;
        public bool Vertical() => Above || Below;
        public bool VerticalAbove() => Above;
    }
}
