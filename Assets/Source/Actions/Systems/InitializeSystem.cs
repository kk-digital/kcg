using Entitas;
using Enums.Tile;
using UnityEngine;

namespace Action
{ 
    public class InitializeSystem
    {
        public int CreatePickUpAction(int agentID, int itemID)
        {
            // Pick Up action.
            int actionID = GameState.ActionCreationSystem.CreateAction((int)Enums.ActionType.PickUpAction, agentID);
            GameState.ActionCreationSystem.SetItem(actionID, itemID);
            return actionID;
        }

        private static void CreatePlaceTileAction(TileID tileID)
        {
            // Todo: Shit code is gonna break all the time. Fix this.
            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.PlaceTilOre1Action + (int)tileID - (int)TileID.Ore1);
            GameState.ActionAttributeManager.SetLogicFactory(new PlaceTileActionCreator());
            var data = new PlaceTileToolAction.Data
            {
                TileID = tileID
            };
            GameState.ActionAttributeManager.SetData(data);
            GameState.ActionAttributeManager.EndActionAttributeType();
        }

        public void Initialize(Material material)
        {
            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.DropAction);
            GameState.ActionAttributeManager.SetLogicFactory(new DropActionCreator());
            GameState.ActionAttributeManager.SetTime(2.0f);
            GameState.ActionAttributeManager.EndActionAttributeType();

            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.PickUpAction);
            GameState.ActionAttributeManager.SetLogicFactory(new PickUpActionCreator());
            GameState.ActionAttributeManager.EndActionAttributeType();

            CreatePlaceTileAction(TileID.Ore1);
            CreatePlaceTileAction(TileID.Ore2);
            CreatePlaceTileAction(TileID.Ore3);
            CreatePlaceTileAction(TileID.Glass);
            CreatePlaceTileAction(TileID.Moon);
            CreatePlaceTileAction(TileID.Pipe);

            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.FireWeaponAction);
            GameState.ActionAttributeManager.SetLogicFactory(new FireWeaponActionCreator());
            GameState.ActionAttributeManager.EndActionAttributeType();

            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.PlaceParticleEmitterAction);
            GameState.ActionAttributeManager.SetLogicFactory(new PlaceParticleEmitterActionCreator());
            PlaceParticleEmitterToolAction.Data placeParticleEmitterData = new PlaceParticleEmitterToolAction.Data();
            placeParticleEmitterData.Material = material;
            GameState.ActionAttributeManager.SetData(placeParticleEmitterData);
            GameState.ActionAttributeManager.EndActionAttributeType();

            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.EnemySpawnAction);
            GameState.ActionAttributeManager.SetLogicFactory(new EnemySpawnActionCreator());
            EnemySpawnToolAction.Data data = new EnemySpawnToolAction.Data();
            data.CharacterSpriteId = GameResources.SlimeSpriteSheet;
            GameState.ActionAttributeManager.SetData(data);
            GameState.ActionAttributeManager.EndActionAttributeType();

            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.MiningLaserAction);
            GameState.ActionAttributeManager.SetLogicFactory(new MiningLaserActionCreator());
            GameState.ActionAttributeManager.EndActionAttributeType();

            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.RemoveTileAction);
            GameState.ActionAttributeManager.SetLogicFactory(new RemoveTileActionCreator());
            GameState.ActionAttributeManager.EndActionAttributeType();
        }
    }
}
