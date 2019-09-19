using UnityEngine;
using System.Collections;
using Devdog.General.UI;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Devdog.SciFiDesign.UI
{
    public class UIWindowHelper : MonoBehaviour
    {
        public UnityEvent onParentWindowShow;
        public UnityEvent onParentWindowHide;



        protected void Start()
        {
            var uiWindow = GetComponentInParent<UIWindow>();
            Assert.IsNotNull(uiWindow, "No UIWindow found in parent!");

            uiWindow.OnShow += () =>
            {
                onParentWindowShow.Invoke();
            };
            uiWindow.OnHide += () =>
            {
                onParentWindowHide.Invoke();
            };
        }
    }
}