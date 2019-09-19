using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Devdog.SciFiDesign.UI
{
    [RequireComponent(typeof(Button))]
    public class OnSelectTriggerButtonUse : MonoBehaviour, ISelectHandler
    {

        public void OnSelect(BaseEventData eventData)
        {
            GetComponent<Button>().OnPointerClick(new PointerEventData(EventSystem.current));
        }

    }
}