using UnityEngine;

namespace RedSilver2.Framework.Interactions.Collectibles
{
    public abstract class Collectible : MonoBehaviour
    {
        private InteractionModule interactionModule;

        private void Awake() {
            interactionModule = GetComponent<InteractionModule>();
            CollectibleModelViewer.AddCollectibleModel(GetData());
        }

        private void Start() {
            SetInteractionModuleEvents(true);
        }

        private void OnEnable() {
            SetInteractionModuleEvents(true);
        }

        private void OnDisable() {
            SetInteractionModuleEvents(false);
        }

        private void SetInteractionModuleEvents(bool isAddingEvents) 
        {
            if (interactionModule == null) return;

            if (interactionModule is SingleInteractionModule)
                SetInteractionModuleEvents(interactionModule as SingleInteractionModule, isAddingEvents);
            else if (interactionModule is TimedHoldInteractionModule)
                SetInteractionModuleEvents(interactionModule as TimedHoldInteractionModule, isAddingEvents);
        }

        private void SetInteractionModuleEvents(SingleInteractionModule module, bool isAddingEvents) {
            if (module == null) return;

            if (isAddingEvents)
                module.AddOnInteractListener(OnInteract);
            else
                module.RemoveOnInteractListener(OnInteract);
        }

        private void SetInteractionModuleEvents(TimedHoldInteractionModule module, bool isAddingEvents) {

            if (module == null) return;

            if (isAddingEvents)
                module.AddOnInteractListener(OnInteract);
            else
                module.RemoveOnInteractListener(OnInteract);
  
        }

        protected virtual void OnInteract()  {
            CollectibleNotificationManager collectibleNotification = GameManager.GetCollectibleNotification();
            if(collectibleNotification != null) collectibleNotification.Notify(this);
           
            if (interactionModule != null) interactionModule.enabled = false;
            gameObject.SetActive(false);
        }

        protected void OnInteract(float progression) {
            if(progression >= 1f) 
                OnInteract();
        }


        public abstract CollectibleData GetData();
    }
}