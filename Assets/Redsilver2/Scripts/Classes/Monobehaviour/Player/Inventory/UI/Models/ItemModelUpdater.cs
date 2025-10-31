using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class ItemModelUpdater : InventoryUI
    {
        protected override void Start(){
            SetNavigatorEvents(true);
        }

        protected override void OnDestroy() {
            SetNavigatorEvents(false);
        }

        private void SetNavigatorEvents(bool addEvents)
        {
            if(navigator is SimpleInventoryUINavigator)
                SetNavigatorEvents(navigator as SimpleInventoryUINavigator, addEvents);
            else if (navigator is VerticalInventoryUINavigator)
                SetNavigatorEvents(navigator as VerticalInventoryUINavigator, addEvents);
        }

        private void SetNavigatorEvents(SimpleInventoryUINavigator navigator, bool addEvents)
        {
            if (navigator == null) return;

            if (!addEvents)
                navigator.RemoveOnUpdateModelListener(UpdateModels);
            else
                navigator.AddOnUpdateModelListener(UpdateModels);
        }
        private void SetNavigatorEvents(VerticalInventoryUINavigator navigator, bool addEvents)
        {
            if (navigator == null) return;

            if (!addEvents)
                navigator.RemoveOnUpdateModelListener(UpdateModels);
            else 
                navigator.AddOnUpdateModelListener(UpdateModels);
        }

        protected abstract void UpdateModels(int horizontalIndex, GameObject model, SimpleInventoryUINavigator navigator);
        protected abstract void UpdateModels(int verticalIndex, int horizontalIndex, GameObject model, VerticalInventoryUINavigator navigator);
    }
}
