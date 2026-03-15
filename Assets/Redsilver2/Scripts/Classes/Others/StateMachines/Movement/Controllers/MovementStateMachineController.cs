using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.Controllers
{
    public abstract class MovementStateMachineController : StateMachineController {
        [SerializeField] private bool use2DMovement;

        private MovementHandler movementHandler;

        public bool Use2DMovement => use2DMovement;
        public MovementHandler MovementHandler => movementHandler;
        protected sealed override void InitializeStateMachine(ref StateMachine stateMachine)
        {
            MovementStateMachine movementStateMachine = GetStateMachine(GetMovementHandler());

            stateMachine    = movementStateMachine;
            movementHandler = movementStateMachine != null ? movementStateMachine.MovementHandler : null;

            stateMachine?.Enable();
        }

        protected abstract MovementStateMachine GetStateMachine(MovementHandler movementHandler);
        protected abstract MovementHandler GetMovementHandler();
    }
}
