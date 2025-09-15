namespace RedSilver2.Framework.Interactions
{
    public class HoldInteractionModule : InteractionModule
    {
        public override void Interact(InteractionHandler handler)
        {
            if(handler != null)
                if (handler.IsInputHeld)
                    Interact();
        }
    }
}
