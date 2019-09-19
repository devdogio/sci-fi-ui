using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderInterpolator : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField]
        private float _from = 0f;
        public float from
        {
            get { return Mathf.Max(_from, minFrom); }
        }

        [Range(0f, 1f)]
        public float minFrom = 0f;

        [Range(0f, 1f)]
        [SerializeField]
        private float _to = 1f;
        public float to
        {
            get { return Mathf.Min(_to, maxTo); }
        }

        [Range(0f, 1f)]
        public float maxTo = 1f;

        public float startDelay = 0f;

        public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public float speed = 1f;

        private Slider _slider;
        public Slider slider
        {
            get { return _slider; }
        }

        private float _timer = 0f;
        private WaitForSeconds _startDelay;

        protected void Awake()
        {
            _slider = GetComponent<Slider>();
            _startDelay = new WaitForSeconds(startDelay);
            InterpolateValue();
        }

        public void InterpolateValue(float to)
        {
            _to = to;
            InterpolateValue();
        }

        public void InterpolateValue(float from, float to)
        {
            _from = from;
            _to = to;
            InterpolateValue();
        }

        public void InterpolateValue()
        {
            StartCoroutine(_InterpolateValue());
        }


        protected IEnumerator _InterpolateValue()
        {
            _timer = 0f;
            _slider.value = _from;
            yield return _startDelay;

            while (_timer < 1f)
            {
                _timer += Time.deltaTime * speed;

                var val = Mathf.Lerp(_from, _to, _timer);
                _slider.value = animationCurve.Evaluate(_timer) * val;
                yield return null;
            }
        }
    }
}