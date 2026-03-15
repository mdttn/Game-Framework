using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories
{
    public class EquippableInventory : Inventory
    {
        [Space]
        [SerializeField] private int defaultSlotsCount;

        [Space]
        [SerializeField] private Transform leftHand;

        private int  selectedSlotIndex;
        private bool canUpdateSlots;

        private List<ItemSlot> slots;

        private UnityEvent<EquippableItem> onItemEquipped, onItemUnequipped;
        private UnityEvent<ItemSlot> onSlotSelected, onSlotDeselected, onSlotUpdated;

        private EquippableItem leftHandItem;
        private IEnumerator    leftHandCoroutine;

        private readonly GamepadButton previousShortcut = GamepadButton.DpadLeft;
        private readonly GamepadButton nextShortcut     = GamepadButton.DpadRight;

        public ItemSlot[] Slots {
            get
            {
                if (slots == null) return null;
                return slots.ToArray();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            slots = new List<ItemSlot>();
            AddSlots(defaultSlotsCount);

            selectedSlotIndex = -1;
            canUpdateSlots    = true;

            onItemEquipped   = new UnityEvent<EquippableItem>();
            onItemUnequipped = new UnityEvent<EquippableItem>();

            onSlotDeselected = new UnityEvent<ItemSlot>();
            onSlotSelected   = new UnityEvent<ItemSlot>();
            onSlotUpdated    = new UnityEvent<ItemSlot>();
        }

        protected void Update() {
            UpdateShortcuts();
        }

        public void AddOnItemEquippedListener(UnityAction<EquippableItem> action)
        {
            if (action != null)
                onItemEquipped?.AddListener(action);
        }
        public void RemoveOnItemEquippedListener(UnityAction<EquippableItem> action)
        {
            if (action != null)
                onItemEquipped?.RemoveListener(action);
        }

        public void AddOnItemUnEquippedListener(UnityAction<EquippableItem> action)
        {
            if (action != null)
                onItemUnequipped?.AddListener(action);
        }
        public void RemoveOnItemUnEquippedListener(UnityAction<EquippableItem> action)
        {
            if (action != null)
                onItemUnequipped?.RemoveListener(action);
        }

        public void AddOnSlotSelectedListener(UnityAction<ItemSlot> action)
        {
            if (action != null)
                onSlotSelected?.AddListener(action);
        }

        public void RemoveOnSlotSelectedListener(UnityAction<ItemSlot> action)
        {
            if (action != null)
                onSlotSelected?.RemoveListener(action);
        }

        public void AddOnSlotDeselectedListener(UnityAction<ItemSlot> action)
        {
            if (action != null)
                onSlotDeselected?.AddListener(action);
        }
        public void RemoveOnSlotDeselectedListener(UnityAction<ItemSlot> action)
        {
            if (action != null)
                onSlotDeselected?.RemoveListener(action);
        }

        public void AddOnSlotUpdatedListener(UnityAction<ItemSlot> action)
        {
            if (action != null)
                onSlotUpdated?.AddListener(action);
        }
        public void RemoveOnSlotUpdatedListener(UnityAction<ItemSlot> action)
        {
            if (action != null)
                onSlotUpdated?.RemoveListener(action);
        }

        private void AddSlot() {
            AddSlots(1);
        }

        private void AddSlots(int slotCount) {

            for (int i = 0; i < slotCount; i++) {
                if (slots.Count >= 9) break;

                ItemSlot slot = new ItemSlot((KeyboardKey)((int)(KeyboardKey.Alpha1) + i), i);
                slots.Add(slot);

                slot.AddOnSelectedListener(() => {
                    onSlotSelected?.Invoke(slot);
                    Debug.Log("Selecting");
                    Equip(slot);
                });
                slot.AddOnDeselectedListener(() => {
                    onSlotDeselected?.Invoke(slot);
                    Debug.Log("Deselecting");
                    UnEquip(slot);
                });

                slot.AddOnUpdatedListener(() => {
                    onSlotUpdated?.Invoke(slot);
                    UnEquip(slot);
                });
            }
        }

        private void RemoveSlot() {
            RemoveSlots(1);
        }

        private void RemoveSlots(int slotCount)
        {
            if (slots == null) return;

            for (int i = 0; i < slotCount; i++) {
                if (slots.Count <= 0) break;
                slots?.RemoveAt(slots.Count - 1);
            }
        }

        private void UpdateShortcuts()
        {
            int result = -1;

            if (TryUpdateGamepadShortcut(slots.Count, out result)) {
                SelectSlot(result);
            }
            else if (TryUpdateSlotsShorcuts(out result, out bool isSelecting))
            {
                if (isSelecting) SelectSlot(result);
                else             DeselectSlot();
            }
        }

        private bool TryUpdateSlotsShorcuts(out int result, out bool isSelecting)
        {
            result = -1;
            isSelecting = false;
            if (slots == null || !canUpdateSlots) return false;

            foreach (ItemSlot slot in slots) {
                if (slot.TryUpdateShortcut(slots.Count, selectedSlotIndex, out isSelecting)) {
                    result = slot.index;
                    return true;
                }
            }

            return false;
        }


        private bool TryUpdateGamepadShortcut(int maxSlotIndex, out int result)
        {
            result = -1;

            if (InputManager.GetKeyDown(previousShortcut)) {
                DecrementSelectedIndex(maxSlotIndex, out result);
                return true;
            }
            else if (InputManager.GetKeyDown(nextShortcut)) {
                IncrementSelectedIndex(maxSlotIndex, out result);
                return true;
            }

            return false;
        }



        private void IncrementSelectedIndex(int maxSlotIndex, out int result)
        {
            result = selectedSlotIndex + 1;
            if (result > maxSlotIndex - 1) result = 0;
        }

        private void DecrementSelectedIndex(int maxSlotIndex, out int result)
        {
            result = selectedSlotIndex - 1;
            if (result < 0) result = maxSlotIndex - 1;
        }


        public void Equip(int index)
        {
            Equip(GetItem(index) as EquippableItem);
        }

        public void Equip(ItemSlot slot) {
            if (slot != null && ContainsSlot(slot))
                Equip(slot.Item);
        }

        public void Equip(EquippableItem item, ItemHandSide handSide = ItemHandSide.Left) {
            if (item == null || !Contains(item) || IsEquippedItem(item)) return;

            switch (handSide) {
                case ItemHandSide.Left:
                    if (leftHandCoroutine != null) StopCoroutine(leftHandCoroutine);
                    leftHandCoroutine = EquipLeftItemCoroutine(item);
                    StartCoroutine(leftHandCoroutine);
                    break;
            }
        }

        public void UnEquip(ItemSlot slot)
        {
            if (slot != null && ContainsSlot(slot))
                UnEquip(slot.Item);
        }

        public void UnEquip(EquippableItem item, ItemHandSide handSide = ItemHandSide.Left) {
            if (item == null || !Contains(item) || !IsEquippedItem(item)) return;


            switch (handSide)
            {
                case ItemHandSide.Left:
                    if(leftHandCoroutine != null) StopCoroutine(leftHandCoroutine);
                    leftHandCoroutine = UnequipLeftItemCoroutine();
                    Debug.Log(leftHandCoroutine);
                    StartCoroutine(leftHandCoroutine);
                break;
            }
        }

        private void StartEnumerator(ref IEnumerator current, IEnumerator newEnumerator)
        {
            if (current != null) StopCoroutine(current);

            current = newEnumerator;
            StartCoroutine(current);
        }

        private IEnumerator EquipLeftItemCoroutine(EquippableItem newItem)
        {
            if (leftHandItem != newItem)
                yield return StartCoroutine(UnequipLeftItemCoroutine());
            newItem?.Equip();
            SetHandSideItem(newItem, ItemHandSide.Left);

            if(newItem) {
                yield return new WaitForSeconds(newItem.AnimationLenght);
            }
        }

        private IEnumerator EquipRightItemCoroutine(EquippableItem newItem)
        {
            yield return null;
        }


        private IEnumerator EquipMiddleItemCoroutine(EquippableItem newItem)
        {
            StopCoroutine(leftHandCoroutine);

            yield return null;
        }



        private IEnumerator UnequipLeftAndRightItemCoroutine()
        {
            if (leftHandItem) leftHandItem?.UnEquip();
            // Add Right Side Item..

            while (true) {
                // Add Right Side Item..
               // if (!IsAnimationPlaying(leftHandItem)) break;
                yield return null;
            }


            leftHandItem?.SetMeshRenderersVisibility(false);


            SetHandSideItem(null, ItemHandSide.Left);
            SetHandSideItem(null, ItemHandSide.Right);
        }

        private IEnumerator UnequipLeftItemCoroutine()
        {
            Debug.Log("Deselecting 2");
            if (leftHandItem != null)
            {
                leftHandItem?.UnEquip();
                yield return new WaitForSeconds(leftHandItem.AnimationLenght);

                leftHandItem?.SetMeshRenderersVisibility(false);
                SetHandSideItem(null, ItemHandSide.Left);
            }
        }


        private IEnumerator UnequipRightItemCoroutine()
        {
            yield return null;  
        }

        private IEnumerator UnequipMiddleItemCoroutine()
        {
            yield return null;
        }

        private void SetHandSideItem(EquippableItem item, ItemHandSide handSide)
        {
            if (IsEquippedItem(item)) return;

            switch (handSide)
            {
                case ItemHandSide.Left: 
                    leftHandItem = item; 
                    break;

                case ItemHandSide.Right:  break;

                case ItemHandSide.Middle:
                leftHandItem = null;
                break;
            }
        }


        public void UpdateItemSlot(EquippableItem item, int index)
        {
            if (slots == null || slots.Count == 0 || index < 0 || index >= slots.Count)
                return;

            if (slots[index].Item != item)
                slots[index].Update(item);
        }

        private void SelectSlot(int index)
        {

            if (slots == null || slots.Count == 0 || index < 0 || index >= slots.Count)
                return;

            DeselectSlot();

            if (slots[index] != null) {
                selectedSlotIndex = index;
                slots[selectedSlotIndex]?.Select();
            }
        }

        private void DeselectSlot() {

            if (slots == null || slots.Count == 0 || selectedSlotIndex < 0 || selectedSlotIndex >= slots.Count)
            {
                selectedSlotIndex = -1;
                return;
            }

            slots[selectedSlotIndex]?.Deselect();
            selectedSlotIndex = -1;
        }



        protected override void OnItemAdded(Item item)
        {
            OnItemAdded(item as EquippableItem);
        }

        protected override void OnItemRemoved(Item item)
        {
            OnItemRemoved(item as EquippableItem);
        }

        protected virtual void OnItemAdded(EquippableItem item)
        {
            if (item == null) return;


            item?.SetTransformParent(leftHand);
            AddItemToFreeSlot(item);
        }

        protected virtual void OnItemRemoved(EquippableItem item)
        {
            if (item == null) return;

            item?.SetTransformParent(null);
            RemoveItemFromSlot(item);
        }

        protected virtual void SwapItemSlots(int newIndex, int oldIndex) {

        }

        protected virtual void AddItemToFreeSlot(EquippableItem item)
        {
            if (item == null || ContainsInSlots(item)) return;
            ItemSlot[] results = GetEmptySlots();

            if (results.Length > 0) results.First().Update(item);
        }

        protected virtual void RemoveItemFromSlot(EquippableItem item)
        {
            ItemSlot slot = GetSlot(item);
            slot?.Update(null);
        }

        public ItemSlot GetSlot(int index) {
            if (slots == null || slots.Count == 0 || index < 0 || index >= slots.Count)
                return null;

            return slots[index];
        }

        public ItemSlot GetSlot(EquippableItem item)
        {
            if (slots == null || slots.Count == 0 || item == null) return null;

            for (int i = 0; i < slots.Count; i++)
                if (slots[i] != null)
                    if (slots[i].Item == item)
                        return slots[i];

            return null;
        }

        public ItemSlot[] GetEmptySlots() {
            if (slots == null || slots.Count == 0) return new ItemSlot[0];
            return slots.Where(x => x != null).Where(x => x.Item == null).ToArray();
        }


        public int GetSlotIndex(ItemSlot slot)
        {
            if (slots == null || slots.Count == 0) return -1;

            for (int i = 0; i < slots.Count; i++)
                if (slots[i] == slot) return i;

            return -1;
        }

        public int GetSlotsIndex(EquippableItem item) {
            if (slots == null || slots.Count == 0) return -1;

            for (int i = 0; i < slots.Count; i++) {
                if (slots[i] == null) continue;
                else if (slots[i].Item == item) return i;
            }

            return -1;
        }

        public int[] GetEmptySlotsIndex()
        {
            List<int> results = new List<int>();
            if (slots == null || slots.Count == 0) return new int[0];

            for (int i = 0; i < slots.Count; i++) {
                ItemSlot slot = slots[i];

                if (slots[i] == null) continue;
                else if (slots[i].Item == null) results.Add(i);
            }

            return results.ToArray();
        }

        public bool ContainsSlot(ItemSlot slot) {
            return ContainsSlot(GetSlotIndex(slot));
        }

        public bool ContainsSlot(int index)
        {
            if (slots == null || slots.Count == 0 || index < 0 || index >= slots.Count)
                return false;

            if (GetSlot(index) != null) return true;
            return false;
        }

        public bool ContainsInSlots(EquippableItem item) {
            if (slots == null || item == null || slots.Count == 0) return false;
            return slots.Where(x => x != null).Where(x => x.Item == item).Count() > 0;
        }

        public bool IsEquippedItem(EquippableItem item)
        {
            if     (item         == null) return false;
            else if(leftHandItem == item) return true;
            return false;
        }
    }
}