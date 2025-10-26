
using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories
{
    public abstract class Inventory : MonoBehaviour 
    {
        [SerializeField] private GameObject invetoryUIParent;

        [Space]
        [SerializeField] private bool allowDuplicateItems;


        [Space]
        [SerializeField] private UnityEvent onOpenUI;
        [SerializeField] private UnityEvent onCloseUI;

        [Space]
        [SerializeField] private UnityEvent<Item> onItemAdded;
        [SerializeField] private UnityEvent<Item> onItemRemoved;

        private bool isUIOpened;

        private static List<Inventory> instances = new List<Inventory>();

        protected bool AllowDuplicateItems => allowDuplicateItems;

        public bool IsUIOpened => isUIOpened;

        public static Inventory[] Instances
        {
            get
            {
                if(instances == null) return new Inventory[0];
                return instances.ToArray();
            }
        }


        protected virtual void Awake() 
        {
            instances.Add(this);

            AddOnOpenUIListener(OnOpenUI);
            AddOnCloseUIListener(OnCloseUI);

            if (invetoryUIParent != null) invetoryUIParent.SetActive(false);
            isUIOpened = false;
        }

        public void OpenUI() {
            if(onOpenUI != null) onOpenUI.Invoke(); 
        }

        public void CloseUI() {
            if(onCloseUI != null) onCloseUI.Invoke(); 
        }

        public void AddOnOpenUIListener(UnityAction action)
        {
            Debug.Log("On Open UI: " + onOpenUI  + " | Action: " + action);

            if(onOpenUI != null && action != null)
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
            if(onItemAdded != null && action != null) {
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
            if (item != null) isItemAdded = Contains(item);
            else isItemAdded = false;

            if (isItemAdded == true && onItemAdded != null)
                onItemAdded.Invoke(item);
        }
        public virtual void RemoveItem(Item item, out bool isItemRemoved) 
        {
            isItemRemoved = !Contains(item);

            if (item != null && onItemRemoved != null && isItemRemoved) {
                onItemRemoved.Invoke(item);
            }
        }

        protected virtual void OnOpenUI()
        {
            Debug.Log("Open Inventory UI");

            PlayerController.Disable();
            CameraControllerModule.Disable();
            if (invetoryUIParent != null) invetoryUIParent.SetActive(true);
            isUIOpened = true;
        }
        protected virtual void OnCloseUI()
        {
            Debug.Log("Close Inventory UI");
            if (invetoryUIParent != null) invetoryUIParent.SetActive(false);

            PlayerController.Enable();
            CameraControllerModule.Enable();

            isUIOpened = false;
        }


        public abstract bool ContainsDuplicate(Item item);
        public abstract bool ContainsDuplicate(string itemName);

        public abstract int GetDuplicateCount(Item item);
        public abstract int  GetDuplicateCount(string itemName);

        public abstract bool Contains(Item item);
        public abstract bool Contains(string itemName);

        public abstract int  GetHorizontalIndex(Item item);
        public abstract int  GetHorizontalIndex(string itemName);

        public abstract Item GetItem(string itemName);

        public static Inventory GetInventory(string inventoryName)
        {
            if (instances == null) return null;

            var results = instances.Where(x => x != null).Where(x => x.name.ToLower() ==  inventoryName.ToLower()).ToList();
            if (results.Count > 0) return results.First();

            return null;
        }
        public static Inventory GetInventory(int index)
        {
            if (instances == null) return null;
            return instances[index];
        }
    }
}
