using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public sealed class ReleaseInput : SingleInput
    {
        private ReleaseInput() { }

        public ReleaseInput(KeyboardKey defaultKeyboardKey, GamepadKey defaultGamepadKey) : base(defaultKeyboardKey, defaultGamepadKey)
        {

        }

        public override bool GetKeyboardKeyValue() => InputManager.GetKeyUp(KeyboardKey);
        public override bool GetGamepadKeyValue()  => InputManager.GetKeyUp(GamepadKey);
    }
}
