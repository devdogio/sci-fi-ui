#if INVENTORY_PRO

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Devdog.General;
using Devdog.InventoryPro;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class RadarGraph : MonoBehaviour
    {
        public PrimitiveShape[] background = new PrimitiveShape[0];

        [Required]
        public PrimitiveShapeRadar radar;
        public Color radarEdgeColor = Color.white;

        protected void Start()
        {
            PlayerManager.instance.OnPlayerChanged += InstanceOnOnPlayerChanged;
            InstanceOnOnPlayerChanged(null, PlayerManager.instance.currentPlayer);
        }

        private void InstanceOnOnPlayerChanged(Player oldPlayer, Player newPlayer)
        {
            if (oldPlayer != null)
            {
                oldPlayer.inventoryPlayer.stats.OnStatValueChanged -= StatOnValueChanged;
            }

            var l = new List<PrimitiveShapeRadar.Element>();
            foreach (var cat in newPlayer.inventoryPlayer.stats)
            {
                foreach (var stat in cat.Value)
                {
                    if (stat.definition.showInUI == false)
                    {
                        continue;
                    }

                    l.Add(new PrimitiveShapeRadar.Element()
                    {
                        name = stat.definition.statName,
                        aimValue = stat.currentValue / stat.definition.maxValue,
                        vertexColor = stat.definition.color
                    });
                }
            }

            radar.elements = l.ToArray();
            radar.AnimateInValues();
            newPlayer.inventoryPlayer.stats.OnStatValueChanged += StatOnValueChanged;
        }

        private void StatOnValueChanged(IStat stat)
        {
            var s = radar.elements.FirstOrDefault(o => o.name == stat.definition.statName);
            if (s != null)
            {
                s.aimValue = stat.currentValue / stat.definition.maxValue;
            }

            radar.AnimateInValues();
        }
    }
}

#endif