#if INVENTORY_PRO

using System;
using UnityEngine;
using System.Collections;
using Devdog.General;
using Devdog.InventoryPro;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    public class SciFiCraftingProgressUI : MonoBehaviour
    {

        [Required]
        public CraftingWindowBase craftingWindow;


        public Color inactiveColor = Color.gray;
        public Color activeColor = Color.white;
        public Image[] images = new Image[0];


        protected virtual void Start()
        {
            craftingWindow.OnCraftProgress += CraftingWindowOnCraftProgress;
            craftingWindow.OnCraftSuccess += CraftEnded;
            craftingWindow.OnCraftFailed += CraftEnded;
            craftingWindow.OnCraftCancelled += CraftEndedProgress;

            Reset();
        }

        private void CraftEndedProgress(CraftingProgressContainer.CraftInfo craftinfo, float progress)
        {
            Reset();
        }

        private void CraftEnded(CraftingProgressContainer.CraftInfo craftinfo)
        {
            Reset();
        }

        private void Reset()
        {
            StartCoroutine(_Reset());
        }

        private IEnumerator _Reset()
        {
            yield return new WaitForSeconds(0.5f);

            float _timer = 0f;
            while (_timer < 0.5f)
            {
                _timer += Time.deltaTime;

                foreach (var image in images)
                {
                    image.color = Color.Lerp(activeColor, inactiveColor, _timer * 2f);
                }

                yield return null;
            }
        }

        protected virtual void CraftingWindowOnCraftProgress(CraftingProgressContainer.CraftInfo craftInfo, float progress)
        {
            int index = Mathf.CeilToInt(progress * images.Length);

            for (int i = 0; i < index; i++)
            {
                images[i].color = activeColor;
            }
        }
    }
}

#endif