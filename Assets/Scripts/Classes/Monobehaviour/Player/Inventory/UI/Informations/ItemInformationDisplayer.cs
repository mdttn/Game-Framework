using RedSilver2.Framework.Interactions.Items;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class ItemInformationDisplayer : InventoryUI
    {
        [Space]
        [SerializeField] protected string nullErrorMessage;

        protected override void Awake()
        {
            SetNavigatorEvents();
        }

        private void SetNavigatorEvents()
        {
            if (navigator == null) return;

             SetInventoryEvents(navigator.Inventory);
             navigator.AddOnHorizontalIndexChangedListener(OnHorizontalIndexChanged);

            if (navigator is VerticalInventoryUINavigator)
            (navigator as VerticalInventoryUINavigator).AddOnVerticalIndexChangedListener(OnVerticalIndexChanged);
        }

        private void SetInventoryEvents(Inventory inventory)
        {
            if(inventory == null) return;
            inventory.AddOnCloseUIListener(OnInventoryUIClose);
            inventory.AddOnOpenUIListener(OnInventoryUIOpen);
        }

        private void OnInventoryUIClose()
        {
            DisplayItemInformation(string.Empty);
        }

        private void OnInventoryUIOpen() {
            DisplayItemInformation();
        }


        private void OnHorizontalIndexChanged(int horizontalIndex) {
            Debug.LogWarning(horizontalIndex);
            DisplayItemInformation();
        }

        private void OnVerticalIndexChanged(int verticalIndex) {
            DisplayItemInformation();
        }

        private void DisplayItemInformation() 
        {
            if (navigator == null) return;
            DisplayItemInformation(navigator, navigator.Inventory);
        }

        private void DisplayItemInformation(InventoryUINavigator navigator, Inventory inventory)
        {
            if (navigator == null || inventory == null) { DisplayItemInformation(nullErrorMessage); return;  }
            
                if (navigator is SimpleInventoryUINavigator)
                    DisplayItemInformation(navigator as SimpleInventoryUINavigator, inventory as SimpleInventory);
                if (navigator is HorizontalInventoryUINavigator)
                    DisplayItemInformation(navigator as HorizontalInventoryUINavigator, inventory as ComplexInventory);
                else if (navigator is VerticalInventoryUINavigator)
                    DisplayItemInformation(navigator as VerticalInventoryUINavigator, inventory as ComplexInventory);
        }

        private void DisplayItemInformation(SimpleInventoryUINavigator navigator, SimpleInventory inventory) 
        {
            if (navigator == null || inventory == null) { DisplayItemInformation(nullErrorMessage); return; }
            DisplayItemInformation(inventory.GetItem(navigator.HorizontalIndex));
        }

        private void DisplayItemInformation(HorizontalInventoryUINavigator navigator, ComplexInventory inventory) {
            if (navigator == null || inventory == null) { DisplayItemInformation(nullErrorMessage); return; }
            DisplayItemInformation(inventory.GetItem(navigator.VerticalIndex, navigator.HorizontalIndex));
        }

        private void DisplayItemInformation(VerticalInventoryUINavigator navigator, ComplexInventory inventory) {
            if (navigator == null || inventory == null) { DisplayItemInformation(nullErrorMessage); return; }
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
