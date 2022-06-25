namespace Animation
{



    public struct Animation
    {
        public int Type;

        public float CurrentTime;
        public int CurrentFrame;



        // must be called once per frame, the animation speed
        // will depend on the velocity of the entity 
        public void Update(float deltaTime, float animationSpeed)
        {
            CurrentTime += animationSpeed * deltaTime;
            AnimationProperties animationType = GameState.AnimationManager.Get(Type);

            if (animationType.TimePerFrame >= -0.001 && animationType.TimePerFrame <= 0.001)
            {
                animationType.TimePerFrame = 1.0f;
            }
            if (animationType.FrameCount == 0)
            {
                animationType.FrameCount = 1;
            }
        
            CurrentFrame = (int)(CurrentTime / animationType.TimePerFrame) % animationType.FrameCount;
        }

        public int GetSpriteId()
        {
            AnimationProperties animationType = GameState.AnimationManager.Get(Type);
            return animationType.BaseSpriteId + CurrentFrame;
        }
    }
}
