using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.States.Extensions;
using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementStateCrouchCheckExtension : MovementStateTransitionExtension
    {
        [SerializeField] private bool ignoreGroundedCondition;
        private IStateTransition groundStateTransition;


        private bool isCrouching;
        public bool  IsCrouching => isCrouching;

        protected override void Start()
        {
            base.Start();
            groundStateTransition = transform.root.GetComponentInChildren<MovementStateGroundCheckExtension>();
        }

        private void Update()
        {
            if (stateMachine is not MovementStateMachine) return;
            MovementHandler movementHandler = (stateMachine as MovementStateMachine).MovementHandler;
            if (movementHandler == null || !movementHandler.CanResetCrouch) { return;  }

            CheckCrouch(groundStateTransition, ignoreGroundedCondition, ref isCrouching);
        }

        protected abstract void CheckCrouch(IStateTransition groundStateTransition, bool ignoreGroundedCondition,  ref bool isCrouching);

        protected  override void OnDisable()
        {
            base.OnDisable();
            stateMachine?.RemoveOnStateExtensionAddedListener(OnStateExtensionAdded);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            stateMachine?.AddOnStateExtensionAddedListener(OnStateExtensionAdded);
          
        }

        protected override void OnStateExtensionAdded(StateExtension stateExtension)
        {
            if (stateExtension is MovementStateGroundCheckExtension)
                groundStateTransition = stateExtension as MovementStateGroundCheckExtension;
        }

        private void OnStateEntered()
        {

        }

        private void OnStateExited() {

        }

        protected override void OnStateAdded(MovementState state)
        {
            base.OnStateAdded(state);
            // Someting here
        }

        protected override void OnStateRemoved(MovementState state)
        {
            base.OnStateRemoved(state);
            
        }


        public sealed override bool Validate()
        {
            Debug.Log("Checking...");
            return isCrouching;
        }

        protected sealed override string GetExtensionName() {
            return "Crouch Check";
        }

        protected override MovementStateType[] GetInclusiveStates()
        {
            return new MovementStateType[]{
                MovementStateType.Crouch, MovementStateType.Walk, MovementStateType.Run,
                MovementStateType.Idol
            };
        }

        protected override MovementStateType[] GetValidResultStateTypes()
        {
            return new MovementStateType[]{
                MovementStateType.Crouch
            };
        }
    }
}
