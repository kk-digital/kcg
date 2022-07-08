using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteliMap
{
    public class Example5Generation : MonoBehaviour
    {
        public float wait = 2.0f;
        private float timer = 0.0f;

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > wait)
            {
                foreach (InteliMapGenerator schematic in GetComponentsInChildren<InteliMapGenerator>())
                {
                    schematic.ClearBounds();
                    schematic.StartGeneration();
                }

                timer = 0.0f;
            }
        }
    }
}