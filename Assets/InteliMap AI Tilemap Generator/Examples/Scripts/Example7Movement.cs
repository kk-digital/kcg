using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteliMap
{
    public class Example7Movement : MonoBehaviour
    {
        public Vector3 start;
        public Vector3 end;

        public float speed;

        private static Vector3 move = new Vector3(1, 0.5f);

        public void LateUpdate()
        {
            transform.position += move * speed * Time.deltaTime;

            if (transform.position.x > end.x && transform.position.y > end.y)
            {
                transform.position = start;
            }
        }
    }
}