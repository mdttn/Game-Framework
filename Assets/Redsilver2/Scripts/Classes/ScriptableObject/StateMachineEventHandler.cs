using RedSilver2.Framework.StateMachines.States.Configurations;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Handlers
{
    public abstract class StateMachineEventHandler : MonoBehaviour {

        private   StateMachineController controller;
        protected StateMachine           stateMachine;

        protected virtual void Awake() {
       
            controller = GetComponent<StateMachineController>();

            controller?.AddOnStateMachineChanged(SetStateMachine);
            SetStateMachine(controller.StateMachine);
        }

        private void OnEnable()  { controller?.AddOnStateMachineChanged(SetStateMachine); }
        private void OnDisable() { controller?.RemoveOnStateMachineChanged(SetStateMachine); }

        public void AddOnAddedListener(UnityAction<StateConfiguration> action) {
            stateMachine?.AddOnStateAddedListener(action);
        }

        public void RemoveOnAddedListener(UnityAction<StateConfiguration> action){
            stateMachine?.RemoveOnStateAddedListener(action);
        }

        public void AddOnRemovedListener(UnityAction<StateConfiguration> action) {
            stateMachine.AddOnStateRemovedListener(action);
        }

        public void RemoveOnRemovedListener(UnityAction<StateConfiguration> action) {
            stateMachine.RemoveOnStateRemovedListener(action);
        }

        protected virtual void SetStateMachine(StateMachine stateMachine) {
            this.stateMachine = stateMachine;   
        }
    }
}
