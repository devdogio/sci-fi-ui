using System;
using UnityEngine;
using System.Collections;
using Devdog.General.UI;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    [RequireComponent(typeof(UIWindow))]
    public class UIWindowPageGroup : MonoBehaviour
    {
        
        public UIWindowPage[] pages = new UIWindowPage[0];
        public bool loop = false;

        public KeyCode[] nextKeyCodes = new KeyCode[0];
        public KeyCode[] prevKeyCodes = new KeyCode[0];


        private int _index;
        private UIWindow _window;
        public int index
        {
            get { return _index; }
            set { _index = GetIndex(value); }
        }

        private int GetIndex(int index)
        {
            if (index < 0)
                return pages.Length + index; // + because index is already negative + -index;

            return index % pages.Length;
        }

        protected void Awake()
        {
            _window = GetComponent<UIWindow>();
        }

        protected void Start()
        {
            for (int i = 0; i < pages.Length; i++)
            {
                // Prev button
                var prev = pages[i].gameObject.GetComponentsInChildren<PrevWindowUI>(true);
                if (prev.Length > 0 && prev[0].text != null)
                {
                    if (i == 0 && loop == false)
                    {
                        prev[0].gameObject.SetActive(false);
                    }
                    else
                    {
                        prev[0].text.text = pages[GetIndex(i - 1)].windowName;
                    }
                }

                // Next button
                var next = pages[i].gameObject.GetComponentsInChildren<NextWindowUI>(true);
                if (next.Length > 0 && next[0].text != null)
                {
                    if (i == pages.Length - 1 && loop == false)
                    {
                        next[0].gameObject.SetActive(false);
                    }
                    else
                    {
                        next[0].text.text = pages[GetIndex(i + 1)].windowName;
                    }
                }

                // Start page
                if (pages[i].isDefaultPage)
                {
                    index = i;
                }
            }
        }

        protected void Update()
        {
            if (_window.isVisible == false)
                return;

            foreach (var next in nextKeyCodes)
            {
                if (Input.GetKeyDown(next))
                {
                    NextPage();
                }
            }
            foreach (var prev in prevKeyCodes)
            {
                if (Input.GetKeyDown(prev))
                {
                    PrevPage();
                }
            }
        }


        public void NextPage()
        {
            if (index + 1 < pages.Length || loop)
            {
                index++;
                pages[index].Show();
            }
        }

        public void PrevPage()
        {
            if (index - 1 >= 0 || loop)
            {
                index--;
                pages[index].Show();
            }
        }
    }
}