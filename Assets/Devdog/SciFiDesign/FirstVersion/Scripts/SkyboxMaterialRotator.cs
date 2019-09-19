using UnityEngine;
using System.Collections;
using Devdog.General;

namespace Devdog.SciFiDesign
{
    public class SkyboxMaterialRotator : MonoBehaviour
    {
        [Required]
        public Material material;

        public float rotationSpeed = 1f;

        private float _timer;
        protected void Update()
        {
            _timer += Time.deltaTime * rotationSpeed;
            _timer = _timer % 360f;
            material.SetFloat("_Rotation", _timer);
        }
    }
}