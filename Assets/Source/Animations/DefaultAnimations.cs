namespace Animation
{



    public struct DefaultAnimation
    {


        public static void CreateDefaultAnimation()
        {
            Game.State.AnimationManager.CreateAnimation((int)AnimationType.Generic);
            Game.State.AnimationManager.SetName("Generic");
            Game.State.AnimationManager.SetFrameCount(5);
            Game.State.AnimationManager.SetTimePerFrame(0.15f);
            Game.State.AnimationManager.EndAnimation();

            Game.State.AnimationManager.CreateAnimation((int)AnimationType.Particle);
            Game.State.AnimationManager.SetName("Particle");
            Game.State.AnimationManager.SetFrameCount(5);
            Game.State.AnimationManager.SetTimePerFrame(0.15f);
            Game.State.AnimationManager.EndAnimation();
        }
    }
}