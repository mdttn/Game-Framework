using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public sealed class MouseVector2Input : Vector2Input
    {
        public MouseVector2Input(string name) : base(name)
        {
        }

        public MouseVector2Input(string name, bool useLeftGamepadStick) : base(name, useLeftGamepadStick)
        {
        }

        private bool GetMouseVector2(out Vector2 result)
        {
            result = InputManager.GetMouseVector2();
            if (result.magnitude > 0f) return true;

            result = Vector2.zero;
            return false;
        }

        protected sealed override Vector2 GetInputVector2()
        {            
            if(GetMouseVector2(out Vector2 result)) return result;
            return base.GetInputVector2();
        }
    }
}
