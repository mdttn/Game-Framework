namespace RedSilver2.Framework.Inputs
{
    public class PressInput : SingleInput
    {
        protected PressInput() {  }

        public PressInput(KeyboardKey defaultKeyboardKey, GamepadKey defaultGamepadKey) : base(defaultKeyboardKey, defaultGamepadKey)
        {
        }

        public override bool GetKeyboardKeyValue() => InputManager.GetKeyDown(KeyboardKey);
        public override bool GetGamepadKeyValue()  => InputManager.GetKeyDown(GamepadKey);
    }
}
