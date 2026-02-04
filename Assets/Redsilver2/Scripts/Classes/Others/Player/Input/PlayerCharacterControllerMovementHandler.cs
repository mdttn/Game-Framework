using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States.Movement
{
    [System.Serializable]
    public class PlayerCharacterControllerMovementHandler : PlayerMovementHandler
    {
        public PlayerCharacterController Controller;

        public PlayerCharacterControllerMovementHandler(PlayerCharacterController controller, bool use2DMovementInputs) : base(controller, use2DMovementInputs)
        {
            this.Controller = controller;
        }

        public PlayerCharacterControllerMovementHandler(PlayerCharacterController controller) : base(controller) {
            this.Controller = controller;
        }

 
        protected sealed override void Move(Vector3 position) {
            if(Controller != null) {
                Controller.Character?.Move(position);
            }
        }

        protected sealed override void Crouch(float height) {
            if (Controller != null) {
                CharacterController character = Controller.Character;
                if(character != null) character.height = Mathf.Lerp(character.height, height, Time.deltaTime * DEFAULT_CROUCH_SPEED);
            }
        }

        public sealed override Transform GetTransform() {
            if(Controller == null) return null;
            return Controller.transform;
        }
    }
}
