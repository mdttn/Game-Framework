using RedSilver2.Framework.StateMachines.Controllers;
using System.Linq;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementStateInitializer : StateInitializer
    {
        private MovementStateType[] inclusiveStates;

        protected override void Start() {
            inclusiveStates = GetInclusiveStates();
            if (inclusiveStates != null) inclusiveStates = inclusiveStates.Distinct().ToArray();

            base.Start();
        }

        protected sealed override State GetDefaultState(StateMachine stateMachine)
        {
            if (!CanAddOrRemoveState(stateMachine)) return null;
            return GetDefaultState(stateMachine as MovementStateMachine);
        }

        protected sealed override void OnStateAdded(State state) {
            if (IsInclusiveState(state as MovementState)) base.OnStateAdded(state);
        }

        protected sealed override void OnStateRemoved(State state) {
            if (IsInclusiveState(state as MovementState)) base.OnStateRemoved(state);
        }

        protected sealed override UnityAction<State> GetOnStateAddedAction() {
            return state => { OnStateAdded(state as MovementState); };
        }

        protected sealed override UnityAction<State> GetOnStateRemovedAction() {
            return state => { OnStateRemoved(state as MovementState); };
        }

        protected sealed override bool CanAddOrRemoveState(StateMachine controller) {
            return controller is MovementStateMachine;
        }


        protected sealed override void SetStateMachine(ref StateMachine stateMachine) {
            if(transform.root.gameObject.TryGetComponent(out StateMachineController controller)) {
                if (controller is MovementStateMachineController)
                    stateMachine = controller.StateMachine;
            }
        }

        public bool IsInclusiveState(MovementState state)
        {
            if (state == null || inclusiveStates == null) return false;
            return inclusiveStates.Where(x => x == state.Type).Count() > 0;
        }

        protected abstract void OnStateAdded(MovementState state);
        protected abstract void OnStateRemoved(MovementState state);

        protected abstract MovementState GetDefaultState(MovementStateMachine stateMachine);
        protected abstract MovementStateType[] GetInclusiveStates();
    }
}