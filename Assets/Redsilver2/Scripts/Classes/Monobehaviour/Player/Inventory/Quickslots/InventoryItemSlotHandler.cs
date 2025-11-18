using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories
{
    public class InventoryItemSlotHandler : MonoBehaviour
    {
        [SerializeField] private Transform parent;  
        private Inventory inventory;

        private List<UpdateableItem> equippedItems;
        private List<ItemSlot> slots;

        private IEnumerator leftHandTransition, rightHandTransition, bothHandTransition;

        private void Awake() {
            inventory = Inventory.GetComponent(transform);
            equippedItems = new List<UpdateableItem>();
            SetItemSlots();
        }

        private void Start() {
            SetInventoryEvents(true);
        }

        private void Update() {
            if (InputManager.GetKeyDown(KeyboardKey.H)) Equip(0);
            if (InputManager.GetKeyDown(KeyboardKey.J)) Equip(1);
            if (InputManager.GetKeyDown(KeyboardKey.K)) Equip(2);
            if (InputManager.GetKeyDown(KeyboardKey.L)) Equip(3);
        }

        private void OnDestroy() {
            SetInventoryEvents(false);
        }

        public void AddItemToSlot(UpdateableItem item)
        {
            if (slots == null || item == null || Contains(item)) return;

            for (int i = 0; i < slots.Count; i++)
                if (slots[i].item == null) {
                    Debug.LogWarning("Added " + item.name + " To Inventory Quick Slot");
                    slots[i].Set(item);
                    break;
                }

        }

        public void RemoveItemToSlot(UpdateableItem item)
        {
            if(slots == null || item == null || !Contains(item)) return;

            for (int i = 0; i < slots.Count; i++)
                if (slots[i].item == item) {
                    Debug.LogWarning("Removed " + item.name + " To Inventory Quick Slot");
                    slots[i].Set(null);
                    break;
                }
        }

        private void SetItemSlots()
        {
            slots = new List<ItemSlot>(4);

            for(int i = 0; i < 4; i++)
                slots.Add(null);

            for (int i = 0; i < slots.Count; i++)
                slots[i] = new ItemSlot(null, i > (slots.Count / 2) - 1 ? ItemHandSide.Right : ItemHandSide.Left);
        }

        private void SetInventoryEvents(bool addEvents)
        {
            if (inventory == null) return;

            if (addEvents) {
                inventory.AddOnItemRemovedListener(OnItemRemovedListener);
                inventory.AddOnItemAddedListener(OnItemAddedListener);
            }
            else
            {
                inventory.RemoveOnItemRemovedListener(OnItemRemovedListener);
                inventory.RemoveOnItemAddedListener(OnItemAddedListener);
            }
        }

        private void Equip(int index) {
            if(slots == null || index < 0 || index >= slots.Count) return;
            Equip(slots[index]);
        }

        private void Equip(ItemSlot slot)  {
            if (slot == null) return;
            UpdateableItem item = slot.item;

                 if (item == null || item == GetCurrentHandItem(slot.handSide)) StartTransition(null, slot.handSide);
            else if (item.HandType == ItemHandType.OneHanded)                   StartTransition(item, slot.handSide);
            else if (item.HandType == ItemHandType.TwoHanded)                   StartTransition(item);
        }

        public IEnumerator EquipCoroutine(UpdateableItem next, UpdateableItem current) 
        {
            Debug.LogWarning("Starting Transition! | Next: " + next + " | Current: " + current);
            yield return StartCoroutine(WaitAnimation(current, false));

            if (current != null) {
                current.gameObject.SetActive(false);
                equippedItems.Remove(current);
            }

            if (next != null){
                next.gameObject.SetActive(true);
                equippedItems.Add(next);
            }

            PlayAnimation(next, true);
            yield return StartCoroutine(WaitAnimation(next, true));
        }


        public IEnumerator EquipCoroutine(UpdateableItem next)
        {
            yield return StartCoroutine(UnequipAll());
            equippedItems.Clear();

            if (next != null)
            {
                next.gameObject.SetActive(true);
                equippedItems.Add(next);
            }

            yield return StartCoroutine(WaitAnimation(next, true));
        }

        private IEnumerator UnequipAll()
        {
            foreach(UpdateableItem item in equippedItems.Where(x => x != null)) PlayAnimation(item, false);

            while (equippedItems.Count > 0)  {
                int count = equippedItems.Where(x => x != null).Where(x => x.IsPlayingAnimation(GetAnimationName(x, false))).Count();
                if (count == 0) break;    
                yield return null;
            }
        }


        private IEnumerator WaitAnimation(UpdateableItem item, bool waitEquipAnimation)
        {
            string animationName = GetAnimationName(item, waitEquipAnimation);

            PlayAnimation(item, false);
            Debug.LogWarning(waitEquipAnimation ? "Equipping " : "Unequipping " + (item == null ? "Nothing" : item.name));


            while (item != null) {
                if (!item.IsPlayingAnimation(animationName)) break;
                yield return null;
            }
        }

        private void PlayAnimation(UpdateableItem item, bool playEquipAnimation) {
            if(item == null) return;

            string animationName = GetAnimationName(item, playEquipAnimation);
            if (parent != null) item.transform.SetParent(parent);

            if (!string.IsNullOrEmpty(animationName) && !item.IsPlayingAnimation(animationName)){
                if (playEquipAnimation) item.Equip();
                else                    item.Unequip();
            }
        }

        private string GetAnimationName(UpdateableItem item, bool getEquipAnimationName)
        {
            return item == null ? string.Empty :
                  getEquipAnimationName ? item.EquipAnimationName :
                                          item.UnequipAnimationName;
        }

        private void StartTransition(UpdateableItem next, ItemHandSide handSide) 
        {
            Debug.LogWarning("Contains : " + Contains(next) + " Is In Inventory: " + inventory.Contains(next));
            if(next != null) next.SetHand(handSide);

            StopTransition(ref bothHandTransition);
            StopTransition(handSide);

            if (handSide == ItemHandSide.Left) StartTransition(next, GetCurrentHandItem(handSide), ref leftHandTransition);
            else StartTransition(next, GetCurrentHandItem(handSide), ref rightHandTransition);
        }

        private void StartTransition(UpdateableItem item)
        {
            if(item != null) item.SetHand(ItemHandSide.Left);

            StopAllTransitions();
            bothHandTransition = EquipCoroutine(item == GetCurrentHandItem(ItemHandSide.Left) || item == GetCurrentHandItem(ItemHandSide.Right) ? null : item);
        }

        private void StartTransition(UpdateableItem next, UpdateableItem current, ref IEnumerator enumerator)
        {
            enumerator = EquipCoroutine(next, current);
            StartCoroutine(enumerator);
        }

        private void StopTransition(ItemHandSide handSide) {
            if (handSide == ItemHandSide.Left) StopTransition(ref leftHandTransition);
            else                               StopTransition(ref rightHandTransition);
        }

        private void StopTransition(ref IEnumerator enumerator)
        {
            if (enumerator != null) StopCoroutine(enumerator);
            enumerator = null;
        }



        private void StopAllTransitions() {
            StopTransition(ref bothHandTransition);
            StopTransition(ItemHandSide.Left);
            StopTransition(ItemHandSide.Right);
        }

        private void OnItemRemovedListener(Item item)
        {
            UpdateableItem updateableItem = item as UpdateableItem;
            if (updateableItem == null || slots == null) return;

            updateableItem.transform.SetParent(null);
            RemoveItemToSlot(updateableItem);
        }

        private void OnItemAddedListener(Item item)
        {
            UpdateableItem updateableItem = item as UpdateableItem;
            if (updateableItem == null || slots == null) return;

            updateableItem.transform.SetParent(parent);
            updateableItem.transform.localPosition = Vector3.zero;

            AddItemToSlot(updateableItem);
        }

        public bool Contains(Item item) {
            return slots == null ? false : slots.Where(x => x.item == item).Count() > 0;
        }

        public int GetIndex(Item item)
        {
            if (slots == null || !Contains(item)) return -1;

            for(int i = 0; i < slots.Count; i++)
                if(slots[i].item == item) return i;

            return -1;
        }

        public int[] GetItemSlotIndexes(ItemHandSide side) 
        {
            List<int> results = new List<int>();  
            if(slots == null || slots.Count == 0) return results.ToArray();

            for(int i = 0;i < slots.Count; i++)
                if (slots[i].handSide == side) results.Add(i);

            return results.ToArray();
        }

        public ItemSlot[] GetItemSlots(ItemHandSide side)
        {
            List<ItemSlot> results = new List<ItemSlot>();
            if (slots == null || slots.Count == 0) return results.ToArray();

            foreach (int i in GetItemSlotIndexes(side))
                results.Add(slots[i]);

            return results.ToArray();
        }

        public UpdateableItem GetCurrentHandItem(ItemHandSide side)
        {
            if (equippedItems == null || equippedItems.Count == 0) return null;

            if (equippedItems.Count == 1) {
                if(equippedItems[0] == null) return null;
                if (equippedItems[0].HandType == ItemHandType.TwoHanded) return slots[0].item;
            }

            foreach (ItemSlot slot in GetItemSlots(side)) {
                var results = equippedItems.Where(x => x == slot.item);
                if(results.Count() > 0) return results.First();
            }

            return null;
        }

        public bool TryGetEquippedItem(Item item, out Item result)
        {
            for(int i = 0;i < equippedItems.Count; i++) {
                if (equippedItems[i] == item) {
                    result = equippedItems[i];
                    return true;
                }
            }

            result = null;
            return false;
        }
        
    }
}
