#if INVENTORY_PRO

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devdog.InventoryPro;
using UnityEngine.UI;

namespace Devodg.SciFiDesign.UI
{
    public class SciFiItemCollectionSlotUIKeyTrigger : SciFiItemCollectionSlotUI
    {
        public Text keyCombinationText;
        public string keyCombination
        {
            get { return keyCombinationText != null ? keyCombinationText.text :  ""; }
            set
            {
                if (keyCombinationText != null)
                    keyCombinationText.text = value;
            }
        }

        public override void TriggerUse()
        {
            if (item == null)
                return;

            if (itemCollection.canUseFromCollection == false)
                return;

            if (item != null)
            {
                item.Use();
            }
            //            var found = InventoryManager.Find(item.ID, false);
            //            if (found != null)
            //            {
            //                int used = found.Use();
            //                if (used >= 0)
            //                    found.itemCollection[found.index].Repaint();
            //            }
        }
    }
}

#endif