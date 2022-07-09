using UnityEngine;
using System.Collections.Generic;

using Enums;
using KMath;

namespace Item
{
    public class SpawnerSystem
    {
        private static int ItemID;

        public ItemEntity SpawnItem(Contexts entitasContext, ItemType itemType, Vec2f position)
        {
            ItemProprieties itemProperty = GameState.ItemCreationApi.Get(itemType);
            FireWeaponPropreties weaponProperty = GameState.ItemCreationApi.GetWeapon(itemType);

            Vec2f size = itemProperty.SpriteSize;

            var entity = entitasContext.item.CreateEntity();
            entity.AddItemID(ItemID);
            entity.AddItemType(itemType);
            entity.AddPhysicsPosition2D(position, Vec2f.Zero);
            entity.AddPhysicsBox2DCollider(size, Vec2f.Zero);
            entity.AddPhysicsMovable(0f, Vec2f.Zero, Vec2f.Zero, true, true, false, false);

            if (weaponProperty.HasClip())
            {
                entity.AddItemFireWeaponClip(weaponProperty.ClipSize);
            }

            if (weaponProperty.HasCharge())
            {
                entity.AddItemFireWeaponCharge(weaponProperty.CanCharge, weaponProperty.ChargeRate, weaponProperty.ChargeMin, 
                    weaponProperty.ChargeMax);
            }

            ItemID++;
            return entity;
        }

        public ItemEntity SpawnInventoryItem(Contexts entitasContext, ItemType itemType)
        {
            ItemProprieties itemProperty = GameState.ItemCreationApi.Get(itemType);
            FireWeaponPropreties weaponProperty = GameState.ItemCreationApi.GetWeapon(itemType);

            var entity = entitasContext.item.CreateEntity();
            entity.AddItemID(ItemID);
            entity.AddItemType(itemType);

            if (weaponProperty.HasClip())
            {
                entity.AddItemFireWeaponClip(weaponProperty.ClipSize);
            }

            if (weaponProperty.HasCharge())
            {
                entity.AddItemFireWeaponCharge(weaponProperty.CanCharge, weaponProperty.ChargeRate, weaponProperty.ChargeMin,
                    weaponProperty.ChargeMax);
            }

            ItemID++;
            return entity;
        }
    }
}

