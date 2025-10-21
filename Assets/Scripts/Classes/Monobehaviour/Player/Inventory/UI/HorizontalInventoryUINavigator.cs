using RedSilver2.Framework.Interactions.Items;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public sealed class HorizontalInventoryUINavigator : InventoryUINavigator 
    {
        [Space]
        [SerializeField] private int verticalIndex;
        public int VerticalIndex => verticalIndex;

        public override void SetIndex(Item item)
        {
            if (inventory is ComplexInventory && item != null) 
            {
                ComplexInventory inventory = (this.inventory as ComplexInventory);
               
                if (inventory.Contains(verticalIndex, item))
                    horizontalIndex = inventory.GetHorizontalIndex(item);
            }             
        }

        protected sealed override int GetMaxHorizontalIndex()
        {
            if (inventory is ComplexInventory)
                return (inventory as ComplexInventory).GetMaxHorizontalIndex(verticalIndex);
            return -1;
        }
    }
}
