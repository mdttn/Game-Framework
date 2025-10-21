using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public sealed class ComplexInventoryUINavigator : InventoryUINavigator
    {
        [Space]
        [SerializeField] private KeyboardKey keyboardNextVertical, keyboardPreviousVertical;
       
        [Space]
        [SerializeField] private GamepadButton gamepadNextVertical, gamepadPreviousVertical;

        [Space]
        [SerializeField] private UnityEvent<int> onVerticalIndexChanged;

        private int verticalIndex;
        public  int VerticalIndex => verticalIndex;

        public void AddOnVerticalIndexChangedListener(UnityAction<int> action) 
        {
            if (onVerticalIndexChanged != null && action != null)
                onVerticalIndexChanged.AddListener(action);
        }

        public void RemoveOnVerticalIndexChangedListener(UnityAction<int> action) 
        {
              if(onVerticalIndexChanged != null && action != null)
                  onVerticalIndexChanged.RemoveListener(action);
        }
        protected sealed override void UpdateInput() {
            base.UpdateInput();

            if (InputManager.GetKey(keyboardNextVertical, gamepadNextVertical))
                IncrementVerticalIndex();
            else if (InputManager.GetKey(keyboardPreviousVertical, gamepadPreviousVertical))
                DecrementVerticalIndex();
        }

        private void DecrementVerticalIndex()  {
            verticalIndex--;
            if(verticalIndex < 0) verticalIndex = GetMaxVerticalIndex() - 1;

            int maxHorizontalIndex = GetMaxHorizontalIndex();
            if(horizontalIndex >= maxHorizontalIndex) horizontalIndex = maxHorizontalIndex - 1;

            if(onVerticalIndexChanged != null) onVerticalIndexChanged.Invoke(verticalIndex);
        }

        private void IncrementVerticalIndex() 
        {
            verticalIndex++;
            if (verticalIndex >= GetMaxVerticalIndex()) verticalIndex = 0;

            int maxHorizontalIndex = GetMaxHorizontalIndex();
            if (horizontalIndex >= maxHorizontalIndex) horizontalIndex = maxHorizontalIndex - 1;

            if (onVerticalIndexChanged != null) onVerticalIndexChanged.Invoke(verticalIndex);
        }

        public sealed override void SetIndex(Item item) 
        {
            if(inventory is ComplexInventory && item != null)
            {
                ComplexInventory inventory = (this.inventory as ComplexInventory);
               
                if(inventory.Contains(item))
                    inventory.GetItemIndexes(item, out verticalIndex, out horizontalIndex);
            }
        }

        protected sealed override int GetMaxHorizontalIndex() {
            if (inventory is ComplexInventory)
                return (inventory as ComplexInventory).GetMaxHorizontalIndex(verticalIndex);
            return -1;
        }

        private int GetMaxVerticalIndex() 
        {
            if (inventory is ComplexInventory)
                return (inventory as ComplexInventory).GetMaxVerticalIndex();
            return -1;
        }
    }
}
