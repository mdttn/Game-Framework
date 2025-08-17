using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs
{
    public abstract class SingleInput : InputHandler, IOverridableSingleInput
    {
        private UnityEvent onUpdate;

        private KeyboardKey keyboardKey;
        private GamepadKey  gamepadKey;

        public bool Value { get; private set; }
       
        public KeyboardKey KeyboardKey => keyboardKey;
        public GamepadKey  GamepadKey  => gamepadKey;

        public SingleInput(string inputHandlerName, KeyboardKey defaultKeyboardKey, GamepadKey defaultGamepadKey) : base(inputHandlerName)
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

        public void OverrideKey(KeyboardKey key) => keyboardKey = key;  
        public void OverrideKey(GamepadKey key)  => gamepadKey  = key; 

        public string GetKeyboardKeyName() => keyboardKey.ToString();
        public string GetGamepadKeyName()  => gamepadKey.ToString();

        public string GetKeyboardKeyPath() => InputManager.GetPath(keyboardKey);
        public string GetGamepadKeyPath()  => InputManager.GetPath(gamepadKey);

        public sealed override string GetPaths() => $"Keyboard Path: {GetKeyboardKeyPath()} | Gamepad Path: {GetGamepadKeyPath()}";

        public abstract bool GetKeyboardKeyValue();
        public abstract bool GetGamepadKeyValue();
    }
}
