using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

namespace Devdog.SciFiDesign.UI
{
    public class DragPreviewItem : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler
    {

        public Transform previewItem;
        public float rotationSpeed = 1f;

        public bool doInertia = true;
        public float inertiaFallofSpeed = 1f;
        private Vector2 _inertia;

        public string controllerAxisX;
        public string controllerAxisY;
        public float controllerRotationSpeed = 10f;

        private bool _stoppedDragging = false;
        private float _timer = 0f;

        public void Update()
        {
            if (string.IsNullOrEmpty(controllerAxisX) == false)
            {
                var axis = Input.GetAxis(controllerAxisX);
                Rotate(new Vector3(axis, 0f, 0f), controllerRotationSpeed);
            }
            if (string.IsNullOrEmpty(controllerAxisY) == false)
            {
                var axis = Input.GetAxis(controllerAxisY);
                Rotate(new Vector3(0f, axis, 0f), controllerRotationSpeed);
            }

            if (doInertia == false)
            {
                return;
            }

            // Handle inertia of spin
            if (_stoppedDragging)
            {
                _timer += Time.deltaTime * inertiaFallofSpeed;
                _inertia = Vector2.Lerp(_inertia, Vector2.zero, _timer);
                Rotate(new Vector3(_inertia.y, -_inertia.x, 0f), _inertia.magnitude);

                // Stop handling interpolation update to avoid jitter.
                if (_inertia == Vector2.zero)
                {
                    _stoppedDragging = false;
                }
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            _stoppedDragging = false;
            _inertia = Vector2.zero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _stoppedDragging = false;

            var d = eventData.delta.normalized;
            Rotate(new Vector3(d.y, -d.x, 0f), eventData.delta.magnitude * rotationSpeed);
        }

        protected virtual void Rotate(Vector3 rotation, float angle)
        {
            previewItem.RotateAround(previewItem.position, rotation, angle);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _stoppedDragging = true;
            _inertia = eventData.delta * rotationSpeed;
            _timer = 0f;
        }


    }
}