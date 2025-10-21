using RedSilver2.Framework.Interactions.Items;
using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class ItemInformationDisplayer : InventoryUI
    {
        [SerializeField] protected string nullErrorMessage;

        protected sealed override void OnHorizontalIndexChanged(int horizontalIndex) {
            DisplayItemInformation();
        }

        protected sealed override void OnVerticalIndexChanged(int verticalIndex) {
            DisplayItemInformation();
        }

        private void DisplayItemInformation() 
        {
            Inventory inventory;
            if (navigator == null) return;

            inventory = navigator.Inventory;
         
            if (inventory == null) return;
            DisplayItemInformation(navigator, inventory as ComplexInventory);
        }

        private void DisplayItemInformation(InventoryUINavigator navigator, Inventory inventory) 
        {
            if(navigator != null && inventory != null) 
            {
                if (navigator is SimpleInventoryUINavigator)
                    DisplayItemInformation(navigator as SimpleInventoryUINavigator, inventory as SimpleInventory);
                if (navigator is HorizontalInventoryUINavigator)
                    DisplayItemInformation(navigator as HorizontalInventoryUINavigator, inventory as ComplexInventory);
                else if (navigator is ComplexInventoryUINavigator)
                    DisplayItemInformation(navigator as ComplexInventoryUINavigator, inventory as ComplexInventory);
            }
        }

        private void DisplayItemInformation(SimpleInventoryUINavigator navigator, SimpleInventory inventory) 
        {
            if (inventory != null && navigator != null) 
                DisplayItemInformation(inventory.GetItem(navigator.HorizontalIndex));
        }

        private void DisplayItemInformation(HorizontalInventoryUINavigator navigator, ComplexInventory inventory) {
            if(navigator != null && inventory != null) 
                DisplayItemInformation(inventory.GetItem(navigator.VerticalIndex, navigator.HorizontalIndex));
        }

        private void DisplayItemInformation(ComplexInventoryUINavigator navigator, ComplexInventory inventory) {
            if (navigator != null && inventory != null)
                DisplayItemInformation(inventory.GetItem(navigator.VerticalIndex, navigator.HorizontalIndex));
        }

        private void DisplayItemInformation(Item item) 
        {
            if (item != null)
                DisplayItemInformation(item.GetData() as ItemData);
            else
                DisplayNullMessage();
        }

        protected void DisplayNullMessage() {
            DisplayItemInformation(nullErrorMessage);
        }

        protected abstract void DisplayItemInformation(ItemData data);
        protected abstract void DisplayItemInformation(string message);
    }
}
