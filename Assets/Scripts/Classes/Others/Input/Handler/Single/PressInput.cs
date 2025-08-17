namespace RedSilver2.Framework.Inputs
{
    public sealed class PressInput : SingleInput
    {
        public PressInput(string inputHandlerName, KeyboardKey defaultKeyboardKey, GamepadKey defaultGamepadKey) : base(inputHandlerName, defaultKeyboardKey, defaultGamepadKey)
        {
        }

        public sealed override bool GetKeyboardKeyValue() => InputManager.GetKeyDown(KeyboardKey);
        public sealed override bool GetGamepadKeyValue()  => InputManager.GetKeyDown(GamepadKey);
    }
}
