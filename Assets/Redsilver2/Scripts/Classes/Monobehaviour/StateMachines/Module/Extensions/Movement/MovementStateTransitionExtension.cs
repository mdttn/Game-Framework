
using RedSilver2.Framework.StateMachines.Controllers;
using System.Linq;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementStateTransitionExtension : MovementStateModule, ICheckableStateTransition
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
            stateMachine?.AddOnStateModuleAddedListener(OnStateExtensionAdded);
        }

        protected override void OnEnable() {
            base.OnEnable();
            stateMachine?.AddOnStateModuleAddedListener(OnStateExtensionAdded);
        }

        protected override void OnDisable() {
            base.OnDisable();
            stateMachine?.RemoveOnStateModuleAddedListener(OnStateExtensionAdded);
        }

        protected override void OnStateAdded(MovementState state)
        {
            if (validResultStateTypes == null || state == null) return;
            state?.RemoveTransitionCheck(ModuleName);
            state?.AddTransitionCheck(ModuleName, this, !validResultStateTypes.Contains(state.Type));
        }

        protected override void OnStateRemoved(MovementState state) {
            state?.RemoveTransitionCheck(ModuleName);
        }

        public abstract bool GetTransitionState();
        protected abstract void OnStateExtensionAdded(StateModule module);
        protected abstract MovementStateType[] GetValidResultStateTypes();
    }
}
