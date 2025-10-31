using UnityEngine.Events;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public abstract class InventoryPageDisplayer : InventoryUI
    {
        [SerializeField] private string messageToDisplay;
        [SerializeField] private string nullMessage;

        protected sealed override void Start()
        {
            SetNavigatorEvents(true);
        }

        protected sealed override void OnDestroy()
        {
            SetNavigatorEvents(false);
        }

        private void SetNavigatorEvents(bool addEvents)
        {
            if (navigator == null || navigator is not PageInventoryUINavigator) return;

            if (addEvents) AddNavigatorEvents   (navigator as PageInventoryUINavigator);
            else           RemoveNavigatorEvents(navigator as PageInventoryUINavigator);
        }

        private void AddNavigatorEvents(PageInventoryUINavigator navigator)
        {
            if(navigator != null)
            {
                navigator.AddOnOpenUIListener(GetOnOpenUIAction(navigator));
                navigator.AddOnPageIndexChangedListener(GetOnPageIndexChangedAction(navigator));
            }
        }

        private void RemoveNavigatorEvents(PageInventoryUINavigator navigator)
        {
            if (navigator != null) {
                navigator.RemoveOnOpenUIListener(GetOnOpenUIAction(navigator));
                navigator.RemoveOnPageIndexChangedListener(GetOnPageIndexChangedAction(navigator));
            }
        }

        private UnityAction GetOnOpenUIAction(PageInventoryUINavigator navigator)
        {
            if(navigator == null) return null;

            return () =>
            {
                DisplayMessage(navigator);
            };
        }

        private UnityAction<int> GetOnPageIndexChangedAction(PageInventoryUINavigator navigator)
        {
            if (navigator == null) return null;

            return pageIndex =>
            {
                DisplayMessage(navigator);
            };
        }

        private void DisplayMessage(PageInventoryUINavigator navigator)
        {
            Debug.LogWarning(GetMessage(navigator.PageIndex, navigator.MaxPages));

            if(navigator != null)
                DisplayMessage(GetMessage(navigator.PageIndex, navigator.MaxPages));
            else 
                DisplayNullMessage();
        }

        protected void DisplayNullMessage() {
            DisplayMessage(nullMessage);
        }

        private string GetMessage(int pageIndex, int maxPageIndex)
        {
            return messageToDisplay.Replace("{0}", $"{pageIndex + 1}")
                                   .Replace("{1}", $"{maxPageIndex}");
        }

        protected abstract void DisplayMessage(string message);



    }
}
