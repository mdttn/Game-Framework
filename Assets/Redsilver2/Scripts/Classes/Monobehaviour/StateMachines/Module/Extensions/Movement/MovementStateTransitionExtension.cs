
using RedSilver2.Framework.StateMachines.States.Extensions;
using System.Linq;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementStateTransitionExtension : MovementStateExtension, IStateTransition
    {
        private MovementStateType[] validResultStateTypes;

        protected sealed override void Awake()
        {
            validResultStateTypes = GetValidResultStateTypes();
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            stateMachine?.AddOnStateExtensionAddedListener(OnStateExtensionAdded);
        }

        protected override void OnEnable() {
            base.OnEnable();
            stateMachine?.AddOnStateExtensionAddedListener(OnStateExtensionAdded);
        }

        protected override void OnDisable() {
            base.OnDisable();
            stateMachine?.RemoveOnStateExtensionAddedListener(OnStateExtensionAdded);
        }

        protected override void OnStateAdded(MovementState state)
        {
            if (validResultStateTypes == null || state == null) return;
            state?.RemoveTransitionCheck(GetExtensionName());
            state?.AddTransitionCheck(GetExtensionName(), this, !validResultStateTypes.Contains(state.Type));
        }

        protected override void OnStateRemoved(MovementState state) {
            state?.RemoveTransitionCheck(GetExtensionName());
        }

        public abstract bool Validate();
        protected abstract void OnStateExtensionAdded(StateExtension extension);
        protected abstract MovementStateType[] GetValidResultStateTypes();
    }
}
