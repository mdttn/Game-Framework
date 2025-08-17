using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public sealed class HoldInput : SingleInput
    {
        public HoldInput(string name, KeyboardKey defaultKeyboardKey, GamepadKey defaultGamepadKey) : base(name, defaultKeyboardKey, defaultGamepadKey)
        {

        }

        public sealed override bool GetKeyboardKeyValue() => InputManager.GetKey(KeyboardKey);
        public sealed override bool GetGamepadKeyValue() => InputManager.GetKey(GamepadKey);
    }
}
