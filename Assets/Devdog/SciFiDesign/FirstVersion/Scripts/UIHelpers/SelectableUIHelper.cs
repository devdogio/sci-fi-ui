using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    public class SelectableUIHelper : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public UnityEvent onSelect;
        public UnityEvent onDeselect;

        public void OnSelect(BaseEventData eventData)
        {
            onSelect.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            onDeselect.Invoke();
        }
    }
}