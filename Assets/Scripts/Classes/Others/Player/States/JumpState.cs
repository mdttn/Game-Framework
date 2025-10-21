using RedSilver2.Framework.Inputs;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public partial class PlayerStateMachine 
    {
        public sealed class JumpState : PlayerState
        {
            private readonly PressInput jumpInput;
            private readonly CharacterController character;
            private readonly Transform transform;

            public const string STATE_NAME = "Jump";
            public const string JUMP_INPUT_NAME = "Press Jump";

            private JumpState()
            {

     
            }

            public JumpState(PlayerStateMachine owner) : base(owner)
            {
                jumpInput = GetJumpInput();
                character = owner.character;
                transform = owner.owner.transform;

                Debug.Log(transform);
                Debug.Log(character);

                IsJumping.Intialize(owner);
                IsNotJumping.Intialize(owner);
            }

            protected override void OnStateEnter()
            {
                Jump();
                base.OnStateEnter();
            }

            protected override void OnStateExit()
            {
                base.OnStateExit();
            }

            public void Jump()
            {
               if(owner != null) Jump(50f);
            }

            public void Jump(float jumpForce)
            {
                if(character != null && transform != null)
                {
                    character.Move(jumpForce * transform.up);
                }
            }

            public sealed override string GetStateName() => STATE_NAME;


            protected override void SetObligatoryTransition(PlayerState[] states, bool removeTransition)
            {
                AddTransitionCondition(IsJumping.Get(owner));

                if (states != null)
                {
                    foreach (PlayerState state in states.Where(x => x != this))
                    {
                        if (removeTransition) { state.RemoveTransitionCondition(IsNotJumping.TRANSITION_CONDITION_NAME); }
                        else                  { state.AddTransitionCondition   (IsNotJumping.TRANSITION_CONDITION_NAME); }
                    }
                }
            }

            public static void SetDefaultJumpInputEvents(PlayerStateMachine owner)
            {
                if(owner != null)
                {
                    owner.AddOnStateAddedListener(OnStateAdded);
                    owner.AddOnStateRemovedListener(OnStateRemoved);
                }
            }

            private static void OnStateAdded(PlayerState state)
            {
                if(state is JumpState)
                {
                    PlayerStateMachine owner = state.Owner;
                    if (owner == null) return;

                    if (Contains(owner, out int count) && count == 1)
                    {
                        PressInput jumpInput = GetJumpInput();
                        owner.AddOnUpdateListener(jumpInput.Update);
                        jumpInput.Enable();
                    }
                }
            }
            private static void OnStateRemoved(PlayerState state)
            {
                if(state is JumpState)
                {
                    PlayerStateMachine owner = state.Owner;
                    if (owner == null) return;

                    if (!Contains(owner))
                    {
                        PressInput jumpInput = GetJumpInput();
                        owner.RemoveOnUpdateListener(jumpInput.Update);
                        jumpInput.Disable();
                    }
                }
            }

            public static bool Contains(PlayerStateMachine owner)
            {
                return Contains(owner, out int count);
            }
            public static bool Contains(PlayerStateMachine owner, out int count)
            {
                count = 0;
                if (owner == null) return false;

                count = owner.GetStates().Where(x => x is JumpState).Count();
                return count > 0;
            }

            public static OverrideablePressInput GetJumpInput()
            {
                OverrideablePressInput result = InputManager.GetInputHandler(JUMP_INPUT_NAME) as OverrideablePressInput;
                if (result == null) return new OverrideablePressInput(JUMP_INPUT_NAME, KeyboardKey.Space, GamepadButton.ButtonSouth);
                return result;
            }
        }
    }
}
