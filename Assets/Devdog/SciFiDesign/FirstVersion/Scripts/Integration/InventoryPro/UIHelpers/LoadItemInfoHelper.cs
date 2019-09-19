#if INVENTORY_PRO

using UnityEngine;
using System.Collections;
using Devdog.InventoryPro;

namespace Devdog.SciFiDesign.UI
{
    public class LoadItemInfoHelper : MonoBehaviour
    {


        public void LoadItemFromCurrentWrapper()
        {
            var info = GetComponentInParent<LoadItemInfo>();
            if (info == null)
            {
//                Debug.LogWarning("Load item info component not found in parent!", gameObject);
                return;
            }

            info.SetItem(GetComponent<ItemCollectionSlotUIBase>());
        }
    }
}

#endif