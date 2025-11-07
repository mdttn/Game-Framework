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
            SetNavigatorEvents(true);
            DisplayNullMessage();
        }

        protected override void OnDestroy() 
        {
            SetNavigatorEvents(false);
        }

        private void SetNavigatorEvents(bool addEvents)
        {
            if (navigator == null) return;

            if (addEvents) {
                navigator.AddOnTransitionsFinishedListener(OnTransitionsFinished);
                navigator.AddOnTransitionsStartedListener(OnTransitionsStarted);

                navigator.AddOnOpenUIListener(OnOpenUI);
                navigator.AddOnCloseUIListener(OnCloseUI);
                navigator.AddOnItemSelectedListener(DisplayItemInformation);
            }
            else {
                navigator.RemoveOnTransitionsFinishedListener(OnTransitionsFinished);
                navigator.RemoveOnTransitionsStartedListener(OnTransitionsStarted);

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

        protected abstract void OnTransitionsStarted();
        protected abstract void OnTransitionsFinished();
    }
}
