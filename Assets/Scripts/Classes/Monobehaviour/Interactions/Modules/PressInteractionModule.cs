namespace RedSilver2.Framework.Interactions
{
    public class PressInteractionModule : InteractionModule
    {
        public sealed override void Interact(InteractionHandler handler)
        {
            if(handler != null)
                if (handler.IsInputPressed)
                    Interact();
        }
    }
}
