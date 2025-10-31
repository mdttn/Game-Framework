using RedSilver2.Framework.Interactions.Items;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class ItemInformationDisplayer : InventoryUI
    {
        [Space]
        [SerializeField] protected string nullErrorMessage;

        protected override void Start()
        {
            if (navigator != null) {
                navigator.AddOnOpenUIListener(OnOpenUI);
                navigator.AddOnCloseUIListener(OnCloseUI);
                navigator.AddOnItemSelectedListener(DisplayItemInformation);
            }

            DisplayNullMessage();
        }

        protected override void OnDestroy() 
        {
            if (navigator != null) {
                navigator.RemoveOnOpenUIListener(OnOpenUI);
                navigator.RemoveOnCloseUIListener(OnCloseUI);
                navigator.RemoveOnItemSelectedListener(DisplayItemInformation);
            }
        }

        private void OnCloseUI()
        {
            DisplayItemInformation(string.Empty);
        }
        private void OnOpenUI() {
            if (navigator == null) return;
            DisplayItemInformation(navigator.GetSelectedItem());
        }
        private void DisplayItemInformation(Item item) 
        {
            if (item != null)
                DisplayItemInformation(item.GetData() as ItemData);
            else
                DisplayNullMessage();
        }

        protected void DisplayNullMessage() {
            DisplayItemInformation(nullErrorMessage);
        }
        protected abstract void DisplayItemInformation(ItemData data);
        protected abstract void DisplayItemInformation(string message);
    }
}
