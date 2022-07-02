using KMath;

namespace FloatingText
{
    public class FloatingTextSpawnerSystem
    {

        public FloatingTextEntity SpawnFloatingText(FloatingTextContext floatingTextContext, string text, 
                                    float timeToLive, Vec2f velocity, Vec2f position,
                                    int Index)
        {
            var entity = floatingTextContext.CreateEntity();

            entity.AddFloatingTextID(Index);
            entity.AddFloatingTextState(timeToLive, text);
            entity.AddFloatingTextMovable(velocity, position);
            entity.AddFloatingTextSprite(Utility.ObjectMesh.CreateEmptyTextGameObject("FloatingText"));

            return entity;
        }
    }
}

