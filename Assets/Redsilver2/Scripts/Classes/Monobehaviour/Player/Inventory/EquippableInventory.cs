using RedSilver2.Framework.Interactions.Items;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Player.Inventories
{
    public class EquippableInventory : Inventory
    {
        private UnityEvent<Item> onItemEquip, onItemUnequip;


        protected override void Awake()
        {
            base.Awake();
            onItemEquip = new UnityEvent<Item>();
            onItemUnequip = new UnityEvent<Item>();
        }

        public void AddOnItemEquipListener(UnityAction<Item> action)
        {
            if (onItemEquip != null && action != null)
                onItemEquip.AddListener(action);
        }

        public void RemoveOnItemEquipListener(UnityAction<Item> action)
        {
            if (onItemEquip != null && action != null)
                onItemEquip.RemoveListener(action);
        }

        public void AddOnItemUnequipListener(UnityAction<Item> action)
        {
            if (onItemUnequip != null && action != null)
                onItemUnequip.AddListener(action);
        }

        public void RemoveOnItemUnequipListener(UnityAction<Item> action)
        {
            if (onItemUnequip != null && action != null)
                onItemUnequip.RemoveListener(action);
        }

        public sealed override void AddItem(Item item, out bool isItemAdded)
        {
            isItemAdded = false;
            if (item is UpdateableItem) base.AddItem(item, out isItemAdded);
        }

        public void Equip(Item item)
        {
            if (item is UpdateableItem && Contains(item))
                Equip(item as  UpdateableItem);
        }

        private void Equip(UpdateableItem item) {
            if (item == null) return;


        }
    }
}