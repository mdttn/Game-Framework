using RedSilver2.Framework.Interactions.Collectibles;
using RedSilver2.Framework.Player.Inventories;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Items
{
    public class Item : Collectible 
    {
        [SerializeField] private ItemData data;

        [Space]
        [SerializeField] private UnityEvent<Item> onAdded;
        [SerializeField] private UnityEvent<Item> onRemoved;

        private Inventory owner;

        protected override void OnInteract() {

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


        public sealed override CollectibleData GetData()
        {
            return data;
        }

    }

}
