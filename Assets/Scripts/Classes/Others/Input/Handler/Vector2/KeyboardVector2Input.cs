using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public partial class KeyboardVector2Input : Vector2Input
    {
        protected readonly Vector2Keyboard keyboardKeys;

        public KeyboardKey Up    => keyboardKeys.Up;
        public KeyboardKey Down  => keyboardKeys.Down;
        public KeyboardKey Left  => keyboardKeys.Left;
        public KeyboardKey Right => keyboardKeys.Right;

        public const KeyboardKey DEFAULT_UP_KEY    = KeyboardKey.W;
        public const KeyboardKey DEFAULT_DOWN_KEY  = KeyboardKey.S;

        public const KeyboardKey DEFAULT_LEFT_KEY  = KeyboardKey.A;
        public const KeyboardKey DEFAULT_RIGHT_KEY = KeyboardKey.D;

        public KeyboardVector2Input(string name) : base(name)
        {
            this.keyboardKeys = new Vector2Keyboard(DEFAULT_UP_KEY, DEFAULT_DOWN_KEY, DEFAULT_LEFT_KEY, DEFAULT_RIGHT_KEY);
        }

        public KeyboardVector2Input(string name, Vector2Keyboard keyboardKeys) : base(name)
        {
            this.keyboardKeys = keyboardKeys;
        }

        public KeyboardVector2Input(string name, bool useLeftGamepadStick) : base(name, useLeftGamepadStick)
        {
            this.keyboardKeys = new Vector2Keyboard(DEFAULT_UP_KEY, DEFAULT_DOWN_KEY, DEFAULT_LEFT_KEY, DEFAULT_RIGHT_KEY);
        }

        public KeyboardVector2Input(string name, Vector2Keyboard keyboardKeys, bool useLeftGamepadStick) : base(name, useLeftGamepadStick) 
        {
            this.keyboardKeys = keyboardKeys;
        }

        public string GetUpPath() => InputManager.GetPath(keyboardKeys.Up);
        public string GetDownPath() => InputManager.GetPath(keyboardKeys.Down);
        public string GetLeftPath() => InputManager.GetPath(keyboardKeys.Left);
        public string GetRightPath() => InputManager.GetPath(keyboardKeys.Right);

        public string GetKeyboardKeysInfos()
        {
            return "| Keyboard Paths |\n"
                  + $"Up Key: {keyboardKeys.Up} | Path: {GetUpPath()} \n"
                  + $"Down Key: {keyboardKeys.Down} | Path: {GetDownPath()} \n"
                  + $"Left Key: {keyboardKeys.Left} | Path: {GetLeftPath()} \n"
                  + $"Right Key: {keyboardKeys.Right} | Path: {GetRightPath()}";
        }

        public sealed override string GetPaths()
        {
            return $"{base.GetPaths()}\n\n{GetKeyboardKeysInfos()}";
        }

        private bool TryGetKeyboardVector2(out Vector2 result)
        {
            result = InputManager.GetVector2(keyboardKeys);
            if (result.magnitude > 0f) { return true; }

            result = Vector2.zero;
            return false;
        }

        protected sealed override Vector2 GetInputVector2()
        {
            if (TryGetKeyboardVector2(out Vector2 result)) return result;
            return base.GetInputVector2();
        }


    }
}
