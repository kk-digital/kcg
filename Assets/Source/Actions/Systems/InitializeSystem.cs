using Entitas;
using Enums.Tile;
using UnityEngine;

namespace Action
{ 
    public class InitializeSystem
    {
        public int CreatePickUpAction(Contexts entitasContext, int agentID, int itemID)
        {
            // Pick Up action.
            int actionID = GameState.ActionCreationSystem.CreateAction(entitasContext,
                                Enums.ActionType.PickUpAction, agentID, itemID);
            return actionID;
        }
        
        private static void CreateToolActionPlaceTile(Contexts entitasContext, TileID tileID, MapLayerType layer)
        {
            // Todo: Shit code is gonna break all the time. Fix this.
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.PlaceTilOre1Action + (int)tileID - (int)TileID.Ore1);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionPlaceTileCreator());
            var data = new ToolActionPlaceTile.Data
            {
                TileID = tileID,
                Layer = layer
            };
            GameState.ActionPropertyManager.SetData(data);
            GameState.ActionPropertyManager.EndActionPropertyType();
        }

        public void Initialize(Contexts entitasContext, Material material)
        {
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.DropAction);
            GameState.ActionPropertyManager.SetLogicFactory(new DropActionCreator());
            GameState.ActionPropertyManager.SetTime(2.0f);
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.PickUpAction);
            GameState.ActionPropertyManager.SetLogicFactory(new PickUpActionCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.MoveAction);
            GameState.ActionPropertyManager.SetLogicFactory(new MoveActionCreator());
            GameState.ActionPropertyManager.Movement();
            GameState.ActionPropertyManager.EndActionPropertyType();

            CreateToolActionPlaceTile(entitasContext, TileID.Ore1, MapLayerType.Front);
            CreateToolActionPlaceTile(entitasContext, TileID.Ore2, MapLayerType.Front);
            CreateToolActionPlaceTile(entitasContext, TileID.Ore3, MapLayerType.Front);
            CreateToolActionPlaceTile(entitasContext, TileID.Glass, MapLayerType.Front);
            CreateToolActionPlaceTile(entitasContext, TileID.Moon, MapLayerType.Front);
            CreateToolActionPlaceTile(entitasContext, TileID.Pipe, MapLayerType.Mid);

            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionFireWeapon);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionFireWeaponCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ReloadAction) ;
            GameState.ActionPropertyManager.SetLogicFactory(new ReloadActionCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionPlaceParticle);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionPlaceParticleCreator());
            ToolActionPlaceParticleEmitter.Data placeParticleEmitterData = new ToolActionPlaceParticleEmitter.Data();
            placeParticleEmitterData.Material = material;
            GameState.ActionPropertyManager.SetData(placeParticleEmitterData);
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionEnemySpawn);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionEnemySpawnCreator());
            ToolActionEnemySpawn.Data data = new ToolActionEnemySpawn.Data();
            data.CharacterSpriteId = GameResources.SlimeSpriteSheet;
            GameState.ActionPropertyManager.SetData(data);
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionMiningLaser);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionMiningLaserCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionRemoveTile);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionRemoveTileCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionThrowGrenade);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionThrowableGrenadeCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionMeleeAttack);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionMeleeAttackCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionPulseWeapon);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionPulseWeaponCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();
        }
    }
}
