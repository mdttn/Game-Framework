using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class RunState : MovementState
    {
        private const string HOLD_RUN_INPUT    = "Hold Run Input";
        private const string PRESS_RUN_INPUT   = "Press Run Input";
        private const string RUN_INPUT_SETTING = "Run Input Setting";
    

        public RunState(MovementStateMachine owner) : base(owner) {
   
        }

        protected sealed override void AddRequiredTransitionStates(MovementStateMachine stateMachine) {
            if (stateMachine == null) return;
            if (!stateMachine.ContainsState(MovementStateType.Idol) && IsValidTransitionState(MovementStateType.Idol)) {
                new IdolState(stateMachine);
                AddTransitionState(stateMachine.GetState(MovementStateType.Idol));
            }
        }

        protected sealed override void SetIncompatibleStateTransitions(ref MovementStateType[] results) {
            results = GetExcludedStateTypes(new MovementStateType[] { MovementStateType.Walk, MovementStateType.Idol, MovementStateType.Fall, MovementStateType.Crouch, MovementStateType.Jump });
        }

        protected sealed override void SetPlayerStateType(ref MovementStateType type) {
            type = MovementStateType.Run;
        }

        public static OverrideableHoldInput GetHoldInput() {
            return InputManager.GetOrCreateOverrideableHoldInput(HOLD_RUN_INPUT, KeyboardKey.LeftShift, GamepadButton.ButtonWest);
        }

        public static OverrideablePressInput GetPressInput() {
            return InputManager.GetOrCreateOverrideablePressInput(PRESS_RUN_INPUT, KeyboardKey.LeftShift, GamepadButton.ButtonWest);
        }

        public static bool HasToHoldInput()
        {
            if (!PlayerPrefs.HasKey(RUN_INPUT_SETTING))
                return true;

            return PlayerPrefs.GetString(RUN_INPUT_SETTING).Equals("hold");
        }
    }
}                                                                                                                                                                                                                                         
