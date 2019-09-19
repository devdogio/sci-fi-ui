using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    public class ToggleUIHelper : MonoBehaviour
    {
        public UnityEvent onCheck;
        public UnityEvent onUnCheck;


        private Toggle _toggle;



        protected void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(ToggleChanged);
        }

        private void ToggleChanged(bool isChecked)
        {
            if(isChecked)
                onCheck.Invoke();
            else
                onUnCheck.Invoke();
        }
    }
}