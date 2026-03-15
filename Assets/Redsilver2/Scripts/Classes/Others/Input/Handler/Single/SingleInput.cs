namespace RedSilver2.Framework.Inputs
{
    public abstract class SingleInput : InputHandler
    {
        protected InputControl defaultControl;
        protected InputControl gamepadControl;
        protected InputControl xrControl;

        private bool allowXRPath;
        public bool AllowXRPath => allowXRPath;

        public bool Value
        {
            get {
                if (!IsEnabled) return false;
                    return GetDefaultPathValue()
                        || GetGamepadPathValue()
                        || GetXRPathValue();
            }
        }

        public SingleInput(string inputHandlerName, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadButton) : base(inputHandlerName)
        {
            allowXRPath    = false;
            xrControl      = null;

            defaultControl = InputManager.GetKeyboardControl(defaultKeyboardKey); 
            gamepadControl = InputManager.GetGamepadControl(defaultGamepadButton);
        }

        public SingleInput(string inputHandlerName, MouseButton defaultMouseButton, GamepadButton defaultGamepadButton) : base(inputHandlerName)
        {
            allowXRPath = false;
            xrControl   = null;

            defaultControl = InputManager.GetMouseControl(defaultMouseButton);
            gamepadControl = InputManager.GetGamepadControl(defaultGamepadButton);
        }

        public sealed override string GetPaths() {
            string result = string.Empty;

            if(defaultControl != null)
            {
               result = defaultControl.Path.Contains("Mouse", System.StringComparison.OrdinalIgnoreCase) ? 
                    $"Mouse Button Path: {defaultControl}" : $"Keyboard Key Path: {defaultControl}";
            }

           if(gamepadControl != null) result += $"\nGamepad Button Path: {gamepadControl}";
           if(xrControl      != null) result += $"\nXR Button Path:      {xrControl}";
           return result;
        }

        public bool GetDefaultPathValue()
        {
            return GetXRValue(defaultControl);
        }
        public bool GetGamepadPathValue()
        {
            return GetXRValue(gamepadControl);
        }
        public bool GetXRPathValue()
        {
            return GetXRValue(xrControl);
        }

        protected abstract bool GetDefaultValue(InputControl control);
        protected abstract bool GetGamepadValue(InputControl control);
        protected abstract bool GetXRValue(InputControl control);
    }
}
