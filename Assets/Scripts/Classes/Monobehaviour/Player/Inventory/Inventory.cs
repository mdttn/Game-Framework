
using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories
{
    public abstract class Inventory : MonoBehaviour 
    {
        [SerializeField] private bool allowDuplicateItems;


        [Space]
        [SerializeField] private UnityEvent<Item> onItemAdded;
        [SerializeField] private UnityEvent<Item> onItemRemoved;

        private static List<Inventory> instances = new List<Inventory>();


        protected bool AllowDuplicateItems => allowDuplicateItems;

        public static Inventory[] Instances
        {
            get
            {
                if(instances == null) return new Inventory[0];
                return instances.ToArray();
            }
        }


        protected virtual void Awake() {
            instances.Add(this);
        }

        public void AddOnItemAddedListener(UnityAction<Item> action)
        {
            if(onItemAdded != null && action != null) {
                onItemAdded.AddListener(action);
            }
        }
        public void RemoveOnItemAddedListener(UnityAction<Item> action)
        {
            if (onItemAdded != null && action != null)
            {
                onItemAdded.RemoveListener(action);
            }
        }

        public void AddOnItemRemovedListener(UnityAction<Item> action)
        {
            if (onItemRemoved != null && action != null) {
                onItemRemoved.AddListener(action);
            }
        }
        public void RemoveOnItemRemovedListener(UnityAction<Item> action)
        {
            if (onItemRemoved != null && action != null)
            {
                onItemRemoved.RemoveListener(action);
            }
        }

        public virtual void AddItem(Item item) {
            if(item != null && onItemAdded != null) {
                onItemAdded.Invoke(item);   
            }
        }
        public virtual void RemoveItem(Item item) {
            if (item != null && onItemRemoved != null) {
                onItemRemoved.Invoke(item);
            }
        }

        public abstract bool Contains(Item item);
        public abstract int  GetMaxHorizontalIndex();
        public abstract int  GetHorizontalIndex(Item item);
    }
}
