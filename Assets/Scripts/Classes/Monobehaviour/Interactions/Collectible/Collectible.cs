using UnityEngine;

namespace RedSilver2.Framework.Interactions.Collectibles
{
    public abstract class Collectible : MonoBehaviour
    {
        private InteractionModule interactionModule;

        private void Awake()
        {
            interactionModule = GetComponent<InteractionModule>();
            CollectibleModelViewer.AddCollectibleModel(GetData());
        }

        private void Start()
        {
            if (interactionModule != null) { interactionModule.AddOnInteractListener(OnInteract); }
        }

        private void OnEnable()
        {
            if (interactionModule != null) { interactionModule.AddOnInteractListener(OnInteract); }
        }

        private void OnDisable()
        {
            if (interactionModule != null) { interactionModule.RemoveOnInteractListener(OnInteract); }
        }

        protected virtual void OnInteract()
        {
            CollectibleNotificationManager collectibleNotification = GameManager.Instance.CollectibleNotification;
            if(collectibleNotification != null) collectibleNotification.Notify(this);
        }


        public abstract CollectibleData GetData();
    }
}