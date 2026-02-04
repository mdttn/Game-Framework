using RedSilver2.Framework.StateMachines.States.Extensions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementStateRunCheckExtension : MovementStateTransitionExtension
    {
        [SerializeField] private bool ignoreGroundedCondition;

        private bool isRunning;
        private IStateTransition groundStateTransition;

        public bool IsRunning => isRunning;

        protected override void Start()
        {
            base.Start();
            groundStateTransition = transform.root.GetComponentInChildren<MovementStateGroundCheckExtension>();
            stateMachine?.AddOnStateExtensionAddedListener(OnStateExtensionAdded);
        }

        private void Update() {
            CheckRun(groundStateTransition, ignoreGroundedCondition, ref isRunning);
        }

        protected abstract void CheckRun(IStateTransition groundStateTransition, bool ignoreGroundedCondition, ref bool isRunning);

        protected sealed override void OnStateExtensionAdded(StateExtension stateExtension)
        {
           if(stateExtension is MovementStateGroundCheckExtension) { groundStateTransition = stateExtension as MovementStateGroundCheckExtension; }
        }

        public sealed override bool Validate()
        {
            return isRunning;
        }

        protected sealed override string GetExtensionName()
        {
            return "Run Check";
        }

        protected sealed override MovementStateType[] GetInclusiveStates()
        {
            return new MovementStateType[] {
              MovementStateType.Run, MovementStateType.Walk
            };
        }

        protected sealed override MovementStateType[] GetValidResultStateTypes() {
            return new MovementStateType[] {
                MovementStateType.Run
            };
        }
    }
}