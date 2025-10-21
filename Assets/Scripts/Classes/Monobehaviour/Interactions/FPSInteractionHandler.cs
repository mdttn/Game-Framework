using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public class FPSInteractionHandler : InteractionHandler
    {
        private Camera camera;

        private FPSInteractionHandler() { }

        public FPSInteractionHandler(FPSInteractionHandlerModule module) : base(module)
        {
            if (module != null) camera = module.GetComponent<Camera>();
        }

        public FPSInteractionHandler(Camera camera, FPSInteractionHandlerModule module) : base(module)
        {
            this.camera = camera;
        }

        public FPSInteractionHandler(KeyboardKey keyboardKey, GamepadButton gamepadKey, FPSInteractionHandlerModule module) : base(keyboardKey, gamepadKey, module)
        {
            if (module != null) camera = module.GetComponent<Camera>();
        }

        public FPSInteractionHandler(KeyboardKey keyboardKey, GamepadButton gamepadKey, Camera camera, FPSInteractionHandlerModule module) : base(keyboardKey, gamepadKey, module)
        {
            this.camera = camera;
        }

        protected sealed override Collider GetCollider(float interactionRange)
        {
            Transform transform;
            if(camera == null) return null;

            transform = camera.transform;
            Debug.DrawRay(transform.position, transform.forward, Color.blue);

            Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 5f);
            return hitInfo.collider;
        }
    }
}
