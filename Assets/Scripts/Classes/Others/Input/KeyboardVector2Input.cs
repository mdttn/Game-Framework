using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public sealed partial class KeyboardVector2Input : Vector2Input
    {
        private Vector2Keyboard keyboardKeys;

        public KeyboardVector2Input(Vector2Keyboard keyboardKeys, bool useGamepadLeftStick) : base(useGamepadLeftStick) 
        {
           this.keyboardKeys = keyboardKeys; 
        }

        public void OverrideUpKey   (KeyboardKey key) => keyboardKeys.OverrideUp(key);
        public void OverrideDownKey (KeyboardKey key) => keyboardKeys.OverrideDown(key);
        public void OverrideLeftKey (KeyboardKey key) => keyboardKeys.OverrideLeft(key);
        public void OverrideRightKey(KeyboardKey key) => keyboardKeys.OverrideRight(key);

        private bool TryGetKeyboardVector2(out Vector2 result)
        {
            result = InputManager.GetKeyboardVector2(keyboardKeys);
            if (result.magnitude > 0f) { return true; }

            result = Vector2.zero;
            return false;
        }

        protected override Vector2 GetInputVector2()
        {
            Vector2 result;     

            if(TryGetKeyboardVector2(out result)) { return result; }
            if(TryGetGamepadVector2 (out result)) { return result; }
            return result;
        }
    }
}
