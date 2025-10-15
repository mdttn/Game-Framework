using RedSilver2.Framework.Inputs;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player
{
    public abstract class PlayerCameraController : CameraController
    {
        protected float rotationClampX;
        protected float rotationClampY;

        protected readonly MouseVector2Input   mouseInput;
        private   readonly UnityEvent<Vector2> onInputUpdate;

        public Vector2 Rotation => Vector2.right * rotationClampX + Vector2.up * rotationClampY;
        public const string MOUSE_INPUT_NAME = "Player Camera Mouse Input";


        protected PlayerCameraController() { }

        public PlayerCameraController(Transform body, Transform head) : base(body, head)
        {
            mouseInput    = GetMouseInput();
            onInputUpdate = new UnityEvent<Vector2>();
        }

        public void AddOnInputUpdate(UnityAction<Vector2> action)
        {
            if (onInputUpdate != null && action != null) onInputUpdate.AddListener(action);
        }

        public void RemoveOnInputUpdate(UnityAction<Vector2> action)
        {
            if (onInputUpdate != null && action != null) onInputUpdate.RemoveListener(action);
        }

        protected sealed override void OnUpdate()
        {
            if(mouseInput != null) mouseInput.Update();
        }

        protected override void OnLateUpdate()
        {
            if(body != null) body.localEulerAngles = Vector2.up    * rotationClampY;
            if(head != null) head.localEulerAngles = Vector2.right * rotationClampX;
        }

        protected virtual void OnInputUpdate(Vector2 input)
        {
            rotationClampY += Time.deltaTime * GetSensitvityX() * input.x;
            rotationClampX -= Time.deltaTime * GetSensitvityY() * input.y;
        }

        public void ResetRotation()
        {
            if (body != null) rotationClampX = body.localEulerAngles.x;
            if (head != null) rotationClampY = head.localEulerAngles.y;
        }

        public void Enable()
        {
            ResetRotation();

            if(mouseInput != null)
            {
                mouseInput.AddOnUpdateListener(OnInputUpdate);
                mouseInput.Enable();
            }
        }

        public void Disable()
        {
            if (mouseInput != null)
            {
                mouseInput.RemoveOnUpdateListener(OnInputUpdate);
                mouseInput.Disable();
            }
        }
       
        private float GetSensitvityX()
        {
            return 5f;
        }

        private float GetSensitvityY()
        {
            return 5f;
        }

        public static MouseVector2Input GetMouseInput()
        {
            MouseVector2Input result = InputManager.GetInputHandler(MOUSE_INPUT_NAME) as MouseVector2Input;
            if(result == null) return new MouseVector2Input(MOUSE_INPUT_NAME, GamepadStick.RightStick);
            return result;
        }
    }
}
