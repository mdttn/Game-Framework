namespace RedSilver2.Framework.Interactions
{
    public class ReleaseInteractionModule : InteractionModule
    {
        public sealed override void Interact(InteractionHandler handler)
        {
            if(handler != null)
            {
                if (handler.IsInputReleased)
                {
                    Interact();
                }
            }
        }
    }
}
