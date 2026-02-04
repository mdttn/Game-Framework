using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class IdolStateInitializer : MovementStateInitializer
    {
        protected sealed override MovementState GetDefaultState(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return null;

            if (stateMachine.ContainsState(MovementStateType.Idol))
                return stateMachine.GetState(MovementStateType.Idol) as IdolState;

            return new IdolState(stateMachine);
        }

        protected override MovementStateType[] GetInclusiveStates() {
            return null;
        }

        protected sealed override void OnStateAdded(MovementState state) {

        }

        protected sealed override void OnStateRemoved(MovementState state) {

        }
    }
}
