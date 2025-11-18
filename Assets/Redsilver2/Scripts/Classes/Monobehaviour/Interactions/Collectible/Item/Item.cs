using RedSilver2.Framework.Interactions.Collectibles;
using RedSilver2.Framework.Player.Inventories;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Items
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Item : Collectible 
    {
        private UnityEvent onAdded, onRemoved;

        private Inventory     owner;
        private Collider     _collider;
        
        private MeshRenderer _renderer;
        private AudioSource  _source;


        private readonly static UnityEvent<Item> onInstanceAdded   = new UnityEvent<Item>();
        private readonly static UnityEvent<Item> onInstanceRemoved = new UnityEvent<Item>();
        private readonly static List<Item>       instances         = new List<Item>();

        public static Item[] Instances {
            get {
                if (instances == null) return new Item[0];
                return instances.ToArray();
            }
        }

        protected override void Awake() 
        {
            base.Awake();

            onAdded   = new UnityEvent();
            onRemoved = new UnityEvent();

            _collider = GetComponent<Collider>();
            _renderer = GetComponent<MeshRenderer>();
            _source   = GetComponent<AudioSource>();

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

        protected virtual void OnAdded()
        {
            SetVisiblity(false, false);
        }
        protected virtual void OnRemoved()
        {
            SetVisiblity(true, true);
        }

        protected void SetVisiblity(bool isColliderVisible, bool isMeshRendererVisible)
        {
            SetColliderVisibility(isColliderVisible);
            SetMeshRendererVisibility(isMeshRendererVisible);
        }

        private void SetColliderVisibility(bool isVisible) {
            if (_collider != null) _collider.enabled = isVisible;
        }

        private void SetMeshRendererVisibility(bool isVisible) {
            if(_renderer != null) _renderer.enabled = isVisible;
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
                onAdded.Invoke();
                owner = inventory;
            }
        }

        private void Remove()
        {
            if (owner == null) return;

            if (owner.Contains(this)) {
                owner.RemoveItem(this, out bool isItemRemoved);
                if (isItemRemoved) onRemoved.Invoke();
            }

            owner = null;
        }

        public void AddOnAddedListener(UnityAction action)
        {
            if(onAdded != null && action != null)
                onAdded.AddListener(action);
        }

        public void RemoveOnAddedListener(UnityAction action)
        {
            if (onAdded != null && action != null)
                onAdded.RemoveListener(action);
        }


        public void AddOnRemovedListener(UnityAction action)
        {
            if (onRemoved != null && action != null)
                onRemoved.AddListener(action);
        }

        public void RemoveOnRemovedListener(UnityAction action)
        {
            if (onRemoved != null && action != null)
                onRemoved.RemoveListener(action);
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

        public abstract void Drop();
    }

}
