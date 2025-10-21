using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs
{
    public abstract class SingleInput : InputHandler
    {
        private UnityEvent onUpdate;

        protected KeyboardKey keyboardKey;
        protected GamepadButton  gamepadKey;

        public bool Value { get; private set; }
       
        public KeyboardKey KeyboardKey => keyboardKey;
        public GamepadButton  GamepadKey  => gamepadKey;

        public SingleInput(string inputHandlerName, KeyboardKey defaultKeyboardKey, GamepadButton defaultGamepadKey) : base(inputHandlerName)
        {
            Value       = false;
            onUpdate    = new UnityEvent();

            keyboardKey = defaultKeyboardKey;
            gamepadKey  = defaultGamepadKey;
        }

        public sealed override void Update()
        {
            Value = IsEnabled ?  GetKeyboardKeyValue()
                              || GetGamepadKeyValue() : false;

            if(IsEnabled && Value) { onUpdate?.Invoke(); }
        }

        public void AddOnUpdateListener(UnityAction action)
        {
            if(onUpdate != null && action != null) { onUpdate.AddListener(action); }
        }
        public void RemoveOnUpdateListener(UnityAction action)
        {
            if (onUpdate != null && action != null) { onUpdate.RemoveListener(action); }
        }

        public void AddOnUpdateListeners(UnityAction[] actions)
        {
            if (actions != null)
                foreach (UnityAction action in actions) AddOnUpdateListener(action);
        }
        public void RemoveOnUpdateListeners(UnityAction[] actions)
        {
            if (actions != null)
                foreach (UnityAction action in actions) RemoveOnUpdateListener(action);
        }

        public string GetKeyboardKeyName() => keyboardKey.ToString();
        public string GetGamepadKeyName()  => gamepadKey.ToString();

        public string GetKeyboardKeyPath() => InputManager.GetPath(keyboardKey);
        public string GetGamepadKeyPath()  => InputManager.GetPath(gamepadKey);

        public sealed override string GetPaths() => $"Keyboard Path: {GetKeyboardKeyPath()} | Gamepad Path: {GetGamepadKeyPath()}";

        public abstract bool GetKeyboardKeyValue();
        public abstract bool GetGamepadKeyValue();
    }
}
