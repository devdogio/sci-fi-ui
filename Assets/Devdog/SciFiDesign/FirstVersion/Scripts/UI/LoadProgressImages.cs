using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    public class LoadProgressImages : MonoBehaviour
    {
        public Color inactiveColor = Color.gray;
        public Color activeColor = Color.white;
        public Image[] images = new Image[0];

        protected virtual void Start()
        {
            Reset();
        }
        
        protected virtual void Reset()
        {
            StartCoroutine(_Reset());
        }

        private IEnumerator _Reset()
        {
            yield return new WaitForSeconds(0.5f);

            float timer = 0f;
            while (timer < 0.5f)
            {
                timer += Time.deltaTime;

                foreach (var image in images)
                {
                    image.color = Color.Lerp(activeColor, inactiveColor, timer * 2f);
                }

                yield return null;
            }
        }

        protected virtual void OnProgress(float progress)
        {
            int index = Mathf.CeilToInt(progress * images.Length);

            for (int i = 0; i < index; i++)
            {
                images[i].color = activeColor;
            }
        }
    }
}