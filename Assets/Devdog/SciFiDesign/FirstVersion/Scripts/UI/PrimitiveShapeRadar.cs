using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class PrimitiveShapeRadar : Graphic
    {
        [System.Serializable]
        public class Element
        {
            public string name;
            public Color vertexColor = Color.white;

            [SerializeField]
            private float _statValue = 1f;
            public float statValue
            {
                get { return _statValue; }
                set { _statValue = value; }
            }

            /// <summary>
            /// Used for interpolation
            /// </summary>
            public float aimValue { get; set; }
            public float startValue { get; set; }
            public float animOffset { get; set; }
        }

        public Element[] elements = new Element[0];
        public float scaleFactor = 1f;
        public Color centerColor = Color.white;

        public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public float animateSpeed = 1f;
        private IEnumerator _valuesCoroutine;


        public void AnimateInValues()
        {
            if (_valuesCoroutine != null)
            {
                StopCoroutine(_valuesCoroutine);
            }

            if (gameObject.activeInHierarchy)
            {
                _valuesCoroutine = _AnimateInValues();
                StartCoroutine(_valuesCoroutine);
            }
            else
            {
                foreach (var element in elements)
                {
                    element.statValue = element.startValue;
                }
            }
        }

        private IEnumerator _AnimateInValues()
        {
            float offset = 0f;
            foreach (var element in elements)
            {
                element.animOffset = UnityEngine.Random.value;
                element.startValue = element.statValue;
                offset = Mathf.Max(offset, element.animOffset);
            }

            float timer = 0f;
            while (timer < (1f + offset))
            {
                timer += Time.deltaTime*animateSpeed;
                var curvedTime = animationCurve.Evaluate(timer);

                foreach (var element in elements)
                {
                    element.statValue = Mathf.Lerp(element.startValue, element.aimValue, curvedTime);
                }

                UpdateElements();
                yield return null;
            }
        }

        public void UpdateElements()
        {
            UpdateGeometry();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            var r = GetPixelAdjustedRect();

            vh.Clear();

            // Center point
            vh.AddVert(new Vector3(r.x + r.width / 2f, r.y + r.height / 2f, 0f), centerColor, Vector2.zero);
            for (int i = 0; i < elements.Length; i++)
            {
                float val = ((float)i / elements.Length) * Mathf.PI * 2;
                var x = Mathf.Cos(val) * elements[i].statValue * scaleFactor;
                var y = Mathf.Sin(val) * elements[i].statValue * scaleFactor;

                vh.AddVert(new Vector3(x * r.x, y * r.y), elements[i].vertexColor * color, Vector2.zero);
            }

            for (int i = 1; i < elements.Length; i++)
            {
                vh.AddTriangle(i, i + 1, 0); // Both edges + center element.
            }

            vh.AddTriangle(elements.Length, 1, 0); // Both edges + center element.
        }
    }
}