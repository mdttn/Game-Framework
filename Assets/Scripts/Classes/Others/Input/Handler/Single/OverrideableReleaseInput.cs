namespace RedSilver2.Framework.Inputs
{
    public sealed class OverrideableReleaseInput : ReleaseInput, IOverridableSingleInput
    {
        public OverrideableReleaseInput(string name, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadKey) : base(name, defaultKeyboardKey, defaultGamepadKey)
        {
        }
        public void OverrideKey(KeyboardKey key) => keyboardKey = key;
        public void OverrideKey(GamepadButton key) => gamepadKey = key;
    }
}
