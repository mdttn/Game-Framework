using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class InventoryUI : MonoBehaviour
    {
        [SerializeField] protected InventoryUINavigator navigator;

        protected virtual void Awake() 
        {
            if(navigator == null) {
                navigator.AddOnHorizontalIndexChangedListener(OnHorizontalIndexChanged);
                
                if(navigator is ComplexInventoryUINavigator)
                    (navigator as ComplexInventoryUINavigator).AddOnVerticalIndexChangedListener(OnVerticalIndexChanged); 
            }
        }

        protected abstract void OnHorizontalIndexChanged(int horizontalIndex);
        protected abstract void OnVerticalIndexChanged(int verticalIndex);
    }
}