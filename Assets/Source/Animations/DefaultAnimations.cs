namespace Animation
{



    public struct DefaultAnimation
    {


        public static void CreateDefaultAnimation()
        {
            GameState.AnimationManager.CreateAnimation((int)AnimationType.Generic);
            GameState.AnimationManager.SetName("Generic");
            GameState.AnimationManager.SetFrameCount(5);
            GameState.AnimationManager.SetTimePerFrame(0.15f);
            GameState.AnimationManager.EndAnimation();

            GameState.AnimationManager.CreateAnimation((int)AnimationType.Particle);
            GameState.AnimationManager.SetName("Particle");
            GameState.AnimationManager.SetFrameCount(5);
            GameState.AnimationManager.SetTimePerFrame(0.15f);
            GameState.AnimationManager.EndAnimation();
        }
    }
}