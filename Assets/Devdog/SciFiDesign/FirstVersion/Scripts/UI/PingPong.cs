using UnityEngine;
using System.Collections;

namespace Devdog.SciFiDesign.UI
{
    public class PingPong : MonoBehaviour
    {
        public Vector2 scaleAxis = new Vector2(1f, 0f);
        public float scaleMin = 5f;
        public float scaleMax = 10f;

        public float speedMin = 5f;
        public float speedMax = 10f;

        private float _scale;
        private float _speed;
        private float _randomOffset;
        private RectTransform _rectTransform;
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _randomOffset = Random.value;
            _speed = Random.Range(speedMin, speedMax);
            _scale = Random.Range(scaleMin, scaleMax);
        }

        protected void Update()
        {
            var val = Mathf.PingPong(Time.time * _speed + _randomOffset, _scale);
            _rectTransform.anchoredPosition = new Vector2(val*scaleAxis.x, val*scaleAxis.y);
        }
    }
}