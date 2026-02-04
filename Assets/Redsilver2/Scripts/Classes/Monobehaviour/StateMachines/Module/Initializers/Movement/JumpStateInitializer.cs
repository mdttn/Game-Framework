using RedSilver2.Framework.StateMachines.States;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.StateMachines
{
    [RequireComponent(typeof(FallStateInitializer))]
    public class JumpStateInitializer : MovementStateInitializer
    {
        [SerializeField] private float jumpForce;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            jumpForce = Mathf.Clamp(jumpForce, 10f, float.MaxValue);
        }
        #endif

        protected sealed override MovementState GetDefaultState(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return null;

            if (stateMachine.ContainsState(MovementStateType.Jump)) 
                return stateMachine.GetState(MovementStateType.Jump) as JumpState;

            return new JumpState(stateMachine);
        }

        private UnityAction OnJumpStateEnter(MovementState state)
        {
            if(state == null)  return null;
            return () => {
                if (state == null) return;
                state.MovementHandler?.SetJumpHeight(jumpForce);
            };
        }

        protected sealed override void OnStateAdded(MovementState state)
        {
            state?.AddOnStateEnteredListener(OnJumpStateEnter(state));
        }

        protected sealed override void OnStateRemoved(MovementState state)
        {
            state?.RemoveOnStateEnteredListener(OnJumpStateEnter(state));
        }

        protected sealed override MovementStateType[] GetInclusiveStates()
        {
            return new MovementStateType[] {
                MovementStateType.Jump
            };
        }
    }
}
