using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class InventoryUINavigator : MonoBehaviour
    {
        [SerializeField] private Transform modelParentTransform;

        [Space]
        [SerializeField] private bool canWrapHorizontalIndex = true;

        protected int  horizontalIndex;

        private UnityEvent       onUpdate, onLateUpdate, onEnable, onDisable;
        private UnityEvent<int>  onHorizontalIndexChanged;
        private UnityEvent<Item> onItemSelected;

        protected Inventory inventory;

        private OverrideablePressInput nextHorizontalPressInput;
        private OverrideablePressInput previousHorizontalPressInput;

        private static List<InventoryUINavigator> navigators;
        private static InventoryUINavigator current;

        protected InventoryUINavigatorTransitionsHandler transitionsHandler;

        public int HorizontalIndex      => horizontalIndex;
        public Transform ModelParentTransform => modelParentTransform;

        public static InventoryUINavigator Current => current;

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
            horizontalIndex = 0;
            inventory = GetInventory();

            onUpdate       = new UnityEvent();
            onLateUpdate   = new UnityEvent();

            onEnable       = new UnityEvent();
            onDisable      = new UnityEvent();

            onItemSelected     = new UnityEvent<Item>();
            transitionsHandler = GetComponentInChildren<InventoryUINavigatorTransitionsHandler>();

            nextHorizontalPressInput = GetNextHorizontalInput();
            previousHorizontalPressInput = GetPreviousHorizontalInput();
        }

        protected virtual void Start() 
        {
            Debug.LogWarning("Inventory: " + inventory);

            nextHorizontalPressInput.Enable();
            previousHorizontalPressInput.Enable();

            SetInventoryEvents(true);
            SetTransitionsEvents(true);
            SetDefaultEvents(true);
        }

        private void Update() {
            if(onUpdate != null) onUpdate.Invoke();
        }

        private void LateUpdate()  {
            if (onLateUpdate != null) onLateUpdate.Invoke();
        }

        private void OnDisable() {
            if (didStart) {
                if (current != this) return;
                if (onDisable != null) onDisable.Invoke();
            }
        }

        private void OnEnable() {

            if (didStart) {
                if (current != this) { enabled = false; return; }
                if (onEnable != null) onEnable.Invoke();
            }
        }

        private void OnDestroy()
        {
            SetInventoryEvents(false);
            SetTransitionsEvents(false);
            SetDefaultEvents(false);
        }

        private void SetInventoryEvents(bool addEvents)
        {
            if (addEvents)
            {
                AddOnItemAddedListener(OnItemAdded);
                AddOnItemRemovedListener(OnItemRemoved);
                AddOnOpenUIListener(OnOpenInventoryUI);
                AddOnCloseUIListener(OnCloseInventoryUI);
            }
            else
            {
                RemoveOnItemAddedListener(OnItemAdded);
                RemoveOnItemRemovedListener(OnItemRemoved);
                RemoveOnOpenUIListener(OnOpenInventoryUI);
                RemoveOnCloseUIListener(OnCloseInventoryUI);
            }
        }
        private void SetTransitionsEvents(bool addEvents)
        {
            if (addEvents) {
                AddOnTransitionsStartedListener(DisableInputs);
                AddOnTransitionsFinishedListener(EnableInputs);
            }
            else {
                RemoveOnTransitionsStartedListener(DisableInputs);
                RemoveOnTransitionsFinishedListener(EnableInputs);
            }
        }
        private void SetDefaultEvents(bool addEvents)
        {
            if (addEvents)
            {
                AddOnHorizontalIndexChangedListener(OnIndexChanged);
                AddOnLateUpdateListener(OnUpdateItemModel);
                AddOnUpdateListener(UpdateInput);
            }
            else
            {
                RemoveOnHorizontalIndexChangedListener(OnIndexChanged);
                RemoveOnLateUpdateListener(OnUpdateItemModel);
                RemoveOnUpdateListener(UpdateInput);
            }
        }

        protected virtual void OnIndexChanged(int index) {
            if(onItemSelected != null) onItemSelected.Invoke(GetSelectedItem());
        }

        protected virtual void OnOpenInventoryUI()
        {
            UpdateModels();



            SetCurrent(this);
        }

        private void OnCloseInventoryUI()
        {
            SetCurrent(null);
        }

        public abstract void ClearModels();

        public void AddOnTransitionsStartedListener(UnityAction action) {
            if (transitionsHandler != null) transitionsHandler.AddOnTransitionStartedListener(action);
        }
        public void RemoveOnTransitionsStartedListener(UnityAction action) {
            if (transitionsHandler != null) transitionsHandler.RemoveOnTransitionStartedListener(action);
        }

        public void AddOnTransitionsFinishedListener(UnityAction action) {
            if (transitionsHandler != null) transitionsHandler.AddOnTransitionFinishedListener(action);
        }

        public void RemoveOnTransitionsFinishedListener(UnityAction action) {
            if (transitionsHandler != null) transitionsHandler.RemoveOnTransitionFinishedListener(action); 
        }


        public void AddOnOpenUIListener(UnityAction action) {
            if(inventory != null) inventory.AddOnOpenUIListener(action);
        }
        public void RemoveOnOpenUIListener(UnityAction action){
            if (inventory != null) inventory.RemoveOnOpenUIListener(action);
        }

        public void AddOnCloseUIListener(UnityAction action){
            Debug.Log(action.Method.Name);
            if (inventory != null) inventory.AddOnCloseUIListener(action);
        }
        public void RemoveOnCloseUIListener(UnityAction action){
            if (inventory != null) inventory.RemoveOnCloseUIListener(action);
        }

        public void AddOnItemAddedListener(UnityAction<Item> action){
            if (inventory != null) inventory.AddOnItemAddedListener(action);
        }
        public void RemoveOnItemAddedListener(UnityAction<Item> action){
            if (inventory != null) inventory.RemoveOnItemAddedListener(action);
        }

        public void AddOnItemRemovedListener(UnityAction<Item> action) {
            if (inventory != null) inventory.AddOnItemRemovedListener(action);
        }
        public void RemoveOnItemRemovedListener(UnityAction<Item> action) {
            if (inventory != null) inventory.RemoveOnItemRemovedListener(action);
        }

        public void AddOnItemSelectedListener(UnityAction<Item> action){
            if(onItemSelected != null && action != null) 
                onItemSelected.AddListener(action);
        }
        public void RemoveOnItemSelectedListener(UnityAction<Item> action)
        {
            if (onItemSelected != null && action != null)
                onItemSelected.RemoveListener(action);
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
            if (onHorizontalIndexChanged != null && action != null)
               onHorizontalIndexChanged.AddListener(action);
        }
        public void RemoveOnHorizontalIndexChangedListener(UnityAction<int> action)
        {
            if (onHorizontalIndexChanged != null && action != null)
                onHorizontalIndexChanged.RemoveListener(action);
        }

        protected virtual void OnItemAdded(Item item)
        {
            if (item == null) return;
            UpdateItems();

            if (inventory == null || !inventory.IsUIOpened) return;
            UpdateModels();
        }
        protected virtual void OnItemRemoved(Item item)
        {
            if (item == null) return;
            UpdateItems();

            if (inventory == null || !inventory.IsUIOpened) return;
            UpdateModels();
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
           ClampIncrementHorizontalIndex(ref horizontalIndex, canWrapHorizontalIndex);
           if (onHorizontalIndexChanged != null) onHorizontalIndexChanged.Invoke(horizontalIndex);
        }
        private void DecrementHorizontalIndex() 
        {
            if (inventory == null) return;
            horizontalIndex--;
            ClampDecrementHorizontalIndex(ref horizontalIndex, canWrapHorizontalIndex);
            if (onHorizontalIndexChanged != null) onHorizontalIndexChanged.Invoke(horizontalIndex);
        }

        protected virtual void ClampIncrementHorizontalIndex(ref int horizontalIndex, bool canWarpHorizontalIndex) {

            int maxHorizontalIndex = GetMaxHorizontalIndex();
            
            if (horizontalIndex >= maxHorizontalIndex) {
                if(canWarpHorizontalIndex) horizontalIndex = 0;
                else                       horizontalIndex = maxHorizontalIndex - 1;
            }
        }
        protected virtual void ClampDecrementHorizontalIndex(ref int horizontalIndex, bool canWarpHorizontalIndex) 
        {
            if (horizontalIndex < 0) {
               if(canWarpHorizontalIndex) horizontalIndex = GetMaxHorizontalIndex() - 1;
               else                       horizontalIndex = 0;
            }
        }

        protected virtual void EnableInputs() {
            if (nextHorizontalPressInput != null) nextHorizontalPressInput.Enable();
            if (previousHorizontalPressInput != null) previousHorizontalPressInput.Enable();
        }
        protected virtual void DisableInputs() {
            if (nextHorizontalPressInput != null) nextHorizontalPressInput.Disable();
            if (previousHorizontalPressInput != null) previousHorizontalPressInput.Disable();
        }

        protected void SetModelParent(GameObject gameObject) {
            if(gameObject == null || modelParentTransform == null) return;
            gameObject.transform.SetParent(modelParentTransform);
            gameObject.SetActive(true);
        }

        private Inventory GetInventory()
        {
            Inventory inventory = GetComponent<Inventory>();
            if(inventory == null) return GetInventory(transform.parent);  
            return inventory;
        }

        private Inventory GetInventory(Transform parent)
        {
            Inventory inventory;
            if (parent == null) return gameObject.AddComponent<Inventory>();

            inventory = parent.GetComponent<Inventory>();
            if(inventory == null) return GetInventory(parent.parent);

            return inventory;
        }


        public async Awaitable AsyncTransition(bool isShowingModels)
        {
            if (transitionsHandler != null)
                await transitionsHandler.AsyncTransition(isShowingModels);  
        }

        public async Awaitable AwaitTransition()
        {
            if (transitionsHandler != null)
                await transitionsHandler.AwaitTransition();
        }

        public abstract void Select(Item item);
        public abstract int GetMaxHorizontalIndex();

        protected abstract void OnUpdateItemModel();

        public  abstract void UpdateItems();
        public  abstract void UpdateModels();

        public abstract int GetHorizontalIndex(Item item);
        public abstract Item GetSelectedItem();

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

            Debug.LogWarning("Current Navigator: " + current);
        }

        public static bool IsCurrent(InventoryUINavigator navigator)
        {
            if (navigator == null) return false;
            return current == navigator;
        }

        public static InventoryUINavigator GetOwned(Inventory inventory) 
        {
            if(inventory == null || navigators == null) return null;
            
            var results = navigators.Where(x => x != null).Where(x => x.inventory == inventory).ToArray();
            if (results.Count() > 0) return results.First();

            return null;
        }

        public static Inventory GetInventory(InventoryUINavigator navigator)
        {
            if (navigator == null) return null;
            return navigator.inventory;
        }

        public static OverrideablePressInput GetNextHorizontalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(NEXT_HORIZONTAL_INPUT_NAME, KeyboardKey.D, GamepadButton.DpadRight);
        }

        public static OverrideablePressInput GetPreviousHorizontalInput() {
            return InputManager.GetOrCreateOverrideablePressInput(PREVIOUS_HORIZONTAL_INPUT_NAME, KeyboardKey.A, GamepadButton.DpadLeft); ;
        }

        public static InventoryUINavigator GetComponent(Transform parent)
        {
            InventoryUINavigator navigator;
            if (parent == null) return null;

            navigator = parent.GetComponent<InventoryUINavigator>();
            if (navigator == null) return GetComponent(parent.parent);

            return navigator;
        }
    }
}
