using UnityEngine;


namespace RedSilver2.Framework.Player
{
    public abstract class CameraControllerModule : MonoBehaviour
    {
        private void Awake()
        {
            SetCameraController(GetCameraController());
        }

        protected abstract void Update();
        protected abstract void LateUpdate();

        protected abstract void OnEnable();
        protected abstract void OnDisable();

        protected abstract void SetCameraController(CameraController cameraController);
        protected abstract CameraController GetCameraController();
    }
}
