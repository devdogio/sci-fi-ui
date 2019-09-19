#if INVENTORY_PRO

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devdog.InventoryPro;
using UnityEngine.UI;

namespace Devodg.SciFiDesign.UI
{
    public class SciFiItemCollectionSlotUI : ItemCollectionSlotUI
    {
        public Image rarityImage;
        public Text sellPrice;

        public override void Repaint()
        {
            base.Repaint();

            if (item != null)
            {
                if (rarityImage != null)
                {
                    rarityImage.color = item.rarity.color;
                    SetActive(rarityImage, true);
                }
                if (sellPrice != null)
                {
                    sellPrice.text = item.sellPrice + " (" + item.sellPrice.ToString(item.currentStackSize) + ")";
                    SetActive(sellPrice, true);
                }
            }
            else
            {
                if (rarityImage != null)
                    SetActive(rarityImage, false);

                if (sellPrice != null)
                    SetActive(sellPrice, false);
            }
        }
    }
}

#endif