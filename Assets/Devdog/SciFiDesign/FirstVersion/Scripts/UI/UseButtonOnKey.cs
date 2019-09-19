using UnityEngine;
using System.Collections;
using Devdog.General.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    public class UseButtonOnKey : MonoBehaviour
    {
        public KeyCode useKeyCode = KeyCode.Return;


        private UIWindow _window;

        protected virtual void Awake()
        {
            _window = GetComponent<UIWindow>();
        }

        protected void Update()
        {
            if (isActiveAndEnabled == false)
            {
                return;
            }

            if (_window != null)
            {
                if (_window.isVisible == false)
                {
                    return;
                }
            }

            if (Input.GetKeyDown(useKeyCode))
            {
                var obj = UIUtility.currentlySelectedGameObject;
                if (obj != null)
                {
                    var button = obj.GetComponent<Button>();
                    if (button != null)
                    {
                        button.OnPointerClick(new PointerEventData(EventSystem.current));
                    }
                }
            }
        }
    }
}