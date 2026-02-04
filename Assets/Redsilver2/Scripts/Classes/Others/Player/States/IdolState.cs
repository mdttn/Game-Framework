namespace RedSilver2.Framework.StateMachines.States {
    public sealed class IdolState : MovementState
    {
        public IdolState(MovementStateMachine owner) : base(owner) {
            
        }

        protected sealed override void AddRequiredTransitionStates(MovementStateMachine stateMachine) {
           
        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
            results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Walk, MovementStateType.Run, MovementStateType.Fall, 
                                                            MovementStateType.Jump, MovementStateType.Crouch });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type) {
            type = MovementStateType.Idol;
        }
    }
}
