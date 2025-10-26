using RedSilver2.Framework.Interactions.Items;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public class SimpleInventoryUINavigator : InventoryUINavigator
    {
        public sealed override void SetIndex(Item item)
        {
            if (inventory is SimpleInventory && item != null) {
                SimpleInventory inventory = this.inventory as SimpleInventory;
                if(inventory.Contains(item)) horizontalIndex = inventory.GetHorizontalIndex(item);
            }
        }

        public override int GetMaxHorizontalIndex()
        {
            if (inventory is SimpleInventory)
                return (inventory as SimpleInventory).GetMaxHorizontalIndex();
            return -1;
        }
    }
}
