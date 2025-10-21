using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public class HoldInput : SingleInput
    {
        public HoldInput(string name, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadKey) : base(name, defaultKeyboardKey, defaultGamepadKey)
        {

        }

        public sealed override bool GetKeyboardKeyValue() => InputManager.GetKey(KeyboardKey);
        public sealed override bool GetGamepadKeyValue() => InputManager.GetKey(GamepadKey);
    }
}
