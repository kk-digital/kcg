using Entitas;
using UnityEngine;
using System;
using KMath;

/*
    How To use it:
        Item.CreationApi.Instance.CreateItem(Item Type, Item Type Name);
        Item.CreationApi.Instance.SetTexture(SpriteSheetID);
        Item.CreationApi.Instance.SetInventoryTexture(SpriteSheetID);
        Item.CreationApi.Instance.MakeStackable(Max number of items in a stack.);
        Item.CreationApi.Instance.EndItem();
*/

namespace Item
{
    public class CreationApi
    {
        private static CreationApi instance;
        public static CreationApi Instance => instance ??= new CreationApi();


        private ItemPropertiesEntity ItemType = null;

        public void CreateItem(Contexts entitasContext, Enums.ItemType itemType, string name)
        {
            ItemType = entitasContext.itemProperties.CreateEntity();
            ItemType.AddItemProperty(itemType, name);
        }

        public void SetName(string name)
        {
            if (ItemType == null)
                return;

            var Attributes = ItemType.itemProperty;
            ItemType.ReplaceItemProperty(Attributes.ItemType, name);
        }

        public void SetSize(Vec2f size)
        {
            ItemType.AddItemPropertySize(size);
        }

        public void SetTexture(int spriteId)
        {
            if (ItemType == null)
                return;

            ItemType.AddItemPropertySprite(spriteId);
        }


        public void SetInventoryTexture(int spriteId)
        {
            if (ItemType == null)
                return;

            ItemType.AddItemPropertyInventorySprite(spriteId);

        }

        public void SetAction(int actionID)
        {
            ItemType.AddItemPropertyAction(actionID);
        }

        public void SetStackable(int maxStackCount)
        {
            if (ItemType == null)
                return;

            ItemType.AddItemPropertyStackable(maxStackCount);
        }

        public void SetPlaceable()
        {
            if (ItemType == null)
                return;

            ItemType.isItemPropertyPlaceable = true;
        }

        public void SetEquipament()
        {
            if (ItemType == null)
                return;

            ItemType.isItemPropertyPlaceable = true;
        }

        public void EndItem()
        {
            // Todo: Check if ItemType is valid in debug mode.
            ItemType = null;
        }
    }
}
