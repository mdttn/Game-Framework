using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public class FPSCameraControllerModule : CameraControllerModule
    {
        [SerializeField] private float minRotationX = -45f;
        [SerializeField] private float maxRotationX = 45f;

        public float MinRotationX => minRotationX;
        public float MaxRotationX => maxRotationX;

        private FPSCameraController controller;
        public  FPSCameraController Controller => controller;

        protected sealed override void Update() {
            if (controller != null) controller.Update();
        }

        protected sealed override void LateUpdate()
        {
            if (controller != null) controller.LateUpdate();
        }

        protected override void OnEnable()
        {
            SetCursorVisibility(false);
            if (controller != null) controller.Enable();
        }

        protected override void OnDisable()
        {
            SetCursorVisibility(true);
            if (controller != null) controller.Disable();
        }

        protected override void SetCameraController(CameraController cameraController)
        {
            this.controller = cameraController as FPSCameraController;
        }

        protected override CameraController GetCameraController()
        {
            if (controller != null) return controller;
            return new FPSCameraController(transform.root, transform, this);
        }

        private void SetCursorVisibility(bool isVisible)
        {
            Cursor.lockState = isVisible ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible   = isVisible;
        }
    }
}
