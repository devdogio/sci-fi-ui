#if INVENTORY_PRO

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Devodg.SciFiDesign.UI
{
    public class SciFiItemCollectionSlotUILoot : SciFiItemCollectionSlotUI
    {
        public bool hideWhenEmpty;


        #region Button handler UI events

        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Start()
        {
            cooldownVisualizer.HideAll();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            PickupItem();
        }

        protected virtual void PickupItem()
        {
            Selectable below = null;
            Selectable above = null;

            // select element below or above 
            var btn = gameObject.GetComponentInChildren<Button>();
            if (btn != null)
            {
                below = btn.FindSelectableOnDown();
                above = btn.FindSelectableOnUp();
            }

            bool added = item.PickupItem();
            if (added)
            {
                var i = item;
                itemCollection.SetItem(index, null); // Remove from loot collection
                itemCollection.NotifyItemRemoved(i, i.ID, index, i.currentStackSize);

                if (below != null)
                    below.Select();
                else if (above != null)
                    above.Select();

                Repaint();
            }
        }

        #endregion

        public override void Repaint()
        {
            gameObject.SetActive(true);
            if (item == null)
            {
                if (hideWhenEmpty)
                {
                    gameObject.SetActive(false);
                }
            }

            base.Repaint();
        }

        public override void RepaintCooldown()
        {
            //base.RepaintCooldown();
        }
    }
}

#endif