using RedSilver2.Framework.StateMachines.States;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public abstract class MovementStateModule : StateModule
    {
        protected sealed override void SetStateMachine(ref StateMachine stateMachine)
        {
            if(transform.root.TryGetComponent(out StateMachineController controller)) {
                if (controller is MovementStateMachineController) stateMachine = (controller as MovementStateMachineController).StateMachine;
            }
        }

        protected sealed override UnityAction<State> GetOnStateAddedAction()
        {
            return state =>
            {
                OnStateAdded(state as MovementState);
            };
        }
        protected sealed override UnityAction<State> GetOnStateRemovedAction()
        {
            return state =>
            {
                OnStateRemoved(state as MovementState);
            };
        }

        protected abstract void OnStateAdded(MovementState state);
        protected abstract void OnStateRemoved(MovementState state);
    }
}
