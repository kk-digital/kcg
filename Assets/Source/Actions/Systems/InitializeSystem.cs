using Entitas;
using UnityEngine;

namespace Action
{ 
    public struct InitializeSystem
    {

        public int CreatePickUpAction(int agentID, int itemID)
        {
            // Pick Up action.
            int actionID = GameState.ActionCreationSystem.CreateAction((int)Enums.ActionType.PickUpAction, agentID);
            GameState.ActionCreationSystem.SetItem(actionID, itemID);
            return actionID;
        }

        private static void CreatePlaceTileAction(Tile.TileEnum tileType, Planet.PlanetState planetState)
        {
            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.PlaceTilOre1Action + (int)tileType);
            GameState.ActionAttributeManager.SetLogicFactory(new PlaceTileActionCreator());
            GameState.ActionAttributeManager.SetPlanet(planetState);
            PlaceTileToolAction.Data data = new PlaceTileToolAction.Data();
            data.tileType = tileType;
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

            CreatePlaceTileAction(Tile.TileEnum.Ore1, planetState);
            CreatePlaceTileAction(Tile.TileEnum.Ore2, planetState);
            CreatePlaceTileAction(Tile.TileEnum.Ore3, planetState);
            CreatePlaceTileAction(Tile.TileEnum.Glass, planetState);
            CreatePlaceTileAction(Tile.TileEnum.Moon, planetState);
            CreatePlaceTileAction(Tile.TileEnum.Pipe, planetState);

            GameState.ActionAttributeManager.CreateActionAttributeType(Enums.ActionType.EnemySpawnAction);
            GameState.ActionAttributeManager.SetLogicFactory(new EnemySpawnActionCreator());
            GameState.ActionAttributeManager.SetPlanet(planetState);
            EnemySpawnToolAction.Data data = new EnemySpawnToolAction.Data();
            data.CharacterSpriteId = GameResources.SlimeSpriteSheet;
            data.Material = material;
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
