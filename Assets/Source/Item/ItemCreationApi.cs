using Entitas;
using UnityEngine;
using System;
using KMath;
using Agent;
using System.Collections.Generic;
using Enums;
using Planet;
using Sprites;

/*
    How To use it:
        Item.CreationApi.CreateItem(Item Type, Item Type Name);
        Item.CreationApi.SetTexture(SpriteSheetID);
        Item.CreationApi.SetInventoryTexture(SpriteSheetID);
        Item.CreationApi.MakeStackable(Max number of items in a stack.);
        Item.CreationApi.EndItem();
*/

namespace Item
{
    public class ItemCreationApi
    {
        // Constructor is called before the first frame update.
         
        // Note[Joao] this arrays are very memory expensive: use array of pointers instead?
        private ItemProprieties[] PropertiesArray;
        private FireWeaponPropreties[] WeaponList;
        private string[] ItemTypeLabels;

        ItemType CurrentIndex;
        int WeaponListSize;

        public ItemCreationApi()
        {
            int length = Enum.GetValues(typeof(ItemType)).Length - 1; // -1 beacause of error item type.
            PropertiesArray = new ItemProprieties[length];
            ItemTypeLabels = new string[length];
            WeaponList = new FireWeaponPropreties[16];
            CurrentIndex = ItemType.Error;
            WeaponListSize = 0;

            for (int i = 0; i < PropertiesArray.Length; i++)
            {
                PropertiesArray[i].ItemType = CurrentIndex;
            }
        }

        public string GetLabel(Enums.ItemType type)
        {
            ItemType itemType = PropertiesArray[(int)type].ItemType;
            IsItemTypeValid(itemType);

            return ItemTypeLabels[(int)type];
        }


        public ItemProprieties Get(Enums.ItemType type)
        {
            ItemType itemType = PropertiesArray[(int)type].ItemType;
            IsItemTypeValid(itemType);

            return PropertiesArray[(int)type];
        }

        public FireWeaponPropreties GetWeapon(Enums.ItemType type)
        {
            ItemType itemType = PropertiesArray[(int)type].ItemType;
            IsItemTypeValid(itemType);

            return WeaponList[PropertiesArray[(int)type].FireWeaponID];
        }

        public void CreateItem(Enums.ItemType itemType, string name)
        {
            CurrentIndex = itemType;

            PropertiesArray[(int)itemType].ItemType = itemType;
            ItemTypeLabels[(int)itemType] = name;
        }

        public void SetName(string name)
        {
            IsItemTypeValid();

            ItemTypeLabels[(int)CurrentIndex] = name;
        }

        public void SetSpriteSize(Vec2f size)
        {
            IsItemTypeValid();

            PropertiesArray[(int)CurrentIndex].SpriteSize = size;
        }

        public void SetTexture(int spriteId)
        {
            IsItemTypeValid();

            PropertiesArray[(int)CurrentIndex].SpriteID = spriteId;
        }


        public void SetInventoryTexture(int spriteId)
        {
            IsItemTypeValid();

            PropertiesArray[(int)CurrentIndex].InventorSpriteID = spriteId;
        }

        public void SetAction(Enums.ActionType actionID)
        {
            IsItemTypeValid();

            PropertiesArray[(int)CurrentIndex].ToolActionType = actionID;
            PropertiesArray[(int)CurrentIndex].ItemFlags |= ItemProprieties.Flags.Tool;
        }

        public void SetConsumable(int maxStackCount)
        {
            IsItemTypeValid();

            PropertiesArray[(int)CurrentIndex].ItemFlags |= ItemProprieties.Flags.Consumable;
        }

        public void SetStackable(int maxStackCount)
        {
            IsItemTypeValid();

            PropertiesArray[(int)CurrentIndex].ItemFlags |= ItemProprieties.Flags.Stackable;
        }

        public void SetPlaceable()
        {
            IsItemTypeValid();

            PropertiesArray[(int)CurrentIndex].ItemFlags |= ItemProprieties.Flags.Placeable;
        }

        public void SetThrowableGrenade(float bulletSpeed, float coolDown, float range, float basicDamage, Vec2f spriteSize, int greandeSpriteID)
        {
            IsItemTypeValid();

            FireWeaponPropreties fireWeapon = new FireWeaponPropreties()
            {
                BulletSpeed = bulletSpeed,
                CoolDown = coolDown,
                Range = range,
                BasicDemage = basicDamage,
                BulletSpriteSize = spriteSize,
                BulletSpriteID = greandeSpriteID
            };

            WeaponList[WeaponListSize] = fireWeapon;
            PropertiesArray[(int)CurrentIndex].FireWeaponID = WeaponListSize++;
        }

