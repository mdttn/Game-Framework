using RedSilver2.Framework.StateMachines.Handlers;
using RedSilver2.Framework.StateMachines.States.Conditions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    [RequireComponent(typeof(MovementStateMachineEventHandler))]
    public class MovementStateMachineController : UpdateableStateMachineController
    {
        [SerializeField] private float groundCheckRange = 0f;
        [SerializeField] private bool  is2DMovement;

        public float GroundCheckRange => groundCheckRange;
        public bool Is2DMovement => is2DMovement;

        public override void SetStateMachine(StateMachine stateMachine)
        {
            if(stateMachine is MovementStateMachine)
                 base.SetStateMachine(stateMachine);
        }


        public bool IsMoving()
        {
            return MovementMoveCondition.IsMoving(GetStateMachine() as MovementStateMachine);
        }

        public bool IsRunning()
        {
            return MovementRunCondition.IsRunning(GetStateMachine() as MovementStateMachine);
        }

        public bool IsGrounded()
        {
            return MovementGroundCondition.IsGrounded(GetStateMachine() as MovementStateMachine);
        }
    }
}
