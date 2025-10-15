using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public abstract class InteractionModule : MonoBehaviour
    {
        [SerializeField] private string interactionName;
        [SerializeField] private UnityEvent onInteract;

        public string InteractionName => interactionName;

        protected virtual void Awake()
        {
            onInteract = new UnityEvent();
            InteractionHandler.AddInteractionModuleInstance(GetComponent<Collider>(), this);
        }

        public void AddOnInteractListener(UnityAction action)
        {
            if(onInteract != null && action != null) onInteract.AddListener(action);        
        }

        public void RemoveOnInteractListener(UnityAction action)
        {
            if (onInteract != null && action != null) onInteract.RemoveListener(action);
        }

        public void Interact() { if (onInteract != null) onInteract.Invoke(); }
        public abstract void Interact(InteractionHandler handler);
    }
}
