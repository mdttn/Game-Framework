using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs
{
    public abstract partial class Vector2Input : InputHandler, IOverridableVector2Input
    {
        public GamepadStick gamepadStick;
        private UnityEvent<Vector2> onUpdate;


        public Vector2 Value { get; private set; }


        protected Vector2Input(string name, GamepadStick gamepadStick) : base(name)
        {
            onUpdate = new UnityEvent<Vector2>();
            this.gamepadStick = gamepadStick;        
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

        public void   OverrideStick(GamepadStick stick) { this.gamepadStick = stick; }
        public string GetGamepadStickPath() => InputManager.GetPath(gamepadStick);

        public override string GetPaths()
        {
            return "| Keys Paths | \n\n" + "|Gamepad|\n" + $"Gamepad Stick: {GetGamepadStickPath()} ({gamepadStick.ToString()})";
        }


        protected bool TryGetGamepadVector2(out Vector2 result)
        {
            result = InputManager.GetVector2(gamepadStick);
            if(result.magnitude > 0f) { return true; }

            result = Vector2.zero;
            return false;
        }

        protected abstract Vector2 GetInputVector2();
    }
}
