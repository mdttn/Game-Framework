using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories
{
    public abstract class SimpleInventory : Inventory  {
        
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

        public sealed override void AddItem(Item item)
        {
            if (items == null || item == null) return;
            
            if((items.Contains(item) && AllowDuplicateItems) || !items.Contains(item)) {
                items.Add(item);
                base.AddItem(item);
            }
        }


        public sealed override void RemoveItem(Item item)
        {
            if (items == null || item == null) return;

            if (items.Contains(item))
            {
                items.Remove(item);
                base.RemoveItem(item);
            }
        }

        public sealed override int GetMaxHorizontalIndex()
        {
            if (items == null) return 0;
            return items.Count;
        }

        public sealed override bool Contains(Item item)
        {
            if (items == null) return false;
            return items.Contains(item);
        }

        public sealed override int GetHorizontalIndex(Item item)
        {
            if (items == null || item == null) return -1;

            for(int i = 0; i < items.Count; i++) 
                if (items[i] == item) return i;

            return -1;
        }
    }
}
