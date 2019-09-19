using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    [AddComponentMenu("UI/Custom/Dropdown", 35)]
    public class Dropdown : UnityEngine.UI.Dropdown
    {
        public Sprite activeSprite;
        public Sprite inActiveSprite;
        public Image backgroundImage;

        public float slideSpeed = 5f;
        public AnimationCurve animationCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

        private Vector2 _startSize;

        private RectTransform _rectTransform;
        private Color _normalStartColor;
        private Color _imageStartColor;


        protected override void Start()
        {
            base.Start();

            _rectTransform = GetComponent<RectTransform>();
            _normalStartColor = colors.normalColor;
            _startSize = _rectTransform.sizeDelta;
            _imageStartColor = backgroundImage.color;
        }

        protected override GameObject CreateBlocker(Canvas rootCanvas)
        {
            var obj = base.CreateBlocker(rootCanvas);

            StartCoroutine(_GrowToSize(_rectTransform));
            backgroundImage.sprite = activeSprite;
            var c = colors;
            c.normalColor = colors.pressedColor;
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 1f);
            colors = c;
            return obj;
        }

        protected override void DestroyBlocker(GameObject blocker)
        {
            base.DestroyBlocker(blocker);
            StartCoroutine(_ShrinkToSize(_rectTransform, _startSize));
            var c = colors;
            c.normalColor = _normalStartColor;
            colors = c;
            backgroundImage.color = _imageStartColor;
            backgroundImage.sprite = inActiveSprite;
        }


        private IEnumerator _GrowToSize(RectTransform rectTransform)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundImage.rectTransform);
            var toSize = new Vector2(LayoutUtility.GetPreferredWidth(backgroundImage.rectTransform), LayoutUtility.GetPreferredHeight(backgroundImage.rectTransform));
            
            float time = 0f;
            var fromSize = rectTransform.sizeDelta;
            while (true)
            {
                var newSize = rectTransform.sizeDelta;
                newSize.x = Mathf.Lerp(fromSize.x, toSize.x, animationCurve.Evaluate(time)); // animationCurve.Evaluate(time)
                newSize.y = Mathf.Lerp(fromSize.y, toSize.y, animationCurve.Evaluate(time));

                rectTransform.sizeDelta = newSize;

                if (newSize.x >= toSize.x && newSize.y >= toSize.y)
                {
                    break;
                }

                time += Time.deltaTime * slideSpeed;
                yield return null;
            }
        }

        private IEnumerator _ShrinkToSize(RectTransform rectTransform, Vector2 toSize)
        {
            float time = 0f;
            var fromSize = rectTransform.sizeDelta;
            while (true)
            {
                var newSize = rectTransform.sizeDelta;
                newSize.x = Mathf.Lerp(fromSize.x, toSize.x, animationCurve.Evaluate(time));
                newSize.y = Mathf.Lerp(fromSize.y, toSize.y, animationCurve.Evaluate(time));

                rectTransform.sizeDelta = newSize;

                if (newSize.x <= toSize.x && newSize.y <= toSize.y)
                {
                    break;
                }

                time += Time.deltaTime * slideSpeed;
                yield return null;
            }
        }
    }
}