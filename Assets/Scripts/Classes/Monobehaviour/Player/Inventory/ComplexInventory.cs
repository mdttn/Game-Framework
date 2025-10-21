using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories
{
    public abstract class ComplexInventory : Inventory
    {
        private List<List<Item>> items;
        public  Item[][] Items
        {
            get
            {
                List<Item[]> results;
                if(items == null || items.Count == 0) return (new List<Item[]>(0)).ToArray();

                results = new List<Item[]>();

                foreach (List<Item> result in items)
                    results.Add(result != null ? result.ToArray() : new Item[0]);

                return results.ToArray();
            }
        }

        public sealed override bool Contains(Item item) 
        {
            if(items == null || item == null || items.Count == 0) return false;

            var results = items.Where(x => x != null).Where(x => x.Contains(item));
            if (results.Count() > 0) return true;

            return false;
        }

        public bool Contains(int verticalIndex, Item item) {
            
            if (items == null || items.Count == 0 || item == null) return false;
            if (verticalIndex < 0 || verticalIndex >= items.Count) return false;

            List<Item> results = items[verticalIndex];
            if(results == null) return false;
            return results.Contains(item);
        }

        public sealed override bool Contains(string itemName) {
            return Contains(GetItem(itemName));
        }

        public override bool ContainsDuplicate(Item item) {
            if (items == null || item == null || items.Count == 0) return false;

            var results = items.Where(x => x != null).Where(x => x.GetType() == item.GetType());
            if (results.Count() > 0) return true;

            return false;
        }

        public sealed override bool ContainsDuplicate(string itemName) {
            return ContainsDuplicate(GetItem(itemName));
        }

        public void GetItemIndexes(Item item, out int verticalIndex, out int horizontalIndex) {
            verticalIndex   = GetVerticalIndex(item);
            horizontalIndex = GetHorizontalIndex(item);
        }
        public void GetItemIndexes(string itemName, out int verticalIndex, out int horizontalIndex) {
            verticalIndex = GetVerticalIndex(itemName);
            horizontalIndex = GetHorizontalIndex(itemName);
        }

        public sealed override int GetHorizontalIndex(Item item) {
            return GetItemIndex(item, false);
        }

        public sealed override int GetHorizontalIndex(string itemName) {
            return GetItemIndex(itemName, false);
        }

        public int GetVerticalIndex(string itemName) {
            return GetItemIndex(itemName, true);
        }

        public int GetVerticalIndex(Item item) {
            return GetItemIndex(item, true);
        }

        private int GetItemIndex(Item item, bool getVerticalIndex) {
            if (items == null || item == null || items.Count == 0) return -1;

            for (int i = 0; i < items.Count; i++)  {

                List<Item> results = items[i];
                if (results == null) continue;

                for (int j = 0; j < results.Count; j++)
                    if (results[j] == item) return getVerticalIndex ? i : j;
            }

            return -1;
        }

        private int GetItemIndex(string itemName, bool getVerticalIndex)  {
            return GetItemIndex(GetItem(itemName), getVerticalIndex);
        }

        public int GetMaxHorizontalIndex(int verticalIndex) 
        {
            if(items == null || items.Count == 0 || verticalIndex < 0 || verticalIndex >= items.Count)
                return -1;

            List<Item> results = items[verticalIndex];
            if(results == null) return -1;

            return results.Count;
        }

        public int GetMaxVerticalIndex()
        {
            if (items == null || items.Count == 0)
                return -1;

            return Items.Length;
        }

        public Item[] GetItems(int verticalIndex)
        {
            int maxVerticalIndex = GetMaxVerticalIndex();
            if (items == null || maxVerticalIndex < 0 || verticalIndex < 0 || verticalIndex >= maxVerticalIndex) return new Item[0];
            return items[verticalIndex].ToArray();
        }

        public Item GetItem(int verticalIndex, int horizontalIndex)
        {
            int maxHorizontalIndex = GetMaxHorizontalIndex(verticalIndex);
            int maxVerticalIndex   = GetMaxVerticalIndex();

            if (items == null || maxHorizontalIndex < 0 || maxVerticalIndex < 0) return null;
           
            if (verticalIndex < 0 || verticalIndex >= maxVerticalIndex || horizontalIndex < 0 || horizontalIndex >= maxHorizontalIndex)
                return null;

            return items[verticalIndex][horizontalIndex];
        }

        public sealed override Item GetItem(string itemName)
        {
            if (items == null || items.Count == 0 || string.IsNullOrEmpty(itemName)) return null;

            var results = items.Where(x => x != null);

            foreach(List<Item> result in results.Where(x => x != null)) {
                var items = result.Where(x => x.name.ToLower() ==  itemName.ToLower());
                if (items.Count() > 0) return items.First();
            }

            return null;
        }
    }
}
