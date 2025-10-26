using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class VerticalInventoryUINavigator : InventoryUINavigator
    {
        

        [Space]
        [SerializeField] private UnityEvent<int> onVerticalIndexChanged;

        private OverrideablePressInput nextVerticalPressInput;
        private OverrideablePressInput previousVerticalPressInput;

        protected int verticalIndex;
        public  int VerticalIndex => verticalIndex;

        public const string NEXT_VERTICAL_INPUT_NAME     = "Next Vertical Navigator Input";
        public const string PREVIOUS_VERTICAL_INPUT_NAME = "Previous Vertical Navigator Input";


        protected override void Awake()
        {
            base.Awake();
            verticalIndex = 0;

            nextVerticalPressInput     = GetNextVerticalInput();
            previousVerticalPressInput = GetPreviousVerticalInput();

            nextVerticalPressInput.Enable();
            previousVerticalPressInput.Enable();
        }

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
        protected sealed override void UpdateInput() 
        {
            base.UpdateInput();

            if (nextVerticalPressInput != null && previousVerticalPressInput != null) {
                nextVerticalPressInput.Update();
                previousVerticalPressInput.Update();

                if      (nextVerticalPressInput.Value)     IncrementVerticalIndex();
                else if (previousVerticalPressInput.Value) DecrementVerticalIndex();
            }
        }

        private void DecrementVerticalIndex()  
        {
            verticalIndex--;
            ClampDecrementVerticalIndex(ref verticalIndex, GetMaxVerticalIndex());
            if(onVerticalIndexChanged != null) onVerticalIndexChanged.Invoke(verticalIndex);
        }

        private void IncrementVerticalIndex()  
        {
            verticalIndex++;
            ClampIncrementVerticalIndex(ref verticalIndex, GetMaxVerticalIndex());
            if (onVerticalIndexChanged != null) onVerticalIndexChanged.Invoke(verticalIndex);
        }

        protected virtual void ClampIncrementVerticalIndex(ref int verticalIndex, int maxValue)
        {
            if (verticalIndex >= maxValue) verticalIndex = 0;
        }

        protected virtual void ClampDecrementVerticalIndex(ref int verticalIndex, int maxValue)
        {
            if (verticalIndex < 0) verticalIndex = maxValue;
        }


        public abstract int GetMaxVerticalIndex();

        public static OverrideablePressInput GetNextVerticalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(NEXT_VERTICAL_INPUT_NAME, KeyboardKey.S, GamepadButton.DpadDown); 
        }


        public OverrideablePressInput GetPreviousVerticalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(PREVIOUS_VERTICAL_INPUT_NAME, KeyboardKey.W, GamepadButton.DpadUp);
        }
    }
}
