using RedSilver2.Framework.Inputs;
using System.Linq;
using UnityEngine;
namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public sealed class RunState : MoveState
        {
            public const string RUN_PRESS_INPUT_NAME = "Run Press Input";
            public const string RUN_HOLD_INPUT_NAME  = "Run Hold Input";
            public const string STATE_NAME = "Run";

            public RunState()
            {
            }

            public RunState(PlayerStateMachine owner) : base(owner)
            {
                IsMoving.Intialize(owner);
            }

            public override string GetStateName() => STATE_NAME;

            protected sealed override void SetObligatoryTransition(PlayerState[] states, bool removeTransition)
            {
                AddTransitionCondition(IsMoving.Get(owner));
            }

            public static bool Contains(PlayerStateMachine owner)
            {
                return Contains(owner, out int count);
            }

            public static bool Contains(PlayerStateMachine owner, out int count)
            {
                count = 0;
                if (owner == null) return false;

                count = owner.GetStates().Where(x => x is RunState).Count();
                return count > 0;
            }

            public static OverrideableHoldInput GetRunHoldInput()
            {
                OverrideableHoldInput result = InputManager.GetInputHandler(RUN_HOLD_INPUT_NAME) as OverrideableHoldInput;
                if (result == null) return new OverrideableHoldInput(RUN_HOLD_INPUT_NAME, KeyboardKey.LeftShift, GamepadButton.ButtonEast);
                return result;
            }

            public static OverrideablePressInput GetRunPressInput()
            {
                OverrideablePressInput result = InputManager.GetInputHandler(RUN_PRESS_INPUT_NAME) as OverrideablePressInput;
                if (result == null) return new OverrideablePressInput(RUN_PRESS_INPUT_NAME, KeyboardKey.LeftShift, GamepadButton.ButtonEast);
                return result;
            }
        }
    }
}
