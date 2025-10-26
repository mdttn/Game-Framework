using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class ItemModelsPositionUpdater : InventoryUI
    {
        [Space]
        [SerializeField] private Transform modelsParent;

        [Space]
        [SerializeField] private ItemModelsDisplayer modelsDisplayer;

        [Space]
        [SerializeField] private bool constantUIUpdate = true;

        protected override void Awake()
        {
            if(navigator != null) {
                if (constantUIUpdate)
                    navigator.AddOnLateUpdateListener(OnLateUpdateEvent);
                else
                    navigator.AddOnEnableListener(OnEnableEvent);
            }
        }

        private void OnLateUpdateEvent() {
            UpdateModelsPosition(navigator, modelsDisplayer);
        }

        private void OnEnableEvent() {

        }

        private void UpdateModelsPosition(InventoryUINavigator navigator, ItemModelsDisplayer modelsDisplayer) 
        {
            if (navigator == null || modelsDisplayer == null) return;

            if (navigator is SimpleInventoryUINavigator)
                UpdateModelsPosition(navigator as SimpleInventoryUINavigator, modelsDisplayer);
            else if (navigator is HorizontalInventoryUINavigator)
                UpdateModelsPosition(navigator as HorizontalInventoryUINavigator, modelsDisplayer);
            else if (navigator is VerticalInventoryUINavigator)
                UpdateModelsPosition(navigator as VerticalInventoryUINavigator, modelsDisplayer);           
        }

        private void UpdateModelsPosition(SimpleInventoryUINavigator navigator, ItemModelsDisplayer modelsDisplayer) 
        {
            if (navigator == null || modelsDisplayer == null) return;
            UpdateModelsPosition(navigator.HorizontalIndex, modelsDisplayer.VisibleItemModels);
        }

        private void UpdateModelsPosition(HorizontalInventoryUINavigator navigator, ItemModelsDisplayer modelsDisplayer) 
        {
            if (navigator == null || modelsDisplayer == null) return;
            UpdateModelsPosition(navigator.HorizontalIndex, modelsDisplayer.VisibleItemModels);
        }

        private void UpdateModelsPosition(VerticalInventoryUINavigator navigator, ItemModelsDisplayer modelsDisplayer) 
        {
            if (navigator == null || modelsDisplayer == null) return;
            UpdateModelsPosition(navigator.HorizontalIndex, navigator.VerticalIndex, 
            navigator.Inventory as ComplexInventory, modelsDisplayer.VisibleItemModels);
        }

        protected virtual void UpdateModelsPosition(int horizontalIndex, GameObject[] models) {
            SetModelsParent(models);
        }

        protected virtual void UpdateModelsPosition(int horizontalIndex, int verticalIndex, ComplexInventory inventory, GameObject[] models)
        {
            SetModelsParent(models);
        }

        private void SetModelsParent(GameObject[] models) 
        {
            if(models == null || modelsParent == null || models.Length == 0) return;

            foreach (GameObject model in models.Where(x => x != null)) {
                model.transform.SetParent(modelsParent);
            }
        }
    }
}
