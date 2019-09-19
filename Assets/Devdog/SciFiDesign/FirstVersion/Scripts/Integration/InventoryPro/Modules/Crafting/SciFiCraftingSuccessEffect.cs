#if INVENTORY_PRO

using System;
using UnityEngine;
using System.Collections;
using Devdog.General;
using Devdog.InventoryPro;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    [RequireComponent(typeof(Animator))]
    public class SciFiCraftingSuccessEffect : MonoBehaviour
    {
        [Required]
        public CraftingWindowBase craftingWindow;

        [Required]
        public AnimationClip animationClip;



        protected virtual void Start()
        {
            craftingWindow.OnCraftSuccess += OnCraftSuccess;
        }
        

        private void OnCraftSuccess(CraftingProgressContainer.CraftInfo craftinfo)
        {
            GetComponent<Animator>().Play(animationClip.name);
        }
    }
}

#endif