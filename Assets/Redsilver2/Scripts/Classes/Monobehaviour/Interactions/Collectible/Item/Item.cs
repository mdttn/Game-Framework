using RedSilver2.Framework.Interactions.Collectibles;
using RedSilver2.Framework.Player.Inventories;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Items
{
    public class Item : Collectible 
    {
        [SerializeField] private ItemData data;

        private UnityEvent<Item> onAdded;
        private UnityEvent<Item> onRemoved;

        private Inventory owner;
        private Collider  _collider;

        private readonly static UnityEvent<Item> onInstanceAdded   = new UnityEvent<Item>();
        private readonly static UnityEvent<Item> onInstanceRemoved = new UnityEvent<Item>();
        private readonly static List<Item>       instances         = new List<Item>();

        public static Item[] Instances {
            get {
                if (instances == null) return new Item[0];
                return instances.ToArray();
            }
        }

        protected override void Awake() {
            base.Awake();
            onAdded   = new UnityEvent<Item>();
            onRemoved = new UnityEvent<Item>();

            _collider = GetComponent<Collider>();
            onInstanceAdded.Invoke(this);
        }

        protected override void Start()
        {
            base.Start();
            AddOnAddedListener(OnAdded);
            AddOnRemovedListener(OnRemoved);
        }

        protected virtual void OnDestroy() {
            onInstanceRemoved.Invoke(this);
        }

        protected virtual void OnAdded(Item item)
        {
            if(item == this && _collider != null) {
                _collider.enabled = false;
            }
        }

        protected virtual void OnRemoved(Item item)
        {
            if (item == this && _collider != null) {
                _collider.enabled = true;
            }
        }

        protected sealed override void OnInteract()
        {
            Inventory inventory = Inventory.GetInventory(0);

            if (inventory == null || inventory == owner) return;
            Add(inventory);

            if (inventory.Contains(this)) {
                base.OnInteract();
            }
        }

        private void Add(Inventory inventory) 
        {
            if (inventory == null || inventory == owner) return;
            Remove();

            inventory.AddItem(this, out bool isItemAdded);

            if (isItemAdded) {
                onAdded.Invoke(this);
                owner = inventory;
            }
        }

        private void Remove()
        {
            if (owner == null) return;

            if (owner.Contains(this)) {
                owner.RemoveItem(this, out bool isItemRemoved);
                if (isItemRemoved) onRemoved.Invoke(this);
            }

            owner = null;
        }

        public void AddOnAddedListener(UnityAction<Item> action)
        {
            if(onAdded != null && action != null)
                onAdded.AddListener(action);
        }

        public void RemoveOnAddedListener(UnityAction<Item> action)
        {
            if (onAdded != null && action != null)
                onAdded.RemoveListener(action);
        }


        public void AddOnRemovedListener(UnityAction<Item> action)
        {
            if (onRemoved != null && action != null)
                onRemoved.AddListener(action);
        }

        public void RemoveOnRemovedListener(UnityAction<Item> action)
        {
            if (onRemoved != null && action != null)
                onRemoved.RemoveListener(action);
        }


        public sealed override CollectibleData GetData()
        {
            return data;
        }

        public static void AddOnInstanceAddedListener(UnityAction<Item> action)
        {
            if(onInstanceAdded != null && action != null)
                onInstanceAdded.AddListener(action);
        }
        public static void RemoveOnInstanceAddedListener(UnityAction<Item> action)
        {
            if (onInstanceAdded != null && action != null)
                onInstanceAdded.RemoveListener(action);
        }

        public static void AddOnInstanceRemovedListener(UnityAction<Item> action) {
            if (onInstanceRemoved != null && action != null)
                onInstanceRemoved.AddListener(action);
        }
        public static void RemoveOnInstanceRemovedListener(UnityAction<Item> action) {
            if (onInstanceRemoved != null && action != null)
                onInstanceRemoved.RemoveListener(action);
        }
    }

}
