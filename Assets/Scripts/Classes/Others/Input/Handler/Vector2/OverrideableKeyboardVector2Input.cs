using UnityEngine;


namespace RedSilver2.Framework.Inputs
{
    public sealed class OverrideableKeyboardVector2Input : KeyboardVector2Input
    {
        public OverrideableKeyboardVector2Input(string name) : base(name)
        {
        }

        public OverrideableKeyboardVector2Input(string name, Vector2Keyboard keyboardKeys) : base(name, keyboardKeys)
        {
        }

        public OverrideableKeyboardVector2Input(string name, bool useLeftGamepadStick) : base(name, useLeftGamepadStick)
        {
        }

        public OverrideableKeyboardVector2Input(string name, Vector2Keyboard keyboardKeys, bool useLeftGamepadStick) : base(name, keyboardKeys, useLeftGamepadStick)
        {
        }

        public void OverrideUpKey(KeyboardKey key)    => keyboardKeys.OverrideUp(key);
        public void OverrideDownKey(KeyboardKey key)  => keyboardKeys.OverrideDown(key);
        public void OverrideLeftKey(KeyboardKey key)  => keyboardKeys.OverrideLeft(key);
        public void OverrideRightKey(KeyboardKey key) => keyboardKeys.OverrideRight(key);
    }
}
