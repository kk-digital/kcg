using Enums;
using KMath;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Item
{

    public struct ItemProprieties
    {
        public ItemType ItemType;
        public int InventorSpriteID;
        public int SpriteID;
        public Vec2f SpriteSize;

        public Enums.ActionType ToolActionType;
        public int FireWeaponID;

        public Flags ItemFlags;
        [Flags]
        public enum Flags : byte
        {
            Placeable   = 1 << 0,
            Consumable  = 1 << 1,
            Stackable   = 1 << 2,
            Tool        = 1 << 3
        }

       public bool IsPlaceable() { return ItemFlags.HasFlag(Flags.Placeable); }
       public bool IsConsumable() { return ItemFlags.HasFlag(Flags.Consumable); }
       public bool IsStackable() { return ItemFlags.HasFlag(Flags.Stackable); }
       public bool IsTool() { return ItemFlags.HasFlag(Flags.Tool); }
    }

    public struct FireWeaponPropreties
    {
        /// <summary>
        /// Fire weapon basic attributes:
        /// 
        /// BulletSpeed     - Start Speed Position.
        /// CoolDown        - How Long it takes to shoot again in seconds.
        /// Range           - Max projectile range. 
        /// BasicDamage     - Demage on hit without any modifiers.
        /// BulletSpriteSize      - Size of the bullet sprite.
        /// BulletSprideID  - SpriteID.
        /// 
        /// </summary>
        public float BulletSpeed;
        public float CoolDown;
        public float Range;
        public float BasicDemage;

        public Vec2f BulletSpriteSize;
        public int BulletSpriteID;

        /// <summary>
        /// Number Off Bullets Per Shot
        /// </summary>
        public int BulletsPerShot;

        /// <summary>
        /// Can Recharge the gun
        /// </summary>
        public bool CanCharge;
        public float ChargeRate;
        public float ChargeRatio;
        public float ChargePerShot;
        public float ChargeMin;
        public float ChargeMax;
        /// <summary>
        /// Clip attributes.
        /// 
        /// ClipSize            - Max number of bullets in the clip.
        /// ReloadTime          - Time take to reload in seconds.
        /// </summary>
        public int ClipSize;
        public float ReloadTime;

        /// <summary>
        /// If gun shoots more than one bullet.
        /// 
        /// SpreadAngle         - Cone angle in which bullets will be spreaded.
        /// NumOfBullets        - Number of bullets discharged every shot.
        /// </summary>
        public float SpreadAngle;
        public int NumOfBullets;

        /// <summary>
        /// Define Accuracy of the firegun.
        /// MaxRecoilAngle -> Max Cone angle which shooted bullets can go to.
        /// MinRecoilAngle -> Cone angle of the first bullet.
        /// RateOfChange -> How much cone angle is increased after every shoot
        /// RecoverTime -> How long it takes for recoil to go back to min from MaxRecoilAngle in seconds.
        /// RecoverDelay -> How long it takes to recoil startRecovering in seconds.
        /// </summary>
        public float MaxRecoilAngle;
        public float MinRecoilAngle;
        public float RateOfChange;
        public float RecoverTime;
        public float RecoverDelay;

        public Flags WeaponFlags;
        [Flags]
        public enum Flags : byte
        {
            HasClip = 1 << 0,
            ShouldSpread = 2 << 1,
            HasCharge = 3 << 2
        }
        public bool HasClip() { return WeaponFlags.HasFlag(Flags.HasClip); }
        public bool ShouldSpread() { return WeaponFlags.HasFlag(Flags.ShouldSpread); }
        public bool HasCharge() { return WeaponFlags.HasFlag(Flags.HasCharge); }
    }
}
