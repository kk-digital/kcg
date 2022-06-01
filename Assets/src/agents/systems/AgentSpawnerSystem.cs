using UnityEngine;

namespace Agent
{
    public class AgentSpawnerSystem
    {
        public static readonly AgentSpawnerSystem Instance;
        public AgentContext AgentContext;

        private static int playerID;

        static AgentSpawnerSystem()
        {
            Instance = new AgentSpawnerSystem();
        }

        public AgentSpawnerSystem()
        {
            AgentContext = Contexts.sharedInstance.agent;
        }

        public AgentEntity SpawnPlayer(Material material)
        {
            var entity = AgentContext.CreateEntity();

            playerID++;

            entity.AddPlayer(playerID);

            var spriteSize = new Vector2Int(32, 48);
            var spritePath = "Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png";
            var spriteID = GameState.SpriteLoader.GetSpriteSheetID(spritePath, spriteSize.x, spriteSize.y);

            entity.AddSprite2D(spriteID, spritePath, spriteSize, material, null);
            entity.sprite2D.Init();

            entity.AddPosition2D(new Vector2(3f, 2f), default);

            return entity;
        }
    }
}

