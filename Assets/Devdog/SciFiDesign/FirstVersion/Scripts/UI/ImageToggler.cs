using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    public class ImageToggler : MonoBehaviour
    {
        public Sprite spriteA;
        public Sprite spriteB;

        public Color colorA;
        public Color colorB;

        private Image _image;
        protected void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Toggle()
        {
            if (_image.sprite == spriteA)
            {
                _image.sprite = spriteB;
                _image.color = colorB;
            }
            else
            {
                _image.sprite = spriteA;
                _image.color = colorA;
            }
        }
    }
}