        public void SetThrowableGrenadeClip(int clipSize, int bulletsPerShot, float reloadTime, FireWeaponPropreties.GrenadesFlags gflags)
        {
            IsItemTypeValid();

            ref FireWeaponPropreties fireWeapon = ref WeaponList[PropertiesArray[(int)CurrentIndex].FireWeaponID];
            fireWeapon.ClipSize = clipSize;
            fireWeapon.BulletsPerShot = bulletsPerShot;
            fireWeapon.ReloadTime = reloadTime;
            fireWeapon.WeaponFlags |= FireWeaponPropreties.Flags.HasClip;
            fireWeapon.GrenadeFlags |= gflags;
        }

        public void SetThrowableRPG(float bulletSpeed, float coolDown, float range, float basicDamage, Vec2f spriteSize, int greandeSpriteID)
        {
            IsItemTypeValid();

            FireWeaponPropreties fireWeapon = new FireWeaponPropreties()
            {
                BulletSpeed = bulletSpeed,
                CoolDown = coolDown,
                Range = range,
                BasicDemage = basicDamage,
                BulletSpriteSize = spriteSize,
                BulletSpriteID = greandeSpriteID
            };

            WeaponList[WeaponListSize] = fireWeapon;
            PropertiesArray[(int)CurrentIndex].FireWeaponID = WeaponListSize++;
        }

        public void SetThrowableRPGClip(int clipSize, int bulletsPerShot, float reloadTime)
        {
            IsItemTypeValid();

            ref FireWeaponPropreties fireWeapon = ref WeaponList[PropertiesArray[(int)CurrentIndex].FireWeaponID];
            fireWeapon.ClipSize = clipSize;
            fireWeapon.BulletsPerShot = bulletsPerShot;
            fireWeapon.ReloadTime = reloadTime;
            fireWeapon.WeaponFlags |= FireWeaponPropreties.Flags.HasClip;
        }

        public void SetShotgunWeapon(float bulletSpeed, float coolDown, float range, float spreadAngle, float basicDamage, Vec2f spriteSize, int bulletSpriteID)
        {
            IsItemTypeValid();

            FireWeaponPropreties fireWeapon = new FireWeaponPropreties()
            {
                BulletSpeed = bulletSpeed,
                CoolDown = coolDown,
                Range = range,
                SpreadAngle = spreadAngle,
                BasicDemage = basicDamage,
                BulletSpriteSize = spriteSize,
                BulletSpriteID = bulletSpriteID
            };

            WeaponList[WeaponListSize] = fireWeapon;
            PropertiesArray[(int)CurrentIndex].FireWeaponID = WeaponListSize++;
        }

        public void SetSwordWeapon(float coolDown, float range, float staggerTime, float staggerRate, float basicDamage)
        {
            IsItemTypeValid();

            FireWeaponPropreties fireWeapon = new FireWeaponPropreties()
            {
                CoolDown = coolDown,
                Range = range,
                StaggerTime = staggerTime,
                StaggerRate = staggerRate,
                BasicDemage = basicDamage,
            };

            WeaponList[WeaponListSize] = fireWeapon;
            PropertiesArray[(int)CurrentIndex].FireWeaponID = WeaponListSize++;
        }

        public void SetSwordWeaponMeleeType(FireWeaponPropreties.MeleeFlags type)
        {
            IsItemTypeValid();

            ref FireWeaponPropreties fireWeapon = ref WeaponList[PropertiesArray[(int)CurrentIndex].FireWeaponID];
            fireWeapon.MeleeAttackFlags |= type;
        }

        public void SetShotgunWeaponClip(int clipSize, int bulletsPerShot, float reloadTime)
        {
            IsItemTypeValid();

            ref FireWeaponPropreties fireWeapon = ref WeaponList[PropertiesArray[(int)CurrentIndex].FireWeaponID];
            fireWeapon.ClipSize = clipSize;
            fireWeapon.BulletsPerShot = bulletsPerShot;
            fireWeapon.ReloadTime = reloadTime;
            fireWeapon.WeaponFlags |= FireWeaponPropreties.Flags.HasClip | FireWeaponPropreties.Flags.ShouldSpread;
        }

