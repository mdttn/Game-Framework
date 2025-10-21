using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs
{
    public sealed class InputGamepadStickControl : InputControl
    {
        private readonly GamepadStick path;

        private bool isKey;
        private bool isKeyDown, canResetKeyDown;
        private bool isKeyUp  , canKeyUpBeTriggered;

        private readonly UnityAction onKey, onKeyDown, onKeyUp;

        public InputGamepadStickControl(GamepadStick path, Sprite icon) : base(icon)  {
            this.path = path;
          
            this.isKeyDown = false;
            this.canResetKeyDown = true;

            this.onKey     = GetOnKeyAction();
            this.onKeyDown = GetOnKeyDownAction();
            this.onKeyUp   = GetOnKeyUpAction();

        }

        private void SetEvent() {

        }

        private UnityAction GetOnKeyAction() {

            if (path == GamepadStick.LeftStickUp    || path == GamepadStick.RightStickDown)  return () => { isKey = GetPositiveAxisYKey(); };
            if (path == GamepadStick.LeftStickDown  || path == GamepadStick.RightStickDown)  return () => { isKey = GetNegativeAxisYKey(); };
            if (path == GamepadStick.LeftStickRight || path == GamepadStick.RightStickRight) return () => { isKey = GetPositiveAxisXKey(); };
            if (path == GamepadStick.LeftStickLeft  || path == GamepadStick.RightStickLeft)  return () => { isKey = GetNegativeAxisXKey(); };
            return () => { isKey = GetStickKey(); };
        }


        private UnityAction GetOnKeyDownAction()
        {
            if (path == GamepadStick.LeftStickUp    || path == GamepadStick.RightStickDown)  return () => { SetPositiveAxisYKeyDown(); if (isKeyDown) canKeyUpBeTriggered = true; };
            if (path == GamepadStick.LeftStickDown  || path == GamepadStick.RightStickDown)  return () => { SetNegativeAxisYKeyDown(); if (isKeyDown) canKeyUpBeTriggered = true; };
            if (path == GamepadStick.LeftStickRight || path == GamepadStick.RightStickRight) return () => { SetPositiveAxisXKeyDown(); if (isKeyDown) canKeyUpBeTriggered = true; };
            if (path == GamepadStick.LeftStickLeft  || path == GamepadStick.RightStickLeft)  return () => { SetNegativeAxisXKeyDown(); if (isKeyDown) canKeyUpBeTriggered = true; };     
            return () => { SetStickKeyDown(); if (isKeyDown) canKeyUpBeTriggered = true; };
        }

        private UnityAction GetOnKeyUpAction() 
        {
            return () =>
            {
                if      (GetKeyDown() || isKeyUp)           { isKeyUp = false;                             }
                else if (!GetKey() && canKeyUpBeTriggered)  { isKeyUp = true; canKeyUpBeTriggered = false; }
            };
        }

        private bool GetStickKey()
        {
           if(path == GamepadStick.LeftStick || path == GamepadStick.RightStick)
              return InputManager.GetGamepadVector2(path == GamepadStick.LeftStick ? true : false).magnitude > 0f; 
           return false;
        }

        private bool GetPositiveAxisYKey() 
        {
            if (path == GamepadStick.LeftStickUp || path == GamepadStick.RightStickUp)
                return InputManager.GetGamepadAxisY(path == GamepadStick.LeftStickUp ? true : false) > 0.5f;
            return false;
        }
        private bool GetNegativeAxisYKey() 
        {
            if (path == GamepadStick.LeftStickDown || path == GamepadStick.RightStickDown)
                return InputManager.GetGamepadAxisY(path == GamepadStick.LeftStickDown ? true : false) < -0.5f;
            return false;
        }      
        private bool GetPositiveAxisXKey()
        {
            if (path == GamepadStick.LeftStickRight || path == GamepadStick.RightStickRight)
              return InputManager.GetGamepadAxisY(path == GamepadStick.LeftStickRight ? true : false) > 0.5f;
            return false;
        }
        private bool GetNegativeAxisXKey()
        {
            if (path == GamepadStick.LeftStickLeft || path == GamepadStick.RightStickLeft)
                return InputManager.GetGamepadAxisY(path == GamepadStick.LeftStickLeft ? true : false) < -0.5f;
            return false;
        }

        private void SetStickKeyDown()
        {
            if (path == GamepadStick.LeftStick || path == GamepadStick.RightStick)
                if(GetKey() && canResetKeyDown) {
                    isKeyDown = true;
                    canResetKeyDown = false;
                }
                else{
                    isKeyDown = false;
                    if(InputManager.GetGamepadVector2(path == GamepadStick.LeftStick ? true : false).magnitude == 0) canResetKeyDown = true;
                }
        }

        private void SetPositiveAxisYKeyDown() 
        {
            if (path == GamepadStick.LeftStickUp || path == GamepadStick.RightStickUp)
            if (GetKey() && canResetKeyDown) {
                isKeyDown       = true;
                canResetKeyDown = false;
            }
            else 
                SetPositiveAxisKeyDown(InputManager.GetGamepadAxisY(path == GamepadStick.LeftStickUp ? true : false));
        }
        private void SetNegativeAxisYKeyDown()
        {
            if (path == GamepadStick.LeftStickDown || path == GamepadStick.RightStickDown)
            if (GetKey() && canResetKeyDown) {
                isKeyDown = true;
                canResetKeyDown = false;
            }
            else 
                SetNegativeAxisKeyDown(InputManager.GetGamepadAxisY(path == GamepadStick.LeftStickDown ? true : false));
        }
        private void SetPositiveAxisXKeyDown()
        {
            if (path == GamepadStick.LeftStickUp || path == GamepadStick.RightStickUp)
            if (GetKey() && canResetKeyDown)
            {
                isKeyDown = true;
                canResetKeyDown = false;
            }
            else 
                SetPositiveAxisKeyDown(InputManager.GetGamepadAxisX(path == GamepadStick.LeftStickRight ? true : false));
        }
        private void SetNegativeAxisXKeyDown()
        {
            if (path == GamepadStick.LeftStickDown || path == GamepadStick.RightStickDown)
            if (GetKey() && canResetKeyDown) {
                isKeyDown       = true;
                canResetKeyDown = false;
            }
            else
                SetNegativeAxisKeyDown(InputManager.GetGamepadAxisY(path == GamepadStick.LeftStickDown ? true : false));
        }

        private void SetPositiveAxisKeyDown(float axis) {
            isKeyDown = false;
            if (axis <= 0.5f) canResetKeyDown = true;
        }
        private void SetNegativeAxisKeyDown(float axis) {
            isKeyDown = false;
            if (axis >= -0.5f) canResetKeyDown = true;
        }


        public override bool GetKey() 
        {
            if (onKey != null) onKey.Invoke();
            return isKey;
        }

        public override bool GetKeyDown() 
        {
            if (onKeyDown != null) onKeyDown.Invoke();
            return isKeyDown;
        }

        public override bool GetKeyUp() 
        {
            if(onKeyUp != null) onKeyUp.Invoke();
            return isKeyUp;
        }

        
    }
}
