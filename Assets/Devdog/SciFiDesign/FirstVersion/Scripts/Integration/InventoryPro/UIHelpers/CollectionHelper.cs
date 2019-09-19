#if INVENTORY_PRO

using UnityEngine;
using System.Collections;
using System.Linq;
using Devdog.InventoryPro;

namespace Devdog.SciFiDesign.UI
{
    public class CollectionHelper : MonoBehaviour
    {

        public void ClearCollectionAndMoveItemsToInventories(ItemCollectionBase collection)
        {
            if (collection.useReferences == false)
            {
                var items = collection.Select(o => o.item).Where(o => o != null).ToArray();
                collection.Clear();

                foreach (var item in items)
                {
                    InventoryManager.AddItem(item);
                }
            }
        }
    }
}

#endif