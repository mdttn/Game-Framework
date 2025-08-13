using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public sealed class HoldInput : SingleInput
    {
        private HoldInput() { }

        public HoldInput(KeyboardKey defaultKeyboardKey, GamepadKey defaultGamepadKey) : base(defaultKeyboardKey, defaultGamepadKey)
        {

        }

        public override bool GetKeyboardKeyValue() => InputManager.GetKey(KeyboardKey);
        public override bool GetGamepadKeyValue() => InputManager.GetKey(GamepadKey);
    }
}
