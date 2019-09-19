#if INVENTORY_PRO

using System;
using UnityEngine;
using System.Collections;
using Devdog.General;
using Devdog.InventoryPro;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    public class LoadItemInfo : MonoBehaviour
    {
        [System.Serializable]
        public class UIWindowActionEvent : UnityEngine.Events.UnityEvent
        {

        }

        public Transform itemPreviewParent;

        [Required]
        public RectTransform itemDetailsRoot;
        public Camera itemPreviewCamera;
        public int itemPreviewLayer = 17;
        public float cameraDistanceToScaleRatio = 1f;

        public bool hideWhenValueZeroOrEmpty = true;

//        public AnimationClip onSelectAnimation;
//        public AnimationClip onUnSelectAnimation;
        public AudioClipInfo onSelectAudioClip;


        [Header("UI References")]
        public Text itemName;
        public Text itemDescription;
        public Text itemAmount;
        public Text itemRarity;
        public bool useItemRarityColor = true;
        public Text itemCategory;
        public Text itemCooldownTime;
        public Text itemWeight;
        public Text itemRequiredLevel;

        public Image itemIcon;
        [Required]
        public RectTransform propertyContainer;
        [Required]
        public StatRowUI propertyRowPrefab;
        public Color proprertyPositiveColor = Color.green;
        public Color propertyNegativeColor = Color.red;

        [Required]
        public RectTransform usageRequirementPropertiesContainer;
        [Required]
        public UsageRequirementRowUI usageRequirementPropertyRowPrefab;
        public Color usageRequirementPositiveColor = Color.green;
        public Color usageRequirementNegativeColor = Color.red;


        public Text buyPrice;
        public Text sellPrice;

        public Text isDroppable;
        public Text isSellable;
        public Text isStorable;

        public UIWindowActionEvent onShowActions = new UIWindowActionEvent();
        public UIWindowActionEvent onHideActions = new UIWindowActionEvent();

        public static InventoryItemBase selectedItem { get; private set; }


//        private static LoadItemInfo _instance;
//        public static LoadItemInfo instance
//        {
//            get
//            {
//                if (_instance == null)
//                    _instance = FindObjectOfType<LoadItemInfo>();
//
//                return _instance;
//            }
//            private set
//            {
//                _instance = value;
//                
//            }
//        }

        private void Awake()
        {
//            instance = this;
            itemDetailsRoot.gameObject.SetActive(false);
        }


        public void SetItem(ItemCollectionSlotUIBase wrapper)
        {
            SetItem(wrapper.item);
        }

        public void SetItem(InventoryItemBase item)
        {
            if (item != null)
            {
                if (selectedItem != null)
                {
                    // Do out animation on previous
                    onHideActions.Invoke();
                    RemovePreviewItem(selectedItem);
                }

                selectedItem = Instantiate<InventoryItemBase>(item);
                selectedItem.currentStackSize = item.currentStackSize;


                AudioManager.AudioPlayOneShot(onSelectAudioClip);


                itemDetailsRoot.gameObject.SetActive(selectedItem != null);

                onShowActions.Invoke();
                SetPreviewObject(selectedItem);
                Repaint(selectedItem);
            }
        }

        private void RemovePreviewItem(InventoryItemBase item)
        {
            Destroy(item.gameObject);
        }

        private void SetPreviewObject(InventoryItemBase item)
        {
            item.gameObject.SetActive(true);
            item.transform.SetParent(itemPreviewParent);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localScale = Vector3.one;
            InventoryUtility.SetLayerRecursive(item.gameObject, itemPreviewLayer); // TODO: Move itemPreviewLayer to settings

            var rigids = item.gameObject.GetComponentsInChildren<Rigidbody>();
            foreach (var rigid in rigids)
            {
                rigid.isKinematic = true;
            }

            var renderer = item.gameObject.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                var totalSize = (renderer.bounds.size.x + renderer.bounds.size.y + renderer.bounds.size.z);

                if (itemPreviewCamera.orthographic)
                {
                    itemPreviewCamera.orthographicSize = totalSize * cameraDistanceToScaleRatio;
                }
                else
                {
                    itemPreviewCamera.transform.position = new Vector3(0, 0, -totalSize * cameraDistanceToScaleRatio);
                }
            }
        }

        public virtual void Repaint(InventoryItemBase item)
        {
            SetText(itemName, item.name, true);
            SetText(itemDescription, item.description, true);
            SetText(itemAmount, item.currentStackSize + "X", true);
            SetText(itemRarity, item.rarity.name, true);
            if (useItemRarityColor && itemRarity != null)
            {
                itemRarity.color = item.rarity.color;
            }

            SetText(itemCategory, item.category.name);
            SetText(itemCooldownTime, item.cooldownTime.ToString(), true);

            DestroyChildren(propertyContainer);
            foreach (var stat in item.stats)
            {
                var row = Instantiate<StatRowUI>(propertyRowPrefab);
                row.transform.SetParent(propertyContainer);
                InventoryUtility.ResetTransform(row.transform);
                row.text.text = string.Format("{0} {1}", stat.value, stat.stat.name);

                if (stat.floatValue < 0f)
                {
                    // - sign is added auto.
                    row.text.text = row.text.text.Remove(0, 1);
                    row.text.text = "- " + row.text.text;
                    row.text.color = propertyNegativeColor;
                }
                else if (stat.floatValue > 0f)
                {
                    if (stat.actionEffect == StatDecorator.ActionEffect.Decrease)
                    {
                        row.text.text = "- " + row.text.text;
                        row.text.color = propertyNegativeColor;
                    }
                    else
                    {
                        row.text.text = "+ " + row.text.text;
                        row.text.color = proprertyPositiveColor;
                    }
                }
            }


            DestroyChildren(usageRequirementPropertiesContainer);
            foreach (var stat in item.usageRequirement)
            {
                var row = Instantiate<UsageRequirementRowUI>(usageRequirementPropertyRowPrefab);
                row.transform.SetParent(usageRequirementPropertiesContainer);
                InventoryUtility.ResetTransform(row.transform);
                row.text.text = string.Format("{0} {1}", stat.value, stat.stat.name);
                
                if (stat.CanUse(PlayerManager.instance.currentPlayer.inventoryPlayer))
                {
                    row.text.color = usageRequirementPositiveColor;
                }
                else
                {
                    row.text.color = usageRequirementNegativeColor;
                }
            }

            SetText(itemWeight, item.weight.ToString(), true);
            SetText(itemRequiredLevel, item.requiredLevel.ToString(), true);
            if (itemIcon != null)
            {
                itemIcon.sprite = item.icon;
            }
            
            if(isDroppable != null)
            {
                isDroppable.gameObject.SetActive(item.isDroppable == false);
            }

            if(isSellable != null)
            {
                isSellable.gameObject.SetActive(item.isSellable == false);
            }

            if(isStorable != null)
            {
                isStorable.gameObject.SetActive(item.isStorable == false);
            }

            SetText(buyPrice, item.buyPrice.ToString());
            SetText(sellPrice, item.sellPrice.ToString());
        }

        private void DestroyChildren(Transform transform)
        {
            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }
        }

        private void SetText(Text text, string a, bool forceShow = false)
        {
            if (text != null)
            {
                text.transform.parent.gameObject.SetActive(true);
                text.text = a;

                if (hideWhenValueZeroOrEmpty && forceShow == false)
                {
                    if (string.IsNullOrEmpty(a) || a == "0")
                    {
                        text.transform.parent.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void SetText(Text text, string a, Color col, bool forceShow = false)
        {
            SetText(text, a, forceShow);
            if (text != null)
            {
                text.color = col;
            }
        }
    }
}

#endif