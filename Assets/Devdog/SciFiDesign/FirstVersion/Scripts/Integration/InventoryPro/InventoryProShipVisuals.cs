#if INVENTORY_PRO

using System;
using UnityEngine;
using System.Collections;
using Devdog.General;
using Devdog.InventoryPro;

namespace Devdog.SciFiDesign
{
    public class InventoryProShipVisuals : ShipVisuals
    {
        [Required]
        public StatDefinition shipEnergyStat;

        private IStat _stat;

        protected override void Start()
        {
            base.Start();

            _stat = PlayerManager.instance.currentPlayer.inventoryPlayer.stats.Get(shipEnergyStat);
        }

        protected override void DoBoost()
        {
            if (_stat.currentValue > 0f)
            {
                _stat.ChangeCurrentValueRaw(-20f * Time.deltaTime);
                base.DoBoost();
            }
        }

        protected override void RestoreBoost()
        {
            _stat.ChangeCurrentValueRaw(5f * Time.deltaTime);
            base.RestoreBoost();
        }
    }
}

#endif