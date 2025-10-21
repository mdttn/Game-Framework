using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class InventoryUINavigator : MonoBehaviour
    {
        [SerializeField] protected Inventory inventory;

        [Space]
        [SerializeField] private KeyboardKey keyboardNextHorizontal, keyboardPreviousHorizontal;

        [Space]
        [SerializeField] private GamepadButton  gamepadNextHorizontal, gamepadPreviousHorizontal;

        [Space]
        [SerializeField] private UnityEvent<int> onHorizontalIndexChanged;

        protected int horizontalIndex;
        public    int HorizontalIndex => horizontalIndex;
        public Inventory Inventory => inventory;

        protected virtual void Awake() {
            this.enabled = false;
        }

        private void Update() {
            UpdateInput();
        }

        public void AddOnHorizontalIndexChangedListener(UnityAction<int> action)
        {
            if(onHorizontalIndexChanged != null && action != null)
               onHorizontalIndexChanged.AddListener(action);
        }
        public void RemoveOnHorizontalIndexChangedListener(UnityAction<int> action)
        {
            if (onHorizontalIndexChanged != null && action != null)
                onHorizontalIndexChanged.RemoveListener(action);
        }

        protected virtual void UpdateInput()
        {
            if (InputManager.GetKey(keyboardNextHorizontal, gamepadNextHorizontal))
                IncrementHorizontalIndex();
            else if (InputManager.GetKey(keyboardPreviousHorizontal, gamepadPreviousHorizontal))
                DecrementHorizontalIndex();
        }

        private void IncrementHorizontalIndex() 
        {
            if (inventory == null) return;
                horizontalIndex++;

            if (horizontalIndex >= GetMaxHorizontalIndex())
                horizontalIndex = 0;

           if(onHorizontalIndexChanged != null) onHorizontalIndexChanged.Invoke(horizontalIndex);
        }

        private void DecrementHorizontalIndex() 
        {
            if (inventory == null) return;
            horizontalIndex--;

            if(horizontalIndex < 0)
                horizontalIndex = GetMaxHorizontalIndex() - 1;

            if (onHorizontalIndexChanged != null) onHorizontalIndexChanged.Invoke(horizontalIndex);
        }

        public abstract void SetIndex(Item item);
        protected abstract int GetMaxHorizontalIndex();
    }
}