        public void SetFireWeapon(float bulletSpeed, float coolDown, float range, float basicDamage, Vec2f spriteSize, int bulletSpriteID)
        {
            IsItemTypeValid();

            FireWeaponPropreties fireWeapon = new FireWeaponPropreties()
            {
                BulletSpeed = bulletSpeed,
                CoolDown = coolDown,
                Range = range,
                BasicDemage = basicDamage,
                BulletSpriteSize = spriteSize,
                BulletSpriteID = bulletSpriteID
            };

            WeaponList[WeaponListSize] = fireWeapon;
            PropertiesArray[(int)CurrentIndex].FireWeaponID = WeaponListSize++;
        }

        public void SetFireWeaponClip(int clipSize, int bulletsPerShot, float reloadTime)
        {
            IsItemTypeValid();

            ref FireWeaponPropreties fireWeapon = ref WeaponList[PropertiesArray[(int)CurrentIndex].FireWeaponID];
            fireWeapon.ClipSize = clipSize;
            fireWeapon.BulletsPerShot = bulletsPerShot;
            fireWeapon.ReloadTime = reloadTime;
            fireWeapon.WeaponFlags |= FireWeaponPropreties.Flags.HasClip;
        }

        public void SetFireWeaponMultiShoot(float speadAngle, int numOfBullet)
        {
            IsItemTypeValid();

            ref FireWeaponPropreties fireWeapon = ref WeaponList[PropertiesArray[(int)CurrentIndex].FireWeaponID];
            fireWeapon.SpreadAngle = speadAngle;
            fireWeapon.NumOfBullets = numOfBullet;
        }

        public void SetFireWeaponRecoil(float maxRecoilAngle, float minRecoilAngle, float rateOfChange, float recoverTime, float recoverDelay)
        {
            IsItemTypeValid();

            ref FireWeaponPropreties fireWeapon = ref WeaponList[PropertiesArray[(int)CurrentIndex].FireWeaponID];
            fireWeapon.MaxRecoilAngle = maxRecoilAngle;
            fireWeapon.MinRecoilAngle = minRecoilAngle;
            fireWeapon.RateOfChange = rateOfChange;
            fireWeapon.RecoverTime = recoverTime;
            fireWeapon.RecoverDelay = recoverDelay;
        }

        public void SetPulseWeapon(float bulletSpeed, float coolDown, float range, bool isLaunchGrenade, float basicDamage, Vec2f spriteSize, int bulletSpriteID)
        {
            IsItemTypeValid();

            FireWeaponPropreties fireWeapon = new FireWeaponPropreties()
            {
                BulletSpeed = bulletSpeed,
                CoolDown = coolDown,
                Range = range,
                isLaunchGreanade = isLaunchGrenade,
                BasicDemage = basicDamage,
                BulletSpriteSize = spriteSize,
                BulletSpriteID = bulletSpriteID
            };

            WeaponList[WeaponListSize] = fireWeapon;
            PropertiesArray[(int)CurrentIndex].FireWeaponID = WeaponListSize++;
        }

        public void SetPulseWeaponClipSize(int bulletClipSize, int greandeClipSize, int bulletsPerShot, float bulletReloadTime)
        {
            IsItemTypeValid();

            ref FireWeaponPropreties fireWeapon = ref WeaponList[PropertiesArray[(int)CurrentIndex].FireWeaponID];
            fireWeapon.ClipSize = bulletClipSize;
            fireWeapon.GrenadeClipSize = greandeClipSize;
            fireWeapon.NumberOfGrenades = greandeClipSize;
            fireWeapon.BulletsPerShot = bulletsPerShot;
            fireWeapon.ReloadTime = bulletReloadTime;
            fireWeapon.WeaponFlags |= FireWeaponPropreties.Flags.HasClip | FireWeaponPropreties.Flags.PulseWeapon;
        }

        public void EndItem()
        {
            // Todo: Check if ItemType is valid in debug mode.
            CurrentIndex = ItemType.Error;
        }

        private void IsItemTypeValid(ItemType itemType)
        {
#if DEBUG
            if (itemType == ItemType.Error)
            {
                Debug.Log("Not valid ItemType");
                Utils.Assert(false);
            }
#endif
        }

        private void IsItemTypeValid()
        {
            IsItemTypeValid(CurrentIndex);
        }
    }
}
