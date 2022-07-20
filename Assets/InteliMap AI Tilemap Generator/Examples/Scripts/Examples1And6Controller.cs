using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace InteliMap
{
    [RequireComponent(typeof(Camera))]
    public class Examples1And6Controller : MonoBehaviour
    {
        public InteliMapGenerator[] schematics;
        public GameObject ui;
        public float speed;

        private Camera cam;

        public void Start()
        {
            cam = GetComponent<Camera>();
        }

        public void LateUpdate()
        {
            if (Input.GetKey(KeyCode.W))
            {
                cam.transform.position += Vector3.up * speed * cam.orthographicSize * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                cam.transform.position += Vector3.left * speed * cam.orthographicSize * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                cam.transform.position += Vector3.down * speed * cam.orthographicSize * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                cam.transform.position += Vector3.right * speed * cam.orthographicSize * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                foreach (InteliMapGenerator schematic in schematics)
                {
                    schematic.ClearBounds();
                    schematic.StartGeneration();
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                ui.SetActive(!ui.activeSelf);
            }
        }
    }
}