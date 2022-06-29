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

        private static void CreatePlaceTileAction(TileID tileID, MapLayerType layer)
        {
            // Todo: Shit code is gonna break all the time. Fix this.
            GameState.ActionPropertyManager.CreateActionPropertyType(Enums.ActionType.PlaceTilOre1Action + (int)tileID - (int)TileID.Ore1);
            GameState.ActionPropertyManager.SetLogicFactory(new PlaceTileActionCreator());
            var data = new PlaceTileToolAction.Data
            {
                TileID = tileID,
                Layer = layer
            };
            GameState.ActionPropertyManager.SetData(data);
            GameState.ActionPropertyManager.EndActionPropertyType();
        }

        public void Initialize(Material material)
        {
            GameState.ActionPropertyManager.CreateActionPropertyType(Enums.ActionType.DropAction);
            GameState.ActionPropertyManager.SetLogicFactory(new DropActionCreator());
            GameState.ActionPropertyManager.SetTime(2.0f);
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(Enums.ActionType.PickUpAction);
            GameState.ActionPropertyManager.SetLogicFactory(new PickUpActionCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            CreatePlaceTileAction(TileID.Ore1, MapLayerType.Front);
            CreatePlaceTileAction(TileID.Ore2, MapLayerType.Front);
            CreatePlaceTileAction(TileID.Ore3, MapLayerType.Front);
            CreatePlaceTileAction(TileID.Glass, MapLayerType.Front);
            CreatePlaceTileAction(TileID.Moon, MapLayerType.Front);
            CreatePlaceTileAction(TileID.Pipe, MapLayerType.Mid);

            GameState.ActionPropertyManager.CreateActionPropertyType(Enums.ActionType.FireWeaponAction);
            GameState.ActionPropertyManager.SetLogicFactory(new FireWeaponActionCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(Enums.ActionType.PlaceParticleEmitterAction);
            GameState.ActionPropertyManager.SetLogicFactory(new PlaceParticleEmitterActionCreator());
            PlaceParticleEmitterToolAction.Data placeParticleEmitterData = new PlaceParticleEmitterToolAction.Data();
            placeParticleEmitterData.Material = material;
            GameState.ActionPropertyManager.SetData(placeParticleEmitterData);
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(Enums.ActionType.EnemySpawnAction);
            GameState.ActionPropertyManager.SetLogicFactory(new EnemySpawnActionCreator());
            EnemySpawnToolAction.Data data = new EnemySpawnToolAction.Data();
            data.CharacterSpriteId = GameResources.SlimeSpriteSheet;
            GameState.ActionPropertyManager.SetData(data);
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(Enums.ActionType.MiningLaserAction);
            GameState.ActionPropertyManager.SetLogicFactory(new MiningLaserActionCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(Enums.ActionType.RemoveTileAction);
            GameState.ActionPropertyManager.SetLogicFactory(new RemoveTileActionCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();
        }
    }
}
