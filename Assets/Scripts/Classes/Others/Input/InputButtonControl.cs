
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace RedSilver2.Framework.Inputs
{
    public class InputButtonControl : InputControl
    {
        private readonly ButtonControl control;
        private string path;
        public string Path => path;

        public InputButtonControl(string path, Sprite icon) : base(icon) {
            this.path = path;
            control = InputSystem.FindControl(path) as ButtonControl;
        }

        public sealed override bool GetKey()
        {
            if (control != null) return control.isPressed;
            return false;
        }

        public sealed override bool GetKeyDown()
        {
            if (control != null) return control.wasPressedThisFrame;
            return false;
        }

        public sealed override bool GetKeyUp()
        {
            if (control != null) return control.wasReleasedThisFrame;
            return false;
        }
    }
}
