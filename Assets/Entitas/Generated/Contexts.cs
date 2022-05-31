//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ContextsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts : Entitas.IContexts {

    public static Contexts sharedInstance {
        get {
            if (_sharedInstance == null) {
                _sharedInstance = new Contexts();
            }

            return _sharedInstance;
        }
        set { _sharedInstance = value; }
    }

    static Contexts _sharedInstance;

    public AgentContext agent { get; set; }
    public GameContext game { get; set; }
    public InputContext input { get; set; }

    public Entitas.IContext[] allContexts { get { return new Entitas.IContext [] { agent, game, input }; } }

    public Contexts() {
        agent = new AgentContext();
        game = new GameContext();
        input = new InputContext();

        var postConstructors = System.Linq.Enumerable.Where(
            GetType().GetMethods(),
            method => System.Attribute.IsDefined(method, typeof(Entitas.CodeGeneration.Attributes.PostConstructorAttribute))
        );

        foreach (var postConstructor in postConstructors) {
            postConstructor.Invoke(this, null);
        }
    }

    public void Reset() {
        var contexts = allContexts;
        for (int i = 0; i < contexts.Length; i++) {
            contexts[i].Reset();
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EntityIndexGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts {

    public const string Inventory2D = "Inventory2D";
    public const string InventoryItem = "InventoryItem";
    public const string Item = "Item";

    [Entitas.CodeGeneration.Attributes.PostConstructor]
    public void InitializeEntityIndices() {
        agent.AddEntityIndex(new Entitas.PrimaryEntityIndex<AgentEntity, int>(
            Inventory2D,
            agent.GetGroup(AgentMatcher.Inventory2D),
            (e, c) => ((Components.Agent.Inventory2DComponent)c).InventoryID));

        game.AddEntityIndex(new Entitas.EntityIndex<GameEntity, int>(
            InventoryItem,
            game.GetGroup(GameMatcher.InventoryItem),
            (e, c) => ((Components.InventoryItemComponent)c).InventoryID));

        game.AddEntityIndex(new Entitas.EntityIndex<GameEntity, Enums.ItemType>(
            Item,
            game.GetGroup(GameMatcher.Item),
            (e, c) => ((Components.ItemComponent)c).ItemType));
    }
}

public static class ContextsExtensions {

    public static AgentEntity GetEntityWithInventory2D(this AgentContext context, int InventoryID) {
        return ((Entitas.PrimaryEntityIndex<AgentEntity, int>)context.GetEntityIndex(Contexts.Inventory2D)).GetEntity(InventoryID);
    }

    public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithInventoryItem(this GameContext context, int InventoryID) {
        return ((Entitas.EntityIndex<GameEntity, int>)context.GetEntityIndex(Contexts.InventoryItem)).GetEntities(InventoryID);
    }

    public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithItem(this GameContext context, Enums.ItemType ItemType) {
        return ((Entitas.EntityIndex<GameEntity, Enums.ItemType>)context.GetEntityIndex(Contexts.Item)).GetEntities(ItemType);
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.VisualDebugging.CodeGeneration.Plugins.ContextObserverGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts {

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

    [Entitas.CodeGeneration.Attributes.PostConstructor]
    public void InitializeContextObservers() {
        try {
            CreateContextObserver(agent);
            CreateContextObserver(game);
            CreateContextObserver(input);
        } catch(System.Exception) {
        }
    }

    public void CreateContextObserver(Entitas.IContext context) {
        if (UnityEngine.Application.isPlaying) {
            var observer = new Entitas.VisualDebugging.Unity.ContextObserver(context);
            UnityEngine.Object.DontDestroyOnLoad(observer.gameObject);
        }
    }

#endif
}
