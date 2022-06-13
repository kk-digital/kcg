namespace Animation
{



    public struct Animation
    {
        public int Type;

        public float CurrentTime;
        public int CurrentFrame;
        public int BaseSpriteId;



        // must be called once per frame, the animation speed
        // will depend on the velocity of the entity 
        public void Update(float deltaTime, float animationSpeed)
        {
            CurrentTime += animationSpeed * deltaTime;
            AnimationProperties animationType = GameState.AnimationManager.Get(Type);
        
            CurrentFrame = (int)(CurrentTime / animationType.TimePerFrame) % animationType.FrameCount;
        }

        public int GetSpriteId()
        {
            return BaseSpriteId + CurrentFrame;
        }
    }
}