using KMath;

namespace FloatingText
{
    public class SpawnerSystem
    {
        Contexts EntitasContext;
        public SpawnerSystem(Contexts entitasContext)
        {
            EntitasContext = entitasContext;
        }

        public GameEntity SpawnFloatingText(string text, 
                                    float timeToLive, Vec2f velocity, Vec2f position,
                                    int Index)
        {
            var entity = EntitasContext.game.CreateEntity();

            entity.AddFloatingTextID(Index);
            entity.AddFloatingTextState(timeToLive, text);
            entity.AddFloatingTextMovable(velocity, position);

            return entity;
        }
    }
}

