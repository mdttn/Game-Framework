using RedSilver2.Framework.Inputs.Configurations;
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public abstract class PlayerMovementStateMachine : MovementStateMachine
    {
        private MovementInputSettings inputSettings;

        protected PlayerMovementStateMachine(MovementStateMachineController controller, MovementInputSettings settings) : base(controller) {
            this.inputSettings = settings;
        }

        protected override void Move()
        {
            Vector2 inputValue = inputSettings == null ? Vector2.zero : inputSettings.GetMoveVector();
            inputValue.Normalize();

            if (Transform != null) {
                float moveSpeed = GetMoveSpeed(), fallSpeed = GetFallSpeed();
                Vector3 nextPosition = Time.deltaTime * ((Transform.right   * moveSpeed * inputValue.x) + 
                                                         (Transform.up      * fallSpeed)
                                                       + (Transform.forward * moveSpeed * inputValue.y));

                Move(nextPosition, fallSpeed, moveSpeed);
            }
        }

    }
}
