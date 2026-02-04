using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.States.Movement;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class JumpState : MovementState
    {
        private const string PRESS_JUMP_INPUT = "Press Jump";

        public JumpState(MovementStateMachine owner) : base(owner) {

        }

        protected sealed override void AddRequiredTransitionStates(MovementStateMachine stateMachine) {
             if(stateMachine == null) return;
            if (!stateMachine.ContainsState(MovementStateType.Fall) && IsValidTransitionState(MovementStateType.Fall)) {
                new FallState(stateMachine);
                AddTransitionState(stateMachine.GetState(MovementStateType.Fall));
            }
        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
            results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Fall });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type) {
            type = MovementStateType.Jump;
        }

        public static OverrideablePressInput GetPressInput()
        {
            return InputManager.GetOrCreateOverrideablePressInput(PRESS_JUMP_INPUT, KeyboardKey.Space, GamepadButton.ButtonSouth);
        }
    }
}
