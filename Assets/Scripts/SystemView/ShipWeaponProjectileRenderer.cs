using System;
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

        public float Scale;

        void Start()
        {
            sr = gameObject.AddComponent<SpriteRenderer>();

            // Temporary circular sprite
            sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");

            Camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

            Scale = (float)Math.Log(Projectile.Damage * 0.3f) * 0.6f;

            sr.transform.position = new Vector3(Projectile.Body.posx, Projectile.Body.posy, -0.06f);
            sr.transform.localScale = new Vector3(Scale / Camera.scale, Scale / Camera.scale, 1.0f);
            sr.color = Projectile.ProjectileColor;
        }

        void Update()
        {
            sr.transform.position = new Vector3(Projectile.Body.posx, Projectile.Body.posy, -0.06f);
            sr.transform.localScale = new Vector3(Scale / Camera.scale, Scale / Camera.scale, 1.0f);
            sr.color = Projectile.ProjectileColor;
        }

        void OnDestroy()
        {
            GameObject.Destroy(sr);
        }
    }
}
