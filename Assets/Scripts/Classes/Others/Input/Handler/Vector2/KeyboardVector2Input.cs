using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public sealed partial class KeyboardVector2Input : Vector2Input
    {
        private readonly Vector2Keyboard keyboardKeys;

        public KeyboardKey Up    => keyboardKeys.Up;
        public KeyboardKey Down  => keyboardKeys.Down;
        public KeyboardKey Left  => keyboardKeys.Left;
        public KeyboardKey Right => keyboardKeys.Right;

        public KeyboardVector2Input(string name, Vector2Keyboard keyboardKeys, GamepadStick gamepadStick) : base(name, gamepadStick) 
        {
           this.keyboardKeys = keyboardKeys; 
        }

        public void OverrideUpKey(KeyboardKey key)    => keyboardKeys.OverrideUp(key);
        public void OverrideDownKey(KeyboardKey key)  => keyboardKeys.OverrideDown(key);
        public void OverrideLeftKey(KeyboardKey key)  => keyboardKeys.OverrideLeft(key);
        public void OverrideRightKey(KeyboardKey key) => keyboardKeys.OverrideRight(key);

        public string GetUpPath()    => InputManager.GetPath(keyboardKeys.Up);
        public string GetDownPath()  => InputManager.GetPath(keyboardKeys.Down);
        public string GetLeftPath()  => InputManager.GetPath(keyboardKeys.Left);
        public string GetRightPath() => InputManager.GetPath(keyboardKeys.Right);

        public sealed override string GetPaths()
        {
            return  $"{base.GetPaths()}\n\n" + "| Keyboard |\n" 
                  + $"Up: {GetUpPath()} ({keyboardKeys.Up})\n"
                  + $"Down: {GetDownPath()} ({keyboardKeys.Down})\n"
                  + $"Left: {GetLeftPath()} ({keyboardKeys.Left})\n"
                  + $"Right: {GetRightPath()} ({keyboardKeys.Right})";
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
            Vector2 result;     

            if(TryGetKeyboardVector2(out result)) { return result; }
            if(TryGetGamepadVector2 (out result)) { return result; }
            return result;
        }
    }
}
