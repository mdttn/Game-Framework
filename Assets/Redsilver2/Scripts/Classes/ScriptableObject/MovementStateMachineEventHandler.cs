using RedSilver2.Framework.StateMachines.States.Conditions;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Handlers
{
    public class MovementStateMachineEventHandler : UpdateableStateEventHandler
    {
        public void AddOnMoveListener(UnityAction<Vector2> action) {
            (stateMachine as MovementStateMachine)?.AddOnMovedListener(action);
        }

        public void RemoveOnMoveListener(UnityAction<Vector2> action) {
            (stateMachine as MovementStateMachine)?.RemoveOnMovedListener(action);
        }

        public void AddOnGroundTagChangedListener(UnityAction<string> action)
        {
            (stateMachine as MovementStateMachine)?.AddOnGroundTagChangedListener(action);
        }
        public void RemoveOnGroundTagChangedListener(UnityAction<string> action) {
            (stateMachine as MovementStateMachine)?.RemoveOnGroundTagChangedListener(action);
        }

        protected override void SetStateMachine(StateMachine stateMachine) {
            if (stateMachine is MovementStateMachine)
                base.SetStateMachine(stateMachine as MovementStateMachine);
        }
    }
}
