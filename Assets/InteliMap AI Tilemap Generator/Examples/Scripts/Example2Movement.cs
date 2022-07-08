using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteliMap
{
    public class Example2Movement : MonoBehaviour
    {
        public float xStart;
        public float xEnd;

        public float speed;

        public void LateUpdate()
        {
            transform.position += Vector3.right * speed * Time.deltaTime;

            if (transform.position.x > xEnd)
            {
                transform.position = new Vector3(xStart, transform.position.y, transform.position.z);
            }
        }
    }
}