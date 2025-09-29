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

        public void Interact()
        {
            if (interactionModule != null) { interactionModule.Interact(); }
        }

        protected virtual void OnInteract()
        {
            CollectibleNotificationManager notificationManager = GameManager.GetCollectibleNotificationManager();
            if (notificationManager != null) notificationManager.Notify(this);
        }


        public abstract CollectibleData GetData();
    }
}