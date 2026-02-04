namespace RedSilver2.Framework.StateMachines.States.Extensions
{
    public abstract class StateExtension : StateModule {

        protected sealed override void AddDefaultEvent()
        {
            base.AddDefaultEvent();
            stateMachine?.AddStateExtension(this);
        }

        protected sealed override void RemoveDefaultEvent()
        {
            base.RemoveDefaultEvent();
            stateMachine?.RemoveStateExtension(this);
        }

        protected abstract string GetExtensionName();
    }
}
