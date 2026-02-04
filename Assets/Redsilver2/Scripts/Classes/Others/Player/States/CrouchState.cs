using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class CrouchState : MovementState
    {
        private const string HOLD_CROUCH_INPUT     = "Hold Crouch Input";
        private const string PRESS_CROUCH_INPUT    = "Press Crouch Input";
        private const string CROUCH_INPUT_SETTING  = "Crouch Input Setting";

        public CrouchState(MovementStateMachine owner) : base(owner) {

        }

        protected sealed override void AddRequiredTransitionStates(MovementStateMachine stateMachine) {

        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
            results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Walk, MovementStateType.Run, MovementStateType.Idol });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type) {
            type = MovementStateType.Crouch;
        }

        public static OverrideableHoldInput GetHoldInput()
        {
            return InputManager.GetOrCreateOverrideableHoldInput(HOLD_CROUCH_INPUT, KeyboardKey.C, GamepadButton.ButtonEast);
        }

        public static OverrideablePressInput GetPressInput()
        {
            return InputManager.GetOrCreateOverrideablePressInput(PRESS_CROUCH_INPUT, KeyboardKey.C, GamepadButton.ButtonEast);
        }

        public static bool HasToHoldInput() {
            if (!PlayerPrefs.HasKey(CROUCH_INPUT_SETTING))
                return true;

            return PlayerPrefs.GetString(CROUCH_INPUT_SETTING).Equals("hold");
        }
    }
}
