
using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories
{
    public class Inventory : MonoBehaviour
    {

        [Space]
        [SerializeField] private bool allowDuplicateItems;

        private UnityEvent onOpenUI, onCloseUI;
        private UnityEvent<Item> onItemAdded, onItemRemoved;

        private bool isUIOpened;
        private List<Item> items;

        private static List<Inventory> instances = new List<Inventory>();
        public bool IsUIOpened => isUIOpened;

        public static Inventory[] Instances
        {
            get
            {
                if (instances == null) return new Inventory[0];
                return instances.ToArray();
            }
        }


        protected virtual void Awake()
        {
            instances.Add(this);

            items = new List<Item>();

            onCloseUI = new UnityEvent();
            onOpenUI = new UnityEvent();

            onItemAdded = new UnityEvent<Item>();
            onItemRemoved = new UnityEvent<Item>();

            isUIOpened = false;

            AddOnOpenUIListener(OnOpenUI);
            AddOnCloseUIListener(OnCloseUI);
        }

        protected virtual void Start()
        {
            gameObject.SetActive(false);
        }

        public void Open() {
            if (onOpenUI != null) onOpenUI.Invoke();
        }

        public void Close() {
            if (onCloseUI != null) onCloseUI.Invoke();
        }

        public void AddOnOpenUIListener(UnityAction action)
        {
            Debug.Log("On Open UI: " + onOpenUI + " | Action: " + action);

            if (onOpenUI != null && action != null)
                onOpenUI.AddListener(action);
        }
        public void RemoveOnOpenUIListener(UnityAction action)
        {
            if (action != null && onOpenUI != null)
                onOpenUI.RemoveListener(action);
        }

        public void AddOnCloseUIListener(UnityAction action)
        {
            if (action != null && onCloseUI != null)
                onCloseUI.AddListener(action);
        }
        public void RemoveOnCloseUIListener(UnityAction action)
        {
            if (action != null && onCloseUI != null)
                onCloseUI.RemoveListener(action);
        }

        public void AddOnItemAddedListener(UnityAction<Item> action)
        {
            if (onItemAdded != null && action != null) {
                onItemAdded.AddListener(action);
            }
        }
        public void RemoveOnItemAddedListener(UnityAction<Item> action)
        {
            if (onItemAdded != null && action != null)
            {
                onItemAdded.RemoveListener(action);
            }
        }

        public void AddOnItemRemovedListener(UnityAction<Item> action)
        {
            if (onItemRemoved != null && action != null) {
                onItemRemoved.AddListener(action);
            }
        }
        public void RemoveOnItemRemovedListener(UnityAction<Item> action)
        {
            if (onItemRemoved != null && action != null) {
                onItemRemoved.RemoveListener(action);
            }
        }

        public virtual void AddItem(Item item, out bool isItemAdded)
        {
            isItemAdded = false;
            if (items == null || item == null) return;

            if (allowDuplicateItems) {
                if (items.Count == 0 || ContainsDuplicate(item)) items.Add(item);
            }
            else if (!Contains(item) && !ContainsDuplicate(item))
                items.Add(item);

            isItemAdded = Contains(item);
            if (isItemAdded == true && onItemAdded != null) onItemAdded.Invoke(item);
        }
        public virtual void RemoveItem(Item item, out bool isItemRemoved)
        {
            isItemRemoved = false;
            if (items == null || item == null) return;

            if (items.Contains(item)) {
                items.Remove(item);
                isItemRemoved = !Contains(item);
                if (isItemRemoved == false && onItemRemoved != null) onItemRemoved.Invoke(item);
            }
        }

        public void EnableUI()
        {
            PlayerController.Disable();
            CameraControllerModule.Disable();

            gameObject.SetActive(true);
            isUIOpened = true;
        }

        public void DisableUI()
        {
            gameObject.SetActive(false);
            isUIOpened = false;

            PlayerController.Enable();
            CameraControllerModule.Enable();
        }

        protected virtual void OnOpenUI()
        {
            Debug.Log("Open Inventory UI");
            EnableUI();
        }

        protected virtual void OnCloseUI() {
            Debug.Log("Close Inventory UI");
        }

        public int GetMaxHorizontalIndex()
        {
            if (items == null) return 0;
            return items.Count;
        }

        public bool ContainsDuplicate(Item item)
        {
            if (items == null || item == null) return false;
            return GetDuplicateCount(item) >= 1;
        }

        public bool ContainsDuplicate(string itemName)
        {
            return ContainsDuplicate(GetItem(itemName));
        }


        public int GetDuplicateCount(Item item)
        {
            if (items == null || item == null) return 0;
            return items.Where(x => x.GetType() == item.GetType()).Count();
        }

        public int GetDuplicateCount(string itemName)
        {
            return GetDuplicateCount(GetItem(itemName));
        }

        public bool Contains(Item item)
        {
            if (items == null || item == null) return false;
            return items.Contains(item);
        }

        public bool Contains(string itemName)
        {
            return Contains(GetItem(itemName));
        }

        public int GetHorizontalIndex(Item item)
        {
            if (items == null || item == null) return -1;

            for (int i = 0; i < items.Count; i++)
                if (items[i] == item) return i;

            return -1;
        }

        public int GetHorizontalIndex(string itemName)
        {
            return GetHorizontalIndex(GetItem(itemName));
        }

        public Item[] GetItems()
        {
            if (items == null) return new Item[0];
            return items.ToArray();
        }

        public Item GetItem(int index)
        {
            if (items == null || index < 0 || index >= items.Count) return null;
            return items[index];
        }

        public Item GetItem(string itemName)
        {
            if (items == null) return null;

            var results = items.Where(x => x != null).Where(x => x.name.ToLower() == itemName.ToLower());
            if (results.Count() > 0) return results.First();

            return null;
        }

        public static Inventory GetComponent(Transform root)
        {
            Inventory result;
            if(root == null) return null;

            result = root.GetComponent<Inventory>();
            if(result == null) return GetComponent(root.parent); 

            return result;
        }

        public static Inventory GetInventory(string inventoryName)
        {
            if (instances == null) return null;

            var results = instances.Where(x => x != null).Where(x => x.name.ToLower() == inventoryName.ToLower()).ToList();
            if (results.Count > 0) return results.First();

            return null;
        }
        public static Inventory GetInventory(int index)
        {
            if (instances == null || instances.Count == 0) return null;
            return instances[index];
        }

        public static Inventory GetInventoryWithItem(Item item)
        {
            if (instances == null) return null;
            var results = instances.Where(x => x != null).Where(x => x.Contains(item));
            return results.Count() > 0 ? results.First() : null;
        }
    }
}
