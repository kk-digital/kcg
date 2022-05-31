using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class ShipWeaponProjectileRenderer : MonoBehaviour
    {
        public ShipWeaponProjectile Projectile;

        public SpriteRenderer sr;

        void Start()
        {
            sr = gameObject.AddComponent<SpriteRenderer>();

            // Temporary circular sprite
            sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

            sr.transform.position = new Vector3(Projectile.PosX, Projectile.PosY, -0.1f);
            sr.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            sr.color = Projectile.ProjectileColor;
        }

        void Update()
        {
            sr.transform.position = new Vector3(Projectile.PosX, Projectile.PosY, -0.1f);
            sr.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            sr.color = Projectile.ProjectileColor;
        }
    }
}
