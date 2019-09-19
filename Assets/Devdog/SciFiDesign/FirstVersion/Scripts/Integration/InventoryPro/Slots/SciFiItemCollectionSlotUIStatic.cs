#if INVENTORY_PRO

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devdog.InventoryPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Devodg.SciFiDesign.UI
{
    public class SciFiItemCollectionSlotUIStatic : SciFiItemCollectionSlotUI
    {

        protected override void Awake()
        {
            //            base.Awake();
        }

        protected virtual void Start()
        {
            cooldownVisualizer.HideAll();
        }

        protected override void OnDisable()
        {

        }

        #region Button handler UI events

        public override void OnPointerUp(PointerEventData eventData)
        {

        }

        public override void OnPointerDown(PointerEventData eventData)
        {

        }

        public override bool OnTap(PointerEventData eventData, InventoryActionInput.EventType eventType)
        {
            return false;
        }

        public override bool OnDoubleTap(PointerEventData eventData, InventoryActionInput.EventType eventType)
        {
            return false;
        }

        public override bool OnLongTap(PointerEventData eventData, InventoryActionInput.EventType eventType)
        {
            return false;
        }

        public virtual void PickupItem()
        {

        }

        public override void OnBeginDrag(PointerEventData eventData)
        {

        }

        public override void OnDrag(PointerEventData eventData)
        {

        }

        public override void OnEndDrag(PointerEventData eventData)
        {

        }

        #endregion

        public override void TriggerContextMenu()
        {
//            base.TriggerContextMenu();
        }

        public override void TriggerDrop(bool useRaycast = true)
        {
//            base.TriggerDrop(useRaycast);
        }

        public override void TriggerUnstack(ItemCollectionBase toCollection, int toIndex = -1)
        {
//            base.TriggerUnstack(toCollection, toIndex);
        }

        public override void TriggerUse()
        {
//            base.TriggerUse();
        }


        public override void RepaintCooldown()
        {

//            base.RepaintCooldown();
        }
    }
}

#endif