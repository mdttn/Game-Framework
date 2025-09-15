using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public abstract class InteractionHandlerModule : MonoBehaviour
    {
        [SerializeField] private float interactionRange;
        public float InteractionRange => interactionRange;

        private void Awake()
        {
            SetInteractionHandler(GetInteractionHandler()); 
        }

        protected abstract void Update();
        protected abstract void SetInteractionHandler(InteractionHandler handler);
        protected abstract InteractionHandler GetInteractionHandler();     
    }
}
