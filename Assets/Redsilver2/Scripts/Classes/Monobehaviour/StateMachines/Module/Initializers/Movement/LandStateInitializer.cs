using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class LandStateInitializer : MovementStateInitializer
    {
        protected sealed override MovementState GetDefaultState(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return null;

            if (stateMachine.ContainsState(MovementStateType.Land)) 
                return stateMachine.GetState(MovementStateType.Land) as LandState;

            return new LandState(stateMachine);
        }

        protected sealed override void OnStateAdded(MovementState state) {

        }

        protected sealed override void OnStateRemoved(MovementState state) {

        }

        protected sealed override MovementStateType[] GetInclusiveStates()
        {
            return null;
        }
    }
}
