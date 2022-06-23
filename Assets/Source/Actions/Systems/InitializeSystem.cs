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

        private static void CreatePlaceTileAction(TileID tileID, Planet.PlanetState planetState)
        {
            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.PlaceTilOre1Action + (int)tileID);
            GameState.ActionAttributeManager.SetLogicFactory(new PlaceTileActionCreator());
            GameState.ActionAttributeManager.SetPlanet(planetState);
            var data = new PlaceTileToolAction.Data
            {
                TileID = tileID
            };
            GameState.ActionAttributeManager.SetData(data);
            GameState.ActionAttributeManager.EndActionAttributeType();
        }

        public void Initialize(Planet.PlanetState planetState, Material material)
        {
            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.DropAction);
            GameState.ActionAttributeManager.SetLogicFactory(new DropActionCreator());
            GameState.ActionAttributeManager.SetTime(2.0f);
            GameState.ActionAttributeManager.EndActionAttributeType();

            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.PickUpAction);
            GameState.ActionAttributeManager.SetLogicFactory(new PickUpActionCreator());
            GameState.ActionAttributeManager.EndActionAttributeType();

            CreatePlaceTileAction(TileID.Ore1, planetState);
            CreatePlaceTileAction(TileID.Ore2, planetState);
            CreatePlaceTileAction(TileID.Ore3, planetState);
            CreatePlaceTileAction(TileID.Glass, planetState);
            CreatePlaceTileAction(TileID.Moon, planetState);
            CreatePlaceTileAction(TileID.Pipe, planetState);

            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.EnemySpawnAction);
            GameState.ActionAttributeManager.SetLogicFactory(new EnemySpawnActionCreator());
            GameState.ActionAttributeManager.SetPlanet(planetState);
            EnemySpawnToolAction.Data data = new EnemySpawnToolAction.Data();
            data.CharacterSpriteId = GameResources.SlimeSpriteSheet;
            GameState.ActionAttributeManager.SetData(data);
            GameState.ActionAttributeManager.EndActionAttributeType();

            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.PlaceParticleEmitterAction);
            GameState.ActionAttributeManager.SetLogicFactory(new PlaceParticleEmitterActionCreator());
            GameState.ActionAttributeManager.SetPlanet(planetState);
            PlaceParticleEmitterToolAction.Data placeParticleEmitterData = new PlaceParticleEmitterToolAction.Data();
            placeParticleEmitterData.Material = material;
            GameState.ActionAttributeManager.SetData(placeParticleEmitterData);
            GameState.ActionAttributeManager.EndActionAttributeType();

            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.MiningLaserAction);
            GameState.ActionAttributeManager.SetLogicFactory(new MiningLaserActionCreator());
            GameState.ActionAttributeManager.SetPlanet(planetState);
            GameState.ActionAttributeManager.EndActionAttributeType();

            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.RemoveTileAction);
            GameState.ActionAttributeManager.SetLogicFactory(new RemoveTileActionCreator());
            GameState.ActionAttributeManager.SetPlanet(planetState);
            GameState.ActionAttributeManager.EndActionAttributeType();
        }
    }
}
