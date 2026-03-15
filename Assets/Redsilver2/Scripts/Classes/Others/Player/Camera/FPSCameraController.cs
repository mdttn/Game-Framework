using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public class FPSCameraController : PlayerCameraController
    {
        private readonly FPSCameraControllerModule module;

        private FPSCameraController()
        {
        }

        public FPSCameraController(Transform body, Transform head, FPSCameraControllerModule module) : base(body, head)
        {
            this.module = module;
        }

        protected override void InputUpdate(Vector2 input)
        {
            base.InputUpdate(input);
            if(module != null) rotationClampX = Mathf.Clamp(rotationClampX, module.MinRotationX, module.MaxRotationX);
        }
    }
}
