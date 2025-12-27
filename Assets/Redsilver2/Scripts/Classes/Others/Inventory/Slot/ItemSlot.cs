using RedSilver2.Framework.Interactions.Items;

namespace RedSilver2.Framework.Player.Inventories
{
    [System.Serializable]
    public class ItemSlot
    {
        public UpdateableItem item;
        public ItemHandSide   handSide;

        public ItemSlot(ItemHandSide handSide)
        {
            this.item = null;
            this.handSide = handSide;
        }

        public ItemSlot(UpdateableItem item, ItemHandSide handSide)
        {
            this.item = item;
            this.handSide = handSide;
        }

        public void Set(UpdateableItem item)
        {
            this.item = item;
        }

        public void Set(ItemHandSide handSide)
        {
            this.handSide = handSide;
        }
    }
}
