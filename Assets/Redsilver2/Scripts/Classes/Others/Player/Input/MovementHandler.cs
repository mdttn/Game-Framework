using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States.Movement
{
    [System.Serializable]
    public abstract class MovementHandler {
        protected bool use2DMovement;
        protected bool canResetCrouch;

        private float moveSpeed;
        private float fallSpeed;
        private float height;

        protected Vector3 nextPosition;

        private MovementStateMachine stateMachine;

        public bool  CanResetCrouch => canResetCrouch;
        public float MoveSpeed      => moveSpeed;    
        public float Height         => height;
        public float FallSpeed      => fallSpeed;

        public const float DEFAULT_CROUCH_SPEED = 5f;



        protected MovementHandler(MovementStateMachineController controller) {
            this.use2DMovement  = false;  
            this.canResetCrouch = true;
        }

        protected MovementHandler(MovementStateMachineController controller, bool use2DMovement) {
            this.use2DMovement  = use2DMovement;
            this.canResetCrouch = true;
        }

        public void SetStateMachine(MovementStateMachine stateMachine) {
            if(this.stateMachine == null) {
                this.stateMachine = stateMachine;
                stateMachine?.AddOnUpdateListener(Update);
                stateMachine?.AddOnLateUpdateListener(LateUpdate);
              
                stateMachine?.AddOnEnabledListener(Enable);
                stateMachine?.AddOnDisabledListener(Disable);
            }
        }



        protected virtual void Update() {
            if (stateMachine == null || stateMachine.Controller == null) return;
            nextPosition = GetNextPosition(GetTransform(), moveSpeed, fallSpeed, stateMachine.Controller != null ?  false : false);
        }

        protected virtual void LateUpdate() {
            Crouch(height);
            Move(nextPosition);
        }

        protected abstract void Enable();
        protected abstract void Disable();

        public void SetCanResetCrouch(bool canResetCrouch) {
            this.canResetCrouch = canResetCrouch;
        }

        public void SetMoveSpeed(float moveSpeed) {
            this.moveSpeed = Mathf.Clamp(moveSpeed, Mathf.Epsilon, Mathf.Infinity);
        }

        public void UpdateMoveSpeed(float nextMoveSpeed, float updateSpeed) {
            if(nextMoveSpeed >= Mathf.Epsilon)
               this.moveSpeed = Mathf.Lerp(moveSpeed, nextMoveSpeed, Time.deltaTime * updateSpeed);
        }

        public void SetFallSpeed(float fallSpeed) {
            this.fallSpeed = Mathf.Clamp(fallSpeed, float.MinValue, 0f);
        }
        public void SetJumpHeight(float jumpHeight) {
            this.fallSpeed = Mathf.Clamp(jumpHeight, 0f, float.MaxValue);
        }

        public void UpdateFallSpeed(float nextFallSpeed, float updateSpeed) {
            if (nextFallSpeed <= 0f)
              this.fallSpeed = Mathf.Lerp(fallSpeed, nextFallSpeed, Time.deltaTime * updateSpeed);
        }

        public void UpdateHeight(float nextHeight, float updateSpeed) {
                this.height = Mathf.Lerp(height, nextHeight, Time.deltaTime * updateSpeed);
        }
        protected abstract void Crouch(float height);
        protected abstract void Move(Vector3 position);

        public    abstract float GetMoveMagnitude();

        protected abstract Vector3 GetNextPosition(Transform transform, float moveSpeed, float fallSpeed, bool use2DMovement);
        public abstract Transform GetTransform();
    }
}
