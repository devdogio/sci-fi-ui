#if INVENTORY_PRO

using UnityEngine;
using System.Collections;
using Devdog.InventoryPro;
using UnityEngine.EventSystems;

namespace Devdog.SciFiDesign.UI
{
    [RequireComponent(typeof(ItemCollectionSlotUI))]
    public class SelectItemOnUIHover : MonoBehaviour, IPointerEnterHandler
    {
        public static InventoryItemBase selectedItem;
        private ItemCollectionSlotUIBase _wrapper;

        protected virtual void Awake()
        {
            _wrapper = GetComponent<ItemCollectionSlotUIBase>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var info = GetComponentInParent<LoadItemInfo>();
            if (info == null)
            {
                Debug.LogWarning("Load item info component not found in parent!", gameObject);
                return;
            }

            info.SetItem(_wrapper);
        }
    }
}

#endif