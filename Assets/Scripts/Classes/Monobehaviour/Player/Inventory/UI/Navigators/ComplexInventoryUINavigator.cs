using RedSilver2.Framework.Interactions.Items;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    [RequireComponent(typeof(ComplexInventory))]
    public sealed class ComplexInventoryUINavigator : VerticalInventoryUINavigator
    {
        public sealed override void SetIndex(Item item)
        {
            if (inventory is ComplexInventory && item != null) {
                ComplexInventory inventory = (this.inventory as ComplexInventory);

                if (inventory.Contains(item))
                    inventory.GetItemIndexes(item, out verticalIndex, out horizontalIndex);
            }
        }

        public sealed override int GetMaxHorizontalIndex()
        {
            if (inventory is ComplexInventory)
                return (inventory as ComplexInventory).GetMaxHorizontalIndex(verticalIndex);
            return -1;
        }

        public sealed override int GetMaxVerticalIndex()
        {
            if (inventory is ComplexInventory)
                return (inventory as ComplexInventory).GetMaxVerticalIndex();
            return -1;
        }
    }
}
