using UnityEngine;
using System.Collections;

namespace Devdog.SciFiDesign.UI
{
    public class TransformRotationFromMousePosition : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _rotation;

        private float _prevX;
        private float _prevY;
        protected void Update()
        {
            if (Mathf.Approximately(Input.mousePosition.x, _prevX) == false || Mathf.Approximately(Input.mousePosition.y, _prevY) == false)
            {
                // Cursor moved

                var normalized = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
                normalized.x -= 0.5f;
                normalized.x *= 2f;

                normalized.y -= 0.5f;
                normalized.y *= 2f;

                var rot = _rotation;
                rot.x *= normalized.y;
                rot.y *= normalized.x;

                transform.rotation = Quaternion.Euler(rot);
            }

            _prevX = Input.mousePosition.x;
            _prevY = Input.mousePosition.y;
        }
    }
}