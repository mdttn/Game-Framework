using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public class FPSCameraControllerModule : CameraControllerModule
    {
        [SerializeField] private float minRotationX = -45f;
        [SerializeField] private float maxRotationX = 45f;

        public float MinRotationX => minRotationX;
        public float MaxRotationX => maxRotationX;

        protected override CameraController GetCameraController()
        {
            if (Controller != null) return Controller;
            return new FPSCameraController(transform.root, transform, this);
        }
    }
}
