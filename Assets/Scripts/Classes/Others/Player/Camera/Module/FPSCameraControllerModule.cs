using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public class FPSCameraControllerModule : CameraControllerModule
    {
        [SerializeField] private float minRotationX = -45f;
        [SerializeField] private float maxRotationX = 45f;

        public float MinRotationX => minRotationX;
        public float MaxRotationX => maxRotationX;

        private FPSCameraController cameraController;
        public FPSCameraController CameraController => cameraController;

        protected sealed override void Update()
        {
            if (cameraController != null) cameraController.Update();
        }

        protected sealed override void LateUpdate()
        {
            if (cameraController != null) cameraController.LateUpdate();
        }

        protected override void OnEnable()
        {
            SetCursorVisibility(false);
            if (cameraController != null) cameraController.Enable();
        }

        protected override void OnDisable()
        {
            SetCursorVisibility(true);
            if (cameraController != null) cameraController.Disable();
        }

        protected override void SetCameraController(CameraController cameraController)
        {
            this.cameraController = cameraController as FPSCameraController;
        }

        protected override CameraController GetCameraController()
        {
            if (cameraController != null) return cameraController;
            return new FPSCameraController(transform.root, transform, this);
        }

        private void SetCursorVisibility(bool isVisible)
        {
            Cursor.lockState = isVisible ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible   = isVisible;
        }
    }
}
