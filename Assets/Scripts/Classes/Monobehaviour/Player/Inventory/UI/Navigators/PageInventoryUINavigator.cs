using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public partial class PageInventoryUINavigator : VerticalInventoryUINavigator
    {
        [Space]
        [SerializeField][Range(1, 4)] private int maxHorizontalPageIndex, maxVerticalPageIndex;

        [Space]
        [SerializeField] private PageChangeDirection pageChangeDirection;
        [SerializeField] private bool canWrapPageIndex;

        private int pageIndex;
        private List<Item[,]> pages;

        public int PageIndex => pageIndex;

        protected sealed override void Awake()
        {
            base.Awake();

            pageIndex = 0;
            pages     = new List<Item[,]>();

            if (inventory != null) {
                inventory.AddOnItemAddedListener(OnItemAdded);
                inventory.AddOnItemRemovedListener(OnItemRemoved);
            }

            AddPage();
        }

        protected virtual void OnItemAdded(Item item) {
            Debug.Log("What??");

            if(pages != null && item != null) { 
               AddPageItem(item);
                DebugPages();
            }
        }

        protected virtual void OnItemRemoved(Item item) {
            if (item != null) {
                RemovePageItem(item);
            }
        }

        public sealed override void SetIndex(Item item)
        {
            int pageIndex       = GetItemPageIndex(item);
            int verticalIndex   = GetItemVerticalIndex(item);
            int horizontalIndex = GetItemHorizontalIndex(item);

            if (pageIndex       >= 0) this.pageIndex       = pageIndex;
            if (verticalIndex   >= 0) this.verticalIndex   = verticalIndex;
            if (horizontalIndex >= 0) this.horizontalIndex = horizontalIndex;
        }

        protected sealed override void ClampDecrementHorizontalIndex(ref int horizontalIndex, int maxValue)
        {
            if (pageChangeDirection == PageChangeDirection.Horizontal && verticalIndex < 0){
                DecrementPageIndex();
            }

            base.ClampDecrementHorizontalIndex(ref horizontalIndex, maxValue);
        }

        protected sealed override void ClampIncrementHorizontalIndex(ref int horizontalIndex, int maxValue)
        {
            if (pageChangeDirection == PageChangeDirection.Horizontal && verticalIndex >= maxValue) {
                IncrementPageIndex();
            }

            base.ClampIncrementHorizontalIndex(ref horizontalIndex, maxValue);
        }

        protected sealed override void ClampIncrementVerticalIndex(ref int verticalIndex, int maxValue)
        {
            if (pageChangeDirection == PageChangeDirection.Vertical && verticalIndex >= maxValue) {
                IncrementPageIndex();
            }

            base.ClampIncrementVerticalIndex(ref verticalIndex, maxValue);
        }

        protected sealed override void ClampDecrementVerticalIndex(ref int verticalIndex, int maxValue)
        {
            if(pageChangeDirection == PageChangeDirection.Vertical && verticalIndex >= maxValue) {
                DecrementPageIndex();
            }

            base.ClampDecrementVerticalIndex(ref verticalIndex, maxValue);
        }

        private void DecrementPageIndex()
        {
            if (!canWrapPageIndex && pageIndex <= 0)
                return;

            pageIndex--;
            if (canWrapPageIndex && pageIndex < 0) pageIndex = GetMaxPages() - 1;
        }

        private void IncrementPageIndex()
        {
            if (!canWrapPageIndex && pageIndex <= 0)
                return;

            pageIndex++;
            if (canWrapPageIndex && pageIndex >= GetMaxPages()) pageIndex = 0;
        }

        private void AddPage()
        {
            CleanPages();

            if (pages != null) {
                pages.Add(new Item[maxVerticalPageIndex, maxHorizontalPageIndex]);
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
            if (pages == null || item == null || pages.Count == 0) return;

            for (int i = 0; i < pages.Count; i++) {
                AddPageItem(item, pages[i], i, out bool wasItemAdded);
                if (wasItemAdded) return;
            }

            AddPage();
            pages[pages.Count - 1][0,0] = item;
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
            int pageIndex = GetItemPageIndex(item);
            int verticalIndex = GetItemVerticalIndex(item);
            int horizontalIndex = GetItemHorizontalIndex(item);

            CleanPages();
            if (pages == null || pages.Count == 0) return;

            if (pageIndex >= 0 && verticalIndex >= 0 && horizontalIndex >= 0)
                pages[pageIndex][verticalIndex, horizontalIndex] = null;

            RemovePages();
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
           //if(pages != null) pages = pages.Where(x => x != null).ToList();
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
            if(items == null || items.GetLength(0) == 0 || items.GetLength(1) == 0)
                { Debug.LogWarning($"Page Index {pageIndex + 1} \nEmpty Page"); return; }

            string result = $"Page {pageIndex + 1} \n";

            for (int i = 0; i < items.GetLength(0) ; i++) {
                result += $"Row {i + 1} | ";

                for (int j = 0; j < items.GetLength(1) ; j++) {
                    result += (items[i,j] != null ? "Item: " + items[i,j].name : "Item: null") + " | ";
                }
            }

            Debug.Log(result);
        }

        private bool CanRemovePage(int pageIndex)
        {  
            if(pages == null || pages.Count == 0) return false;
            if(pages.Count > 1) return CanRemovePage(GetPageItems(pageIndex));
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

        public int GetMaxPages()
        {
            if (pages == null) return -1;
            return pages.Count;
        }
        public int GetItemPageIndex(Item item)
        {
            if(pages == null || item == null || pages.Count == 0) return -1;

            for(int i = 0; i < pages.Count; i++) {
                int verticalIndex = GetItemIndex(item, pages[i], i, true);
                if (verticalIndex >= 0) return i;
            }

            return -1;
        }

        public  int GetItemVerticalIndex(Item item) 
        {         
            if(pages == null || item == null || pages.Count == 0) return -1;
            
            for(int i = 0;i < pages.Count; i++) {
                int verticalIndex = GetItemIndex(item, pages[i], i, true);
                if (verticalIndex >= 0) return verticalIndex;
            }

            return -1;
        }

        public  int GetItemHorizontalIndex(Item item)
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
            Item[,] items = GetPageItems(pageIndex);
            if (items == null) return -1;
            return items.GetLength(1);
        }


        public sealed override int GetMaxVerticalIndex()
        {
            return GetMaxVerticalIndex(pageIndex);
        }
        public int GetMaxVerticalIndex(int pageIndex)
        {
            Item[,] items = GetPageItems(pageIndex);

            if (items == null) return -1;
            return items.GetLength(0);
        }

        public Item[,] GetPageItems()
        {
            return GetPageItems(pageIndex);
        }
        public Item[,] GetPageItems(int pageIndex)
        {
            if(pages == null || pages.Count == 0 || pageIndex < 0 || pageIndex >= pages.Count)
                return null;

            return pages[pageIndex];
        }

        public Item[] GetItems() {
            return GetItems(pageIndex, verticalIndex);
        }
        public Item[] GetItems(int pageIndex, int verticalIndex)
        {
            Item[,] items = GetPageItems(pageIndex);
           
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
    }
}
