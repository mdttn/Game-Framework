using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public class ReleaseInput : SingleInput
    {
        public ReleaseInput(string name, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadKey) : base(name, defaultKeyboardKey, defaultGamepadKey)
        {

        }

        public sealed override bool GetKeyboardKeyValue() => InputManager.GetKeyUp(KeyboardKey);
        public sealed override bool GetGamepadKeyValue()  => InputManager.GetKeyUp(GamepadKey);
    }
}
