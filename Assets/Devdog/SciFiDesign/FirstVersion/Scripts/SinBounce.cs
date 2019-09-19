using UnityEngine;
using System.Collections;

namespace Devdog.SciFiDesign
{
    public class SinBounce : MonoBehaviour
    {
        public float speed = 1f;
        public float range = 1f;

        private float _time;

        protected void Update()
        {
            _time += Time.deltaTime * speed;
            transform.Translate(0f, Mathf.Sin(_time) * range, 0f);
        }
    }
}