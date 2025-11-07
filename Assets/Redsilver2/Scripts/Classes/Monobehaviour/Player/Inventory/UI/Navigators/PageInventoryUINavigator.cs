using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    [RequireComponent(typeof(InventoryUINavigatorTransitionsHandler))]
    public partial class PageInventoryUINavigator : VerticalInventoryUINavigator
    {
        [Space]
        [SerializeField][Range(1, 4)] private int maxHorizontalPageIndex, maxVerticalPageIndex;

        [Space]
        [SerializeField] private bool canWrapPageIndex = true;

        [Space]
        [SerializeField] private PageChangeDirection pageChangeDirection;

        private UnityEvent<int> onPageIndexChanged;

        private int pageIndex;
        private List<Item[,]> pages;

        public int PageIndex => pageIndex;
        public int MaxPages {
            get {
                return GetMaxPages();
            }  
        }

        protected sealed override void Awake()
        {
            base.Awake();

            pageIndex = 0;
            pages = new List<Item[,]>();
            onPageIndexChanged = new UnityEvent<int>();
        }

        public void AddOnPageIndexChangedListener(UnityAction<int> action)
        {
            if (onPageIndexChanged != null && action != null)
                onPageIndexChanged.AddListener(action);
        }

        public void RemoveOnPageIndexChangedListener(UnityAction<int> action)
        {
            if (onPageIndexChanged != null && action != null)
                onPageIndexChanged.RemoveListener(action);
        }

        protected override void OnItemAdded(Item item) 
        {
            if(pages != null && item != null) { 
                AddPageItem(item);
               // DebugPages();
            }

            base.OnItemAdded(item);
        }

        protected override void OnItemRemoved(Item item) 
        {
            if (item != null) {
                RemovePageItem(item);
            }

            base.OnItemAdded(item);
        }

        public sealed override void Select(Item item)
        {
            int pageIndex       = GetPageIndex(item);
            int verticalIndex   = GetVerticalIndex(item);
            int horizontalIndex = GetHorizontalIndex(item);

            if (pageIndex >= 0 && verticalIndex >= 0 && horizontalIndex >= 0) 
            {
                this.pageIndex       = pageIndex;
                this.verticalIndex   = verticalIndex;
                this.horizontalIndex = horizontalIndex;
            }   
        }

        protected sealed override void ClampDecrementHorizontalIndex(ref int horizontalIndex, bool canWarpHorizontalIndex)
        {
            if (horizontalIndex < 0){
                if(pageChangeDirection == PageChangeDirection.Horizontal) DecrementPageIndex();
                base.ClampDecrementHorizontalIndex(ref horizontalIndex, canWarpHorizontalIndex);
            }
        }

        protected sealed override void ClampIncrementHorizontalIndex(ref int horizontalIndex, bool canWarpHorizontalIndex)
        {
            if (horizontalIndex >= GetMaxHorizontalIndex(pageIndex)) {
                if(pageChangeDirection == PageChangeDirection.Horizontal) IncrementPageIndex();
                base.ClampIncrementHorizontalIndex(ref horizontalIndex, canWarpHorizontalIndex);;
            }
        }

        protected sealed override void ClampIncrementVerticalIndex(ref int verticalIndex, bool canWarpVerticalIndex)
        {
            if (verticalIndex >= GetMaxVerticalIndex(pageIndex))
            {
                if (pageChangeDirection == PageChangeDirection.Vertical) IncrementPageIndex();
                base.ClampIncrementVerticalIndex(ref verticalIndex, canWarpVerticalIndex);
            }
        }

        protected sealed override void ClampDecrementVerticalIndex(ref int verticalIndex, bool canWarpVerticalIndex)
        {
            if (verticalIndex < 0)
            {
                if (pageChangeDirection == PageChangeDirection.Vertical) DecrementPageIndex();
                base.ClampDecrementVerticalIndex(ref verticalIndex, canWarpVerticalIndex);
            }
        }

        private void UpdateItemsAndModels()
        {
            UpdateItems();
            if (transitionsHandler == null) UpdateModels();
        }

        private void DecrementPageIndex()
        {
            int maxPageIndex = GetMaxPages();

            if (!canWrapPageIndex && pageIndex >= maxPageIndex - 1)
                return;

            pageIndex--;

            if (pageIndex < 0) {
                if (canWrapPageIndex) pageIndex = maxPageIndex - 1;
                else pageIndex = 0;
            }


            if (maxPageIndex > 1)
                onPageIndexChanged.Invoke(pageIndex);
        }

        private void IncrementPageIndex()
        {
            if (!canWrapPageIndex && pageIndex <= 0)
                return;

            int maxPageIndex = GetMaxPages();
            pageIndex++;

            if (pageIndex >= maxPageIndex)
            {
                if (canWrapPageIndex) pageIndex = 0;
                else pageIndex = maxPageIndex - 1;
            }

            if (maxPageIndex > 1) 
                onPageIndexChanged.Invoke(pageIndex);
        }

        private void AddPage()
        {
            CleanPages();

            if (pages != null) {
                pages.Add(new Item[maxVerticalPageIndex, maxHorizontalPageIndex]);
            }
        }

        private void AddPage(Item item)
        {
            AddPage();

            if (item != null) {
                pages[pages.Count - 1][0, 0] = item;
            }
        }



        private void RemovePages()
        {
            CleanPages();
            if (pages == null) return;

            for (int i = 0; i < pages.Count; i++) 
               if(CanRemovePage(i)) pages.RemoveAt(i);
        }

        private void AddPageItem(Item item)
        {
            CleanPages();
            Debug.Log("Max Pages: " + GetMaxPages());
            if (pages == null || pages.Count == 0) { AddPage(); }
            if (item == null) return;

            for (int i = 0; i < pages.Count; i++) {
                AddPageItem(item, pages[i], i, out bool wasItemAdded);
                if (wasItemAdded) return;
            }

            AddPage(item);
        }

        private void AddPageItem(Item item, Item[,] items, int pageIndex, out bool wasItemAdded)
        {
            wasItemAdded = false;
            if (items == null || items.GetLength(0) == 0 || items.GetLength(1) == 0) return;
            if (pages == null || item == null            || pages.Count        == 0) return;

            for (int i = 0; i < items.GetLength(0); i++) {
                for (int j = 0; j < items.GetLength(1); j++) {
                    AddPageItem(item, pageIndex, i, j, out wasItemAdded);
                    if(wasItemAdded) return;    
                }
            }
        }

        private void AddPageItem(Item item, int pageIndex, int verticalIndex, int horizontalIndex, out bool wasItemAdded)
        {
            wasItemAdded = false;
            if (pages == null || item == null || pages.Count == 0) return;

            if (pages[pageIndex][verticalIndex, horizontalIndex] == null) {
                pages[pageIndex][verticalIndex, horizontalIndex] = item;
                wasItemAdded = true;
            }
        }

        private void RemovePageItem(Item item)
        {
            int pageIndex       = GetPageIndex(item);
            int verticalIndex   = GetVerticalIndex(item);
            int horizontalIndex = GetHorizontalIndex(item);

            CleanPages();
            if (pages == null || pages.Count == 0) return;

            if (pageIndex >= 0 && verticalIndex >= 0 && horizontalIndex >= 0) {
                pages[pageIndex][verticalIndex, horizontalIndex] = null;
                RemovePages();
            }  
        }
        private void RemovePageItem(Item item, Item[] currents, out int index, out bool wasItemRemoved)
        {
            wasItemRemoved = false;
            index = 0;

            if (currents == null || item == null) return;

            for (int i = 0; i < currents.Length; i++)
            {
                Item current = currents[i];
                if (current == null || current != item) continue;

                index = i;
                wasItemRemoved = true;
                break;
            }
        }

        private void CleanPages() {
           if(pages != null) pages = pages.Where(x => x != null).ToList();
        }

        public void DebugPages()
        {
            if (pages == null || pages.Count == 0) { Debug.LogWarning("No Pages To Debug."); return; }

            for(int i = 0;i < pages.Count; i++) {
                DebugPages(pages[i], i);
            }
        }

        private void DebugPages(Item[,] items, int pageIndex)
        {
            if(items == null || items.GetLength(0) == 0 || items.GetLength(1) == 0) { Debug.LogWarning($"Page Index {pageIndex + 1} \nEmpty Page"); return; }

            string result = $"Page {pageIndex + 1} \n";

            for (int i = 0; i < items.GetLength(0) ; i++) {
                result += $"Row {i + 1} | ";
                for (int j = 0; j < items.GetLength(1) ; j++) result += (items[i,j] != null ? "Item: " + items[i,j].name : "Item: null") + " | ";
            }

            Debug.Log(result);
        }

        private bool CanRemovePage(int pageIndex)
        {  
            if(pages == null || pages.Count == 0) return false;
            if(pages.Count > 1) return CanRemovePage(GetItems(pageIndex));
            return false;
        }
        private bool CanRemovePage(Item[,] items)
        {
            int count = 0;
            if (items == null) return true;

            for (int i = 0; i < items.GetLength(0); i++)
                for (int j = 0; j < items.GetLength(1); j++)
                    if (items[i, j] == null) count++;

            return count == items.GetLength(0) * items.GetLength(1);
        }

        private int GetMaxPages()
        {
            if (pages == null) return -1;
            return pages.Count;
        }

        public int GetPageIndex(Item item)
        {
            if(pages == null || item == null || pages.Count == 0) return -1;

            for(int i = 0; i < pages.Count; i++) {
                int verticalIndex = GetItemIndex(item, pages[i], i, true);
                if (verticalIndex >= 0) return i;
            }

            return -1;
        }

        public sealed override int GetVerticalIndex(Item item) 
        {         
            if(pages == null || item == null || pages.Count == 0) return -1;
            
            for(int i = 0;i < pages.Count; i++) {
                int verticalIndex = GetItemIndex(item, pages[i], i, true);
                if (verticalIndex >= 0) return verticalIndex;
            }

            return -1;
        }

        public sealed override int GetHorizontalIndex(Item item)
        {
            if (pages == null || item == null || pages.Count == 0) return -1;

            for (int i = 0; i < pages.Count; i++) {
                int horizontalIndex = GetItemIndex(item, pages[i], i, false);
                if(horizontalIndex >= 0) return horizontalIndex;
            }

            return -1;
        }

        private int GetItemIndex(Item item, Item[,] items, int pageIndex, bool getVerticalIndex)
        {
            if (pages == null || pages.Count == 0) return -1;
            if (item  == null || items == null || items.GetLength(0) == 0 || items.GetLength(1) == 0) return -1;

            for (int i = 0; i < items.GetLength(0); i++) 
                for (int j = 0; j < items.GetLength(1); j++)
                    if (pages[pageIndex][i, j] == item) return getVerticalIndex ? i : j;

            return -1;
        }


        public sealed override int GetMaxHorizontalIndex() {
            return GetMaxHorizontalIndex(pageIndex);
        }

        public int GetMaxHorizontalIndex(int pageIndex)
        {
            Item[,] items = GetItems(pageIndex);
            if (items == null) return -1;
            return items.GetLength(1);
        }


        public sealed override int GetMaxVerticalIndex()
        {
            return GetMaxVerticalIndex(pageIndex);
        }
        public int GetMaxVerticalIndex(int pageIndex)
        {
            Item[,] items = GetItems(pageIndex);

            if (items == null) return -1;
            return items.GetLength(0);
        }

        public sealed override Item[,] GetItems() {
            return GetItems(pageIndex);
        }

        public Item[,] GetItems(int pageIndex)
        {
            if (pages == null || pages.Count == 0 || pageIndex < 0 || pageIndex >= pages.Count)
                return null;

            return pages[pageIndex];
        }

        public Item[] GetItems(int pageIndex, int verticalIndex)
        {
            Item[,] items = GetItems(pageIndex);
           
            if (items == null || items.GetLength(0) == 0 || verticalIndex  < 0 || verticalIndex >= items.GetLength(0))
                return null;

            List<Item> results = new List<Item>();

            for(int i = 0; i < items.GetLength(1); i++)
                results.Add(items[verticalIndex, i]);

            return results.ToArray();
        }

        public Item GetItem(int pageIndex, int horizontalIndex, int verticalIndex)
        {
            Item[] items = GetItems(pageIndex, verticalIndex);
            if(items == null || items.Length == 0 || verticalIndex < 0 || verticalIndex >= items.Length) return null;
            return items[horizontalIndex];
        }

        public override Item GetSelectedItem()
        {
            if(pages == null || pages.Count == 0) return null;
            if(pageIndex < 0 || verticalIndex < 0 || horizontalIndex < 0) return null;

            Item[,] items = GetItems(pageIndex);
            if(items == null || verticalIndex >= items.GetLength(0) || horizontalIndex >= items.GetLength(1)) return null;

            return items[verticalIndex, horizontalIndex];
        }
    }
}
