using UnityEngine;
using System.Collections;

namespace Devdog.SciFiDesign
{
    public class Spin : MonoBehaviour
    {

        public Vector3 rotation;

        protected void Update()
        {
            transform.Rotate(rotation * Time.deltaTime);
        }
    }
}