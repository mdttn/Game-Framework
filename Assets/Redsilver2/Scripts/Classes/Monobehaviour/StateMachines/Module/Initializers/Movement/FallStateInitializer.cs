using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine.Events;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    [RequireComponent(typeof(LandStateInitializer))]
    public class FallStateInitializer : MovementStateInitializer
    {

        [SerializeField] private float defaultFallSpeed;
        [SerializeField] private float fallSpeed;

        [Space]
        [SerializeField] private float fallMoveSpeed;

        #if UNITY_EDITOR

        private void OnValidate() {
            defaultFallSpeed = Mathf.Clamp(defaultFallSpeed, float.MinValue, 0f);
            fallSpeed        = Mathf.Clamp(fallSpeed       , float.MinValue, defaultFallSpeed - 1f);
            fallMoveSpeed    = Mathf.Clamp(fallMoveSpeed   , Mathf.Epsilon, float.MaxValue);
        }

#endif

        protected sealed override MovementState GetDefaultState(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return null;

            if (stateMachine.ContainsState(MovementStateType.Fall)) 
                return stateMachine.GetState(MovementStateType.Fall) as FallState;

            return new FallState(stateMachine);
        }

        private UnityAction OnEnterLandState(MovementState state) {
            if (state == null) return null;

            return () => {
                if (state == null) return;
                state.MovementHandler?.SetFallSpeed(defaultFallSpeed);
            };
        }


        private UnityAction OnUpdateFall(MovementState state)
        {
            if (state == null) return null;

            return () => {
                if (state == null) return;
                state.MovementHandler?.UpdateMoveSpeed(fallMoveSpeed, 5f);
                state.MovementHandler?.UpdateFallSpeed(fallSpeed, 2.5f);
            };
        }

        protected sealed override void OnStateAdded(MovementState state)
        {
            if (state is FallState){
                state?.AddOnUpdateListener(OnUpdateFall(state));
            }
            else {
                state?.AddOnStateEnteredListener(OnEnterLandState(state));
            }
        }

        protected sealed override void OnStateRemoved(MovementState state)
        {
            if (state is FallState)
            {
                state?.RemoveOnUpdateListener(OnUpdateFall(state));
            }
            else
            {
                state?.RemoveOnStateEnteredListener(OnEnterLandState(state));
            }
        }

        protected override MovementStateType[] GetInclusiveStates()
        {
            return new MovementStateType[] {
                MovementStateType.Land, MovementStateType.Fall
            };
        }
    }
}
