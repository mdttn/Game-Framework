using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public class SimpleInventoryUINavigator : InventoryUINavigator
    {
        private UnityEvent<GameObject[]> onModelsChanged;
        private UnityEvent<int, GameObject, SimpleInventoryUINavigator> onUpdateModel;

        private Item[]   items;
        private GameObject[] models;

        public Item[] Items
        {
            get
            {
                if (items == null) return new Item[0];
                return items;
            }
        }

        public GameObject[] Models => models;

        protected override void Awake()
        {
            base.Awake();
            items = new Item[0];

            onModelsChanged = new UnityEvent<GameObject[]>();
            onUpdateModel   = new UnityEvent<int, GameObject, SimpleInventoryUINavigator>();
        }

        protected override void Start()
        {
            AddOnModelsChangedListener(OnModelsChanged);
            base.Start();
        }

        public void AddOnUpdateModelListener(UnityAction<int, GameObject, SimpleInventoryUINavigator> action)
        {
            if (onUpdateModel != null && action != null)
                onUpdateModel.AddListener(action);
        }
        public void RemoveOnUpdateModelListener(UnityAction<int, GameObject, SimpleInventoryUINavigator> action)
        {
            if (onUpdateModel != null && action != null)
                onUpdateModel.RemoveListener(action);
        }

        public void AddOnModelsChangedListener(UnityAction<GameObject[]> action)
        {
            if (onModelsChanged != null && action != null)
                onModelsChanged.AddListener(action);
        }
        public void RemoveOnModelsChangedListener(UnityAction<GameObject[]> action)
        {
            if (onModelsChanged != null && action != null)
                onModelsChanged.RemoveListener(action);
        }

        public bool DoesModelExist(int index)
        {
            if (models == null || models.Length == 0 || index < 0 || index >= models.Length) return false;
            return models[index] != null;
        }

        private void OnModelsChanged(GameObject[] models) {
            if (models == null) return;

            foreach (GameObject model in models.Where(x => x != null))
                     SetModelParent(model);
        }

        public sealed override void UpdateItems()
        {
            items = inventory.GetItems();
        }

        public sealed override void Select(Item item)
        {
            if (inventory == null || item == null) return;
            if (inventory.Contains(item)) horizontalIndex = GetHorizontalIndex(item);
        }


        public sealed override int GetMaxHorizontalIndex()
        {
            if(inventory != null) return inventory.GetMaxHorizontalIndex();
            return -1;
        }

        public sealed override void ClearModels()
        {
            ItemModel.ReturnBorrowedModels(models);
            models = new GameObject[0];
        }

        protected sealed override void OnItemAdded(Item item)
        {
            if (items != null && item != null){
                UpdateItems();
            }
        }

        protected sealed override void OnItemRemoved(Item item) 
        {
            if (items != null && item != null) {
                UpdateItems();
            }
        }

        public sealed override void UpdateModels()
        {
            ItemModel.ReturnBorrowedModels(models);
            models = ItemModel.GetModels(items.ToArray());
            if(onModelsChanged != null && models != null) onModelsChanged.Invoke(models);
        }

        public GameObject GetModel(int horizontalIndex)
        {
            if(models == null || models.Length == 0 || horizontalIndex < 0 || horizontalIndex >= models.Length)
                return null;

            return models[horizontalIndex];
        }

        public Item[] GetItems()
        {
            if (items == null) return new Item[0];
            return items.ToArray();
        }

        public override Item GetSelectedItem()
        {
            if(items == null || horizontalIndex <= 0 || horizontalIndex >= items.Length) return null;
            return items[horizontalIndex];
        }

        protected  override void OnUpdateItemModel()
        {
            if (models == null) return;

            for (int i = 0; i < models.Length; i++)
                if (onUpdateModel != null && models[i] != null)
                    onUpdateModel.Invoke(i, models[i], this);
        }

        public override int GetHorizontalIndex(Item item)
        {
            if (item == null) return -1;

            for(int i = 0; i < items.Length; i++)
                if(items[i] == item) return i;

            return -1;
        }
    }
}
