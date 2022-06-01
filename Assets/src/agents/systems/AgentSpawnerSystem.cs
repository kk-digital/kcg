using UnityEngine;

namespace Agent
{
    public static class AgentContextExtension
    {
        public static AgentEntity CreatePlayer(this AgentContext agentContext, Material material, Transform parent)
        {
            var entity = agentContext.CreateEntity();

            entity.isPlayer = true;
            
            var spriteSize = new Vector2Int(32, 48);
            var spriteID = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\Moonbunker\\Tilesets\\Sprites\\character\\character.png", spriteSize.x, spriteSize.y);
            var atlasIndex = GameState.SpriteAtlasManager.CopySpriteToAtlas(spriteID, 0, 0, 0);
            entity.AddSprite2D(atlasIndex, parent, material, spriteSize);

            entity.AddPosition2D(new Vector2(3f, 2f), default);

            return entity;
        }
    }
    
    public class AgentSpawnerSystem
    {
        public static readonly AgentSpawnerSystem Instance;
        public AgentContext AgentContext;

        static AgentSpawnerSystem()
        {
            Instance = new AgentSpawnerSystem();
        }

        public AgentSpawnerSystem()
        {
            AgentContext = Contexts.sharedInstance.agent;
        }

        public AgentEntity SpawnPlayer(Material material, Transform parent)
        {
            return AgentContext.CreatePlayer(material, parent);
        }
    }
}

