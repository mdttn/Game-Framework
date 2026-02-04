using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    [RequireComponent(typeof(IdolStateInitializer))]
    public sealed class WalkStateInitializer : MovementStateInitializer
    {
        [SerializeField] private float walkSpeed;

        protected sealed override MovementState GetDefaultState(MovementStateMachine stateMachine) {
            if(stateMachine == null)  return null;
            
            if (stateMachine.ContainsState(MovementStateType.Walk)) 
                return stateMachine.GetState(MovementStateType.Walk) as WalkState;

            return new WalkState(stateMachine);
        }

        private UnityAction OnUpdateWalkState(MovementState state)
        {
            if(state == null) return null;
            return () =>
            {
                if (state == null) return;
                state.MovementHandler?.UpdateMoveSpeed(walkSpeed, 5f);
            };
        }

        protected sealed override void OnStateAdded(MovementState state)
        {
            state?.AddOnUpdateListener(OnUpdateWalkState(state));
        }

        protected sealed override void OnStateRemoved(MovementState state)
        {
            state?.AddOnUpdateListener(OnUpdateWalkState(state));
        }

        protected override MovementStateType[] GetInclusiveStates()
        {
            return new MovementStateType[] { MovementStateType.Walk };
        }
    }
}
