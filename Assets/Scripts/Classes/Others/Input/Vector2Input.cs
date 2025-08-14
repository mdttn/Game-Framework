using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs
{
    public abstract partial class Vector2Input : InputHandler
    {
        private bool useGamepadLeftStick;
        private UnityEvent<Vector2> onUpdate;
        public Vector2 Value { get; private set; }

        private Vector2Input() { }

        public Vector2Input(bool useGamepadLeftStick) : base()
        {
            onUpdate = new UnityEvent<Vector2>();
            this.useGamepadLeftStick = useGamepadLeftStick;
        }


        public override void Update()
        {
            Value = GetInputVector2();
            if(IsEnabled) { onUpdate.Invoke(Value); }
        }

        public override string GetKeysPaths()
        {
            return string.Empty;
        }

        public void AddOnUpdateListener(UnityAction<Vector2> action)
        {
            if(onUpdate != null && action != null) onUpdate.AddListener(action);
        }

        public void RemoveOnUpdateListiner(UnityAction<Vector2> action)
        {
            if (onUpdate != null && action != null) onUpdate.RemoveListener(action);
        }

        protected bool TryGetGamepadVector2(out Vector2 result)
        {
            result = InputManager.GetGamepadVector2(useGamepadLeftStick);
            if(result.magnitude > 0f) { return true; }

            result = Vector2.zero;
            return false;
        }

        protected abstract Vector2 GetInputVector2();
    }
}
