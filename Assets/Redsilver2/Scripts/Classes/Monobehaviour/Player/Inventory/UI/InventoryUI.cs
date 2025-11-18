using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class InventoryUI : MonoBehaviour
    {
        protected InventoryUINavigator navigator;

        protected virtual void Awake() {
            navigator = InventoryUINavigator.GetComponent(transform);
        }

        protected abstract void Start();
        protected abstract void OnDestroy();
    }
}