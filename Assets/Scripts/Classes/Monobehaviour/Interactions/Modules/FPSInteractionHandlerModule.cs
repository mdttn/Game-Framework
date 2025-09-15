using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public class FPSInteractionHandlerModule : InteractionHandlerModule
    {
        private FPSInteractionHandler handler;
        public FPSInteractionHandler Handler => handler;

        protected sealed override void Update()
        {
            if (handler != null) handler.Update();   
        }

        protected sealed override void SetInteractionHandler(InteractionHandler handler)
        {
            this.handler = handler as FPSInteractionHandler;
        }

        protected sealed override InteractionHandler GetInteractionHandler()
        {
            if (handler != null) return handler;
            return new FPSInteractionHandler(this);
        }

    }
}
