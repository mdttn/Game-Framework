using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.Controllers
{
    public abstract class MovementStateMachineController : StateMachineController {
        [SerializeField] private bool use2DMovement;
        public bool Use2DMovement => use2DMovement;

        protected sealed override void InitializeStateMachine(ref StateMachine stateMachine)
        {
            stateMachine = GetStateMachine(GetMovementHandler());
            stateMachine?.Enable();
        }

        protected abstract MovementStateMachine GetStateMachine(MovementHandler movementHandler);
        protected abstract MovementHandler GetMovementHandler();
    }
}
