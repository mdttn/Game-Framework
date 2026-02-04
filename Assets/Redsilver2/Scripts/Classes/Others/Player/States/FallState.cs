using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class FallState : MovementState
    {
        public FallState(MovementStateMachine owner) : base(owner) {

        }

        protected sealed override void AddRequiredTransitionStates(MovementStateMachine stateMachine) {
            if (stateMachine == null) return;


            Debug.LogWarning("Create Land State | Contains: " + !stateMachine.ContainsState(MovementStateType.Land) +  " | Is Valid Transition: " + IsValidTransitionState(MovementStateType.Land));

            if (!stateMachine.ContainsState(MovementStateType.Land) && IsValidTransitionState(MovementStateType.Land)) {
                new LandState(stateMachine);
                AddTransitionState(stateMachine.GetState(MovementStateType.Land));
            }
        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
           results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Land });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type) {
            type = MovementStateType.Fall;
        }
    }
}
