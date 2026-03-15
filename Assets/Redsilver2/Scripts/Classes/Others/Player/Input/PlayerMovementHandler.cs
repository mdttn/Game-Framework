using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States.Movement
{
    [System.Serializable]
    public abstract class PlayerMovementHandler : MovementHandler
    {
        private OverrideableKeyboardVector2Input moveInput;
        public KeyboardVector2Input MoveInput => moveInput;
        private const string MOVE_INPUT = "Move Input";

        protected PlayerMovementHandler(PlayerController controller) : base(controller) {       
            moveInput = GetMoveInput();
            moveInput?.Enable();
        }

        protected PlayerMovementHandler(PlayerController controller, bool use2DMovement) : base(controller, use2DMovement) {
            moveInput = GetMoveInput();
            moveInput?.Enable();
        }


        public bool IsMoveInputEnabled() {
            if (moveInput == null) return false;
            return moveInput.IsEnabled;
        }

        protected sealed override void Disable() {
            base.Disable();
            moveInput?.Disable();
        }

        protected sealed override void Enable() {
            moveInput?.Enable();
        }


        protected override Vector3 GetNextPosition(Transform transform, float moveSpeed, float fallSpeed, bool use2DMovement) {
            if(transform == null || moveInput == null) return Vector3.zero;

            Vector2 input = moveInput.Value;
            input.Normalize();

            return  (transform.forward *                       input.y * moveSpeed +
                     transform.up                                      * fallSpeed +
                     transform.right   * (use2DMovement ? 0f : input.x * moveSpeed));
        }

        public sealed override float GetMagnitude()
        {
            if (moveInput == null) return 0f;
            return moveInput.Value.magnitude;
        }

        public static OverrideableKeyboardVector2Input GetMoveInput() {
            return InputManager.GetOrCreateOverrideableKeyboardVector2Input(MOVE_INPUT);
        }
    }
}
