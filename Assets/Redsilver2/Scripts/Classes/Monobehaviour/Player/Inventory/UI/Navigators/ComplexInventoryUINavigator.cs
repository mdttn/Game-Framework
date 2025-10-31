using RedSilver2.Framework.Interactions.Items;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public sealed class ComplexInventoryUINavigator : VerticalInventoryUINavigator
    {
        [Space]
        [SerializeField] private int maxHorizontalIndex;

        public sealed override void Select(Item item)
        {
           int horizontalIndex = GetHorizontalIndex(item);
           int verticalIndex   = GetVerticalIndex(item);

           if(verticalIndex >= 0 && horizontalIndex >= 0) {
                this.horizontalIndex = horizontalIndex;
                this.verticalIndex = verticalIndex;
           }
        }

        public sealed override int GetMaxHorizontalIndex()
        {
            Item[,] items = GetItems();
            if (items == null || items.GetLength(0) == 0 || items.GetLength(1) == 0) return -1;
            return items.GetLength(1);
        }

        public sealed override int GetMaxVerticalIndex()
        {
            Item[,] items = GetItems();
            if (items == null || items.GetLength(0) == 0 || items.GetLength(1) == 0) return -1;
            return items.GetLength(0);
        }

        protected override void OnItemAdded(Item item)
        {
            base.OnItemAdded(item); 

            Item[,] items = GetItems();
            string results = "";

            if (items == null) return;

            for (int i = 0; i < items.GetLength(0); i++)
            {
                results += $"Row {i + 1} | ";

                for (int j = 0; j < items.GetLength(1); j++) {
                    results += $"Item Name: " + (items[i, j] == null ? "Null" : items[i, j].name) + "| ";
                }

                results += "\n";
            }
                    
            Debug.LogWarning(results);
        }

        public sealed override Item[,] GetItems()
        {
            if (inventory == null) return new Item[0, 0];
            return GetItems(inventory);
        }

        private Item[,] GetItems(Inventory inventory)
        {
            if (inventory == null || maxHorizontalIndex == 0) return new Item[0, 0];
            return GetItems(inventory, (inventory.GetMaxHorizontalIndex() + 1) / maxHorizontalIndex);
        }

        private Item[,] GetItems(Inventory inventory, int maxVerticalIndex)
        {
            Item[,] results;
            if (inventory == null) return new Item[0, 0];

            results = new Item[maxVerticalIndex == 0 ? 1 : maxVerticalIndex + 1, maxHorizontalIndex];

            GetItems(inventory, ref results);
            return results == null ? new Item[0, 0] : results;
        }

        private void GetItems(Inventory inventory, ref Item[,] results)
        {
            if (inventory == null || results == null || results.GetLength(0) == 0 || results.GetLength(1) == 0 ) return;

            int verticalIndex = 0, horizontalIndex = 0;
            GetItems(inventory.GetItems(), ref verticalIndex, ref horizontalIndex, ref results);
        }


        private void GetItems(Item[] items, ref int verticalIndex, ref int horizontalIndex, ref Item[,] results) 
        {
            if (items == null || results == null) return;

            for (int i = 0; i < items.Length; i++) 
                GetItem(items[i], ref verticalIndex, ref horizontalIndex, ref results);
        }

        private void GetItem(Item item, ref int verticalIndex, ref int horizontalIndex, ref Item[,] results)
        {
            if(results == null) return;

            results[verticalIndex, horizontalIndex] = item;
            horizontalIndex++;

            if (horizontalIndex >= results.GetLength(1)){
                horizontalIndex = 0;
                verticalIndex++;
            }
        }

        public sealed override Item GetSelectedItem()
        {
            Item[,] items = GetItems();
            if (items == null || items.GetLength(0) == 0 || items.GetLength(1) == 0) return null;
            return items[verticalIndex, horizontalIndex];
        }
    }
}
