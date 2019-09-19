using UnityEngine;
using System.Collections;
using Devdog.General;

namespace Devdog.SciFiDesign
{
    public class SmoothFollow : MonoBehaviour
    {
        [Required]
        public Transform target;

        public float distance = 10.0f;
        public float height = 5.0f;

        public float heightDamping = 2.0f;
        public float rotationDamping = 3.0f;

        protected void LateUpdate()
        {
            var newRotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationDamping * Time.deltaTime);
            var newHeight = Mathf.Lerp(transform.position.y, target.position.y + (target.up.y * height), heightDamping * Time.deltaTime);

            var p = target.position;
            p -= newRotation * target.forward * distance;
            p.y = newHeight;

            transform.position = p;
            transform.rotation = newRotation;
        }
    }
}