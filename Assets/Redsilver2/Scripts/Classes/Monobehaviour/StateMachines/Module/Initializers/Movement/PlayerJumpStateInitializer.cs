using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public class PlayerJumpStateInitializer : JumpStateInitializer {

        private PressInput input;
        public  PressInput Input => input;

        private const string PRESS_JUMP_INPUT = "Press Jump";

        protected override void Awake()
        {
            base.Awake();
            input = GetPressInput();
        }

        protected override void Start() {
            base.Start();
            input?.Enable();
        }

        protected sealed override void OnDisable() {
            base.OnDisable();
            input?.Disable();    
        }


        protected sealed override void OnEnable() {
            base.OnEnable();
            input?.Disable();
        }

        protected sealed override string GetModuleName()
        {
            return  "Player " +  base.GetModuleName();
        }

        protected sealed override void OnUpdate()
        {
            transitionState = input != null ? input.Value : false;
            base.OnUpdate();
        }

        public static OverrideablePressInput GetPressInput()
        {
            return InputManager.GetOrCreateOverrideablePressInput(PRESS_JUMP_INPUT, KeyboardKey.Space, GamepadButton.ButtonSouth);
        }
    }
}
