#if INVENTORY_PRO

using UnityEngine;
using System.Collections;
using Devdog.InventoryPro;
using Devdog.InventoryPro.UI;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    public class SciFiInventoryCraftingBlueprintUI : InventoryCraftingBlueprintUI
    {

        public Image rarityImage;


        public override void Repaint(CraftingBlueprint blueprint)
        {
            base.Repaint(blueprint);

            if (rarityImage != null && blueprint.resultItems.Length > 0)
                rarityImage.color = blueprint.resultItems[0].item.rarity.color;

        }
    }
}

#endif