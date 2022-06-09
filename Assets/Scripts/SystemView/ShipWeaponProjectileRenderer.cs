using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class ShipWeaponProjectileRenderer : MonoBehaviour
    {
        public ShipWeaponProjectile Projectile;

        public SpriteRenderer sr;

        public CameraController Camera;

        void Start()
        {
            sr = gameObject.AddComponent<SpriteRenderer>();

            // Temporary circular sprite
            sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

            Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

            sr.transform.position = new Vector3(Projectile.Body.PosX, Projectile.Body.PosY, -0.06f);
            sr.transform.localScale = new Vector3(2.0f / Camera.scale, 2.0f / Camera.scale, 1.0f);
            sr.color = Projectile.ProjectileColor;
        }

        void Update()
        {
            sr.transform.position = new Vector3(Projectile.Body.PosX, Projectile.Body.PosY, -0.06f);
            sr.transform.localScale = new Vector3(2.0f / Camera.scale, 2.0f / Camera.scale, 1.0f);
            sr.color = Projectile.ProjectileColor;
        }
    }
}
