using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs
{
    public abstract class Vector2Input : InputHandler
    {
        protected bool useLeftGamepadStick;
        private UnityEvent<Vector2> onUpdate;
       

        public Vector2 Value { get; private set; }

        public const GamepadStick DEFAULT_GAMEPAD_STICK   = GamepadStick.LeftStick;


        protected Vector2Input(string name) : base(name)
        {
            onUpdate = new UnityEvent<Vector2>();
            this.useLeftGamepadStick = true;
        }

        protected Vector2Input(string name, bool useLeftGamepadStick) : base(name)
        {
            onUpdate = new UnityEvent<Vector2>();
            this.useLeftGamepadStick = useLeftGamepadStick;
        }

        public sealed override void Update()
        {
            Value = GetInputVector2();
            if(IsEnabled) { onUpdate.Invoke(Value); }
        }

        public void AddOnUpdateListener(UnityAction<Vector2> action)
        {
            if(onUpdate != null && action != null) onUpdate.AddListener(action);
        }

        public void RemoveOnUpdateListener(UnityAction<Vector2> action)
        {
            if (onUpdate != null && action != null) onUpdate.RemoveListener(action);
        }

        // Fix this stupid bug (for future reference)
        public string GetGamepadStickPath() => InputManager.GetPath(KeyboardKey.A);

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
            result = InputManager.GetGamepadVector2(useLeftGamepadStick);
            if(result.magnitude > 0f) { return true; }

            result = Vector2.zero;
            return false;
        }

        protected virtual Vector2 GetInputVector2()
        {
            Vector2 result;
            if (TryGetGamepadVector2(out result)) { return result; }
            return result;
        }
    }
}
