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
            // Return Action ID
            return actionID;
        }
        
        private static void CreateToolActionPlaceTile(Contexts entitasContext, TileID tileID, MapLayerType layer)
        {
            // Create Action Property Type
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.PlaceTilOre1Action + (int)tileID - (int)TileID.Ore1);

            // Set Logic Factory
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionPlaceTileCreator());

            // Set Data Struct
            var data = new ToolActionPlaceTile.Data
            {
                // Set Tile ID
                TileID = tileID,
                
                // Set Layer
                Layer = layer
            };

            // Set Data
            GameState.ActionPropertyManager.SetData(data);

            // Call End Action Property Type
            GameState.ActionPropertyManager.EndActionPropertyType();
        }

        public void Initialize(Contexts entitasContext, Material material)
        {
            // Create Drop Action Property
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.DropAction);

            // Drop Action Creator
            GameState.ActionPropertyManager.SetLogicFactory(new DropActionCreator());
            GameState.ActionPropertyManager.SetTime(2.0f); // Time Component
            GameState.ActionPropertyManager.EndActionPropertyType();

            // Create Pickup Action
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.PickUpAction);
            GameState.ActionPropertyManager.SetLogicFactory(new PickUpActionCreator()); // Set Logic Factory
            GameState.ActionPropertyManager.EndActionPropertyType();

            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.MoveAction);
            GameState.ActionPropertyManager.SetLogicFactory(new MoveActionCreator());
            GameState.ActionPropertyManager.Movement();
            GameState.ActionPropertyManager.EndActionPropertyType();

            // Create Place Tile Tool Front
            CreateToolActionPlaceTile(entitasContext, TileID.Ore1, MapLayerType.Front);

            // Create Place Tile Tool Front
            CreateToolActionPlaceTile(entitasContext, TileID.Ore2, MapLayerType.Front);

            // Create Place Tile Tool Front
            CreateToolActionPlaceTile(entitasContext, TileID.Ore3, MapLayerType.Front);

            // Create Place Tile Tool Front
            CreateToolActionPlaceTile(entitasContext, TileID.Glass, MapLayerType.Front);

            // Create Place Tile Tool Front
            CreateToolActionPlaceTile(entitasContext, TileID.Moon, MapLayerType.Front);

            // Create Place Tile Tool Mid
            CreateToolActionPlaceTile(entitasContext, TileID.Pipe, MapLayerType.Mid);

            // Create Action Fire Weapon
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionFireWeapon);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionFireWeaponCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            // Create Action Reload
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ReloadAction) ;
            GameState.ActionPropertyManager.SetLogicFactory(new ReloadActionCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            // Create Shield Action
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ShieldAction);
            GameState.ActionPropertyManager.SetLogicFactory(new ShieldActionCreator());
            GameState.ActionPropertyManager.SetShieldActive(false);
            GameState.ActionPropertyManager.EndActionPropertyType();

            // Create Tool Action Place Particle
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionPlaceParticle);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionPlaceParticleCreator());
            ToolActionPlaceParticleEmitter.Data placeParticleEmitterData = new ToolActionPlaceParticleEmitter.Data();
            placeParticleEmitterData.Material = material;
            GameState.ActionPropertyManager.SetData(placeParticleEmitterData);
            GameState.ActionPropertyManager.EndActionPropertyType();

            // Create Tool Action Spawn Enemy
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionEnemySpawn);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionEnemySpawnCreator());
            ToolActionEnemySpawn.Data data = new ToolActionEnemySpawn.Data();
            data.CharacterSpriteId = GameResources.SlimeSpriteSheet;
            GameState.ActionPropertyManager.SetData(data);
            GameState.ActionPropertyManager.EndActionPropertyType();

            // Create Tool Action Mining Laser
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionMiningLaser);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionMiningLaserCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            // Create Tool Action Remove Tile
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionRemoveTile);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionRemoveTileCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            // Create Tool Action Throw Grenade
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionThrowGrenade);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionThrowableGrenadeCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            // Create Tool Action Melee Attack
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionMeleeAttack);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionMeleeAttackCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            // Create Tool Action Pulse Weapon
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionPulseWeapon);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionPulseWeaponCreator());
            GameState.ActionPropertyManager.EndActionPropertyType();

            // Create Tool Action Shield
            GameState.ActionPropertyManager.CreateActionPropertyType(entitasContext, Enums.ActionType.ToolActionShield);
            GameState.ActionPropertyManager.SetLogicFactory(new ToolActionShieldCreator());
            GameState.ActionPropertyManager.SetShieldActive(false);
            GameState.ActionPropertyManager.EndActionPropertyType();
        }
    }
}
