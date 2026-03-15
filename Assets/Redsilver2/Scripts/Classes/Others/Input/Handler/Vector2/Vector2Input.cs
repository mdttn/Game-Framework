using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs
{
    public abstract class Vector2Input : InputHandler
    {
        protected GamepadStick gamepadStick;
       
        public Vector2 Value { 
            get {
                if (!IsEnabled) return Vector2.zero;
                return GetInputVector2();
            } 
        }

        public const GamepadStick DEFAULT_GAMEPAD_STICK = GamepadStick.LeftStick;


        protected Vector2Input(string name) : base(name)
        {
            this.gamepadStick = GamepadStick.LeftStick;
        }

        protected Vector2Input(string name, GamepadStick gamepadStick) : base(name)
        {
            this.gamepadStick = gamepadStick;
        }


        // Fix this stupid bug (for future reference)
        public string GetGamepadStickPath() => $"{gamepadStick.ToString()} (There is no path)";

        public string GetGamepadStickInfos()
        {
            return "| Gamepad Paths |\n" +
                    $"Gamepad Stick: { GetGamepadStickPath() } | Path: {GetGamepadStickPath()}";
        }

        public override string GetPaths()
        {
            return "| Keys Paths | \n\n" + $"{GetGamepadStickInfos()}";
        }


        private bool TryGetGamepadVector2(out Vector2 result)
        {
            result = InputManager.GetGamepadVector2(gamepadStick);
            if(result.magnitude > 0f) { return true; }

            result = Vector2.zero;
            return false;
        }

        protected virtual Vector2 GetInputVector2()
        {
            Vector2 result;
            if (TryGetGamepadVector2(out result)) { return result; }
            // Add XR Controls...
            return result;
        }
    }
}
