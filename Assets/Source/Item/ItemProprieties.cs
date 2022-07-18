using Enums;
using KMath;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Item
{

    public struct ItemProprieties
    {
        // Item Type
        public ItemType ItemType;

        // Item's Invetory Sprite Id
        public int InventorSpriteID;

        // Item's Sprite ID
        public int SpriteID;

        // Item's Sprite Size
        public Vec2f SpriteSize;

        // Item's Tool Action Type
        public Enums.ActionType ToolActionType;

        // Item's Fire Weapon ID
        public int FireWeaponID;

        /// <summary>
        /// Item Flags:
        /// 
        /// Placeable -> If Item Placable Flag
        /// Consumable -> If Item Consumable
        /// Stackable -> If Item Stackable (Takeable more than once)
        /// Tool -> If Item is Tool
        /// 
        /// </summary>
        public Flags ItemFlags;
        [Flags]
        public enum Flags : byte
        {
            Placeable   = 1 << 0,
            Consumable  = 1 << 1,
            Stackable   = 1 << 2,
            Tool        = 1 << 3
        }


        // Is Item Placable
       public bool IsPlaceable() { return ItemFlags.HasFlag(Flags.Placeable); }

        // Is Item Consumable
        public bool IsConsumable() { return ItemFlags.HasFlag(Flags.Consumable); }

        // Is Item Stackable
        public bool IsStackable() { return ItemFlags.HasFlag(Flags.Stackable); }

        // Is Item Tool
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
        /// Can Recharge -> Recharge Condition
        /// Charge Rate -> Charge Precentage
        /// Charge Ratio -> Charge Multipiler
        /// Charge Per Shot -> Charge will decrease per shot
        /// Charge Min -> Minumum Charge
        /// Charge Max -> Maximum Charge
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

        /// <summary>
        /// Melee Attack Properties
        /// 
        /// StaggerTime -> Stun Time
        /// Stagger Rate -> Stagger Multipiler
        /// Shield Active -> Shield Condition
        /// 
        /// </summary>
        public float StaggerTime;
        [Range(0, 1)]
        public float StaggerRate;

        public bool ShieldActive;

        /// <summary>
        /// Pulse Weapon Properties
        /// 
        /// isLaunchGreanade -> Launch Grenade Mode Condition
        /// NumberOfGrenades-> Number Of Grenades in Clip
        /// GrenadeClipSize -> Size of the Grenade Clip
        /// 
        /// </summary>
        public bool isLaunchGreanade;
        public int NumberOfGrenades;
        public int GrenadeClipSize;

        /// <summary>
        /// Weapon Flags
        /// 
        /// HasClip -> If Weapon Has an Ammo Clip
        /// ShouldSpread -> Should Weapon Spread the Ammos (ex, pump shotgun)
        /// HasCharge -> If Weapon Chargable or Not
        /// PulseWeapon -> If the weapon is a pulse weapon or not
        /// 
        /// </summary>
        public Flags WeaponFlags;
        [Flags]
        public enum Flags : byte
        {
            HasClip = 1 << 0,
            ShouldSpread = 2 << 1,
            HasCharge = 3 << 2,
            PulseWeapon = 4 << 3, 
        }

        /// <summary>
        /// Melee Flags
        /// 
        /// Stab -> If Melee Weapon Stabs
        /// Slash -> If Melee Weapon Slashes
        /// 
        /// </summary>
        public MeleeFlags MeleeAttackFlags;
        public enum MeleeFlags : byte
        {
            Stab = 1 << 0,
            Slash = 2 << 1
        }

        /// <summary>
        /// Grenades Flags
        /// 
        /// Cocussions -> Cocussions Bombs
        /// Flame -> Flame Bombs
        /// Fragmentation -> Fragmentation Bombs
        /// 
        /// </summary>
        public GrenadesFlags GrenadeFlags;
        public enum GrenadesFlags : byte
        {
            Cocussions = 1 << 0,
            Flame = 2 << 1,
            Fragmentation = 3 << 2
        }

        // Weapon Has Clip?
        public bool HasClip() { return WeaponFlags.HasFlag(Flags.HasClip); }

        // Weapon Should Spread?
        public bool ShouldSpread() { return WeaponFlags.HasFlag(Flags.ShouldSpread); }

        // Weapon Has Charge Flag?
        public bool HasCharge() { return WeaponFlags.HasFlag(Flags.HasCharge); }

        // Is Weapon Stabs
        public bool IsStab() { return WeaponFlags.HasFlag(MeleeFlags.Stab); }

        // Is Weapon Slash
        public bool IsSlash() { return WeaponFlags.HasFlag(MeleeFlags.Slash); }

        // Is Weapon Pulse
        public bool IsPulse() { return WeaponFlags.HasFlag(Flags.PulseWeapon); }

    }
}
