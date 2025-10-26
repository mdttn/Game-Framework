using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class InventoryUINavigator : MonoBehaviour
    {
        [SerializeField] protected Inventory inventory;

        [Space]
        [SerializeField] private UnityEvent onUpdate, onLateUpdate, onEnable, onDisable;

        [Space]
        [SerializeField] private UnityEvent<int> onHorizontalIndexChanged;

        protected int horizontalIndex;

        private OverrideablePressInput nextHorizontalPressInput;
        private OverrideablePressInput previousHorizontalPressInput;

        private static List<InventoryUINavigator> navigators;
        private static InventoryUINavigator current;


        public    int HorizontalIndex => horizontalIndex;
        public Inventory Inventory => inventory;

        public InventoryUINavigator[] Navigators
        {
            get
            {
                if(navigators == null) return new InventoryUINavigator[0];
                return navigators.ToArray();
            }
        }

        public const string NEXT_HORIZONTAL_INPUT_NAME     = "Next Horizontal Navigator Input";
        public const string PREVIOUS_HORIZONTAL_INPUT_NAME = "Previous Horizontal Navigator Input";
        protected virtual void Awake() 
        {
            this.enabled    = false;
            horizontalIndex = 0;

            nextHorizontalPressInput     = GetNextHorizontalInput();
            previousHorizontalPressInput = GetPreviousHorizontalInput();

            nextHorizontalPressInput.Enable();
            previousHorizontalPressInput.Enable();

            inventory.AddOnOpenUIListener(OnOpenInventoryUI);
            inventory.AddOnCloseUIListener(OnCloseInventoryUI);
            AddOnUpdateListener(UpdateInput);
        }

        private void Update() {
            if(onUpdate != null) onUpdate.Invoke();
        }

        private void LateUpdate()  {
            if (onLateUpdate != null) onLateUpdate.Invoke();
        }

        private void OnDisable() 
        {
            if (current != this) return;
            if (onDisable != null && didAwake) onDisable.Invoke();
        }

        private void OnEnable() {
            if (current != this) { enabled = false; return; }
            if (onEnable != null && didAwake) onEnable.Invoke();
        }

        private void OnOpenInventoryUI() {
            SetCurrent(this);
        }

        private void OnCloseInventoryUI(){
            SetCurrent(null);
        }

        public void AddOnEnableListener(UnityAction action) {
            if (onEnable != null && action != null)
                onEnable.AddListener(action);
        }
        public void RemoveOnEnableListener(UnityAction action)
        {
            if (onEnable != null && action != null)
                onEnable.RemoveListener(action);
        }

        public void AddOnDisableListener(UnityAction action)
        {
            if (onDisable != null && action != null)
                onDisable.AddListener(action);
        }
        public void RemoveOnDisableListener(UnityAction action)
        {
            if (onDisable != null && action != null)
                onDisable.RemoveListener(action);
        }

        public void AddOnUpdateListener(UnityAction action)
        {
            if(onUpdate != null && action != null)
                onUpdate.AddListener(action);
        }
        public void RemoveOnUpdateListener(UnityAction action)
        {
            if (onUpdate != null && action != null)
                onUpdate.RemoveListener(action);
        }

        public void AddOnLateUpdateListener(UnityAction action)
        {
            if (onLateUpdate != null && action != null)
                onLateUpdate.AddListener(action);
        }
        public void RemoveOnLateUpdateListener(UnityAction action)
        {
            if (onLateUpdate != null && action != null)
                onLateUpdate.RemoveListener(action);
        }

        public void AddOnHorizontalIndexChangedListener(UnityAction<int> action)
        {
            Debug.LogWarning(action);
            if (onHorizontalIndexChanged != null && action != null)
               onHorizontalIndexChanged.AddListener(action);
        }

        public void RemoveOnHorizontalIndexChangedListener(UnityAction<int> action)
        {
            if (onHorizontalIndexChanged != null && action != null)
                onHorizontalIndexChanged.RemoveListener(action);
        }

        protected virtual void UpdateInput()
        {
            if (nextHorizontalPressInput != null) {
                nextHorizontalPressInput.Update();
                if(nextHorizontalPressInput.Value) IncrementHorizontalIndex();
            }

            if(previousHorizontalPressInput != null){
                previousHorizontalPressInput.Update();
                if (previousHorizontalPressInput.Value) DecrementHorizontalIndex();
            }
        }

        private void IncrementHorizontalIndex() 
        {
           if (inventory == null) return;
           horizontalIndex++;
           ClampIncrementHorizontalIndex(ref horizontalIndex, GetMaxHorizontalIndex());
           if (onHorizontalIndexChanged != null) onHorizontalIndexChanged.Invoke(horizontalIndex);
        }

        private void DecrementHorizontalIndex() 
        {
            if (inventory == null) return;
            horizontalIndex--;
            ClampDecrementHorizontalIndex(ref horizontalIndex, GetMaxHorizontalIndex());
            if (onHorizontalIndexChanged != null) onHorizontalIndexChanged.Invoke(horizontalIndex);
        }

        protected virtual void ClampIncrementHorizontalIndex(ref int horizontalIndex, int maxValue) {
            if (horizontalIndex >= maxValue) horizontalIndex = 0;
        }

        protected virtual void ClampDecrementHorizontalIndex(ref int horizontalIndex, int maxValue) {
            if (horizontalIndex < 0) horizontalIndex = maxValue;
        }

        public abstract void SetIndex(Item item);
        public abstract int GetMaxHorizontalIndex();

        public static void Disable() {
            if (current != null) current.enabled = false;
        }

        public static void Enable() {
            if (current != null) current.enabled = true;
        }

        public static void SetCurrent(InventoryUINavigator navigator) 
        {
            if (current != navigator) {
                Disable();
                current = navigator;
                Enable();
            }
        }

        public static OverrideablePressInput GetNextHorizontalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(NEXT_HORIZONTAL_INPUT_NAME, KeyboardKey.D, GamepadButton.DpadRight);
        }

        public static OverrideablePressInput GetPreviousHorizontalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(PREVIOUS_HORIZONTAL_INPUT_NAME, KeyboardKey.A, GamepadButton.DpadLeft); ;
        }
    }
}
