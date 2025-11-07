using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using RedSilver2.Framework.Player.Inventories.UI;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private Inventory mainInventory;

        private OverrideablePressInput uiInput;
        public const string INVENTORY_INTERACTION_PRESS_INPUT = "Inventory Interaction Press Input";

        private void Awake() {
            uiInput = GetUIInput();
            uiInput.AddOnUpdateListener(OnInputInteract);
            uiInput.Enable();
        }

        protected virtual void Update() {
            if(uiInput != null) uiInput.Update();
        }

        protected virtual void OnInputInteract()
        {
            if (mainInventory == null) return;

            if (mainInventory.IsUIOpened)
                mainInventory.Close();
            else
                mainInventory.Open();
        }

        public virtual void AddItem(Item item)
        {
            if (item == null) return;

            if (mainInventory == null) {
                mainInventory.AddItem(item, out bool isItemAdded);
            }
        }

        public static OverrideablePressInput GetUIInput() {
           return InputManager.GetOrCreateOverrideablePressInput(INVENTORY_INTERACTION_PRESS_INPUT, KeyboardKey.I, GamepadButton.ButtonNorth);
        }
    }
}
