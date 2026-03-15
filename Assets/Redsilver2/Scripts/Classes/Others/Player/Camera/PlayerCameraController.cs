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
        public MouseVector2Input MouseInput => mouseInput;
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
            if (mouseInput != null) InputUpdate(mouseInput.Value);
        }

        protected override void OnLateUpdate()
        {
            if(body != null) body.localEulerAngles = Vector2.up    * rotationClampY;
            if(head != null) head.localEulerAngles = Vector2.right * rotationClampX;
        }

        protected virtual void InputUpdate(Vector2 input)
        {
            rotationClampY += Time.deltaTime * GetSensitvityX() * input.x;
            rotationClampX -= Time.deltaTime * GetSensitvityY() * input.y;
        }

        public override void Enable()
        {
            if(mouseInput != null) {
                mouseInput.Enable();
            }
        }

        public override void Disable()
        {
            if (mouseInput != null) {
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

        public static MouseVector2Input GetMouseInput() {
            return InputManager.GetOrCreateMouseVector2Input(MOUSE_INPUT_NAME, GamepadStick.RightStick);
        }
    }
}
