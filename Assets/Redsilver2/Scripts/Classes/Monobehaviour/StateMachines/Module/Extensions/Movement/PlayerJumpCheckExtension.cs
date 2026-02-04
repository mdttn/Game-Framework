using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.States.Extensions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class PlayerJumpCheckExtension : MovementStateJumpCheckExtension
    {
        private OverrideablePressInput input;
        public PressInput Input => input;

        protected override void Start()
        {
            input = JumpState.GetPressInput();
            input?.Enable();
            base.Start();
        }

        protected sealed override void OnDisable()
        {
            base.OnDisable();
            input?.Disable();
        }


        protected sealed override void OnEnable()
        {
            base.OnEnable();
            input?.Disable();
        }

        protected sealed override void CheckJump(IStateTransition groundStateTransition, bool ignoreGroundedCondition, ref bool isJumping)
        {
            input?.Update();

            if (ignoreGroundedCondition && input != null) { isJumping = input.Value; }
            else if(groundStateTransition != null && input != null) {
                isJumping = !groundStateTransition.Validate() ? false : input.Value;
            }
            else {
                isJumping = false;
            }
        }
    }
}
