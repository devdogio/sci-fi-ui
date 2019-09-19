using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    public class PointerUIHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
//        public UnityEvent onHover = new UnityEvent();
        public UnityEvent onPointerEnter = new UnityEvent();
        public UnityEvent onPointerExit = new UnityEvent();
        public UnityEvent onPointerDown = new UnityEvent();
        public UnityEvent onPointerUp = new UnityEvent();
        public UnityEvent onPointerClick = new UnityEvent();

        public void OnPointerEnter(PointerEventData eventData)
        {
            var s = GetComponent<Selectable>();
            if ((s != null && s.IsInteractable()) || s == null)
                onPointerEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            var s = GetComponent<Selectable>();
            if ((s != null && s.IsInteractable()) || s == null)
                onPointerExit.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var s = GetComponent<Selectable>();
            if ((s != null && s.IsInteractable()) || s == null)
                onPointerDown.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var s = GetComponent<Selectable>();
            if ((s != null && s.IsInteractable()) || s == null)
                onPointerUp.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var s = GetComponent<Selectable>();
            if ((s != null && s.IsInteractable()) || s == null)
                onPointerClick.Invoke();
        }
    }
}