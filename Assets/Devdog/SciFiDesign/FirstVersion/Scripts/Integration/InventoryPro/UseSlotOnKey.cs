#if INVENTORY_PRO

using UnityEngine;
using System.Collections;
using Devdog.General.UI;
using Devdog.InventoryPro.UI;

namespace Devdog.SciFiDesign.UI
{
    public class UseSlotOnKey : MonoBehaviour
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
                var wrapper = InventoryUIUtility.currentlySelectedSlot;
                if (wrapper != null)
                {
                    wrapper.TriggerUse();
                    wrapper.Repaint();
                }
            }
        }
    }
}

#endif