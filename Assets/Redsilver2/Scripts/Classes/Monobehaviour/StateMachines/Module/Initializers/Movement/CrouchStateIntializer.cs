using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines {
    public class CrouchStateIntializer : MovementStateInitializer {
        [SerializeField] private float standingHeight;
        [SerializeField] private float crouchHeight;

        [Space]
        [SerializeField] private float crouchMoveSpeed;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            standingHeight  = Mathf.Clamp(standingHeight , 2f, float.MaxValue);
            crouchHeight    = Mathf.Clamp(crouchHeight   , 0f, standingHeight - 1f);
            crouchMoveSpeed = Mathf.Clamp(crouchMoveSpeed, 1f, float.MaxValue);
        }
        #endif

        protected sealed override MovementState GetDefaultState(MovementStateMachine stateMachine) {
            if (stateMachine == null) return null;

            if (stateMachine.ContainsState(MovementStateType.Crouch)) 
                return stateMachine.GetState(MovementStateType.Crouch) as CrouchState;

            return new CrouchState(stateMachine);
        }

        private UnityAction OnUpdateCrouchHeight(MovementState state) {
            if(state == null) return null;
            
            return () => {

                if (state == null || stateMachine == null || stateMachine.Controller == null || state.MovementHandler == null) return;
                MovementHandler movementHandler = state.MovementHandler;
              
                if (movementHandler == null) return;
                Transform transform = movementHandler.GetTransform();       
               
                movementHandler?.SetCanResetCrouch(CanResetCrouch(transform, stateMachine.Controller));
                Debug.DrawRay(transform.position, transform.up, Color.red);

                state.MovementHandler?.UpdateMoveSpeed(crouchMoveSpeed, 5f);
                UpdateHeight(state, crouchHeight, 5f);
            };
        }

        private bool CanResetCrouch(Transform transform, StateMachineController controller)
        {
            if (transform == null || controller == null) return false;
            return !Physics.Raycast(transform.position, transform.up, 2f, ~transform.gameObject.layer);
        }


        private UnityAction OnUpdateStandHeight(MovementState state) {
            if (state == null) return null;
            return () => { UpdateHeight(state, standingHeight, 5f);  };
        }

        private void UpdateHeight(MovementState state, float nextHeight, float updateSpeed)
        {
            if (state == null) return;
            state.MovementHandler?.UpdateHeight(nextHeight, updateSpeed);
        }

        protected sealed override void OnStateAdded(MovementState state) {

            if (state is CrouchState) {
                state?.AddOnUpdateListener(OnUpdateCrouchHeight(state));
            }
            else {
                state?.AddOnUpdateListener(OnUpdateStandHeight(state));
            }
        }

        protected sealed override void OnStateRemoved(MovementState state)
        {
            if (state is CrouchState) {
                state?.RemoveOnUpdateListener(OnUpdateCrouchHeight(state));
            }
            else {
                state?.RemoveOnUpdateListener(OnUpdateStandHeight(state));
            }
        }

        protected sealed override MovementStateType[] GetInclusiveStates()
        {
            return new MovementStateType[] {
                MovementStateType.Idol, MovementStateType.Run, MovementStateType.Walk,
                MovementStateType.Land, MovementStateType.Jump, MovementStateType.Crouch,
                MovementStateType.Fall
            };
        }
    }
}
