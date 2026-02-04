using RedSilver2.Framework.StateMachines.States.Movement;
using System.Collections.Generic;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class WalkState : MovementState
    {
        public WalkState(MovementStateMachine owner) : base(owner) {

        }

        protected sealed override void AddRequiredTransitionStates(MovementStateMachine stateMachine) {
            if (stateMachine == null) return;

            if (!stateMachine.ContainsState(MovementStateType.Idol) && IsValidTransitionState(MovementStateType.Idol)) {
                new IdolState(stateMachine);
                AddTransitionState(stateMachine.GetState(MovementStateType.Idol));
            }
        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
            results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Fall, MovementStateType.Idol, MovementStateType.Run, MovementStateType.Crouch, MovementStateType.Jump });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type)  {
            type = MovementStateType.Walk;
        }
    }
}
