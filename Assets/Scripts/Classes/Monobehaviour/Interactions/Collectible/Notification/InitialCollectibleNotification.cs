using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Interactions.Collectibles
{
    public abstract class InitialCollectibleNotification : CollectibleNotification
    {
        [SerializeField] private GameObject             uiParent;
                         private CollectibleModelViewer modelViewer;

        protected override void Awake()
        {
            base.Awake();   
            modelViewer = GetComponentInChildren<CollectibleModelViewer>(true);
            if(uiParent != null) uiParent.SetActive(false);
        }

        protected abstract void SetInformationsText(CollectibleData collectibleData);

        private void SetViewModel(CollectibleData data, bool isShowingModel)
        {
            if (modelViewer != null)
            {
                if (isShowingModel)
                    modelViewer.ShowModel(GetDataModel(data));
                else
                    modelViewer.HideModel();
            }
        }

        protected override void OnNotificationShown(Collectible collectible)
        {
            base.OnNotificationShown(collectible);

            if (collectible != null)
            {
                CollectibleData data = collectible.GetData();

                SetInformationsText(data);
                if (uiParent != null) uiParent.SetActive(true);

                SetViewModel(data, true);
            }
        }

        protected override void OnNotificationHid(Collectible collectible)
        {
            if (collectible != null)
            {
                SetViewModel(collectible.GetData(), false);
                if (uiParent != null) uiParent.SetActive(false);
            }
        }

        protected sealed override IEnumerator DisplayNotification(Collectible collectible)
        {
            GameManager manager = GameManager.Instance;

            if (manager != null)
            {
                CollectibleNotificationManager notificationManager = GameManager.GetCollectibleNotificationManager();
                if(notificationManager != null) yield return StartCoroutine(notificationManager.CloseInitialNotification());
            }
        }
    }
}
