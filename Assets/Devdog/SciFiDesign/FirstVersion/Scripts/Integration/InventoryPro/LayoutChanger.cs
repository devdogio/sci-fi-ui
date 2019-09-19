#if INVENTORY_PRO

using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Devdog.General;
using Devdog.General.UI;
using Devdog.InventoryPro;
using Devdog.InventoryPro.UI;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Devdog.SciFiDesign.UI
{
    public class LayoutChanger : MonoBehaviour
    {
        private enum LayoutChoice
        {
            DynamicGrid,
            Vertical
        }

        [System.Serializable]
        public class CollectionLookup
        {
            public CollectionCopyToContainer container;
            public UIWindow window;
        }
        
        public CollectionLookup[] copyToContainers = new CollectionLookup[0];
        public CollectionLookup defaultCopyToContainer = new CollectionLookup();
        public static CollectionLookup currentLookup { get; set; }

        /// <summary>
        /// The inventory collection we want to copy around.
        /// </summary>
        [Required]
        public InventoryUI inventoryCollection;

        public ItemCollectionSlotUI gridLayoutElementPrefab;
        public ItemCollectionSlotUI verticalLayoutElementPrefab;

        public bool enableDragInListView = false;
        public bool enableDragInGridView = true;


        [SerializeField]
        private LayoutChoice defaultLayout = LayoutChoice.DynamicGrid;

        #region GridLayout settings

        [Header("DynamicGrid layout settings")]
        [SerializeField]
        protected RectOffset m_GridLayoutPadding = new RectOffset();
        [SerializeField]
        protected TextAnchor m_GridLayoutChildAlignment = TextAnchor.UpperLeft;
        [SerializeField]
        protected DynamicLayoutGroup.Corner m_GridLayoutStartCorner = DynamicLayoutGroup.Corner.UpperLeft;
        [SerializeField]
        protected GridLayoutGroup.Axis m_GridLayoutStartAxis = GridLayoutGroup.Axis.Horizontal;
        [SerializeField]
        protected Vector2 m_GridLayoutCellSize = new Vector2(100, 100);
        [SerializeField]
        protected Vector2 m_GridLayoutSpacing = Vector2.zero;
        [SerializeField]
        protected GridLayoutGroup.Constraint m_GridLayoutConstraint = GridLayoutGroup.Constraint.Flexible;
        [SerializeField]
        protected int m_GridLayoutConstraintCount = 2;

        #endregion

        #region VerticalLayout settings

        [Header("Vertical layout settings")]
        [SerializeField]
        protected RectOffset m_VerticalLayoutPadding = new RectOffset();
        [SerializeField]
        protected TextAnchor m_VerticalLayoutChildAlignment = TextAnchor.UpperLeft;
        [SerializeField]
        protected float m_VerticalLayoutSpacing = 0;
        [SerializeField]
        protected bool m_VerticalLayoutChildForceExpandWidth = true;
        [SerializeField]
        protected bool m_VerticalLayoutChildForceExpandHeight = true;

        #endregion

//        private RectTransform _rectTransform;
        private LayoutChoice _currentLayout;

        protected void Awake()
        {
            currentLookup = defaultCopyToContainer;
            foreach (var lookup in copyToContainers)
            {
                var l = lookup; // Temp
                lookup.window.OnShow += () =>
                {
                    WindowOnOnShow(l);
                };
            }

            switch (defaultLayout)
            {
                case LayoutChoice.DynamicGrid:
                    _ChangeToGridLayoutDoSettings();
                    break;
                case LayoutChoice.Vertical:
                    ChangeToVerticalLayout();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void WindowOnOnShow(CollectionLookup lookup)
        {
            foreach (Transform child in currentLookup.container.transform)
            {
                child.SetParent(lookup.container.transform);

                var rect = child.GetComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.sizeDelta = Vector2.zero;
                rect.anchoredPosition = Vector2.zero;
            }

            currentLookup = lookup;
        }

        private void DisableAllLayouts()
        {
            var gridLayout = inventoryCollection.container.GetComponent<DynamicLayoutGroup>();
            if (gridLayout != null)
                DestroyImmediate(gridLayout); // Destroy immediate, becuase otherwise we'll have to wait a frame...

            var verticalLayout = inventoryCollection.container.GetComponent<VerticalLayoutGroup>();
            if (verticalLayout != null)
                DestroyImmediate(verticalLayout); // Destroy immediate, becuase otherwise we'll have to wait a frame...
        }

        public void ChangeToGridLayout()
        {
            if (_currentLayout != LayoutChoice.DynamicGrid)
            {
                _ChangeToGridLayoutDoSettings();
                _ChangeToGridLayoutDoWrappers();
            }
        }

        private void _ChangeToGridLayoutDoSettings()
        {
            DisableAllLayouts();
            var grid = inventoryCollection.container.gameObject.AddComponent<DynamicLayoutGroup>();

            grid.padding = m_GridLayoutPadding;
            grid.childAlignment = m_GridLayoutChildAlignment;
            grid.startCorner = m_GridLayoutStartCorner;
            grid.cellSize = m_GridLayoutCellSize;
            grid.spacing = m_GridLayoutSpacing;
            grid.columnsCount = m_GridLayoutConstraintCount;

            inventoryCollection.canDragInCollection = enableDragInGridView;
            inventoryCollection.ignoreItemLayoutSizes = false;
            _currentLayout = LayoutChoice.DynamicGrid;
        }

        private void _ChangeToGridLayoutDoWrappers()
        {
            // Get all items, create new wrappers and fill the new wrappers.
            int collectionSize = inventoryCollection.items.Length;
            var items = inventoryCollection.items.Select(o => o.item).Where(o => o != null).ToArray();

            // Get rid of old wrappers
            foreach (var wrapper in inventoryCollection.items)
            {
                var c = wrapper as UnityEngine.Component;
                if (c != null)
                {
                    Destroy(c.gameObject);
                }
            }

            for (uint i = 0; i < collectionSize; i++)
            {
                CreateWapper(gridLayoutElementPrefab, i);
            }

            foreach (var item in items)
            {
                inventoryCollection.AddItem(item, null, true, false);
            }
            
            LayoutRebuilder.MarkLayoutForRebuild(inventoryCollection.container.GetComponent<RectTransform>());
        }

        private void CreateWapper(ItemCollectionSlotUI prefab, uint index)
        {
            var c = Instantiate<ItemCollectionSlotUI>(prefab);
            c.transform.SetParent(inventoryCollection.container);
            InventoryUtility.ResetTransform(c.transform);

            c.itemCollection = inventoryCollection;
            c.index = index;
            inventoryCollection.items[index] = c;

            c.Repaint();
        }

        public void ChangeToVerticalLayout()
        {
            if (_currentLayout != LayoutChoice.Vertical)
            {
                _ChangeToVerticalLayoutDoSettings();
                _ChangeToVerticalLayoutDoWrappers();
            }
        }

        private void _ChangeToVerticalLayoutDoSettings()
        {
            DisableAllLayouts();
            var vertical = inventoryCollection.container.gameObject.AddComponent<VerticalLayoutGroup>();

            vertical.padding = m_VerticalLayoutPadding;
            vertical.childAlignment = m_VerticalLayoutChildAlignment;
            vertical.spacing = m_VerticalLayoutSpacing;
            vertical.childForceExpandWidth = m_VerticalLayoutChildForceExpandWidth;
            vertical.childForceExpandHeight = m_VerticalLayoutChildForceExpandHeight;

            inventoryCollection.canDragInCollection = enableDragInListView;
            inventoryCollection.ignoreItemLayoutSizes = true;
            _currentLayout = LayoutChoice.Vertical;
        }

        private void _ChangeToVerticalLayoutDoWrappers()
        {
            // Get all items, create new wrappers and fill the new wrappers.
            int collectionSize = inventoryCollection.items.Length;
            var items = inventoryCollection.items.Select(o => o.item).Where(o => o != null).ToArray();

            // Get rid of old wrappers
            foreach (var wrapper in inventoryCollection.items)
            {
                var c = wrapper as UnityEngine.Component;
                if (c != null)
                {
                    Destroy(c.gameObject);
                }
            }

            for (uint i = 0; i < collectionSize; i++)
            {
                CreateWapper(verticalLayoutElementPrefab, i);
            }

            for (uint i = 0; i < items.Length; i++)
            {
                for (uint j = 0; j < collectionSize; j++)
                {
                    if (inventoryCollection.items[j].item == null)
                    {
                        bool set = inventoryCollection.SetItem(j, items[j], true);
                        if (set)
                        {
                            break;
                        }
                    }
                }
            }

            LayoutRebuilder.MarkLayoutForRebuild(inventoryCollection.container.GetComponent<RectTransform>());
        }
    }
}

#endif