using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using System.Linq;

namespace RedSilver2.Framework.Player.Inventories
{
    public class SimpleInventory : Inventory  {
        
        private List<Item> items;
        public Item[] Items
        {
            get
            {
                if (items == null) return new Item[0];
                return items.ToArray();
            }
        }

        protected sealed override void Awake()
        {
            items = new List<Item>();
            base.Awake();
        }

        public sealed override void AddItem(Item item, out bool isItemAdded)
        {
            if (items == null || item == null) {
                isItemAdded = false;
                return;
            }

            isItemAdded = false;

            if ((ContainsDuplicate(item) && AllowDuplicateItems) || Contains(item)) {
                items.Add(item);
                base.AddItem(item, out isItemAdded);
            }
        }


        public sealed override void RemoveItem(Item item, out bool isItemRemoved)
        {
            if (items == null || item == null)
            {
                isItemRemoved = false;
                return;
            }

            isItemRemoved = false;

            if (items.Contains(item))
            {
                items.Remove(item);
                base.RemoveItem(item, out isItemRemoved);
            }
        }

        public int GetMaxHorizontalIndex()
        {
            if (items == null) return 0;
            return items.Count;
        }

        public sealed override bool ContainsDuplicate(Item item)
        {
            if(items == null || item == null) return false;
            return items.Where(x => x.GetType() == item.GetType()).Count() > 0;
        }

        public sealed override bool ContainsDuplicate(string itemName) {
            return ContainsDuplicate(GetItem(itemName));
        }

        public sealed override bool Contains(Item item)
        {
            if (items == null || item == null) return false;
            return items.Contains(item);
        }

        public sealed override bool Contains(string itemName) {
            return Contains(GetItem(itemName));
        }

        public sealed override int GetHorizontalIndex(Item item)
        {
            if (items == null || item == null) return -1;

            for(int i = 0; i < items.Count; i++) 
                if (items[i] == item) return i;

            return -1;
        }

        public sealed override int GetHorizontalIndex(string itemName) {
            return GetHorizontalIndex(GetItem(itemName));
        }

        public Item GetItem(int index) {
            if(items == null || index < 0 || index >= items.Count) return null;
            return items[index];
        }

        public sealed override Item GetItem(string itemName)  {
            if (items == null) return null;

            var results = items.Where(x => x != null).Where(x => x.name.ToLower() == itemName.ToLower());
            if (results.Count() > 0) return results.First();

            return null;
        }
    }
}
