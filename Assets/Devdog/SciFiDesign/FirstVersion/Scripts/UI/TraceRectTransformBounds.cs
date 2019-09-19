using UnityEngine;
using System.Collections;
using Devdog.General;
using UnityEngine.Assertions;

namespace Devdog.SciFiDesign.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class TraceRectTransformBounds : MonoBehaviour
    {

        [Range(-10f, 10f)]
        public float speed = 1f;

        public float offset;

        [Required]
        public RectTransform toTrace;


        public Vector2 multiplier = Vector2.one;


        private RectTransform _rectTransform;
        private float _time = 0f;

        protected void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            Assert.IsNotNull(_rectTransform, "No rectTransform found, which is required on TraceRectTransformBounds");

            _time = offset;
        }
        
        protected void Update()
        {
            var startPosition = toTrace.rect.position;
            startPosition.x += toTrace.rect.width / 2;
            startPosition.y += toTrace.rect.height / 2;

            _time += Time.deltaTime * speed;

            startPosition += new Vector2(Mathf.Cos(_time) * (toTrace.rect.width / 2) * multiplier.x, Mathf.Sin(_time) * (toTrace.rect.height / 2) * multiplier.y);
            
            _rectTransform.anchoredPosition = startPosition;
        }
    }
}