using KMath;

namespace FloatingText
{
    public class FloatingTextSpawnerSystem
    {

        public GameEntity SpawnFloatingText(GameContext gameContext, string text, 
                                    float timeToLive, Vec2f velocity, Vec2f position,
                                    int Index)
        {
            var entity = gameContext.CreateEntity();

            entity.AddFloatingTextID(Index);
            entity.AddFloatingTextState(timeToLive, text);
            entity.AddFloatingTextMovable(velocity, position);

            return entity;
        }
    }
}

